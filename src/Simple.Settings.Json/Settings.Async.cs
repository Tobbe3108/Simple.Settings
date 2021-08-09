using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
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

      string? json = null;

      if (Configuration.EncryptionOptions is not null)
      {
        OnBeforeDecrypt();

        var (stream, s1, a2) =
          await EncryptionHelper.DecryptAsync(Configuration.EncryptionOptions.EncryptionKey, jsonStream as FileStream);
        var streamReader = new StreamReader(stream);
        json = await streamReader.ReadToEndAsync();

        stream.Close();
        stream.Dispose();
        s1.Close();
        s1.Dispose();
        a2.Clear();
        a2.Dispose();
        streamReader.Close();
        streamReader.Dispose();

        OnAfterDecrypt();
      }
      
      if (json is null)
      {
        var streamReader = new StreamReader(jsonStream);
        json = await streamReader.ReadToEndAsync();
      }

      await Task.Factory.StartNew(() => JsonConvert.PopulateObject(json, this));

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