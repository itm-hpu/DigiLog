namespace KafkaConsumerClient
{
    partial class GetMessage
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
            this.components = new System.ComponentModel.Container();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.Message = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTopicName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtKafkaAddress = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Message
            // 
            this.Message.Location = new System.Drawing.Point(42, 110);
            this.Message.Margin = new System.Windows.Forms.Padding(2);
            this.Message.Multiline = true;
            this.Message.Name = "Message";
            this.Message.ReadOnly = true;
            this.Message.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.Message.Size = new System.Drawing.Size(454, 185);
            this.Message.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(39, 85);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Messages";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtTopicName
            // 
            this.txtTopicName.Location = new System.Drawing.Point(120, 50);
            this.txtTopicName.Name = "txtTopicName";
            this.txtTopicName.Size = new System.Drawing.Size(376, 20);
            this.txtTopicName.TabIndex = 2;
            this.txtTopicName.Text = "Testtopic11";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Location = new System.Drawing.Point(39, 53);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Topic Name";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label3.Location = new System.Drawing.Point(39, 22);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Kafka Address";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtKafkaAddress
            // 
            this.txtKafkaAddress.Location = new System.Drawing.Point(120, 19);
            this.txtKafkaAddress.Name = "txtKafkaAddress";
            this.txtKafkaAddress.Size = new System.Drawing.Size(376, 20);
            this.txtKafkaAddress.TabIndex = 5;
            this.txtKafkaAddress.Text = "PLAINTEXT://kafkabroker.northeurope.cloudapp.azure.com:9092";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 306);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(533, 22);
            this.statusStrip1.TabIndex = 6;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // GetMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 328);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.txtKafkaAddress);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtTopicName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Message);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "GetMessage";
            this.Text = "Get Message Form";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GetMessage_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GetMessage_FormClosed);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox Message;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTopicName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtKafkaAddress;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}

