internal class ChefService
{

  private IMongoCollection<Chef> _Chefs;

  public ChefService(String url) => this._Chefs = new MongoUrl(url).Collection<Chef>();

  public async Task<Chef> One(Expression<Func<Chef, bool>> filter) => await this._Chefs.Find(filter).FirstOrDefaultAsync();

  public async Task<IEnumerable<Chef>> Some(Expression<Func<Chef, bool>> filter) => await this._Chefs.Find(filter).ToListAsync();

  public async Task<IResult> Create(Chef chef)
  {

    try
    {
      await this._Chefs.InsertOneAsync(chef);
      return Results.Created<Chef>($"/chef/{chef.Name}", chef);
    }
    catch (MongoWriteException e)
    {
      if (e.WriteError.Code == 11000)
      {
        return Results.Conflict($"A chef already exists by the name of '{chef.Name}'");
      }
      throw;
    }
  }

  private void Conflict()
  {
    throw new NotImplementedException();
  }

  public async Task<DeleteResult> Delete(string name) => await this._Chefs.DeleteOneAsync(c => c.Name == name);

  public async Task<UpdateResult> AddCuisine(String name, IEnumerable<Cuisine> cuisines)
  {
    var scribble = Builders<Chef>.Update.AddToSetEach(c => c.Cuisines, cuisines);
    return await _Chefs.UpdateOneAsync(c => c.Name == name, scribble);
  }
}