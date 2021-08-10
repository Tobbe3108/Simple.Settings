using System.IO;
using System.Threading.Tasks;
using Simple.Settings.Helpers;
using Simple.Settings.Json.Configuration;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Simple.Settings.Json
{
  public abstract partial class Settings
  {
    public async Task LoadAsync(string path)
    {
      FileInfo = new FileInfo(path);
      if (!FileInfo.Exists || FileInfo.Length == 0)
      {
        return;
      }
      
      using var stream = new FileStream(FileInfo.FullName, FileMode.OpenOrCreate);
      await InternalLoadAsync(stream);
    }
    
    public async Task ReloadAsync()
    {
      if (!FileInfo.Exists || FileInfo.Length == 0)
      {
        return;
      }

      using var stream = new FileStream(FileInfo.FullName, FileMode.OpenOrCreate);
      await InternalLoadAsync(stream);
    }

    public async Task SaveAsync()
    {
      using var stream = new FileStream(FileInfo.FullName, FileMode.OpenOrCreate);
      await InternalSaveAsync(stream);
    }

    protected override async Task InternalLoadAsync(Stream jsonStream)
    {
      OnBeforeLoad();

      object? source = null;

      if (Configuration.EncryptionOptions is not null)
      {
        OnBeforeDecrypt();

        var (stream, s1, a2) =
          await EncryptionHelper.DecryptAsync(Configuration.EncryptionOptions.EncryptionKey, jsonStream as FileStream);
        source = await JsonSerializer.DeserializeAsync(stream, GetType(),
          ((SimpleSettingsJsonConfiguration) Configuration).JsonSerializerOptions);

        stream.Close();
        stream.Dispose();
        s1.Close();
        s1.Dispose();
        a2.Clear();
        a2.Dispose();

        OnAfterDecrypt();
      }

      source ??= await JsonSerializer.DeserializeAsync(jsonStream, GetType(),
        ((SimpleSettingsJsonConfiguration) Configuration).JsonSerializerOptions);

      await Task.Run(() => CopyValues(this, source));

      OnAfterLoad();
    }

    protected override async Task InternalSaveAsync(Stream jsonStream)
    {
      OnBeforeSave();

      if (Configuration.EncryptionOptions is not null)
      {
        OnBeforeEncrypt();
        
        var (stream,s1,a2) = await EncryptionHelper.EncryptAsync(Configuration.EncryptionOptions.EncryptionKey, jsonStream as FileStream);
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
      
      await JsonSerializer.SerializeAsync(jsonStream, this, GetType(),
        ((SimpleSettingsJsonConfiguration) Configuration).JsonSerializerOptions);
      
      OnAfterSave();
    }

    protected override async Task InternalReloadAsync(Stream jsonStream)
    {
      await InternalLoadAsync(jsonStream);
    }
  }
}