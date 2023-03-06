using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Cuisine
{
  Afghan,
  Brazilian,
  Chinese,
  Danish,
  Ethiopian,
  French,
  German,
  Hungarian,
  Israeli,
  Japanese,
  Korean,
  Lebanese,
  Mexican,
  Nigerian,
  Omani,
  Polish,
  Qatari,
  Russian,
  Spanish,
  Thai,
  Ugandan,
  Vietnamese,
  Welsh,
  Xinjiang,
  Yemeni,
  Zimbabwean,

}