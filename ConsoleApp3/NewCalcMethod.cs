namespace ConsoleApp3
{
    public class NewCalcMethod
    {
        //Срок жизни
        public int Lifespan { set; get; }
        //Коэфф. рождаемости
        public double TFR { set; get; }
        //Стартовое кол-во населения
        public ulong Startpopulation { set; get; }
        //Возраст фертильности. Для упрощения, считаем, что при его наступлении сразу же рождается новое поколение
        public int AgeOfFertility { set; get; }
        //время
        public int Timespan { set; get; }
        //В той коллекции мы будем хранить кол-во населения для каждого года
        public Dictionary<int, ulong> PopsCountPerYear = new();
        //массив с разспределением по возрастам
        private ulong[] agebracket = Array.Empty<ulong>();

        /// <summary>
        /// Расчитывает размер населени согласно параметрам класса.
        /// </summary>
        /// <param name="writeToCSV">Экспорт динамик роста населения в файл. true по умолчанию</param>
        /// <returns></returns>
        public ulong DoCalc(bool writeToCSV = true)
        {
            //создаем массив, каждый элемент - значение возраста.
            agebracket = new ulong[Lifespan];

            //считаем, что начальное население имеет средний возраст, равный в половину предельного, и внсим их кол-во в массив
            agebracket[Lifespan / 2 - 1] = (ulong)Startpopulation;

            //DoOnce. Это стартовое население успешно расплодилось. Добавляем его в массив.
            agebracket[0] = GetNewbornNumber(Startpopulation);

            //выполняем цикл прокрутки времени

            for (var i = 1; i <= Timespan; i++)
            {
                //этим циклом мы выполняем старение населения на 1 год, перемещаяя его по массиву.
                for (var j = agebracket.Length - 2; j >= 0; j--)
                {
                    agebracket[j + 1] = agebracket[j];
                }
                //находим кол-во населения, родившегося у пар, достигших фертильного возраста.
                agebracket[0] = GetNewbornNumber(agebracket[AgeOfFertility - 1]);

                PopsCountPerYear.Add(i, CalcTotalPops(agebracket));
            }

            if (writeToCSV)
                WriteResultsToFile();

            return PopsCountPerYear.Last().Value;
        }


        /// <summary>
        /// Метод расчета прибавки населения.
        /// </summary>
        /// <param name="pop">Количество фертильного населения</param>
        /// <returns></returns>
        private ulong GetNewbornNumber(ulong pop)
        {
            return (ulong)((pop / 2) * TFR);
        }

        /// <summary>
        /// Подсчет общего кол-ва насления в массиве распределения по возрасту
        /// </summary>
        /// <param name="popsbracket">V</param>
        /// <returns></returns>
        private ulong CalcTotalPops(ulong[] popsbracket)
        {
            //Считаем суммарное кол-во насленея по всему распределению возрастов и возвращаем значение.
            ulong totalPops = 0ul;
            for (int i = 0; i < popsbracket.Length - 1; i++)
            {
                totalPops += popsbracket[i];
            }
            return totalPops;
        }

        /// <summary>
        /// Экспорт реультатов в .csv файл
        /// </summary>
        /// <param name="filepath">Путь к фалу сохранения</param>
        private void WriteResultsToFile(string filepath = "./popdynamic.csv")
        {
            using (StreamWriter sw = new StreamWriter(filepath))
            {
                sw.WriteLine("Year;Population");
                foreach (var item in PopsCountPerYear)
                {
                    sw.WriteLine($"{item.Key};{item.Value}");
                }
            }
        }
    }
}