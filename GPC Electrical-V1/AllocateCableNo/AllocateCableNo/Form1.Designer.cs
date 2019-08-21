namespace AllocateCableNo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.OK_Button = new System.Windows.Forms.Button();
            this.CableNoTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.AllocateButton = new System.Windows.Forms.Button();
            this.CablePrefix = new System.Windows.Forms.ComboBox();
            this.ProjectDesc = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBulkRegister = new System.Windows.Forms.Button();
            this.spnCableCount = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.spnCableCount)).BeginInit();
            this.SuspendLayout();
            // 
            // OK_Button
            // 
            this.OK_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.OK_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OK_Button.Location = new System.Drawing.Point(410, 143);
            this.OK_Button.Name = "OK_Button";
            this.OK_Button.Size = new System.Drawing.Size(116, 39);
            this.OK_Button.TabIndex = 14;
            this.OK_Button.Text = "Done";
            this.OK_Button.UseVisualStyleBackColor = true;
            this.OK_Button.Click += new System.EventHandler(this.OK_Button_Click);
            // 
            // CableNoTextBox
            // 
            this.CableNoTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CableNoTextBox.ForeColor = System.Drawing.Color.Green;
            this.CableNoTextBox.Location = new System.Drawing.Point(183, 102);
            this.CableNoTextBox.Name = "CableNoTextBox";
            this.CableNoTextBox.ReadOnly = true;
            this.CableNoTextBox.Size = new System.Drawing.Size(203, 22);
            this.CableNoTextBox.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label3.Location = new System.Drawing.Point(12, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(155, 16);
            this.label3.TabIndex = 12;
            this.label3.Text = "Allocated Cable Number";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // AllocateButton
            // 
            this.AllocateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AllocateButton.Location = new System.Drawing.Point(410, 12);
            this.AllocateButton.Name = "AllocateButton";
            this.AllocateButton.Size = new System.Drawing.Size(116, 38);
            this.AllocateButton.TabIndex = 11;
            this.AllocateButton.Text = "Allocate";
            this.AllocateButton.UseVisualStyleBackColor = true;
            this.AllocateButton.Click += new System.EventHandler(this.AllocateButton_Click);
            // 
            // CablePrefix
            // 
            this.CablePrefix.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CablePrefix.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CablePrefix.FormattingEnabled = true;
            this.CablePrefix.Location = new System.Drawing.Point(183, 12);
            this.CablePrefix.Name = "CablePrefix";
            this.CablePrefix.Size = new System.Drawing.Size(203, 24);
            this.CablePrefix.TabIndex = 10;
            // 
            // ProjectDesc
            // 
            this.ProjectDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProjectDesc.Location = new System.Drawing.Point(183, 59);
            this.ProjectDesc.Name = "ProjectDesc";
            this.ProjectDesc.Size = new System.Drawing.Size(203, 22);
            this.ProjectDesc.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(46, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 16);
            this.label2.TabIndex = 8;
            this.label2.Text = "Project Description";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(126, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 16);
            this.label1.TabIndex = 15;
            this.label1.Text = "Prefix";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnBulkRegister
            // 
            this.btnBulkRegister.Location = new System.Drawing.Point(29, 147);
            this.btnBulkRegister.Name = "btnBulkRegister";
            this.btnBulkRegister.Size = new System.Drawing.Size(138, 31);
            this.btnBulkRegister.TabIndex = 16;
            this.btnBulkRegister.Text = "BulkRegister";
            this.btnBulkRegister.UseVisualStyleBackColor = true;
            this.btnBulkRegister.Click += new System.EventHandler(this.btnBulkRegister_Click);
            // 
            // spnCableCount
            // 
            this.spnCableCount.Location = new System.Drawing.Point(183, 152);
            this.spnCableCount.Maximum = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.spnCableCount.Name = "spnCableCount";
            this.spnCableCount.Size = new System.Drawing.Size(203, 20);
            this.spnCableCount.TabIndex = 17;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.OK_Button;
            this.ClientSize = new System.Drawing.Size(552, 207);
            this.Controls.Add(this.spnCableCount);
            this.Controls.Add(this.btnBulkRegister);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OK_Button);
            this.Controls.Add(this.CableNoTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.AllocateButton);
            this.Controls.Add(this.CablePrefix);
            this.Controls.Add(this.ProjectDesc);
            this.Controls.Add(this.label2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Allocate Cable Numbers";
            ((System.ComponentModel.ISupportInitialize)(this.spnCableCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OK_Button;
        private System.Windows.Forms.TextBox CableNoTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button AllocateButton;
        private System.Windows.Forms.ComboBox CablePrefix;
        private System.Windows.Forms.TextBox ProjectDesc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBulkRegister;
        private System.Windows.Forms.NumericUpDown spnCableCount;
    }
}