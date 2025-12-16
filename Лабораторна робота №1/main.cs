using System;

class P
{
    static void Main()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Облiк витратiв i доходiв");
        Console.ResetColor();

        double d = 0, v = 0;
        string x;

        while (true)
        {
            Console.WriteLine("\n1 - Дохід | 2 - Витрата | 3 - Вихід");
            Console.Write("-> ");
            x = Console.ReadLine();

            if (x == "1")
            {
                Console.Write("Введіть суму доходу: ");
                d += Convert.ToDouble(Console.ReadLine());
            }
            else if (x == "2")
            {
                Console.Write("Введіть суму витрати: ");
                v += Convert.ToDouble(Console.ReadLine());
            }
            else if (x == "3") break;
        }

        double bal = Math.Round(d - v, 2);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nДоходи: " + d);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Витрати: " + v);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Баланс: " + bal);
        Console.ResetColor();
        Console.WriteLine("\nДякуємо за користування програмою!");
    }
}
