using Microsoft.EntityFrameworkCore;
using MinimalApiEntityFramework.Models;
using MinimalApiEntityFramework.PizzaDb;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<PizzaDb>(options => options.UseInMemoryDatabase("items"));
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


//app.MapGet("/", () => "Hello World!");

//Obtener todas las pizzas
app.MapGet("/pizzas", async (PizzaDb db) => await db.Pizzas.ToListAsync());

//Obtener pizza por id
app.MapGet("/pizza/{id}", async (PizzaDb db, int id) => await db.Pizzas.FindAsync(id));

//Create
app.MapPost("/pizza", async (PizzaDb db, PizzaModel pizza) =>
{
    await db.Pizzas.AddAsync(pizza);
    await db.SaveChangesAsync();
    return Results.Created($"/pizza/{pizza.Id}", pizza);
});

//Update
app.MapPut("/pizza/{id}", async (PizzaDb db, PizzaModel updatePizza, int id) =>
{
    var pizza = await db.Pizzas.FindAsync(id);
    if (pizza == null) return Results.NotFound();
    pizza.Name = updatePizza.Name;
    pizza.Description = updatePizza.Description;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

//Delete
app.MapDelete("/pizza/id", async(PizzaDb db, int id) => {
    var pizza = await db.Pizzas.FindAsync(id);
    if (pizza == null) return Results.NotFound();

    db.Remove(pizza);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.Run();
