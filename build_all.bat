@echo off
chcp 65001 >nul
title Windows 应用程式自动构建脚本 v2.0

echo ========================================
echo    🎉 Windows 应用程式自动构建脚本 v2.0
echo ========================================
echo    现代化多技术栈桌面应用开发工具
echo ========================================
echo.

set "SCRIPT_DIR=%~dp0"
cd /d "%SCRIPT_DIR%"

echo 📁 当前目录: %CD%
echo 📅 构建时间: %DATE% %TIME%
echo.

:: 检查必要工具
call :check_requirements

:menu
echo ========================================
echo 🛠️  请选择要构建的应用程式类型:
echo ========================================
echo.
echo 1. 🐍 Python + PyInstaller (推荐用于快速原型)
echo 2. 🔷 C# + .NET (推荐用于企业应用)
echo 3. ⚡ Electron + JavaScript (推荐用于现代UI)
echo 4. 🚀 构建所有版本
echo 5. 🧪 运行测试
echo 6. 🗑️  清理构建文件
echo 7. 📊 显示构建统计
echo 0. ❌ 退出
echo.
set /p choice="请输入选择 (0-7): "

if "%choice%"=="1" goto build_python
if "%choice%"=="2" goto build_csharp
if "%choice%"=="3" goto build_electron
if "%choice%"=="4" goto build_all
if "%choice%"=="5" goto run_tests
if "%choice%"=="6" goto clean_all
if "%choice%"=="7" goto show_stats
if "%choice%"=="0" goto exit_script
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

:: 新增功能函数
:check_requirements
echo 🔍 检查开发环境...
set "python_ok=0"
set "dotnet_ok=0"
set "node_ok=0"

python --version >nul 2>&1 && set "python_ok=1"
dotnet --version >nul 2>&1 && set "dotnet_ok=1"
node --version >nul 2>&1 && set "node_ok=1"

echo 📋 环境检查结果:
if "%python_ok%"=="1" (echo ✅ Python: 已安装) else (echo ❌ Python: 未安装)
if "%dotnet_ok%"=="1" (echo ✅ .NET SDK: 已安装) else (echo ❌ .NET SDK: 未安装)
if "%node_ok%"=="1" (echo ✅ Node.js: 已安装) else (echo ❌ Node.js: 未安装)
echo.
goto :eof

:run_tests
echo.
echo ========================================
echo 🧪 运行应用程式测试...
echo ========================================
echo.

echo 测试 Python 版本...
if exist "main.py" (
    python main.py --test >nul 2>&1 && echo ✅ Python 版本测试通过 || echo ❌ Python 版本测试失败
) else (
    echo ⚠️  main.py 不存在
)

echo 测试 C# 版本...
if exist "Program.cs" (
    dotnet run --project . >nul 2>&1 && echo ✅ C# 版本测试通过 || echo ❌ C# 版本测试失败
) else (
    echo ⚠️  Program.cs 不存在
)

echo 测试 Electron 版本...
if exist "package.json" (
    npm test >nul 2>&1 && echo ✅ Electron 版本测试通过 || echo ❌ Electron 版本测试失败
) else (
    echo ⚠️  package.json 不存在
)

echo.
pause
goto menu

:show_stats
echo.
echo ========================================
echo 📊 构建统计信息
echo ========================================
echo.

echo 📁 项目文件统计:
if exist "main.py" (
    for %%F in (main.py) do echo   Python 主文件: %%~zF 字节
)
if exist "Program.cs" (
    for %%F in (Program.cs) do echo   C# 主文件: %%~zF 字节
)
if exist "main.js" (
    for %%F in (main.js) do echo   Electron 主文件: %%~zF 字节
)

echo.
echo 📦 构建输出统计:
if exist "dist" (
    echo   构建目录大小:
    dir "dist" /s /-c | find "个文件"
) else (
    echo   ⚠️  未找到构建输出
)

echo.
echo 🕒 最后修改时间:
if exist "main.py" (
    for %%F in (main.py) do echo   Python: %%~tF
)
if exist "Program.cs" (
    for %%F in (Program.cs) do echo   C#: %%~tF
)
if exist "main.js" (
    for %%F in (main.js) do echo   Electron: %%~tF
)

echo.
pause
goto menu

:exit_script
echo.
echo ========================================
echo 🎉 感谢使用 Windows 应用程式构建脚本 v2.0!
echo ========================================
echo.
echo 📈 构建统计:
if exist "dist\MyWindowsApp.exe" echo ✅ Python 版本已构建
if exist "dist\csharp\MyWindowsApp.exe" echo ✅ C# 版本已构建  
if exist "dist\*.exe" echo ✅ Electron 版本已构建
echo.
echo 💡 提示: 
echo   - 所有构建文件保存在 dist\ 目录中
echo   - 使用选项 6 可以清理构建文件
echo   - 访问 https://github.com/lf3lk3123/aaa 获取更多信息
echo.
pause
exit /b 0