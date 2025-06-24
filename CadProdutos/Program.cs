using System.Runtime.CompilerServices;
using System.Windows.Markup;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


var produtos = new List<Produto>()
{
    new Produto() {Id = 1, Nome = "Teclado", Preco = 23.20M, Estoque = 24},
    new Produto() {Id = 2, Nome = "Monitor", Preco = 230.50M, Estoque = 12}
};

app.MapGet("/produtos", () =>
{
    return produtos;
});

app.MapGet("/produtos/{id}", (int id) =>
{
    var produto = produtos.FirstOrDefault(x => x.Id == id);
    return produto is not null ? Results.Ok(produto) : Results.NotFound($"Produto com ID {id} não encontrado.");
});

app.MapPost("/produtos", (Produto novoProduto) =>
{
    produtos.Add(novoProduto);
    return Results.Created();
});

app.MapPut("/produtos/{id}", (int id, Produto produtoAtualizado) =>
{
    var produto = produtos.FirstOrDefault(x => x.Id == id);

    if (produto is null)
    {
        return Results.NotFound($"Produto com ID {id} não encontrado.");
    }

    produto.Nome = produtoAtualizado.Nome;
    produto.Preco = produtoAtualizado.Preco;
    produto.Estoque = produtoAtualizado.Estoque;

    return Results.Ok(produto);
});

app.MapDelete("/produtos/{id}", (int id) =>
{
    var produto = produtos.FirstOrDefault(x => x.Id == id);

    if (produto is null)
    {
        return Results.NotFound($"Produto com ID {id} não encontrado.");
    }

    produtos.Remove(produto);

    return Results.NoContent();
});

app.Run();

class Produto
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
}
