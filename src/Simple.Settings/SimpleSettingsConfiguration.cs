// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
namespace Simple.Settings
{
  public class SimpleSettingsConfiguration
  {
    public SaveOnPropertyChanged? SaveOnPropertyChanged { get; set; }
  }
  
  public class SaveOnPropertyChanged
  {
    public SaveOnPropertyChanged(bool enabled = false, SaveType type = SaveType.Sync)
    {
      Enabled = enabled;
      Type = type;
    }

    public bool Enabled { get; set; }
    public SaveType Type { get; set; }

    public enum SaveType
    {
      Async,
      Sync
    }
  }
}