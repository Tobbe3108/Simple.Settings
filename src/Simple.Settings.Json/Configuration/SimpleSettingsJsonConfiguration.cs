using Newtonsoft.Json;
using Simple.Settings.Configuration;

namespace Simple.Settings.Json.Configuration
{
  public class SimpleSettingsJsonConfiguration : ISimpleSettingsConfiguration
  {
    public JsonSerializerSettings JsonSerializerSettings { get; set; } = new()
    {
      Formatting = Formatting.Indented,
      DefaultValueHandling = DefaultValueHandling.Populate,
      ContractResolver = new ShouldSerializeContractResolver(),
      ObjectCreationHandling = ObjectCreationHandling.Replace
    };

    public EncryptionOptions? EncryptionOptions { get; set; }
  }
}