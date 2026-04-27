<div align="center">

# ProductClientHub

**RESTful API para gerenciamento de Produtos e Clientes**

[![.NET](https://img.shields.io/badge/.NET_10-512BD4?style=flat-square&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-239120?style=flat-square&logo=csharp&logoColor=white)](https://learn.microsoft.com/dotnet/csharp/)
[![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=flat-square&logo=swagger&logoColor=black)](https://swagger.io/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow?style=flat-square)](./LICENSE)
[![Status](https://img.shields.io/badge/Status-Em_Desenvolvimento-orange?style=flat-square)]()

<br/>

> API desenvolvida com **.NET 10** como parte dos estudos na trilha backend da [Rocketseat](https://rocketseat.com.br), aplicando boas práticas de arquitetura em camadas, tratamento centralizado de exceções e contratos bem definidos de comunicação.

</div>

---

## Índice

- [Visão Geral](#visão-geral)
- [Arquitetura](#arquitetura)
- [Tecnologias](#tecnologias)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Como Executar](#como-executar)
- [Endpoints da API](#endpoints-da-api)
- [Tratamento de Erros](#tratamento-de-erros)
- [Autor](#autor)

---

## Visão Geral

O **ProductClientHub** é uma API RESTful voltada para o gerenciamento centralizado de **produtos** e **clientes**. O projeto foi concebido com foco em clareza arquitetural e separação de responsabilidades, dividindo a solução em projetos independentes que representam camadas bem definidas do sistema.

---

## Arquitetura

O projeto adota uma **arquitetura em camadas** organizada em três projetos distintos dentro da mesma solution:

```
┌─────────────────────────────────────────────────────────┐
│                   ProductClientHub.API                  │
│         Controllers · Program.cs · Middlewares          │
├─────────────────────────────────────────────────────────┤
│              ProductClientHub.Comunication              │
│              Requests · Responses · DTOs                │
├─────────────────────────────────────────────────────────┤
│              ProductClientHub.Exceptions                │
│        Exceções customizadas · Tratamento global        │
└─────────────────────────────────────────────────────────┘
```

| Camada | Responsabilidade |
|--------|-----------------|
| `API` | Exposição dos endpoints, configuração do pipeline HTTP e injeção de dependências |
| `Comunication` | Contratos de entrada e saída da API (requests e responses tipados) |
| `Exceptions` | Exceções de domínio customizadas e filtro global de erros |

---

## Tecnologias

| Tecnologia | Versão | Uso |
|---|---|---|
| [.NET](https://dotnet.microsoft.com/) | 10 | Framework principal |
| [ASP.NET Core](https://learn.microsoft.com/aspnet/core) | 10 | Web API |
| [C#](https://learn.microsoft.com/dotnet/csharp/) | 13 | Linguagem |
| [Swagger / Scalar](https://swagger.io/) | 10 | Documentação interativa |
| [Entity Framework Core](https://learn.microsoft.com/ef/core/) | 10 | ORM *(se aplicável)* |
| [Serilog](https://serilog.net/) | - Logs

---

## Estrutura do Projeto

```
ProductClientHub/
│
├── ProductClientHub.API/
│   ├── Controllers/
│   │   ├── ProductController.cs
│   │   └── ClientController.cs
│   ├── Filters/
│   ├── Program.cs
│   └── appsettings.json
│
├── ProductClientHub.Comunication/
│   ├── Requests/
│   │   ├── RequestProductJson.cs
│   │   └── RequestClientJson.cs
│   └── Responses/
│       ├── ResponseProductJson.cs
│       └── ResponseClientJson.cs
│
├── ProductClientHub.Exceptions/
│   └── ExceptionsBase/
│       ├── ProductClientHubException.cs
│       └── NotFoundException.cs
│
└── ProductClientHub.slnx
```

---

## Como Executar

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) ou superior
- [Git](https://git-scm.com/)

### Passo a passo

```bash
# 1. Clone o repositório
git clone https://github.com/Foqsz/ProductClientHub.git

# 2. Acesse a pasta raiz da solution
cd ProductClientHub

# 3. Restaure as dependências
dotnet restore

# 4. Execute a API
dotnet run --project ProductClientHub.API
```

A API estará disponível nos endereços abaixo:

| Ambiente | URL |
|----------|-----|
| HTTP | `http://localhost:5000` |
| HTTPS | `https://localhost:5001` |
| Swagger UI | `https://localhost:5001/swagger` |

---

## Endpoints da API

> A documentação interativa completa está disponível via **Swagger UI** após iniciar a aplicação.

### Produtos — `/api/products`

| Método | Rota | Descrição |
|--------|------|-----------|
| `GET` | `/api/products` | Retorna todos os produtos |
| `GET` | `/api/products/{id}` | Retorna um produto pelo ID |
| `POST` | `/api/products` | Cadastra um novo produto |
| `PUT` | `/api/products/{id}` | Atualiza os dados de um produto |
| `DELETE` | `/api/products/{id}` | Remove um produto |

### Clientes — `/api/clients`

| Método | Rota | Descrição |
|--------|------|-----------|
| `GET` | `/api/clients` | Retorna todos os clientes |
| `GET` | `/api/clients/{id}` | Retorna um cliente pelo ID |
| `POST` | `/api/clients` | Cadastra um novo cliente |
| `PUT` | `/api/clients/{id}` | Atualiza os dados de um cliente |
| `DELETE` | `/api/clients/{id}` | Remove um cliente |

---

## Tratamento de Erros

O projeto utiliza **exceções customizadas** centralizadas na camada `Exceptions`, garantindo respostas padronizadas em toda a API.

```json
{
  "title": "Recurso não encontrado",
  "status": 404,
  "errors": ["O produto com o ID informado não existe."]
}
```

| Código | Significado |
|--------|-------------|
| `200` | Operação realizada com sucesso |
| `201` | Recurso criado com sucesso |
| `400` | Requisição inválida |
| `404` | Recurso não encontrado |
| `500` | Erro interno do servidor |

---

## Autor

<div align="left">

**Victor Vinicius Alves de L. Souza**  
Backend Developer · Estudante de Análise e Desenvolvimento de Sistemas

[![LinkedIn](https://img.shields.io/badge/LinkedIn-victorvinicius-0A66C2?style=flat-square&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/victorvinicius/)
[![GitHub](https://img.shields.io/badge/GitHub-Foqsz-181717?style=flat-square&logo=github&logoColor=white)](https://github.com/Foqsz)
[![Portfolio](https://img.shields.io/badge/Portfolio-foqsz.github.io-blueviolet?style=flat-square&logo=googlechrome&logoColor=white)](https://foqsz.github.io/)

</div>

---

<div align="center">
  <sub>Desenvolvido durante os estudos na trilha backend da <a href="https://rocketseat.com.br">Rocketseat e aprimorado.</a> 🚀</sub>
</div>
