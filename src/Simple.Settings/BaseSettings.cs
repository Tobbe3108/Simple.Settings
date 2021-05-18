using System.IO;
using System.Threading.Tasks;

namespace Simple.Settings
{
  public abstract class BaseSettings
  {
    public abstract FileInfo FileInfo { get; protected set; }
    public abstract void Load(string path);

    public abstract Task LoadAsync(string path);

    public abstract void Save();

    public abstract Task SaveAsync();
  }
}