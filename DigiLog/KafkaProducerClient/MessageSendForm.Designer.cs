namespace KafkaProducerClient
{
    partial class MessageSendForm
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
            this.MassegeSend_Button = new System.Windows.Forms.Button();
            this.send_Messages = new System.Windows.Forms.TextBox();
            this.txtKafkaAddress = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTopicName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MassegeSend_Button
            // 
            this.MassegeSend_Button.Location = new System.Drawing.Point(356, 254);
            this.MassegeSend_Button.Margin = new System.Windows.Forms.Padding(2);
            this.MassegeSend_Button.Name = "MassegeSend_Button";
            this.MassegeSend_Button.Size = new System.Drawing.Size(138, 27);
            this.MassegeSend_Button.TabIndex = 0;
            this.MassegeSend_Button.Text = "Send Messages";
            this.MassegeSend_Button.UseVisualStyleBackColor = true;
            this.MassegeSend_Button.Click += new System.EventHandler(this.button1_Click);
            // 
            // send_Messages
            // 
            this.send_Messages.Location = new System.Drawing.Point(37, 34);
            this.send_Messages.Margin = new System.Windows.Forms.Padding(2);
            this.send_Messages.Multiline = true;
            this.send_Messages.Name = "send_Messages";
            this.send_Messages.Size = new System.Drawing.Size(457, 130);
            this.send_Messages.TabIndex = 1;
            // 
            // txtKafkaAddress
            // 
            this.txtKafkaAddress.Location = new System.Drawing.Point(118, 187);
            this.txtKafkaAddress.Name = "txtKafkaAddress";
            this.txtKafkaAddress.Size = new System.Drawing.Size(376, 20);
            this.txtKafkaAddress.TabIndex = 9;
            this.txtKafkaAddress.Text = "http://localhost:9092";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label3.Location = new System.Drawing.Point(37, 190);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Kafka Address";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Location = new System.Drawing.Point(37, 221);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Topic Name";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtTopicName
            // 
            this.txtTopicName.Location = new System.Drawing.Point(118, 218);
            this.txtTopicName.Name = "txtTopicName";
            this.txtTopicName.Size = new System.Drawing.Size(376, 20);
            this.txtTopicName.TabIndex = 6;
            this.txtTopicName.Text = "test";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(37, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Messages";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 295);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(533, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // MessageSendForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 317);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtKafkaAddress);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtTopicName);
            this.Controls.Add(this.send_Messages);
            this.Controls.Add(this.MassegeSend_Button);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MessageSendForm";
            this.Text = "Message Send Form";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button MassegeSend_Button;
        private System.Windows.Forms.TextBox send_Messages;
        private System.Windows.Forms.TextBox txtKafkaAddress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTopicName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}

