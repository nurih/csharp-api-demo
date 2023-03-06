using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Chef
{
  public Chef(string name, IEnumerable<Cuisine> cuisines)
  {
    this.Name = name;
    this.Cuisines = cuisines.ToList();
  }

  [BsonId]
  public string Name { get; set; }

  [BsonRepresentation(BsonType.String)]
  public IEnumerable<Cuisine> Cuisines { get; private set; }
}

