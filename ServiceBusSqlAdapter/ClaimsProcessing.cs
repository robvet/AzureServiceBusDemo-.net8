using System;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ServiceBusSqlAdapter
{
    public class ClaimsProcessing
    {
        private readonly ILogger<ClaimsProcessing> _logger;
        private readonly IConfiguration _configuration;
        //private readonly ServiceBusClient _client;

        public ClaimsProcessing(ILogger<ClaimsProcessing> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [Function(nameof(ClaimsProcessing))]
        public void Run([ServiceBusTrigger("test-topic-1", "claims-subscription", Connection = "ServiceBusConnection")] ServiceBusReceivedMessage message)
        {
            var mySecret = _configuration["MySecret"];

            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);
        }
    }
}
