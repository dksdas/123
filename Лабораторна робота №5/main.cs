using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;

struct Operaciya
{
    public int id;
    public string name;
    public double sum;
    public string category;
    public string type;
    public string date;
}

class P
{
    static string opsPath = "operacii.csv";
    static string usersPath = "users.csv";

    static void Main()
    {
        EnsureFiles();
        Login();
        Menu();
    }

    static void EnsureFiles()
    {
        if (!File.Exists(usersPath)) File.WriteAllText(usersPath, "Id,Email,PasswordHash\r\n");
        if (!File.Exists(opsPath)) File.WriteAllText(opsPath, "Id,Name,Sum,Category,Type,Date\r\n");
    }

    static string Hash(string s)
    {
        using (var sha = SHA256.Create())
        {
            var b = Encoding.UTF8.GetBytes(s ?? "");
            var h = sha.ComputeHash(b);
            return BitConverter.ToString(h).Replace("-", "").ToLower();
        }
    }

    static bool TryLogin(string email, string pass)
    {
        if (!File.Exists(usersPath)) return false;
        string hash = Hash(pass);
        var lines = File.ReadAllLines(usersPath);
        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i];
            if (string.IsNullOrWhiteSpace(line)) continue;
            var parts = line.Split(',');
            if (parts.Length >= 3 && parts[1].ToLower() == email.ToLower() && parts[2] == hash) return true;
        }
        return false;
    }

    static void RegisterUser()
    {
        Console.Write("Email: ");
        string email = Console.ReadLine();
        Console.Write("Пароль: ");
        string p1 = Console.ReadLine();
        Console.Write("Повторіть пароль: ");
        string p2 = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(p1) || p1 != p2)
        {
            Console.WriteLine("Помилка реєстрації");
            return;
        }
        var lines = File.ReadAllLines(usersPath);
        for (int i = 1; i < lines.Length; i++)
        {
            var parts = lines[i].Split(',');
            if (parts.Length >= 3 && parts[1].ToLower() == email.ToLower())
            {
                Console.WriteLine("Email вже існує");
                return;
            }
        }
        int id = GenerateNewId(usersPath);
        string hash = Hash(p1);
        File.AppendAllText(usersPath, id + "," + email + "," + hash + "\r\n");
        Console.WriteLine("Реєстрація успішна");
    }

    static int GenerateNewId(string path)
    {
        if (!File.Exists(path)) return 1;
        var lines = File.ReadAllLines(path);
        int max = 0;
        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i];
            if (string.IsNullOrWhiteSpace(line)) continue;
            var parts = line.Split(',');
            int id;
            if (parts.Length > 0 && int.TryParse(parts[0], out id))
            {
                if (id > max) max = id;
            }
        }
        return max + 1;
    }

    static void Login()
    {
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n1 - Вхід");
            Console.WriteLine("2 - Реєстрація");
            Console.WriteLine("0 - Вихід");
            Console.ResetColor();
            Console.Write("-> ");
            string c = Console.ReadLine();
            if (c == "1")
            {
                int attempts = 3;
                bool ok = false;
                while (!ok && attempts > 0)
                {
                    Console.Write("Email: ");
                    string e = Console.ReadLine();
                    Console.Write("Пароль: ");
                    string p = Console.ReadLine();
                    if (TryLogin(e, p))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Успішний вхід!");
                        Console.ResetColor();
                        ok = true;
                    }
                    else
                    {
                        attempts--;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Невірні дані. Спроб залишилось: " + attempts);
                        Console.ResetColor();
                    }
                }
                if (ok) break;
                else Environment.Exit(0);
            }
            else if (c == "2")
            {
                RegisterUser();
            }
            else if (c == "0")
            {
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("не те ввiв");
            }
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

    static List<Operaciya> ReadOperacii()
    {
        var list = new List<Operaciya>();
        if (!File.Exists(opsPath)) return list;
        var lines = File.ReadAllLines(opsPath);
        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i];
            if (string.IsNullOrWhiteSpace(line)) continue;
            try
            {
                var parts = line.Split(',');
                if (parts.Length != 6) continue;
                Operaciya o = new Operaciya();
                o.id = int.Parse(parts[0]);
                double s;
                if (!double.TryParse(parts[2], NumberStyles.Any, CultureInfo.InvariantCulture, out s)) continue;
                o.name = parts[1];
                o.sum = s;
                o.category = parts[3];
                o.type = parts[4];
                o.date = parts[5];
                list.Add(o);
            }
            catch { continue; }
        }
        return list;
    }

    static void WriteOperacii(List<Operaciya> list)
    {
        var lines = new List<string>();
        lines.Add("Id,Name,Sum,Category,Type,Date");
        for (int i = 0; i < list.Count; i++)
        {
            var o = list[i];
            lines.Add(o.id + "," + o.name + "," + o.sum.ToString(CultureInfo.InvariantCulture) + "," + o.category + "," + o.type + "," + o.date);
        }
        File.WriteAllLines(opsPath, lines.ToArray());
    }

    static void AppendOperaciya(Operaciya o)
    {
        if (!File.Exists(opsPath)) File.WriteAllText(opsPath, "Id,Name,Sum,Category,Type,Date\r\n");
        string line = o.id + "," + o.name + "," + o.sum.ToString(CultureInfo.InvariantCulture) + "," + o.category + "," + o.type + "," + o.date + "\r\n";
        File.AppendAllText(opsPath, line);
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
            o.id = GenerateNewId(opsPath);
            AppendOperaciya(o);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Операція додана");
            Console.ResetColor();
        }
    }

    static void ShowOperacii()
    {
        var data = ReadOperacii();
        if (data.Count == 0)
        {
            Console.WriteLine("Немає операцій");
            return;
        }

        Console.WriteLine("\n=== ОПЕРАЦІЇ ===");
        Console.WriteLine("{0,3} | {1,4} | {2,-14} | {3,10} | {4,-8} | {5,-12} | {6,-10}", "#", "ID", "Назва", "Сума", "Тип", "Категорія", "Дата");
        Console.WriteLine(new string('-', 80));
        for (int i = 0; i < data.Count; i++)
        {
            Console.WriteLine("{0,3} | {1,4} | {2,-14} | {3,10:F2} | {4,-8} | {5,-12} | {6,-10}",
                i + 1, data[i].id, data[i].name, data[i].sum, data[i].type, data[i].category, data[i].date);
        }
    }

    static void SearchOperaciya()
    {
        var data = ReadOperacii();
        if (data.Count == 0)
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
        for (int i = 0; i < data.Count; i++)
        {
            if ((m == "1" && data[i].name.ToLower().Contains(q.ToLower())) ||
                (m == "2" && data[i].category.ToLower().Contains(q.ToLower())))
            {
                found = i;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Знайдено: {0}. {1} - {2} грн ({3}), {4}, {5}",
                    i + 1, data[i].name, data[i].sum, data[i].type, data[i].category, data[i].date);
                Console.ResetColor();
                break;
            }
        }
        if (found == -1) Console.WriteLine("Не знайдено");
    }

    static void DeleteOperaciya()
    {
        var data = ReadOperacii();
        if (data.Count == 0)
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
                if (idx < 1 || idx > data.Count)
                {
                    Console.WriteLine("Невірний індекс");
                    return;
                }
                data.RemoveAt(idx - 1);
                WriteOperacii(data);
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
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].name.ToLower() == q.ToLower())
                {
                    data.RemoveAt(i);
                    WriteOperacii(data);
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
        var data = ReadOperacii();
        if (data.Count < 2)
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
            data.Sort((a, b) => string.Compare(a.name, b.name, StringComparison.OrdinalIgnoreCase));
            WriteOperacii(data);
            Console.WriteLine("Відсортовано за назвою");
        }
        else if (m == "2")
        {
            BubbleSortBySum(data);
            WriteOperacii(data);
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
        var baseData = ReadOperacii();
        List<Operaciya> x = new List<Operaciya>(baseData);
        List<Operaciya> y = new List<Operaciya>(baseData);
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
        var oper = ReadOperacii();
        if (oper.Count == 0)
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
        double minSum = oper[0].sum;
        double maxSum = oper[0].sum;
        int countOver500 = 0;

        for (int i = 0; i < oper.Count; i++)
        {
            if (oper[i].type.ToLower() == "дохід")
            {
                sumDohid += oper[i].sum;
                countDohid++;
            }
            else if (oper[i].type.ToLower() == "витрата")
            {
                sumVytrata += oper[i].sum;
                countVytrata++;
            }

            if (oper[i].sum > 500) countOver500++;
            if (oper[i].sum < minSum) minSum = oper[i].sum;
            if (oper[i].sum > maxSum) maxSum = oper[i].sum;
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
        var oper = ReadOperacii();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n========== ЗВІТ ==========");
        Console.ResetColor();

        Console.WriteLine("Кількість операцій: " + oper.Count);

        if (oper.Count > 0)
        {
            double totalDohid = 0;
            double totalVytrata = 0;
            for (int i = 0; i < oper.Count; i++)
            {
                if (oper[i].type.ToLower() == "дохід") totalDohid += oper[i].sum;
                else if (oper[i].type.ToLower() == "витрата") totalVytrata += oper[i].sum;
            }
            Console.WriteLine("Загальні доходи: " + Math.Round(totalDohid, 2) + " грн");
            Console.WriteLine("Загальні витрати: " + Math.Round(totalVytrata, 2) + " грн");
            Console.WriteLine("Фінансовий результат: " + Math.Round(totalDohid - totalVytrata, 2) + " грн");
        }

        Console.WriteLine("========================");
    }
}
