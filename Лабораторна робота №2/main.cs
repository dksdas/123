
using System;
class P
{
    static double d = 0, v = 0;

    static void Main()
    {
        Menu();
    }

    static void Menu()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\nОблiк особистих фiнанciв");
        Console.ResetColor();

        Console.WriteLine("1 - Додати операцiю");
        Console.WriteLine("2 - Каталог категорiй");
        Console.WriteLine("3 - Пiдсумок");
        Console.WriteLine("0 - Вихiд");
        Console.Write("-> ");

        string x = Console.ReadLine();

        switch (x)
        {
            case "1":
                AddOp();
                break;
            case "2":
                Cat();
                break;
            case "3":
                Total();
                break;
            case "0":
                return;
            default:
                Console.WriteLine("не те ввiв");
                break;
        }

        Menu();
    }

    static void AddOp()
    {
        Console.WriteLine("\n1 - Дохiд");
        Console.WriteLine("2 - Витрата");
        Console.WriteLine("0 - Назад");
        Console.Write("-> ");

        string x = Console.ReadLine();

        if (x == "1")
        {
            try
            {
                Console.Write("Сума доходу: ");
                d += Convert.ToDouble(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("некоректне значення");
            }
        }
        else if (x == "2")
        {
            try
            {
                Console.Write("Сума витрати: ");
                v += Convert.ToDouble(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("некоректне значення");
            }
        }
        else if (x == "0") return;
        else Console.WriteLine("не те ввiв");

        AddOp();
    }

    static void Cat()
    {
        Console.WriteLine("\nКатегорiї витрат:");
        Console.WriteLine("1. Продукти");
        Console.WriteLine("2. Транспорт");
        Console.WriteLine("3. Розваги");
        Console.WriteLine("4. Комуналка");
        Console.WriteLine("5. Інше");
        Console.WriteLine("0. Назад");
        Console.Write("-> ");

        string x = Console.ReadLine();

        if (x == "0") return;
        else if (x == "1") Console.WriteLine("Продукти: покупки їжі, вода, супермаркет");
        else if (x == "2") Console.WriteLine("Транспорт: автобус, таксі, пальне");
        else if (x == "3") Console.WriteLine("Розваги: кіно, ігри, кафе");
        else if (x == "4") Console.WriteLine("Комуналка: світло, газ, вода");
        else if (x == "5") Console.WriteLine("Інше: дрібні покупки");
        else Console.WriteLine("не те ввiв");

        Cat();
    }

    static void Total()
    {
        double bal = Math.Round(d - v, 2);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nДоходи: " + d);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Витрати: " + v);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Баланс: " + bal);
        Console.ResetColor();
        Console.WriteLine("Enter щоб назад");
        Console.ReadLine();
    }
}
