// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace Simple.Settings.Configuration
{
  public interface ISimpleSettingsConfiguration
  {
    public SaveOnPropertyChanged SaveOnPropertyChanged { get; set; }
    public object EncryptionOptions { get; set; }
  }
}