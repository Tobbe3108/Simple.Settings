using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sample.Models;
using Simple.Settings.Configuration;
using Simple.Settings.Json;
using Simple.Settings.Json.Configuration;

namespace Sample
{
  class Program
  {
    private static readonly string Path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "SampleSettings.json");

    static async Task Main(string[] args)
    {
       // var setting = new SampleSettings().WithConfiguration(configuration =>
       // {
       //   configuration.JsonSerializerSettings = new JsonSerializerSettings()
       //   {
       //      Formatting = Formatting.Indented,
       //      DefaultValueHandling = DefaultValueHandling.Populate,
       //      ContractResolver = new ShouldSerializeContractResolver(),
       //      ObjectCreationHandling = ObjectCreationHandling.Replace
       //   };
       //   configuration.EncryptionOptions = new EncryptionOptions
       //   {
       //     EncryptionKey = "StrongEncryptionKey"
       //   };
       // });

      var setting = new SampleSettings();
      
      #region Events

      setting.BeforeLoad += () =>
      {
        Console.WriteLine("before load");
      };

      setting.BeforeEncrypt += () =>
      {
        Console.WriteLine("before encrypt");
      };

      setting.BeforeDecrypt += () =>
      {
        Console.WriteLine("before decrypt");
      };

      setting.BeforeSave += () =>
      {
        Console.WriteLine("before save");
      };

      setting.AfterLoad += () =>
      {
        Console.WriteLine("after load");
      };

      setting.AfterEncrypt += () =>
      {
        Console.WriteLine("after encrypt");
      };

      setting.AfterDecrypt += () =>
      {
        Console.WriteLine("after decrypt");
      };

      setting.AfterSave += () =>
      {
        Console.WriteLine("after save");
      };

      #endregion

      // Load settings
      // setting.Load(Path);
      await setting.LoadAsync(Path);

      // Read settings
      var value = setting.SomeString;
      var complexValue = setting.SomeComplexObject;
      var list = setting.SomeComplexObjectList;

      
      //Change settings
      setting.SomeString = "Some other value";
      
      //Reload settings
      // setting.Reload();
      await setting.ReloadAsync();
      
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
      // setting.Save();
      await setting.SaveAsync();
    }
  }
}