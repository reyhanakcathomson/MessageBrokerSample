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
Sending Messages: Use "/upload-ledger-data" endpoint in API to send/publish messages.

Receiving Messages: The ConsumerWorker automatically listens for messages on the configured queue and processes them as they arrive.

Request/Response Pattern: Use "/cancel-ledger-data" endpoint in API to use a request client to publish requests and wait for a response. The request client is asynchronous, and supports use of the await keyword since it returns a Task.

Conclusion
This POC provides a basic setup for integrating RabbitMQ and Azure Service Bua with a .NET application using MassTransit. It demonstrates the roles of a publisher and a consumer in a microservices architecture, facilitating learning and experimentation with message-driven systems.

## Azure Server Bus - Broker Topology
### Send
![send (1)](https://github.com/user-attachments/assets/1c50b7a8-ba41-4727-84d7-ceef8e5f6f24)
<br />
<br />
### Publish
![publish](https://github.com/user-attachments/assets/48680889-8160-4e53-b9a5-c452844575cc)
<br />
<br />
### Request/Response Pattern
![Azure-Request drawio](https://github.com/user-attachments/assets/0eb36638-99c5-4ddd-b80e-ad642e3d71a8)
<br />
<br />

## RabbitMQ - Broker Topology
### Send
![RabbitMq-Send drawio](https://github.com/user-attachments/assets/688956ea-850a-4c32-9a37-c90ab73762ba)
<br />
<br />
### Publish
![RabbitMq-Publish drawio](https://github.com/user-attachments/assets/2521f16f-2a32-49b2-a42e-65a535f11b0d)
<br />
<br />
### Request/Response Pattern
![RabbitMq-Request drawio](https://github.com/user-attachments/assets/94b9dbc3-524d-40c5-a669-c43982c07db4)
<br />
<br />
