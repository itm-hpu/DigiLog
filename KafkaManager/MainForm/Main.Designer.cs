﻿namespace MainForm
{
    partial class Main
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
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabProducer = new System.Windows.Forms.TabPage();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtPMessage = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPTopic = new System.Windows.Forms.TextBox();
            this.txtPServer = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabConsumer = new System.Windows.Forms.TabPage();
            this.btnEnd = new System.Windows.Forms.Button();
            this.txtCMessage = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCTopic = new System.Windows.Forms.TextBox();
            this.txtCServer = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.chbConsumer = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.txtToolTip = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabMain.SuspendLayout();
            this.tabProducer.SuspendLayout();
            this.tabConsumer.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabProducer);
            this.tabMain.Controls.Add(this.tabConsumer);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(662, 479);
            this.tabMain.TabIndex = 0;
            // 
            // tabProducer
            // 
            this.tabProducer.Controls.Add(this.btnSend);
            this.tabProducer.Controls.Add(this.txtPMessage);
            this.tabProducer.Controls.Add(this.label3);
            this.tabProducer.Controls.Add(this.txtPTopic);
            this.tabProducer.Controls.Add(this.txtPServer);
            this.tabProducer.Controls.Add(this.label2);
            this.tabProducer.Controls.Add(this.label1);
            this.tabProducer.Location = new System.Drawing.Point(4, 22);
            this.tabProducer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabProducer.Name = "tabProducer";
            this.tabProducer.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabProducer.Size = new System.Drawing.Size(654, 453);
            this.tabProducer.TabIndex = 0;
            this.tabProducer.Text = "Producer";
            this.tabProducer.UseVisualStyleBackColor = true;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(529, 394);
            this.btnSend.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(112, 30);
            this.btnSend.TabIndex = 6;
            this.btnSend.Text = "Send message";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtPMessage
            // 
            this.txtPMessage.Location = new System.Drawing.Point(20, 124);
            this.txtPMessage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtPMessage.Multiline = true;
            this.txtPMessage.Name = "txtPMessage";
            this.txtPMessage.Size = new System.Drawing.Size(623, 266);
            this.txtPMessage.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Messages";
            // 
            // txtPTopic
            // 
            this.txtPTopic.Location = new System.Drawing.Point(98, 50);
            this.txtPTopic.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtPTopic.Name = "txtPTopic";
            this.txtPTopic.Size = new System.Drawing.Size(545, 20);
            this.txtPTopic.TabIndex = 3;
            this.txtPTopic.Text = "test";
            // 
            // txtPServer
            // 
            this.txtPServer.Location = new System.Drawing.Point(98, 15);
            this.txtPServer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtPServer.Name = "txtPServer";
            this.txtPServer.Size = new System.Drawing.Size(545, 20);
            this.txtPServer.TabIndex = 2;
            this.txtPServer.Text = "http://130.237.77.240:9092";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Topic name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Kafka server";
            // 
            // tabConsumer
            // 
            this.tabConsumer.Controls.Add(this.btnEnd);
            this.tabConsumer.Controls.Add(this.txtCMessage);
            this.tabConsumer.Controls.Add(this.label6);
            this.tabConsumer.Controls.Add(this.txtCTopic);
            this.tabConsumer.Controls.Add(this.txtCServer);
            this.tabConsumer.Controls.Add(this.label4);
            this.tabConsumer.Controls.Add(this.label5);
            this.tabConsumer.Controls.Add(this.btnExport);
            this.tabConsumer.Controls.Add(this.btnClear);
            this.tabConsumer.Controls.Add(this.btnStop);
            this.tabConsumer.Controls.Add(this.btnStart);
            this.tabConsumer.Controls.Add(this.chbConsumer);
            this.tabConsumer.Location = new System.Drawing.Point(4, 22);
            this.tabConsumer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabConsumer.Name = "tabConsumer";
            this.tabConsumer.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabConsumer.Size = new System.Drawing.Size(654, 453);
            this.tabConsumer.TabIndex = 1;
            this.tabConsumer.Text = "Consumer";
            this.tabConsumer.UseVisualStyleBackColor = true;
            // 
            // btnEnd
            // 
            this.btnEnd.Location = new System.Drawing.Point(590, 13);
            this.btnEnd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(50, 24);
            this.btnEnd.TabIndex = 17;
            this.btnEnd.Text = "End";
            this.btnEnd.UseVisualStyleBackColor = true;
            this.btnEnd.Click += new System.EventHandler(this.btnEnd_Click);
            // 
            // txtCMessage
            // 
            this.txtCMessage.Location = new System.Drawing.Point(17, 163);
            this.txtCMessage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtCMessage.Multiline = true;
            this.txtCMessage.Name = "txtCMessage";
            this.txtCMessage.ReadOnly = true;
            this.txtCMessage.Size = new System.Drawing.Size(623, 251);
            this.txtCMessage.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 137);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Messages";
            // 
            // txtCTopic
            // 
            this.txtCTopic.Location = new System.Drawing.Point(95, 92);
            this.txtCTopic.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtCTopic.Name = "txtCTopic";
            this.txtCTopic.Size = new System.Drawing.Size(545, 20);
            this.txtCTopic.TabIndex = 14;
            // 
            // txtCServer
            // 
            this.txtCServer.Location = new System.Drawing.Point(95, 57);
            this.txtCServer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtCServer.Name = "txtCServer";
            this.txtCServer.Size = new System.Drawing.Size(545, 20);
            this.txtCServer.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Topic name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Kafka server";
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(535, 13);
            this.btnExport.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(50, 24);
            this.btnExport.TabIndex = 10;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(480, 13);
            this.btnClear.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(50, 24);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(425, 13);
            this.btnStop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(50, 24);
            this.btnStop.TabIndex = 8;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(370, 13);
            this.btnStart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(50, 24);
            this.btnStart.TabIndex = 7;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // chbConsumer
            // 
            this.chbConsumer.AutoSize = true;
            this.chbConsumer.Checked = true;
            this.chbConsumer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbConsumer.Location = new System.Drawing.Point(21, 17);
            this.chbConsumer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chbConsumer.Name = "chbConsumer";
            this.chbConsumer.Size = new System.Drawing.Size(113, 17);
            this.chbConsumer.TabIndex = 0;
            this.chbConsumer.Text = "Same as Producer";
            this.chbConsumer.UseVisualStyleBackColor = true;
            this.chbConsumer.CheckedChanged += new System.EventHandler(this.chbConsumer_CheckedChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txtToolTip});
            this.statusStrip1.Location = new System.Drawing.Point(0, 457);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusStrip1.Size = new System.Drawing.Size(662, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // txtToolTip
            // 
            this.txtToolTip.Name = "txtToolTip";
            this.txtToolTip.Size = new System.Drawing.Size(48, 17);
            this.txtToolTip.Text = "DigiLog";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(662, 479);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Main";
            this.Text = "DigiLog Kafka Manager";
            this.tabMain.ResumeLayout(false);
            this.tabProducer.ResumeLayout(false);
            this.tabProducer.PerformLayout();
            this.tabConsumer.ResumeLayout(false);
            this.tabConsumer.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabProducer;
        private System.Windows.Forms.TabPage tabConsumer;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel txtToolTip;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtPMessage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPTopic;
        private System.Windows.Forms.TextBox txtPServer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chbConsumer;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtCMessage;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCTopic;
        private System.Windows.Forms.TextBox txtCServer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnEnd;
    }
}
