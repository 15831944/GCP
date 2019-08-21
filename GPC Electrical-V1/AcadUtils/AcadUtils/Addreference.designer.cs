namespace AcadUtils
{
    partial class Addreference
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Addreference));
            this.button1_OK = new System.Windows.Forms.Button();
            this.button2_Cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1_OK
            // 
            this.button1_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1_OK.Location = new System.Drawing.Point(36, 117);
            this.button1_OK.Name = "button1_OK";
            this.button1_OK.Size = new System.Drawing.Size(75, 23);
            this.button1_OK.TabIndex = 2;
            this.button1_OK.Text = "OK";
            this.button1_OK.UseVisualStyleBackColor = true;
            this.button1_OK.Click += new System.EventHandler(this.button1_OK_Click);
            // 
            // button2_Cancel
            // 
            this.button2_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2_Cancel.Location = new System.Drawing.Point(174, 117);
            this.button2_Cancel.Name = "button2_Cancel";
            this.button2_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button2_Cancel.TabIndex = 3;
            this.button2_Cancel.Text = "Cancel";
            this.button2_Cancel.UseVisualStyleBackColor = true;
            this.button2_Cancel.Click += new System.EventHandler(this.button2_Cancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(33, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(216, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Enter the reference drawing numbers";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(14, 25);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(257, 20);
            this.textBox1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(36, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "separated by commas.";
            // 
            // Addreference
            // 
            this.AcceptButton = this.button1_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2_Cancel;
            this.ClientSize = new System.Drawing.Size(292, 161);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2_Cancel);
            this.Controls.Add(this.button1_OK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Addreference";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Drawing Reference";
            this.Load += new System.EventHandler(this.Addreference_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1_OK;
        private System.Windows.Forms.Button button2_Cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
    }
}