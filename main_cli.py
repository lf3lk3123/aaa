#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
命令行版本的 Windows 应用程式示例
用于演示和测试功能
"""

import sys
import os
from datetime import datetime

class CLIApplication:
    def __init__(self):
        self.messages = []
        self.running = True
        
    def show_banner(self):
        print("=" * 50)
        print("    我的 Windows 应用程式 (命令行版本)")
        print("=" * 50)
        print()
        
    def show_menu(self):
        print("\n可用命令:")
        print("1. greet [姓名] - 问候用户")
        print("2. list - 显示所有消息")
        print("3. clear - 清除所有消息")
        print("4. about - 关于信息")
        print("5. help - 显示帮助")
        print("6. exit - 退出程序")
        print()
        
    def greet(self, name=None):
        if not name:
            name = input("请输入您的姓名: ").strip()
        
        if name:
            timestamp = datetime.now().strftime("%Y-%m-%d %H:%M:%S")
            message = f"[{timestamp}] 你好, {name}! 欢迎使用这个应用程式!"
            self.messages.append(message)
            print(f"✅ {message}")
        else:
            print("❌ 请输入有效的姓名!")
            
    def list_messages(self):
        if self.messages:
            print("\n📝 消息历史:")
            print("-" * 40)
            for i, msg in enumerate(self.messages, 1):
                print(f"{i}. {msg}")
            print("-" * 40)
        else:
            print("📝 暂无消息记录")
            
    def clear_messages(self):
        if self.messages:
            self.messages.clear()
            print("✅ 已清除所有消息")
        else:
            print("📝 消息列表已经是空的")
            
    def show_about(self):
        about_text = """
📱 我的 Windows 应用程式 v1.0

这是一个使用 Python 开发的示例应用程式。
可以打包成 Windows exe 文件。

特性:
• 支持图形界面 (tkinter)
• 支持命令行界面
• 可打包成独立的 exe 文件
• 跨平台兼容

开发者: OpenHands
日期: 2025-07-04
Python 版本: {python_version}
操作系统: {os_name}
        """.format(
            python_version=sys.version.split()[0],
            os_name=os.name
        )
        print(about_text)
        
    def show_help(self):
        help_text = """
🔧 使用帮助:

命令格式:
  greet [姓名]     - 问候指定用户，如果不提供姓名会提示输入
  list            - 显示所有问候消息的历史记录
  clear           - 清除所有消息历史
  about           - 显示应用程式的详细信息
  help            - 显示此帮助信息
  exit            - 退出应用程式

示例:
  > greet 张三
  > greet
  > list
  > clear

提示: 输入命令时不区分大小写
        """
        print(help_text)
        
    def process_command(self, command_line):
        parts = command_line.strip().lower().split()
        if not parts:
            return
            
        command = parts[0]
        args = parts[1:] if len(parts) > 1 else []
        
        if command in ['1', 'greet']:
            name = ' '.join(args) if args else None
            self.greet(name)
        elif command in ['2', 'list']:
            self.list_messages()
        elif command in ['3', 'clear']:
            self.clear_messages()
        elif command in ['4', 'about']:
            self.show_about()
        elif command in ['5', 'help']:
            self.show_help()
        elif command in ['6', 'exit', 'quit']:
            print("👋 感谢使用，再见!")
            self.running = False
        else:
            print(f"❌ 未知命令: {command}")
            print("💡 输入 'help' 查看可用命令")
            
    def run(self):
        self.show_banner()
        self.show_help()
        
        while self.running:
            try:
                command = input("\n> ").strip()
                if command:
                    self.process_command(command)
            except KeyboardInterrupt:
                print("\n\n👋 检测到 Ctrl+C，正在退出...")
                break
            except EOFError:
                print("\n\n👋 检测到 EOF，正在退出...")
                break

def main():
    """主函数"""
    app = CLIApplication()
    app.run()

if __name__ == "__main__":
    main()