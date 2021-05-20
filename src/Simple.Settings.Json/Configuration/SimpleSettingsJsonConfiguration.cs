using System.Text.Json;
using Simple.Settings.Configuration;

namespace Simple.Settings.Json.Configuration
{
  public class SimpleSettingsJsonConfiguration : ISimpleSettingsConfiguration
  {
    public JsonSerializerOptions JsonSerializerOptions { get; set; } = new()
    {
      WriteIndented = true
    };

    public SaveOnPropertyChanged? SaveOnPropertyChanged { get; set; }
    public EncryptionOptions? EncryptionOptions { get; set; }
  }
}