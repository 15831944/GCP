namespace cableFinder
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Banner_Form));
            this.SaveFileBtn = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteDrawingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadFileBtn = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.OK_But = new System.Windows.Forms.Button();
            this.CancelBut = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.Superseded_Button = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SaveFileBtn
            // 
            this.SaveFileBtn.Location = new System.Drawing.Point(289, 12);
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
            this.listBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.Location = new System.Drawing.Point(18, 55);
            this.listBox1.Name = "listBox1";
            this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox1.Size = new System.Drawing.Size(788, 147);
            this.listBox1.TabIndex = 19;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteDrawingToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(155, 26);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // deleteDrawingToolStripMenuItem
            // 
            this.deleteDrawingToolStripMenuItem.Name = "deleteDrawingToolStripMenuItem";
            this.deleteDrawingToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.deleteDrawingToolStripMenuItem.Text = "Delete Drawing";
            this.deleteDrawingToolStripMenuItem.Click += new System.EventHandler(this.deleteDrawingToolStripMenuItem_Click);
            // 
            // LoadFileBtn
            // 
            this.LoadFileBtn.Location = new System.Drawing.Point(154, 12);
            this.LoadFileBtn.Name = "LoadFileBtn";
            this.LoadFileBtn.Size = new System.Drawing.Size(114, 37);
            this.LoadFileBtn.TabIndex = 20;
            this.LoadFileBtn.Text = "Load File";
            this.LoadFileBtn.UseVisualStyleBackColor = true;
            this.LoadFileBtn.Click += new System.EventHandler(this.LoadFileBtn_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(18, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(116, 37);
            this.button2.TabIndex = 18;
            this.button2.Text = "Select Drawing Files";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // OK_But
            // 
            this.OK_But.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK_But.Location = new System.Drawing.Point(205, 363);
            this.OK_But.Name = "OK_But";
            this.OK_But.Size = new System.Drawing.Size(137, 34);
            this.OK_But.TabIndex = 26;
            this.OK_But.Text = "Run";
            this.OK_But.UseVisualStyleBackColor = true;
            this.OK_But.Click += new System.EventHandler(this.OK_But_Click_1);
            // 
            // CancelBut
            // 
            this.CancelBut.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBut.Location = new System.Drawing.Point(422, 363);
            this.CancelBut.Name = "CancelBut";
            this.CancelBut.Size = new System.Drawing.Size(133, 34);
            this.CancelBut.TabIndex = 27;
            this.CancelBut.Text = "Cancel";
            this.CancelBut.UseVisualStyleBackColor = true;
            // 
            // Superseded_Button
            // 
            this.Superseded_Button.Appearance = System.Windows.Forms.Appearance.Button;
            this.Superseded_Button.AutoSize = true;
            this.Superseded_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Superseded_Button.Location = new System.Drawing.Point(24, 30);
            this.Superseded_Button.MinimumSize = new System.Drawing.Size(100, 30);
            this.Superseded_Button.Name = "Superseded_Button";
            this.Superseded_Button.Size = new System.Drawing.Size(137, 30);
            this.Superseded_Button.TabIndex = 0;
            this.Superseded_Button.TabStop = true;
            this.Superseded_Button.Text = "Load Cable Text file";
            this.Superseded_Button.UseVisualStyleBackColor = true;
            this.Superseded_Button.CheckedChanged += new System.EventHandler(this.Superceded_Button_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.Superseded_Button);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(18, 208);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(780, 149);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "cableFinder Options";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select Dwg text file";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(169, 68);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(605, 22);
            this.textBox1.TabIndex = 1;
            this.textBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseClick);
            // 
            // Banner_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 417);
            this.Controls.Add(this.OK_But);
            this.Controls.Add(this.CancelBut);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.SaveFileBtn);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.LoadFileBtn);
            this.Controls.Add(this.button2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Banner_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create drawing reference text file";
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button SaveFileBtn;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button LoadFileBtn;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button OK_But;
        private System.Windows.Forms.Button CancelBut;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.RadioButton Superseded_Button;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem deleteDrawingToolStripMenuItem;
    }
}