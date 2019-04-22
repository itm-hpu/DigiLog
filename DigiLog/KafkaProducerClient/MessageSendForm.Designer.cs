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
            this.SuspendLayout();
            // 
            // MassegeSend_Button
            // 
            this.MassegeSend_Button.Location = new System.Drawing.Point(65, 365);
            this.MassegeSend_Button.Name = "MassegeSend_Button";
            this.MassegeSend_Button.Size = new System.Drawing.Size(207, 42);
            this.MassegeSend_Button.TabIndex = 0;
            this.MassegeSend_Button.Text = "Send Messages";
            this.MassegeSend_Button.UseVisualStyleBackColor = true;
            this.MassegeSend_Button.Click += new System.EventHandler(this.button1_Click);
            // 
            // send_Messages
            // 
            this.send_Messages.Location = new System.Drawing.Point(65, 46);
            this.send_Messages.Multiline = true;
            this.send_Messages.Name = "send_Messages";
            this.send_Messages.Size = new System.Drawing.Size(624, 267);
            this.send_Messages.TabIndex = 1;
            // 
            // MessageSendForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.send_Messages);
            this.Controls.Add(this.MassegeSend_Button);
            this.Name = "MessageSendForm";
            this.Text = "Message Send Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button MassegeSend_Button;
        private System.Windows.Forms.TextBox send_Messages;
    }
}

