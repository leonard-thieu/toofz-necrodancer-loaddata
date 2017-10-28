using System;
using System.IO;
using toofz.NecroDancer.Data;

namespace toofz.NecroDancer.LoadData
{
    internal static class Program
    {
        private static void Main(string[] args)
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
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.Database.ExecuteSqlCommand("DELETE FROM [dbo].[Items];");
                    db.Items.AddRange(data.Items);

                    db.Database.ExecuteSqlCommand("DELETE FROM [dbo].[Enemies];");
                    db.Enemies.AddRange(data.Enemies);

                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
