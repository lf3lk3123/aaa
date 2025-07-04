using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.Data
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;
        private readonly string _dbPath;

        public DatabaseHelper()
        {
            _dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "local.db");
            _connectionString = $"Data Source={_dbPath};Version=3;";
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            if (!File.Exists(_dbPath))
            {
                SQLiteConnection.CreateFile(_dbPath);
            }

            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            // 創建商品表
            var createItemsTable = @"
                CREATE TABLE IF NOT EXISTS Items (
                    ItemID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Price REAL NOT NULL DEFAULT 0,
                    Stock INTEGER NOT NULL DEFAULT 0,
                    MinStock INTEGER NOT NULL DEFAULT 10,
                    Category TEXT,
                    Description TEXT,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                )";

            // 創建進貨記錄表
            var createPurchasesTable = @"
                CREATE TABLE IF NOT EXISTS Purchases (
                    PurchaseID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date DATETIME NOT NULL,
                    Supplier TEXT NOT NULL,
                    ItemID INTEGER NOT NULL,
                    Quantity INTEGER NOT NULL,
                    UnitPrice REAL NOT NULL,
                    TotalAmount REAL NOT NULL,
                    Notes TEXT,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (ItemID) REFERENCES Items(ItemID)
                )";

            // 創建銷售記錄表
            var createSalesTable = @"
                CREATE TABLE IF NOT EXISTS Sales (
                    SaleID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date DATETIME NOT NULL,
                    Customer TEXT,
                    ItemID INTEGER NOT NULL,
                    Quantity INTEGER NOT NULL,
                    UnitPrice REAL NOT NULL,
                    TotalAmount REAL NOT NULL,
                    Discount REAL DEFAULT 0,
                    FinalAmount REAL NOT NULL,
                    Notes TEXT,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (ItemID) REFERENCES Items(ItemID)
                )";

            // 創建會員表
            var createMembersTable = @"
                CREATE TABLE IF NOT EXISTS Members (
                    MemberID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Phone TEXT,
                    Email TEXT,
                    Address TEXT,
                    Points INTEGER DEFAULT 0,
                    JoinDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                    LastVisit DATETIME,
                    IsActive BOOLEAN DEFAULT 1
                )";

            // 創建會員消費記錄表
            var createMemberPurchasesTable = @"
                CREATE TABLE IF NOT EXISTS MemberPurchases (
                    RecordID INTEGER PRIMARY KEY AUTOINCREMENT,
                    MemberID INTEGER NOT NULL,
                    SaleID INTEGER NOT NULL,
                    Date DATETIME NOT NULL,
                    Amount REAL NOT NULL,
                    PointsEarned INTEGER DEFAULT 0,
                    PointsUsed INTEGER DEFAULT 0,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (MemberID) REFERENCES Members(MemberID),
                    FOREIGN KEY (SaleID) REFERENCES Sales(SaleID)
                )";

            // 創建供應商表
            var createSuppliersTable = @"
                CREATE TABLE IF NOT EXISTS Suppliers (
                    SupplierID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    ContactPerson TEXT,
                    Phone TEXT,
                    Email TEXT,
                    Address TEXT,
                    Notes TEXT,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                    IsActive BOOLEAN DEFAULT 1
                )";

            // 執行創建表格命令
            ExecuteNonQuery(createItemsTable);
            ExecuteNonQuery(createPurchasesTable);
            ExecuteNonQuery(createSalesTable);
            ExecuteNonQuery(createMembersTable);
            ExecuteNonQuery(createMemberPurchasesTable);
            ExecuteNonQuery(createSuppliersTable);

            // 創建索引
            ExecuteNonQuery("CREATE INDEX IF NOT EXISTS idx_items_name ON Items(Name)");
            ExecuteNonQuery("CREATE INDEX IF NOT EXISTS idx_purchases_date ON Purchases(Date)");
            ExecuteNonQuery("CREATE INDEX IF NOT EXISTS idx_sales_date ON Sales(Date)");
            ExecuteNonQuery("CREATE INDEX IF NOT EXISTS idx_members_phone ON Members(Phone)");

            // 插入初始數據
            InsertInitialData();
        }

        private void InsertInitialData()
        {
            // 檢查是否已有數據
            var itemCount = ExecuteScalar("SELECT COUNT(*) FROM Items");
            if (Convert.ToInt32(itemCount) == 0)
            {
                // 插入示例商品
                var sampleItems = new[]
                {
                    ("蘋果", 25.0, 100, 20, "水果"),
                    ("香蕉", 15.0, 80, 15, "水果"),
                    ("牛奶", 45.0, 50, 10, "乳製品"),
                    ("麵包", 30.0, 30, 5, "烘焙"),
                    ("雞蛋", 8.0, 200, 50, "蛋類")
                };

                foreach (var (name, price, stock, minStock, category) in sampleItems)
                {
                    var sql = @"INSERT INTO Items (Name, Price, Stock, MinStock, Category) 
                               VALUES (@name, @price, @stock, @minStock, @category)";
                    var parameters = new[]
                    {
                        new SQLiteParameter("@name", name),
                        new SQLiteParameter("@price", price),
                        new SQLiteParameter("@stock", stock),
                        new SQLiteParameter("@minStock", minStock),
                        new SQLiteParameter("@category", category)
                    };
                    ExecuteNonQuery(sql, parameters);
                }

                // 插入示例供應商
                var sampleSuppliers = new[]
                {
                    ("新鮮果菜批發", "王小明", "02-1234-5678", "supplier1@example.com"),
                    ("優質乳品公司", "李大華", "02-2345-6789", "supplier2@example.com"),
                    ("美味烘焙坊", "張美麗", "02-3456-7890", "supplier3@example.com")
                };

                foreach (var (name, contact, phone, email) in sampleSuppliers)
                {
                    var sql = @"INSERT INTO Suppliers (Name, ContactPerson, Phone, Email) 
                               VALUES (@name, @contact, @phone, @email)";
                    var parameters = new[]
                    {
                        new SQLiteParameter("@name", name),
                        new SQLiteParameter("@contact", contact),
                        new SQLiteParameter("@phone", phone),
                        new SQLiteParameter("@email", email)
                    };
                    ExecuteNonQuery(sql, parameters);
                }
            }
        }

        public void ExecuteNonQuery(string sql, SQLiteParameter[]? parameters = null)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            using var command = new SQLiteCommand(sql, connection);
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }
            command.ExecuteNonQuery();
        }

        public object? ExecuteScalar(string sql, SQLiteParameter[]? parameters = null)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            using var command = new SQLiteCommand(sql, connection);
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }
            return command.ExecuteScalar();
        }

        public DataTable ExecuteQuery(string sql, SQLiteParameter[]? parameters = null)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            using var command = new SQLiteCommand(sql, connection);
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }
            using var adapter = new SQLiteDataAdapter(command);
            var dataTable = new DataTable();
            adapter.Fill(dataTable);
            return dataTable;
        }

        // 商品相關操作
        public List<Item> GetAllItems()
        {
            var items = new List<Item>();
            var sql = "SELECT * FROM Items ORDER BY Name";
            var dataTable = ExecuteQuery(sql);

            foreach (DataRow row in dataTable.Rows)
            {
                items.Add(new Item
                {
                    ItemID = Convert.ToInt32(row["ItemID"]),
                    Name = row["Name"].ToString() ?? "",
                    Price = Convert.ToDouble(row["Price"]),
                    Stock = Convert.ToInt32(row["Stock"]),
                    MinStock = Convert.ToInt32(row["MinStock"]),
                    Category = row["Category"].ToString(),
                    Description = row["Description"].ToString(),
                    CreatedAt = Convert.ToDateTime(row["CreatedAt"])
                });
            }
            return items;
        }

        public List<Item> GetLowStockItems()
        {
            var items = new List<Item>();
            var sql = "SELECT * FROM Items WHERE Stock <= MinStock ORDER BY Stock";
            var dataTable = ExecuteQuery(sql);

            foreach (DataRow row in dataTable.Rows)
            {
                items.Add(new Item
                {
                    ItemID = Convert.ToInt32(row["ItemID"]),
                    Name = row["Name"].ToString() ?? "",
                    Price = Convert.ToDouble(row["Price"]),
                    Stock = Convert.ToInt32(row["Stock"]),
                    MinStock = Convert.ToInt32(row["MinStock"]),
                    Category = row["Category"].ToString(),
                    Description = row["Description"].ToString()
                });
            }
            return items;
        }

        public void AddItem(Item item)
        {
            var sql = @"INSERT INTO Items (Name, Price, Stock, MinStock, Category, Description) 
                       VALUES (@name, @price, @stock, @minStock, @category, @description)";
            var parameters = new[]
            {
                new SQLiteParameter("@name", item.Name),
                new SQLiteParameter("@price", item.Price),
                new SQLiteParameter("@stock", item.Stock),
                new SQLiteParameter("@minStock", item.MinStock),
                new SQLiteParameter("@category", item.Category ?? ""),
                new SQLiteParameter("@description", item.Description ?? "")
            };
            ExecuteNonQuery(sql, parameters);
        }

        public void UpdateItemStock(int itemId, int newStock)
        {
            var sql = "UPDATE Items SET Stock = @stock, UpdatedAt = CURRENT_TIMESTAMP WHERE ItemID = @itemId";
            var parameters = new[]
            {
                new SQLiteParameter("@stock", newStock),
                new SQLiteParameter("@itemId", itemId)
            };
            ExecuteNonQuery(sql, parameters);
        }

        // 進貨相關操作
        public void AddPurchase(Purchase purchase)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                // 新增進貨記錄
                var sql = @"INSERT INTO Purchases (Date, Supplier, ItemID, Quantity, UnitPrice, TotalAmount, Notes) 
                           VALUES (@date, @supplier, @itemId, @quantity, @unitPrice, @totalAmount, @notes)";
                using var command = new SQLiteCommand(sql, connection, transaction);
                command.Parameters.AddRange(new[]
                {
                    new SQLiteParameter("@date", purchase.Date),
                    new SQLiteParameter("@supplier", purchase.Supplier),
                    new SQLiteParameter("@itemId", purchase.ItemID),
                    new SQLiteParameter("@quantity", purchase.Quantity),
                    new SQLiteParameter("@unitPrice", purchase.UnitPrice),
                    new SQLiteParameter("@totalAmount", purchase.TotalAmount),
                    new SQLiteParameter("@notes", purchase.Notes ?? "")
                });
                command.ExecuteNonQuery();

                // 更新庫存
                var updateStockSql = "UPDATE Items SET Stock = Stock + @quantity WHERE ItemID = @itemId";
                using var updateCommand = new SQLiteCommand(updateStockSql, connection, transaction);
                updateCommand.Parameters.AddRange(new[]
                {
                    new SQLiteParameter("@quantity", purchase.Quantity),
                    new SQLiteParameter("@itemId", purchase.ItemID)
                });
                updateCommand.ExecuteNonQuery();

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        // 銷售相關操作
        public void AddSale(Sale sale)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                // 檢查庫存
                var checkStockSql = "SELECT Stock FROM Items WHERE ItemID = @itemId";
                using var checkCommand = new SQLiteCommand(checkStockSql, connection, transaction);
                checkCommand.Parameters.Add(new SQLiteParameter("@itemId", sale.ItemID));
                var currentStock = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (currentStock < sale.Quantity)
                {
                    throw new InvalidOperationException($"庫存不足！目前庫存：{currentStock}，需要：{sale.Quantity}");
                }

                // 新增銷售記錄
                var sql = @"INSERT INTO Sales (Date, Customer, ItemID, Quantity, UnitPrice, TotalAmount, Discount, FinalAmount, Notes) 
                           VALUES (@date, @customer, @itemId, @quantity, @unitPrice, @totalAmount, @discount, @finalAmount, @notes)";
                using var command = new SQLiteCommand(sql, connection, transaction);
                command.Parameters.AddRange(new[]
                {
                    new SQLiteParameter("@date", sale.Date),
                    new SQLiteParameter("@customer", sale.Customer ?? ""),
                    new SQLiteParameter("@itemId", sale.ItemID),
                    new SQLiteParameter("@quantity", sale.Quantity),
                    new SQLiteParameter("@unitPrice", sale.UnitPrice),
                    new SQLiteParameter("@totalAmount", sale.TotalAmount),
                    new SQLiteParameter("@discount", sale.Discount),
                    new SQLiteParameter("@finalAmount", sale.FinalAmount),
                    new SQLiteParameter("@notes", sale.Notes ?? "")
                });
                command.ExecuteNonQuery();

                // 更新庫存
                var updateStockSql = "UPDATE Items SET Stock = Stock - @quantity WHERE ItemID = @itemId";
                using var updateCommand = new SQLiteCommand(updateStockSql, connection, transaction);
                updateCommand.Parameters.AddRange(new[]
                {
                    new SQLiteParameter("@quantity", sale.Quantity),
                    new SQLiteParameter("@itemId", sale.ItemID)
                });
                updateCommand.ExecuteNonQuery();

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        // 統計相關操作
        public double GetTodaySalesAmount()
        {
            var sql = "SELECT COALESCE(SUM(FinalAmount), 0) FROM Sales WHERE DATE(Date) = DATE('now')";
            var result = ExecuteScalar(sql);
            return Convert.ToDouble(result);
        }

        public double GetTodayPurchaseAmount()
        {
            var sql = "SELECT COALESCE(SUM(TotalAmount), 0) FROM Purchases WHERE DATE(Date) = DATE('now')";
            var result = ExecuteScalar(sql);
            return Convert.ToDouble(result);
        }

        public List<string> GetSuppliers()
        {
            var suppliers = new List<string>();
            var sql = "SELECT Name FROM Suppliers WHERE IsActive = 1 ORDER BY Name";
            var dataTable = ExecuteQuery(sql);

            foreach (DataRow row in dataTable.Rows)
            {
                suppliers.Add(row["Name"].ToString() ?? "");
            }
            return suppliers;
        }
    }
}