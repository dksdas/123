using System;
using System.Collections.Generic;

struct Operaciya
{
    public string name;
    public double sum;
    public string category;
    public string type;
    public string date;
}

class P
{
    static List<Operaciya> operacii = new List<Operaciya>();
    static string login = "user";
    static string password = "1234";

    static void Main()
    {
        Login();
        Menu();
    }

    static void Login()
    {
        int attempts = 3;
        bool auth = false;
        do
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n=== ВХІД В СИСТЕМУ ===");
            Console.ResetColor();
            Console.Write("Логін: ");
            string l = Console.ReadLine();
            Console.Write("Пароль: ");
            string p = Console.ReadLine();
            if (l == login && p == password)
            {
                auth = true;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Успішний вхід!");
                Console.ResetColor();
            }
            else
            {
                attempts--;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Невірні дані. Спроб залишилось: " + attempts);
                Console.ResetColor();
            }
        } while (!auth && attempts > 0);

        if (!auth)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Спроби вичерпані!");
            Console.ResetColor();
            Environment.Exit(0);
        }
    }

    static void Menu()
    {
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nОблiк особистих фiнанцив");
            Console.ResetColor();

            Console.WriteLine("1 - Додати операцiї");
            Console.WriteLine("2 - Показати операцiї (таблиця)");
            Console.WriteLine("3 - Статистика");
            Console.WriteLine("4 - Звіт");
            Console.WriteLine("5 - Пошук");
            Console.WriteLine("6 - Видалення");
            Console.WriteLine("7 - Сортування");
            Console.WriteLine("0 - Вихiд");
            Console.Write("-> ");

            string x = Console.ReadLine();

            switch (x)
            {
                case "1":
                    AddOperacii();
                    break;
                case "2":
                    ShowOperacii();
                    break;
                case "3":
                    Statistics();
                    break;
                case "4":
                    Report();
                    break;
                case "5":
                    SearchOperaciya();
                    break;
                case "6":
                    DeleteOperaciya();
                    break;
                case "7":
                    SortOperacii();
                    break;
                case "0":
                    Console.WriteLine("До побачення!");
                    return;
                default:
                    Console.WriteLine("не те ввiв");
                    break;
            }
        }
    }

    static void AddOperacii()
    {
        Console.WriteLine("\nСкільки операцій додати? ");
        int count = 0;
        try
        {
            count = Convert.ToInt32(Console.ReadLine());
        }
        catch
        {
            Console.WriteLine("Помилка введення");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            Console.WriteLine("\nОперація " + (i + 1));
            Operaciya o = new Operaciya();
            Console.Write("Назва: ");
            o.name = Console.ReadLine();
            try
            {
                Console.Write("Сума: ");
                o.sum = Convert.ToDouble(Console.ReadLine());
                Console.Write("Категорія: ");
                o.category = Console.ReadLine();
                Console.Write("Тип (Дохід/Витрата): ");
                o.type = Console.ReadLine();
                Console.Write("Дата (ДД.ММ.РРРР): ");
                o.date = Console.ReadLine();
            }
            catch
            {
                Console.WriteLine("Помилка введення");
                continue;
            }
            operacii.Add(o);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Операція додана");
            Console.ResetColor();
        }
    }

    static void ShowOperacii()
    {
        if (operacii.Count == 0)
        {
            Console.WriteLine("Немає операцій");
            return;
        }

        Console.WriteLine("\n=== ОПЕРАЦІЇ ===");
        Console.WriteLine("{0,3} | {1,-14} | {2,10} | {3,-8} | {4,-12} | {5,-10}", "#", "Назва", "Сума", "Тип", "Категорія", "Дата");
        Console.WriteLine(new string('-', 70));
        for (int i = 0; i < operacii.Count; i++)
        {
            Console.WriteLine("{0,3} | {1,-14} | {2,10:F2} | {3,-8} | {4,-12} | {5,-10}",
                i + 1, operacii[i].name, operacii[i].sum, operacii[i].type, operacii[i].category, operacii[i].date);
        }
    }

    static void SearchOperaciya()
    {
        if (operacii.Count == 0)
        {
            Console.WriteLine("Немає операцій");
            return;
        }

        Console.WriteLine("\nПошук за:\n1 - Назва\n2 - Категорія\n0 - Назад");
        Console.Write("-> ");
        string m = Console.ReadLine();
        if (m == "0") return;
        Console.Write("Введіть значення: ");
        string q = Console.ReadLine();

        int found = -1;
        for (int i = 0; i < operacii.Count; i++)
        {
            if ((m == "1" && operacii[i].name.ToLower().Contains(q.ToLower())) ||
                (m == "2" && operacii[i].category.ToLower().Contains(q.ToLower())))
            {
                found = i;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Знайдено: {0}. {1} - {2} грн ({3}), {4}, {5}",
                    i + 1, operacii[i].name, operacii[i].sum, operacii[i].type, operacii[i].category, operacii[i].date);
                Console.ResetColor();
                break;
            }
        }
        if (found == -1) Console.WriteLine("Не знайдено");
    }

    static void DeleteOperaciya()
    {
        if (operacii.Count == 0)
        {
            Console.WriteLine("Немає операцій");
            return;
        }

        Console.WriteLine("\nВидалення за:\n1 - Номер у списку\n2 - Назва\n0 - Назад");
        Console.Write("-> ");
        string m = Console.ReadLine();
        if (m == "0") return;

        if (m == "1")
        {
            Console.Write("Номер: ");
            try
            {
                int idx = Convert.ToInt32(Console.ReadLine());
                if (idx < 1 || idx > operacii.Count)
                {
                    Console.WriteLine("Невірний індекс");
                    return;
                }
                operacii.RemoveAt(idx - 1);
                Console.WriteLine("Видалено");
            }
            catch
            {
                Console.WriteLine("Помилка введення");
            }
        }
        else if (m == "2")
        {
            Console.Write("Назва: ");
            string q = Console.ReadLine();
            bool removed = false;
            for (int i = 0; i < operacii.Count; i++)
            {
                if (operacii[i].name.ToLower() == q.ToLower())
                {
                    operacii.RemoveAt(i);
                    Console.WriteLine("Видалено");
                    removed = true;
                    break;
                }
            }
            if (!removed) Console.WriteLine("Не знайдено");
        }
        else Console.WriteLine("не те ввiв");
    }

    static void SortOperacii()
    {
        if (operacii.Count < 2)
        {
            Console.WriteLine("Недостатньо даних");
            return;
        }

        Console.WriteLine("\nСортування:\n1 - Вбудоване за назвою (A->Z)\n2 - Бульбашкою за сумою (зростання)\n0 - Назад");
        Console.Write("-> ");
        string m = Console.ReadLine();
        if (m == "0") return;

        if (m == "1")
        {
            operacii.Sort((a, b) => string.Compare(a.name, b.name, StringComparison.OrdinalIgnoreCase));
            Console.WriteLine("Відсортовано за назвою");
        }
        else if (m == "2")
        {
            BubbleSortBySum(operacii);
            Console.WriteLine("Відсортовано за сумою (бульбашкою)");
            CompareSorts();
        }
        else Console.WriteLine("не те ввiв");
    }

    static void BubbleSortBySum(List<Operaciya> list)
    {
        for (int i = 0; i < list.Count - 1; i++)
        {
            bool swapped = false;
            for (int j = 0; j < list.Count - 1 - i; j++)
            {
                if (list[j].sum > list[j + 1].sum)
                {
                    Operaciya t = list[j];
                    list[j] = list[j + 1];
                    list[j + 1] = t;
                    swapped = true;
                }
            }
            if (!swapped) break;
        }
    }

    static void CompareSorts()
    {
        List<Operaciya> x = new List<Operaciya>(operacii);
        List<Operaciya> y = new List<Operaciya>(operacii);
        x.Sort((m, n) => m.sum.CompareTo(n.sum));
        BubbleSortBySum(y);
        bool same = true;
        for (int i = 0; i < x.Count; i++)
        {
            if (x[i].sum != y[i].sum)
            {
                same = false;
                break;
            }
        }
        Console.WriteLine("Порівняння з List.Sort(): " + (same ? "однаковий порядок за сумою" : "різний порядок за сумою"));
    }

    static void Statistics()
    {
        if (operacii.Count == 0)
        {
            Console.WriteLine("Немає даних для статистики");
            return;
        }

        double sumDohid = 0;
        double sumVytrata = 0;
        double avgDohid = 0;
        double avgVytrata = 0;
        int countDohid = 0;
        int countVytrata = 0;
        double minSum = operacii[0].sum;
        double maxSum = operacii[0].sum;
        int countOver500 = 0;

        for (int i = 0; i < operacii.Count; i++)
        {
            if (operacii[i].type.ToLower() == "дохід")
            {
                sumDohid += operacii[i].sum;
                countDohid++;
            }
            else if (operacii[i].type.ToLower() == "витрата")
            {
                sumVytrata += operacii[i].sum;
                countVytrata++;
            }

            if (operacii[i].sum > 500) countOver500++;
            if (operacii[i].sum < minSum) minSum = operacii[i].sum;
            if (operacii[i].sum > maxSum) maxSum = operacii[i].sum;
        }

        if (countDohid > 0) avgDohid = sumDohid / countDohid;
        if (countVytrata > 0) avgVytrata = sumVytrata / countVytrata;

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n=== СТАТИСТИКА ===");
        Console.WriteLine("Загальні доходи: " + Math.Round(sumDohid, 2) + " грн");
        Console.WriteLine("Загальні витрати: " + Math.Round(sumVytrata, 2) + " грн");
        Console.WriteLine("Баланс: " + Math.Round(sumDohid - sumVytrata, 2) + " грн");
        Console.WriteLine("Середній дохід: " + Math.Round(avgDohid, 2) + " грн");
        Console.WriteLine("Середня витрата: " + Math.Round(avgVytrata, 2) + " грн");
        Console.WriteLine("Операцій більше 500 грн: " + countOver500);
        Console.WriteLine("Мінімальна сума: " + minSum + " грн");
        Console.WriteLine("Максимальна сума: " + maxSum + " грн");
        Console.ResetColor();
    }

    static void Report()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n========== ЗВІТ ==========");
        Console.ResetColor();

        Console.WriteLine("Кількість операцій: " + operacii.Count);

        if (operacii.Count > 0)
        {
            double totalDohid = 0;
            double totalVytrata = 0;
            for (int i = 0; i < operacii.Count; i++)
            {
                if (operacii[i].type.ToLower() == "дохід") totalDohid += operacii[i].sum;
                else if (operacii[i].type.ToLower() == "витрата") totalVytrata += operacii[i].sum;
            }
            Console.WriteLine("Загальні доходи: " + Math.Round(totalDohid, 2) + " грн");
            Console.WriteLine("Загальні витрати: " + Math.Round(totalVytrata, 2) + " грн");
            Console.WriteLine("Фінансовий результат: " + Math.Round(totalDohid - totalVytrata, 2) + " грн");
        }

        Console.WriteLine("========================");
    }
}
