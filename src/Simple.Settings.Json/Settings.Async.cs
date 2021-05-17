using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Simple.Settings.Json
{
  public abstract partial class Settings
  {
    public override async Task LoadAsync(string path, bool createIfNotExisting = true)
    {
      FileInfo = new FileInfo(path);

      await CreateOrThrowAsync(createIfNotExisting);

      using var stream = File.OpenRead(FileInfo.Name);
      if (stream.Length == 0)
      {
        return;
      }
      
      var source = await JsonSerializer.DeserializeAsync(stream, GetType());
      await Task.Run(() => CopyValues(this, source));
    }
    
    public override async Task SaveAsync(bool createIfNotExisting = true)
    {
      await CreateOrThrowAsync(createIfNotExisting);
      
      using var stream = new MemoryStream();
      await JsonSerializer.SerializeAsync(stream, this, GetType(), JsonSerializerOptions);
      stream.Position = 0;
      using var reader = new StreamReader(stream);
      var json = await reader.ReadToEndAsync();

      var encodedText = Encoding.Unicode.GetBytes(json);
      using var fileStream = File.OpenWrite(FileInfo.Name);
      await fileStream.WriteAsync(encodedText, 0, encodedText.Length);
    }
    
    private async Task CreateOrThrowAsync(bool createIfNotExisting)
    {
      switch (FileInfo.Exists)
      {
        case false when createIfNotExisting:
        {
          using var fileStream = File.OpenWrite(FileInfo.Name);
          await fileStream.WriteAsync(new byte[0], 0, 0);
          break;
        }
        case false:
          throw new FileNotFoundException();
      }
    }
  }
}