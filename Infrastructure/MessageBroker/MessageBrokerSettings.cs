namespace Infrastructure.MessageBroker;

public  class MessageBrokerSettings
{
    public ServiceBusType Type { get; set; }
    public RabbitMqSettings RabbitMq { get; set; }
    public AzureServiceBusSettings AzureServiceBus { get; set; } 
  
}

public  class RabbitMqSettings
{
    public string Host { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public  class AzureServiceBusSettings
{
    public string ConnectionString { get; set; } = string.Empty;   
}