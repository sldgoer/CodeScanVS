namespace CodeSCAN
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tb_CheckList = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btn_StopCheck = new System.Windows.Forms.Button();
            this.btn_StartCheck = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_COMS = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cb_Baud = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_PrintTicket = new System.Windows.Forms.Button();
            this.btn_ShowHide = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tb_CheckList
            // 
            this.tb_CheckList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_CheckList.Location = new System.Drawing.Point(3, 233);
            this.tb_CheckList.Multiline = true;
            this.tb_CheckList.Name = "tb_CheckList";
            this.tb_CheckList.Size = new System.Drawing.Size(268, 41);
            this.tb_CheckList.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.btn_ShowHide);
            this.splitContainer1.Panel2.Controls.Add(this.btn_PrintTicket);
            this.splitContainer1.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel2_Paint);
            this.splitContainer1.Size = new System.Drawing.Size(834, 546);
            this.splitContainer1.SplitterDistance = 278;
            this.splitContainer1.TabIndex = 17;
            // 
            // btn_StopCheck
            // 
            this.btn_StopCheck.Location = new System.Drawing.Point(9, 160);
            this.btn_StopCheck.Name = "btn_StopCheck";
            this.btn_StopCheck.Size = new System.Drawing.Size(235, 47);
            this.btn_StopCheck.TabIndex = 7;
            this.btn_StopCheck.Text = "暂停签到";
            this.btn_StopCheck.UseVisualStyleBackColor = true;
            this.btn_StopCheck.Click += new System.EventHandler(this.btn_StopCheck_Click);
            // 
            // btn_StartCheck
            // 
            this.btn_StartCheck.Location = new System.Drawing.Point(9, 97);
            this.btn_StartCheck.Name = "btn_StartCheck";
            this.btn_StartCheck.Size = new System.Drawing.Size(235, 44);
            this.btn_StartCheck.TabIndex = 6;
            this.btn_StartCheck.Text = "开始签到";
            this.btn_StartCheck.UseVisualStyleBackColor = true;
            this.btn_StartCheck.Click += new System.EventHandler(this.btn_StartCheck_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_Baud);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btn_StopCheck);
            this.groupBox1.Controls.Add(this.btn_StartCheck);
            this.groupBox1.Controls.Add(this.cb_COMS);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(268, 224);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "COM口设置";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "端口号：";
            // 
            // cb_COMS
            // 
            this.cb_COMS.FormattingEnabled = true;
            this.cb_COMS.Location = new System.Drawing.Point(65, 18);
            this.cb_COMS.Name = "cb_COMS";
            this.cb_COMS.Size = new System.Drawing.Size(179, 20);
            this.cb_COMS.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "波特率：";
            // 
            // cb_Baud
            // 
            this.cb_Baud.FormattingEnabled = true;
            this.cb_Baud.Items.AddRange(new object[] {
            "300",
            "600",
            "1200",
            "1800",
            "2400",
            "3600",
            "4800",
            "7200",
            "9600",
            "14400",
            "19200",
            "28800"});
            this.cb_Baud.Location = new System.Drawing.Point(65, 58);
            this.cb_Baud.Name = "cb_Baud";
            this.cb_Baud.Size = new System.Drawing.Size(179, 20);
            this.cb_Baud.TabIndex = 5;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tb_CheckList, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 230F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(274, 542);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btn_PrintTicket
            // 
            this.btn_PrintTicket.ForeColor = System.Drawing.Color.Lime;
            this.btn_PrintTicket.Location = new System.Drawing.Point(240, 306);
            this.btn_PrintTicket.Name = "btn_PrintTicket";
            this.btn_PrintTicket.Size = new System.Drawing.Size(75, 52);
            this.btn_PrintTicket.TabIndex = 0;
            this.btn_PrintTicket.Text = "我要签到";
            this.btn_PrintTicket.UseVisualStyleBackColor = true;
            this.btn_PrintTicket.Click += new System.EventHandler(this.btn_PrintTicket_Click);
            // 
            // btn_ShowHide
            // 
            this.btn_ShowHide.Location = new System.Drawing.Point(4, 4);
            this.btn_ShowHide.Name = "btn_ShowHide";
            this.btn_ShowHide.Size = new System.Drawing.Size(36, 23);
            this.btn_ShowHide.TabIndex = 1;
            this.btn_ShowHide.Text = "<<";
            this.btn_ShowHide.UseVisualStyleBackColor = true;
            this.btn_ShowHide.Click += new System.EventHandler(this.btn_ShowHide_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(228, 180);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "网上预约签到系统";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 546);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "业务预约签到系统";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tb_CheckList;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cb_Baud;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cb_COMS;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_StartCheck;
        private System.Windows.Forms.Button btn_StopCheck;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btn_ShowHide;
        private System.Windows.Forms.Button btn_PrintTicket;
        private System.Windows.Forms.Label label2;
    }
}

