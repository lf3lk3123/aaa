#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Windows GUI 应用程式示例
使用 tkinter 创建现代化图形界面
优化版本 - 包含错误处理、主题支持和改进的用户体验
"""

import tkinter as tk
from tkinter import ttk, messagebox, filedialog
import sys
import os
import json
import datetime
from pathlib import Path

class MainApplication:
    def __init__(self, root):
        self.root = root
        self.setup_window()
        self.setup_variables()
        self.create_widgets()
        self.load_settings()
        
    def setup_window(self):
        """设置窗口属性"""
        self.root.title("我的 Windows 应用程式 v2.0")
        self.root.geometry("600x500")
        self.root.minsize(500, 400)
        
        # 设置窗口图标
        try:
            icon_path = Path(__file__).parent / "icon.ico"
            if icon_path.exists():
                self.root.iconbitmap(str(icon_path))
        except Exception as e:
            print(f"无法加载图标: {e}")
        
        # 设置窗口居中
        self.center_window()
        
    def setup_variables(self):
        """初始化变量"""
        self.name_var = tk.StringVar()
        self.message_count = 0
        self.settings_file = Path(__file__).parent / "settings.json"
        
    def center_window(self):
        """窗口居中显示"""
        self.root.update_idletasks()
        width = self.root.winfo_width()
        height = self.root.winfo_height()
        x = (self.root.winfo_screenwidth() // 2) - (width // 2)
        y = (self.root.winfo_screenheight() // 2) - (height // 2)
        self.root.geometry(f"{width}x{height}+{x}+{y}")
        
    def create_widgets(self):
        """创建界面组件"""
        # 创建样式
        style = ttk.Style()
        style.theme_use('clam')
        
        # 主框架
        main_frame = ttk.Frame(self.root, padding="15")
        main_frame.grid(row=0, column=0, sticky=(tk.W, tk.E, tk.N, tk.S))
        
        # 标题框架
        title_frame = ttk.Frame(main_frame)
        title_frame.grid(row=0, column=0, columnspan=3, pady=(0, 20), sticky=(tk.W, tk.E))
        
        title_label = ttk.Label(title_frame, text="🎉 欢迎使用我的应用程式", 
                               font=("Microsoft YaHei", 18, "bold"))
        title_label.pack()
        
        subtitle_label = ttk.Label(title_frame, text="现代化 Python GUI 应用程式", 
                                  font=("Microsoft YaHei", 10))
        subtitle_label.pack()
        
        # 输入区域
        input_frame = ttk.LabelFrame(main_frame, text="用户信息", padding="10")
        input_frame.grid(row=1, column=0, columnspan=3, pady=(0, 15), sticky=(tk.W, tk.E))
        
        ttk.Label(input_frame, text="请输入您的姓名:", font=("Microsoft YaHei", 10)).grid(
            row=0, column=0, sticky=tk.W, pady=5)
        
        name_entry = ttk.Entry(input_frame, textvariable=self.name_var, width=40, font=("Microsoft YaHei", 10))
        name_entry.grid(row=0, column=1, pady=5, padx=(10, 0), sticky=(tk.W, tk.E))
        name_entry.bind('<Return>', lambda e: self.greet())
        
        # 按钮区域
        button_frame = ttk.Frame(main_frame)
        button_frame.grid(row=2, column=0, columnspan=3, pady=(0, 15))
        
        buttons = [
            ("🤝 问候", self.greet, "#4CAF50"),
            ("🗑️ 清除", self.clear, "#FF9800"),
            ("💾 保存", self.save_messages, "#2196F3"),
            ("ℹ️ 关于", self.show_about, "#9C27B0"),
            ("❌ 退出", self.quit_app, "#F44336")
        ]
        
        for i, (text, command, color) in enumerate(buttons):
            btn = ttk.Button(button_frame, text=text, command=command, width=12)
            btn.grid(row=0, column=i, padx=5)
        
        # 消息显示区域
        text_frame = ttk.LabelFrame(main_frame, text="消息历史", padding="10")
        text_frame.grid(row=3, column=0, columnspan=3, pady=(0, 10), sticky=(tk.W, tk.E, tk.N, tk.S))
        
        # 文本区域和滚动条
        text_container = ttk.Frame(text_frame)
        text_container.pack(fill=tk.BOTH, expand=True)
        
        self.text_area = tk.Text(text_container, height=12, width=60, wrap=tk.WORD,
                                font=("Microsoft YaHei", 10), bg="#f8f9fa", fg="#333")
        scrollbar = ttk.Scrollbar(text_container, orient="vertical", command=self.text_area.yview)
        
        self.text_area.pack(side=tk.LEFT, fill=tk.BOTH, expand=True)
        scrollbar.pack(side=tk.RIGHT, fill=tk.Y)
        self.text_area.configure(yscrollcommand=scrollbar.set)
        
        # 状态栏
        self.status_frame = ttk.Frame(main_frame)
        self.status_frame.grid(row=4, column=0, columnspan=3, sticky=(tk.W, tk.E))
        
        self.status_label = ttk.Label(self.status_frame, text="就绪", font=("Microsoft YaHei", 9))
        self.status_label.pack(side=tk.LEFT)
        
        self.message_count_label = ttk.Label(self.status_frame, text="消息数: 0", font=("Microsoft YaHei", 9))
        self.message_count_label.pack(side=tk.RIGHT)
        
        # 配置网格权重
        self.root.columnconfigure(0, weight=1)
        self.root.rowconfigure(0, weight=1)
        main_frame.columnconfigure(1, weight=1)
        main_frame.rowconfigure(3, weight=1)
        input_frame.columnconfigure(1, weight=1)
        
    def greet(self):
        """问候功能 - 增强版"""
        name = self.name_var.get().strip()
        if not name:
            messagebox.showwarning("⚠️ 警告", "请先输入您的姓名!")
            return
            
        try:
            current_time = datetime.datetime.now().strftime("%H:%M:%S")
            self.message_count += 1
            
            greeting = f"[{current_time}] 🤝 你好, {name}! 欢迎使用这个应用程式! (#{self.message_count})\n"
            self.text_area.insert(tk.END, greeting)
            self.text_area.see(tk.END)
            
            self.update_status(f"已问候 {name}")
            self.update_message_count()
            
        except Exception as e:
            messagebox.showerror("错误", f"问候时发生错误: {str(e)}")
            
    def clear(self):
        """清除功能 - 增强版"""
        if self.message_count > 0:
            if messagebox.askyesno("🗑️ 确认清除", f"确定要清除所有 {self.message_count} 条消息吗?"):
                self.name_var.set("")
                self.text_area.delete(1.0, tk.END)
                self.message_count = 0
                self.update_status("已清除所有消息")
                self.update_message_count()
        else:
            self.name_var.set("")
            self.text_area.delete(1.0, tk.END)
            self.update_status("已清除")
            
    def save_messages(self):
        """保存消息到文件"""
        try:
            if self.message_count == 0:
                messagebox.showinfo("💾 保存", "没有消息可保存!")
                return
                
            filename = filedialog.asksaveasfilename(
                defaultextension=".txt",
                filetypes=[("文本文件", "*.txt"), ("所有文件", "*.*")],
                title="保存消息历史"
            )
            
            if filename:
                content = self.text_area.get(1.0, tk.END)
                with open(filename, 'w', encoding='utf-8') as f:
                    f.write(f"消息历史 - 导出时间: {datetime.datetime.now()}\n")
                    f.write("=" * 50 + "\n\n")
                    f.write(content)
                
                messagebox.showinfo("💾 保存成功", f"消息已保存到:\n{filename}")
                self.update_status(f"已保存 {self.message_count} 条消息")
                
        except Exception as e:
            messagebox.showerror("❌ 保存失败", f"保存时发生错误:\n{str(e)}")
            
    def load_settings(self):
        """加载设置"""
        try:
            if self.settings_file.exists():
                with open(self.settings_file, 'r', encoding='utf-8') as f:
                    settings = json.load(f)
                    self.name_var.set(settings.get('last_name', ''))
        except Exception as e:
            print(f"加载设置失败: {e}")
            
    def save_settings(self):
        """保存设置"""
        try:
            settings = {
                'last_name': self.name_var.get(),
                'message_count': self.message_count,
                'last_used': datetime.datetime.now().isoformat()
            }
            with open(self.settings_file, 'w', encoding='utf-8') as f:
                json.dump(settings, f, ensure_ascii=False, indent=2)
        except Exception as e:
            print(f"保存设置失败: {e}")
            
    def update_status(self, message):
        """更新状态栏"""
        self.status_label.config(text=message)
        self.root.after(3000, lambda: self.status_label.config(text="就绪"))
        
    def update_message_count(self):
        """更新消息计数"""
        self.message_count_label.config(text=f"消息数: {self.message_count}")
        
    def show_about(self):
        """关于对话框 - 增强版"""
        about_text = f"""🎉 我的 Windows 应用程式 v2.0

✨ 功能特点:
• 现代化用户界面设计
• 智能消息历史管理
• 文件保存和加载功能
• 自动设置保存
• 增强的错误处理

🛠️ 技术信息:
• 开发语言: Python {sys.version.split()[0]}
• GUI 框架: tkinter
• 支持平台: Windows, macOS, Linux

👨‍💻 开发信息:
• 开发者: OpenHands
• 版本: 2.0.0
• 更新日期: {datetime.datetime.now().strftime('%Y-%m-%d')}
• 许可证: MIT License

📊 当前会话:
• 消息数量: {self.message_count}
• 运行时间: {datetime.datetime.now().strftime('%H:%M:%S')}
        """
        messagebox.showinfo("ℹ️ 关于应用程式", about_text)
        
    def quit_app(self):
        """退出应用程式 - 增强版"""
        try:
            self.save_settings()
            if messagebox.askokcancel("❌ 退出确认", 
                                    f"确定要退出应用程式吗?\n\n当前会话信息:\n• 消息数: {self.message_count}\n• 设置已自动保存"):
                self.root.quit()
        except Exception as e:
            print(f"退出时发生错误: {e}")
            self.root.quit()

def main():
    """主函数"""
    try:
        root = tk.Tk()
        app = MainApplication(root)
        
        # 设置退出处理
        root.protocol("WM_DELETE_WINDOW", app.quit_app)
        
        # 启动应用
        root.mainloop()
        
    except Exception as e:
        messagebox.showerror("❌ 启动错误", f"应用程式启动失败:\n{str(e)}")
        sys.exit(1)

if __name__ == "__main__":
    main()