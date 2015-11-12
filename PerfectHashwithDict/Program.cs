using System;
using System.Collections.Generic;
using System.Data.HashFunction;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfectHashwithDict
{
    class Program
    {
        static void Main(string[] args)
        {
            var strings=new string[1000000];
            for (int i = 0; i < strings.Length; i++)
            {
                strings[i] = i.ToString();
            }

            //максимальный код
            var n = (int)(strings.Length * 100);
            var coded = new string[n];
            var haveCodes = new bool[n];
            Queue<string> withCollision=new Queue<string>();

          //  int minHash = strings.Min(s => s.GetHash());
         //   int maxHash = strings.Max(s => s.GetHash());

            for (int i = 0; i < strings.Length; i++)
            {
                var hash = strings[i].GetHash();
                var code = hash.GetCode(n);
                //var code = hash.GetCode(n, minHash, maxHash);
                if (haveCodes[code])//collision
                {
                    withCollision.Enqueue(strings[i]);
                }
                else // first code occurence
                {

                    haveCodes[code] = true;
                    coded[code] = strings[i];
                }
            }
            
            var dict = new Dictionary<string, int>();
            for (int i = 0; i < n; i++)
            {
                if(haveCodes[i]) continue;

                // i - свободный код
                
                var s = withCollision.Dequeue();
                dict.Add(s, i);

                coded[i] = s;

                // все коллизии размещены
                if(!withCollision.Any()) break;
            }

            Console.WriteLine("число строк " + strings.Length / 1000000.0+"млн.");
            Console.WriteLine("диапазон кодирования 0 ... n =" + n / 1000000.0 + "млн.");
            Console.WriteLine("число коллизий " + dict.Count);
            Console.WriteLine("процент коллизий (относительно n ) {0}%", 100 * dict.Count / n );
            Console.WriteLine("процент коллизий (относительно числа строк ) {0}%", 100 * dict.Count / strings.Length);


        }
    }

    public static class Hash
    {
        public static ModifiedBernsteinHash HashFunc=new ModifiedBernsteinHash() ;
        public static int GetHash(this string s)
        {
           return BitConverter.ToInt32(HashFunc.ComputeHash(Encoding.UTF32.GetBytes(s)), 0);
        }

        public static int GetCode(this int hash, int n)
        {
            var code = hash % n;
            if (code < 0) code = -code;
            return code;
        }

        public static int GetCode(this int hash, int n, int minHash, int maxHash)
        {
            var code = n * (hash - minHash)/ (maxHash-minHash+1);
            if (code < 0) code = -code;
            return code;
        }
    }
}
