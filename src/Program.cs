using MongoDB.Driver;
public class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration.AddEnvironmentVariables();
    builder.Services.AddCors();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    String uri = builder.Configuration.GetValue<String>("CHEFS_DB") ?? String.Empty;
    var chefDB = new ChefDB(uri);

    builder.Services.AddSingleton<ChefDB>(_ => chefDB);


    var app = builder.Build();

    app.UseSwagger();

    app.UseSwaggerUI(o => o.EnableTryItOutByDefault());

    app.UseHttpsRedirection();


    app.MapGet("/coin_toss/", () => Random.Shared.Next(2) > 0 ? "heads" : "tails")
       .Produces<string>();

    app.MapGet("/chef/{id}", async (string id) => await chefDB.Chefs.Find<Chef>(c => c.Name == id).FirstOrDefaultAsync())
       .Produces<Chef>();

    app.MapPost("/chef/", async (Chef chef) => await chefDB.Chefs.InsertOneAsync(chef));

    app.MapPatch("/chef/{id}", async (string id, Cuisine[] cuisines) => await chefDB.AddCuisine(id, cuisines));

    app.MapGet("/chef/cuisine/{cuisine}", async (Cuisine cuisine) => await chefDB.Chefs.Find<Chef>(c => c.Cuisines.Any(i => i == cuisine)
    ).ToListAsync())
       .Produces<List<Chef>>();

    app.MapDelete("/chef/{id}", async (string id) => await chefDB.Chefs.DeleteOneAsync(c=> c.Name == id));

    app.Run();
  }
}