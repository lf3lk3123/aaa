using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;

namespace InventoryManagementSystem.Views
{
    public partial class SalesView : UserControl
    {
        private readonly DatabaseHelper _databaseHelper;
        private readonly SpeechService _speechService;
        private Item? _selectedItem;

        public SalesView(DatabaseHelper databaseHelper, SpeechService speechService)
        {
            InitializeComponent();
            _databaseHelper = databaseHelper;
            _speechService = speechService;
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var items = _databaseHelper.GetAllItems();
                ItemComboBox.ItemsSource = items;
                ItemComboBox.DisplayMemberPath = "Name";
                ItemComboBox.SelectedValuePath = "ItemID";

                DatePicker.SelectedDate = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"載入資料失敗: {ex.Message}", "錯誤", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ItemComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ItemComboBox.SelectedItem is Item selectedItem)
            {
                _selectedItem = selectedItem;
                UnitPriceTextBox.Text = selectedItem.Price.ToString("F2");
                
                // 顯示庫存資訊
                StockInfoText.Text = $"目前庫存：{selectedItem.Stock} 件";
                
                if (selectedItem.IsLowStock)
                {
                    StockInfoText.Foreground = System.Windows.Media.Brushes.Red;
                    StockInfoText.Text += " (庫存不足)";
                }
                else
                {
                    StockInfoText.Foreground = System.Windows.Media.Brushes.Gray;
                }
                
                CalculateAmounts();
            }
            else
            {
                _selectedItem = null;
                StockInfoText.Text = "";
                UnitPriceTextBox.Text = "";
            }
        }

        private void QuantityTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateAmounts();
        }

        private void UnitPriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateAmounts();
        }

        private void DiscountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateAmounts();
        }

        private void CalculateAmounts()
        {
            if (int.TryParse(QuantityTextBox.Text, out int quantity) &&
                double.TryParse(UnitPriceTextBox.Text, out double unitPrice))
            {
                var subtotal = quantity * unitPrice;
                SubtotalText.Text = $"NT$ {subtotal:F2}";

                double.TryParse(DiscountTextBox.Text, out double discount);
                var finalAmount = subtotal - discount;
                FinalAmountText.Text = $"NT$ {finalAmount:F2}";
            }
            else
            {
                SubtotalText.Text = "NT$ 0.00";
                FinalAmountText.Text = "NT$ 0.00";
            }
        }

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 驗證輸入
                if (_selectedItem == null)
                {
                    MessageBox.Show("請選擇商品", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity <= 0)
                {
                    MessageBox.Show("請輸入有效的數量", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!double.TryParse(UnitPriceTextBox.Text, out double unitPrice) || unitPrice <= 0)
                {
                    MessageBox.Show("請輸入有效的單價", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // 檢查庫存
                if (quantity > _selectedItem.Stock)
                {
                    MessageBox.Show($"庫存不足！目前庫存：{_selectedItem.Stock} 件，需要：{quantity} 件", 
                        "庫存不足", MessageBoxButton.OK, MessageBoxImage.Warning);
                    _speechService.SpeakError("庫存不足");
                    return;
                }

                double.TryParse(DiscountTextBox.Text, out double discount);
                var totalAmount = quantity * unitPrice;
                var finalAmount = totalAmount - discount;

                // 創建銷售記錄
                var sale = new Sale
                {
                    Date = DatePicker.SelectedDate ?? DateTime.Now,
                    Customer = CustomerTextBox.Text,
                    ItemID = _selectedItem.ItemID,
                    Quantity = quantity,
                    UnitPrice = unitPrice,
                    TotalAmount = totalAmount,
                    Discount = discount,
                    FinalAmount = finalAmount,
                    Notes = NotesTextBox.Text
                };

                // 保存到資料庫
                var success = await Task.Run(() => _databaseHelper.AddSale(sale));
                
                if (!success)
                {
                    MessageBox.Show("銷售記錄新增失敗！", "錯誤", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    _speechService.SpeakError("銷售記錄新增失敗");
                    return;
                }

                // 語音提示
                _speechService.SpeakSaleComplete(_selectedItem.Name, quantity, finalAmount);

                MessageBox.Show("銷售記錄已成功新增！", "成功", 
                    MessageBoxButton.OK, MessageBoxImage.Information);

                // 重新載入商品資料以更新庫存顯示
                LoadData();
                
                // 清空表單
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"新增銷售記錄失敗: {ex.Message}", "錯誤", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                _speechService.SpeakError("銷售記錄新增失敗");
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            ItemComboBox.SelectedIndex = -1;
            CustomerTextBox.Text = "";
            QuantityTextBox.Text = "";
            UnitPriceTextBox.Text = "";
            DiscountTextBox.Text = "";
            NotesTextBox.Text = "";
            SubtotalText.Text = "NT$ 0.00";
            FinalAmountText.Text = "NT$ 0.00";
            StockInfoText.Text = "";
            DatePicker.SelectedDate = DateTime.Now;
            _selectedItem = null;
        }
    }
}