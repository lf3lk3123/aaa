const { contextBridge, ipcRenderer } = require('electron');

// 安全地暴露 API 到渲染进程
contextBridge.exposeInMainWorld('electronAPI', {
    // 保存文件
    saveFile: (content) => ipcRenderer.invoke('save-file', content),
    
    // 获取应用信息
    getAppInfo: () => ipcRenderer.invoke('get-app-info'),
    
    // 监听菜单操作
    onMenuAction: (callback) => {
        ipcRenderer.on('menu-action', (event, action) => callback(action));
    },
    
    // 移除菜单操作监听器
    removeMenuActionListener: () => {
        ipcRenderer.removeAllListeners('menu-action');
    },
    
    // 获取平台信息
    platform: process.platform,
    
    // 版本信息
    versions: {
        node: process.versions.node,
        chrome: process.versions.chrome,
        electron: process.versions.electron
    }
});

// 在窗口加载完成时通知渲染进程
window.addEventListener('DOMContentLoaded', () => {
    console.log('Preload script loaded successfully');
});