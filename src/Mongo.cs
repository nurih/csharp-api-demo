


using Microsoft.AspNetCore.Mvc.Filters;

public static class Mongo
{
  public static IMongoCollection<T> Collection<T>(this MongoUrl url)
  {
    return new MongoClient(url)
      .GetDatabase(url.DatabaseName)
      .GetCollection<T>(typeof(T).Name);
  }



}
