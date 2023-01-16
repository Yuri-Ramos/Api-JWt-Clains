using Api_JWt_Clains.Data;
using Microsoft.EntityFrameworkCore;
using Api_JWt_Clains.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MinimalContextDb>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/fonercedor",async (MinimalContextDb context)=> await context.Fornecedores.ToListAsync())
.WithName("GetFornecedor")
.WithTags("Fornecedor");

app.MapGet("/fonercedor/{id}",async 
(Guid id,
 MinimalContextDb context)=> 
       await context.Fornecedores.FindAsync(id)
              is Fornecedor fornecedor
                  ? Results.Ok(fornecedor)
                  : Results.NotFound())        
        .Produces<Fornecedor>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetFornecedorPorId")
        .WithTags("Fornecedor");

app.MapPost("/Fornecedor", async (
  MinimalContextDb context, Fornecedor fornecedor )=>{
    context.Fornecedores.Add(fornecedor);
    var result = await context.SaveChangesAsync();

  }
).Produces<Fornecedor>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithName("GetFornecedorPorId")
        .WithTags("Fornecedor");

app.Run();
