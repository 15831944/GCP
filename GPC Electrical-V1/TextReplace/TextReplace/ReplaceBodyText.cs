using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using System.Windows.Forms;

namespace TextReplace
{
    class ReplaceBodyText : IState
    {
        public int ReplacedCount;
        private string DrawingNumber = string.Empty;
        private ReplaceBodyText()
        { }

        private static ReplaceBodyText _instance;
        public static ReplaceBodyText Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ReplaceBodyText();
                }
                return _instance;
            }
        }

        #region IState Members

        public void Enter(IEntity entity)
        {
            //TODO: code to do something as we enter this state
            //Helper.InfoMessageBox("Entering the ReplaceBodyText state");
        }

        public void Execute(IEntity entity)
        {

            //TODO: code to do actions while we are in this state
            //System.Windows.Forms.Cursor myCursor = new System.Windows.Forms.Cursor(GetType(), "Resources.wait_rl.cur");
            //System.Diagnostics.Debugger.Launch();
            BlockDetails bd = (BlockDetails)entity;
            ReplacedCount = 0;

            if (bd.lbDwgs.Count > 0)
            {
                bd.DrawingFileName = (string)bd.lbDwgs[bd.DwgCounter];
                bd.CriticalError = 0;
                bd.extractionState = true;

                if (bd.DwgCounter < bd.lbDwgs.Count)
                {

                    try
                    {
                        DocumentCollection acDocMgr = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager;
                        //Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument = acDocMgr.MdiActiveDocument;
                        Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument = acDocMgr.Open(System.IO.Path.Combine(bd.FullPath, bd.DrawingFileName), false);
                        DrawingNumber = bd.DrawingFileName;
                        SelectionSet StringSet = null;
                        //Boolean myText = false;
                        //Boolean myMText = false;
                        Editor acDocEd = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
                        Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                        Database db = doc.Database;
                        string dwgno = Path.GetFileName(doc.Name);
                        Transaction acTrans = db.TransactionManager.StartTransaction();
                        using (acTrans)
                        {

                            DocumentLock dl = acDocMgr.MdiActiveDocument.LockDocument();
                            using (dl)
                            {
                                // Create a TypedValue array to define the filter criteria
                                TypedValue[] acTypValAr = new TypedValue[1];
                                TypedValue[] acTypValAr2 = new TypedValue[1];

                                acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "TEXT"), 0);
                                acTypValAr2.SetValue(new TypedValue((int)DxfCode.Start, "MTEXT"), 0);

                                // Assign the filter criteria to a SelectionFilter object
                                SelectionFilter acSelFtr = new SelectionFilter(acTypValAr);
                                SelectionFilter acSelFtr2 = new SelectionFilter(acTypValAr2);

                                // Request for objects to be selected in the drawing area
                                PromptSelectionResult acSSPrompt;
                                PromptSelectionResult acSSPrompt2;
                                acSSPrompt = acDocEd.SelectAll(acSelFtr);
                                acSSPrompt2 = acDocEd.SelectAll(acSelFtr2);



                                //Process MText

                                if (acSSPrompt2.Status == PromptStatus.OK)
                                {
                                    StringSet = acSSPrompt2.Value;
                                    if (StringSet.Count > 0)
                                    {
                                        // Step through the objects in the selection set
                                        foreach (SelectedObject acSSObj in StringSet)
                                        {
                                            // Check to make sure a valid SelectedObject object was returned
                                            if (acSSObj != null)
                                            {
                                                Autodesk.AutoCAD.DatabaseServices.MText acEnt = acTrans.GetObject(acSSObj.ObjectId, OpenMode.ForRead) as MText;

                                                try
                                                {
                                                    if (acEnt != null && acEnt.Text != "")
                                                    {
                                                        StringBuilder b = new StringBuilder(acEnt.Contents);
                                                        foreach (string rstring in bd.lbtext)
                                                        {
                                                            try
                                                            {
                                                                if (!string.IsNullOrEmpty(rstring))
                                                                {
                                                                    string[] words = rstring.Split(':');
                                                                    if (b.ToString().Contains(words[0]))
                                                                    {
                                                                        b.Replace(words[0], words[1]);
                                                                        acEnt.UpgradeOpen();
                                                                        acEnt.Contents = b.ToString();
                                                                        bd.MTextList.Add(acEnt);
                                                                        ReplacedCount = ReplacedCount + 1;
                                                                        acEnt.DowngradeOpen();
                                                                    }
                                                                }
                                                            }
                                                            catch (Exception)
                                                            {

                                                            }                                                           

                                                        }
                                                    }
                                                }
                                                catch (System.Exception ex)
                                                {
                                                    Helper.InfoMessageBox(ex.ToString());
                                                }
                                            }
                                        }
                                    }
                                }

                                //Helper.InfoMessageBox("Processing Text");

                                if (acSSPrompt.Status == PromptStatus.OK)
                                {
                                    StringSet = acSSPrompt.Value;
                                    if (StringSet.Count > 0)
                                    {
                                        // Step through the objects in the selection set
                                        foreach (SelectedObject acSSObj in StringSet)
                                        {
                                            // Check to make sure a valid SelectedObject object was returned
                                            if (acSSObj != null)
                                            {
                                                Autodesk.AutoCAD.DatabaseServices.DBText acEnt = acTrans.GetObject(acSSObj.ObjectId, OpenMode.ForRead) as DBText;
                                                try
                                                {
                                                    if (acEnt != null && acEnt.TextString != "")
                                                    {

                                                        StringBuilder b = new StringBuilder(acEnt.TextString);
                                                        foreach (string rstring in bd.lbtext)
                                                        {
                                                            try
                                                            {
                                                                if (!string.IsNullOrEmpty(rstring))
                                                                {
                                                                    string[] words = rstring.Split(':');
                                                                    if (b.ToString().Contains(words[0]))
                                                                    {
                                                                        b.Replace(words[0], words[1]);
                                                                        acEnt.UpgradeOpen();
                                                                        acEnt.TextString = b.ToString();
                                                                        bd.DBTextList.Add(acEnt);
                                                                        ReplacedCount = ReplacedCount + 1;
                                                                        acEnt.DowngradeOpen();
                                                                    }
                                                                }
                                                            }
                                                            catch (Exception)
                                                            {

                                                            }
                                                            
                                                        }

                                                    }

                                                }
                                                catch (System.Exception ex)
                                                {
                                                    Helper.InfoMessageBox(ex.ToString());
                                                }
                                            }
                                        }
                                    }
                                }
                                acTrans.Commit();
                            }

                        }
                        Transaction tr = doc.TransactionManager.StartTransaction();
                        ObjectId msId;

                        using (tr)
                        {
                            DocumentLock dl = acDocMgr.MdiActiveDocument.LockDocument();
                            using (dl)
                            {

                                //PROCESSING Model Space
                                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                                msId = bt[BlockTableRecord.ModelSpace];
                                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(msId, OpenMode.ForRead);
                                foreach (ObjectId entId in btr)
                                {
                                    Entity ent = tr.GetObject(entId, OpenMode.ForRead) as Entity;
                                    if (ent != null)
                                    {
                                        BlockReference br = ent as BlockReference;
                                        if (br != null)
                                        {
                                            BlockTableRecord btr2 = (BlockTableRecord)tr.GetObject(br.BlockTableRecord, OpenMode.ForRead);
                                            foreach (ObjectId arId in br.AttributeCollection)
                                            {
                                                DBObject obj = tr.GetObject(arId, OpenMode.ForRead);
                                                AttributeReference ar = obj as AttributeReference;
                                                if (ar != null && ar.TextString != "")
                                                {
                                                    StringBuilder b = new StringBuilder(ar.TextString);
                                                    foreach (string rstring in bd.lbtext)
                                                    {
                                                        try
                                                        {
                                                            if (!string.IsNullOrEmpty(rstring))
                                                            {
                                                                string[] words = rstring.Split(':');
                                                                b.Replace(words[0], words[1]);
                                                            }
                                                        }
                                                        catch (Exception)
                                                        {

                                                        }
                                                        
                                                    }
                                                    ar.UpgradeOpen();
                                                    ar.TextString = b.ToString();
                                                    ar.DowngradeOpen();
                                                }
                                            }
                                        }
                                    }
                                }
                                //PROCESSING Paper Space
                                ObjectId psId = bt[BlockTableRecord.PaperSpace];
                                BlockTableRecord btrp = (BlockTableRecord)tr.GetObject(psId, OpenMode.ForRead);
                                foreach (ObjectId entId in btrp)
                                {
                                    Entity ent2 = tr.GetObject(entId, OpenMode.ForRead) as Entity;
                                    if (ent2 != null)
                                    {
                                        BlockReference brp = ent2 as BlockReference;
                                        if (brp != null)
                                        {
                                            BlockTableRecord btr2 = (BlockTableRecord)tr.GetObject(brp.BlockTableRecord, OpenMode.ForRead);
                                            foreach (ObjectId arId in brp.AttributeCollection)
                                            {
                                                DBObject obj = tr.GetObject(arId, OpenMode.ForRead);
                                                AttributeReference ar = obj as AttributeReference;
                                                if (ar != null && ar.TextString != "")
                                                {
                                                    StringBuilder b = new StringBuilder(ar.TextString);
                                                    foreach (string rstring in bd.lbtext)
                                                    {
                                                        try
                                                        {
                                                            if (!string.IsNullOrEmpty(rstring))
                                                            {
                                                                string[] words = rstring.Split(',');
                                                                b.Replace(words[0], words[1]);
                                                            }
                                                        }
                                                        catch (Exception)
                                                        {

                                                        }
                                                       
                                                    }
                                                    ar.UpgradeOpen();
                                                    ar.TextString = b.ToString();
                                                    ar.DowngradeOpen();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            tr.Commit();
                        }
                        doc.CloseAndSave(System.IO.Path.Combine(bd.FullPath, bd.DrawingFileName));
                        bd.ChangeState(IncrementDWG.Instance);
                    }

                    catch (System.Exception ex)
                    {
                        bd.DidItOk = false;
                        bd.ErrorMessage = "Error replacing text " + ex.ToString();
                        Helper.InfoMessageBox("error:" + ex.ToString());
                        bd.CriticalError = 2;
                        bd.ChangeState(ErrorState.Instance);

                    }
                }
                else
                {
                    bd.KeepRunning = false;
                    bd.ChangeState(Idle.Instance);
                }
            }

            else
            {
                bd.KeepRunning = false;
                bd.ChangeState(Idle.Instance);
            }

            //MessageBox.Show("Total text entities changed is " + ReplacedCount.ToString());
        }

        public void Exit(IEntity entity)
        {
            //TODO: code to do something as we exit this state
            //Console.WriteLine ("Exiting the ExtractBodyText state");

        }

        #endregion

    }
}
