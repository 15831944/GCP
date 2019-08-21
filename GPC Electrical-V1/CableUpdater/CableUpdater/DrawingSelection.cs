using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Reflection;

namespace CableUpdater
{
    public partial class DrawingSelection : Form
    {
        //setup some variables to hold the data derived from the dialog
        public string DWGFolderPath { get; set; }
        public string ErrorPath { get; set; }
        public string DataPath { get; set; }
        public ListBox.ObjectCollection lbo { get; set; }
        public List<string> StringList = new List<string>();//Again this list is initialized differently to other lists

        public DrawingSelection()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Opens the standard Windows folder browser dialog to select the drawings folder
        /// Only one folder can be processed at a time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Select_Folder_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
                folderBrowserDialog1.Description = "Select the folder containing the drawings to be processed";
                System.Windows.Forms.DialogResult result = folderBrowserDialog1.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    FolderPath.Text = folderBrowserDialog1.SelectedPath;
                    //A separate method to fill the listbox
                    FillListBox(FolderPath.Text);
                }
            }
            catch (Exception)
            {

            }

        }


        /// <summary>
        /// Fills the listbox with *.dwg files in the selected folder.
        /// </summary>
        /// <param name="path"></param>
        private void FillListBox(string path)
        {
            try
            {
                //clear the contents before popupating list box
                listBox1.Items.Clear();
                foreach (string s in Directory.GetFiles(path, "*.dwg"))
                {
                    listBox1.BeginUpdate();
                    listBox1.Items.Add(s.Substring(s.LastIndexOf(@"\") + 1));
                    listBox1.EndUpdate();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error while refreshing the listbox with project drawings because: " + ex.ToString());
                System.Windows.Forms.Application.Exit();
            }
        }
        /// <summary>
        /// This method fills the DataGridView with the text values from the text data file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Select_Text_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog2 = new OpenFileDialog();
                openFileDialog2.Title = " Select the Data file";
                openFileDialog2.Filter = "TXT Files(.txt)|*.txt";
                openFileDialog2.Multiselect = false;
                System.Windows.Forms.DialogResult result = openFileDialog2.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    TextFilePath.Text = openFileDialog2.FileName;

                    //create a dataset and add a table as a data source for the datagridview
                    DataSet ds = new DataSet();
                    ds.Tables.Add("TextTable");
                    ds.Tables["TextTable"].Columns.Add("Original Cable No");
                    ds.Tables["TextTable"].Columns.Add("New Cable No");
                    ds.Tables["TextTable"].Columns.Add("Cable Description");

                    //Read all lines of the text file
                    string[] rows = System.IO.File.ReadAllLines(openFileDialog2.FileName);

                    //process each row
                    foreach (string r in rows)
                    {
                        //Split the row at the delimiter.
                        string[] items = r.Split(",".ToCharArray());

                        //Add the item to the table. You could check each string in the items array to check for nulls

                        ds.Tables["TextTable"].Rows.Add(items);
                    }
                    //Set the datagridview datasource as the dataset
                    dataGridView1.DataSource = ds;
                    //Set the table as as the particular data(you could have multiple tables in 1 dataset and display each in a different DGV.
                    dataGridView1.DataMember = "TextTable";
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Invalid data data :" + Environment.NewLine + "Error while refreshing the cable data because: " + ex.ToString());
                System.Windows.Forms.Application.Exit();
            }
        }

        /// <summary>
        /// This gathers the data from the dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OK_But_Click(object sender, EventArgs e)
        {
            try
            {

                if (listBox1.Items.Count > 0)
                {
                    lbo = listBox1.Items;//drawing numbers
                }
                List<string> stringlist = new List<string>();
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    stringlist.Add(dr.Cells[0].Value + "," + dr.Cells[1].Value + "," + dr.Cells[2].Value);//cable data
                }
                StringList = stringlist;
                DWGFolderPath = FolderPath.Text;//Drawing path
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error gathering the dialog data because: " + ex.ToString());
                System.Windows.Forms.Application.Exit();
            }
        }

        /// <summary>
        /// This deletes a drawing from the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void deleteDrawingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                while (listBox1.SelectedItem != null)
                {
                    listBox1.Items.Remove(listBox1.SelectedItem);

                }
            }
            catch (Exception)
            {

            }

        }

        private void btnHyperlink_Click(object sender, EventArgs e)
        {
            string settingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Setting.ini";
            if (!File.Exists(settingPath))
            {
                MessageBox.Show("Setting.ini data does not exist..!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                DocumentCollection acDocMgr = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager;
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    try
                    {
                        string strfilename = FolderPath.Text + "/" + listBox1.Items[i].ToString();
                        //MessageBox.Show(strfilename);
                        if (File.Exists(strfilename))
                        {
                            //acDocMgr.Open(strfilename, false);
                            Document acDoc = acDocMgr.Open(strfilename, false);
                            // MessageBox.Show("Opened Document");
                            Database db = acDoc.Database;
                            string strDWG = acDoc.Name;
                            string blockName = "CABLEID";
                            string attbName = "CABLEID";

                            var MyIni = new IniFile("Setting.ini");
                            string CableUpdaterPathForCableId = MyIni.Read("CableUpdaterPathForCableId", "CableUpdater"); //http://controllogix/cable/cadwebservice.asmx/GetCableDetails?CableID=

                            string attbValue = CableUpdaterPathForCableId; // "http://controllogix/cable/cadwebservice.asmx/GetCableDetails?CableID=";
                            UpdateAttributesInDatabase(db, blockName, attbName, attbValue);
                            acDoc.Database.SaveAs(strDWG, true, DwgVersion.Current, acDoc.Database.SecurityParameters);
                            acDoc.CloseAndSave(strDWG);
                            //MessageBox.Show("Closed and Saved Document" + strDWG);
                        }
                    }
                    catch (Exception b)
                    {
                        //MessageBox.Show("Error Processing Drawing " + listBox1.Items[i].ToString()+" "+b.Message);
                        continue;
                    }

                }
            }
            catch (Exception)
            {

            }
        }

        private void UpdateAttributesInDatabase(Database db, string blockName, string attbName, string attbValue)
        {
            try
            {
                Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                Editor ed = doc.Editor;
                // Get the IDs of the spaces we want to process
                // and simply call a function to process each
                ObjectId msId, psId;
                Transaction tr = db.TransactionManager.StartTransaction();
                using (tr)
                {

                    BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                    msId = bt[BlockTableRecord.ModelSpace];
                    psId = bt[BlockTableRecord.PaperSpace];
                    // Not needed, but quicker than aborting
                    tr.Commit();

                }

                int msCount = UpdateAttributesInBlock(msId, blockName, attbName, attbValue);
                int psCount = UpdateAttributesInBlock(psId, blockName, attbName, attbValue);
                ed.Regen();

                // Display the results

                //ed.WriteMessage("\nProcessing file: " + db.Filename);
                //ed.WriteMessage("\nUpdated {0} instance{1} of " +"attribute {2} in the modelspace.",msCount, msCount == 1 ? "" : "s",attbName);
                //ed.WriteMessage("\nUpdated {0} instance{1} of " +"attribute {2} in the default paperspace.", psCount, psCount == 1 ? "" : "s" attbName);
            }
            catch (Exception)
            {

            }


        }

        private int UpdateAttributesInBlock(ObjectId btrId, string blockName, string attbName, string attbValue)
        {
            string settingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Setting.ini";
            if (!File.Exists(settingPath))
            {
                MessageBox.Show("Setting.ini data does not exist..!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
            //string blockName = "CABLEID";
            //string attbName = "HYPERLINK";
            string LinkText = "";

            var MyIni = new IniFile("Setting.ini");

            // Will return the number of attributes modified
            int changedCount = 0;

            try
            {
                Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                Database db = doc.Database;
                Editor ed = doc.Editor;

                using (DocumentLock DocLock = doc.LockDocument())
                {

                    Transaction tr = doc.TransactionManager.StartTransaction();
                    using (tr)
                    {
                        BlockTableRecord btr = (BlockTableRecord)tr.GetObject(btrId, OpenMode.ForRead);
                        // Test each entity in the container...

                        foreach (ObjectId entId in btr)
                        {

                            Entity ent = tr.GetObject(entId, OpenMode.ForRead) as Entity;
                            //Entity entEdit = tr.GetObject(entId, OpenMode.ForWrite) as Entity;
                            if (ent != null)
                            {
                                BlockReference br = ent as BlockReference;
                                MText mt = ent as MText;
                                DBText dt = ent as DBText;
                                if (br != null)
                                {

                                    BlockTableRecord bd = (BlockTableRecord)tr.GetObject(br.BlockTableRecord, OpenMode.ForRead);
                                    // ... to see whether it's a block with
                                    // the name we're after

                                    if (bd.Name.ToUpper() == blockName)
                                    {
                                        // MessageBox.Show("Selected block");
                                        // Check each of the attributes...
                                        foreach (ObjectId arId in br.AttributeCollection)
                                        {

                                            DBObject obj = tr.GetObject(arId, OpenMode.ForRead);
                                            AttributeReference ar = obj as AttributeReference;

                                            if (ar != null)
                                            {
                                                // ... to see whether it has
                                                // the tag we're after

                                                if (ar.Tag.ToUpper() == attbName)
                                                {
                                                    // MessageBox.Show("Selected attribte");
                                                    // add the text to a variable
                                                    //ar.UpgradeOpen();
                                                    LinkText = ar.TextString;
                                                    //ar.DowngradeOpen();
                                                    //changedCount++;
                                                    //MessageBox.Show("Ready to Write " + LinkText);
                                                    ent.UpgradeOpen();
                                                    //Get the hyperlink collection from the entity
                                                    HyperLinkCollection linkCollection = ent.Hyperlinks;
                                                    for (int i = 0; i < linkCollection.Count; i++)
                                                    {
                                                        linkCollection.RemoveAt(i);
                                                    }


                                                    //Create The Hyperlink
                                                    HyperLink hyperLink = new HyperLink();
                                                    hyperLink.Description = "Cable Database";
                                                    string CableUpdaterPathForCableId = MyIni.Read("CableUpdaterPathForCableId", "CableUpdater"); //http://controllogix/cable/cadwebservice.asmx/GetCableDetails?CableID=

                                                    hyperLink.Name = CableUpdaterPathForCableId + LinkText; //"http://controllogix/cable/cadwebservice.asmx/GetCableDetails?CableID=" + LinkText;

                                                    //hyperLink.SubLocation = "http://controllogix/cable/cadwebservice.asmx/GetCableDetails?CableID=" + LinkText;
                                                    //Add the hyperlink to the collection
                                                    //MessageBox.Show("ERROR PRIO TO aTTACH hYPERLINK");
                                                    linkCollection.Add(hyperLink);
                                                    //MessageBox.Show("LinkAdded");

                                                    ent.DowngradeOpen();

                                                }

                                            }

                                        }


                                    }
                                    // Recurse for nested blocks
                                    changedCount += UpdateAttributesInBlock(br.BlockTableRecord, blockName, attbName, attbValue);

                                }

                                else if (mt != null)
                                {
                                    //MessageBox.Show("begining MText Match");
                                    TextEditor te = TextEditor.CreateTextEditor(mt);
                                    if (te != null)
                                    {
                                        te.SelectAll();
                                        TextEditorSelection sel = te.Selection;
                                        string compare = sel.SelectionString;
                                        //MessageBox.Show(compare);
                                        Match m = Regex.Match(compare, "[0-9]{3}-[0-9]{4,6}");
                                        if (m.Success)
                                        {
                                            // MessageBox.Show("Successfully Matched text " + m.Value);  
                                            string DrawingSuffix = m.Value;
                                            ent.UpgradeOpen();
                                            //Get the hyperlink collection from the entity
                                            HyperLinkCollection linkCollection = ent.Hyperlinks;
                                            for (int i = 0; i < linkCollection.Count; i++)
                                            {
                                                linkCollection.RemoveAt(i);
                                            }

                                            //Create The Hyperlink
                                            HyperLink hyperLink = new HyperLink();
                                            hyperLink.Description = "Published to Share Point Drawings";

                                            string CableUpdaterPathForTechnicalDrawing = MyIni.Read("CableUpdaterPathForTechnicalDrawing", "CableUpdater"); //"http://intranet/CargoHandling/TechnicalDrawing/"

                                            hyperLink.Name = CableUpdaterPathForTechnicalDrawing + DrawingSuffix + ".dwg"; // "http://intranet/CargoHandling/TechnicalDrawing/" + DrawingSuffix + ".dwg";

                                            //Add the hyperlink to the collection
                                            linkCollection.Add(hyperLink);
                                            //MessageBox.Show("HyperLink added");
                                            ent.DowngradeOpen();
                                        }

                                    }

                                }

                                else if (dt != null)
                                {
                                    //MessageBox.Show("begining DBText Match");
                                    //TextEditor te TextEditor.CreateTextEditor(dt) 
                                    if (dt.TextString != null)
                                    {

                                        string compare = dt.TextString;
                                        Match m = Regex.Match(compare, "[0-9]{3}-[0-9]{4,6}"); //Regex to Find Drawing Number structure
                                        if (m.Success)
                                        {
                                            //MessageBox.Show("Successfully Matched text " + m.Value);  
                                            string DrawingSuffix = m.Value;
                                            ent.UpgradeOpen();
                                            //Get the hyperlink collection from the entity
                                            HyperLinkCollection linkCollection = ent.Hyperlinks;
                                            for (int i = 0; i < linkCollection.Count; i++)
                                            {
                                                linkCollection.RemoveAt(i);
                                            }

                                            //Create The Hyperlink
                                            HyperLink hyperLink = new HyperLink();
                                            hyperLink.Description = "Published to Share Point Drawings";

                                            string CableUpdaterPathForTechnicalDrawing = MyIni.Read("CableUpdaterPathForTechnicalDrawing", "CableUpdater"); //"http://intranet/CargoHandling/TechnicalDrawing/"

                                            hyperLink.Name = CableUpdaterPathForTechnicalDrawing + DrawingSuffix + ".dwg"; // "http://intranet/CargoHandling/TechnicalDrawing/" + DrawingSuffix + ".dwg";

                                            //Add the hyperlink to the collection
                                            linkCollection.Add(hyperLink);
                                            //MessageBox.Show("HyperLink added");
                                            ent.DowngradeOpen();
                                        }

                                    }
                                }


                            }

                        }

                        tr.Commit();

                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return changedCount;

        }
        private void AtsyncCode()
        {
            try
            {
                Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                Database db = doc.Database;
                Editor ed = doc.Editor;

                using (DocumentLock DocLock = doc.LockDocument())
                {
                    doc.SendStringToExecute("ATTREQ\n0\nzoom\ne\nATTSYNC\nS\n0,0\nY\nzoom\ne\nQSAVE\nEXIT\nY", false, false, true);
                }
            }
            catch (Exception)
            {

            }


        }

        private void btnAtsync_Click(object sender, EventArgs e)
        {
            string settingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Setting.ini";
            if (!File.Exists(settingPath))
            {
                MessageBox.Show("Setting.ini data does not exist..!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                DocumentCollection acDocMgr = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager;
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    try
                    {
                        string strfilename = FolderPath.Text + "/" + listBox1.Items[i].ToString();
                        //MessageBox.Show(strfilename);
                        if (File.Exists(strfilename))
                        {
                            //acDocMgr.Open(strfilename, false);
                            Document acDoc = acDocMgr.Open(strfilename, false);
                            // MessageBox.Show("Opened Document");
                            Database db = acDoc.Database;
                            string strDWG = acDoc.Name;
                            string blockName = "CABLEID";
                            string attbName = "CABLEID";

                            var MyIni = new IniFile("Setting.ini");
                            string CableUpdaterPathForCableId = MyIni.Read("CableUpdaterPathForCableId", "CableUpdater"); //"C:/Datax/DWGList.txt"

                            string attbValue = CableUpdaterPathForCableId; // "http://controllogix/cable/cadwebservice.asmx/GetCableDetails?CableID=";
                            UpdateAttributesInDatabase(db, blockName, attbName, attbValue);
                            AtsyncCode();
                            acDoc.Database.SaveAs(strDWG, true, DwgVersion.Current, acDoc.Database.SecurityParameters);
                            //acDoc.CloseAndSave(strDWG);
                            //MessageBox.Show("Closed and Saved Document" + strDWG);
                        }
                    }
                    catch (Exception b)
                    {
                        //MessageBox.Show("Error Processing Drawing " + listBox1.Items[i].ToString()+" "+b.Message);
                        continue;
                    }
                    Close();

                }
            }
            catch (Exception)
            {

            }

        }
    }

    public class IniFile
    {
        string Path;
        string EXE = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        public IniFile(string IniPath = null)
        {
            Path = new FileInfo(IniPath ?? EXE + ".ini").FullName.ToString();
        }

        public string Read(string Key, string Section = null)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
            return RetVal.ToString();
        }

        public void Write(string Key, string Value, string Section = null)
        {
            WritePrivateProfileString(Section ?? EXE, Key, Value, Path);
        }

        public void DeleteKey(string Key, string Section = null)
        {
            Write(Key, null, Section ?? EXE);
        }

        public void DeleteSection(string Section = null)
        {
            Write(null, null, Section ?? EXE);
        }

        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section).Length > 0;
        }
    }
}

