using System.Data;
using Taller.Manager;
using Taller.Models;

var Route = "D:\\Cursos\\Distribuidos\\Taller_C#\\Taller\\Taller\\Resources";

var DirectoryManager = new DirectoryManager(Route);

var FilesFound = DirectoryManager.GetFiles();

if(!FilesFound)
    return;

for (int i = 0; i < DirectoryManager.Files.Count; i++)
{
    var Item = DirectoryManager.Files[i];
    var Dataset = $"Dataset {i+1}";
    Console.WriteLine($"\n {Dataset} \n");

    try
    {
        var FrameManager = new DataFrameManager<SalesModel>(Item);
        FrameManager.LoadCsv();

        var ThreadsManager = new ThreadsManager<SalesModel>(FrameManager);
        
        var Tasks = new List<Task>()
        {
            Task.Run(ThreadsManager.CalculateSalesByRegion),
            Task.Run(ThreadsManager.CalculateSalesByRegionAndProduct)
        };

        await Task.WhenAll(Tasks);
    }
    catch (DataException e)
    {
        Console.WriteLine($"Error al cargar ({Dataset}): {e.Message}");
    }
    catch (Exception e)
    {
        Console.WriteLine($"Ocurrió un error al ejecutar los procesos: {e.Message}");
    }
}