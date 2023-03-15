public class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration.AddEnvironmentVariables();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddControllers(options => { options.Filters.Add<MongoExceptionFilter>(); });

    String uri = builder.Configuration.GetValue<String>("CHEFS_DB") ?? String.Empty;
    var chefDB = new ChefService(uri);

    builder.Services.AddSingleton<ChefService>(_ => chefDB);


    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();


    app.MapGet("/coin_toss/", () => Random.Shared.Next(2) > 0 ? "heads" : "tails")
       .Produces<string>();


    app.MapGet("/chef/{id}", async (string id) => await chefDB.One(c => c.Name == id))
       .Produces<Chef>();


    app.MapGet("/chef/", async () => await chefDB.Some(_ => true))
       .Produces<IEnumerable<Chef>>();


    app.MapPost("/chef/", async (Chef chef) => await chefDB.Create(chef));


    app.MapPatch("/chef/{id}", async (string id, Cuisine[] cuisines) => await chefDB.AddCuisine(id, cuisines)).Produces<Object>();


    app.MapGet("/chef/cuisine/{cuisine}", async (Cuisine cuisine) => await chefDB.SomeByCuisine(cuisine)
    ).Produces<List<Chef>>();


    app.MapDelete("/chef/{id}", async (string id) => await chefDB.Delete(id)).Produces<DeleteResult>();


    app.Run();
  }
}