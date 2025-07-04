#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
自动化打包脚本
使用 PyInstaller 将 Python 应用程式打包成 Windows exe 文件
"""

import os
import sys
import subprocess
import shutil

def build_exe():
    """打包应用程式为 exe 文件"""
    
    print("开始打包 Windows exe 应用程式...")
    
    # 清理之前的构建文件
    if os.path.exists("build"):
        shutil.rmtree("build")
        print("清理 build 目录")
    
    if os.path.exists("dist"):
        shutil.rmtree("dist")
        print("清理 dist 目录")
    
    # PyInstaller 命令参数
    cmd = [
        "pyinstaller",
        "--onefile",                    # 打包成单个文件
        "--windowed",                   # 不显示控制台窗口
        "--name=MyWindowsApp",          # 指定输出文件名
        "--add-data=.;.",              # 添加数据文件（如果有的话）
        # "--icon=icon.ico",            # 添加图标（如果有的话）
        "main.py"
    ]
    
    try:
        # 执行打包命令
        result = subprocess.run(cmd, check=True, capture_output=True, text=True)
        print("打包成功!")
        print(f"输出: {result.stdout}")
        
        # 检查输出文件
        exe_path = os.path.join("dist", "MyWindowsApp.exe")
        if os.path.exists(exe_path):
            file_size = os.path.getsize(exe_path) / (1024 * 1024)  # MB
            print(f"生成的 exe 文件: {exe_path}")
            print(f"文件大小: {file_size:.2f} MB")
        else:
            print("警告: 未找到生成的 exe 文件")
            
    except subprocess.CalledProcessError as e:
        print(f"打包失败: {e}")
        print(f"错误输出: {e.stderr}")
        return False
    
    return True

def create_installer_script():
    """创建安装脚本"""
    installer_script = """
@echo off
echo 安装 MyWindowsApp...

REM 创建程序目录
if not exist "C:\\Program Files\\MyWindowsApp" (
    mkdir "C:\\Program Files\\MyWindowsApp"
)

REM 复制文件
copy "MyWindowsApp.exe" "C:\\Program Files\\MyWindowsApp\\"

REM 创建桌面快捷方式
echo 创建桌面快捷方式...
powershell "$WshShell = New-Object -comObject WScript.Shell; $Shortcut = $WshShell.CreateShortcut('%USERPROFILE%\\Desktop\\MyWindowsApp.lnk'); $Shortcut.TargetPath = 'C:\\Program Files\\MyWindowsApp\\MyWindowsApp.exe'; $Shortcut.Save()"

echo 安装完成!
pause
"""
    
    with open("dist/install.bat", "w", encoding="utf-8") as f:
        f.write(installer_script)
    
    print("创建安装脚本: dist/install.bat")

if __name__ == "__main__":
    if build_exe():
        create_installer_script()
        print("\n打包完成! 文件位于 dist/ 目录中")
        print("- MyWindowsApp.exe: 主程序")
        print("- install.bat: 安装脚本")
    else:
        print("打包失败!")
        sys.exit(1)