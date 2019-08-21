namespace TextReplace
{
    partial class DrawingSelection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DrawingSelection));
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TextFilePath = new System.Windows.Forms.Label();
            this.Select_Text = new System.Windows.Forms.Button();
            this.CancelBut = new System.Windows.Forms.Button();
            this.OK_But = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.Save_Button = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.Select_Folder = new System.Windows.Forms.Button();
            this.FolderPath = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox2
            // 
            this.listBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 16;
            this.listBox2.Location = new System.Drawing.Point(41, 107);
            this.listBox2.Name = "listBox2";
            this.listBox2.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBox2.Size = new System.Drawing.Size(380, 212);
            this.listBox2.TabIndex = 17;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteSelectionToolStripMenuItem,
            this.addTextToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(159, 48);
            // 
            // deleteSelectionToolStripMenuItem
            // 
            this.deleteSelectionToolStripMenuItem.Name = "deleteSelectionToolStripMenuItem";
            this.deleteSelectionToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.deleteSelectionToolStripMenuItem.Text = "Delete Selection";
            this.deleteSelectionToolStripMenuItem.Click += new System.EventHandler(this.deleteSelectionToolStripMenuItem_Click);
            // 
            // addTextToolStripMenuItem
            // 
            this.addTextToolStripMenuItem.Name = "addTextToolStripMenuItem";
            this.addTextToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.addTextToolStripMenuItem.Text = "Add Text";
            this.addTextToolStripMenuItem.Click += new System.EventHandler(this.addTextToolStripMenuItem_Click);
            // 
            // TextFilePath
            // 
            this.TextFilePath.AutoEllipsis = true;
            this.TextFilePath.Location = new System.Drawing.Point(464, 91);
            this.TextFilePath.Name = "TextFilePath";
            this.TextFilePath.Size = new System.Drawing.Size(222, 13);
            this.TextFilePath.TabIndex = 16;
            this.TextFilePath.Text = "No file selected";
            // 
            // Select_Text
            // 
            this.Select_Text.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Select_Text.Location = new System.Drawing.Point(721, 189);
            this.Select_Text.Name = "Select_Text";
            this.Select_Text.Size = new System.Drawing.Size(115, 32);
            this.Select_Text.TabIndex = 15;
            this.Select_Text.Text = "Select Text File";
            this.Select_Text.UseVisualStyleBackColor = false;
            this.Select_Text.Click += new System.EventHandler(this.Select_Text_Click);
            // 
            // CancelBut
            // 
            this.CancelBut.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBut.Location = new System.Drawing.Point(453, 350);
            this.CancelBut.Name = "CancelBut";
            this.CancelBut.Size = new System.Drawing.Size(130, 40);
            this.CancelBut.TabIndex = 19;
            this.CancelBut.Text = "Cancel";
            this.CancelBut.UseVisualStyleBackColor = true;
            this.CancelBut.Click += new System.EventHandler(this.CancelBut_Click);
            // 
            // OK_But
            // 
            this.OK_But.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK_But.Location = new System.Drawing.Point(270, 350);
            this.OK_But.Name = "OK_But";
            this.OK_But.Size = new System.Drawing.Size(142, 40);
            this.OK_But.TabIndex = 18;
            this.OK_But.Text = "Replace";
            this.OK_But.UseVisualStyleBackColor = true;
            this.OK_But.Click += new System.EventHandler(this.OK_But_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Save_Button
            // 
            this.Save_Button.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Save_Button.Location = new System.Drawing.Point(721, 258);
            this.Save_Button.Name = "Save_Button";
            this.Save_Button.Size = new System.Drawing.Size(118, 32);
            this.Save_Button.TabIndex = 20;
            this.Save_Button.Text = "Save text to file";
            this.Save_Button.UseVisualStyleBackColor = false;
            this.Save_Button.Click += new System.EventHandler(this.Save_Button_Click);
            // 
            // listBox1
            // 
            this.listBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(467, 107);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(248, 212);
            this.listBox1.TabIndex = 21;
            // 
            // Select_Folder
            // 
            this.Select_Folder.Location = new System.Drawing.Point(41, 28);
            this.Select_Folder.Name = "Select_Folder";
            this.Select_Folder.Size = new System.Drawing.Size(177, 41);
            this.Select_Folder.TabIndex = 22;
            this.Select_Folder.Text = "Select Drawing Folder";
            this.Select_Folder.UseVisualStyleBackColor = true;
            this.Select_Folder.Click += new System.EventHandler(this.Select_Folder_Click);
            // 
            // FolderPath
            // 
            this.FolderPath.AutoSize = true;
            this.FolderPath.Location = new System.Drawing.Point(38, 91);
            this.FolderPath.Name = "FolderPath";
            this.FolderPath.Size = new System.Drawing.Size(93, 13);
            this.FolderPath.TabIndex = 23;
            this.FolderPath.Text = "No folder selected";
            // 
            // DrawingSelection
            // 
            this.AcceptButton = this.OK_But;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(878, 402);
            this.Controls.Add(this.FolderPath);
            this.Controls.Add(this.Select_Folder);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.Save_Button);
            this.Controls.Add(this.CancelBut);
            this.Controls.Add(this.OK_But);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.TextFilePath);
            this.Controls.Add(this.Select_Text);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DrawingSelection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Replace Text Strings";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Label TextFilePath;
        private System.Windows.Forms.Button Select_Text;
        private System.Windows.Forms.Button CancelBut;
        private System.Windows.Forms.Button OK_But;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem deleteSelectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addTextToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button Save_Button;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button Select_Folder;
        private System.Windows.Forms.Label FolderPath;
    }
}