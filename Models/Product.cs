namespace InventoryManagementSystem.Models
{
    public class Product
    {
        private int _productId;
        private string _name;
        private string _description;
        private decimal _price;
        private int _stockQuantity;
        private int _lowStockThreshold;
        private int _categoryId;
        private int _supplierId;

        public int ProductId
        {
            get => _productId;
            private set => _productId = value;
        }

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Product name cannot be empty.");
                _name = value.Trim();
            }
        }

        public string Description
        {
            get => _description;
            set => _description = value?.Trim() ?? string.Empty;
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Price cannot be negative.");
                _price = value;
            }
        }

        public int StockQuantity
        {
            get => _stockQuantity;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Stock quantity cannot be negative.");
                _stockQuantity = value;
            }
        }

        public int LowStockThreshold
        {
            get => _lowStockThreshold;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Low stock threshold cannot be negative.");
                _lowStockThreshold = value;
            }
        }

        public int CategoryId
        {
            get => _categoryId;
            set => _categoryId = value;
        }

        public int SupplierId
        {
            get => _supplierId;
            set => _supplierId = value;
        }

        public bool IsLowStock => _stockQuantity <= _lowStockThreshold;

        public decimal TotalValue => _price * _stockQuantity;

        public Product(int productId, string name, string description, decimal price,
                       int stockQuantity, int lowStockThreshold, int categoryId, int supplierId)
        {
            ProductId = productId;
            Name = name;
            Description = description;
            Price = price;
            StockQuantity = stockQuantity;
            LowStockThreshold = lowStockThreshold;
            CategoryId = categoryId;
            SupplierId = supplierId;
        }

        public override string ToString()
        {
            return $"[{ProductId}] {Name} | Price: ₱{Price:F2} | Stock: {StockQuantity} | " +
                   $"CatID: {CategoryId} | SupplierID: {SupplierId}";
        }
    }
}
