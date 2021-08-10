using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Simple.Settings.Json
{
  public class ShouldSerializeContractResolver : DefaultContractResolver
  {
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
      JsonProperty property = base.CreateProperty(member, memberSerialization);

      if (property.DeclaringType == typeof(BaseSettings) && property.PropertyName == "Configuration")
      {
        property.ShouldSerialize = _ => false;
      }

      return property;
    }
  }
}