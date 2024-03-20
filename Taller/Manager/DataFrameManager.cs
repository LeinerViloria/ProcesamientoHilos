
using System.Diagnostics;
using Microsoft.Data.Analysis;

namespace Taller.Manager
{
    public class DataFrameManager (string Path)
    {

        public DataFrame Frame { get; set; } = null!;

        public void LoadCsv()
        {
            var Watch = new Stopwatch();
            Watch.Start();

            Frame = DataFrame.LoadCsv(Path);

            Console.WriteLine($"Ya se cargó el dataframe: {Watch.Elapsed}");
        }
    }
}
