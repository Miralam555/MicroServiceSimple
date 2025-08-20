# MicroServiceSagaPatternChoreography
# Microservices E-Commerce Project

A comprehensive microservices-based e-commerce solution built with .NET 8.0, featuring distributed architecture with multiple databases and message queuing.

## 🏗️ Architecture Overview

This project implements a microservices architecture pattern with the following services:

- **Order.API** - Handles order management and processing
- **Payment.API** - Manages payment transactions and validation
- **Stock.API** - Inventory and stock management
- **Shared Library** - Common utilities, models, and shared functionality

## 🛠️ Technology Stack

- **Framework**: .NET 8.0
- **Databases**: 
  - MongoDB (NoSQL) - Document storage
  - MS SQL Server - Relational data
- **Message Broker**: RabbitMQ
- **Architecture**: Microservices with API Gateway pattern

## 📋 Prerequisites

Before running this application, ensure you have the following installed:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MongoDB](https://www.mongodb.com/try/download/community)
- [MS SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [RabbitMQ](https://www.rabbitmq.com/download.html)



## 🔄 Message Queue Integration

The services communicate asynchronously using RabbitMQ:

- **Order Events**: Order created, updated, cancelled
- **Payment Events**: Payment processed, failed, refunded  
- **Stock Events**: Stock reserved, released, updated

## 🗄️ Database Schema

### MongoDB Collections (NoSQL)
- **Orders**: Order documents with embedded items
- **OrderHistory**: Order status change logs
