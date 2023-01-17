using Api_JWt_Clains.Data;
using Microsoft.EntityFrameworkCore;
using Api_JWt_Clains.Models;
using MiniValidation;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MinimalContextDb>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityEntityFrameworkContextConfiguration(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
b=> b.MigrationsAssembly("MinimalPilot")));


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

app.MapGet("/fonercedor", async (MinimalContextDb context) => await context.Fornecedores.ToListAsync())
.WithName("GetFornecedor")
.WithTags("Fornecedor");

app.MapGet("/fonercedor/{id}", async
(Guid id,
 MinimalContextDb context) =>
       await context.Fornecedores.FindAsync(id)
              is Fornecedor fornecedor
                  ? Results.Ok(fornecedor)
                  : Results.NotFound())
        .Produces<Fornecedor>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetFornecedorPorId")
        .WithTags("Fornecedor");


app.MapPost("/fornecedor", async (
  MinimalContextDb context,
  Fornecedor fornecedor) =>
  {
      context.Fornecedores.Add(fornecedor);
      var result = await context.SaveChangesAsync();
  })
  .Produces<Fornecedor>(StatusCodes.Status201Created)
  .Produces(StatusCodes.Status400BadRequest)
  .WithName("PostFornecedor")
  .WithTags("Fornecedor");

app.MapPut("/fornecedor/{id}", async (
    Guid id, MinimalContextDb context, Fornecedor fornecedor

) =>
{
    var fornecedorBanco = await context.Fornecedores.AsNoTracking<Fornecedor>()
    .FirstOrDefaultAsync(f=>f.Id ==id);
    if (fornecedorBanco == null) return Results.NotFound();
 if (!MiniValidator.TryValidate(fornecedor, out var errors))
        return Results.ValidationProblem(errors);
    context.Fornecedores.Update(fornecedor);
    var result = await context.SaveChangesAsync();

    return result > 0
        ? Results.NoContent()
        : Results.BadRequest("Houve um problema ao salvar o registro");


}).ProducesValidationProblem()
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status400BadRequest)
.WithName("PutFornecedor")
.WithTags("Fornecedor");




app.MapDelete("/fornecedor/{id}", async (
    Guid id,
    MinimalContextDb context) =>
{
    var fornecedor = await context.Fornecedores.FindAsync(id);
    if (fornecedor == null) return Results.NotFound();

    context.Fornecedores.Remove(fornecedor);
    var result = await context.SaveChangesAsync();

    return result > 0
        ? Results.NoContent()
        : Results.BadRequest("Houve um problema ao salvar o registro");

}).Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound)
.RequireAuthorization("ExcluirFornecedor")
.WithName("DeleteFornecedor")
.WithTags("Fornecedor");


app.Run();
