namespace AzureServiceBusConsumer.Model
{
    public class TransactionMessage
    {
        public string Type { get; set; } = null!;

        public string Insured { get; set; } = null!;
    }
}
