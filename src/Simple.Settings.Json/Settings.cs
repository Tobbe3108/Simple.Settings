using System.IO;
using System.Text.Json;
using Simple.Settings.Json.Configuration;

namespace Simple.Settings.Json
{
  public abstract partial class Settings
  {
    public override void Load(string path)
    {
      FileInfo = new FileInfo(path);
      if (!FileInfo.Exists || FileInfo.Length == 0)
      {
        return;
      }

      var json = File.ReadAllText(FileInfo.Name);
      var source = JsonSerializer.Deserialize(json, GetType(), ((SimpleSettingsJsonConfiguration)Configuration).JsonSerializerOptions);
      CopyValues(this, source);
    }
    
    public override void Save()
    {
      var json = JsonSerializer.Serialize(this, GetType(), ((SimpleSettingsJsonConfiguration)Configuration).JsonSerializerOptions);
      File.WriteAllText(FileInfo.Name, json);
    }
  }
}