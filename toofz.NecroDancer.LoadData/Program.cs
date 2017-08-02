using toofz.NecroDancer.Data;
using toofz.NecroDancer.EntityFramework;

namespace toofz.NecroDancer.LoadData
{
    static class Program
    {
        static void Main(string[] args)
        {
            // TODO: This shouldn't be hardcoded.
            var uri = @"S:\Applications\Steam\steamapps\common\Crypt of the NecroDancer\data\necrodancer.xml";
            var data = NecroDancerDataSerializer.Read(uri);

            using (var db = new NecroDancerContext())
            {
                db.Set<Item>().AddRange(data.Items);
                db.Set<Enemy>().AddRange(data.Enemies);
                db.SaveChanges();
            }
        }
    }
}
