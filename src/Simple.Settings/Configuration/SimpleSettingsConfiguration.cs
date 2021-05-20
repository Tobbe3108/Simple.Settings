namespace Simple.Settings.Configuration
{
  public interface ISimpleSettingsConfiguration
  {
    public SaveOnPropertyChanged? SaveOnPropertyChanged { get; set; }
    public EncryptionOptions? EncryptionOptions { get; set; }
  }
}