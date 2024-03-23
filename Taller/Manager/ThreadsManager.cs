
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.Json;
using Newtonsoft.Json;

namespace Taller.Manager;

public class ThreadsManager<T>
{
    private ConcurrentDictionary<string, float> SalesByRegion { get; set; } = new();
    private ConcurrentDictionary<string, float> SalesByRegionAndProduct { get; set; } = new();
    private readonly DataFrameManager<T> FrameManager;

    public ThreadsManager(DataFrameManager<T> FrameManager)
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

        var SalesOrdered = SalesByRegion.OrderBy(x => x.Key)
            .ToDictionary(x => x.Key, x => x.Value);

        var Result = JsonConvert.SerializeObject(SalesOrdered, Formatting.Indented);
        Console.WriteLine($"\nSe terminó de calcular las ventas por región: ({Watch.Elapsed})\n{Result}\n");
    }

    public void CalculateSalesByRegionAndProduct()
    {
        Console.WriteLine($"Se están calculando las ventas por región y producto");

        var Watch = new Stopwatch();
        Watch.Start();
        
        Parallel.ForEach(FrameManager.Frame.Rows, GetParallelOptions(), (Item)=>
        {
            var Region = (string)Item["Region"];
            var Product = (string)Item["Product"];
            var Sale = (float)Item["Sales"];

            var Key = $"{Region} y {Product}";

            SalesByRegionAndProduct.AddOrUpdate(Key, Sale, (_, value) => value + Sale);
        });

        Watch.Stop();

        var SalesOrdered = SalesByRegionAndProduct.OrderBy(x => x.Key)
            .ToDictionary(x => x.Key, x => x.Value);

        var Result = JsonConvert.SerializeObject(SalesOrdered, Formatting.Indented);
        Console.WriteLine($"\nSe terminó de calcular las ventas por región y producto: ({Watch.Elapsed})\n{Result}\n");
    }
}