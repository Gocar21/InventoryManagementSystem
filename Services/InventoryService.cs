using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.Services
{
    public class InventoryService
    {
        private readonly List<Product> _products;
        private readonly List<Category> _categories;
        private readonly List<Supplier> _suppliers;
        private readonly List<User> _users;
        private readonly List<TransactionRecord> _transactions;

        private int _nextProductId = 1;
        private int _nextCategoryId = 1;
        private int _nextSupplierId = 1;
        private int _nextUserId = 1;
        private int _nextTransactionId = 1;

        private User? _currentUser;

        public User? CurrentUser => _currentUser;

        public InventoryService()
        {
            _products = new List<Product>();
            _categories = new List<Category>();
            _suppliers = new List<Supplier>();
            _users = new List<User>();
            _transactions = new List<TransactionRecord>();

            SeedData();
        }

        private void SeedData()
        {
            // Seed default admin
            _users.Add(new User(_nextUserId++, "admin", "admin123", "System Administrator", UserRole.Admin));
            _users.Add(new User(_nextUserId++, "staff1", "staff123", "Juan dela Cruz", UserRole.Staff));

            // Seed categories
            _categories.Add(new Category(_nextCategoryId++, "Hardware", "Tools and building materials"));
            _categories.Add(new Category(_nextCategoryId++, "School Supplies", "Notebooks, pens, and school items"));
            _categories.Add(new Category(_nextCategoryId++, "Beverages", "Drinks and liquid refreshments"));

            // Seed suppliers
            _suppliers.Add(new Supplier(_nextSupplierId++, "Builders Depot PH", "Carlo Mendoza", "09171110001", "carlo@buildersdepot.ph"));
            _suppliers.Add(new Supplier(_nextSupplierId++, "PaperMate Supplies", "Liza Cruz", "09282220002", "liza@papermate.ph"));
            _suppliers.Add(new Supplier(_nextSupplierId++, "CoolSip Trading", "Ramon Bautista", "09393330003", "ramon@coolsip.ph"));

            // Seed products
            _products.Add(new Product(_nextProductId++, "Claw Hammer 16oz", "Steel head, rubber grip", 349.00m, 30, 5, 1, 1));
            _products.Add(new Product(_nextProductId++, "Measuring Tape 5m", "Retractable steel tape measure", 189.00m, 15, 3, 1, 1));
            _products.Add(new Product(_nextProductId++, "Composition Notebook", "100 leaves, college ruled", 49.00m, 80, 15, 2, 2));
            _products.Add(new Product(_nextProductId++, "Colored Pencil Set 12s", "Assorted colors, wax-based", 95.00m, 4, 8, 2, 2));
            _products.Add(new Product(_nextProductId++, "Bottled Water 500ml", "Purified drinking water", 15.00m, 200, 30, 3, 3));
        }

        // ─── Authentication ───────────────────────────────────────────────────

        public bool Login(string username, string password)
        {
            var user = _users.Find(u => u.Username == username.ToLower());
            if (user != null && user.ValidatePassword(password))
            {
                _currentUser = user;
                return true;
            }
            return false;
        }

        public void Logout() => _currentUser = null;

        // ─── Category Management ──────────────────────────────────────────────

        public Category AddCategory(string name, string description)
        {
            if (_categories.Exists(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException($"A category named '{name}' already exists.");

            var category = new Category(_nextCategoryId++, name, description);
            _categories.Add(category);
            return category;
        }

        public List<Category> GetAllCategories() => new List<Category>(_categories);

        public Category? GetCategoryById(int id) => _categories.Find(c => c.CategoryId == id);

        // ─── Supplier Management ──────────────────────────────────────────────

        public Supplier AddSupplier(string name, string contactPerson, string phone, string email)
        {
            if (_suppliers.Exists(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException($"A supplier named '{name}' already exists.");

            var supplier = new Supplier(_nextSupplierId++, name, contactPerson, phone, email);
            _suppliers.Add(supplier);
            return supplier;
        }

        public List<Supplier> GetAllSuppliers() => new List<Supplier>(_suppliers);

        public Supplier? GetSupplierById(int id) => _suppliers.Find(s => s.SupplierId == id);

        // ─── Product Management ───────────────────────────────────────────────

        public Product AddProduct(string name, string description, decimal price,
                                   int stock, int lowStockThreshold, int categoryId, int supplierId)
        {
            if (GetCategoryById(categoryId) == null)
                throw new ArgumentException($"Category ID {categoryId} does not exist.");
            if (GetSupplierById(supplierId) == null)
                throw new ArgumentException($"Supplier ID {supplierId} does not exist.");
            if (_products.Exists(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException($"A product named '{name}' already exists.");

            var product = new Product(_nextProductId++, name, description, price, stock, lowStockThreshold, categoryId, supplierId);
            _products.Add(product);

            LogTransaction(product.ProductId, product.Name, TransactionType.ProductAdded, stock, 0, stock, "Initial stock added");
            return product;
        }

        public List<Product> GetAllProducts() => new List<Product>(_products);

        public Product? GetProductById(int id) => _products.Find(p => p.ProductId == id);

        public List<Product> SearchProducts(string keyword)
        {
            string kw = keyword.ToLower();
            return _products.FindAll(p =>
                p.Name.ToLower().Contains(kw) ||
                p.Description.ToLower().Contains(kw) ||
                p.ProductId.ToString() == kw);
        }

        public void UpdateProduct(int productId, string name, string description,
                                   decimal price, int lowStockThreshold, int categoryId, int supplierId)
        {
            var product = GetProductById(productId)
                ?? throw new KeyNotFoundException($"Product ID {productId} not found.");

            if (GetCategoryById(categoryId) == null)
                throw new ArgumentException($"Category ID {categoryId} does not exist.");
            if (GetSupplierById(supplierId) == null)
                throw new ArgumentException($"Supplier ID {supplierId} does not exist.");

            // Check name uniqueness (allow same product to keep its name)
            var duplicate = _products.Find(p =>
                p.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && p.ProductId != productId);
            if (duplicate != null)
                throw new InvalidOperationException($"Another product named '{name}' already exists.");

            product.Name = name;
            product.Description = description;
            product.Price = price;
            product.LowStockThreshold = lowStockThreshold;
            product.CategoryId = categoryId;
            product.SupplierId = supplierId;

            LogTransaction(product.ProductId, product.Name, TransactionType.ProductUpdated,
                           0, product.StockQuantity, product.StockQuantity, "Product details updated");
        }

        public void DeleteProduct(int productId)
        {
            var product = GetProductById(productId)
                ?? throw new KeyNotFoundException($"Product ID {productId} not found.");

            LogTransaction(product.ProductId, product.Name, TransactionType.ProductDeleted,
                           -product.StockQuantity, product.StockQuantity, 0, "Product deleted");
            _products.Remove(product);
        }

        public void RestockProduct(int productId, int quantity, string notes = "")
        {
            if (quantity <= 0)
                throw new ArgumentException("Restock quantity must be greater than zero.");

            var product = GetProductById(productId)
                ?? throw new KeyNotFoundException($"Product ID {productId} not found.");

            int before = product.StockQuantity;
            product.StockQuantity += quantity;

            LogTransaction(product.ProductId, product.Name, TransactionType.Restocked,
                           quantity, before, product.StockQuantity, notes);
        }

        public void DeductStock(int productId, int quantity, string notes = "")
        {
            if (quantity <= 0)
                throw new ArgumentException("Deduction quantity must be greater than zero.");

            var product = GetProductById(productId)
                ?? throw new KeyNotFoundException($"Product ID {productId} not found.");

            if (quantity > product.StockQuantity)
                throw new InvalidOperationException(
                    $"Insufficient stock. Available: {product.StockQuantity}, Requested: {quantity}.");

            int before = product.StockQuantity;
            product.StockQuantity -= quantity;

            LogTransaction(product.ProductId, product.Name, TransactionType.StockDeducted,
                           -quantity, before, product.StockQuantity, notes);
        }

        // ─── Inventory Reports ────────────────────────────────────────────────

        public List<Product> GetLowStockProducts()
        {
            return _products.FindAll(p => p.IsLowStock);
        }

        public decimal GetTotalInventoryValue()
        {
            decimal total = 0;
            foreach (var p in _products)
                total += p.TotalValue;
            return total;
        }

        // ─── Transaction History ──────────────────────────────────────────────

        public List<TransactionRecord> GetAllTransactions() => new List<TransactionRecord>(_transactions);

        public List<TransactionRecord> GetTransactionsByProduct(int productId)
        {
            return _transactions.FindAll(t => t.ProductId == productId);
        }

        // ─── Private Helpers ──────────────────────────────────────────────────

        private void LogTransaction(int productId, string productName, TransactionType type,
                                     int quantityChanged, int stockBefore, int stockAfter, string notes = "")
        {
            string performedBy = _currentUser?.Username ?? "system";
            _transactions.Add(new TransactionRecord(
                _nextTransactionId++, productId, productName,
                type, quantityChanged, stockBefore, stockAfter, performedBy, notes));
        }
    }
}
