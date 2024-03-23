
using System.Data;
using System.Diagnostics;
using Microsoft.Data.Analysis;

namespace Taller.Manager
{
    public class DataFrameManager<T> (string Path)
    {

        public DataFrame Frame { get; set; } = null!;

        public void LoadCsv()
        {
            var Watch = new Stopwatch();
            Watch.Start();

            Frame = DataFrame.LoadCsv(Path);

            Console.WriteLine($"Ya se cargó el dataframe: {Watch.Elapsed}");

            var ModelProperties = typeof(T).GetProperties()
                .Select(x => x.Name);

            var FrameIsValid = Frame.Columns.All(x => ModelProperties.Contains(x.Name));
            
            if(!FrameIsValid)
                throw new DataException("La estructura del fichero no es la esperada");
        }
    }
}
