# Desafio Backend

Este projeto é uma implementação de uma API para gerenciar o aluguel de motos e entregadores.

## Arquitetura

A documentação detalhada da arquitetura do projeto pode ser encontrada em [docs/arquitetura.md](./docs/arquitetura.md).

## Stack de Tecnologia

*   **.NET 9**: Plataforma de desenvolvimento.
*   **Entity Framework Core**: ORM para acesso ao banco de dados relacional.
*   **PostgreSQL**: Banco de dados relacional para armazenar os dados principais.
*   **MongoDB**: Banco de dados NoSQL para armazenar dados de eventos e notificações.
*   **RabbitMQ**: Sistema de mensageria para comunicação assíncrona entre serviços.
*   **Docker & Docker Compose**: Para containerização e orquestração do ambiente de desenvolvimento.

## Como Executar o Projeto

Siga os passos abaixo para configurar e executar o ambiente de desenvolvimento local.

### Pré-requisitos

*   [Docker](https://www.docker.com/get-started)
*   [Docker Compose](https://docs.docker.com/compose/install/)

### Passos

1.  **Clone o repositório:**
    ```bash
    git clone https://github.com/antonioernesto1/desafio-backend.git
    cd desafio-backend
    ```

2.  **Configure as variáveis de ambiente:**
    *   Crie uma cópia do arquivo `.env.example` e renomeie para `.env`.
    *   Abra o arquivo `.env` e substitua os valores das variáveis pelas suas credenciais.

3.  **Inicie os contêineres:**
    *   Execute o comando abaixo na raiz do projeto:
    ```bash
    docker-compose up --build
    ```
    *   Isso irá construir as imagens, iniciar todos os serviços e aplicar as migrações do banco de dados automaticamente.

4.  **Acesse a API:**
    *   A API estará disponível em `http://localhost:8080`.
    *   Você pode encontrar a especificação da API e interagir com ela através do Swagger em `http://localhost:8080/swagger`.

## Configuração do .env

O arquivo `.env` é usado para configurar as credenciais e outras variáveis do ambiente.

*   `POSTGRES_DB`: O nome do banco de dados PostgreSQL.
*   `POSTGRES_USER`: O usuário para o banco de dados PostgreSQL.
*   `POSTGRES_PASSWORD`: A senha para o banco de dados PostgreSQL.
*   `MONGO_USER`: O usuário para o banco de dados MongoDB.
*   `MONGO_PASSWORD`: A senha para o banco de dados MongoDB.
*   `RABBITMQ_USER`: O usuário para o RabbitMQ.
*   `RABBITMQ_PASSWORD`: A senha para o RabbitMQ.