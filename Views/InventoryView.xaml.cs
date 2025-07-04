using System;
using System.Collections.Generic;
using System.Linq;
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

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 開啟新增商品對話框
            MessageBox.Show("新增商品功能開發中", "提示", 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadItems();
            _speechService.Speak("商品資料已重新整理");
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