namespace Upload2SP
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
            this.ListBox1CMS = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteDrawingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadFileBtn = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.OK_But = new System.Windows.Forms.Button();
            this.CancelBut = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.ListBox1CMS.SuspendLayout();
            this.SuspendLayout();
            // 
            // SaveFileBtn
            // 
            this.SaveFileBtn.Location = new System.Drawing.Point(331, 12);
            this.SaveFileBtn.Name = "SaveFileBtn";
            this.SaveFileBtn.Size = new System.Drawing.Size(139, 40);
            this.SaveFileBtn.TabIndex = 21;
            this.SaveFileBtn.Text = "Save To Text File";
            this.SaveFileBtn.UseVisualStyleBackColor = true;
            this.SaveFileBtn.Click += new System.EventHandler(this.SaveFileBtn_Click);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.ContextMenuStrip = this.ListBox1CMS;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.Location = new System.Drawing.Point(18, 68);
            this.listBox1.Name = "listBox1";
            this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox1.Size = new System.Drawing.Size(788, 303);
            this.listBox1.TabIndex = 19;
            // 
            // ListBox1CMS
            // 
            this.ListBox1CMS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteDrawingToolStripMenuItem});
            this.ListBox1CMS.Name = "ListBox1CMS";
            this.ListBox1CMS.Size = new System.Drawing.Size(155, 26);
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
            this.LoadFileBtn.Location = new System.Drawing.Point(179, 12);
            this.LoadFileBtn.Name = "LoadFileBtn";
            this.LoadFileBtn.Size = new System.Drawing.Size(139, 40);
            this.LoadFileBtn.TabIndex = 20;
            this.LoadFileBtn.Text = "Load File";
            this.LoadFileBtn.UseVisualStyleBackColor = true;
            this.LoadFileBtn.Click += new System.EventHandler(this.LoadFileBtn_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(24, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(139, 40);
            this.button2.TabIndex = 18;
            this.button2.Text = "Select Drawing Files";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // OK_But
            // 
            this.OK_But.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK_But.Location = new System.Drawing.Point(169, 384);
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
            this.CancelBut.Location = new System.Drawing.Point(429, 384);
            this.CancelBut.Name = "CancelBut";
            this.CancelBut.Size = new System.Drawing.Size(133, 34);
            this.CancelBut.TabIndex = 27;
            this.CancelBut.Text = "Cancel";
            this.CancelBut.UseVisualStyleBackColor = true;
            // 
            // Banner_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 430);
            this.Controls.Add(this.OK_But);
            this.Controls.Add(this.CancelBut);
            this.Controls.Add(this.SaveFileBtn);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.LoadFileBtn);
            this.Controls.Add(this.button2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Banner_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Upload Drawings to The SharePoint File Store";
            this.ListBox1CMS.ResumeLayout(false);
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
        private System.Windows.Forms.ContextMenuStrip ListBox1CMS;
        private System.Windows.Forms.ToolStripMenuItem deleteDrawingToolStripMenuItem;
    }
}