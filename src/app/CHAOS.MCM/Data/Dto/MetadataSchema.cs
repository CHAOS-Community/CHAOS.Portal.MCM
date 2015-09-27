using Newtonsoft.Json;

namespace Chaos.Mcm.Data.Dto
{
  using System;
  using System.Xml.Linq;
  using CHAOS.Serialization;
  using CHAOS.Serialization.XML;
  using Portal.Core.Data.Model;

  public class MetadataSchema : AResult
  {
    [Serialize("Guid")]
    public Guid Guid { get; set; }

    [Serialize("Name")]
    public string Name { get; set; }

    public XDocument SchemaXmls
    {
      get { return XDocument.Parse(Schema); }
      set { Schema = value.ToString(SaveOptions.DisableFormatting); }
    }

    [SerializeXML(false, true)]
    [Serialize("Schema")]
    public string Schema { get; set; }

    [Serialize("DateCreated")]
    public DateTime DateCreated { get; set; }

    public MetadataSchema(Guid guid, string name, string schema, DateTime dateCreated)
    {
      Guid = guid;
      Name = name;
      Schema = schema;
      DateCreated = dateCreated;
    }

    public MetadataSchema()
    {
    }

    public T GetSchema<T>()
    {
      return JsonConvert.DeserializeObject<T>(Schema);
    }
  }
}