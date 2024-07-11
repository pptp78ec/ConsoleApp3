using System.Text;

namespace ConsoleApp3
{
    internal class Program
    {
        static void Main(string[] args)
        {
             var newcalc = new NewCalcMethod(){ AgeOfFertility = 100, Lifespan = 2000, Startpopulation = 1000, TFR = 2.1, Timespan = 10000 };
             System.Console.WriteLine( $"Parameters:\nFertility age: {newcalc.AgeOfFertility},\nLifespan: {newcalc.Lifespan},\nStarting pop.: {newcalc.Startpopulation},\nTotal Fertility Rate: {newcalc.TFR}.\n*******\nTotal pop after {newcalc.Timespan} years is: {String.Format("{0:N}", newcalc.DoCalc())}");
        }
    }
}
