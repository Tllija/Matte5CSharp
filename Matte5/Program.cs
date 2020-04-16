using Flee.PublicTypes;
using System;
using System.Collections.Generic;

namespace Matte5
{
    class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.WriteLine("\n\nPress enter after every pramter\nEnter n, A0, func for A, C0, func for C");

                var n = int.Parse(Console.ReadLine());

                var a0 = Double.Parse(Console.ReadLine());
                Func<double, double, double> aLambda = ParseString(Console.ReadLine());

                var c0 = Double.Parse(Console.ReadLine());
                Func<double, double, double> cLambda = ParseString(Console.ReadLine());

                //var (an, cn) = iter(n, a0, aLambda, c0, cLambda);
                Console.WriteLine("Calculatig for n = {0}, A0 = {1}, C0 = {2}", n, a0, c0);
                var (an, cn) = calc(n, a0, aLambda, c0, cLambda, true);
                Console.WriteLine("a: {0}, c: {1}", an, cn);
            }
            
        }

        private static Func<double, double, double> ParseString(string str)
        {
            var context = new ExpressionContext();
            context.Imports.AddType(typeof(Math));
            context.Variables.Add("x", (double)0.0);
            context.Variables.Add("n", (double)0);
            var e = context.CompileDynamic(str);

            Func<double, double, double> func = (double x, double n) =>
            {
                context.Variables["x"] = x;
                context.Variables["n"] = n;
                var result = (double)e.Evaluate();
                return result;
            };

            return func;
        }

        // Normal loop, this performs much better due to not having to have a bazillion stack frames 
        public static (double, double) calc(int n,
            double a0, Func<double, double, double> af,
            double c0, Func<double, double, double> cf,
            bool clamp = false)
        {

            double a = a0, c = c0;
            for(int i = 0; i < n; i++)
            {
                var aRented = af(a, i);
                var cRented = cf(c, i);

                if (clamp)
                {
                    // Negative cars cannot be rented and more cars than exist cannot be rented.
                    aRented = aRented.Clamp(0, a);
                    cRented = cRented.Clamp(0, c);
                }

                var an = a - aRented * 0.75 + cRented * 0.30;
                var cn = c - cRented * 0.30 + aRented * 0.75;

                a = an;
                c = cn;

                Console.WriteLine("N: {0} => ({1}, {2}) where the amount rented was ({3}, {4})", i, a.Round(2), c.Round(2), aRented.Round(2), cRented.Round(2));
            }

            return (a, c);
        }

        [Obsolete]
        public static (double, double) iter(int n, double a0, Func<double, double, double> af, double c0, Func<double, double, double> cf)
        {
            switch (n)
            {
                case 0: return (a0, c0); // N = 0, do nothing
                default:
                    var aCarsRented = af(a0, n); // Call function to determine amount of cars rented from a
                    var cCarsRented = cf(c0, n); // Do the same for c

                    var an = a0 - aCarsRented * 0.75 + cCarsRented * 0.3; // A at n+1
                    var cn = c0 - cCarsRented * 0.3 + aCarsRented * 0.75; // C at n+1

                    // unfortunately, C# doesn't support tail call optimization,
                    //so this will only work for very, very, very small values of n, a0, and c0.
                    return iter(n + 1, an, af, cn, cf);
            }
        }
    }
}
