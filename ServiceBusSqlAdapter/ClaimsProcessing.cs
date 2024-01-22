using System;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Azure.Amqp.Framing;
using ServiceBusSqlAdapter.Models;
using System.Text.Json;

namespace ServiceBusSqlAdapter
{
    public class ClaimsProcessing
    {
        private readonly ILogger<ClaimsProcessing> _logger;
        private readonly IConfiguration _configuration;
        private readonly ServiceBusDemoSqldbContext _dbContext;

        public ClaimsProcessing(ILogger<ClaimsProcessing> logger, 
                                IConfiguration configuration,
                                ServiceBusDemoSqldbContext dbContext)
        {
            _logger = logger;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [Function(nameof(ClaimsProcessing))]
        public void Run([ServiceBusTrigger("test-topic-1", "claims-subscription", Connection = "ServiceBusConnection")] ServiceBusReceivedMessage message)
        {
            //var mySecret = _configuration["MySecret"];

            // Retrieve the message data
            //var messageId = message.MessageId;
            var body = message.Body.ToString();
            var messageMetaDataType = message.Subject;
            //var contentType = message.ContentType;
           
            try
            {
                // Deserialize the message body
                var transactionMessage = JsonSerializer.Deserialize<TransactionMessage>(body, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Insert the message data into the SQL database using Entity Framework
                var entity = new Claim
                {
                    ClaimNumber = transactionMessage.ReferenceIdentifier ?? throw new NullReferenceException("Transaction Type required"),
                    Insured = transactionMessage.InsuredName ?? throw new NullReferenceException("Insured required"),
                };
                _dbContext.Claims.Add(entity);
                _dbContext.SaveChanges();

                _logger.LogInformation($"Message {transactionMessage.ReferenceIdentifier} sent to Azure Service Bus");
            }
            catch (Exception ex)
            {
                var error = ex.Message + ex.InnerException;
                _logger.LogError(error);
                
                // Message is automatically abandoned to Azure Service Bus and then retried.
                // No need to call AbandonAsync()

                throw;
            }

            //_logger.LogInformation("Message ID: {id}", message.MessageId);
            //_logger.LogInformation("Message Body: {body}", message.Body);
            //_logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

            // Message is automatically settled to Azure Service Bus.
            // No need to call CompleteAsync()
        }
    }
}
