
# DeveloperEvaluationAmbev

Este projeto segue os princípios da **Clean Architecture**, com separação clara entre camadas de domínio, aplicação, infraestrutura e apresentação.

## 🧱 Estrutura do Projeto

```
backend/
├── Ambev.DeveloperEvaluation.sln
├── src/
│   ├── Ambev.DeveloperEvaluation.Domain           # Entidades e interfaces de domínio
│   ├── Ambev.DeveloperEvaluation.Application      # Casos de uso e lógica de aplicação
│   ├── Ambev.DeveloperEvaluation.Common           # Utilitários e helpers comuns
│   ├── Ambev.DeveloperEvaluation.ORM              # Persistência e repositórios (Entity Framework)
│   ├── Ambev.DeveloperEvaluation.IoC              # Injeção de dependências
│   └── Ambev.DeveloperEvaluation.WebApi           # API REST
├── tests/
│   ├── Ambev.DeveloperEvaluation.Unit             # Testes unitários
│   ├── Ambev.DeveloperEvaluation.Integration      # Testes de integração
│   └── Ambev.DeveloperEvaluation.Functional       # Testes funcionais
```

## 🛠️ Executando o `update-database` via PowerShell

Certifique-se de que o projeto de inicialização esteja definido como `Ambev.DeveloperEvaluation.WebApi`. Então, no terminal PowerShell, navegue até a pasta `backend` e execute:

```powershell
cd .\backend
dotnet ef database update --startup-project .\src\Ambev.DeveloperEvaluation.WebApi\ --project .\src\Ambev.DeveloperEvaluation.ORM\
```

> 💡 Requer instalação do pacote `Microsoft.EntityFrameworkCore.Tools`

## 🐳 Executando com Docker Compose

Certifique-se de que o Docker esteja instalado e rodando. Para iniciar os serviços:

```powershell
cd .\backend
docker-compose up --build
```

> Isso irá subir a infraestrutura necessária (ex: banco de dados, aplicação).

---

## 📄 Licença

Este projeto está licenciado sob os termos do arquivo [LICENSE](../LICENSE).
