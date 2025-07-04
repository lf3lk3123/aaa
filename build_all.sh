#!/bin/bash

# Windows 应用程式自动构建脚本 (Linux/macOS 版本)

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$SCRIPT_DIR"

echo "========================================"
echo "   Windows 应用程式自动构建脚本"
echo "========================================"
echo
echo "当前目录: $(pwd)"
echo

show_menu() {
    echo "请选择要构建的应用程式类型:"
    echo
    echo "1. Python + PyInstaller"
    echo "2. C# + .NET"
    echo "3. Electron + JavaScript"
    echo "4. 构建所有版本"
    echo "5. 清理构建文件"
    echo "0. 退出"
    echo
}

build_python() {
    echo
    echo "========================================"
    echo "构建 Python 版本..."
    echo "========================================"
    echo

    # 检查 Python 是否安装
    if ! command -v python3 &> /dev/null && ! command -v python &> /dev/null; then
        echo "错误: 未找到 Python，请先安装 Python 3.8+"
        return 1
    fi

    # 使用 python3 或 python
    PYTHON_CMD="python3"
    if ! command -v python3 &> /dev/null; then
        PYTHON_CMD="python"
    fi

    # 安装依赖
    echo "安装 Python 依赖..."
    $PYTHON_CMD -m pip install -r requirements.txt

    # 构建
    echo "开始构建..."
    $PYTHON_CMD build_exe.py

    if [ -f "dist/MyWindowsApp.exe" ]; then
        echo
        echo "✅ Python 版本构建成功!"
        echo "📁 输出目录: dist/"
        echo "📄 可执行文件: MyWindowsApp.exe"
    else
        echo
        echo "❌ Python 版本构建失败!"
        return 1
    fi
}

build_csharp() {
    echo
    echo "========================================"
    echo "构建 C# 版本..."
    echo "========================================"
    echo

    # 检查 .NET 是否安装
    if ! command -v dotnet &> /dev/null; then
        echo "错误: 未找到 .NET SDK，请先安装 .NET 6.0+"
        return 1
    fi

    # 构建
    echo "开始构建..."
    dotnet publish -c Release --self-contained true -r win-x64 -p:PublishSingleFile=true -o "dist/csharp"

    if [ -f "dist/csharp/MyWindowsApp.exe" ]; then
        echo
        echo "✅ C# 版本构建成功!"
        echo "📁 输出目录: dist/csharp/"
        echo "📄 可执行文件: MyWindowsApp.exe"
    else
        echo
        echo "❌ C# 版本构建失败!"
        return 1
    fi
}

build_electron() {
    echo
    echo "========================================"
    echo "构建 Electron 版本..."
    echo "========================================"
    echo

    # 检查 Node.js 是否安装
    if ! command -v node &> /dev/null; then
        echo "错误: 未找到 Node.js，请先安装 Node.js 16+"
        return 1
    fi

    # 安装依赖
    echo "安装 Node.js 依赖..."
    npm install

    # 构建
    echo "开始构建..."
    npm run build-win

    if ls dist/*.exe 1> /dev/null 2>&1; then
        echo
        echo "✅ Electron 版本构建成功!"
        echo "📁 输出目录: dist/"
        echo "📄 安装程序: *.exe"
    else
        echo
        echo "❌ Electron 版本构建失败!"
        return 1
    fi
}

build_all() {
    echo
    echo "========================================"
    echo "构建所有版本..."
    echo "========================================"
    echo

    build_python_silent
    build_csharp_silent
    build_electron_silent

    echo
    echo "========================================"
    echo "构建完成总结:"
    echo "========================================"
    [ -f "dist/MyWindowsApp.exe" ] && echo "✅ Python 版本: dist/MyWindowsApp.exe"
    [ -f "dist/csharp/MyWindowsApp.exe" ] && echo "✅ C# 版本: dist/csharp/MyWindowsApp.exe"
    ls dist/*.exe 1> /dev/null 2>&1 && echo "✅ Electron 版本: dist/*.exe"
    echo
}

build_python_silent() {
    echo "构建 Python 版本..."
    if command -v python3 &> /dev/null || command -v python &> /dev/null; then
        PYTHON_CMD="python3"
        if ! command -v python3 &> /dev/null; then
            PYTHON_CMD="python"
        fi
        $PYTHON_CMD -m pip install -r requirements.txt > /dev/null 2>&1
        $PYTHON_CMD build_exe.py > /dev/null 2>&1
    else
        echo "❌ Python 未安装，跳过"
    fi
}

build_csharp_silent() {
    echo "构建 C# 版本..."
    if command -v dotnet &> /dev/null; then
        dotnet publish -c Release --self-contained true -r win-x64 -p:PublishSingleFile=true -o "dist/csharp" > /dev/null 2>&1
    else
        echo "❌ .NET SDK 未安装，跳过"
    fi
}

build_electron_silent() {
    echo "构建 Electron 版本..."
    if command -v node &> /dev/null; then
        npm install > /dev/null 2>&1
        npm run build-win > /dev/null 2>&1
    else
        echo "❌ Node.js 未安装，跳过"
    fi
}

clean_all() {
    echo
    echo "========================================"
    echo "清理构建文件..."
    echo "========================================"
    echo

    [ -d "build" ] && rm -rf "build" && echo "已删除 build 目录"
    [ -d "dist" ] && rm -rf "dist" && echo "已删除 dist 目录"
    [ -d "bin" ] && rm -rf "bin" && echo "已删除 bin 目录"
    [ -d "obj" ] && rm -rf "obj" && echo "已删除 obj 目录"
    [ -d "node_modules" ] && rm -rf "node_modules" && echo "已删除 node_modules 目录"

    echo
    echo "✅ 清理完成!"
    echo
}

# 主循环
while true; do
    show_menu
    read -p "请输入选择 (0-5): " choice
    
    case $choice in
        1)
            build_python
            echo
            read -p "按回车键继续..."
            ;;
        2)
            build_csharp
            echo
            read -p "按回车键继续..."
            ;;
        3)
            build_electron
            echo
            read -p "按回车键继续..."
            ;;
        4)
            build_all
            echo
            read -p "按回车键继续..."
            ;;
        5)
            clean_all
            read -p "按回车键继续..."
            ;;
        0)
            echo
            echo "感谢使用 Windows 应用程式构建脚本!"
            echo
            exit 0
            ;;
        *)
            echo "无效选择，请重新输入。"
            echo
            ;;
    esac
done