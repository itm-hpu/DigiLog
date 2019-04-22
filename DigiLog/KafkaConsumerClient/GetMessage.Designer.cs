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
            this.SuspendLayout();
            // 
            // Message
            // 
            this.Message.Location = new System.Drawing.Point(63, 80);
            this.Message.Multiline = true;
            this.Message.Name = "Message";
            this.Message.ReadOnly = true;
            this.Message.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.Message.Size = new System.Drawing.Size(679, 371);
            this.Message.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(59, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Messages";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // GetMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 496);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Message);
            this.Name = "GetMessage";
            this.Text = "Get Message Form";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GetMessage_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GetMessage_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox Message;
        private System.Windows.Forms.Label label1;
    }
}

