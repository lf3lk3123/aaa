@echo off
chcp 65001 >nul
echo.
echo ========================================
echo    🚀 庫存管理系統 - 快速啟動
echo ========================================
echo.

echo 📋 檢查 .NET 8.0 SDK...
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ❌ 錯誤: 未找到 .NET 8.0 SDK
    echo.
    echo 請先安裝 .NET 8.0 SDK:
    echo https://dotnet.microsoft.com/download/dotnet/8.0
    echo.
    pause
    exit /b 1
)

echo ✅ .NET SDK 已安裝
echo.

echo 📦 還原 NuGet 套件...
dotnet restore
if %errorlevel% neq 0 (
    echo ❌ 套件還原失敗
    pause
    exit /b 1
)

echo ✅ 套件還原完成
echo.

echo 🔨 編譯應用程序...
dotnet build --configuration Release
if %errorlevel% neq 0 (
    echo ❌ 編譯失敗
    pause
    exit /b 1
)

echo ✅ 編譯成功
echo.

echo 🚀 啟動庫存管理系統...
echo.
echo ================================================
echo  系統正在啟動，請稍候...
echo  首次運行會自動創建數據庫
echo  關閉此窗口將停止應用程序
echo ================================================
echo.

dotnet run

echo.
echo 📊 應用程序已關閉
pause