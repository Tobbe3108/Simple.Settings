using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Simple.Settings.Annotations;
using Simple.Settings.Configuration;

// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global

namespace Simple.Settings
{
  public abstract class BaseSettings : INotifyPropertyChanged
  {
    #region Fields
    
    protected internal FileInfo FileInfo = null!;
    public ISimpleSettingsConfiguration Configuration = null!;
    
    #endregion
    
    #region Methods
    
    public abstract void Load(string path);
    public abstract Task LoadAsync(string path);
    public abstract void Save();
    public abstract Task SaveAsync();
    
    #endregion

    #region PropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected async Task OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
      if (Configuration?.SaveOnPropertyChanged is not null)
      {
        switch (Configuration.SaveOnPropertyChanged.Type)
        {
          case SaveOnPropertyChanged.SaveType.Async:
            await SaveAsync();
            break;
          case SaveOnPropertyChanged.SaveType.Sync:
            // ReSharper disable once MethodHasAsyncOverload
            Save();
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
  }
}