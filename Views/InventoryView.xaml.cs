using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;

namespace InventoryManagementSystem.Views
{
    public partial class InventoryView : UserControl
    {
        private readonly DatabaseHelper _databaseHelper;
        private readonly SpeechService _speechService;
        private List<Item> _allItems;

        public InventoryView(DatabaseHelper databaseHelper, SpeechService speechService)
        {
            InitializeComponent();
            _databaseHelper = databaseHelper;
            _speechService = speechService;
            _allItems = new List<Item>();
            LoadItems();
        }

        private void LoadItems()
        {
            try
            {
                _allItems = _databaseHelper.GetAllItems();
                ItemsDataGrid.ItemsSource = _allItems;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"載入商品資料失敗: {ex.Message}", "錯誤", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new AddItemDialog();
                if (dialog.ShowDialog() == true)
                {
                    var newItem = dialog.Item;
                    var success = await Task.Run(() => _databaseHelper.AddItem(newItem));
                    
                    if (success)
                    {
                        LoadItems();
                        _speechService.Speak($"商品 {newItem.Name} 已新增");
                        MessageBox.Show("商品新增成功！", "成功", 
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("商品新增失敗！", "錯誤", 
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"新增商品時發生錯誤：{ex.Message}", "錯誤", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await LoadItemsAsync();
            _speechService.Speak("商品資料已重新整理");
        }

        private async Task LoadItemsAsync()
        {
            try
            {
                _allItems = await Task.Run(() => _databaseHelper.GetAllItems());
                ItemsDataGrid.ItemsSource = _allItems;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"載入商品資料失敗：{ex.Message}", "錯誤", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = SearchTextBox.Text.ToLower();
            if (string.IsNullOrWhiteSpace(searchText))
            {
                ItemsDataGrid.ItemsSource = _allItems;
            }
            else
            {
                var filteredItems = _allItems.Where(item => 
                    item.Name.ToLower().Contains(searchText) ||
                    (item.Category?.ToLower().Contains(searchText) ?? false) ||
                    (item.Description?.ToLower().Contains(searchText) ?? false)
                ).ToList();
                
                ItemsDataGrid.ItemsSource = filteredItems;
            }
        }
    }
}