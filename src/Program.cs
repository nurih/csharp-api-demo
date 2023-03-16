public class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder();

    builder.Configuration.AddEnvironmentVariables();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();

    String uri = builder.Configuration.GetValue<String>("CHEFS_DB") ?? String.Empty;
    var chefDB = new ChefService(uri);

    app.MapGet("/coin_toss/", () => Random.Shared.Next(2) > 0 ? "👍" : "👎")
       .Produces<string>();

    app.MapGet("/chef/", async () => await chefDB.Some(_ => true))
       .Produces<IEnumerable<Chef>>();

    app.MapGet("/chef/{id}", async (string id) => await chefDB.One(c => c.Name == id))
       .Produces<Chef>();


    app.MapPost("/chef/", async (Chef chef) => await chefDB.Create(chef)).Produces(200).Produces(201).Produces(409);


    app.MapPatch("/chef/{id}", async (string id, Cuisine[] cuisines) => await chefDB.AddCuisine(id, cuisines)).Produces<Object>();

    app.MapGet("/chisine/", async () => await chefDB.CuisinesInUse()).Produces<IEnumerable<string>>();

    app.MapGet("/chef/cuisine/{cuisine}", async (Cuisine cuisine) => await chefDB.Some(chef => chef.Cuisines.Any(item => item == cuisine))
    ).Produces<List<Chef>>();


    app.MapDelete("/chef/{id}", async (string id) => await chefDB.Delete(id)).Produces<DeleteResult>();


    app.Run();
  }
}