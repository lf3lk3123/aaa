using System;
using System.Windows.Forms;
using System.Drawing;

namespace MyWindowsApp
{
    public partial class MainForm : Form
    {
        private TextBox nameTextBox;
        private TextBox outputTextBox;
        private Button greetButton;
        private Button clearButton;
        private Button aboutButton;
        private Button exitButton;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // 窗口设置
            this.Text = "我的 Windows 应用程式";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // 标题标签
            Label titleLabel = new Label();
            titleLabel.Text = "欢迎使用我的应用程式";
            titleLabel.Font = new Font("Microsoft YaHei", 14, FontStyle.Bold);
            titleLabel.Location = new Point(150, 20);
            titleLabel.Size = new Size(200, 30);
            this.Controls.Add(titleLabel);

            // 姓名输入
            Label nameLabel = new Label();
            nameLabel.Text = "请输入您的姓名:";
            nameLabel.Location = new Point(20, 70);
            nameLabel.Size = new Size(120, 20);
            this.Controls.Add(nameLabel);

            nameTextBox = new TextBox();
            nameTextBox.Location = new Point(150, 68);
            nameTextBox.Size = new Size(200, 25);
            this.Controls.Add(nameTextBox);

            // 按钮
            greetButton = new Button();
            greetButton.Text = "问候";
            greetButton.Location = new Point(20, 110);
            greetButton.Size = new Size(80, 30);
            greetButton.Click += GreetButton_Click;
            this.Controls.Add(greetButton);

            clearButton = new Button();
            clearButton.Text = "清除";
            clearButton.Location = new Point(110, 110);
            clearButton.Size = new Size(80, 30);
            clearButton.Click += ClearButton_Click;
            this.Controls.Add(clearButton);

            aboutButton = new Button();
            aboutButton.Text = "关于";
            aboutButton.Location = new Point(200, 110);
            aboutButton.Size = new Size(80, 30);
            aboutButton.Click += AboutButton_Click;
            this.Controls.Add(aboutButton);

            exitButton = new Button();
            exitButton.Text = "退出";
            exitButton.Location = new Point(290, 110);
            exitButton.Size = new Size(80, 30);
            exitButton.Click += ExitButton_Click;
            this.Controls.Add(exitButton);

            // 输出文本框
            outputTextBox = new TextBox();
            outputTextBox.Location = new Point(20, 160);
            outputTextBox.Size = new Size(440, 180);
            outputTextBox.Multiline = true;
            outputTextBox.ScrollBars = ScrollBars.Vertical;
            outputTextBox.ReadOnly = true;
            this.Controls.Add(outputTextBox);
        }

        private void GreetButton_Click(object sender, EventArgs e)
        {
            string name = nameTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(name))
            {
                string greeting = $"你好, {name}! 欢迎使用这个应用程式!\r\n";
                outputTextBox.AppendText(greeting);
            }
            else
            {
                MessageBox.Show("请先输入您的姓名!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            nameTextBox.Clear();
            outputTextBox.Clear();
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            string aboutText = "我的 Windows 应用程式 v1.0\n\n" +
                              "这是一个使用 C# 和 Windows Forms 开发的示例应用程式。\n" +
                              "可以编译成 Windows exe 文件。\n\n" +
                              "开发者: OpenHands\n" +
                              "日期: 2025-07-04";
            
            MessageBox.Show(aboutText, "关于", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要退出应用程式吗?", "退出", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
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