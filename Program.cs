using InventoryManagementSystem.Helpers;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;

class Program
{
    static readonly InventoryService store = new InventoryService();

    static void Main(string[] args)
    {
        Console.Title = "StockTracker";
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        UI.Banner();
        UI.Enter();
        MainMenu();
    }

    // ── MAIN MENU ─────────────────────────────────────────────────────────────

    static void MainMenu()
    {
        while (true)
        {
            UI.Clear();
            UI.Banner();
            UI.Title("Main Menu");
            UI.Option("1", "Categories");
            UI.Option("2", "Suppliers");
            UI.Option("3", "Products");
            UI.Option("4", "Stock");
            UI.Option("5", "Reports");
            UI.Option("6", "Transactions");
            Console.WriteLine();
            UI.Option("0", "Exit");
            UI.Line();

            string choice = UI.Ask("choice");

            if (choice == "1") CategoriesMenu();
            else if (choice == "2") SuppliersMenu();
            else if (choice == "3") ProductsMenu();
            else if (choice == "4") StockMenu();
            else if (choice == "5") ReportsMenu();
            else if (choice == "6") ShowTransactions();
            else if (choice == "0")
            {
                UI.Clear();
                UI.Banner();
                UI.Ok("Goodbye.");
                return;
            }
            else
            {
                UI.Err("Invalid choice.");
                UI.Enter();
            }
        }
    }

    // ── CATEGORIES ────────────────────────────────────────────────────────────

    static void CategoriesMenu()
    {
        while (true)
        {
            UI.Clear();
            UI.Title("Categories");
            UI.Option("1", "Add Category");
            UI.Option("2", "View All");
            UI.Option("0", "Back");
            UI.Line();

            string choice = UI.Ask("choice");

            if (choice == "1") AddCategory();
            else if (choice == "2") ViewCategories();
            else if (choice == "0") return;
            else { UI.Err("Invalid."); UI.Enter(); }
        }
    }

    static void AddCategory()
    {
        UI.Clear();
        UI.Title("New Category");
        try
        {
            string name = UI.Ask("name");
            string desc = UI.Ask("description", needed: false);
            var c = store.AddCategory(name, desc);
            UI.Ok("Category saved  [ id: " + c.CategoryId + " ]");
        }
        catch (Exception ex) { UI.Err(ex.Message); }
        UI.Enter();
    }

    static void ViewCategories()
    {
        UI.Clear();
        UI.Title("All Categories");
        var list = store.GetAllCategories();
        if (list.Count == 0) { UI.Info("No categories found."); UI.Enter(); return; }

        UI.Head(
            ("ID",          5),
            ("Name",        22),
            ("Description", 30));

        foreach (var c in list)
        {
            UI.Row(ConsoleColor.White,
                (c.CategoryId.ToString(), 5),
                (c.Name,                  22),
                (c.Description,           30));
        }
        UI.Enter();
    }

    // ── SUPPLIERS ─────────────────────────────────────────────────────────────

    static void SuppliersMenu()
    {
        while (true)
        {
            UI.Clear();
            UI.Title("Suppliers");
            UI.Option("1", "Add Supplier");
            UI.Option("2", "View All");
            UI.Option("0", "Back");
            UI.Line();

            string choice = UI.Ask("choice");

            if (choice == "1") AddSupplier();
            else if (choice == "2") ViewSuppliers();
            else if (choice == "0") return;
            else { UI.Err("Invalid."); UI.Enter(); }
        }
    }

    static void AddSupplier()
    {
        UI.Clear();
        UI.Title("New Supplier");
        try
        {
            string name    = UI.Ask("company name");
            string contact = UI.Ask("contact person", needed: false);
            string phone   = UI.Ask("phone",          needed: false);
            string email   = UI.Ask("email",          needed: false);
            var s = store.AddSupplier(name, contact, phone, email);
            UI.Ok("Supplier saved  [ id: " + s.SupplierId + " ]");
        }
        catch (Exception ex) { UI.Err(ex.Message); }
        UI.Enter();
    }

    static void ViewSuppliers()
    {
        UI.Clear();
        UI.Title("All Suppliers");
        var list = store.GetAllSuppliers();
        if (list.Count == 0) { UI.Info("No suppliers found."); UI.Enter(); return; }

        UI.Head(
            ("ID",      5),
            ("Company", 20),
            ("Contact", 16),
            ("Phone",   14),
            ("Email",   22));

        foreach (var s in list)
        {
            UI.Row(ConsoleColor.White,
                (s.SupplierId.ToString(), 5),
                (s.Name,                  20),
                (s.ContactPerson,         16),
                (s.Phone,                 14),
                (s.Email,                 22));
        }
        UI.Enter();
    }

    // ── PRODUCTS ──────────────────────────────────────────────────────────────

    static void ProductsMenu()
    {
        while (true)
        {
            UI.Clear();
            UI.Title("Products");
            UI.Option("1", "Add Product");
            UI.Option("2", "View All");
            UI.Option("3", "Search");
            UI.Option("4", "Edit Product");
            UI.Option("5", "Remove Product");
            UI.Option("0", "Back");
            UI.Line();

            string choice = UI.Ask("choice");

            if (choice == "1") AddProduct();
            else if (choice == "2") ViewProducts();
            else if (choice == "3") SearchProducts();
            else if (choice == "4") EditProduct();
            else if (choice == "5") RemoveProduct();
            else if (choice == "0") return;
            else { UI.Err("Invalid."); UI.Enter(); }
        }
    }

    static void AddProduct()
    {
        UI.Clear();
        UI.Title("New Product");
        ShowCatsInline();
        ShowSupsInline();
        UI.ThinLine();
        try
        {
            string  name      = UI.Ask("product name");
            string  desc      = UI.Ask("description",       needed: false);
            decimal price     = UI.AskDecimal("price (PHP)");
            int     qty       = UI.AskInt("initial stock",  0);
            int     threshold = UI.AskInt("low stock alert at", 0);
            int     catId     = UI.AskInt("category id");
            int     supId     = UI.AskInt("supplier id");

            var p = store.AddProduct(name, desc, price, qty, threshold, catId, supId);
            UI.Ok("Product saved  [ id: " + p.ProductId + " ]");
        }
        catch (Exception ex) { UI.Err(ex.Message); }
        UI.Enter();
    }

    static void ViewProducts()
    {
        UI.Clear();
        UI.Title("All Products");
        var list = store.GetAllProducts();
        if (list.Count == 0) { UI.Info("No products found."); UI.Enter(); return; }
        PrintProductTable(list);
        UI.Enter();
    }

    static void SearchProducts()
    {
        UI.Clear();
        UI.Title("Search Products");
        string kw = UI.Ask("keyword");
        var results = store.SearchProducts(kw);
        Console.WriteLine();
        if (results.Count == 0)
            UI.Info("No results for \"" + kw + "\".");
        else
        {
            UI.Info(results.Count + " result(s) found.");
            PrintProductTable(results);
        }
        UI.Enter();
    }

    static void EditProduct()
    {
        UI.Clear();
        UI.Title("Edit Product");
        ViewProducts_Inline();
        try
        {
            int id = UI.AskInt("product id");
            var p  = store.GetProductById(id);
            if (p == null) { UI.Err("Product not found."); UI.Enter(); return; }

            UI.ThinLine();
            UI.Info("editing: " + p.Name + "  (blank = keep current)");
            UI.ThinLine();
            ShowCatsInline();
            ShowSupsInline();
            UI.ThinLine();

            string name = UI.Ask("name [" + p.Name + "]", needed: false);
            if (name == "") name = p.Name;

            string desc = UI.Ask("description [" + p.Description + "]", needed: false);
            if (desc == "") desc = p.Description;

            string ps     = UI.Ask("price [" + p.Price.ToString("F2") + "]", needed: false);
            decimal price = ps == "" ? p.Price : decimal.Parse(ps);

            string ts     = UI.Ask("low stock alert [" + p.LowStockThreshold + "]", needed: false);
            int threshold = ts == "" ? p.LowStockThreshold : int.Parse(ts);

            string cs  = UI.Ask("category id [" + p.CategoryId + "]", needed: false);
            int catId  = cs == "" ? p.CategoryId : int.Parse(cs);

            string ss  = UI.Ask("supplier id [" + p.SupplierId + "]", needed: false);
            int supId  = ss == "" ? p.SupplierId : int.Parse(ss);

            store.UpdateProduct(id, name, desc, price, threshold, catId, supId);
            UI.Ok("Product updated.");
        }
        catch (FormatException) { UI.Err("Invalid number format."); }
        catch (Exception ex)    { UI.Err(ex.Message); }
        UI.Enter();
    }

    static void RemoveProduct()
    {
        UI.Clear();
        UI.Title("Remove Product");
        ViewProducts_Inline();
        try
        {
            int id = UI.AskInt("product id");
            var p  = store.GetProductById(id);
            if (p == null) { UI.Err("Product not found."); UI.Enter(); return; }

            UI.Warn("About to remove: " + p.Name + "  (stock: " + p.StockQuantity + ")");
            if (UI.Confirm("Are you sure?"))
            {
                store.DeleteProduct(id);
                UI.Ok("Product removed.");
            }
            else UI.Info("Cancelled.");
        }
        catch (Exception ex) { UI.Err(ex.Message); }
        UI.Enter();
    }

    // ── STOCK ─────────────────────────────────────────────────────────────────

    static void StockMenu()
    {
        while (true)
        {
            UI.Clear();
            UI.Title("Stock");
            UI.Option("1", "Add Stock");
            UI.Option("2", "Deduct Stock");
            UI.Option("0", "Back");
            UI.Line();

            string choice = UI.Ask("choice");

            if (choice == "1") AddStock();
            else if (choice == "2") DeductStock();
            else if (choice == "0") return;
            else { UI.Err("Invalid."); UI.Enter(); }
        }
    }

    static void AddStock()
    {
        UI.Clear();
        UI.Title("Add Stock");
        ViewProducts_Inline();
        try
        {
            int id = UI.AskInt("product id");
            var p  = store.GetProductById(id);
            if (p == null) { UI.Err("Product not found."); UI.Enter(); return; }

            UI.Info(p.Name + "  |  current stock: " + p.StockQuantity);
            int    qty  = UI.AskInt("quantity to add", 1);
            string note = UI.Ask("notes", needed: false);

            store.RestockProduct(id, qty, note);
            UI.Ok("Stock updated.  New qty: " + store.GetProductById(id)!.StockQuantity);
        }
        catch (Exception ex) { UI.Err(ex.Message); }
        UI.Enter();
    }

    static void DeductStock()
    {
        UI.Clear();
        UI.Title("Deduct Stock");
        ViewProducts_Inline();
        try
        {
            int id = UI.AskInt("product id");
            var p  = store.GetProductById(id);
            if (p == null) { UI.Err("Product not found."); UI.Enter(); return; }

            UI.Info(p.Name + "  |  current stock: " + p.StockQuantity);
            int    qty  = UI.AskInt("quantity to deduct", 1);
            string note = UI.Ask("notes", needed: false);

            store.DeductStock(id, qty, note);
            var updated = store.GetProductById(id)!;
            UI.Ok("Stock updated.  New qty: " + updated.StockQuantity);
            if (updated.IsLowStock)
                UI.Warn(updated.Name + " is running low  [ " + updated.StockQuantity + " left ]");
        }
        catch (Exception ex) { UI.Err(ex.Message); }
        UI.Enter();
    }

    // ── REPORTS ───────────────────────────────────────────────────────────────

    static void ReportsMenu()
    {
        while (true)
        {
            UI.Clear();
            UI.Title("Reports");
            UI.Option("1", "Low Stock Items");
            UI.Option("2", "Inventory Value");
            UI.Option("3", "Summary");
            UI.Option("0", "Back");
            UI.Line();

            string choice = UI.Ask("choice");

            if (choice == "1") LowStockReport();
            else if (choice == "2") ValueReport();
            else if (choice == "3") SummaryReport();
            else if (choice == "0") return;
            else { UI.Err("Invalid."); UI.Enter(); }
        }
    }

    static void LowStockReport()
    {
        UI.Clear();
        UI.Title("Low Stock Items");
        var list = store.GetLowStockProducts();
        if (list.Count == 0)
        {
            UI.Ok("All products are well stocked.");
        }
        else
        {
            UI.Info(list.Count + " item(s) need restocking.");
            UI.Head(
                ("ID",       5),
                ("Name",     24),
                ("Stock",    7),
                ("Alert At", 9),
                ("Value",    14));

            foreach (var p in list)
            {
                UI.Row(ConsoleColor.Yellow,
                    (p.ProductId.ToString(),        5),
                    (p.Name,                        24),
                    (p.StockQuantity.ToString(),    7),
                    (p.LowStockThreshold.ToString(), 9),
                    ("PHP " + p.TotalValue.ToString("F2"), 14));
            }
        }
        UI.Enter();
    }

    static void ValueReport()
    {
        UI.Clear();
        UI.Title("Inventory Value");
        var list      = store.GetAllProducts();
        decimal total = store.GetTotalInventoryValue();

        UI.Head(
            ("ID",    5),
            ("Name",  24),
            ("Price", 12),
            ("Qty",   5),
            ("Total", 14));

        foreach (var p in list)
        {
            UI.Row(ConsoleColor.White,
                (p.ProductId.ToString(),             5),
                (p.Name,                             24),
                ("PHP " + p.Price.ToString("F2"),    12),
                (p.StockQuantity.ToString(),          5),
                ("PHP " + p.TotalValue.ToString("F2"), 14));
        }

        UI.ThinLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("  Total Inventory Value          PHP " + total.ToString("F2"));
        Console.ResetColor();
        UI.Enter();
    }

    static void SummaryReport()
    {
        UI.Clear();
        UI.Title("Summary");
        UI.Stat("Total Products",     store.GetAllProducts().Count.ToString());
        UI.Stat("Total Categories",   store.GetAllCategories().Count.ToString());
        UI.Stat("Total Suppliers",    store.GetAllSuppliers().Count.ToString());
        UI.Stat("Low Stock Items",    store.GetLowStockProducts().Count.ToString());
        UI.Stat("Total Transactions", store.GetAllTransactions().Count.ToString());
        UI.ThinLine();
        Console.ForegroundColor = ConsoleColor.Green;
        UI.Stat("Total Inventory Value", "PHP " + store.GetTotalInventoryValue().ToString("F2"));
        Console.ResetColor();
        UI.Enter();
    }

    // ── TRANSACTIONS ──────────────────────────────────────────────────────────

    static void ShowTransactions()
    {
        UI.Clear();
        UI.Title("Transaction History");
        var list = store.GetAllTransactions();

        if (list.Count == 0) { UI.Info("No transactions yet."); UI.Enter(); return; }

        UI.Info("total: " + list.Count + " transaction(s)");
        UI.Head(
            ("ID",      4),
            ("Date",    18),
            ("Type",    14),
            ("Product", 20),
            ("Chg",     5),
            ("Before",  7),
            ("After",   7),
            ("By",      9));

        foreach (var t in list)
        {
            ConsoleColor color;
            if (t.TransactionType == TransactionType.Restocked ||
                t.TransactionType == TransactionType.ProductAdded)
                color = ConsoleColor.Green;
            else if (t.TransactionType == TransactionType.StockDeducted ||
                     t.TransactionType == TransactionType.ProductDeleted)
                color = ConsoleColor.Red;
            else
                color = ConsoleColor.Gray;

            string chg = (t.QuantityChanged >= 0 ? "+" : "") + t.QuantityChanged;

            UI.Row(color,
                (t.TransactionId.ToString(),              4),
                (t.Timestamp.ToString("MM/dd/yy HH:mm"), 18),
                (t.TransactionType.ToString(),            14),
                (t.ProductName,                           20),
                (chg,                                      5),
                (t.StockBefore.ToString(),                 7),
                (t.StockAfter.ToString(),                  7),
                (t.PerformedBy,                            9));
        }
        UI.Enter();
    }

    // ── HELPERS ───────────────────────────────────────────────────────────────

    static void PrintProductTable(List<Product> list)
    {
        UI.Head(
            ("ID",     5),
            ("Name",   22),
            ("Price",  12),
            ("Qty",    5),
            ("Alert",  6),
            ("Cat",    4),
            ("Sup",    4),
            ("Status", 7));

        foreach (var p in list)
        {
            ConsoleColor c = p.IsLowStock ? ConsoleColor.Yellow : ConsoleColor.White;
            UI.Row(c,
                (p.ProductId.ToString(),           5),
                (p.Name,                           22),
                ("PHP " + p.Price.ToString("F2"),  12),
                (p.StockQuantity.ToString(),         5),
                (p.LowStockThreshold.ToString(),     6),
                (p.CategoryId.ToString(),            4),
                (p.SupplierId.ToString(),            4),
                (p.IsLowStock ? "LOW" : "ok",        7));
        }
    }

    static void ViewProducts_Inline()
    {
        var list = store.GetAllProducts();
        if (list.Count == 0) return;
        PrintProductTable(list);
        UI.ThinLine();
    }

    static void ShowCatsInline()
    {
        var list = store.GetAllCategories();
        if (list.Count == 0) return;
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.Write("  categories : ");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(string.Join("   ", list.ConvertAll(c => c.CategoryId + ":" + c.Name)));
        Console.ResetColor();
    }

    static void ShowSupsInline()
    {
        var list = store.GetAllSuppliers();
        if (list.Count == 0) return;
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.Write("  suppliers  : ");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(string.Join("   ", list.ConvertAll(s => s.SupplierId + ":" + s.Name)));
        Console.ResetColor();
    }
}
