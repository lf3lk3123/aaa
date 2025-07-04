// 应用程序状态
let messageCount = 0;
let appInfo = null;

// DOM 元素
const nameInput = document.getElementById('nameInput');
const outputArea = document.getElementById('outputArea');
const greetBtn = document.getElementById('greetBtn');
const clearBtn = document.getElementById('clearBtn');
const saveBtn = document.getElementById('saveBtn');
const aboutBtn = document.getElementById('aboutBtn');
const exitBtn = document.getElementById('exitBtn');
const statusLabel = document.getElementById('statusLabel');
const messageCountLabel = document.getElementById('messageCountLabel');

// 初始化应用程序
async function initializeApp() {
    try {
        // 获取应用信息
        appInfo = await window.electronAPI.getAppInfo();
        
        // 设置事件监听器
        setupEventListeners();
        
        // 设置菜单操作监听
        window.electronAPI.onMenuAction(handleMenuAction);
        
        // 更新状态
        updateStatus('应用程式已启动');
        updateMessageCount();
        
        // 聚焦到输入框
        nameInput.focus();
        
        console.log('应用程序初始化完成', appInfo);
    } catch (error) {
        console.error('初始化失败:', error);
        showNotification('初始化失败', error.message, 'error');
    }
}

// 设置事件监听器
function setupEventListeners() {
    greetBtn.addEventListener('click', greetUser);
    clearBtn.addEventListener('click', clearAll);
    saveBtn.addEventListener('click', saveMessages);
    aboutBtn.addEventListener('click', showAbout);
    exitBtn.addEventListener('click', exitApp);
    
    // 回车键触发问候
    nameInput.addEventListener('keypress', (e) => {
        if (e.key === 'Enter') {
            greetUser();
        }
    });
    
    // 输入框变化时更新状态
    nameInput.addEventListener('input', () => {
        if (nameInput.value.trim()) {
            updateStatus('准备问候...');
        } else {
            updateStatus('请输入姓名');
        }
    });
}

// 处理菜单操作
function handleMenuAction(action) {
    switch (action) {
        case 'clear':
            clearAll();
            break;
        case 'save':
            saveMessages();
            break;
        default:
            console.log('未知菜单操作:', action);
    }
}

// 问候用户 - 增强版
function greetUser() {
    try {
        const name = nameInput.value.trim();
        if (!name) {
            showNotification('⚠️ 警告', '请先输入您的姓名!', 'warning');
            nameInput.focus();
            return;
        }

        messageCount++;
        const currentTime = new Date().toLocaleTimeString();
        const greeting = `[${currentTime}] 🤝 你好, ${name}! 欢迎使用这个应用程式! (#${messageCount})\n`;
        
        outputArea.value += greeting;
        outputArea.scrollTop = outputArea.scrollHeight;
        
        updateStatus(`已问候 ${name}`);
        updateMessageCount();
        
        // 添加动画效果
        greetBtn.classList.add('button-clicked');
        setTimeout(() => greetBtn.classList.remove('button-clicked'), 200);
        
    } catch (error) {
        console.error('问候时发生错误:', error);
        showNotification('❌ 错误', `问候时发生错误: ${error.message}`, 'error');
    }
}

// 清除所有内容 - 增强版
function clearAll() {
    try {
        if (messageCount > 0) {
            if (confirm(`🗑️ 确定要清除所有 ${messageCount} 条消息吗?`)) {
                nameInput.value = '';
                outputArea.value = '';
                messageCount = 0;
                updateStatus('已清除所有消息');
                updateMessageCount();
                nameInput.focus();
            }
        } else {
            nameInput.value = '';
            outputArea.value = '';
            updateStatus('已清除');
            nameInput.focus();
        }
    } catch (error) {
        console.error('清除时发生错误:', error);
        showNotification('❌ 错误', `清除时发生错误: ${error.message}`, 'error');
    }
}

// 保存消息到文件
async function saveMessages() {
    try {
        if (messageCount === 0) {
            showNotification('💾 保存', '没有消息可保存!', 'info');
            return;
        }

        const content = outputArea.value;
        const result = await window.electronAPI.saveFile(content);
        
        if (result.success) {
            showNotification('💾 保存成功', `消息已保存到:\n${result.filePath}`, 'success');
            updateStatus(`已保存 ${messageCount} 条消息`);
        } else if (result.cancelled) {
            updateStatus('保存已取消');
        } else {
            showNotification('❌ 保存失败', `保存时发生错误:\n${result.error}`, 'error');
        }
    } catch (error) {
        console.error('保存时发生错误:', error);
        showNotification('❌ 保存失败', `保存时发生错误: ${error.message}`, 'error');
    }
}

// 显示关于信息 - 增强版
async function showAbout() {
    try {
        const info = appInfo || await window.electronAPI.getAppInfo();
        const aboutText = `🎉 我的 Windows 应用程式 v2.0

✨ 功能特点:
• 现代化用户界面设计
• 智能消息历史管理
• 文件保存和加载功能
• 跨平台支持
• 安全的进程间通信

🛠️ 技术信息:
• 开发语言: JavaScript
• 框架: Electron ${info.electronVersion}
• Node.js: ${info.nodeVersion}
• Chrome: ${info.chromeVersion}
• 平台: ${info.platform} (${info.arch})

👨‍💻 开发信息:
• 开发者: OpenHands
• 版本: 2.0.0
• 更新日期: ${new Date().toLocaleDateString()}
• 许可证: MIT License

📊 当前会话:
• 消息数量: ${messageCount}
• 运行时间: ${new Date().toLocaleTimeString()}`;

        showNotification('ℹ️ 关于应用程式', aboutText, 'info');
    } catch (error) {
        console.error('显示关于信息时发生错误:', error);
        showNotification('❌ 错误', `显示关于信息时发生错误: ${error.message}`, 'error');
    }
}

// 退出应用程序
function exitApp() {
    try {
        const exitMessage = `❌ 确定要退出应用程式吗?\n\n当前会话信息:\n• 消息数: ${messageCount}\n• 运行时间: ${new Date().toLocaleTimeString()}`;
        
        if (confirm(exitMessage)) {
            window.close();
        }
    } catch (error) {
        console.error('退出时发生错误:', error);
        window.close();
    }
}

// 显示通知 - 增强版
function showNotification(title, message, type = 'info') {
    // 创建自定义通知元素
    const notification = document.createElement('div');
    notification.className = `notification notification-${type}`;
    notification.innerHTML = `
        <div class="notification-header">
            <strong>${title}</strong>
            <button class="notification-close">&times;</button>
        </div>
        <div class="notification-body">${message.replace(/\n/g, '<br>')}</div>
    `;
    
    // 添加到页面
    document.body.appendChild(notification);
    
    // 设置关闭事件
    const closeBtn = notification.querySelector('.notification-close');
    closeBtn.addEventListener('click', () => {
        notification.remove();
    });
    
    // 自动关闭
    setTimeout(() => {
        if (notification.parentNode) {
            notification.remove();
        }
    }, type === 'error' ? 8000 : 5000);
    
    // 添加动画
    setTimeout(() => notification.classList.add('notification-show'), 10);
}

// 更新状态栏
function updateStatus(message) {
    if (statusLabel) {
        statusLabel.textContent = message;
        
        // 3秒后恢复为"就绪"
        setTimeout(() => {
            if (statusLabel.textContent === message) {
                statusLabel.textContent = '就绪';
            }
        }, 3000);
    }
}

// 更新消息计数
function updateMessageCount() {
    if (messageCountLabel) {
        messageCountLabel.textContent = `消息数: ${messageCount}`;
    }
}

// 页面加载完成后初始化
window.addEventListener('DOMContentLoaded', initializeApp);

// 页面卸载时清理
window.addEventListener('beforeunload', () => {
    if (window.electronAPI && window.electronAPI.removeMenuActionListener) {
        window.electronAPI.removeMenuActionListener();
    }
});