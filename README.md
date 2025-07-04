# Windows 桌面应用程式开发指南

本仓库包含了三种不同技术栈开发 Windows exe 应用程式的完整示例。

## 📋 目录结构

```
aaa/
├── main.py                 # Python + tkinter 版本
├── build_exe.py           # Python 打包脚本
├── requirements.txt       # Python 依赖
├── Program.cs             # C# Windows Forms 版本
├── MyWindowsApp.csproj    # C# 项目文件
├── package.json           # Electron 项目配置
├── main.js                # Electron 主进程
├── index.html             # Electron 界面
├── style.css              # Electron 样式
├── renderer.js            # Electron 渲染进程
└── README.md              # 本文档
```

## 🚀 方法一：Python + PyInstaller

### 特点
- ✅ 简单易学，适合初学者
- ✅ 跨平台支持
- ✅ 丰富的第三方库
- ❌ 打包文件较大
- ❌ 启动速度相对较慢

### 安装依赖
```bash
pip install -r requirements.txt
```

### 运行开发版本
```bash
python main.py
```

### 打包成 exe
```bash
python build_exe.py
```

### 手动打包命令
```bash
pyinstaller --onefile --windowed --name=MyWindowsApp main.py
```

生成的文件位于 `dist/` 目录中。

## 🚀 方法二：C# + .NET

### 特点
- ✅ 性能优秀
- ✅ 与 Windows 系统集成度高
- ✅ 打包文件小
- ✅ 启动速度快
- ❌ 主要限于 Windows 平台
- ❌ 需要 .NET 运行时

### 安装 .NET SDK
下载并安装 [.NET 6.0 SDK](https://dotnet.microsoft.com/download)

### 运行开发版本
```bash
dotnet run
```

### 编译发布版本
```bash
# 编译为依赖框架的版本
dotnet publish -c Release

# 编译为自包含版本（包含运行时）
dotnet publish -c Release --self-contained true -r win-x64

# 编译为单文件版本
dotnet publish -c Release --self-contained true -r win-x64 -p:PublishSingleFile=true
```

## 🚀 方法三：Electron + JavaScript

### 特点
- ✅ 使用 Web 技术开发
- ✅ 跨平台支持
- ✅ 界面美观，易于定制
- ✅ 开发效率高
- ❌ 打包文件很大
- ❌ 内存占用较高

### 安装依赖
```bash
npm install
```

### 运行开发版本
```bash
npm start
```

### 打包成 exe
```bash
# 仅打包 Windows 版本
npm run build-win

# 打包所有平台
npm run build
```

生成的文件位于 `dist/` 目录中。

## 🛠️ 开发环境设置

### Windows 开发环境
1. **Python 环境**：安装 Python 3.8+
2. **C# 环境**：安装 Visual Studio 或 .NET SDK
3. **Node.js 环境**：安装 Node.js 16+

### Linux/macOS 交叉编译
虽然在 Linux/macOS 上也可以为 Windows 打包应用程式：

```bash
# Python (PyInstaller 支持交叉编译，但有限制)
# C# (.NET 支持交叉编译)
dotnet publish -c Release -r win-x64

# Electron (完全支持交叉编译)
npm run build-win
```

## 📦 打包优化建议

### Python 优化
- 使用 `--exclude-module` 排除不需要的模块
- 使用 UPX 压缩可执行文件
- 考虑使用 Nuitka 替代 PyInstaller

### C# 优化
- 使用 AOT 编译提高启动速度
- 启用代码裁剪减小文件大小
- 使用 ReadyToRun 镜像

### Electron 优化
- 使用 `electron-builder` 的压缩选项
- 排除开发依赖
- 使用 V8 快照提高启动速度

## 🎯 选择建议

| 场景 | 推荐技术 | 理由 |
|------|----------|------|
| 快速原型 | Python | 开发速度快，学习成本低 |
| 企业应用 | C# | 性能好，与 Windows 集成度高 |
| 现代界面 | Electron | 界面美观，Web 技术栈 |
| 跨平台 | Python/Electron | 一次开发，多平台运行 |
| 性能要求高 | C# | 原生性能，资源占用少 |

## 🔧 常见问题

### Python 打包问题
- **问题**：缺少模块
- **解决**：使用 `--hidden-import` 参数

### C# 编译问题
- **问题**：缺少 .NET 运行时
- **解决**：使用自包含发布

### Electron 打包问题
- **问题**：文件过大
- **解决**：配置 `files` 字段，排除不需要的文件

## 📝 许可证

MIT License - 可自由使用和修改。

## 🤝 贡献

欢迎提交 Issue 和 Pull Request！

---

**开发者**: OpenHands  
**日期**: 2025-07-04  
**版本**: 1.0.0
