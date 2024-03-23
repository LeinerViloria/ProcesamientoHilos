using Taller.Manager;

var Route = "D:\\Cursos\\Distribuidos\\Taller_C#\\Taller\\Taller\\Resources";

var DirectoryManager = new DirectoryManager(Route);

var FilesFound = DirectoryManager.GetFiles();

if(!FilesFound)
    return;

for (int i = 0; i < DirectoryManager.Files.Count; i++)
{
    var Item = DirectoryManager.Files[i];
    Console.WriteLine($"\n Dataset {i+1}\n");

    var FrameManager = new DataFrameManager(Item);
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