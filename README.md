# MessageBrokerSample - Azure Service Bus Integration POC with MassTransit
# POC descriptions:

We are exploring the use of Azure Service Bus paired with RabbitMQ for local development to address the identified challenges. Utilizing a shared instance of Azure Service Bus hampers effective end-to-end testing in our development environment, as a published message might be unintentionally consumed by another consumer. This scenario leads to unreliable test results.

To overcome this, we propose enabling developers to use RabbitMq on their local machines. This setup will allow each developer to conduct isolated tests, ensuring more controlled and reliable outcomes.

**Objective:**
The objective of this spike is to investigate and implement POC where:
- Developers use RabbitMq(in Docker) in the development environment for isolated testing.
- The actual Azure Service Bus is used in the DevOps environment, ensuring integrity in more formal stages of deployment.

**Tasks include:**
1. Research to find a suitable RabbitMQ that can mirror the behaviors expected from the actual Azure Service Bus.
2. Create a foundational setup of RabbitMQ in a local environment suitable for developer use.
3. Design the integration such that switching between the RabbitMQ during development and the actual service in staging/production is seamless.
4. Conduct tests to validate that this dual setup meets project requirements without disrupting the existing workflow.

## Project Structure

- **MessageBrokerAPI**: An ASP.NET Core Web API designed to produce messages. It acts as the message publisher.
- **ConsumerWorker**: A .NET background service that listens to the message queue and consumes messages. It acts as the message consumer.
- **Infrastructure**: A shared project used by both the MessageBrokerAPI and ConsumerWorker. It contains common classes, interfaces, and message contracts.

## Prerequisites

- .NET 8.0 SDK
- Docker Desktop
- Azure Servide Bus in Devops


## Setup

### RabbitMQ in Docker

1. **Docker Setup for RabbitMQ**:
   Ensure Docker is running on your machine. We will use Docker to run RabbitMQ. `docker-compose.yml` file at the root of the project sets up RabbitMQ with the management plugin and default user credentials. It also creates a custom network for our services to communicate.

```json
"MessageBroker": {
   
    "RabbitMq": {
      "Host": "amqp://localhost",
      "Username": "guest",
      "Password": "guest"
    }
   
  }
```

3. **Build and Run Docker Containers**: Navigate to the directory containing docker-compose.yml and run the following command to start RabbitMQ:
   ```bash
   docker-compose up
  RabbitMQ management console can be accessed at http://localhost:15672 using the default credentials (Username: guest Password: guest)


### Azure Service Bus in DevOps

1. **Create a Service Bus Namespacee in DevOps**:
   As POC will create a topic, ensure pricing tier should be "Standart" at least. The connection string of "Shared access policy" should have "Managed" permission

2. **Azure Service Bus Connection String**: Ensure the connection string of Azure Service Bus Namespace should be in both appsettings in Consumer and API

```json
"MessageBroker": {

    "AzureServiceBus": {
      "ConnectionString": "YOUR-AZURE-SERVICE-BUS-CONNECTIONSTRING"
    }
  }
```

## Running the Solutions  

To specify which message broker to use: 

```json
"MessageBroker": {

   "Type": "RabbitMQ",  // or "AzureBus"

  }
```

### MessageBrokerAPI:

1.  Navigate to the MessageBrokerAPI directory.

   2.  Run the following commands to build and run the Web API:
   
         ```bash
         dotnet build
         dotnet run
         ```

### ConsumerWorker
 
1.  Navigate to the ConsumerWorker directory.

2.  Execute the following commands to start the background service:
   
    ```bash
      dotnet build
      dotnet run
      ```

## Usage
Sending Messages: Use the MessageBrokerAPI to send messages.

Receiving Messages: The ConsumerWorker automatically listens for messages on the configured queue and processes them as they arrive.

Conclusion
This POC provides a basic setup for integrating RabbitMQ and Azure Service Bua with a .NET application using MassTransit. It demonstrates the roles of a publisher and a consumer in a microservices architecture, facilitating learning and experimentation with message-driven systems.
