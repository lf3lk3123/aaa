# Windows EXE Application Development Framework

本仓库包含了多種不同技術棧開發 Windows exe 應用程式的完整示例，包括基礎範例和完整的商業應用程式。

## 🏗️ 專案結構

### 基礎範例
- **Python + tkinter** - 簡單易學的跨平台解決方案
- **C# Windows Forms** - 高效能的原生 Windows 應用程式
- **Electron + JavaScript** - 現代化的 Web 技術桌面應用

### 完整商業應用程式
- **進銷存管理系統** - 使用 C# WPF + SQLite 的完整商業應用程式

## 📋 目錄結構

```
aaa/
├── 📁 基礎範例/
│   ├── main.py                          # Python + tkinter 版本
│   ├── build_exe.py                     # Python 打包腳本
│   ├── requirements.txt                 # Python 依賴
│   ├── Program.cs                       # C# Windows Forms 版本
│   ├── MyWindowsApp.csproj             # C# 專案檔案
│   ├── package.json                     # Electron 專案配置
│   ├── main.js                          # Electron 主進程
│   ├── index.html                       # Electron 界面
│   ├── style.css                        # Electron 樣式
│   └── renderer.js                      # Electron 渲染進程
│
├── 📁 進銷存管理系統/
│   ├── InventoryManagementSystem.csproj # WPF 專案檔案
│   ├── MainWindow.xaml                  # 主視窗
│   ├── App.xaml                         # 應用程式入口
│   ├── 📂 Data/                         # 資料存取層
│   │   └── DatabaseHelper.cs           # SQLite 資料庫操作
│   ├── 📂 Models/                       # 資料模型
│   │   ├── Item.cs                      # 商品模型
│   │   ├── Purchase.cs                  # 進貨模型
│   │   └── Sale.cs                      # 銷售模型
│   ├── 📂 Services/                     # 服務層
│   │   └── SpeechService.cs             # 語音服務
│   └── 📂 Views/                        # 視圖層
│       ├── InventoryView.xaml           # 庫存管理界面
│       ├── PurchaseView.xaml            # 進貨管理界面
│       └── SalesView.xaml               # 銷售管理界面
└── README.md                            # 本文檔
```

## 🚀 方法一：Python + PyInstaller

### 特點
- ✅ 簡單易學，適合初學者
- ✅ 跨平台支援
- ✅ 豐富的第三方庫
- ❌ 打包檔案較大
- ❌ 啟動速度相對較慢

### 安裝依賴
```bash
pip install -r requirements.txt
```

### 執行開發版本
```bash
python main.py
```

### 打包成 exe
```bash
python build_exe.py
```

### 手動打包命令
```bash
pyinstaller --onefile --windowed --name=MyWindowsApp main.py
```

生成的檔案位於 `dist/` 目錄中。

## 🚀 方法二：C# + .NET

### 特點
- ✅ 效能優秀
- ✅ 與 Windows 系統整合度高
- ✅ 打包檔案小
- ✅ 啟動速度快
- ❌ 主要限於 Windows 平台
- ❌ 需要 .NET 執行時

### 安裝 .NET SDK
下載並安裝 [.NET 6.0 SDK](https://dotnet.microsoft.com/download)

### 執行開發版本
```bash
dotnet run
```

### 編譯發布版本
```bash
# 編譯為依賴框架的版本
dotnet publish -c Release

# 編譯為自包含版本（包含執行時）
dotnet publish -c Release --self-contained true -r win-x64

# 編譯為單檔案版本
dotnet publish -c Release --self-contained true -r win-x64 -p:PublishSingleFile=true
```

## 🚀 方法三：Electron + JavaScript

### 特點
- ✅ 使用 Web 技術開發
- ✅ 跨平台支援
- ✅ 界面美觀，易於定制
- ✅ 開發效率高
- ❌ 打包檔案很大
- ❌ 記憶體佔用較高

### 安裝依賴
```bash
npm install
```

### 執行開發版本
```bash
npm start
```

### 打包成 exe
```bash
# 僅打包 Windows 版本
npm run build-win

# 打包所有平台
npm run build
```

生成的檔案位於 `dist/` 目錄中。

## 🏪 方法四：完整商業應用程式 - 進銷存管理系統

### 特點
- ✅ 完整的商業邏輯
- ✅ Material Design 現代化界面
- ✅ SQLite 本地資料庫
- ✅ 語音提示功能
- ✅ 深色/淺色主題切換
- ✅ 即時庫存監控和警示

### 核心功能
- **📦 庫存管理** - 商品資料管理、庫存監控、低庫存警示
- **📥 進貨管理** - 進貨登記、供應商管理、庫存自動更新
- **💰 銷售管理** - 銷售登記、客戶管理、庫存自動扣減
- **👥 會員管理** - 會員資料、積分系統（架構已建立）
- **📊 統計報表** - 即時銷售統計、進貨分析（架構已建立）

### 快速開始
```bash
# 還原套件
dotnet restore

# 編譯專案
dotnet build

# 執行應用程式
dotnet run

# 打包為 EXE
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

### 技術架構
- **前端**: WPF + Material Design In XAML
- **資料庫**: SQLite 本地資料庫
- **語音**: System.Speech.Synthesis
- **框架**: .NET 6.0

## 🛠️ 開發環境設置

### Windows 開發環境
1. **Python 環境**：安裝 Python 3.8+
2. **C# 環境**：安裝 Visual Studio 或 .NET SDK
3. **Node.js 環境**：安裝 Node.js 16+

### Linux/macOS 交叉編譯
雖然在 Linux/macOS 上也可以為 Windows 打包應用程式：

```bash
# Python (PyInstaller 支援交叉編譯，但有限制)
# C# (.NET 支援交叉編譯)
dotnet publish -c Release -r win-x64

# Electron (完全支援交叉編譯)
npm run build-win
```

## 📦 打包優化建議

### Python 優化
- 使用 `--exclude-module` 排除不需要的模組
- 使用 UPX 壓縮可執行檔案
- 考慮使用 Nuitka 替代 PyInstaller

### C# 優化
- 使用 AOT 編譯提高啟動速度
- 啟用程式碼裁剪減小檔案大小
- 使用 ReadyToRun 映像

### Electron 優化
- 使用 `electron-builder` 的壓縮選項
- 排除開發依賴
- 使用 V8 快照提高啟動速度

## 🎯 選擇建議

| 場景 | 推薦技術 | 理由 |
|------|----------|------|
| 快速原型 | Python | 開發速度快，學習成本低 |
| 企業應用 | C# WPF | 效能好，與 Windows 整合度高 |
| 現代界面 | Electron | 界面美觀，Web 技術棧 |
| 跨平台 | Python/Electron | 一次開發，多平台執行 |
| 效能要求高 | C# | 原生效能，資源佔用少 |
| 完整商業應用 | C# WPF + SQLite | 完整功能，專業界面，本地資料庫 |

## 🔧 常見問題

### Python 打包問題
- **問題**：缺少模組
- **解決**：使用 `--hidden-import` 參數

### C# 編譯問題
- **問題**：缺少 .NET 執行時
- **解決**：使用自包含發布

### Electron 打包問題
- **問題**：檔案過大
- **解決**：配置 `files` 欄位，排除不需要的檔案

## 📝 授權條款

MIT License - 可自由使用和修改。

## 🤝 貢獻

歡迎提交 Issue 和 Pull Request！

---

**開發者**: OpenHands  
**日期**: 2025-07-04  
**版本**: 2.0.0
