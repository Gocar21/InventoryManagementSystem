namespace InventoryManagementSystem.Models
{
    public enum TransactionType
    {
        ProductAdded,
        ProductUpdated,
        ProductDeleted,
        Restocked,
        StockDeducted
    }

    public class TransactionRecord
    {
        private int _transactionId;
        private int _productId;
        private string _productName;
        private TransactionType _transactionType;
        private int _quantityChanged;
        private int _stockBefore;
        private int _stockAfter;
        private string _performedBy;
        private DateTime _timestamp;
        private string _notes;

        public int TransactionId
        {
            get => _transactionId;
            private set => _transactionId = value;
        }

        public int ProductId
        {
            get => _productId;
            private set => _productId = value;
        }

        public string ProductName
        {
            get => _productName;
            private set => _productName = value;
        }

        public TransactionType TransactionType
        {
            get => _transactionType;
            private set => _transactionType = value;
        }

        public int QuantityChanged
        {
            get => _quantityChanged;
            private set => _quantityChanged = value;
        }

        public int StockBefore
        {
            get => _stockBefore;
            private set => _stockBefore = value;
        }

        public int StockAfter
        {
            get => _stockAfter;
            private set => _stockAfter = value;
        }

        public string PerformedBy
        {
            get => _performedBy;
            private set => _performedBy = value;
        }

        public DateTime Timestamp
        {
            get => _timestamp;
            private set => _timestamp = value;
        }

        public string Notes
        {
            get => _notes;
            private set => _notes = value;
        }

        public TransactionRecord(int transactionId, int productId, string productName,
                                  TransactionType transactionType, int quantityChanged,
                                  int stockBefore, int stockAfter, string performedBy, string notes = "")
        {
            TransactionId = transactionId;
            ProductId = productId;
            ProductName = productName;
            TransactionType = transactionType;
            QuantityChanged = quantityChanged;
            StockBefore = stockBefore;
            StockAfter = stockAfter;
            PerformedBy = performedBy;
            Timestamp = DateTime.Now;
            Notes = notes;
        }

        public override string ToString()
        {
            return $"[{TransactionId}] {Timestamp:yyyy-MM-dd HH:mm:ss} | {TransactionType,-15} | " +
                   $"Product: {ProductName} (ID:{ProductId}) | " +
                   $"Qty Change: {QuantityChanged:+#;-#;0} | " +
                   $"Stock: {StockBefore} → {StockAfter} | By: {PerformedBy}";
        }
    }
}
