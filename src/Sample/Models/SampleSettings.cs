using System.Collections.Generic;
using Simple.Settings.Json;

namespace Sample.Models
{
  public class SampleSettings : Settings
  {
    public string SomeString { get; set; } = "Some value";
    
    public ComplexObject SomeComplexObject { get; set; } = new()
    {
      SomeBool = true
    };
    
    public List<ComplexObject> SomeComplexObjectList { get; set; } = new()
    {
      new ComplexObject
      {
        SomeBool = true
      }
    };
  }

  public class ComplexObject
  {
    public bool SomeBool { get; set; }
  }
}