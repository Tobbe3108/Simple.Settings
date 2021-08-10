using System;
using System.IO;
using Newtonsoft.Json;
using Simple.Settings.Annotations;
using Simple.Settings.Helpers;
using Simple.Settings.Json.Configuration;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Simple.Settings.Json
{
  public abstract partial class Settings
  {
    public void Load([NotNull] string path)
    {
      if (path == null) throw new ArgumentNullException(nameof(path));
      FileInfo = new FileInfo(path);
      if (!FileInfo.Exists || FileInfo.Length == 0)
      {
        return;
      }

      InternalLoad(FileInfo.FullName);
    }

    public void Reload()
    {
      if (FileInfo == null) throw new ArgumentNullException(nameof(FileInfo));
      if (!FileInfo.Exists || FileInfo.Length == 0)
      {
        return;
      }
      
      InternalLoad(FileInfo.FullName);
    }

    public void Save()
    {
      if (FileInfo == null) throw new ArgumentNullException(nameof(FileInfo));
      InternalSave(FileInfo.FullName);
    }

    protected override void InternalSave(string path)
    {
      OnBeforeSave();
      
      var json = JsonSerializer.Serialize(this, GetType(), ((SimpleSettingsJsonConfiguration)Configuration).JsonSerializerOptions);
      
      if (Configuration.EncryptionOptions is not null)
      {
        OnBeforeEncrypt();
        
        json.Encrypt(Configuration.EncryptionOptions.EncryptionKey, path);
        
        OnAfterEncrypt();
        OnAfterSave();
        
        return;
      }
      
      File.WriteAllText(path, json);
      
      OnAfterSave();
    }

    protected override void InternalReload(string path) => Load(path);

    protected override void InternalLoad(string path)
    {
      OnBeforeLoad();
      
      var json = string.Empty;
      
      if (Configuration.EncryptionOptions is not null)
      {
        OnBeforeDecrypt();
        
        json = EncryptionHelper.Decrypt(Configuration.EncryptionOptions.EncryptionKey, path);
        
        OnAfterDecrypt();
      }

      if (string.IsNullOrEmpty(json))
      {
        json = File.ReadAllText(path);
      }

      var source = JsonSerializer.Deserialize(json, GetType(),
        ((SimpleSettingsJsonConfiguration) Configuration).JsonSerializerOptions);
      CopyValues(this, source);

      OnAfterLoad();
    }
  }
}