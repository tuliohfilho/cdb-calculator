# Calculadora de Investimento CDB

## Visão Geral do Projeto

Este projeto consiste em uma aplicação web simples para calcular o rendimento bruto e líquido de um investimento em CDB (Certificado de Depósito Bancário), baseado em um valor inicial e um prazo em meses. A aplicação é composta por um backend desenvolvido em .NET 8 com C# (seguindo princípios de Domain-Driven Design - DDD) e um frontend em Angular.

O objetivo principal foi criar uma solução robusta, bem arquitetada e fácil de executar, utilizando Docker para containerização.

## Funcionalidades

*   **Interface Web:** Permite ao usuário inserir o valor inicial do investimento (positivo) e o prazo em meses (maior que 1).
*   **Cálculo de CDB:** O backend realiza o cálculo do valor bruto do investimento mês a mês, utilizando taxas fixas de CDI (0.9%) e TB (108%) conforme especificado.
*   **Cálculo de Imposto:** Calcula o imposto de renda sobre o lucro do investimento, baseado na tabela progressiva fornecida.
*   **Exibição de Resultados:** Apresenta ao usuário o valor final bruto e o valor final líquido do investimento.
*   **Validações:** Garante que os valores de entrada sejam válidos (valor inicial positivo, prazo maior que 1 mês).

## Arquitetura

A solução segue uma arquitetura separada entre backend e frontend, com o backend adotando princípios de Domain-Driven Design (DDD) para uma melhor organização e manutenibilidade.

### Backend (.NET 8 / C#)

O backend é estruturado em camadas lógicas, com foco em clareza, organização e boas práticas:

*   **`CdbCalculator.Application`:** Orquestra as operações. Contém:
    *   **Commands:** Define comandos de cálculo (ex: `CdbCalculateCommand`).
    *   **Dtos:** Contém objetos de transferência de dados, como:
        *   `CdbCalculateRequest`: Dados de entrada para o cálculo do CDB.
        *   `CdbCalculateResponse`: Resultado calculado do CDB.
    *   **Extensions:** Métodos de extensão para decimal e configurações de serviços.
    *   **Interfaces:** Define contratos de serviços (ex: `ICdbCalculationService`).
    *   **Services:** Implementações de serviços de negócio (ex: `CdbCalculationService`).
    *   **Validators:** Validações para os DTOs de entrada.
*   **`CdbCalculator.Application.Tests`:** Contém os testes automatizados para a camada Application.
*   **`CdbCalculator.Api`:** Camada de apresentação (Web API). Expõe os endpoints RESTful (ex: `/api/cdb/calculate`) para o frontend consumir. Utiliza controllers (`CdbController`) que recebem as requisições, invocam os serviços da camada Application e retornam as respostas. Também configura a injeção de dependências e o CORS.

**Padrões e Princípios:**

*   **SOLID:** O código segue os princípios SOLID para garantir design limpo, flexível e de fácil manutenção.
*   **Injeção de Dependência (DI):** Utilizada para desacoplar as camadas (configurada no `Program.cs` da API).
*   **Separação de Responsabilidades:** Organiza as responsabilidades em módulos coesos para facilitar a manutenção e evolução do código.


### Frontend (Angular)

O frontend é uma Single Page Application (SPA) desenvolvida com Angular (utilizando componentes standalone):

*   **`AppComponent`:** Componente principal que contém:
    *   Template HTML (`app.html`) com os campos de input (valor inicial, meses) e a área para exibição dos resultados (bruto e líquido) e mensagens de erro.
    *   Lógica TypeScript (`app.ts`) para:
        *   Gerenciar o estado dos inputs (`ngModel`).
        *   Chamar a API backend (`HttpClient`) ao clicar no botão "Calcular".
        *   Tratar respostas de sucesso e erro da API.
        *   Exibir os resultados ou mensagens de erro na interface.
    *   Estilização CSS (`app.css`) para a aparência da página.
*   **`app.config.ts`:** Configuração da aplicação Angular, incluindo o `provideHttpClient` para permitir requisições HTTP.

## Tecnologias Utilizadas

*   **Backend:** .NET 8, C#
*   **Frontend:** Angular CLI, TypeScript, HTML, CSS
*   **Containerização:** Docker, Docker Compose
*   **Servidor Web (Frontend):** Nginx (dentro do container Docker)

## Pré-requisitos

Para executar esta aplicação localmente utilizando Docker, você precisará ter instalado:

*   [Docker](https://docs.docker.com/get-docker/)
*   [Docker Compose](https://docs.docker.com/compose/install/)

## Como Executar

1.  **Clone o repositório:**
    ```bash
    git clone <URL_DO_REPOSITORIO>
    cd <NOME_DO_DIRETORIO>
    ```

2.  **Execute com Docker Compose:**
    Navegue até o diretório raiz do projeto (onde o arquivo `docker-compose.yml` está localizado) e execute o seguinte comando no terminal:
    ```bash
    docker-compose up --build
    ```
    *   O comando `--build` garante que as imagens Docker sejam construídas (necessário na primeira vez ou após alterações no código).
    *   O Docker Compose irá construir as imagens para o backend e frontend e iniciar os containers.

3.  **Acesse a Aplicação:**
    *   **Frontend:** Abra seu navegador e acesse `http://localhost:4200`.
    *   **Backend API (para testes diretos, se necessário):** A API estará acessível em `http://localhost:5000`. Você pode usar ferramentas como Postman ou curl para enviar requisições POST para `http://localhost:5000/api/cdb/calculate` com um corpo JSON como:
        ```json
        {
          "initialValue": 1000,
          "months": 12
        }
        ```

4.  **Para Parar a Aplicação:**
    Pressione `Ctrl + C` no terminal onde o `docker-compose up` está rodando. Para remover os containers, execute:
    ```bash
    docker-compose down
    ```

## Decisões de Design

*   **.NET 8 vs .NET Framework 4.7.2:** Embora o documento original mencionasse .NET Framework 4.7.2, optou-se por usar .NET 8. As razões incluem:
    *   Suporte nativo e mais robusto a C# 8 e versões posteriores.
    *   Melhor desempenho e recursos modernos da plataforma .NET.
    *   Excelente integração com Docker e ambientes multi-plataforma (Linux, macOS, Windows).
    *   Ciclo de vida de suporte mais longo.
*   **Angular Standalone Components:** Utilizado para simplificar a estrutura do projeto Angular, alinhado com as práticas mais recentes do framework.
*   **Validações no Domínio:** As validações de regras de negócio (valor inicial positivo, meses > 1) foram colocadas no construtor da entidade `InvestmentInput` no Domínio, garantindo que objetos inválidos não possam ser criados.
*   **Tratamento de Erros:** A API retorna códigos de status HTTP apropriados (400 para Bad Request, 500 para Internal Server Error) e o frontend exibe mensagens de erro amigáveis.
*   **Dockerização:** Facilita a configuração do ambiente de desenvolvimento e a execução da aplicação de forma consistente.

## Possíveis Melhorias

*   **Configuração de Ambiente:** Tornar a URL da API no frontend configurável por ambiente (desenvolvimento, produção) em vez de fixa no código.
*   **Interface do Usuário:** Melhorar a experiência do usuário (UX) e o design visual (UI) do frontend.
*   **Tratamento de Erros Mais Detalhado:** Implementar logging mais robusto e talvez um mecanismo global de tratamento de exceções na API.
*   **Segurança:** Adicionar medidas de segurança apropriadas se a aplicação fosse exposta publicamente.
*   **Variáveis de Taxa:** Carregar as taxas CDI e TB de uma configuração externa em vez de valores fixos no código.

