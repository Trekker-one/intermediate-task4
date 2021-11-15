using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Consummer_api.DBContext;
using Consummer_api.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Consummer_api.Controllers
{
    [Route("api/consumeBpi")]
    [ApiController]
    public class ConsumeController : ControllerBase
    {
        private CustomMessageDbContext _context;
        private readonly ILogger<ConsumeController> _logger;
        public ConsumeController(
            CustomMessageDbContext context,
            ILogger<ConsumeController> logger
            )
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<string>>> Get()
        {
            List<string> queueItems = new List<string>();
            var factory = new ConnectionFactory()
            {
                HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST"),
                Port = Convert.ToInt32(Environment.GetEnvironmentVariable("RABBITMQ_PORT"))
                //HostName = "localhost", 
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                CustomMessage record = new CustomMessage();
                channel.QueueDeclare(queue: "Bpi",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    queueItems.Add(message);
                    // Store into In-Memeory DB
                    
                    record = JsonConvert.DeserializeObject<CustomMessage>(message);
                    Console.WriteLine(" [x] Received {0}", message);
                };
                if (record.bpi != null)
                {
                    _context.Add(record);
                    await _context.SaveChangesAsync();
                }
                
                channel.BasicConsume(queue: "Bpi",
                                     autoAck: true,
                                     consumer: consumer);
            }

            if (queueItems.Count == 0)
            {
                return NoContent();
            }

            return queueItems;
        }

    }
}
