using System.Text;

namespace ConsoleApp3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int lifespan = 2000;
            double TFR = 2.1;
            int startpop = 1000;
            int timespan = 5000;

            int[] pops = new int[startpop];

            Array.Fill(pops, 1000);




            var uselesspops = Array.Empty<int>();

            for (var i = 1; i <= timespan; i++)
            {
                var fertileCount = 0l;

                var fertilepops = Array.Empty<int>();

                Parallel.ForEach(Enumerable.Range(0, pops.Length), i =>
                {
                    pops[i] += 1;
                });

                fertilepops = pops.AsParallel().Where(p => p > 20).ToArray();

                fertileCount = fertilepops.LongLength;

                uselesspops = uselesspops.ToList().Concat(fertilepops).ToArray();

                var bornPop = (int)((fertileCount / 2) * TFR);

                //for (var j = 0; j < bornPop; j++) 
                //{ 
                //    newBornPops.Add( 1 ); 
                //} 
                var newBornPops = new int[bornPop];
                Array.Fill(newBornPops, 1);

                pops = pops.ToList().Concat(newBornPops).ToArray();

                pops = pops.AsParallel().Where(p => p <= 20).ToArray();

                if (i % 100 == 0)
                {
                    uselesspops = pops.AsParallel().Where(p => p < lifespan).ToArray();
                }


            }

            pops = pops.ToList().Concat(uselesspops).ToArray();

            Console.WriteLine($"Totalpops: {pops.Length}");

            var file = File.Create("result.txt");
            file.WriteAsync(ASCIIEncoding.UTF8.GetBytes(pops.Length.ToString()));
            file.Close();

        }
    }
}
