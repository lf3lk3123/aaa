#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
简单的 Windows GUI 应用程式示例
使用 tkinter 创建图形界面
"""

import tkinter as tk
from tkinter import ttk, messagebox
import sys
import os

class MainApplication:
    def __init__(self, root):
        self.root = root
        self.root.title("我的 Windows 应用程式")
        self.root.geometry("400x300")
        
        # 设置窗口图标（如果有的话）
        try:
            # 可以添加图标文件
            # self.root.iconbitmap("icon.ico")
            pass
        except:
            pass
        
        self.create_widgets()
        
    def create_widgets(self):
        # 主框架
        main_frame = ttk.Frame(self.root, padding="10")
        main_frame.grid(row=0, column=0, sticky=(tk.W, tk.E, tk.N, tk.S))
        
        # 标题
        title_label = ttk.Label(main_frame, text="欢迎使用我的应用程式", 
                               font=("Arial", 16, "bold"))
        title_label.grid(row=0, column=0, columnspan=2, pady=10)
        
        # 输入框
        ttk.Label(main_frame, text="请输入您的姓名:").grid(row=1, column=0, sticky=tk.W, pady=5)
        self.name_var = tk.StringVar()
        name_entry = ttk.Entry(main_frame, textvariable=self.name_var, width=30)
        name_entry.grid(row=1, column=1, pady=5)
        
        # 按钮
        button_frame = ttk.Frame(main_frame)
        button_frame.grid(row=2, column=0, columnspan=2, pady=20)
        
        ttk.Button(button_frame, text="问候", command=self.greet).pack(side=tk.LEFT, padx=5)
        ttk.Button(button_frame, text="清除", command=self.clear).pack(side=tk.LEFT, padx=5)
        ttk.Button(button_frame, text="关于", command=self.show_about).pack(side=tk.LEFT, padx=5)
        ttk.Button(button_frame, text="退出", command=self.quit_app).pack(side=tk.LEFT, padx=5)
        
        # 文本显示区域
        self.text_area = tk.Text(main_frame, height=10, width=50)
        self.text_area.grid(row=3, column=0, columnspan=2, pady=10)
        
        # 滚动条
        scrollbar = ttk.Scrollbar(main_frame, orient="vertical", command=self.text_area.yview)
        scrollbar.grid(row=3, column=2, sticky="ns")
        self.text_area.configure(yscrollcommand=scrollbar.set)
        
        # 配置网格权重
        self.root.columnconfigure(0, weight=1)
        self.root.rowconfigure(0, weight=1)
        main_frame.columnconfigure(1, weight=1)
        main_frame.rowconfigure(3, weight=1)
        
    def greet(self):
        name = self.name_var.get().strip()
        if name:
            greeting = f"你好, {name}! 欢迎使用这个应用程式!\n"
            self.text_area.insert(tk.END, greeting)
            self.text_area.see(tk.END)
        else:
            messagebox.showwarning("警告", "请先输入您的姓名!")
            
    def clear(self):
        self.name_var.set("")
        self.text_area.delete(1.0, tk.END)
        
    def show_about(self):
        about_text = """
我的 Windows 应用程式 v1.0

这是一个使用 Python 和 tkinter 开发的示例应用程式。
可以打包成 Windows exe 文件。

开发者: OpenHands
日期: 2025-07-04
        """
        messagebox.showinfo("关于", about_text)
        
    def quit_app(self):
        if messagebox.askokcancel("退出", "确定要退出应用程式吗?"):
            self.root.quit()

def main():
    root = tk.Tk()
    app = MainApplication(root)
    root.mainloop()

if __name__ == "__main__":
    main()