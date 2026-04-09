namespace InventoryManagementSystem.Helpers
{
    public static class UI
    {
        private const int W = 60;

        public static void Clear() => Console.Clear();

        public static void Line()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("  " + new string('_', W));
            Console.ResetColor();
        }

        public static void ThinLine()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("  " + new string('.', W));
            Console.ResetColor();
        }

        public static void Title(string text)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  {text.ToUpper()}");
            Line();
            Console.ResetColor();
        }

        public static void Banner()
        {
            Console.Clear();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("  " + new string('_', W));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("        S T O C K  T R A C K E R");
            Console.WriteLine("        Inventory Management System");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("  " + new string('_', W));
            Console.ResetColor();
            Console.WriteLine();
        }

        public static void Ok(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n  OK   {msg}");
            Console.ResetColor();
        }

        public static void Err(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n  ERR  {msg}");
            Console.ResetColor();
        }

        public static void Warn(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  LOW  {msg}");
            Console.ResetColor();
        }

        public static void Info(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"  {msg}");
            Console.ResetColor();
        }

        public static void Stat(string label, string value)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"  {label,-26}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(value);
            Console.ResetColor();
        }

        public static void Option(string key, string desc)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"   {key}  ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(desc);
            Console.ResetColor();
        }

        public static void Enter()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("  enter to continue . . .");
            Console.ResetColor();
            Console.ReadLine();
        }

        public static string Ask(string label, bool needed = true)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write($"  {label} : ");
                Console.ForegroundColor = ConsoleColor.White;
                string val = Console.ReadLine()?.Trim() ?? "";
                Console.ResetColor();
                if (!needed || val != "") return val;
                Err("This field is required.");
            }
        }

        public static int AskInt(string label, int min = int.MinValue, int max = int.MaxValue)
        {
            while (true)
            {
                string v = Ask(label);
                if (int.TryParse(v, out int r) && r >= min && r <= max) return r;
                Err($"Enter a whole number ({min} to {max}).");
            }
        }

        public static decimal AskDecimal(string label, decimal min = 0)
        {
            while (true)
            {
                string v = Ask(label);
                if (decimal.TryParse(v, out decimal r) && r >= min) return r;
                Err($"Enter a valid number (min {min}).");
            }
        }

        public static bool Confirm(string label)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"  {label} (y/n) : ");
            Console.ResetColor();
            string v = Console.ReadLine()?.Trim().ToLower() ?? "";
            return v == "y" || v == "yes";
        }

        public static void Row(ConsoleColor color, params (string val, int w)[] cols)
        {
            Console.ForegroundColor = color;
            Console.Write("  ");
            foreach (var (val, w) in cols)
            {
                string t = val.Length > w ? val[..(w - 2)] + ".." : val;
                Console.Write(t.PadRight(w));
            }
            Console.WriteLine();
            Console.ResetColor();
        }

        public static void Head(params (string lbl, int w)[] cols)
        {
            ThinLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("  ");
            foreach (var (lbl, w) in cols)
                Console.Write(lbl.PadRight(w));
            Console.WriteLine();
            ThinLine();
            Console.ResetColor();
        }
    }
}
