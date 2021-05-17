using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Simple.Settings.Json
{
  public abstract partial class Settings
  {
    public override void Load(string path, bool createIfNotExisting = true)
    {
      FileInfo = new FileInfo(path);

      CreateOrThrow(createIfNotExisting);

      var json = File.ReadAllText(FileInfo.Name);
      if (string.IsNullOrEmpty(json))
      {
        return;
      }

      var source = JsonSerializer.Deserialize(json, GetType(), JsonSerializerOptions);
      CopyValues(this, source);
    }
    
    public override void Save(bool createIfNotExisting = true)
    {
      CreateOrThrow(createIfNotExisting);
      var json = JsonSerializer.Serialize(this, GetType(), JsonSerializerOptions);
      File.WriteAllText(FileInfo.Name, json);
    }
    
    private void CreateOrThrow(bool createIfNotExisting)
    {
      switch (FileInfo.Exists)
      {
        case false when createIfNotExisting:
        {
          using var fileStream = File.OpenWrite(FileInfo.Name);
          fileStream.Write(new byte[0], 0, 0);
          break;
        }
        case false:
          throw new FileNotFoundException();
      }
    }
  }
}