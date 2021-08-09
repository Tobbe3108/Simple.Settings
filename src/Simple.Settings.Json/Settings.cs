using System.IO;
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
  }
}