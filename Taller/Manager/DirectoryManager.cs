
namespace Taller.Manager;

public class DirectoryManager(string Route)
{
    public List<string> Files {get; set;} = null!;
    public bool GetFiles()
    {
        var Exists = Directory.Exists(Route);

        if(!Exists)
        {
            Console.WriteLine("Ruta no encontrada");
            return false;
        }

        Files = Directory.GetFiles(Route)
            .Where(x => x.EndsWith(".csv"))
            .ToList();

        if(Files.Count == 0)
        {
            Console.WriteLine("Archivos no encontrados");
            return false;
        }

        return true;
    }

    public static void CreateResponse(string Route, string Name, dynamic Value)
    {
        const string FolderName = "Datasets";
        var FolderRoute = AppDomain.CurrentDomain.BaseDirectory;

        FolderRoute = Path.Combine(FolderRoute, FolderName);

        if(!Directory.Exists(FolderRoute))
            Directory.CreateDirectory(FolderRoute);

        Route = Route.Replace(".csv", "");
        Route = Route.Split("\\").Last();

        FolderRoute = Path.Combine(FolderRoute, Route);

        if(!Directory.Exists(FolderRoute))
            Directory.CreateDirectory(FolderRoute);

        
    }
}