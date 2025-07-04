const { app, BrowserWindow, Menu, dialog, ipcMain } = require('electron');
const path = require('path');
const fs = require('fs');

let mainWindow;

function createWindow() {
    // 创建浏览器窗口 - 修复安全性问题
    mainWindow = new BrowserWindow({
        width: 600,
        height: 500,
        minWidth: 500,
        minHeight: 400,
        webPreferences: {
            nodeIntegration: false,  // 修复安全性问题
            contextIsolation: true,  // 修复安全性问题
            enableRemoteModule: false,
            preload: path.join(__dirname, 'preload.js') // 添加预加载脚本
        },
        icon: getIconPath(),
        show: false, // 先不显示，等加载完成后再显示
        titleBarStyle: 'default'
    });

    // 加载应用的 index.html
    mainWindow.loadFile('index.html');

    // 窗口加载完成后显示
    mainWindow.once('ready-to-show', () => {
        mainWindow.show();
        
        // 开发环境下打开开发者工具
        if (process.env.NODE_ENV === 'development') {
            mainWindow.webContents.openDevTools();
        }
    });

    // 创建菜单
    createMenu();

    // 设置窗口居中
    mainWindow.center();

    // 当窗口关闭时触发
    mainWindow.on('closed', () => {
        mainWindow = null;
    });

    // 防止新窗口打开
    mainWindow.webContents.setWindowOpenHandler(() => {
        return { action: 'deny' };
    });
}

function getIconPath() {
    const iconPath = path.join(__dirname, 'icon.ico');
    return fs.existsSync(iconPath) ? iconPath : undefined;
}

function createMenu() {
    const template = [
        {
            label: '文件',
            submenu: [
                {
                    label: '新建',
                    accelerator: 'CmdOrCtrl+N',
                    click: () => {
                        mainWindow.webContents.send('menu-action', 'clear');
                    }
                },
                {
                    label: '保存',
                    accelerator: 'CmdOrCtrl+S',
                    click: () => {
                        mainWindow.webContents.send('menu-action', 'save');
                    }
                },
                { type: 'separator' },
                {
                    label: '退出',
                    accelerator: process.platform === 'darwin' ? 'Cmd+Q' : 'Ctrl+Q',
                    click: () => {
                        app.quit();
                    }
                }
            ]
        },
        {
            label: '编辑',
            submenu: [
                {
                    label: '撤销',
                    accelerator: 'CmdOrCtrl+Z',
                    role: 'undo'
                },
                {
                    label: '重做',
                    accelerator: 'Shift+CmdOrCtrl+Z',
                    role: 'redo'
                },
                { type: 'separator' },
                {
                    label: '剪切',
                    accelerator: 'CmdOrCtrl+X',
                    role: 'cut'
                },
                {
                    label: '复制',
                    accelerator: 'CmdOrCtrl+C',
                    role: 'copy'
                },
                {
                    label: '粘贴',
                    accelerator: 'CmdOrCtrl+V',
                    role: 'paste'
                }
            ]
        },
        {
            label: '视图',
            submenu: [
                {
                    label: '重新加载',
                    accelerator: 'CmdOrCtrl+R',
                    click: () => {
                        mainWindow.reload();
                    }
                },
                {
                    label: '强制重新加载',
                    accelerator: 'CmdOrCtrl+Shift+R',
                    click: () => {
                        mainWindow.webContents.reloadIgnoringCache();
                    }
                },
                {
                    label: '开发者工具',
                    accelerator: process.platform === 'darwin' ? 'Alt+Cmd+I' : 'Ctrl+Shift+I',
                    click: () => {
                        mainWindow.webContents.toggleDevTools();
                    }
                },
                { type: 'separator' },
                {
                    label: '实际大小',
                    accelerator: 'CmdOrCtrl+0',
                    role: 'resetZoom'
                },
                {
                    label: '放大',
                    accelerator: 'CmdOrCtrl+Plus',
                    role: 'zoomIn'
                },
                {
                    label: '缩小',
                    accelerator: 'CmdOrCtrl+-',
                    role: 'zoomOut'
                },
                { type: 'separator' },
                {
                    label: '全屏',
                    accelerator: process.platform === 'darwin' ? 'Ctrl+Cmd+F' : 'F11',
                    role: 'togglefullscreen'
                }
            ]
        },
        {
            label: '帮助',
            submenu: [
                {
                    label: '关于',
                    click: async () => {
                        const { response } = await dialog.showMessageBox(mainWindow, {
                            type: 'info',
                            title: '关于应用程式',
                            message: '🎉 我的 Windows 应用程式 v2.0',
                            detail: `✨ 功能特点:
• 现代化用户界面设计
• 智能消息历史管理
• 文件保存和加载功能
• 跨平台支持

🛠️ 技术信息:
• 开发语言: JavaScript
• 框架: Electron ${process.versions.electron}
• Node.js: ${process.versions.node}
• Chrome: ${process.versions.chrome}

👨‍💻 开发信息:
• 开发者: OpenHands
• 版本: 2.0.0
• 更新日期: ${new Date().toLocaleDateString()}
• 许可证: MIT License`,
                            buttons: ['确定', '查看许可证'],
                            defaultId: 0
                        });
                        
                        if (response === 1) {
                            // 显示许可证信息
                            dialog.showMessageBox(mainWindow, {
                                type: 'info',
                                title: 'MIT 许可证',
                                message: 'MIT License',
                                detail: 'Copyright (c) 2025 OpenHands\n\nPermission is hereby granted, free of charge, to any person obtaining a copy of this software...'
                            });
                        }
                    }
                }
            ]
        }
    ];

    const menu = Menu.buildFromTemplate(template);
    Menu.setApplicationMenu(menu);
}

// IPC 处理程序
ipcMain.handle('save-file', async (event, content) => {
    try {
        const { filePath } = await dialog.showSaveDialog(mainWindow, {
            title: '保存消息历史',
            defaultPath: `消息历史_${new Date().toISOString().slice(0, 19).replace(/:/g, '-')}.txt`,
            filters: [
                { name: '文本文件', extensions: ['txt'] },
                { name: '所有文件', extensions: ['*'] }
            ]
        });

        if (filePath) {
            const fullContent = `消息历史 - 导出时间: ${new Date().toLocaleString()}\n${'='.repeat(50)}\n\n${content}`;
            fs.writeFileSync(filePath, fullContent, 'utf8');
            return { success: true, filePath };
        }
        return { success: false, cancelled: true };
    } catch (error) {
        return { success: false, error: error.message };
    }
});

ipcMain.handle('get-app-info', () => {
    return {
        version: app.getVersion(),
        electronVersion: process.versions.electron,
        nodeVersion: process.versions.node,
        chromeVersion: process.versions.chrome,
        platform: process.platform,
        arch: process.arch
    };
});

// 应用程序事件处理
app.whenReady().then(() => {
    createWindow();
    
    // 设置应用程序安全策略
    app.on('web-contents-created', (event, contents) => {
        contents.on('new-window', (event, navigationUrl) => {
            event.preventDefault();
        });
        
        contents.on('will-navigate', (event, navigationUrl) => {
            const parsedUrl = new URL(navigationUrl);
            if (parsedUrl.origin !== 'file://') {
                event.preventDefault();
            }
        });
    });
});

app.on('window-all-closed', () => {
    if (process.platform !== 'darwin') {
        app.quit();
    }
});

app.on('activate', () => {
    if (BrowserWindow.getAllWindows().length === 0) {
        createWindow();
    }
});

// 处理应用程序退出前的清理工作
app.on('before-quit', (event) => {
    // 可以在这里添加保存设置等清理工作
    console.log('应用程序即将退出...');
});