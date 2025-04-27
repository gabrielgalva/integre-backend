# IntegreBackend

Esse é o backend que desenvolvi usando **ASP.NET Core**, **Entity Framework Core (EF Core)** e **SQLite**. O objetivo foi criar APIs seguras para registro e autenticação de usuários (tanto estudantes quanto empresas), usando **JWT (JSON Web Token)**, além de preparar a base para gerenciar cursos, projetos e vagas de emprego.

## 📚 O que eu fiz até agora?

1. **Configurei o Banco de Dados**:
   - Escolhi o **SQLite** por ser leve e simples pra ambiente local.
   - Ajustei a string de conexão no `appsettings.json` pra apontar para o arquivo `.db`.

2. **Criei as Entidades (Models)**:
   - `ApplicationUser` herda do `IdentityUser` e inclui campos como `FullName` e `Role`.
   - Modelei também `Course`, `Project` e `JobVacancy`.

3. **Configurei o Contexto (`ApplicationDbContext`)**:
   - Fiz a integração do EF Core com o Identity e conectei minhas entidades ao banco.

4. **Configurei o Identity e a Autenticação JWT**:
   - Ativei o Identity pra gerenciar autenticação.
   - Configurei a autenticação **JWT** com validação de token, issuer e audience.

5. **Rodei as Migrations**:
   - Criei a primeira migration com:
     ```bash
     dotnet ef migrations add InitialCreate
     ```
   - Apliquei no banco com:
     ```bash
     dotnet ef database update
     ```

6. **Criei os Endpoints de Autenticação**:
   - No `AuthController`, adicionei:
     - **POST /api/Auth/register**: para registrar novos usuários.
     - **POST /api/Auth/login**: para autenticar e gerar o token JWT.

7. **Ativei o Swagger**:
   - Configurei o Swagger pra facilitar o teste das APIs em:
     ```
     http://localhost:5132/swagger
     ```

---

## 🚀 Tecnologias que usei

- **ASP.NET Core 8.0**
- **Entity Framework Core (EF Core)**
- **SQLite**
- **JWT (JSON Web Token)** pra autenticação
- **Swagger (Swashbuckle)** pra documentação das APIs

---

## ⚙️ Como rodar o projeto localmente

1. **Clonei o repositório:**

```bash
git clone https://github.com/seu-usuario/seu-repositorio.git
cd IntegreBackend
Restaurei os pacotes:

bash
Copiar código
dotnet restore
Apliquei as migrations pra criar o banco:

bash
Copiar código
dotnet ef database update
Rodei a aplicação:

bash
Copiar código
dotnet run
A API fica disponível em:

bash
Copiar código
http://localhost:5132/swagger
🔑 Endpoints disponíveis
POST /api/Auth/register: Registra um novo usuário (empresa ou estudante).

POST /api/Auth/login: Faz login e retorna um token JWT.

Exemplo pra Registro:
json
Copiar código
{
  "fullName": "Gabriel Galvão",
  "role": "Student",
  "email": "gabriel@example.com",
  "password": "Senha@123"
}
Exemplo pra Login:
json
Copiar código
{
  "email": "gabriel@example.com",
  "password": "Senha@123"
}
