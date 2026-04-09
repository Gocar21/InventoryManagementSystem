namespace InventoryManagementSystem.Models
{
    public class Category
    {
        private int _categoryId;
        private string _name;
        private string _description;

        public int CategoryId
        {
            get => _categoryId;
            private set => _categoryId = value;
        }

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Category name cannot be empty.");
                _name = value.Trim();
            }
        }

        public string Description
        {
            get => _description;
            set => _description = value?.Trim() ?? string.Empty;
        }

        public Category(int categoryId, string name, string description = "")
        {
            CategoryId = categoryId;
            Name = name;
            Description = description;
        }

        public override string ToString()
        {
            return $"[{CategoryId}] {Name} - {Description}";
        }
    }
}
