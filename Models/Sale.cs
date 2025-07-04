using System;
using System.ComponentModel;

namespace InventoryManagementSystem.Models
{
    public class Sale : INotifyPropertyChanged
    {
        private int _saleID;
        private DateTime _date;
        private string? _customer;
        private int _itemID;
        private string _itemName = "";
        private int _quantity;
        private double _unitPrice;
        private double _totalAmount;
        private double _discount;
        private double _finalAmount;
        private string? _notes;
        private DateTime _createdAt;

        public int SaleID
        {
            get => _saleID;
            set
            {
                _saleID = value;
                OnPropertyChanged(nameof(SaleID));
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

        public string? Customer
        {
            get => _customer;
            set
            {
                _customer = value;
                OnPropertyChanged(nameof(Customer));
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
                CalculateAmounts();
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
                CalculateAmounts();
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

        public double Discount
        {
            get => _discount;
            set
            {
                _discount = value;
                OnPropertyChanged(nameof(Discount));
                OnPropertyChanged(nameof(DiscountFormatted));
                CalculateAmounts();
            }
        }

        public double FinalAmount
        {
            get => _finalAmount;
            set
            {
                _finalAmount = value;
                OnPropertyChanged(nameof(FinalAmount));
                OnPropertyChanged(nameof(FinalAmountFormatted));
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
        public string DiscountFormatted => $"NT$ {Discount:F2}";
        public string FinalAmountFormatted => $"NT$ {FinalAmount:F2}";

        private void CalculateAmounts()
        {
            TotalAmount = Quantity * UnitPrice;
            FinalAmount = TotalAmount - Discount;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}