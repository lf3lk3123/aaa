using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using MaterialDesignThemes.Wpf;
using InventoryManagementSystem.Data;
using InventoryManagementSystem.Services;
using InventoryManagementSystem.Views;

namespace InventoryManagementSystem
{
    public partial class MainWindow : Window
    {
        private readonly DatabaseHelper _databaseHelper;
        private readonly SpeechService _speechService;
        private readonly DispatcherTimer _timer;
        private bool _isDarkTheme = false;

        public MainWindow()
        {
            InitializeComponent();
            
            _databaseHelper = new DatabaseHelper();
            _speechService = new SpeechService();
            
            // 設置定時器更新時間和統計
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();

            // 初始化界面
            LoadStatistics();
            CheckLowStock();
            
            // 語音歡迎
            _speechService.SpeakWelcome();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeText.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }

        private void LoadStatistics()
        {
            try
            {
                var todaySales = _databaseHelper.GetTodaySalesAmount();
                var todayPurchases = _databaseHelper.GetTodayPurchaseAmount();
                var allItems = _databaseHelper.GetAllItems();
                var lowStockItems = _databaseHelper.GetLowStockItems();

                TodaySalesText.Text = $"NT$ {todaySales:N0}";
                TodayPurchasesText.Text = $"NT$ {todayPurchases:N0}";
                TotalItemsText.Text = $"{allItems.Count} 項";
                LowStockText.Text = $"{lowStockItems.Count} 項";

                StatusText.Text = "統計資料已更新";
            }
            catch (Exception ex)
            {
                StatusText.Text = $"載入統計資料失敗: {ex.Message}";
            }
        }

        private void CheckLowStock()
        {
            try
            {
                var lowStockItems = _databaseHelper.GetLowStockItems();
                if (lowStockItems.Any())
                {
                    LowStockAlert.Visibility = Visibility.Visible;
                    var message = $"發現 {lowStockItems.Count} 項商品庫存不足：\n";
                    message += string.Join("\n", lowStockItems.Take(3).Select(item => 
                        $"• {item.Name} (剩餘 {item.Stock} 件)"));
                    
                    if (lowStockItems.Count > 3)
                    {
                        message += $"\n... 還有 {lowStockItems.Count - 3} 項";
                    }

                    LowStockMessage.Text = message;

                    // 語音提醒
                    _speechService.Speak($"注意！發現 {lowStockItems.Count} 項商品庫存不足");
                }
                else
                {
                    LowStockAlert.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                StatusText.Text = $"檢查庫存失敗: {ex.Message}";
            }
        }

        private void InventoryButton_Click(object sender, RoutedEventArgs e)
        {
            ContentTitle.Text = "庫存管理";
            MainContent.Content = new InventoryView(_databaseHelper, _speechService);
            StatusText.Text = "已切換到庫存管理";
        }

        private void PurchaseButton_Click(object sender, RoutedEventArgs e)
        {
            ContentTitle.Text = "進貨管理";
            MainContent.Content = new PurchaseView(_databaseHelper, _speechService);
            StatusText.Text = "已切換到進貨管理";
        }

        private void SalesButton_Click(object sender, RoutedEventArgs e)
        {
            ContentTitle.Text = "銷售管理";
            MainContent.Content = new SalesView(_databaseHelper, _speechService);
            StatusText.Text = "已切換到銷售管理";
        }

        private void MemberButton_Click(object sender, RoutedEventArgs e)
        {
            ContentTitle.Text = "會員管理";
            // TODO: 實現會員管理視圖
            StatusText.Text = "會員管理功能開發中";
        }

        private void ReportsButton_Click(object sender, RoutedEventArgs e)
        {
            ContentTitle.Text = "報表分析";
            // TODO: 實現報表分析視圖
            StatusText.Text = "報表分析功能開發中";
        }

        private void ThemeToggleButton_Click(object sender, RoutedEventArgs e)
        {
            _isDarkTheme = !_isDarkTheme;
            
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();
            theme.SetBaseTheme(_isDarkTheme ? Theme.Dark : Theme.Light);
            paletteHelper.SetTheme(theme);

            StatusText.Text = $"已切換到{(_isDarkTheme ? "深色" : "淺色")}主題";
            _speechService.Speak($"已切換到{(_isDarkTheme ? "深色" : "淺色")}主題");
        }

        private void SpeechToggleButton_Click(object sender, RoutedEventArgs e)
        {
            _speechService.IsEnabled = !_speechService.IsEnabled;
            SpeechIcon.Kind = _speechService.IsEnabled ? PackIconKind.VolumeHigh : PackIconKind.VolumeOff;
            
            StatusText.Text = $"語音提示已{(_speechService.IsEnabled ? "開啟" : "關閉")}";
            
            if (_speechService.IsEnabled)
            {
                _speechService.Speak("語音提示已開啟");
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 實現設定視窗
            StatusText.Text = "設定功能開發中";
        }

        protected override void OnClosed(EventArgs e)
        {
            _timer?.Stop();
            _speechService?.Dispose();
            base.OnClosed(e);
        }
    }
}