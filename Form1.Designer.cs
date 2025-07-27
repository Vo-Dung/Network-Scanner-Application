namespace NetworkScannerApp
{
    partial class MainForm
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

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.progressBarScan = new System.Windows.Forms.ProgressBar();
            this.listViewResults = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonStart = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonClearLog = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSaveLog = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripDropDownButtonExport = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItemTxt = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCsv = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelNetworkInfo = new System.Windows.Forms.Label();
            this.labelScanInfo = new System.Windows.Forms.Label();
            this.textBoxTcpPortRange = new System.Windows.Forms.TextBox();
            this.textBoxUdpPortRange = new System.Windows.Forms.TextBox();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.labelTcpPorts = new System.Windows.Forms.Label();
            this.labelUdpPorts = new System.Windows.Forms.Label();
            this.textBoxTimeout = new System.Windows.Forms.TextBox();
            this.labelTimeout = new System.Windows.Forms.Label();
            this.textBoxUdpTimeout = new System.Windows.Forms.TextBox();
            this.labelUdpTimeout = new System.Windows.Forms.Label();
            this.comboBoxScanMode = new System.Windows.Forms.ComboBox();
            this.labelScanMode = new System.Windows.Forms.Label();
            this.groupBoxScanSettings = new System.Windows.Forms.GroupBox();
            this.groupBoxResults = new System.Windows.Forms.GroupBox();
            this.groupBoxLog = new System.Windows.Forms.GroupBox();
            this.timerUpdate = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBoxScanSettings.SuspendLayout();
            this.groupBoxResults.SuspendLayout();
            this.groupBoxLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBarScan
            // 
            this.progressBarScan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.progressBarScan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.progressBarScan.Location = new System.Drawing.Point(12, 29);
            this.progressBarScan.Name = "progressBarScan";
            this.progressBarScan.Size = new System.Drawing.Size(958, 30);
            this.progressBarScan.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarScan.TabIndex = 0;
            // 
            // listViewResults
            // 
            this.listViewResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listViewResults.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.listViewResults.FullRowSelect = true;
            this.listViewResults.GridLines = true;
            this.listViewResults.HideSelection = false;
            this.listViewResults.Location = new System.Drawing.Point(12, 65);
            this.listViewResults.Name = "listViewResults";
            this.listViewResults.Size = new System.Drawing.Size(958, 222);
            this.listViewResults.TabIndex = 1;
            this.listViewResults.UseCompatibleStateImageBehavior = false;
            this.listViewResults.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Địa chỉ IP";
            this.columnHeader1.Width = 150;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Địa chỉ MAC";
            this.columnHeader2.Width = 150;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Tên máy chủ";
            this.columnHeader3.Width = 200;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Cổng mở";
            this.columnHeader4.Width = 500;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonStart,
            this.toolStripButtonStop,
            this.toolStripButtonClearLog,
            this.toolStripButtonSaveLog,
            this.toolStripSeparator1,
            this.toolStripDropDownButtonExport});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1000, 30);
            this.toolStrip1.TabIndex = 3;
            // 
            // toolStripButtonStart
            // 
            this.toolStripButtonStart.Name = "toolStripButtonStart";
            this.toolStripButtonStart.Size = new System.Drawing.Size(113, 27);
            this.toolStripButtonStart.Text = "Bắt đầu quét";
            this.toolStripButtonStart.ToolTipText = "Bắt đầu quá trình quét mạng";
            this.toolStripButtonStart.Click += new System.EventHandler(this.btnStartScan_Click);
            // 
            // toolStripButtonStop
            // 
            this.toolStripButtonStop.Enabled = false;
            this.toolStripButtonStop.Name = "toolStripButtonStop";
            this.toolStripButtonStop.Size = new System.Drawing.Size(96, 27);
            this.toolStripButtonStop.Text = "Dừng quét";
            this.toolStripButtonStop.ToolTipText = "Dừng quá trình quét mạng";
            this.toolStripButtonStop.Click += new System.EventHandler(this.btnStopScan_Click);
            // 
            // toolStripButtonClearLog
            // 
            this.toolStripButtonClearLog.Name = "toolStripButtonClearLog";
            this.toolStripButtonClearLog.Size = new System.Drawing.Size(104, 27);
            this.toolStripButtonClearLog.Text = "Xóa nhật ký";
            this.toolStripButtonClearLog.ToolTipText = "Xóa toàn bộ nội dung nhật ký";
            this.toolStripButtonClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // toolStripButtonSaveLog
            // 
            this.toolStripButtonSaveLog.Name = "toolStripButtonSaveLog";
            this.toolStripButtonSaveLog.Size = new System.Drawing.Size(103, 27);
            this.toolStripButtonSaveLog.Text = "Lưu nhật ký";
            this.toolStripButtonSaveLog.ToolTipText = "Lưu nhật ký quét vào tệp";
            this.toolStripButtonSaveLog.Click += new System.EventHandler(this.btnSaveLog_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 30);
            // 
            // toolStripDropDownButtonExport
            // 
            this.toolStripDropDownButtonExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemTxt,
            this.toolStripMenuItemCsv});
            this.toolStripDropDownButtonExport.Name = "toolStripDropDownButtonExport";
            this.toolStripDropDownButtonExport.Size = new System.Drawing.Size(121, 27);
            this.toolStripDropDownButtonExport.Text = "Xuất kết quả";
            this.toolStripDropDownButtonExport.ToolTipText = "Xuất kết quả quét sang định dạng .txt hoặc .csv";
            // 
            // toolStripMenuItemTxt
            // 
            this.toolStripMenuItemTxt.Name = "toolStripMenuItemTxt";
            this.toolStripMenuItemTxt.Size = new System.Drawing.Size(244, 28);
            this.toolStripMenuItemTxt.Text = "Xuất dưới dạng .txt";
            this.toolStripMenuItemTxt.Click += new System.EventHandler(this.btnExportTxt_Click);
            // 
            // toolStripMenuItemCsv
            // 
            this.toolStripMenuItemCsv.Name = "toolStripMenuItemCsv";
            this.toolStripMenuItemCsv.Size = new System.Drawing.Size(244, 28);
            this.toolStripMenuItemCsv.Text = "Xuất dưới dạng .csv";
            this.toolStripMenuItemCsv.Click += new System.EventHandler(this.btnExportCsv_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 746);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1000, 29);
            this.statusStrip1.TabIndex = 4;
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(79, 23);
            this.statusLabel.Text = "Sẵn sàng";
            // 
            // labelNetworkInfo
            // 
            this.labelNetworkInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelNetworkInfo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.labelNetworkInfo.Location = new System.Drawing.Point(12, 296);
            this.labelNetworkInfo.Name = "labelNetworkInfo";
            this.labelNetworkInfo.Size = new System.Drawing.Size(958, 80);
            this.labelNetworkInfo.TabIndex = 2;
            this.labelNetworkInfo.Text = "Thông tin mạng";
            // 
            // labelScanInfo
            // 
            this.labelScanInfo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.labelScanInfo.Location = new System.Drawing.Point(12, 385);
            this.labelScanInfo.Name = "labelScanInfo";
            this.labelScanInfo.Size = new System.Drawing.Size(958, 30);
            this.labelScanInfo.TabIndex = 3;
            this.labelScanInfo.Text = "Thông tin quét: Chưa bắt đầu";
            // 
            // textBoxTcpPortRange
            // 
            this.textBoxTcpPortRange.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textBoxTcpPortRange.Location = new System.Drawing.Point(100, 30);
            this.textBoxTcpPortRange.Name = "textBoxTcpPortRange";
            this.textBoxTcpPortRange.Size = new System.Drawing.Size(120, 30);
            this.textBoxTcpPortRange.TabIndex = 1;
            this.textBoxTcpPortRange.Text = "1-1000";
            // 
            // textBoxUdpPortRange
            // 
            this.textBoxUdpPortRange.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textBoxUdpPortRange.Location = new System.Drawing.Point(100, 70);
            this.textBoxUdpPortRange.Name = "textBoxUdpPortRange";
            this.textBoxUdpPortRange.Size = new System.Drawing.Size(120, 30);
            this.textBoxUdpPortRange.TabIndex = 3;
            this.textBoxUdpPortRange.Text = "1-1000";
            // 
            // textBoxLog
            // 
            this.textBoxLog.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textBoxLog.Location = new System.Drawing.Point(12, 25);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ReadOnly = true;
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(958, 120);
            this.textBoxLog.TabIndex = 0;
            // 
            // labelTcpPorts
            // 
            this.labelTcpPorts.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.labelTcpPorts.Location = new System.Drawing.Point(12, 30);
            this.labelTcpPorts.Name = "labelTcpPorts";
            this.labelTcpPorts.Size = new System.Drawing.Size(80, 27);
            this.labelTcpPorts.TabIndex = 0;
            this.labelTcpPorts.Text = "Cổng TCP:";
            // 
            // labelUdpPorts
            // 
            this.labelUdpPorts.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.labelUdpPorts.Location = new System.Drawing.Point(10, 70);
            this.labelUdpPorts.Name = "labelUdpPorts";
            this.labelUdpPorts.Size = new System.Drawing.Size(80, 27);
            this.labelUdpPorts.TabIndex = 2;
            this.labelUdpPorts.Text = "Cổng UDP:";
            // 
            // textBoxTimeout
            // 
            this.textBoxTimeout.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textBoxTimeout.Location = new System.Drawing.Point(501, 30);
            this.textBoxTimeout.Name = "textBoxTimeout";
            this.textBoxTimeout.Size = new System.Drawing.Size(120, 30);
            this.textBoxTimeout.TabIndex = 5;
            this.textBoxTimeout.Text = "1000";
            // 
            // labelTimeout
            // 
            this.labelTimeout.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.labelTimeout.Location = new System.Drawing.Point(300, 33);
            this.labelTimeout.Name = "labelTimeout";
            this.labelTimeout.Size = new System.Drawing.Size(195, 27);
            this.labelTimeout.TabIndex = 4;
            this.labelTimeout.Text = "Thời gian chờ TCP (ms):";
            // 
            // textBoxUdpTimeout
            // 
            this.textBoxUdpTimeout.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textBoxUdpTimeout.Location = new System.Drawing.Point(501, 70);
            this.textBoxUdpTimeout.Name = "textBoxUdpTimeout";
            this.textBoxUdpTimeout.Size = new System.Drawing.Size(120, 30);
            this.textBoxUdpTimeout.TabIndex = 7;
            this.textBoxUdpTimeout.Text = "500";
            // 
            // labelUdpTimeout
            // 
            this.labelUdpTimeout.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.labelUdpTimeout.Location = new System.Drawing.Point(300, 73);
            this.labelUdpTimeout.Name = "labelUdpTimeout";
            this.labelUdpTimeout.Size = new System.Drawing.Size(195, 27);
            this.labelUdpTimeout.TabIndex = 6;
            this.labelUdpTimeout.Text = "Thời gian chờ UDP (ms):";
            // 
            // comboBoxScanMode
            // 
            this.comboBoxScanMode.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.comboBoxScanMode.Location = new System.Drawing.Point(758, 33);
            this.comboBoxScanMode.Name = "comboBoxScanMode";
            this.comboBoxScanMode.Size = new System.Drawing.Size(140, 31);
            this.comboBoxScanMode.TabIndex = 9;
            this.comboBoxScanMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxScanMode_SelectedIndexChanged);
            // 
            // labelScanMode
            // 
            this.labelScanMode.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.labelScanMode.Location = new System.Drawing.Point(670, 33);
            this.labelScanMode.Name = "labelScanMode";
            this.labelScanMode.Size = new System.Drawing.Size(80, 27);
            this.labelScanMode.TabIndex = 8;
            this.labelScanMode.Text = "Chế độ quét:";
            // 
            // groupBoxScanSettings
            // 
            this.groupBoxScanSettings.Controls.Add(this.labelTcpPorts);
            this.groupBoxScanSettings.Controls.Add(this.textBoxTcpPortRange);
            this.groupBoxScanSettings.Controls.Add(this.labelUdpPorts);
            this.groupBoxScanSettings.Controls.Add(this.textBoxUdpPortRange);
            this.groupBoxScanSettings.Controls.Add(this.labelTimeout);
            this.groupBoxScanSettings.Controls.Add(this.textBoxTimeout);
            this.groupBoxScanSettings.Controls.Add(this.labelUdpTimeout);
            this.groupBoxScanSettings.Controls.Add(this.textBoxUdpTimeout);
            this.groupBoxScanSettings.Controls.Add(this.labelScanMode);
            this.groupBoxScanSettings.Controls.Add(this.comboBoxScanMode);
            this.groupBoxScanSettings.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxScanSettings.Location = new System.Drawing.Point(12, 40);
            this.groupBoxScanSettings.Name = "groupBoxScanSettings";
            this.groupBoxScanSettings.Size = new System.Drawing.Size(976, 110);
            this.groupBoxScanSettings.TabIndex = 2;
            this.groupBoxScanSettings.TabStop = false;
            this.groupBoxScanSettings.Text = "Cài đặt quét";
            // 
            // groupBoxResults
            // 
            this.groupBoxResults.Controls.Add(this.progressBarScan);
            this.groupBoxResults.Controls.Add(this.listViewResults);
            this.groupBoxResults.Controls.Add(this.labelNetworkInfo);
            this.groupBoxResults.Controls.Add(this.labelScanInfo);
            this.groupBoxResults.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxResults.Location = new System.Drawing.Point(12, 160);
            this.groupBoxResults.Name = "groupBoxResults";
            this.groupBoxResults.Size = new System.Drawing.Size(976, 420);
            this.groupBoxResults.TabIndex = 1;
            this.groupBoxResults.TabStop = false;
            this.groupBoxResults.Text = "Kết quả quét";
            // 
            // groupBoxLog
            // 
            this.groupBoxLog.Controls.Add(this.textBoxLog);
            this.groupBoxLog.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxLog.Location = new System.Drawing.Point(12, 583);
            this.groupBoxLog.Name = "groupBoxLog";
            this.groupBoxLog.Size = new System.Drawing.Size(976, 150);
            this.groupBoxLog.TabIndex = 0;
            this.groupBoxLog.TabStop = false;
            this.groupBoxLog.Text = "Nhật ký";
            // 
            // timerUpdate
            // 
            this.timerUpdate.Interval = 1000;
            this.timerUpdate.Tick += new System.EventHandler(this.timerUpdate_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 775);
            this.Controls.Add(this.groupBoxLog);
            this.Controls.Add(this.groupBoxResults);
            this.Controls.Add(this.groupBoxScanSettings);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Network Scanner Application";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBoxScanSettings.ResumeLayout(false);
            this.groupBoxScanSettings.PerformLayout();
            this.groupBoxResults.ResumeLayout(false);
            this.groupBoxLog.ResumeLayout(false);
            this.groupBoxLog.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.ProgressBar progressBarScan;
        private System.Windows.Forms.ListView listViewResults;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonStart;
        private System.Windows.Forms.ToolStripButton toolStripButtonStop;
        private System.Windows.Forms.ToolStripButton toolStripButtonClearLog;
        private System.Windows.Forms.ToolStripButton toolStripButtonSaveLog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonExport;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTxt;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCsv;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.Label labelNetworkInfo;
        private System.Windows.Forms.Label labelScanInfo;
        private System.Windows.Forms.TextBox textBoxTcpPortRange;
        private System.Windows.Forms.TextBox textBoxUdpPortRange;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.Label labelTcpPorts;
        private System.Windows.Forms.Label labelUdpPorts;
        private System.Windows.Forms.TextBox textBoxTimeout;
        private System.Windows.Forms.Label labelTimeout;
        private System.Windows.Forms.TextBox textBoxUdpTimeout;
        private System.Windows.Forms.Label labelUdpTimeout;
        private System.Windows.Forms.ComboBox comboBoxScanMode;
        private System.Windows.Forms.Label labelScanMode;
        private System.Windows.Forms.GroupBox groupBoxScanSettings;
        private System.Windows.Forms.GroupBox groupBoxResults;
        private System.Windows.Forms.GroupBox groupBoxLog;
        private System.Windows.Forms.Timer timerUpdate;
    }
}