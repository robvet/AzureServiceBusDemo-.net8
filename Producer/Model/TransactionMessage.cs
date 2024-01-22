namespace AzureServiceBusConsumer.Model
{
    public class TransactionMessage
    {
        public string ReferenceIdentifier { get; set; } = null!;

        public string InsuredName { get; set; } = null!;
    }
}
