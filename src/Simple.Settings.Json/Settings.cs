using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Simple.Settings.Json
{
  public abstract class Settings : BaseSettings
  {
    [JsonIgnore] public override FileInfo FileInfo { get; protected set; }

    public override void Load(string path, bool createIfNotExisting = true)
    {
      FileInfo = new FileInfo(path);
      var json = File.ReadAllText(FileInfo.Name);
      var source = JsonSerializer.Deserialize(json, GetType());
      CopyValues(this, source);
    }

    public override async Task LoadAsync(string path, bool createIfNotExisting = true)
    {
      FileInfo = new FileInfo(path);
      using var stream = File.OpenRead(FileInfo.Name);
      var source = await JsonSerializer.DeserializeAsync(stream, GetType());
      await Task.Run(() => CopyValues(this, source));
    }

    public override void Save()
    {
      var json = JsonSerializer.Serialize(this, GetType());
      File.WriteAllText(FileInfo.Name, json);
    }

    public override async Task SaveAsync()
    {
      using var stream = new MemoryStream();
      await JsonSerializer.SerializeAsync(stream, this, GetType());
      stream.Position = 0;
      using var reader = new StreamReader(stream);
      var json = await reader.ReadToEndAsync();
      
      var encodedText = Encoding.Unicode.GetBytes(json);
      using var fileStream = File.OpenWrite(FileInfo.Name);
      await fileStream.WriteAsync(encodedText, 0, encodedText.Length);
      
    }
    
    private static void CopyValues<T>(T target, T source)
    {
      var properties = target.GetType().GetProperties(
        BindingFlags.Instance | BindingFlags.Public).Where(prop => 
        prop.CanRead 
        && prop.CanWrite 
        && prop.GetIndexParameters().Length == 0);

      foreach (var prop in properties)
      {
        if (prop.PropertyType == typeof(FileInfo))
        {
          continue;
        }
        var value = prop.GetValue(source, null);
        prop.SetValue(target, value, null);
      }
    }
  }
}