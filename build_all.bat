@echo off
chcp 65001 >nul
echo ========================================
echo    Windows 应用程式自动构建脚本
echo ========================================
echo.

set "SCRIPT_DIR=%~dp0"
cd /d "%SCRIPT_DIR%"

echo 当前目录: %CD%
echo.

:menu
echo 请选择要构建的应用程式类型:
echo.
echo 1. Python + PyInstaller
echo 2. C# + .NET
echo 3. Electron + JavaScript
echo 4. 构建所有版本
echo 5. 清理构建文件
echo 0. 退出
echo.
set /p choice="请输入选择 (0-5): "

if "%choice%"=="1" goto build_python
if "%choice%"=="2" goto build_csharp
if "%choice%"=="3" goto build_electron
if "%choice%"=="4" goto build_all
if "%choice%"=="5" goto clean_all
if "%choice%"=="0" goto exit
echo 无效选择，请重新输入。
echo.
goto menu

:build_python
echo.
echo ========================================
echo 构建 Python 版本...
echo ========================================
echo.

REM 检查 Python 是否安装
python --version >nul 2>&1
if errorlevel 1 (
    echo 错误: 未找到 Python，请先安装 Python 3.8+
    pause
    goto menu
)

REM 安装依赖
echo 安装 Python 依赖...
pip install -r requirements.txt

REM 构建
echo 开始构建...
python build_exe.py

if exist "dist\MyWindowsApp.exe" (
    echo.
    echo ✅ Python 版本构建成功!
    echo 📁 输出目录: dist\
    echo 📄 可执行文件: MyWindowsApp.exe
) else (
    echo.
    echo ❌ Python 版本构建失败!
)

echo.
pause
goto menu

:build_csharp
echo.
echo ========================================
echo 构建 C# 版本...
echo ========================================
echo.

REM 检查 .NET 是否安装
dotnet --version >nul 2>&1
if errorlevel 1 (
    echo 错误: 未找到 .NET SDK，请先安装 .NET 6.0+
    pause
    goto menu
)

REM 构建
echo 开始构建...
dotnet publish -c Release --self-contained true -r win-x64 -p:PublishSingleFile=true -o "dist\csharp"

if exist "dist\csharp\MyWindowsApp.exe" (
    echo.
    echo ✅ C# 版本构建成功!
    echo 📁 输出目录: dist\csharp\
    echo 📄 可执行文件: MyWindowsApp.exe
) else (
    echo.
    echo ❌ C# 版本构建失败!
)

echo.
pause
goto menu

:build_electron
echo.
echo ========================================
echo 构建 Electron 版本...
echo ========================================
echo.

REM 检查 Node.js 是否安装
node --version >nul 2>&1
if errorlevel 1 (
    echo 错误: 未找到 Node.js，请先安装 Node.js 16+
    pause
    goto menu
)

REM 安装依赖
echo 安装 Node.js 依赖...
npm install

REM 构建
echo 开始构建...
npm run build-win

if exist "dist\*.exe" (
    echo.
    echo ✅ Electron 版本构建成功!
    echo 📁 输出目录: dist\
    echo 📄 安装程序: *.exe
) else (
    echo.
    echo ❌ Electron 版本构建失败!
)

echo.
pause
goto menu

:build_all
echo.
echo ========================================
echo 构建所有版本...
echo ========================================
echo.

call :build_python_silent
call :build_csharp_silent
call :build_electron_silent

echo.
echo ========================================
echo 构建完成总结:
echo ========================================
if exist "dist\MyWindowsApp.exe" echo ✅ Python 版本: dist\MyWindowsApp.exe
if exist "dist\csharp\MyWindowsApp.exe" echo ✅ C# 版本: dist\csharp\MyWindowsApp.exe
if exist "dist\*.exe" echo ✅ Electron 版本: dist\*.exe
echo.
pause
goto menu

:build_python_silent
echo 构建 Python 版本...
python --version >nul 2>&1 && (
    pip install -r requirements.txt >nul 2>&1
    python build_exe.py >nul 2>&1
) || echo ❌ Python 未安装，跳过
goto :eof

:build_csharp_silent
echo 构建 C# 版本...
dotnet --version >nul 2>&1 && (
    dotnet publish -c Release --self-contained true -r win-x64 -p:PublishSingleFile=true -o "dist\csharp" >nul 2>&1
) || echo ❌ .NET SDK 未安装，跳过
goto :eof

:build_electron_silent
echo 构建 Electron 版本...
node --version >nul 2>&1 && (
    npm install >nul 2>&1
    npm run build-win >nul 2>&1
) || echo ❌ Node.js 未安装，跳过
goto :eof

:clean_all
echo.
echo ========================================
echo 清理构建文件...
echo ========================================
echo.

if exist "build" (
    rmdir /s /q "build"
    echo 已删除 build 目录
)

if exist "dist" (
    rmdir /s /q "dist"
    echo 已删除 dist 目录
)

if exist "bin" (
    rmdir /s /q "bin"
    echo 已删除 bin 目录
)

if exist "obj" (
    rmdir /s /q "obj"
    echo 已删除 obj 目录
)

if exist "node_modules" (
    rmdir /s /q "node_modules"
    echo 已删除 node_modules 目录
)

echo.
echo ✅ 清理完成!
echo.
pause
goto menu

:exit
echo.
echo 感谢使用 Windows 应用程式构建脚本!
echo.
pause
exit /b 0