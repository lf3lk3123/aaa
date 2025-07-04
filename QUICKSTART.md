# 🚀 快速开始指南

本指南将帮助您在 5 分钟内开始开发 Windows exe 应用程式。

## 📋 前置要求

根据您选择的技术栈，安装相应的开发环境：

### Python 方式 (推荐新手)
```bash
# 安装 Python 3.8+ (从 python.org 下载)
python --version
```

### C# 方式 (推荐性能要求高的场景)
```bash
# 安装 .NET 6.0+ SDK (从 dotnet.microsoft.com 下载)
dotnet --version
```

### Electron 方式 (推荐现代 UI)
```bash
# 安装 Node.js 16+ (从 nodejs.org 下载)
node --version
npm --version
```

## ⚡ 一键开始

### Windows 用户
```cmd
# 克隆或下载项目后，双击运行
build_all.bat
```

### Linux/macOS 用户
```bash
# 克隆或下载项目后
chmod +x build_all.sh
./build_all.sh
```

## 🎯 选择您的技术栈

### 1️⃣ Python + tkinter (最简单)

**适合**: 初学者、快速原型、学习用途

```bash
# 运行开发版本
python main.py

# 或运行命令行版本 (无需图形界面)
python main_cli.py

# 打包成 exe
pip install pyinstaller
python build_exe.py
```

**优点**: 
- ✅ 学习成本低
- ✅ 代码简洁
- ✅ 跨平台

**缺点**: 
- ❌ 打包文件较大 (~50MB)
- ❌ 启动速度较慢

### 2️⃣ C# + Windows Forms (最高性能)

**适合**: 企业应用、性能要求高、Windows 专用

```bash
# 运行开发版本
dotnet run

# 打包成 exe
dotnet publish -c Release --self-contained true -r win-x64 -p:PublishSingleFile=true
```

**优点**: 
- ✅ 性能优秀
- ✅ 文件小 (~10MB)
- ✅ 启动快速
- ✅ 与 Windows 集成度高

**缺点**: 
- ❌ 主要限于 Windows
- ❌ 需要学习 C#

### 3️⃣ Electron + JavaScript (最美观)

**适合**: 现代 UI、Web 开发者、跨平台

```bash
# 安装依赖
npm install

# 运行开发版本
npm start

# 打包成 exe
npm run build-win
```

**优点**: 
- ✅ 界面美观现代
- ✅ Web 技术栈
- ✅ 跨平台
- ✅ 开发效率高

**缺点**: 
- ❌ 文件很大 (~150MB)
- ❌ 内存占用高

## 🔧 自定义您的应用程式

### 修改应用程式名称
1. **Python**: 编辑 `main.py` 中的 `self.root.title()`
2. **C#**: 编辑 `Program.cs` 中的 `this.Text`
3. **Electron**: 编辑 `index.html` 中的 `<title>` 和 `main.js` 中的窗口标题

### 添加功能
每个版本都有相同的基础结构：
- 用户输入处理
- 按钮事件处理
- 消息显示
- 关于对话框

您可以在此基础上添加：
- 文件操作
- 网络请求
- 数据库连接
- 系统集成

### 添加图标
1. 准备 `.ico` 格式的图标文件
2. **Python**: 在 `build_exe.py` 中取消注释 `--icon=icon.ico`
3. **C#**: 在项目文件中添加 `<ApplicationIcon>icon.ico</ApplicationIcon>`
4. **Electron**: 在 `main.js` 中设置 `icon` 属性

## 📦 分发您的应用程式

### 创建安装程序
- **Python**: 使用 NSIS 或 Inno Setup
- **C#**: 使用 Visual Studio Installer Projects
- **Electron**: 内置 NSIS 安装程序生成

### 代码签名 (可选)
为了避免 Windows Defender 警告，可以购买代码签名证书。

## 🐛 常见问题

### Q: 为什么 Python 版本这么大？
A: PyInstaller 会打包整个 Python 运行时。可以使用 `--exclude-module` 优化。

### Q: C# 版本需要安装 .NET 吗？
A: 使用 `--self-contained` 参数可以包含运行时，无需用户安装。

### Q: Electron 应用启动慢怎么办？
A: 这是正常现象，可以添加启动画面改善用户体验。

### Q: 如何添加数据库？
A: 
- **Python**: 使用 SQLite (内置) 或 PostgreSQL
- **C#**: 使用 Entity Framework + SQL Server/SQLite
- **Electron**: 使用 SQLite 或 IndexedDB

## 🎓 进阶学习

### Python 开发者
- 学习 PyQt/PySide 创建更专业的界面
- 了解 Nuitka 获得更好的性能
- 掌握 cx_Freeze 作为 PyInstaller 的替代

### C# 开发者
- 学习 WPF 创建现代界面
- 了解 .NET MAUI 进行跨平台开发
- 掌握 Entity Framework 进行数据库操作

### JavaScript 开发者
- 学习 React/Vue 集成到 Electron
- 了解 Tauri 作为更轻量的替代
- 掌握 Node.js 进行后端集成

## 🤝 获得帮助

- 📖 查看完整文档: [README.md](README.md)
- 🔄 查看更新日志: [CHANGELOG.md](CHANGELOG.md)
- 🐛 报告问题: 创建 GitHub Issue
- 💬 讨论交流: 创建 GitHub Discussion

---

**祝您开发愉快！** 🎉