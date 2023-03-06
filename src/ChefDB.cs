using MongoDB.Driver;
internal class ChefDB
{

  public IMongoCollection<Chef> Chefs { get; private set; }

  public ChefDB(String url) => this.Chefs = new MongoUrl(url).Collection<Chef>();


  public async Task<UpdateResult> AddCuisine(String name, IEnumerable<Cuisine> cuisines)
  {
    var scribble = Builders<Chef>.Update.AddToSetEach(c => c.Cuisines, cuisines);
    return await Chefs.UpdateOneAsync(c => c.Name == name, scribble);
  }  
}