namespace MaidenDashboard
{
    partial class DashboardForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private System.Windows.Forms.Label lblLastRun;
        private System.Windows.Forms.Label lblTotalFreed;
        private System.Windows.Forms.ListView listHistory;
        private System.Windows.Forms.ColumnHeader colHistoryDate;
        private System.Windows.Forms.ColumnHeader colHistoryFreed;
        private System.Windows.Forms.ListView listSizes;
        private System.Windows.Forms.ColumnHeader colSizesFolder;
        private System.Windows.Forms.ColumnHeader colSizesSize;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnClearHistory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblSubheader;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Panel statsPanel;
        private System.Windows.Forms.Label lblHistoryTitle;
        private System.Windows.Forms.Label lblSizesTitle;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem nástrojeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkUpdatesMenuItem;
        private System.Windows.Forms.Panel cleanupProgressPanel;
        private System.Windows.Forms.Label lblCleanupProgress;
        private System.Windows.Forms.ProgressBar cleanupProgressBar;

        private void InitializeComponent()
        {
            lblLastRun = new Label();
            lblTotalFreed = new Label();
            listHistory = new ListView();
            colHistoryDate = new ColumnHeader();
            colHistoryFreed = new ColumnHeader();
            listSizes = new ListView();
            colSizesFolder = new ColumnHeader();
            colSizesSize = new ColumnHeader();
            btnRefresh = new Button();
            btnClearHistory = new Button();
            button1 = new Button();
            label1 = new Label();
            headerPanel = new Panel();
            lblSubheader = new Label();
            lblHeader = new Label();
            statsPanel = new Panel();
            lblHistoryTitle = new Label();
            lblSizesTitle = new Label();
            menuStrip = new MenuStrip();
            nástrojeMenuItem = new ToolStripMenuItem();
            checkUpdatesMenuItem = new ToolStripMenuItem();
            cleanupProgressPanel = new Panel();
            lblCleanupProgress = new Label();
            cleanupProgressBar = new ProgressBar();

            var root = new TableLayoutPanel();
            var statsLayout = new TableLayoutPanel();
            var content = new TableLayoutPanel();
            var buttons = new FlowLayoutPanel();

            headerPanel.SuspendLayout();
            statsPanel.SuspendLayout();
            menuStrip.SuspendLayout();
            cleanupProgressPanel.SuspendLayout();
            root.SuspendLayout();
            statsLayout.SuspendLayout();
            content.SuspendLayout();
            buttons.SuspendLayout();
            SuspendLayout();

            // Menu
            menuStrip.Dock = DockStyle.Top;
            menuStrip.Items.AddRange(new ToolStripItem[] { nástrojeMenuItem });
            menuStrip.Name = "menuStrip";
            menuStrip.TabIndex = 0;
            nástrojeMenuItem.DropDownItems.AddRange(new ToolStripItem[] { checkUpdatesMenuItem });
            nástrojeMenuItem.Name = "nástrojeMenuItem";
            nástrojeMenuItem.Text = "Tools";
            checkUpdatesMenuItem.Name = "checkUpdatesMenuItem";
            checkUpdatesMenuItem.Text = "🔍 Check for Updates";
            checkUpdatesMenuItem.Click += CheckUpdates_Click;

            // Header: docked and content-sized instead of fixed coordinates
            headerPanel.BackColor = Color.FromArgb(90, 60, 200);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Padding = new Padding(18, 10, 18, 10);
            headerPanel.AutoSize = true;
            headerPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            headerPanel.Controls.Add(lblHeader);
            headerPanel.Controls.Add(lblSubheader);
            lblHeader.AutoSize = true;
            lblHeader.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblHeader.ForeColor = Color.White;
            lblHeader.Name = "lblHeader";
            lblHeader.Text = "Maiden";
            lblHeader.Dock = DockStyle.Top;
            lblSubheader.AutoSize = true;
            lblSubheader.Font = new Font("Segoe UI", 11F);
            lblSubheader.ForeColor = Color.FromArgb(220, 215, 255);
            lblSubheader.Name = "lblSubheader";
            lblSubheader.Text = "System Cleanup | Optimization Utility";
            lblSubheader.Dock = DockStyle.Top;

            // Statistics flow into two columns and wrap naturally at larger text scales.
            statsPanel.BackColor = Color.FromArgb(245, 245, 250);
            statsPanel.Dock = DockStyle.Top;
            statsPanel.Padding = new Padding(12, 10, 12, 10);
            statsPanel.AutoSize = true;
            statsPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            statsPanel.Controls.Add(statsLayout);
            statsLayout.Dock = DockStyle.Fill;
            statsLayout.AutoSize = true;
            statsLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            statsLayout.ColumnCount = 2;
            statsLayout.RowCount = 1;
            statsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            statsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            statsLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            lblLastRun.AutoSize = true;
            lblLastRun.Dock = DockStyle.Fill;
            lblLastRun.Font = new Font("Segoe UI", 11F);
            lblLastRun.ForeColor = Color.FromArgb(60, 60, 60);
            lblLastRun.Name = "lblLastRun";
            lblLastRun.Text = "⏱️  Last cleanup: -";
            lblLastRun.Margin = new Padding(4, 3, 8, 3);
            lblTotalFreed.AutoSize = true;
            lblTotalFreed.Dock = DockStyle.Fill;
            lblTotalFreed.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTotalFreed.ForeColor = Color.FromArgb(90, 60, 200);
            lblTotalFreed.Name = "lblTotalFreed";
            lblTotalFreed.Text = "💾 Total freed: -";
            lblTotalFreed.TextAlign = ContentAlignment.MiddleRight;
            lblTotalFreed.Margin = new Padding(8, 3, 4, 3);
            statsLayout.Controls.Add(lblLastRun, 0, 0);
            statsLayout.Controls.Add(lblTotalFreed, 1, 0);

            // Main area uses percentage rows, so content remains usable at high DPI.
            root.Dock = DockStyle.Fill;
            root.Padding = new Padding(16, 10, 16, 12);
            root.ColumnCount = 1;
            root.RowCount = 1;
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            root.Controls.Add(content, 0, 0);

            content.Dock = DockStyle.Fill;
            content.ColumnCount = 1;
            content.RowCount = 6;
            content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            content.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            content.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
            content.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            content.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            content.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            content.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            lblHistoryTitle.AutoSize = true;
            lblHistoryTitle.Dock = DockStyle.Fill;
            lblHistoryTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblHistoryTitle.ForeColor = Color.FromArgb(90, 60, 200);
            lblHistoryTitle.Name = "lblHistoryTitle";
            lblHistoryTitle.Text = "📋 Cleanup History";
            lblHistoryTitle.Margin = new Padding(2, 5, 2, 6);

            listHistory.BackColor = Color.FromArgb(250, 250, 255);
            listHistory.BorderStyle = BorderStyle.FixedSingle;
            listHistory.Columns.AddRange(new ColumnHeader[] { colHistoryDate, colHistoryFreed });
            listHistory.Dock = DockStyle.Fill;
            listHistory.FullRowSelect = true;
            listHistory.Name = "listHistory";
            listHistory.UseCompatibleStateImageBehavior = false;
            listHistory.View = View.Details;
            listHistory.Margin = new Padding(0, 0, 0, 8);
            listHistory.SelectedIndexChanged += listHistory_SelectedIndexChanged;
            colHistoryDate.Text = "Date/Time";
            colHistoryDate.Width = 300;
            colHistoryFreed.Text = "Freed";
            colHistoryFreed.TextAlign = HorizontalAlignment.Right;
            colHistoryFreed.Width = 180;

            lblSizesTitle.AutoSize = true;
            lblSizesTitle.Dock = DockStyle.Fill;
            lblSizesTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblSizesTitle.ForeColor = Color.FromArgb(90, 60, 200);
            lblSizesTitle.Name = "lblSizesTitle";
            lblSizesTitle.Text = "📁 Folders to Clean";
            lblSizesTitle.Margin = new Padding(2, 2, 2, 6);

            listSizes.BackColor = Color.FromArgb(250, 250, 255);
            listSizes.BorderStyle = BorderStyle.FixedSingle;
            listSizes.Columns.AddRange(new ColumnHeader[] { colSizesFolder, colSizesSize });
            listSizes.Dock = DockStyle.Fill;
            listSizes.Name = "listSizes";
            listSizes.UseCompatibleStateImageBehavior = false;
            listSizes.View = View.Details;
            listSizes.Margin = new Padding(0, 0, 0, 10);
            colSizesFolder.Text = "Location";
            colSizesFolder.Width = 300;
            colSizesSize.Text = "Current Size";
            colSizesSize.TextAlign = HorizontalAlignment.Right;
            colSizesSize.Width = 180;

            buttons.Dock = DockStyle.Fill;
            buttons.AutoSize = true;
            buttons.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            buttons.FlowDirection = FlowDirection.LeftToRight;
            buttons.WrapContents = true;
            buttons.Padding = new Padding(0, 2, 0, 0);
            buttons.Controls.Add(btnRefresh);
            buttons.Controls.Add(button1);
            buttons.Controls.Add(btnClearHistory);

            ConfigureButton(btnRefresh, "🔄 Scan", Color.FromArgb(100, 150, 255));
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Click += BtnRefresh_Click;
            ConfigureButton(button1, "🧹 Cleanup", Color.FromArgb(75, 180, 75));
            button1.Name = "button1";
            button1.Click += Button1_Click;
            ConfigureButton(btnClearHistory, "❌ Clear History", Color.FromArgb(220, 100, 100));
            btnClearHistory.Name = "btnClearHistory";
            btnClearHistory.Click += BtnClearHistory_Click;

            cleanupProgressPanel.Dock = DockStyle.Fill;
            cleanupProgressPanel.AutoSize = true;
            cleanupProgressPanel.MinimumSize = new Size(0, 42);
            cleanupProgressPanel.Visible = false;
            cleanupProgressPanel.Controls.Add(cleanupProgressBar);
            cleanupProgressPanel.Controls.Add(lblCleanupProgress);
            lblCleanupProgress.AutoSize = true;
            lblCleanupProgress.Dock = DockStyle.Top;
            lblCleanupProgress.Name = "lblCleanupProgress";
            cleanupProgressBar.Dock = DockStyle.Bottom;
            cleanupProgressBar.Name = "cleanupProgressBar";
            cleanupProgressBar.Minimum = 0;
            cleanupProgressBar.Maximum = 100;
            cleanupProgressBar.Height = 18;

            content.Controls.Add(lblHistoryTitle, 0, 0);
            content.Controls.Add(listHistory, 0, 1);
            content.Controls.Add(lblSizesTitle, 0, 2);
            content.Controls.Add(listSizes, 0, 3);
            content.Controls.Add(cleanupProgressPanel, 0, 4);
            content.Controls.Add(buttons, 0, 5);

            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(100, 23);
            label1.Visible = false;

            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScaleDimensions = new SizeF(96F, 96F);
            BackColor = Color.White;
            ClientSize = new Size(760, 650);
            MinimumSize = new Size(520, 520);
            Controls.Add(root);
            Controls.Add(statsPanel);
            Controls.Add(headerPanel);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Name = "DashboardForm";
            Text = "Maiden - System Cleanup";

            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            statsPanel.ResumeLayout(false);
            statsPanel.PerformLayout();
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            cleanupProgressPanel.ResumeLayout(false);
            cleanupProgressPanel.PerformLayout();
            root.ResumeLayout(false);
            statsLayout.ResumeLayout(false);
            statsLayout.PerformLayout();
            content.ResumeLayout(false);
            content.PerformLayout();
            buttons.ResumeLayout(false);
            buttons.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

            // Keep list columns proportional when the window is resized.
            Resize += DashboardForm_ResponsiveResize;
            DashboardForm_ResponsiveResize(this, EventArgs.Empty);
        }

        private static void ConfigureButton(Button button, string text, Color color)
        {
            button.AutoSize = true;
            button.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button.MinimumSize = new Size(112, 38);
            button.Padding = new Padding(12, 5, 12, 5);
            button.Margin = new Padding(0, 0, 10, 4);
            button.BackColor = color;
            button.Cursor = Cursors.Hand;
            button.FlatAppearance.BorderSize = 0;
            button.FlatStyle = FlatStyle.Flat;
            button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            button.ForeColor = Color.White;
            button.Text = text;
            button.UseVisualStyleBackColor = false;
        }

        private void DashboardForm_ResponsiveResize(object sender, EventArgs e)
        {
            if (listHistory == null || listSizes == null) return;
            int historyWidth = Math.Max(80, listHistory.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 4);
            int sizesWidth = Math.Max(80, listSizes.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 4);
            colHistoryDate.Width = (int)(historyWidth * 0.67);
            colHistoryFreed.Width = Math.Max(50, historyWidth - colHistoryDate.Width);
            colSizesFolder.Width = (int)(sizesWidth * 0.67);
            colSizesSize.Width = Math.Max(50, sizesWidth - colSizesFolder.Width);
        }
    }
}
