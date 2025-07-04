using System;
using System.Windows;
using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.Views
{
    public partial class AddItemDialog : Window
    {
        public Item Item { get; private set; }

        public AddItemDialog()
        {
            InitializeComponent();
            Item = new Item();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 驗證輸入
                if (string.IsNullOrWhiteSpace(NameTextBox.Text))
                {
                    MessageBox.Show("請輸入商品名稱", "驗證錯誤", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    NameTextBox.Focus();
                    return;
                }

                if (!double.TryParse(PriceTextBox.Text, out double price) || price < 0)
                {
                    MessageBox.Show("請輸入有效的價格", "驗證錯誤", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    PriceTextBox.Focus();
                    return;
                }

                if (!int.TryParse(StockTextBox.Text, out int stock) || stock < 0)
                {
                    MessageBox.Show("請輸入有效的庫存數量", "驗證錯誤", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    StockTextBox.Focus();
                    return;
                }

                if (!int.TryParse(MinStockTextBox.Text, out int minStock) || minStock < 0)
                {
                    MessageBox.Show("請輸入有效的最低庫存", "驗證錯誤", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    MinStockTextBox.Focus();
                    return;
                }

                // 建立商品物件
                Item = new Item
                {
                    Name = NameTextBox.Text.Trim(),
                    Price = price,
                    Stock = stock,
                    MinStock = minStock,
                    Category = CategoryComboBox.Text?.Trim(),
                    Description = DescriptionTextBox.Text?.Trim()
                };

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"儲存時發生錯誤：{ex.Message}", "錯誤", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}