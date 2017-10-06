using System;
using System.IO;
using System.Threading.Tasks;
using toofz.NecroDancer.Data;

namespace toofz.NecroDancer.LoadData
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length != 1)
                throw new ArgumentException("Invalid number of arguments.");

            var serializer = new NecroDancerDataSerializer();
            NecroDancerData data;
            var path = args[0];
            using (var fs = File.OpenRead(path))
            {
                data = serializer.Deserialize(fs);
            }

            using (var db = new NecroDancerContext())
            {
                await db.Database.ExecuteSqlCommandAsync("DELETE FROM [dbo].[Items];").ConfigureAwait(false);
                await db.Database.ExecuteSqlCommandAsync("DELETE FROM [dbo].[Enemies];").ConfigureAwait(false);

                db.Items.AddRange(data.Items);
                db.Enemies.AddRange(data.Enemies);
                await db.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
