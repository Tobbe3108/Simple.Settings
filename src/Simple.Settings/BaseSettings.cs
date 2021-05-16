using System.IO;
using System.Threading.Tasks;

namespace Simple.Settings
{
  public abstract class BaseSettings
  {
    public abstract FileInfo FileInfo { get; protected set; }
    public abstract void Load(string path, bool createIfNotExisting = true);

    public abstract Task LoadAsync(string path, bool createIfNotExisting = true);

    public abstract void Save();

    public abstract Task SaveAsync();
  }
}