using Simple.Settings.Json;

namespace Sample.Models
{
  public class SampleSettings : Settings
  {
    public string SomeSetting { get; set; } = "Test";
  }
}