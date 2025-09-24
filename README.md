# minimal-api

Projeto de exemplo utilizando .NET Minimal API com arquitetura em camadas, separando claramente API, domínio, infraestrutura e testes.

## Visão Geral
- **Api/**: Ponto de entrada da aplicação, configuração e endpoints HTTP.
- **Dominio/**: Lógica de negócio (DTOs, entidades, interfaces, serviços, model views).
- **Infraestrutura/**: Acesso a dados (Entity Framework Core, contexto do banco).
- **Test/**: Testes unitários e de integração, mocks e helpers.

## Arquitetura
- Segue arquitetura em camadas: API → Serviços de Domínio → Infraestrutura.
- Serviços de domínio implementam regras de negócio e são acessados via interfaces.
- DTOs para transferência de dados entre camadas.
- Entities representam os objetos centrais do negócio.
- ModelViews para respostas de API e erros de validação.
- Migrations gerenciadas via Entity Framework Core.

## Como rodar o projeto
1. **Build:**
   ```sh
   dotnet build minimal-api.sln
   ```
2. **Executar:**
   ```sh
   dotnet run --project Api/minimal-api.csproj
   ```
3. **Testes:**
   ```sh
   dotnet test Test/Test.csproj
   ```
4. **Migrations:**
   ```sh
   dotnet ef migrations add <Nome> --project Api/ --startup-project Api/
   dotnet ef database update --project Api/ --startup-project Api/
   ```
5. **Debug:**
   Perfis de execução em `Api/Properties/launchSettings.json`.

## Convenções
- Interfaces de serviço começam com `I` (ex: `IAdministradorServico`).
- DTOs terminam com `DTO`, entidades são substantivos singulares, serviços terminam com `Servico`.
- Testes espelham a estrutura do domínio.
- Injeção de dependência configurada em `Startup.cs`.
- Configuração de endpoints e middlewares em `Program.cs` e `Startup.cs`.

## Pontos de Integração
- Entity Framework Core para persistência.
- Autenticação JWT implementada (`JwtService.cs`).
- Dependências externas via NuGet.

## Exemplo: Adicionando uma nova entidade
1. Crie a entidade em `Dominio/Entidades/`.
2. Adicione um DTO em `Dominio/DTOs/`.
3. Implemente/atualize o serviço em `Dominio/Servicos/` e sua interface.
4. Registre o serviço em `Startup.cs`.
5. Adicione endpoints em `Program.cs`.
6. Crie testes em `Test/Domain/Entidades/` e `Test/Domain/Servicos/`.

## Principais arquivos
- `Api/Program.cs`, `Api/Startup.cs` – Entrada/configuração da API
- `Api/Dominio/Servicos/` – Lógica de negócio
- `Api/Infraestrutura/Db/DbContexto.cs` – Contexto EF Core
- `Api/Migrations/` – Migrations do banco
- `Test/` – Testes automatizados

---

Contribuições são bem-vindas! Para dúvidas ou sugestões, abra uma issue.
