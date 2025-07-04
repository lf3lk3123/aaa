const { dialog } = require('electron').remote || require('@electron/remote');

// DOM 元素
const nameInput = document.getElementById('nameInput');
const outputArea = document.getElementById('outputArea');
const greetBtn = document.getElementById('greetBtn');
const clearBtn = document.getElementById('clearBtn');
const aboutBtn = document.getElementById('aboutBtn');
const exitBtn = document.getElementById('exitBtn');

// 事件监听器
greetBtn.addEventListener('click', greetUser);
clearBtn.addEventListener('click', clearAll);
aboutBtn.addEventListener('click', showAbout);
exitBtn.addEventListener('click', exitApp);

// 回车键触发问候
nameInput.addEventListener('keypress', (e) => {
    if (e.key === 'Enter') {
        greetUser();
    }
});

function greetUser() {
    const name = nameInput.value.trim();
    if (name) {
        const greeting = `你好, ${name}! 欢迎使用这个应用程式!\n`;
        outputArea.value += greeting;
        outputArea.scrollTop = outputArea.scrollHeight;
    } else {
        showAlert('警告', '请先输入您的姓名!');
    }
}

function clearAll() {
    nameInput.value = '';
    outputArea.value = '';
    nameInput.focus();
}

function showAbout() {
    const aboutText = `我的 Windows 应用程式 v1.0

这是一个使用 Electron 开发的桌面应用程式。
可以打包成 Windows exe 文件。

开发者: OpenHands
日期: 2025-07-04`;

    showAlert('关于', aboutText);
}

function exitApp() {
    if (confirm('确定要退出应用程式吗?')) {
        window.close();
    }
}

function showAlert(title, message) {
    // 使用浏览器原生 alert 作为备选
    alert(`${title}\n\n${message}`);
}

// 页面加载完成后聚焦到输入框
window.addEventListener('DOMContentLoaded', () => {
    nameInput.focus();
});