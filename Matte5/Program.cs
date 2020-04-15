using System;

namespace Matte5
{
    class Program
    {
        static void Main()
        {
            
        }

        public static (double, double) iter(int n, double a0, Func<double, int, double> af, double c0, Func<double, int, double> cf)
        {
            switch (n)
            {
                case 0: return (a0, c0); // N = 0, do nothing
                default:
                    var aCarsRented = af(a0, n); // Call function to determine amount of cars rented from a
                    var cCarsRented = cf(c0, n); // Do the same for c

                    var an = a0 - aCarsRented * 0.75 + cCarsRented * 0.3; // A at n+1
                    var cn = c0 - cCarsRented * 0.3 + aCarsRented * 0.75; // C at n+1

                    return (an, cn);
            }
        }
    }
}
