using System.IO;
using System.Threading.Tasks;
using System.Text.Json;

namespace Simple.Settings.Json
{
  public abstract partial class Settings
  {
    public override async Task LoadAsync(string path)
    {
      FileInfo = new FileInfo(path);
      if (FileInfo.Length == 0)
      {
        return;
      }
      
      using var stream = new FileStream(FileInfo.Name, FileMode.OpenOrCreate);
      var source = await JsonSerializer.DeserializeAsync(stream, GetType());
      await Task.Run(() => CopyValues(this, source));
    }
    
    public override async Task SaveAsync()
    {
      using var fileStream = new FileStream(FileInfo.Name, FileMode.OpenOrCreate);
      await JsonSerializer.SerializeAsync(fileStream, this, GetType(), JsonSerializerOptions);
    }
  }
}