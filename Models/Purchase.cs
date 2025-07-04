using System;
using System.ComponentModel;

namespace InventoryManagementSystem.Models
{
    public class Purchase : INotifyPropertyChanged
    {
        private int _purchaseID;
        private DateTime _date;
        private string _supplier = "";
        private int _itemID;
        private string _itemName = "";
        private int _quantity;
        private double _unitPrice;
        private double _totalAmount;
        private string? _notes;
        private DateTime _createdAt;

        public int PurchaseID
        {
            get => _purchaseID;
            set
            {
                _purchaseID = value;
                OnPropertyChanged(nameof(PurchaseID));
            }
        }

        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
                OnPropertyChanged(nameof(DateFormatted));
            }
        }

        public string Supplier
        {
            get => _supplier;
            set
            {
                _supplier = value;
                OnPropertyChanged(nameof(Supplier));
            }
        }

        public int ItemID
        {
            get => _itemID;
            set
            {
                _itemID = value;
                OnPropertyChanged(nameof(ItemID));
            }
        }

        public string ItemName
        {
            get => _itemName;
            set
            {
                _itemName = value;
                OnPropertyChanged(nameof(ItemName));
            }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
                CalculateTotalAmount();
            }
        }

        public double UnitPrice
        {
            get => _unitPrice;
            set
            {
                _unitPrice = value;
                OnPropertyChanged(nameof(UnitPrice));
                OnPropertyChanged(nameof(UnitPriceFormatted));
                CalculateTotalAmount();
            }
        }

        public double TotalAmount
        {
            get => _totalAmount;
            set
            {
                _totalAmount = value;
                OnPropertyChanged(nameof(TotalAmount));
                OnPropertyChanged(nameof(TotalAmountFormatted));
            }
        }

        public string? Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                OnPropertyChanged(nameof(Notes));
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

        // 格式化屬性
        public string DateFormatted => Date.ToString("yyyy/MM/dd HH:mm");
        public string UnitPriceFormatted => $"NT$ {UnitPrice:F2}";
        public string TotalAmountFormatted => $"NT$ {TotalAmount:F2}";

        private void CalculateTotalAmount()
        {
            TotalAmount = Quantity * UnitPrice;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}