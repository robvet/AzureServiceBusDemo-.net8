# Simple Demo for Azure Service Bus

## Producer Project

Publishes messages to a Topic

## Secrets are best placed the VS or VSC User Secrets feature

```json
{
  "AzureServiceBus:ConnectionString": "<Connection string that supports both sending and receiving",
  "AzureServiceBus:QueueName": "<queue name>",
  "AzureServiceBus:TopicName": "<topic name>",
  "AzureServiceBus:FirstSubscriptionName": "<name of first subscrioption>",
  "AzureServiceBus:SecondSubscriptionName": "<name of second subscrioption>",
  "AzureServiceBus:ThirdSubscriptionName": "<name of ..nth subscrioption>",
  "ApplicationInsights:InstrumentationKey": "<Instrumentation Key for App Insights>"
}
```  
  
## Azure Function

Consumes Messages from each Subscription

## Secrets are best placed the VS or VSC User Secrets feature

```json
{
"ServiceBusConnection": "<Connection string that supports both sending and receiving",
"TopicName": "<topic name>",
"ClaimSubscriptionName": "<subscrioption name>",
"BillingSubscriptionName": "<subscrioption name>",
"LoggingSubscriptionName": "<subscrioption name>",
"ApplicationInsights:InstrumentationKey": "<Instrumentation Key for App Insights>",
"ConnectionStrings": {
      "ServiceBusDemoDBConnection":"<Connection string that supports both sending and receiving"
   }
}
```