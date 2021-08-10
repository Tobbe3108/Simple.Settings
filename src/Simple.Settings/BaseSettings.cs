using System.IO;
using System.Threading.Tasks;
using Simple.Settings.Configuration;

// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global

namespace Simple.Settings
{
  public abstract class BaseSettings
  {
    #region Fields
    
    public ISimpleSettingsConfiguration Configuration = null!;
    
    #endregion
    
    #region Methods

    protected abstract void InternalLoad(string path);
    protected abstract Task InternalLoadAsync(Stream jsonStream);
    protected abstract void InternalSave(string path);
    protected abstract Task InternalSaveAsync(FileStream fileStream);
    protected abstract void InternalReload(string path);
    protected abstract Task InternalReloadAsync(Stream jsonStream);
    
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
    
    #region Reload

    public delegate void BeforeReloadHandler();

    public event BeforeReloadHandler BeforeReload;

    protected virtual void OnBeforeReload()
    {
      BeforeReload?.Invoke();
    }
    
    public delegate void AfterReloadHandler();

    public event AfterReloadHandler AfterReload;

    protected virtual void OnAfterReload()
    {
      AfterReload?.Invoke();
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