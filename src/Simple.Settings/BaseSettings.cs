using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Simple.Settings.Annotations;
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global

namespace Simple.Settings
{
  public abstract class BaseSettings : INotifyPropertyChanged
  {
    protected internal FileInfo FileInfo = null!;
    protected internal SimpleSettingsConfiguration? Configuration;
    public abstract void Load(string path);
    public abstract Task LoadAsync(string path);
    public abstract void Save();
    public abstract Task SaveAsync();
    public event PropertyChangedEventHandler? PropertyChanged;
    
    [NotifyPropertyChangedInvocator]
    protected async Task OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
      if (Configuration?.SaveOnPropertyChanged is {Enabled: true})
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
  }
}