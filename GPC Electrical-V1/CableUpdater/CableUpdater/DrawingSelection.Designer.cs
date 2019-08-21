namespace CableUpdater
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DrawingSelection));
            this.TextFilePath = new System.Windows.Forms.Label();
            this.Select_Text = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteDrawingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FolderPath = new System.Windows.Forms.Label();
            this.Select_Folder = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.CancelBut = new System.Windows.Forms.Button();
            this.OK_But = new System.Windows.Forms.Button();
            this.btnHyperlink = new System.Windows.Forms.Button();
            this.btnAtsync = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // TextFilePath
            // 
            this.TextFilePath.AutoEllipsis = true;
            this.TextFilePath.Location = new System.Drawing.Point(428, 80);
            this.TextFilePath.Name = "TextFilePath";
            this.TextFilePath.Size = new System.Drawing.Size(222, 13);
            this.TextFilePath.TabIndex = 19;
            this.TextFilePath.Text = "No file selected";
            // 
            // Select_Text
            // 
            this.Select_Text.Location = new System.Drawing.Point(431, 30);
            this.Select_Text.Name = "Select_Text";
            this.Select_Text.Size = new System.Drawing.Size(180, 32);
            this.Select_Text.TabIndex = 18;
            this.Select_Text.Text = "Select Text File";
            this.Select_Text.UseVisualStyleBackColor = true;
            this.Select_Text.Click += new System.EventHandler(this.Select_Text_Click);
            // 
            // listBox1
            // 
            this.listBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.Location = new System.Drawing.Point(63, 109);
            this.listBox1.Name = "listBox1";
            this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBox1.Size = new System.Drawing.Size(261, 225);
            this.listBox1.TabIndex = 17;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteDrawingToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(155, 26);
            // 
            // deleteDrawingToolStripMenuItem
            // 
            this.deleteDrawingToolStripMenuItem.Name = "deleteDrawingToolStripMenuItem";
            this.deleteDrawingToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.deleteDrawingToolStripMenuItem.Text = "Delete Drawing";
            this.deleteDrawingToolStripMenuItem.Click += new System.EventHandler(this.deleteDrawingToolStripMenuItem_Click);
            // 
            // FolderPath
            // 
            this.FolderPath.AutoSize = true;
            this.FolderPath.Location = new System.Drawing.Point(60, 80);
            this.FolderPath.Name = "FolderPath";
            this.FolderPath.Size = new System.Drawing.Size(93, 13);
            this.FolderPath.TabIndex = 16;
            this.FolderPath.Text = "No folder selected";
            // 
            // Select_Folder
            // 
            this.Select_Folder.Location = new System.Drawing.Point(63, 30);
            this.Select_Folder.Name = "Select_Folder";
            this.Select_Folder.Size = new System.Drawing.Size(184, 32);
            this.Select_Folder.TabIndex = 15;
            this.Select_Folder.Text = "Select Drawing Folder";
            this.Select_Folder.UseVisualStyleBackColor = true;
            this.Select_Folder.Click += new System.EventHandler(this.Select_Folder_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(431, 104);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(368, 230);
            this.dataGridView1.TabIndex = 22;
            // 
            // CancelBut
            // 
            this.CancelBut.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBut.Location = new System.Drawing.Point(519, 366);
            this.CancelBut.Name = "CancelBut";
            this.CancelBut.Size = new System.Drawing.Size(75, 33);
            this.CancelBut.TabIndex = 24;
            this.CancelBut.Text = "Cancel";
            this.CancelBut.UseVisualStyleBackColor = true;
            // 
            // OK_But
            // 
            this.OK_But.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK_But.Location = new System.Drawing.Point(375, 366);
            this.OK_But.Name = "OK_But";
            this.OK_But.Size = new System.Drawing.Size(115, 33);
            this.OK_But.TabIndex = 23;
            this.OK_But.Text = "Update Cables";
            this.OK_But.UseVisualStyleBackColor = true;
            this.OK_But.Click += new System.EventHandler(this.OK_But_Click);
            // 
            // btnHyperlink
            // 
            this.btnHyperlink.Location = new System.Drawing.Point(236, 366);
            this.btnHyperlink.Name = "btnHyperlink";
            this.btnHyperlink.Size = new System.Drawing.Size(99, 33);
            this.btnHyperlink.TabIndex = 25;
            this.btnHyperlink.Text = "Hyperlink";
            this.btnHyperlink.UseVisualStyleBackColor = true;
            this.btnHyperlink.Click += new System.EventHandler(this.btnHyperlink_Click);
            // 
            // btnAtsync
            // 
            this.btnAtsync.Location = new System.Drawing.Point(90, 366);
            this.btnAtsync.Name = "btnAtsync";
            this.btnAtsync.Size = new System.Drawing.Size(99, 33);
            this.btnAtsync.TabIndex = 26;
            this.btnAtsync.Text = "AtSync";
            this.btnAtsync.UseVisualStyleBackColor = true;
            this.btnAtsync.Click += new System.EventHandler(this.btnAtsync_Click);
            // 
            // DrawingSelection
            // 
            this.AcceptButton = this.OK_But;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBut;
            this.ClientSize = new System.Drawing.Size(839, 437);
            this.Controls.Add(this.btnAtsync);
            this.Controls.Add(this.btnHyperlink);
            this.Controls.Add(this.CancelBut);
            this.Controls.Add(this.OK_But);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.TextFilePath);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.FolderPath);
            this.Controls.Add(this.Select_Text);
            this.Controls.Add(this.Select_Folder);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DrawingSelection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DrawingSelection";
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TextFilePath;
        private System.Windows.Forms.Button Select_Text;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label FolderPath;
        private System.Windows.Forms.Button Select_Folder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button CancelBut;
        private System.Windows.Forms.Button OK_But;
        private System.Windows.Forms.ToolStripMenuItem deleteDrawingToolStripMenuItem;
        private System.Windows.Forms.Button btnHyperlink;
        private System.Windows.Forms.Button btnAtsync;
    }
}