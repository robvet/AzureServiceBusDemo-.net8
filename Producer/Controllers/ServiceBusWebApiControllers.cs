using Microsoft.AspNetCore.Mvc;
using Azure.Messaging.ServiceBus;
using System.Text.Json;
using AzureServiceBus.Model;
using AzureServiceBusConsumer.Model;

namespace AzureServiceBus.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly ServiceBusConfig _config;
        private readonly ILogger<MessageController> _logger;

        public MessageController(ServiceBusConfig config, ILogger<MessageController> logger)
        {
            _config = config;
            _logger = logger;
        }
        
        [ProducesResponseType(typeof(TransactionMessage), 200)]
        [ProducesResponseType(400)]
        [HttpPost("PostMessageToTopic")]
        public async Task<IActionResult> PostMessageToTopic([FromBody] TransactionMessage transactionMessage)
        {
            await using var client = new ServiceBusClient(_config.ConnectionString);
            ServiceBusSender sender = client.CreateSender(_config.TopicName);

            try
            {
                string messageBody = JsonSerializer.Serialize(transactionMessage);
                ServiceBusMessage message = new ServiceBusMessage(messageBody);
                //{
                //    // For topic filtering, return a random subject type
                //    Subject = GetRandomSubjectType(),
                //};

                await sender.SendMessageAsync(message);

                _logger.LogInformation($"Message {transactionMessage.ReferenceIdentifier} sent to Azure Service Bus");

                return Ok("Message sent to Azure Service Bus Topic");
            }
            catch (Exception ex)
            {
                var error = ex.Message + ex.InnerException;
                _logger.LogError(error);

                return StatusCode(500, $"Internal server error: {ex}");
            }
            finally
            {
                await sender.DisposeAsync();
            }
        }

        private static string GetRandomSubjectType()
        {
            var random = new Random();
            return random.Next(2) == 0 ? "claim" : "billing";
        }
    }
}
