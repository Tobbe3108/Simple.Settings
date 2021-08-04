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
      FileInfo = new FileInfo(path);
      Load();
    }
    
    public override void Save()
    {
      OnBeforeSave();
      
      var json = JsonSerializer.Serialize(this, GetType(), ((SimpleSettingsJsonConfiguration)Configuration).JsonSerializerOptions);
      
      if (Configuration.EncryptionOptions is not null)
      {
        OnBeforeEncrypt();
        
        json.Encrypt(Configuration.EncryptionOptions.EncryptionKey, FileInfo);
        
        OnAfterEncrypt();
        OnAfterSave();
        
        return;
      }
      
      File.WriteAllText(FileInfo.FullName, json);
      
      OnAfterSave();
    }

    public override void Reload() => Load();

    private void Load()
    {
      OnBeforeLoad();
      
      var json = string.Empty;

      if (!FileInfo.Exists || FileInfo.Length == 0)
      {
        return;
      }

      if (Configuration.EncryptionOptions is not null)
      {
        OnBeforeDecrypt();
        
        json = EncryptionHelper.Decrypt(Configuration.EncryptionOptions.EncryptionKey, FileInfo);
        
        OnAfterDecrypt();
      }

      if (string.IsNullOrEmpty(json))
      {
        json = File.ReadAllText(FileInfo.FullName);
      }
      var source = JsonSerializer.Deserialize(json, GetType(), ((SimpleSettingsJsonConfiguration)Configuration).JsonSerializerOptions);
      CopyValues(this, source);
      
      OnAfterLoad();
    }
  }
}