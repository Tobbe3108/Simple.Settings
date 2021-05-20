using System.IO;
using System.Text.Json;
using Simple.Settings.Helpers;
using Simple.Settings.Json.Configuration;

namespace Simple.Settings.Json
{
  public abstract partial class Settings
  {
    public override void Load(string path)
    {
      var json = string.Empty;
      
      FileInfo = new FileInfo(path);
      if (!FileInfo.Exists || FileInfo.Length == 0)
      {
        return;
      }

      if (Configuration.EncryptionOptions is not null)
      {
        json = EncryptionHelper.Decrypt(Configuration.EncryptionOptions.EncryptionKey, FileInfo);
      }

      if (string.IsNullOrEmpty(json))
      {
        json = File.ReadAllText(FileInfo.Name);
      }
      var source = JsonSerializer.Deserialize(json, GetType(), ((SimpleSettingsJsonConfiguration)Configuration).JsonSerializerOptions);
      CopyValues(this, source);
    }
    
    public override void Save()
    {
      var json = JsonSerializer.Serialize(this, GetType(), ((SimpleSettingsJsonConfiguration)Configuration).JsonSerializerOptions);
      
      if (Configuration.EncryptionOptions is not null)
      {
        json.Encrypt(Configuration.EncryptionOptions.EncryptionKey, FileInfo);
        return;
      }
      
      File.WriteAllText(FileInfo.Name, json);
    }
  }
}