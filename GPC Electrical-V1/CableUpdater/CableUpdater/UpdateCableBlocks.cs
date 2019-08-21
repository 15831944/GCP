using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;


namespace CableUpdater
{
    class UpdateCableBlocks : IState
    {
        //private string DrawingNumber = string.Empty;
        private UpdateCableBlocks()
        { }
        private static UpdateCableBlocks _instance;
        public static UpdateCableBlocks Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UpdateCableBlocks();
                }
                return _instance;
            }
        }
        #region IState Members

        public void Enter(IEntity entity)
        {
            //TODO: code to do something as we enter this state

        }

        public void Execute(IEntity entity)
        {
            //TODO: code to do actions while we are in this state
            BlockDetails bd = (BlockDetails)entity;

            if (bd.DwgCounter < bd.lbDwgs.Count)
            {
                bd.DrawingFileName = (string)bd.lbDwgs[bd.DwgCounter];
                bd.CriticalError = 0;

                try
                {
                    DocumentCollection acDocMgr = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager;

                    //Check that the path parameters are not null and if the file exists before opening it
                    if (bd.FullPath != null && bd.DrawingFileName != null)
                    {
                        if (File.Exists(System.IO.Path.Combine(bd.FullPath, bd.DrawingFileName)))
                        {
                            Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument = acDocMgr.Open(System.IO.Path.Combine(bd.FullPath, bd.DrawingFileName), false);
                        }
                        else
                        {
                            throw new System.Exception("File Does Not Exist");
                        }
                    }

                    Editor acDocEd = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
                    Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                    Database db = doc.Database;
                    ObjectId msId;

                    Transaction tr = db.TransactionManager.StartTransaction();
                    using (tr)
                    {

                        DocumentLock dl = acDocMgr.MdiActiveDocument.LockDocument();
                        using (dl)
                        {
                            //PROCESSING Model Space.  Copy the section below to process Paper Space
                            //*************************************************
                            string[] words = new string[3];
                            BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);

                            //Change to  'msId = bt[BlockTableRecord.PaperSpace];' for paper space
                            msId = bt[BlockTableRecord.ModelSpace];

                            //Check the ModelSpace table objects for block references
                            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(msId, OpenMode.ForRead);
                            foreach (ObjectId entId in btr)
                            {
                                Entity ent = tr.GetObject(entId, OpenMode.ForRead) as Entity;
                                if (ent != null)
                                {
                                    BlockReference br = ent as BlockReference;
                                    //If the cable block name is 
                                    if (br != null && br.Name.ToUpper() == "CABLEID")
                                    {

                                        //Check the attributes to find the 'CABLEID' tag
                                        foreach (ObjectId arId in br.AttributeCollection)
                                        {
                                            DBObject obj = tr.GetObject(arId, OpenMode.ForRead);
                                            AttributeReference ar = obj as AttributeReference;

                                            if (ar != null && ar.Tag.ToUpper() == "CABLEID")
                                            {
                                                foreach (string rstring in bd.CableList)
                                                {
                                                    try
                                                    {
                                                        if (!string.IsNullOrEmpty(rstring))
                                                        {//split the line into individual text strings
                                                            words = rstring.Split(',');

                                                            //If the existing tag matches the 'From' cable id the change the attributes to the new values
                                                            if (ar.TextString == words[0])
                                                            {
                                                                int BlockCount = UpdateAttributesInBlock(br, "CABLEID", words[1]);
                                                                BlockCount += UpdateAttributesInBlock(br, "CABLE_SIZE", words[2]);
                                                                if (BlockCount != 2)
                                                                {
                                                                    System.Windows.Forms.MessageBox.Show("There may have been a problem updating cable " + words[0] + " on drawing " + bd.DrawingFileName);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch (Exception)
                                                    {

                                                    }


                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //*************************************************
                            //Add Paper Space section here.
                        }

                        tr.Commit();
                        doc.CloseAndSave(System.IO.Path.Combine(bd.FullPath, bd.DrawingFileName));
                        bd.ChangeState(IncrementDWG.Instance);

                    }
                }
                catch (Exception ex)
                {
                    //if there is an error pass on the drawing filename to the error state
                    bd.DidItOk = false;
                    bd.ErrorMessage = "Error during the cable block processing operation in " + bd.DrawingFileName + ". " + ex.ToString();
                    bd.CriticalError = 1;
                    bd.ChangeState(ErrorState.Instance);
                }
            }

        }

        public void Exit(IEntity entity)
        {

            //TODO: code to do something as we exit this state
            //Console.WriteLine ("Exiting the extractBodyBlockText state");

        }

        #endregion

        /// <summary>
        /// This is used to update the cable block attributes
        /// </summary>
        /// <param name="br"></param> The block reference is the reference to the instance of the block in model/paper space
        /// <param name="attbName"></param>The block attribute to change
        /// <param name="attbValue"></param>The new attribute value
        /// <returns></returns>
        public int UpdateAttributesInBlock(BlockReference br, string attbName, string attbValue)
        {
            int changedCount = 0;
            try
            {
                Document doc = Application.DocumentManager.MdiActiveDocument;
                Transaction tr = doc.TransactionManager.StartTransaction();

                using (tr)
                {
                    //Process each attribute looking for the correct attribute tag
                    foreach (ObjectId arId in br.AttributeCollection)
                    {
                        DBObject obj = tr.GetObject(arId, OpenMode.ForRead);

                        AttributeReference ar = obj as AttributeReference;

                        if (ar != null)
                        {
                            // ... to see whether it has the tag we're after
                            //.Net is case sensitave so set both to 'uppercase' for the comparison
                            if (ar.Tag.ToUpper() == attbName.ToUpper())
                            {
                                // If so, update the value and increment the counter

                                ar.UpgradeOpen();

                                ar.TextString = attbValue;

                                ar.DowngradeOpen();

                                changedCount++;
                            }
                        }
                    }

                    tr.Commit();

                }

                return changedCount;
            }
            catch (Exception ex)
            {
                //if there is an error return 'changedcount' at -1
                changedCount = -1;
                return changedCount;
            }

        }
    }
}
