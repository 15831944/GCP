// (C) Copyright 2014 by Microsoft 
//
using System;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using System.Windows;
using System.IO;
using System.Text.RegularExpressions;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System.Runtime.InteropServices;
using System.Text;
using System.Reflection;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(HyperlinkAddingOnSave.MyCommands))]

namespace HyperlinkAddingOnSave
{

    // This class is instantiated by AutoCAD for each document when
    // a command is called by the user the first time in the context
    // of a given document. In other words, non static data in this class
    // is implicitly per-document!
    public class MyCommands
    {
        // The CommandMethod attribute can be applied to any public  member 
        // function of any public class.
        // The function should take no arguments and return nothing.
        // If the method is an intance member then the enclosing class is 
        // intantiated for each document. If the member is a static member then
        // the enclosing class is NOT intantiated.
        //
        // NOTE: CommandMethod has overloads where you can provide helpid and
        // context menu.

        // Modal Command with localized name
        [CommandMethod("AddDocEvent")]
        public void AddDocEvent() // This method can have any name
        {
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            acDoc.BeginDocumentClose += new DocumentBeginCloseEventHandler(DocBeginDocClose);
            Database db = acDoc.Database;
            db.BeginSave += new DatabaseIOEventHandler(DocBeginSave);
        }

        // Modal Command with pickfirst selection
        [CommandMethod("RemoveDocEvent")]
        public void RemoveDocEvent() // This method can have any name
        {
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            acDoc.BeginDocumentClose -= new DocumentBeginCloseEventHandler(DocBeginDocClose);
            Database db = acDoc.Database;
            db.BeginSave += new DatabaseIOEventHandler(DocBeginSave);
        }

        //// Application Session Command with localized name
        [CommandMethod("DocBeginDocClose")]
        public void DocBeginDocClose(object senderObj, DocumentBeginCloseEventArgs docBegClsEvtArgs) // This method can have any name
        {
            try
            {

            }
            catch (System.Exception)
            {

            }
            //MessageBox.Show("ClosingDocument");
        }

        [CommandMethod("DocBeginSave")]
        public void DocBeginSave(object senderObj, DatabaseIOEventArgs docBegSavEvtArgs) // This method can have any name
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            try
            {
                // Have the user choose the block and attribute
                // names, and the new attribute value

                string blockName = "CABLEID";
                string attbName = "CABLEID";

                var MyIni = new IniFile("Setting.ini");

                string HyperlinkPathForCableId = MyIni.Read("HyperlinkPathForCableId", "HyperlinkAddingOnSave"); //http://controllogix/cable/cadwebservice.asmx/GetCableDetails?CableID=

                string attbValue = HyperlinkPathForCableId;         //"http://controllogix/cable/cadwebservice.asmx/GetCableDetails?CableID=";

                UpdateAttributesInDatabase(db, blockName, attbName, attbValue);
            }
            catch (System.Exception)
            {

            }

        }

        private void UpdateAttributesInDatabase(Database db, string blockName, string attbName, string attbValue)
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            try
            {
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
            catch (System.Exception)
            {

            }

        }

        private int UpdateAttributesInBlock(ObjectId btrId, string blockName, string attbName, string attbValue)
        {
            string settingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Setting.ini";
            if (!File.Exists(settingPath))
            {
                MessageBox.Show("Setting.ini data does not exist..!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

                                                    string HyperlinkPathForCableId = MyIni.Read("HyperlinkPathForCableId", "HyperlinkAddingOnSave"); //http://controllogix/cable/cadwebservice.asmx/GetCableDetails?CableID=

                                                    hyperLink.Name = HyperlinkPathForCableId + LinkText;

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
                                        string compare = sel.ToString();
                                        Match m = Regex.Match(compare, "[0-9]{3}-[0-9]{4,6}");
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

                                            string HyperlinkPathForTechnicalDrawing = MyIni.Read("HyperlinkPathForTechnicalDrawing", "HyperlinkAddingOnSave"); //"http://intranet/CargoHandling/TechnicalDrawing/"

                                            hyperLink.Name = HyperlinkPathForTechnicalDrawing + DrawingSuffix + ".dwg";

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

                                            string HyperlinkPathForTechnicalDrawing = MyIni.Read("HyperlinkPathForTechnicalDrawing", "HyperlinkAddingOnSave"); //"http://intranet/CargoHandling/TechnicalDrawing/"

                                            hyperLink.Name = HyperlinkPathForTechnicalDrawing + DrawingSuffix + ".dwg";
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
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

            return changedCount;

        }

        [CommandMethod("DocBatchUpdate")]
        public void DocBatchUpdate()
        {
            string settingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Setting.ini";
            if (!File.Exists(settingPath))
            {
                MessageBox.Show("Setting.ini data does not exist..!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var MyIni = new IniFile("Setting.ini");
            string HyperlinkAddingDocBatchUpdateFilePath = MyIni.Read("HyperlinkAddingDocBatchUpdateFilePath", "HyperlinkAddingOnSave"); //"C:/Datax/DWGList.txt"
            try
            {
                if (System.IO.File.Exists(HyperlinkAddingDocBatchUpdateFilePath))
                {
                    DocumentCollection acDocMgr = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager;
                    System.IO.StreamReader txtRead = new System.IO.StreamReader(HyperlinkAddingDocBatchUpdateFilePath);
                    string strfilename = "Test";
                    while (strfilename != null)
                    {
                        strfilename = txtRead.ReadLine();
                        try
                        {
                            if (File.Exists(strfilename))
                            {
                                //acDocMgr.Open(strfilename, false);
                                Document acDoc = acDocMgr.Open(strfilename, false);
                                string strDWG = acDoc.Name;
                                acDoc.Database.SaveAs(strDWG, true, DwgVersion.Current, acDoc.Database.SecurityParameters);
                                acDoc.CloseAndDiscard();

                            }
                        }
                        catch (System.Exception)
                        {

                        }
                    }
                }
                else
                {
                    MessageBox.Show(HyperlinkAddingDocBatchUpdateFilePath + " does not exist");
                }
            }
            catch (System.Exception)
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
