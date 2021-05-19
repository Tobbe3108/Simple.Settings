using System;

namespace Simple.Settings
{
  public static class SimpleSettingsExtensions
  {
    public static T WithConfiguration<T>(this T obj, Action<SimpleSettingsConfiguration>? configuration = null) where T : BaseSettings
    {
      var temp = new SimpleSettingsConfiguration();
      configuration?.Invoke(temp);
      obj.Configuration = temp;
      return obj;
    }
  }
}