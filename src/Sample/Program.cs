using System.IO;
using System.Threading.Tasks;
using Sample.Models;

namespace Sample
{
  class Program
  {
    static async Task Main(string[] args)
    {
      var path = Path.Combine(Directory.GetCurrentDirectory(), "Test.json");
      
      var setting = new SampleSettings();

      setting.Load(path);

      setting.SomeSetting = "1";
      
      setting.Save();

      await setting.LoadAsync(path);

      setting.SomeSetting = "TestEdited";
      
      await setting.SaveAsync();
    }
  }
}