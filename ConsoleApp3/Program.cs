using System.Text;

namespace ConsoleApp3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // продолжительность жизни индивида
            int lifespan = 2000;

            //коэфф. рождаемости. Среднее количество детей у фертильной женщины
            double TFR = 2.1;
            //начальное население
            int startpop = 1000;
            //время, которое должно будет пройти
            int timespan = 5000;

            // Пул населения. Создаем массив размером с начальное кол-во. Каждый элемент - это индивид, значение - возраст
            int[] pops = new int[startpop];

            // Допуская, что каждому индивиду по 1000 лет, заполняем значениями массив.
            Array.Fill(pops, 1000);



            // "Бесполезные индивиды" - те, которые уже родили один раз детей и больше не будут
            var uselesspops = Array.Empty<int>();

            //цикл по годам.
            for (var i = 1; i <= timespan; i++)
            {
                //Кол-во фертильных особей
                var fertileCount = 0l;
                //массив фертильных особей
                var fertilepops = Array.Empty<int>();
                //добавляем каждому индивиду в общем пуле по одному году
                Parallel.ForEach(Enumerable.Range(0, pops.Length), i =>
                {
                    pops[i] += 1;
                });

                //находим фертильные особи в общем пуле
                fertilepops = pops.AsParallel().Where(p => p > 20).ToArray();
                //определяем их кол-во
                fertileCount = fertilepops.LongLength;
                //и сразу определяем их как негодные
                uselesspops = uselesspops.ToList().Concat(fertilepops).ToArray();
                //опеределяем прирост от этих фертильных особей
                var bornPop = (int)((fertileCount / 2) * TFR);

                //for (var j = 0; j < bornPop; j++) 
                //{ 
                //    newBornPops.Add( 1 ); 
                //} 
                //помещаем их в массив и указываем возраст в 1 год
                var newBornPops = new int[bornPop];
                Array.Fill(newBornPops, 1);

                //добавляем их в общий пул
                pops = pops.ToList().Concat(newBornPops).ToArray();
                //чистим общий пул от всех, кто старше 21-года (они все равно уже в "бесполезных")
                pops = pops.AsParallel().Where(p => p <= 20).ToArray();
                //каждые 100 лет чистим пул бесполезных от мертвецов, это больше для экономии памяти
                if (i % 100 == 0)
                {
                    uselesspops = pops.AsParallel().Where(p => p < lifespan).ToArray();
                }


            }
            //объеденяем оба пула
            pops = pops.ToList().Concat(uselesspops).ToArray();
            //выводим результат в консоль
            Console.WriteLine($"Totalpops: {pops.Length}");
            //и пищем в файл
            var file = File.Create("result.txt");
            file.WriteAsync(ASCIIEncoding.UTF8.GetBytes(pops.Length.ToString()));
            file.Close();

        }
    }
}
