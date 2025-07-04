@echo off
chcp 65001 >nul
title 🎉 Windows 應用程式快速啟動器

echo ========================================
echo    🎉 Windows 應用程式快速啟動器
echo ========================================
echo.

:menu
echo 請選擇要運行的應用程式版本:
echo.
echo 1. 🐍 Python 版本 (推薦新手)
echo 2. 🔷 C# 版本 (推薦企業用戶)  
echo 3. ⚡ Electron 版本 (推薦現代 UI)
echo 4. 📖 查看使用指南
echo 5. 🛠️ 打開構建工具
echo 0. ❌ 退出
echo.
set /p choice="請輸入選擇 (0-5): "

if "%choice%"=="1" goto run_python
if "%choice%"=="2" goto run_csharp
if "%choice%"=="3" goto run_electron
if "%choice%"=="4" goto show_guide
if "%choice%"=="5" goto build_tools
if "%choice%"=="0" goto exit
echo 無效選擇，請重新輸入。
echo.
goto menu

:run_python
echo.
echo 🐍 啟動 Python 版本...
echo.
python --version >nul 2>&1
if errorlevel 1 (
    echo ❌ 錯誤: 未找到 Python
    echo 請先安裝 Python: https://www.python.org/downloads/
    echo.
    pause
    goto menu
)

echo 檢查依賴...
pip install -r requirements.txt >nul 2>&1

echo 啟動應用程式...
python main.py

echo.
pause
goto menu

:run_csharp
echo.
echo 🔷 啟動 C# 版本...
echo.
dotnet --version >nul 2>&1
if errorlevel 1 (
    echo ❌ 錯誤: 未找到 .NET SDK
    echo 請先安裝 .NET: https://dotnet.microsoft.com/download
    echo.
    pause
    goto menu
)

echo 編譯並啟動應用程式...
dotnet run

echo.
pause
goto menu

:run_electron
echo.
echo ⚡ 啟動 Electron 版本...
echo.
node --version >nul 2>&1
if errorlevel 1 (
    echo ❌ 錯誤: 未找到 Node.js
    echo 請先安裝 Node.js: https://nodejs.org/
    echo.
    pause
    goto menu
)

echo 安裝依賴...
npm install >nul 2>&1

echo 啟動應用程式...
npm start

echo.
pause
goto menu

:show_guide
echo.
echo 📖 打開使用指南...
if exist "使用指南.md" (
    start "" "使用指南.md"
) else (
    echo ❌ 使用指南文件不存在
)
echo.
pause
goto menu

:build_tools
echo.
echo 🛠️ 打開構建工具...
call build_all.bat
goto menu

:exit
echo.
echo 感謝使用 Windows 應用程式快速啟動器！
echo.
pause
exit /b 0