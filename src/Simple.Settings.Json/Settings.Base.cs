using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Simple.Settings.Json
{
  public abstract partial class Settings : BaseSettings
  {
    [JsonIgnore] public override FileInfo FileInfo { get; protected set; }

    [JsonIgnore] private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
    {
      WriteIndented = true
    };
    
    private static void CopyValues<T>(T target, T source)
    {
      var properties = target.GetType().GetProperties(
        BindingFlags.Instance | BindingFlags.Public).Where(prop =>
        prop.CanRead
        && prop.CanWrite
        && prop.GetIndexParameters().Length == 0);

      foreach (var prop in properties)
      {
        if (prop.Name == nameof(FileInfo))
        {
          continue;
        }

        var value = prop.GetValue(source, null);
        prop.SetValue(target, value, null);
      }
    }
  }
}