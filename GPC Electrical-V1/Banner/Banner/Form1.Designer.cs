namespace Banner
{
    partial class Banner_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Banner_Form));
            this.SaveFileBtn = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.LoadFileBtn = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Drawn_Date = new System.Windows.Forms.DateTimePicker();
            this.Drawn_TextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Reserved_Text = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Line3_Text = new System.Windows.Forms.TextBox();
            this.Line2_Text = new System.Windows.Forms.TextBox();
            this.Line1_Text = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Reserved_Button = new System.Windows.Forms.RadioButton();
            this.Redundant_Button = new System.Windows.Forms.RadioButton();
            this.Superseded_Button = new System.Windows.Forms.RadioButton();
            this.OK_But = new System.Windows.Forms.Button();
            this.CancelBut = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.UploadButton1 = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // SaveFileBtn
            // 
            this.SaveFileBtn.Location = new System.Drawing.Point(295, 12);
            this.SaveFileBtn.Name = "SaveFileBtn";
            this.SaveFileBtn.Size = new System.Drawing.Size(117, 37);
            this.SaveFileBtn.TabIndex = 21;
            this.SaveFileBtn.Text = "Save To Text File";
            this.SaveFileBtn.UseVisualStyleBackColor = true;
            this.SaveFileBtn.Click += new System.EventHandler(this.SaveFileBtn_Click);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.Location = new System.Drawing.Point(18, 55);
            this.listBox1.Name = "listBox1";
            this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox1.Size = new System.Drawing.Size(788, 147);
            this.listBox1.TabIndex = 19;
            // 
            // LoadFileBtn
            // 
            this.LoadFileBtn.Location = new System.Drawing.Point(160, 12);
            this.LoadFileBtn.Name = "LoadFileBtn";
            this.LoadFileBtn.Size = new System.Drawing.Size(114, 37);
            this.LoadFileBtn.TabIndex = 20;
            this.LoadFileBtn.Text = "Load File";
            this.LoadFileBtn.UseVisualStyleBackColor = true;
            this.LoadFileBtn.Click += new System.EventHandler(this.LoadFileBtn_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(24, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(116, 37);
            this.button2.TabIndex = 18;
            this.button2.Text = "Select Drawing Files";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 461);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 16);
            this.label1.TabIndex = 24;
            this.label1.Text = "Drawn By:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Drawn_Date
            // 
            this.Drawn_Date.CustomFormat = "dd/MM/yy";
            this.Drawn_Date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.Drawn_Date.Location = new System.Drawing.Point(298, 461);
            this.Drawn_Date.Name = "Drawn_Date";
            this.Drawn_Date.Size = new System.Drawing.Size(104, 20);
            this.Drawn_Date.TabIndex = 23;
            // 
            // Drawn_TextBox
            // 
            this.Drawn_TextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.Drawn_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Drawn_TextBox.Location = new System.Drawing.Point(86, 460);
            this.Drawn_TextBox.Name = "Drawn_TextBox";
            this.Drawn_TextBox.Size = new System.Drawing.Size(188, 22);
            this.Drawn_TextBox.TabIndex = 22;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Reserved_Text);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.Line3_Text);
            this.groupBox1.Controls.Add(this.Line2_Text);
            this.groupBox1.Controls.Add(this.Line1_Text);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.Reserved_Button);
            this.groupBox1.Controls.Add(this.Redundant_Button);
            this.groupBox1.Controls.Add(this.Superseded_Button);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(18, 208);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(780, 233);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Banner Options";
            // 
            // Reserved_Text
            // 
            this.Reserved_Text.Enabled = false;
            this.Reserved_Text.Location = new System.Drawing.Point(153, 110);
            this.Reserved_Text.Name = "Reserved_Text";
            this.Reserved_Text.Size = new System.Drawing.Size(241, 22);
            this.Reserved_Text.TabIndex = 33;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(76, 206);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 16);
            this.label4.TabIndex = 32;
            this.label4.Text = "Line 3";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(76, 177);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 16);
            this.label3.TabIndex = 31;
            this.label3.Text = "Line 2";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(76, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 16);
            this.label2.TabIndex = 30;
            this.label2.Text = "Line 1";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Line3_Text
            // 
            this.Line3_Text.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.Line3_Text.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Line3_Text.Location = new System.Drawing.Point(153, 203);
            this.Line3_Text.Name = "Line3_Text";
            this.Line3_Text.Size = new System.Drawing.Size(241, 22);
            this.Line3_Text.TabIndex = 29;
            // 
            // Line2_Text
            // 
            this.Line2_Text.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.Line2_Text.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Line2_Text.Location = new System.Drawing.Point(153, 177);
            this.Line2_Text.Name = "Line2_Text";
            this.Line2_Text.Size = new System.Drawing.Size(241, 22);
            this.Line2_Text.TabIndex = 28;
            // 
            // Line1_Text
            // 
            this.Line1_Text.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.Line1_Text.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Line1_Text.Location = new System.Drawing.Point(153, 151);
            this.Line1_Text.Name = "Line1_Text";
            this.Line1_Text.Size = new System.Drawing.Size(241, 22);
            this.Line1_Text.TabIndex = 27;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(419, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(345, 122);
            this.pictureBox1.TabIndex = 26;
            this.pictureBox1.TabStop = false;
            // 
            // Reserved_Button
            // 
            this.Reserved_Button.Appearance = System.Windows.Forms.Appearance.Button;
            this.Reserved_Button.AutoSize = true;
            this.Reserved_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Reserved_Button.Location = new System.Drawing.Point(24, 102);
            this.Reserved_Button.MinimumSize = new System.Drawing.Size(100, 30);
            this.Reserved_Button.Name = "Reserved_Button";
            this.Reserved_Button.Size = new System.Drawing.Size(100, 30);
            this.Reserved_Button.TabIndex = 2;
            this.Reserved_Button.TabStop = true;
            this.Reserved_Button.Text = "Reserved";
            this.Reserved_Button.UseVisualStyleBackColor = true;
            this.Reserved_Button.CheckedChanged += new System.EventHandler(this.Reserved_Button_CheckedChanged);
            // 
            // Redundant_Button
            // 
            this.Redundant_Button.Appearance = System.Windows.Forms.Appearance.Button;
            this.Redundant_Button.AutoSize = true;
            this.Redundant_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Redundant_Button.Location = new System.Drawing.Point(24, 66);
            this.Redundant_Button.MinimumSize = new System.Drawing.Size(100, 30);
            this.Redundant_Button.Name = "Redundant_Button";
            this.Redundant_Button.Size = new System.Drawing.Size(100, 30);
            this.Redundant_Button.TabIndex = 1;
            this.Redundant_Button.TabStop = true;
            this.Redundant_Button.Text = "Redundant";
            this.Redundant_Button.UseVisualStyleBackColor = true;
            this.Redundant_Button.CheckedChanged += new System.EventHandler(this.Redundant_Button_CheckedChanged);
            // 
            // Superseded_Button
            // 
            this.Superseded_Button.Appearance = System.Windows.Forms.Appearance.Button;
            this.Superseded_Button.AutoSize = true;
            this.Superseded_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Superseded_Button.Location = new System.Drawing.Point(24, 30);
            this.Superseded_Button.MinimumSize = new System.Drawing.Size(100, 30);
            this.Superseded_Button.Name = "Superseded_Button";
            this.Superseded_Button.Size = new System.Drawing.Size(100, 30);
            this.Superseded_Button.TabIndex = 0;
            this.Superseded_Button.TabStop = true;
            this.Superseded_Button.Text = "Superseded";
            this.Superseded_Button.UseVisualStyleBackColor = true;
            this.Superseded_Button.CheckedChanged += new System.EventHandler(this.Superceded_Button_CheckedChanged);
            // 
            // OK_But
            // 
            this.OK_But.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK_But.Location = new System.Drawing.Point(192, 539);
            this.OK_But.Name = "OK_But";
            this.OK_But.Size = new System.Drawing.Size(137, 34);
            this.OK_But.TabIndex = 26;
            this.OK_But.Text = "Run";
            this.OK_But.UseVisualStyleBackColor = true;
            this.OK_But.Click += new System.EventHandler(this.OK_But_Click);
            // 
            // CancelBut
            // 
            this.CancelBut.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBut.Location = new System.Drawing.Point(408, 539);
            this.CancelBut.Name = "CancelBut";
            this.CancelBut.Size = new System.Drawing.Size(133, 34);
            this.CancelBut.TabIndex = 27;
            this.CancelBut.Text = "Cancel";
            this.CancelBut.UseVisualStyleBackColor = true;
            // 
            // UploadButton1
            // 
            this.UploadButton1.AutoSize = true;
            this.UploadButton1.Location = new System.Drawing.Point(489, 461);
            this.UploadButton1.Name = "UploadButton1";
            this.UploadButton1.Size = new System.Drawing.Size(173, 17);
            this.UploadButton1.TabIndex = 28;
            this.UploadButton1.TabStop = true;
            this.UploadButton1.Text = "Upload to SharePoint File Store";
            this.UploadButton1.UseVisualStyleBackColor = true;
            // 
            // Banner_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 594);
            this.Controls.Add(this.UploadButton1);
            this.Controls.Add(this.OK_But);
            this.Controls.Add(this.CancelBut);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Drawn_Date);
            this.Controls.Add(this.Drawn_TextBox);
            this.Controls.Add(this.SaveFileBtn);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.LoadFileBtn);
            this.Controls.Add(this.button2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Banner_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Insert A Banner";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SaveFileBtn;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button LoadFileBtn;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker Drawn_Date;
        private System.Windows.Forms.TextBox Drawn_TextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Line3_Text;
        private System.Windows.Forms.TextBox Line2_Text;
        private System.Windows.Forms.TextBox Line1_Text;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RadioButton Reserved_Button;
        private System.Windows.Forms.RadioButton Redundant_Button;
        private System.Windows.Forms.RadioButton Superseded_Button;
        private System.Windows.Forms.Button OK_But;
        private System.Windows.Forms.Button CancelBut;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TextBox Reserved_Text;
        private System.Windows.Forms.RadioButton UploadButton1;
    }
}