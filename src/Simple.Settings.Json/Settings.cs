using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Simple.Settings.Json.Configuration;

namespace Simple.Settings.Json
{
  public abstract partial class Settings : BaseSettings
  {
    protected internal FileInfo FileInfo = null!;
    
    protected Settings()
    {
      Configuration = new SimpleSettingsJsonConfiguration();
    }
    
    private static void CopyValues<T>(T target, T source)
    {
      var properties = target?.GetType().GetProperties(
        BindingFlags.Instance | BindingFlags.Public).Where(prop =>
        prop.CanRead
        && prop.CanWrite
        && prop.GetIndexParameters().Length == 0
      && prop.CustomAttributes.All(data => data.AttributeType != typeof(JsonIgnoreAttribute)));
      
      foreach (var prop in properties ?? Array.Empty<PropertyInfo>())
      {
        var value = prop.GetValue(source, null);
        if (value is null) continue;
        prop.SetValue(target, value, null);
      }
    }
  }
}