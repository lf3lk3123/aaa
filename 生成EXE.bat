@echo off
chcp 65001 >nul
echo.
echo ========================================
echo    📦 生成獨立 EXE 文件
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

echo 🔨 清理舊的發布文件...
if exist "publish" rmdir /s /q "publish"
echo ✅ 清理完成
echo.

echo 📦 生成獨立 EXE 文件...
echo    目標平台: Windows x64
echo    配置: Release
echo    包含 .NET 運行時: 是
echo.

dotnet publish --configuration Release --runtime win-x64 --self-contained true --output ./publish --verbosity minimal

if %errorlevel% neq 0 (
    echo ❌ EXE 生成失敗
    pause
    exit /b 1
)

echo.
echo ========================================
echo    🎉 EXE 文件生成成功！
echo ========================================
echo.
echo 📁 文件位置: %cd%\publish\MyWindowsApp.exe
echo 📊 文件大小: 
for %%A in ("publish\MyWindowsApp.exe") do echo    %%~zA 字節
echo.
echo 💡 使用說明:
echo    1. 可以將整個 publish 文件夾複製到任何 Windows 電腦
echo    2. 雙擊 MyWindowsApp.exe 即可運行
echo    3. 不需要安裝 .NET 運行時
echo    4. 數據庫文件會在程序目錄下自動創建
echo.

echo 🚀 是否現在運行生成的 EXE 文件？ (Y/N)
set /p choice=請選擇: 
if /i "%choice%"=="Y" (
    echo.
    echo 🚀 啟動應用程序...
    start "" "publish\MyWindowsApp.exe"
    echo ✅ 應用程序已啟動
) else (
    echo ✅ 您可以稍後手動運行 publish\MyWindowsApp.exe
)

echo.
pause