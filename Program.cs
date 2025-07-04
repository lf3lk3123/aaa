using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Text;
using System.Globalization;

namespace MyWindowsApp
{
    public partial class MainForm : Form
    {
        private TextBox nameTextBox;
        private TextBox outputTextBox;
        private Button greetButton;
        private Button clearButton;
        private Button saveButton;
        private Button aboutButton;
        private Button exitButton;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
        private ToolStripStatusLabel messageCountLabel;
        private int messageCount = 0;

        public MainForm()
        {
            InitializeComponent();
            this.Load += MainForm_Load;
        }

        private void InitializeComponent()
        {
            // 窗口设置
            this.Text = "🎉 我的 Windows 应用程式 v2.0";
            this.Size = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimumSize = new Size(500, 400);
            this.Icon = LoadIcon();

            // 主面板
            Panel mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Padding = new Padding(15);
            this.Controls.Add(mainPanel);

            // 标题区域
            Panel titlePanel = new Panel();
            titlePanel.Location = new Point(15, 15);
            titlePanel.Size = new Size(550, 60);
            titlePanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            mainPanel.Controls.Add(titlePanel);

            Label titleLabel = new Label();
            titleLabel.Text = "🎉 欢迎使用我的应用程式";
            titleLabel.Font = new Font("Microsoft YaHei", 16, FontStyle.Bold);
            titleLabel.ForeColor = Color.FromArgb(33, 150, 243);
            titleLabel.Location = new Point(0, 5);
            titleLabel.Size = new Size(400, 30);
            titlePanel.Controls.Add(titleLabel);

            Label subtitleLabel = new Label();
            subtitleLabel.Text = "现代化 C# Windows Forms 应用程式";
            subtitleLabel.Font = new Font("Microsoft YaHei", 10);
            subtitleLabel.ForeColor = Color.Gray;
            subtitleLabel.Location = new Point(0, 35);
            subtitleLabel.Size = new Size(300, 20);
            titlePanel.Controls.Add(subtitleLabel);

            // 输入区域
            GroupBox inputGroup = new GroupBox();
            inputGroup.Text = "用户信息";
            inputGroup.Font = new Font("Microsoft YaHei", 10);
            inputGroup.Location = new Point(15, 85);
            inputGroup.Size = new Size(550, 60);
            inputGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            mainPanel.Controls.Add(inputGroup);

            Label nameLabel = new Label();
            nameLabel.Text = "请输入您的姓名:";
            nameLabel.Font = new Font("Microsoft YaHei", 10);
            nameLabel.Location = new Point(15, 25);
            nameLabel.Size = new Size(120, 20);
            inputGroup.Controls.Add(nameLabel);

            nameTextBox = new TextBox();
            nameTextBox.Font = new Font("Microsoft YaHei", 10);
            nameTextBox.Location = new Point(140, 23);
            nameTextBox.Size = new Size(390, 25);
            nameTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            nameTextBox.KeyPress += NameTextBox_KeyPress;
            inputGroup.Controls.Add(nameTextBox);

            // 按钮区域
            Panel buttonPanel = new Panel();
            buttonPanel.Location = new Point(15, 155);
            buttonPanel.Size = new Size(550, 40);
            buttonPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            mainPanel.Controls.Add(buttonPanel);

            // 创建按钮
            CreateButton(ref greetButton, "🤝 问候", new Point(0, 5), Color.FromArgb(76, 175, 80), GreetButton_Click);
            CreateButton(ref clearButton, "🗑️ 清除", new Point(110, 5), Color.FromArgb(255, 152, 0), ClearButton_Click);
            CreateButton(ref saveButton, "💾 保存", new Point(220, 5), Color.FromArgb(33, 150, 243), SaveButton_Click);
            CreateButton(ref aboutButton, "ℹ️ 关于", new Point(330, 5), Color.FromArgb(156, 39, 176), AboutButton_Click);
            CreateButton(ref exitButton, "❌ 退出", new Point(440, 5), Color.FromArgb(244, 67, 54), ExitButton_Click);

            buttonPanel.Controls.AddRange(new Control[] { greetButton, clearButton, saveButton, aboutButton, exitButton });

            // 消息显示区域
            GroupBox messageGroup = new GroupBox();
            messageGroup.Text = "消息历史";
            messageGroup.Font = new Font("Microsoft YaHei", 10);
            messageGroup.Location = new Point(15, 205);
            messageGroup.Size = new Size(550, 220);
            messageGroup.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            mainPanel.Controls.Add(messageGroup);

            outputTextBox = new TextBox();
            outputTextBox.Font = new Font("Microsoft YaHei", 10);
            outputTextBox.Location = new Point(15, 25);
            outputTextBox.Size = new Size(520, 180);
            outputTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            outputTextBox.Multiline = true;
            outputTextBox.ScrollBars = ScrollBars.Vertical;
            outputTextBox.ReadOnly = true;
            outputTextBox.BackColor = Color.FromArgb(248, 249, 250);
            messageGroup.Controls.Add(outputTextBox);

            // 状态栏
            statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel("就绪");
            messageCountLabel = new ToolStripStatusLabel("消息数: 0");
            messageCountLabel.Spring = true;
            messageCountLabel.TextAlign = ContentAlignment.MiddleRight;
            
            statusStrip.Items.AddRange(new ToolStripItem[] { statusLabel, messageCountLabel });
            this.Controls.Add(statusStrip);
        }

        private void CreateButton(ref Button button, string text, Point location, Color backColor, EventHandler clickHandler)
        {
            button = new Button();
            button.Text = text;
            button.Location = location;
            button.Size = new Size(100, 30);
            button.Font = new Font("Microsoft YaHei", 9);
            button.BackColor = backColor;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Cursor = Cursors.Hand;
            button.Click += clickHandler;
        }

        private Icon LoadIcon()
        {
            try
            {
                string iconPath = Path.Combine(Application.StartupPath, "icon.ico");
                if (File.Exists(iconPath))
                {
                    return new Icon(iconPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"无法加载图标: {ex.Message}");
            }
            return null;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            UpdateStatus("应用程式已启动");
            LoadSettings();
        }

        private void NameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                GreetButton_Click(sender, e);
                e.Handled = true;
            }
        }

        private void GreetButton_Click(object sender, EventArgs e)
        {
            try
            {
                string name = nameTextBox.Text.Trim();
                if (string.IsNullOrEmpty(name))
                {
                    MessageBox.Show("⚠️ 请先输入您的姓名!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    nameTextBox.Focus();
                    return;
                }

                messageCount++;
                string currentTime = DateTime.Now.ToString("HH:mm:ss");
                string greeting = $"[{currentTime}] 🤝 你好, {name}! 欢迎使用这个应用程式! (#{messageCount})\r\n";
                
                outputTextBox.AppendText(greeting);
                outputTextBox.ScrollToCaret();
                
                UpdateStatus($"已问候 {name}");
                UpdateMessageCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ 问候时发生错误:\n{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (messageCount > 0)
                {
                    DialogResult result = MessageBox.Show(
                        $"🗑️ 确定要清除所有 {messageCount} 条消息吗?",
                        "确认清除",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        nameTextBox.Clear();
                        outputTextBox.Clear();
                        messageCount = 0;
                        UpdateStatus("已清除所有消息");
                        UpdateMessageCount();
                    }
                }
                else
                {
                    nameTextBox.Clear();
                    outputTextBox.Clear();
                    UpdateStatus("已清除");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ 清除时发生错误:\n{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (messageCount == 0)
                {
                    MessageBox.Show("💾 没有消息可保存!", "保存", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "文本文件 (*.txt)|*.txt|所有文件 (*.*)|*.*";
                    saveDialog.DefaultExt = "txt";
                    saveDialog.Title = "保存消息历史";
                    saveDialog.FileName = $"消息历史_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        StringBuilder content = new StringBuilder();
                        content.AppendLine($"消息历史 - 导出时间: {DateTime.Now}");
                        content.AppendLine(new string('=', 50));
                        content.AppendLine();
                        content.AppendLine(outputTextBox.Text);

                        File.WriteAllText(saveDialog.FileName, content.ToString(), Encoding.UTF8);
                        
                        MessageBox.Show($"💾 消息已保存到:\n{saveDialog.FileName}", "保存成功", 
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                        UpdateStatus($"已保存 {messageCount} 条消息");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ 保存时发生错误:\n{ex.Message}", "保存失败", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            string aboutText = $@"🎉 我的 Windows 应用程式 v2.0

✨ 功能特点:
• 现代化用户界面设计
• 智能消息历史管理
• 文件保存和加载功能
• 增强的错误处理
• 响应式布局设计

🛠️ 技术信息:
• 开发语言: C# (.NET {Environment.Version})
• GUI 框架: Windows Forms
• 支持平台: Windows

👨‍💻 开发信息:
• 开发者: OpenHands
• 版本: 2.0.0
• 更新日期: {DateTime.Now:yyyy-MM-dd}
• 许可证: MIT License

📊 当前会话:
• 消息数量: {messageCount}
• 运行时间: {DateTime.Now:HH:mm:ss}";

            MessageBox.Show(aboutText, "ℹ️ 关于应用程式", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            try
            {
                SaveSettings();
                string exitMessage = $"❌ 确定要退出应用程式吗?\n\n当前会话信息:\n• 消息数: {messageCount}\n• 设置已自动保存";
                
                if (MessageBox.Show(exitMessage, "退出确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"退出时发生错误: {ex.Message}");
                Application.Exit();
            }
        }

        private void UpdateStatus(string message)
        {
            statusLabel.Text = message;
            
            // 3秒后恢复为"就绪"
            Timer timer = new Timer();
            timer.Interval = 3000;
            timer.Tick += (s, e) =>
            {
                statusLabel.Text = "就绪";
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();
        }

        private void UpdateMessageCount()
        {
            messageCountLabel.Text = $"消息数: {messageCount}";
        }

        private void LoadSettings()
        {
            try
            {
                string settingsPath = Path.Combine(Application.StartupPath, "settings.txt");
                if (File.Exists(settingsPath))
                {
                    string[] lines = File.ReadAllLines(settingsPath, Encoding.UTF8);
                    if (lines.Length > 0)
                    {
                        nameTextBox.Text = lines[0];
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载设置失败: {ex.Message}");
            }
        }

        private void SaveSettings()
        {
            try
            {
                string settingsPath = Path.Combine(Application.StartupPath, "settings.txt");
                string[] settings = {
                    nameTextBox.Text,
                    messageCount.ToString(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };
                File.WriteAllLines(settingsPath, settings, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"保存设置失败: {ex.Message}");
            }
        }
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}