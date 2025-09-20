using minimal_api.Infraestrutura.Db;
using minimal_api.Dominio.DTOs;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using minimal_api.Dominio.Interfaces;
using minimal_api.Dominio.Servicos;
using Microsoft.AspNetCore.Mvc;
using minimal_api.Dominio.ModelViews;
using minimal_api.Dominio.Entidades;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;

#region Builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();
builder.Services.AddScoped<IVeiculoServico, VeiculosServico>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT desta maneira: Bearer {seu token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});



builder.Services.AddDbContext<DbContexto>(
    Options => {
    Options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
        );
    }
);

var app = builder.Build();
#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");
#endregion

#region Admin
ErrorValidation validateDTOAdmin(AdministradorDTO administradorDTO)
{
    var validation = new ErrorValidation
    {
        Messages = new List<string>()
    };
    if (string.IsNullOrWhiteSpace(administradorDTO.Email))
    {
        validation.Messages.Add("O email não pode estar vazio");
    }
    if (string.IsNullOrWhiteSpace(administradorDTO.Perfil))
    {
        validation.Messages.Add("O perfil não pode estar vazio");
    }
    if (string.IsNullOrWhiteSpace(administradorDTO.Senha))
    {
        validation.Messages.Add("A senha não pode ser vazia");    
    }

    return validation;
}

app.MapPost("/administradores", async (AdministradorDTO administradorDTO, IAdministradorServico administradorServico) =>
{
    try
    {
        var administrador = new Administrador
        {
            Email = administradorDTO.Email,
            Senha = administradorDTO.Senha,
            Perfil = administradorDTO.Perfil
        };
        var novoAdministrador = await administradorServico.Create(administrador);
        return Results.Created($"/administradores/{novoAdministrador.Id}", new
        {
            novoAdministrador.Id,
            novoAdministrador.Email,
            novoAdministrador.Perfil
        });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { mensagem = ex.Message });
    }
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute{Roles = "ADM"})
.WithTags("Administradores");

app.MapPost("/administradores/login", async ([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico) =>
{
    var token = await administradorServico.Login(loginDTO);
    if (token == null)
    {
        return Results.Unauthorized();
    }
    return Results.Ok(new {Token = token});
}).WithTags("Administradores");

app.MapGet("/administradores", async ([FromQuery] int? pagina, IAdministradorServico administradorServico) =>
{
    var administradores = await administradorServico.FindAll(pagina ?? 1);
    return Results.Ok(administradores);
}).RequireAuthorization().WithTags("Administradores");

app.MapGet("/administradores/{id}", async (int id, IAdministradorServico administradorServico) =>
{
    var administrador = await administradorServico.FindById(id);
    return administrador is not null
    ? Results.Ok(administrador)
    : Results.NotFound();
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute{Roles = "ADM"})
.WithTags("Administradores");

app.MapPut("/administradores/{id}", async ([FromRoute] int id, AdministradorDTO administradorDTO, IAdministradorServico administradorServico) =>
{
    var validation = validateDTOAdmin(administradorDTO);
    if (validation.Messages.Count > 0) return Results.BadRequest(validation);

    var administrador = await administradorServico.FindById(id);
    if (administrador == null) return Results.NotFound();

    administrador.Email = administradorDTO.Email;
    administrador.Perfil = administradorDTO.Perfil;
    administrador.Senha = administradorDTO.Senha;

    await administradorServico.Update(administrador);
    return Results.Ok(administrador);
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute{Roles = "ADM"})
.WithTags("Administradores");

app.MapDelete("/administradores/{id}", async ([FromRoute] int id, IAdministradorServico administradorServico) =>
{
    var administrador = administradorServico.FindById(id);
    if (administrador == null) return Results.NotFound();
    await administradorServico.DeleteById(id);
    return Results.Ok();
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute{Roles = "ADM"})
.WithTags("Administradores");
#endregion

#region Veiculos

ErrorValidation validateDTOVeiculo(VeiculoDTO veiculoDTO)
{
    var validation = new ErrorValidation
    {
        Messages = new List<string>()
    };
    if (string.IsNullOrWhiteSpace(veiculoDTO.Nome))
    {
        validation.Messages.Add("O nome não pode ser vazio");
    }
    if (string.IsNullOrWhiteSpace(veiculoDTO.Marca))
    {
        validation.Messages.Add("A marca não pode ser vazia");
    }
    if (veiculoDTO.Ano < 1950)
    {
        validation.Messages.Add("O ano do veiculo não pode ser inferior a 1950");
    }

    return validation;
}

app.MapPost("/veiculos", async ([FromBody] VeiculoDTO veiculoDTO, IVeiculoServico VeiculosServico) =>
{

    var validation = validateDTOVeiculo(veiculoDTO);
    if (validation.Messages.Count > 0)
    {
        return Results.BadRequest(validation);
    }
    var veiculo = new Veiculo
    {
        Nome = veiculoDTO.Nome,
        Marca = veiculoDTO.Marca,
        Ano = veiculoDTO.Ano
    };
    await VeiculosServico.Create(veiculo);
    return Results.Created($"/veiculo/{veiculo.Id}", veiculo);
}).WithTags("Veiculos");

app.MapGet("/veiculos", async ([FromQuery] int? pagina, IVeiculoServico veiculoServico) =>
{
    var veiculos = await veiculoServico.FindAll(pagina ?? 1);
    return Results.Ok(veiculos);
}).WithTags("Veiculos");

app.MapGet("/veiculos/{id}", async  (int id, IVeiculoServico veiculoServico) =>
{
    var veiculo = await veiculoServico.FindById(id);
    return veiculo is not null
    ? Results.Ok(veiculo)
    : Results.NotFound();
}).WithTags("Veiculos");

app.MapPut("/veiculos/{id}", async ([FromRoute] int id, VeiculoDTO veiculoDTO, IVeiculoServico veiculoServico) =>
{

    var validation = validateDTOVeiculo(veiculoDTO);
    if (validation.Messages.Count > 0)
    {
        return Results.BadRequest(validation);
    }
    var veiculo = await veiculoServico.FindById(id);
    if (veiculo == null) return Results.NotFound();

    veiculo.Nome = veiculoDTO.Nome;
    veiculo.Marca = veiculoDTO.Marca;
    veiculo.Ano = veiculoDTO.Ano;

    await veiculoServico.Update(veiculo);

    return Results.Ok(veiculo);
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute{Roles = "ADM"})
.WithTags("Veiculos");

app.MapDelete("/veiculos/{id}",async ([FromRoute] int id, IVeiculoServico veiculoServico) =>
{
    var veiculo = await veiculoServico.FindById(id);
    if (veiculo == null) return Results.NotFound();
    await veiculoServico.DeleteById(id);
    return Results.Ok();
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute{Roles = "ADM"})
.WithTags("Veiculos");

#endregion

#region app
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();


app.Run();

#endregion
