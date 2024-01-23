using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceBusSqlAdapter.Models;
using System.Text.Json;

namespace ServiceBusSqlAdapter
{
    public class BillingProcessing
    {
        private readonly ILogger<BillingProcessing> _logger;
        private readonly IConfiguration _configuration;
        private readonly ServiceBusDemoSqldbContext _dbContext;

        public BillingProcessing(ILogger<BillingProcessing> logger, 
                                IConfiguration configuration,
                                ServiceBusDemoSqldbContext dbContext)
        {
            _logger = logger;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [Function(nameof(BillingProcessing))]
        public void Run([ServiceBusTrigger("%TopicName%", "%BillingSubscriptionName%", Connection = "ServiceBusConnection")] ServiceBusReceivedMessage message)
        {
            //var mySecret = _configuration["MySecret"];

            // Retrieve the message data
            //var messageId = message.MessageId;
            var body = message.Body.ToString();
            var messageMetaDataType = message.Subject;

            try
            {
                // Deserialize the message body
                var transactionMessage = JsonSerializer.Deserialize<TransactionMessage>(body, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Insert the message data into the SQL database using Entity Framework
                var entity = new Billing
                {
                    BillingNumber = transactionMessage.ReferenceIdentifier ?? throw new NullReferenceException("Transaction Type required"),
                    Insured = transactionMessage.InsuredName ?? throw new NullReferenceException("Insured required"),
                };
                _dbContext.Billings.Add(entity);
                _dbContext.SaveChanges();

                _logger.LogInformation($"Billing Message {transactionMessage.ReferenceIdentifier} sent to Azure Service Bus");
            }
            catch (Exception ex)
            {
                var error = ex.Message + ex.InnerException;
                _logger.LogError(error);

                // Message is automatically abandoned to Azure Service Bus and then retried.
                // No need to call AbandonAsync()

                throw;
            }

            // Message is automatically settled to Azure Service Bus.
            // No need to call CompleteAsync()
        }
    }
}
