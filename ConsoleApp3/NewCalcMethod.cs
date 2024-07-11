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
        //массив с разспределением по возрастам
        private ulong[] agebracket = Array.Empty<ulong>();

        public ulong DoCalc()
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
            }

            //Считаем суммарное кол-во насленея по всему распределению возрастов и возвращаем значение.
            ulong totalPops = 0ul;
            for (int i = 0; i < agebracket.Length - 1; i++)
            {
                totalPops += agebracket[i];
            }
            return totalPops;
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
    }
}