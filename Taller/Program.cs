using System.Text.Json;
using Microsoft.Data.Analysis;
using Taller.Manager;

var dataPath = "D:\\Cursos\\Distribuidos\\Taller_C#\\Taller\\Taller\\Resources\\Sales_Data_1.csv";

var dataFrame = DataFrame.LoadCsv(dataPath);

var Regions = dataFrame.GroupBy<string>("Region")
   .Groupings
   .AsParallel()
   .Select(x => new
   {
       Region = x.Key,
       Total = x.Select(row => (float) row["Sales"])
           .Sum()
   });

var Result = JsonSerializer.Serialize(Regions);

var SourceList = new Dictionary<string, string>()
{
  {"DATASET 1", "D:\\Cursos\\Distribuidos\\Taller_C#\\Taller\\Taller\\Resources\\Sales_Data_1.csv"}  ,
  {"DATASET 2", "D:\\Cursos\\Distribuidos\\Taller_C#\\Taller\\Taller\\Resources\\Sales_Data_2.csv"},
  {"DATASET 3", "D:\\Cursos\\Distribuidos\\Taller_C#\\Taller\\Taller\\Resources\\Sales_Data_3.csv"},
  {"DATASET 4", "D:\\Cursos\\Distribuidos\\Taller_C#\\Taller\\Taller\\Resources\\Sales_Data_4.csv"}
};

foreach (var Item in SourceList)
{
    Console.WriteLine($"\n{Item.Key}\n");

    var FrameManager = new DataFrameManager(Item.Value);
    FrameManager.LoadCsv();

    var ThreadsManager = new ThreadsManager(FrameManager);

    try
    {
        var Tasks = new List<Task>()
        {
            Task.Run(ThreadsManager.CalculateSalesByRegion),
            Task.Run(ThreadsManager.CalculateSalesByRegionAndProduct)
        };

        await Task.WhenAll(Tasks);
    }
    catch (Exception e)
    {
        Console.WriteLine($"Ocurrió un error al ejecutar los procesos: {e.Message}");
    }   
}