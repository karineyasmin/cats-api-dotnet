# Cats API

## Atenção
O arquivo `secrets.json` está incluído no projeto, mas não contém as credenciais necessárias. Certifique-se de adicionar suas credenciais antes de executar o projeto.

## Descrição
Cats API é uma aplicação ASP.NET Core para gerenciar informações sobre gatos. A API permite adicionar, atualizar, deletar e visualizar informações sobre gatos, raças de gatos e imagens de gatos.

## Estrutura do Projeto


Cats.Api ├── appsettings.Development.json ├── appsettings.json ├── bin/ ├── Cats.Api.csproj ├── Controllers/ │ └── CatsController.cs ├── Database/ │ └── AppDbContext.cs ├── DTOs/ │ └── CatDto.cs ├── Migrations/ │ └── 20250129171850_AddCats.cs │ └── 20250129171850_AddCats.Designer.cs │ └── AppDbContextModelSnapshot.cs ├── Models/ │ └── Cat.cs │ └── CatBreed.cs │ └── CatImage.cs ├── obj/ ├── Program.cs ├── Properties/ │ └── launchSettings.json ├── Repositories/ ├── secrets.json └── Services/



## Configuração
1. Clone o repositório:
    ```sh
    git clone <URL_DO_REPOSITORIO>
    cd Cats.Api
    ```

2. Adicione suas credenciais no arquivo [secrets.json](http://_vscodecontentref_/5):
    ```json
    {
        "ConnectionStrings:DefaultConnection": "Server=localhost;Database=CatsDb;User=root;Password=root;",
        "TheCatApi:ApiKey": "sua_api_key_aqui"
    }
    ```

3. Restaure os pacotes NuGet:
    ```sh
    dotnet restore
    ```

4. Execute as migrações do banco de dados:
    ```sh
    dotnet ef database update
    ```

5. Execute o projeto:
    ```sh
    dotnet run
    ```

## Documentação
A documentação da API está disponível via Swagger. Quando o projeto estiver em execução, acesse `http://localhost:<porta>/` para visualizar a interface do Swagger.
