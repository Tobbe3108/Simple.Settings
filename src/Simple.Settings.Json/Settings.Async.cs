using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using Simple.Settings.Helpers;
using Simple.Settings.Json.Configuration;

namespace Simple.Settings.Json
{
  public abstract partial class Settings
  {
    public override async Task LoadAsync(string path)
    {
      OnBeforeLoad();
      
      object? source = null;

      FileInfo = new FileInfo(path);
      if (!FileInfo.Exists || FileInfo.Length == 0)
      {
        return;
      }

      if (Configuration.EncryptionOptions is not null)
      {
        OnBeforeDecrypt();
        
        var (stream,s1,a2) = await EncryptionHelper.DecryptAsync(Configuration.EncryptionOptions.EncryptionKey, FileInfo);
        source = await JsonSerializer.DeserializeAsync(stream , GetType(), ((SimpleSettingsJsonConfiguration)Configuration).JsonSerializerOptions);

        stream.Close();
        stream.Dispose();
        s1.Close();
        s1.Dispose();
        a2.Clear();
        a2.Dispose();
        
        OnAfterDecrypt();
      }

      if (source is null)
      {
        using var stream = new FileStream(FileInfo.Name, FileMode.OpenOrCreate);
        source = await JsonSerializer.DeserializeAsync(stream, GetType(), ((SimpleSettingsJsonConfiguration)Configuration).JsonSerializerOptions);
      }

      await Task.Run(() => CopyValues(this, source));
      
      OnAfterLoad();
    }

    public override async Task SaveAsync()
    {
      OnBeforeSave();
      
      if (Configuration.EncryptionOptions is not null)
      {
        OnBeforeEncrypt();
        
        var (stream,s1,a2) = await EncryptionHelper.EncryptAsync(Configuration.EncryptionOptions.EncryptionKey, FileInfo);
        await JsonSerializer.SerializeAsync(stream, this, GetType(),
          ((SimpleSettingsJsonConfiguration) Configuration).JsonSerializerOptions);
        
        stream.Close();
        stream.Dispose();
        s1.Close();
        s1.Dispose();
        a2.Clear();
        a2.Dispose();
        
        OnAfterEncrypt();
        OnAfterSave();
        
        return;
      }
      
      using var fileStream = new FileStream(FileInfo.Name, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);
      await JsonSerializer.SerializeAsync(fileStream, this, GetType(),
        ((SimpleSettingsJsonConfiguration) Configuration).JsonSerializerOptions);
      
      OnAfterSave();
    }
  }
}