namespace RegisterDwg
{
    partial class DwgInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DwgInfo));
            this.DwgBorder = new System.Windows.Forms.GroupBox();
            this.ckbArtistName = new System.Windows.Forms.CheckBox();
            this.BorderComboBox = new System.Windows.Forms.ComboBox();
            this.ScaleComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DwgTitle = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.RegoNo = new System.Windows.Forms.TextBox();
            this.DwgNo = new System.Windows.Forms.TextBox();
            this.Title2ListBox = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.RegoDwg = new System.Windows.Forms.TextBox();
            this.RegoUnit = new System.Windows.Forms.TextBox();
            this.Title5ListBox = new System.Windows.Forms.ComboBox();
            this.Title1ListBox = new System.Windows.Forms.ComboBox();
            this.DwgCancelButton = new System.Windows.Forms.Button();
            this.RegisterButton = new System.Windows.Forms.Button();
            this.RegoFacility = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.DwgPrefix = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.Title4TextBox = new System.Windows.Forms.TextBox();
            this.Title3TextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBulkReg = new System.Windows.Forms.Button();
            this.btnOpenCSV = new System.Windows.Forms.Button();
            this.txtBulkPath = new System.Windows.Forms.TextBox();
            this.DwgBorder.SuspendLayout();
            this.DwgTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // DwgBorder
            // 
            this.DwgBorder.Controls.Add(this.ckbArtistName);
            this.DwgBorder.Controls.Add(this.BorderComboBox);
            this.DwgBorder.Controls.Add(this.ScaleComboBox);
            this.DwgBorder.Controls.Add(this.label1);
            this.DwgBorder.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DwgBorder.Location = new System.Drawing.Point(14, 16);
            this.DwgBorder.Name = "DwgBorder";
            this.DwgBorder.Size = new System.Drawing.Size(508, 75);
            this.DwgBorder.TabIndex = 0;
            this.DwgBorder.TabStop = false;
            this.DwgBorder.Text = "Border";
            // 
            // ckbArtistName
            // 
            this.ckbArtistName.AutoSize = true;
            this.ckbArtistName.Location = new System.Drawing.Point(333, 26);
            this.ckbArtistName.Name = "ckbArtistName";
            this.ckbArtistName.Size = new System.Drawing.Size(175, 24);
            this.ckbArtistName.TabIndex = 7;
            this.ckbArtistName.Text = "Add Drawn By Name";
            this.ckbArtistName.UseVisualStyleBackColor = true;
            // 
            // BorderComboBox
            // 
            this.BorderComboBox.FormattingEnabled = true;
            this.BorderComboBox.Items.AddRange(new object[] {
            "A3",
            "A2",
            "A1"});
            this.BorderComboBox.Location = new System.Drawing.Point(7, 22);
            this.BorderComboBox.Name = "BorderComboBox";
            this.BorderComboBox.Size = new System.Drawing.Size(136, 28);
            this.BorderComboBox.TabIndex = 6;
            this.BorderComboBox.SelectedIndexChanged += new System.EventHandler(this.BorderComboBox_SelectedIndexChanged);
            // 
            // ScaleComboBox
            // 
            this.ScaleComboBox.FormattingEnabled = true;
            this.ScaleComboBox.Items.AddRange(new object[] {
            "NTS",
            "1",
            "2.5",
            "5",
            "10",
            "25",
            "50",
            "100"});
            this.ScaleComboBox.Location = new System.Drawing.Point(217, 22);
            this.ScaleComboBox.Name = "ScaleComboBox";
            this.ScaleComboBox.Size = new System.Drawing.Size(110, 28);
            this.ScaleComboBox.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(159, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Scale";
            // 
            // DwgTitle
            // 
            this.DwgTitle.Controls.Add(this.label11);
            this.DwgTitle.Controls.Add(this.RegoNo);
            this.DwgTitle.Controls.Add(this.DwgNo);
            this.DwgTitle.Controls.Add(this.Title2ListBox);
            this.DwgTitle.Controls.Add(this.label10);
            this.DwgTitle.Controls.Add(this.label9);
            this.DwgTitle.Controls.Add(this.RegoDwg);
            this.DwgTitle.Controls.Add(this.RegoUnit);
            this.DwgTitle.Controls.Add(this.Title5ListBox);
            this.DwgTitle.Controls.Add(this.Title1ListBox);
            this.DwgTitle.Controls.Add(this.DwgCancelButton);
            this.DwgTitle.Controls.Add(this.RegisterButton);
            this.DwgTitle.Controls.Add(this.RegoFacility);
            this.DwgTitle.Controls.Add(this.label8);
            this.DwgTitle.Controls.Add(this.DwgPrefix);
            this.DwgTitle.Controls.Add(this.label7);
            this.DwgTitle.Controls.Add(this.Title4TextBox);
            this.DwgTitle.Controls.Add(this.Title3TextBox);
            this.DwgTitle.Controls.Add(this.label6);
            this.DwgTitle.Controls.Add(this.label5);
            this.DwgTitle.Controls.Add(this.label4);
            this.DwgTitle.Controls.Add(this.label3);
            this.DwgTitle.Controls.Add(this.label2);
            this.DwgTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DwgTitle.Location = new System.Drawing.Point(14, 97);
            this.DwgTitle.Name = "DwgTitle";
            this.DwgTitle.Size = new System.Drawing.Size(508, 332);
            this.DwgTitle.TabIndex = 1;
            this.DwgTitle.TabStop = false;
            this.DwgTitle.Text = "Drawing Title";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(305, 241);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(18, 20);
            this.label11.TabIndex = 25;
            this.label11.Text = "+";
            // 
            // RegoNo
            // 
            this.RegoNo.Enabled = false;
            this.RegoNo.Location = new System.Drawing.Point(330, 238);
            this.RegoNo.Name = "RegoNo";
            this.RegoNo.Size = new System.Drawing.Size(46, 26);
            this.RegoNo.TabIndex = 24;
            // 
            // DwgNo
            // 
            this.DwgNo.Enabled = false;
            this.DwgNo.Location = new System.Drawing.Point(215, 197);
            this.DwgNo.Name = "DwgNo";
            this.DwgNo.Size = new System.Drawing.Size(83, 26);
            this.DwgNo.TabIndex = 23;
            // 
            // Title2ListBox
            // 
            this.Title2ListBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Title2ListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Title2ListBox.FormattingEnabled = true;
            this.Title2ListBox.Location = new System.Drawing.Point(81, 57);
            this.Title2ListBox.Name = "Title2ListBox";
            this.Title2ListBox.Size = new System.Drawing.Size(422, 26);
            this.Title2ListBox.TabIndex = 22;
            this.Title2ListBox.SelectedValueChanged += new System.EventHandler(this.Title2ListBox_SelectedValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(254, 241);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(18, 20);
            this.label10.TabIndex = 21;
            this.label10.Text = "+";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(172, 241);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(18, 20);
            this.label9.TabIndex = 20;
            this.label9.Text = "+";
            // 
            // RegoDwg
            // 
            this.RegoDwg.Enabled = false;
            this.RegoDwg.Location = new System.Drawing.Point(275, 238);
            this.RegoDwg.Name = "RegoDwg";
            this.RegoDwg.Size = new System.Drawing.Size(24, 26);
            this.RegoDwg.TabIndex = 19;
            // 
            // RegoUnit
            // 
            this.RegoUnit.Enabled = false;
            this.RegoUnit.Location = new System.Drawing.Point(196, 238);
            this.RegoUnit.Name = "RegoUnit";
            this.RegoUnit.Size = new System.Drawing.Size(52, 26);
            this.RegoUnit.TabIndex = 18;
            // 
            // Title5ListBox
            // 
            this.Title5ListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Title5ListBox.FormattingEnabled = true;
            this.Title5ListBox.Location = new System.Drawing.Point(81, 156);
            this.Title5ListBox.Name = "Title5ListBox";
            this.Title5ListBox.Size = new System.Drawing.Size(421, 26);
            this.Title5ListBox.TabIndex = 17;
            this.Title5ListBox.SelectedValueChanged += new System.EventHandler(this.Title5ListBox_SelectedValueChanged);
            // 
            // Title1ListBox
            // 
            this.Title1ListBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Title1ListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Title1ListBox.FormattingEnabled = true;
            this.Title1ListBox.Location = new System.Drawing.Point(82, 26);
            this.Title1ListBox.Name = "Title1ListBox";
            this.Title1ListBox.Size = new System.Drawing.Size(421, 26);
            this.Title1ListBox.TabIndex = 16;
            this.Title1ListBox.SelectedValueChanged += new System.EventHandler(this.Title1ListBox_SelectedValueChanged);
            // 
            // DwgCancelButton
            // 
            this.DwgCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DwgCancelButton.Location = new System.Drawing.Point(333, 282);
            this.DwgCancelButton.Name = "DwgCancelButton";
            this.DwgCancelButton.Size = new System.Drawing.Size(136, 32);
            this.DwgCancelButton.TabIndex = 15;
            this.DwgCancelButton.Text = "Cancel";
            this.DwgCancelButton.UseVisualStyleBackColor = true;
            // 
            // RegisterButton
            // 
            this.RegisterButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.RegisterButton.Location = new System.Drawing.Point(18, 282);
            this.RegisterButton.Name = "RegisterButton";
            this.RegisterButton.Size = new System.Drawing.Size(142, 33);
            this.RegisterButton.TabIndex = 14;
            this.RegisterButton.Text = "Register";
            this.RegisterButton.UseVisualStyleBackColor = true;
            this.RegisterButton.Click += new System.EventHandler(this.RegisterButton_Click);
            // 
            // RegoFacility
            // 
            this.RegoFacility.Enabled = false;
            this.RegoFacility.Location = new System.Drawing.Point(142, 238);
            this.RegoFacility.Name = "RegoFacility";
            this.RegoFacility.Size = new System.Drawing.Size(24, 26);
            this.RegoFacility.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 238);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(123, 20);
            this.label8.TabIndex = 12;
            this.label8.Text = "Registration No.";
            // 
            // DwgPrefix
            // 
            this.DwgPrefix.Enabled = false;
            this.DwgPrefix.Location = new System.Drawing.Point(142, 197);
            this.DwgPrefix.Name = "DwgPrefix";
            this.DwgPrefix.Size = new System.Drawing.Size(67, 26);
            this.DwgPrefix.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 197);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 20);
            this.label7.TabIndex = 12;
            this.label7.Text = "Drawing No.";
            // 
            // Title4TextBox
            // 
            this.Title4TextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.Title4TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Title4TextBox.Location = new System.Drawing.Point(81, 120);
            this.Title4TextBox.Name = "Title4TextBox";
            this.Title4TextBox.Size = new System.Drawing.Size(422, 24);
            this.Title4TextBox.TabIndex = 11;
            // 
            // Title3TextBox
            // 
            this.Title3TextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.Title3TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Title3TextBox.Location = new System.Drawing.Point(82, 87);
            this.Title3TextBox.Name = "Title3TextBox";
            this.Title3TextBox.Size = new System.Drawing.Size(422, 24);
            this.Title3TextBox.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 155);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 20);
            this.label6.TabIndex = 9;
            this.label6.Text = "Title 5";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 20);
            this.label5.TabIndex = 7;
            this.label5.Text = "Title 4";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 20);
            this.label4.TabIndex = 5;
            this.label4.Text = "Title 3";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 20);
            this.label3.TabIndex = 3;
            this.label3.Text = "Title 2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Title 1";
            // 
            // btnBulkReg
            // 
            this.btnBulkReg.Location = new System.Drawing.Point(347, 447);
            this.btnBulkReg.Name = "btnBulkReg";
            this.btnBulkReg.Size = new System.Drawing.Size(75, 30);
            this.btnBulkReg.TabIndex = 7;
            this.btnBulkReg.Text = "Bulk Reg";
            this.btnBulkReg.UseVisualStyleBackColor = true;
            this.btnBulkReg.Click += new System.EventHandler(this.btnBulkReg_Click_1);
            // 
            // btnOpenCSV
            // 
            this.btnOpenCSV.Location = new System.Drawing.Point(244, 447);
            this.btnOpenCSV.Name = "btnOpenCSV";
            this.btnOpenCSV.Size = new System.Drawing.Size(75, 30);
            this.btnOpenCSV.TabIndex = 6;
            this.btnOpenCSV.Text = "OpenCSV";
            this.btnOpenCSV.UseVisualStyleBackColor = true;
            this.btnOpenCSV.Click += new System.EventHandler(this.btnOpenCSV_Click_1);
            // 
            // txtBulkPath
            // 
            this.txtBulkPath.Location = new System.Drawing.Point(34, 452);
            this.txtBulkPath.Name = "txtBulkPath";
            this.txtBulkPath.Size = new System.Drawing.Size(188, 20);
            this.txtBulkPath.TabIndex = 5;
            // 
            // DwgInfo
            // 
            this.AcceptButton = this.RegisterButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 485);
            this.Controls.Add(this.btnBulkReg);
            this.Controls.Add(this.btnOpenCSV);
            this.Controls.Add(this.txtBulkPath);
            this.Controls.Add(this.DwgTitle);
            this.Controls.Add(this.DwgBorder);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DwgInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DwgInfo";
            this.DwgBorder.ResumeLayout(false);
            this.DwgBorder.PerformLayout();
            this.DwgTitle.ResumeLayout(false);
            this.DwgTitle.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox DwgBorder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox DwgTitle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Title4TextBox;
        private System.Windows.Forms.TextBox Title3TextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button DwgCancelButton;
        private System.Windows.Forms.Button RegisterButton;
        private System.Windows.Forms.TextBox RegoFacility;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox DwgPrefix;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox Title1ListBox;
        private System.Windows.Forms.ComboBox Title5ListBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox RegoDwg;
        private System.Windows.Forms.TextBox RegoUnit;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox Title2ListBox;
        private System.Windows.Forms.ComboBox BorderComboBox;
        private System.Windows.Forms.ComboBox ScaleComboBox;
        private System.Windows.Forms.TextBox DwgNo;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox RegoNo;
        private System.Windows.Forms.Button btnBulkReg;
        private System.Windows.Forms.Button btnOpenCSV;
        private System.Windows.Forms.TextBox txtBulkPath;
        private System.Windows.Forms.CheckBox ckbArtistName;
    }
}