# ASP.NET Core Eventos.IO

Projeto ASP.NET Core Dividido em camadas.

## Executando a migration

Abra o arquivo abaixo no editor de sua preferência.

    src\Eventos.IO.Infra.Data\Eventos.IO.Infra.Data.csproj

Edite a tag `TargetFramework` de:

```xml
<TargetFramework>netstandard2.0</TargetFramework>
```

Para:

```xml
<TargetFramework>netcoreapp2.2</TargetFramework>
```

Então vá até a pasta `src\Eventos.IO.Infra.Data\` pelo terminal e execute o comando abaixo.

    dotnet ef database update

Então volte a chave como estava de:

```xml
<TargetFramework>netcoreapp2.2</TargetFramework>
```

Para:

```xml
<TargetFramework>netstandard2.0</TargetFramework>
```

Processo finalizado.