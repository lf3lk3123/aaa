using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;

namespace InventoryManagementSystem.Views
{
    public partial class PurchaseView : UserControl
    {
        private readonly DatabaseHelper _databaseHelper;
        private readonly SpeechService _speechService;

        public PurchaseView(DatabaseHelper databaseHelper, SpeechService speechService)
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

                var suppliers = _databaseHelper.GetSuppliers();
                SupplierComboBox.ItemsSource = suppliers;

                DatePicker.SelectedDate = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"載入資料失敗: {ex.Message}", "錯誤", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void QuantityTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateTotal();
        }

        private void UnitPriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateTotal();
        }

        private void CalculateTotal()
        {
            if (int.TryParse(QuantityTextBox.Text, out int quantity) &&
                double.TryParse(UnitPriceTextBox.Text, out double unitPrice))
            {
                var total = quantity * unitPrice;
                TotalAmountText.Text = $"NT$ {total:F2}";
            }
            else
            {
                TotalAmountText.Text = "NT$ 0.00";
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 驗證輸入
                if (ItemComboBox.SelectedValue == null)
                {
                    MessageBox.Show("請選擇商品", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(SupplierComboBox.Text))
                {
                    MessageBox.Show("請選擇或輸入供應商", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
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

                // 創建進貨記錄
                var purchase = new Purchase
                {
                    Date = DatePicker.SelectedDate ?? DateTime.Now,
                    Supplier = SupplierComboBox.Text,
                    ItemID = (int)ItemComboBox.SelectedValue,
                    Quantity = quantity,
                    UnitPrice = unitPrice,
                    TotalAmount = quantity * unitPrice,
                    Notes = NotesTextBox.Text
                };

                // 保存到資料庫
                _databaseHelper.AddPurchase(purchase);

                // 獲取商品名稱用於語音提示
                var selectedItem = (Item)ItemComboBox.SelectedItem;
                _speechService.SpeakPurchaseComplete(selectedItem.Name, quantity);

                MessageBox.Show("進貨記錄已成功新增！", "成功", 
                    MessageBoxButton.OK, MessageBoxImage.Information);

                // 清空表單
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"新增進貨記錄失敗: {ex.Message}", "錯誤", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                _speechService.SpeakError("進貨記錄新增失敗");
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            ItemComboBox.SelectedIndex = -1;
            SupplierComboBox.Text = "";
            QuantityTextBox.Text = "";
            UnitPriceTextBox.Text = "";
            NotesTextBox.Text = "";
            TotalAmountText.Text = "NT$ 0.00";
            DatePicker.SelectedDate = DateTime.Now;
        }
    }
}