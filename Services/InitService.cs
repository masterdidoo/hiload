using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using hiload.MemDb;
using hiload.Model;
using Newtonsoft.Json;

namespace hiload.Services
{
    public class InitService
    {
        private readonly HiloadContext _context;

        public InitService(HiloadContext context)
        {
            _context = context;
            Visit.context = context;
        }

        public void LoadData(string fileName)
        {
            Console.WriteLine("PC {0}", Environment.ProcessorCount);
            
            var file = Path.Combine(fileName, "options.txt");
            if (File.Exists(file)) LoadNowDate(File.Open(file, FileMode.Open, FileAccess.Read));

            LoadZip(Path.Combine(fileName, "data.zip"));

            var oldMem = GC.GetTotalMemory(false);

            var clock = Stopwatch.StartNew();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);
            GC.WaitForPendingFinalizers();
            clock.Stop();
            Console.WriteLine("GC {0}",clock.Elapsed);
            Console.WriteLine("Cleaned: {0:N0}", oldMem - GC.GetTotalMemory(false));
        }

        public void LoadZip(string fileName)
        {
            var countUsers = 0;
            var countLocations = 0;
            var countVisits = 0;

            var clock = Stopwatch.StartNew();
            using(var zf = ZipFile.OpenRead(fileName))
            {
                countUsers = LoadJson<User>(zf, "users");
                countLocations = LoadJson<Location>(zf, "locations");
                countVisits = LoadJson<Visit>(zf, "visits");
            }
            Console.WriteLine("Users     {0}", countUsers);
            Console.WriteLine("Locations {0}", countLocations);
            Console.WriteLine("Visits    {0}", countVisits);
            
            clock.Stop();
            Console.WriteLine(clock.Elapsed);
        }

        private int LoadJson<T>(ZipArchive zf, string startName) where T:class, IEntity, new()
        {
            int count = 0;
            var visits = zf.Entries.Where(x=>
                x.Name.EndsWith(".json") && x.Name.StartsWith(startName));
            // Parallel.ForEach(ToMemLoader(visits), ms=>
            foreach(var stream in visits.Select(zip=>zip.Open()))
            {
                using(stream)
                {
                    using(var streamReader = new StreamReader(stream))
                    {
                        Interlocked.Add(ref count, Read<T>(streamReader));
                    }
                }
            }
            // });

            return count;
        }

        private void LoadNowDate(Stream stream)
        {
            using(var streamReader = new StreamReader(stream))
            {
                var now = streamReader.ReadLine();
                User.Now = int.Parse(now);
                User.NowDate = new DateTime(1970,1,1).AddSeconds(User.Now);

                Console.WriteLine(User.Now);
                Console.WriteLine(User.NowDate);
            }
        }

        private int Read<T>(StreamReader streamReader) where T:class, IEntity, new()
        {
            var count = 0;
            var rep = _context.Set<T>();

            using(var js = new JsonTextReader(streamReader))
            {
                js.SupportMultipleContent = true;

                while (js.Read() && js.TokenType != JsonToken.StartArray);

                var serializer = new JsonSerializer();
                while (js.Read() && js.TokenType != JsonToken.EndArray)
                {
                    var value = serializer.Deserialize<T>(js);

                    rep.Add(value);

                    count++;
                }
            }
            return count;
        }

        private IEnumerable<MemoryStream> ToMemLoader(IEnumerable<ZipArchiveEntry> jsons)
        {
            foreach (var zipItem in jsons)
            {
                var ms = new MemoryStream();
                {
                    using (var stream = zipItem.Open())
                    {
                        stream.CopyTo(ms);
                    }

                    ms.Seek(0, SeekOrigin.Begin);

                    yield return ms;
                }
            }
        }
    }
}
