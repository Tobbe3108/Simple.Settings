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

    #region Events

    #region Load
    
    public delegate void BeforeLoadHandler();

    public event BeforeLoadHandler BeforeLoad;

    protected virtual void OnBeforeLoad()
    {
      BeforeLoad?.Invoke();
    }

    public delegate void AfterLoadHandler();

    public event AfterLoadHandler AfterLoad;

    protected virtual void OnAfterLoad()
    {
      AfterLoad?.Invoke();
    }

    #endregion

    #region Save

    public delegate void BeforeSaveHandler();

    public event BeforeSaveHandler BeforeSave;

    protected virtual void OnBeforeSave()
    {
      BeforeSave?.Invoke();
    }
    
    public delegate void AfterSaveHandler();

    public event AfterSaveHandler AfterSave;

    protected virtual void OnAfterSave()
    {
      AfterSave?.Invoke();
    }

    #endregion
    
    #region Encrypt

    public delegate void BeforeEncryptHandler();

    public event BeforeEncryptHandler BeforeEncrypt;

    protected virtual void OnBeforeEncrypt()
    {
      BeforeEncrypt?.Invoke();
    }

    public delegate void AfterEncryptHandler();

    public event AfterEncryptHandler AfterEncrypt;

    protected virtual void OnAfterEncrypt()
    {
      AfterEncrypt?.Invoke();
    }

    #endregion

    #region Decrypt

    public delegate void BeforeDecryptHandler();

    public event BeforeDecryptHandler BeforeDecrypt;

    protected virtual void OnBeforeDecrypt()
    {
      BeforeDecrypt?.Invoke();
    }
    
    public delegate void AfterDecryptHandler();

    public event AfterDecryptHandler AfterDecrypt;

    protected virtual void OnAfterDecrypt()
    {
      AfterDecrypt?.Invoke();
    }

    #endregion

    #endregion
  }
}