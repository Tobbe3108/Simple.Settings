using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Sample.Models;

namespace Sample
{
  class Program
  {
    private static readonly string Path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "SampleSettings.json");

    static async Task Main(string[] args)
    {
      var setting = new SampleSettings();
      
      // Load settings
      setting.Load(Path);
      await setting.LoadAsync(Path);
      
      // Read settings
      var value = setting.SomeString;
      var complexValue = setting.SomeComplexObject;
      var list = setting.SomeComplexObjectList;
      
      //Change settings
      setting.SomeString = "Some other value";
      setting.SomeComplexObject = new ComplexObject
      {
        SomeBool = false
      };
      setting.SomeComplexObjectList = new List<ComplexObject>
      {
        new()
        {
          SomeBool = false
        }
      };
      
      // Save settings
      setting.Save();
      await setting.SaveAsync();
    }
  }
}