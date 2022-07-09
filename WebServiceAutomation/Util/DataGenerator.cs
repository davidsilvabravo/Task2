using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2.Utils
{
    public class DataGenerator
    {
        //Generar un string de una longitud determinada
        public static string RandomString(int length)
        {
            Random random = new Random(DateTime.Now.Millisecond);
            var rString = "";
            for (var i = 0; i < length; i++)
                rString += ((char)(random.Next(1, 26) + 64)).ToString().ToUpper();
            // Thread.Sleep(1);
            return rString;
        }
    }
}
