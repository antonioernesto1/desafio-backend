# Arquitetura do Projeto

Esta documentação detalha as decisões arquiteturais, padrões e a estrutura geral do projeto.

## Princípios Fundamentais

A arquitetura é fundamentada em três pilares principais: **Clean Architecture**, **Domain-Driven Design (DDD)** e **CQRS (Command Query Responsibility Segregation)**. A combinação desses padrões visa criar um sistema robusto, escalável, testável e de fácil manutenção.

### Domain-Driven Design (DDD)

Dado que o projeto possui regras de negócio bem definidas e um domínio complexo (gestão de aluguéis, entregadores, motos), o DDD foi adotado para modelar o software em torno do domínio de negócio. Conceitos como **Agregados** (`Motorcycle`, `DeliveryDriver`, `Rental`), **Entidades** e **Objetos de Valor** (`Cnh`) são utilizados para garantir que a lógica de negócio principal seja explícita e encapsulada no coração do software (a camada de Domínio).

### CQRS (Command Query Responsibility Segregation)

O padrão CQRS é utilizado para separar as operações de escrita (Commands) das operações de leitura (Queries). Essa separação traz diversos benefícios:

*   **Simplicidade**: Modelos de escrita podem ser focados em validação e consistência, enquanto modelos de leitura podem ser otimizados para performance e apresentação.
*   **Single Responsibility Principle (SOLID)**: Alinha-se ao primeiro princípio do SOLID, onde uma classe (neste caso, um `Handler`) tem uma única responsabilidade: ou alterar o estado do sistema (Command) ou buscar dados (Query), mas não ambos.
*   **Escalabilidade**: Permite escalar as partes de leitura e escrita do sistema de forma independente.

## Camadas

A solução segue os princípios da **Clean Architecture**, dividindo-se em camadas concêntricas, onde as dependências apontam sempre para dentro.

### 1. Domain (Core)

*   **Localização**: `src/Core/MotorcycleRental.Domain`
*   **Responsabilidade**: A camada mais interna. Contém a lógica de negócio pura, encapsulada em Agregados e Entidades. Define as interfaces para os repositórios, que são os contratos para a persistência de dados. Não depende de nenhuma outra camada.

### 2. Application (Core)

*   **Localização**: `src/Core/MotorcycleRental.Application`
*   **Responsabilidade**: Orquestra os casos de uso do sistema. É aqui que residem os `Handlers` de Commands e Queries. Esta camada depende do Domínio, mas é independente da Infraestrutura. Ela define as regras da aplicação, utilizando o domínio para executar a lógica de negócio.

### 3. Infrastructure

*   **Localização**: `src/Infrastructure/MotorcycleRental.Infrastructure`
*   **Responsabilidade**: Implementa as abstrações (interfaces) definidas nas camadas internas. Contém:
    *   **Persistência**: Implementações dos repositórios usando Entity Framework Core para o PostgreSQL.
    *   **Mensageria**: Serviços para publicação de eventos com RabbitMQ.
    *   **Storage**: Implementação de serviços de armazenamento de arquivos (ex: imagem da CNH).
    *   **Banco de Dados NoSQL (MongoDB)**: Utilizado para armazenamento de eventos e notificações. Sua flexibilidade de esquema e performance de leitura o tornam ideal para dados que não são alterados com frequência mas precisam ser consultados rapidamente.

### 4. Presentation (API)

*   **Localização**: `src/Presentation/MotorcycleRental.API`
*   **Responsabilidade**: Ponto de entrada síncrono da aplicação. É uma API REST que expõe os casos de uso para o cliente. Os `Controllers` recebem requisições HTTP, as convertem em `Commands` ou `Queries` e as despacham (via MediatR) para a camada de Aplicação.

### 5. Workers

*   **Localização**: `src/Workers/Consumers`
*   **Responsabilidade**: Ponto de entrada assíncrono. Contém processos que rodam em segundo plano, consumindo eventos de uma fila (RabbitMQ). São essenciais para processar tarefas que podem ser demoradas ou que não precisam ser executadas em tempo real.

## Fluxo de Dados

### Fluxo de Comando (Operação de Escrita)

1.  Uma requisição HTTP (`POST`, `PUT`, `DELETE`) chega a um `Controller` na camada **Presentation**.
2.  O `Controller` cria um objeto de `Command` com os dados da requisição.
3.  O `Command` é enviado para o MediatR, que o direciona para o `CommandHandler` correspondente na camada **Application**.
4.  O `Handler` busca os agregados necessários do banco de dados através das interfaces de repositório.
5.  A lógica de negócio no agregado (camada **Domain**) é executada.
6.  O `Handler` persiste as alterações no banco de dados (PostgreSQL) através da implementação do repositório na camada **Infrastructure**.
7.  Se a operação de negócio gerar eventos de domínio, eles são publicados em uma fila (RabbitMQ).

### Fluxo de Query (Operação de Leitura)

1.  Uma requisição HTTP (`GET`) chega a um `Controller` na camada **Presentation**.
2.  O `Controller` cria um objeto de `Query`.
3.  A `Query` é enviada para o `QueryHandler` correspondente na camada **Application**.
4.  Para otimizar a performance, o `Handler` pode consultar o banco de dados diretamente (muitas vezes ignorando os agregados do domínio) e mapear o resultado para um DTO (Data Transfer Object).
5.  O DTO é retornado ao `Controller`, que o serializa como uma resposta HTTP JSON.

### Fluxo de Evento (Operação Assíncrona)

1.  Um evento é publicado em uma fila (RabbitMQ), geralmente por um `CommandHandler`.
2.  Um `Consumer` na camada de **Workers** recebe a mensagem da fila.
3.  O `Consumer` processa o evento, o que pode envolver a execução de lógica de aplicação e a interação com a camada de **Infrastructure** (por exemplo, para salvar uma notificação no MongoDB ou enviar um e-mail).