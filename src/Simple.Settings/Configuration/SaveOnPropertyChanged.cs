namespace Simple.Settings.Configuration
{
  public class SaveOnPropertyChanged
  { 
    public SaveType Type { get; set; }
    
    public enum SaveType
    {
      Async,
      Sync
    }
  }
}