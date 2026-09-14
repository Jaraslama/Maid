namespace MaidenDashboard
{
    partial class DashboardForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
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
            headerPanel.SuspendLayout();
            statsPanel.SuspendLayout();
            menuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // lblLastRun
            // 
            lblLastRun.Font = new Font("Segoe UI", 11F);
            lblLastRun.ForeColor = Color.FromArgb(60, 60, 60);
            lblLastRun.Location = new Point(20, 15);
            lblLastRun.Name = "lblLastRun";
            lblLastRun.Size = new Size(280, 25);
            lblLastRun.TabIndex = 1;
            lblLastRun.Text = "⏱️  Last cleanup: -";
            // 
            // lblTotalFreed
            // 
            lblTotalFreed.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTotalFreed.ForeColor = Color.FromArgb(90, 60, 200);
            lblTotalFreed.Location = new Point(300, 15);
            lblTotalFreed.Name = "lblTotalFreed";
            lblTotalFreed.Size = new Size(280, 25);
            lblTotalFreed.TabIndex = 0;
            lblTotalFreed.Text = "💾 Total freed: -";
            lblTotalFreed.TextAlign = ContentAlignment.MiddleRight;
            // 
            // listHistory
            // 
            listHistory.BackColor = Color.FromArgb(250, 250, 255);
            listHistory.BorderStyle = BorderStyle.FixedSingle;
            listHistory.Columns.AddRange(new ColumnHeader[] { colHistoryDate, colHistoryFreed });
            listHistory.Location = new Point(20, 195);
            listHistory.Name = "listHistory";
            listHistory.Size = new Size(560, 130);
            listHistory.TabIndex = 2;
            listHistory.UseCompatibleStateImageBehavior = false;
            listHistory.View = View.Details;
            listHistory.SelectedIndexChanged += listHistory_SelectedIndexChanged;
            // 
            // colHistoryDate
            // 
            colHistoryDate.Text = "Date/Time";
            colHistoryDate.Width = 280;
            // 
            // colHistoryFreed
            // 
            colHistoryFreed.Text = "Freed";
            colHistoryFreed.TextAlign = HorizontalAlignment.Right;
            colHistoryFreed.Width = 250;
            // 
            // listSizes
            // 
            listSizes.BackColor = Color.FromArgb(250, 250, 255);
            listSizes.BorderStyle = BorderStyle.FixedSingle;
            listSizes.Columns.AddRange(new ColumnHeader[] { colSizesFolder, colSizesSize });
            listSizes.Location = new Point(20, 365);
            listSizes.Name = "listSizes";
            listSizes.Size = new Size(560, 75);
            listSizes.TabIndex = 3;
            listSizes.UseCompatibleStateImageBehavior = false;
            listSizes.View = View.Details;
            // 
            // colSizesFolder
            // 
            colSizesFolder.Text = "Location";
            colSizesFolder.Width = 280;
            // 
            // colSizesSize
            // 
            colSizesSize.Text = "Current Size";
            colSizesSize.TextAlign = HorizontalAlignment.Right;
            colSizesSize.Width = 250;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(100, 150, 255);
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(20, 455);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(130, 35);
            btnRefresh.TabIndex = 4;
            btnRefresh.Text = "🔄 Scan";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += BtnRefresh_Click;
            // 
            // btnClearHistory
            // 
            btnClearHistory.BackColor = Color.FromArgb(220, 100, 100);
            btnClearHistory.Cursor = Cursors.Hand;
            btnClearHistory.FlatAppearance.BorderSize = 0;
            btnClearHistory.FlatStyle = FlatStyle.Flat;
            btnClearHistory.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnClearHistory.ForeColor = Color.White;
            btnClearHistory.Location = new Point(471, 325);
            btnClearHistory.Name = "btnClearHistory";
            btnClearHistory.Size = new Size(100, 35);
            btnClearHistory.TabIndex = 6;
            btnClearHistory.Text = "❌ Clear";
            btnClearHistory.UseVisualStyleBackColor = false;
            btnClearHistory.Click += BtnClearHistory_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(75, 180, 75);
            button1.Cursor = Cursors.Hand;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            button1.ForeColor = Color.White;
            button1.Location = new Point(450, 455);
            button1.Name = "button1";
            button1.Size = new Size(130, 35);
            button1.TabIndex = 5;
            button1.Text = "\U0001f9f9 Cleanup";
            button1.UseVisualStyleBackColor = false;
            button1.Click += Button1_Click;
            // 
            // label1
            // 
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(100, 23);
            label1.TabIndex = 0;
            label1.Visible = false;
            // 
            // headerPanel
            // 
            headerPanel.BackColor = Color.FromArgb(90, 60, 200);
            headerPanel.Controls.Add(lblSubheader);
            headerPanel.Controls.Add(lblHeader);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Location = new Point(0, 24);
            headerPanel.Name = "headerPanel";
            headerPanel.Size = new Size(600, 80);
            headerPanel.TabIndex = 7;
            // 
            // lblSubheader
            // 
            lblSubheader.Font = new Font("Segoe UI", 11F);
            lblSubheader.ForeColor = Color.FromArgb(200, 200, 255);
            lblSubheader.Location = new Point(124, 31);
            lblSubheader.Name = "lblSubheader";
            lblSubheader.Size = new Size(438, 22);
            lblSubheader.TabIndex = 0;
            lblSubheader.Text = "System Cleanup | Optimization Utility";
            // 
            // lblHeader
            // 
            lblHeader.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblHeader.ForeColor = Color.White;
            lblHeader.Location = new Point(0, 0);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new Size(133, 40);
            lblHeader.TabIndex = 1;
            lblHeader.Text = "Maiden";
            // 
            // statsPanel
            // 
            statsPanel.BackColor = Color.FromArgb(245, 245, 250);
            statsPanel.Controls.Add(lblTotalFreed);
            statsPanel.Controls.Add(lblLastRun);
            statsPanel.Location = new Point(0, 80);
            statsPanel.Name = "statsPanel";
            statsPanel.Size = new Size(600, 75);
            statsPanel.TabIndex = 5;
            // 
            // lblHistoryTitle
            // 
            lblHistoryTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblHistoryTitle.ForeColor = Color.FromArgb(90, 60, 200);
            lblHistoryTitle.Location = new Point(20, 165);
            lblHistoryTitle.Name = "lblHistoryTitle";
            lblHistoryTitle.Size = new Size(300, 25);
            lblHistoryTitle.TabIndex = 4;
            lblHistoryTitle.Text = "📋 Cleanup History";
            // 
            // lblSizesTitle
            // 
            lblSizesTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblSizesTitle.ForeColor = Color.FromArgb(90, 60, 200);
            lblSizesTitle.Location = new Point(20, 335);
            lblSizesTitle.Name = "lblSizesTitle";
            lblSizesTitle.Size = new Size(300, 25);
            lblSizesTitle.TabIndex = 0;
            lblSizesTitle.Text = "📁 Folders to Clean";
            // 
            // menuStrip
            // 
            menuStrip.Items.AddRange(new ToolStripItem[] { nástrojeMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(600, 24);
            menuStrip.TabIndex = 8;
            // 
            // nástrojeMenuItem
            // 
            nástrojeMenuItem.DropDownItems.AddRange(new ToolStripItem[] { checkUpdatesMenuItem });
            nástrojeMenuItem.Name = "nástrojeMenuItem";
            nástrojeMenuItem.Size = new Size(46, 20);
            nástrojeMenuItem.Text = "Tools";
            // 
            // checkUpdatesMenuItem
            // 
            checkUpdatesMenuItem.Name = "checkUpdatesMenuItem";
            checkUpdatesMenuItem.Size = new Size(186, 22);
            checkUpdatesMenuItem.Text = "🔍 Check for Updates";
            checkUpdatesMenuItem.Click += CheckUpdates_Click;
            // 
            // DashboardForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(600, 505);
            Controls.Add(lblSizesTitle);
            Controls.Add(listSizes);
            Controls.Add(lblHistoryTitle);
            Controls.Add(listHistory);
            Controls.Add(statsPanel);
            Controls.Add(button1);
            Controls.Add(btnClearHistory);
            Controls.Add(btnRefresh);
            Controls.Add(headerPanel);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            MinimumSize = new Size(600, 505);
            Name = "DashboardForm";
            Text = "Maiden - System Cleanup";
            headerPanel.ResumeLayout(false);
            statsPanel.ResumeLayout(false);
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private Panel headerPanel;
        private Label lblSubheader;
        private Label lblHeader;
        private Panel statsPanel;
        private Label lblHistoryTitle;
        private Label lblSizesTitle;
        private MenuStrip menuStrip;
        private ToolStripMenuItem languageMenuItem;
        private ToolStripMenuItem czechMenuItem;
        private ToolStripMenuItem englishMenuItem;
        private ToolStripMenuItem nástrojeMenuItem;
        private ToolStripMenuItem checkUpdatesMenuItem;
        private ToolStripMenuItem toolStripMenuItem1;
    }
}
