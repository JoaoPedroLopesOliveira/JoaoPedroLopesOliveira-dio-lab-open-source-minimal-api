var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


app.MapGet("/", () => "teste hot Reload");

app.MapPost("/login", (minimal_api.Dominio.DTOs.LoginDTO loginDTO) =>
{
    if (loginDTO.Email == "adm@teste.com" && loginDTO.Senha == "123456")
    {
        return Results.Ok("Logado com sucesso");
    }
    else
    {
        return Results.Unauthorized();
    }
});


app.Run();

