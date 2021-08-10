using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Simple.Settings.Helpers;
using Simple.Settings.Json.Configuration;

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

      string json = string.Empty;

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

      if (string.IsNullOrEmpty(json))
      {
        var streamReader = new StreamReader(jsonStream);

        json = await streamReader.ReadToEndAsync();

        streamReader.Close();
        streamReader.Dispose();
      }
      
      await Task.Run(()=> JsonConvert.PopulateObject(json, this, ((SimpleSettingsJsonConfiguration) Configuration).JsonSerializerSettings));

      OnAfterLoad();
    }

    protected override async Task InternalSaveAsync(FileStream fileStream)
    {
      OnBeforeSave();

      var json = JsonConvert.SerializeObject(this, GetType(),
        ((SimpleSettingsJsonConfiguration) Configuration).JsonSerializerSettings);

      if (Configuration.EncryptionOptions is not null)
      {
        OnBeforeEncrypt();


        var (stream, s1, a2) =
          await EncryptionHelper.EncryptAsync(Configuration.EncryptionOptions.EncryptionKey,
            GenerateStreamFromString(json));

        await stream.CopyToAsync(fileStream);

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

      var streamWriter = new StreamWriter(fileStream);
      await streamWriter.WriteAsync(json);
      streamWriter.Close();
      streamWriter.Dispose();

      OnAfterSave();
    }

    protected override async Task InternalReloadAsync(Stream jsonStream)
    {
      await InternalLoadAsync(jsonStream);
    }

    private static MemoryStream GenerateStreamFromString(string? s)
    {
      var stream = new MemoryStream();
      var writer = new StreamWriter(stream);
      writer.Write(s);
      writer.Flush();
      stream.Position = 0;
      return stream;
    }
  }
}