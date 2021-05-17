using System.IO;
using System.Threading.Tasks;
using Benchmark.Models;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Benchmark
{
  [MemoryDiagnoser]
  public class Benchmark
  {
    private TonsOfSettings _settings;
    private TonsOfSettings _settingsLoaded;
    private static readonly string Path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "BenchmarkSettings.json");

    [GlobalSetup]
    public async Task Setup()
    {
      _settings = new TonsOfSettings();
      _settingsLoaded = new TonsOfSettings();
      await _settingsLoaded.LoadAsync(Path);
    }

    [Benchmark]
    public void Load()
    {
      _settings.Load(Path);
    }
      
    [Benchmark]
    public async Task LoadAsync()
    {
      await _settings.LoadAsync(Path);
    }
      
    [Benchmark]
    public void Save()
    {
      _settingsLoaded.Save();
    }
      
    [Benchmark]
    public async Task SaveAsync()
    {
      await _settingsLoaded.SaveAsync();
    }
  }

  internal static class Program
  {
    private static void Main(string[] args)
    {
      BenchmarkRunner.Run<Benchmark>();
    }
  }
}