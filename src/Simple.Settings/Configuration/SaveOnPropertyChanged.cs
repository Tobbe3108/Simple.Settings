namespace Simple.Settings.Configuration
{
  public class SaveOnPropertyChanged
  {
    public bool Enabled { get; set; }
    public SaveType Type { get; set; }

    public enum SaveType
    {
      Async,
      Sync
    }
  }
}