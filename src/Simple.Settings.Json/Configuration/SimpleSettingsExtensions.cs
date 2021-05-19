using System;

namespace Simple.Settings.Json.Configuration
{
  public static class SimpleSettingsExtensions
  {
    public static T WithConfiguration<T>(this T baseSettings, Action<SimpleSettingsJsonConfiguration>? configurationAction = null) where T : BaseSettings
    {
      var configuration = new SimpleSettingsJsonConfiguration();
      configurationAction?.Invoke(configuration);
      baseSettings.Configuration = configuration;
      return baseSettings;
    }
  }
}