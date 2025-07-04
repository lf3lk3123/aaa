using System;
using System.ComponentModel;

namespace InventoryManagementSystem.Models
{
    public class Item : INotifyPropertyChanged
    {
        private int _itemID;
        private string _name = "";
        private double _price;
        private int _stock;
        private int _minStock;
        private string? _category;
        private string? _description;
        private DateTime _createdAt;
        private DateTime _updatedAt;

        public int ItemID
        {
            get => _itemID;
            set
            {
                _itemID = value;
                OnPropertyChanged(nameof(ItemID));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public double Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        public int Stock
        {
            get => _stock;
            set
            {
                _stock = value;
                OnPropertyChanged(nameof(Stock));
                OnPropertyChanged(nameof(IsLowStock));
                OnPropertyChanged(nameof(StockStatus));
            }
        }

        public int MinStock
        {
            get => _minStock;
            set
            {
                _minStock = value;
                OnPropertyChanged(nameof(MinStock));
                OnPropertyChanged(nameof(IsLowStock));
                OnPropertyChanged(nameof(StockStatus));
            }
        }

        public string? Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged(nameof(Category));
            }
        }

        public string? Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set
            {
                _createdAt = value;
                OnPropertyChanged(nameof(CreatedAt));
            }
        }

        public DateTime UpdatedAt
        {
            get => _updatedAt;
            set
            {
                _updatedAt = value;
                OnPropertyChanged(nameof(UpdatedAt));
            }
        }

        // 計算屬性
        public bool IsLowStock => Stock <= MinStock;

        public string StockStatus
        {
            get
            {
                if (Stock == 0) return "缺貨";
                if (Stock <= MinStock) return "庫存不足";
                return "正常";
            }
        }

        public string PriceFormatted => $"NT$ {Price:F2}";

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}