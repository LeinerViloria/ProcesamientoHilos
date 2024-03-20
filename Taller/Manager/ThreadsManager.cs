
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.Json;

namespace Taller.Manager;

public class ThreadsManager
{
    private ConcurrentDictionary<string, float> SalesByRegion { get; set; } = new();
    private ConcurrentDictionary<string, float> SalesByRegionAndProduct { get; set; } = new();
    private readonly DataFrameManager FrameManager;

    public ThreadsManager(DataFrameManager FrameManager)
    {
        this.FrameManager = FrameManager;

        Console.WriteLine($"Nucleos disponibles: {Environment.ProcessorCount}");
        Console.WriteLine($"Filas para procesar: {FrameManager.Frame.Rows.Count}\n");
    }

    private static ParallelOptions GetParallelOptions() => new(){
        MaxDegreeOfParallelism = Environment.ProcessorCount
    };

    public void CalculateSalesByRegion()
    {
        Console.WriteLine("Se están calculando las ventas por región");

        var Watch = new Stopwatch();
        Watch.Start();

        Parallel.ForEach(FrameManager.Frame.Rows, GetParallelOptions(), (Item) =>
        {
            var Region = (string)Item["Region"];
            var Sale = (float)Item["Sales"];

            SalesByRegion.AddOrUpdate(Region, Sale, (_, value) => value + Sale);
        });

        Watch.Stop();

        Console.WriteLine($"\nSe terminó de calcular las ventas por región: ({Watch.Elapsed})");
        var Result = JsonSerializer.Serialize(SalesByRegion);
        Console.WriteLine($"{Result}\n");
    }

    public void CalculateSalesByRegionAndProduct()
    {
        Console.WriteLine($"Se están calculando las ventas por región y producto");
        
        Parallel.ForEach(FrameManager.Frame.Rows, GetParallelOptions(), (Item)=>
        {
            
        });
    }
}