# MessageBrokerSample - RabbitMQ Integration POC with .NET and MassTransit

This Proof of Concept (POC) demonstrates how to integrate RabbitMQ into a .NET project using MassTransit for message brokering. The project is structured into three separate solutions to clearly define roles and responsibilities.

## Project Structure

- **MessageBrokerAPI**: An ASP.NET Core Web API designed to produce messages. It acts as the message publisher.
- **ConsumerWorker**: A .NET background service that listens to the message queue and consumes messages. It acts as the message consumer.
- **Infrastructure**: A shared project used by both the MessageBrokerAPI and ConsumerWorker. It contains common classes, interfaces, and message contracts.

## Prerequisites

- .NET 8.0 SDK
- Docker Desktop

## Setup

### RabbitMQ and MassTransit with Docker

1. **Docker Setup for RabbitMQ**:
   Ensure Docker is running on your machine. We will use Docker to run RabbitMQ. `docker-compose.yml` file at the root of the project sets up RabbitMQ with the management plugin and default user credentials. It also creates a custom network for our services to communicate.

2. **Build and Run Docker Containers**: Navigate to the directory containing docker-compose.yml and run the following command to start RabbitMQ:
   ```bash
   docker-compose up
  RabbitMQ management console can be accessed at http://localhost:15672 using the default credentials (guest/guest).

## Running the Solutions  

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
Sending Messages: Use the MessageBrokerAPI to send messages. This can be done via an HTTP POST request to the endpoint /produce-email or /produce-sms with a JSON payload.

Receiving Messages: The ConsumerWorker automatically listens for messages on the configured queue and processes them as they arrive.

Conclusion
This POC provides a basic setup for integrating RabbitMQ with a .NET application using MassTransit. It demonstrates the roles of a publisher and a consumer in a microservices architecture, facilitating learning and experimentation with message-driven systems.
