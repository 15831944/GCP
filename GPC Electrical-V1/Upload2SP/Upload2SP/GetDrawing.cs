using System;
using System.IO;
using System.Xml;
using AcadUtils;
using System.Collections.Generic;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using RegisteredDrawingUpload;
using acadApp = Autodesk.AutoCAD.ApplicationServices.Application;

namespace Upload2SP
{
    class GetDrawing : IState
    {
        private GetDrawing()
        { }

        private static GetDrawing _instance;
        public static GetDrawing Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GetDrawing();
                }
                return _instance;
            }
        }

        #region IState Members

        public void Enter(IEntity entity)
        {
            //TODO: code to do something as we enter this state
            //Helper.InfoMessageBox("Entering the GetDrawing state");


        }

        public void Execute(IEntity entity)
        {

            //TODO: code to do actions while we are in this state
            BlockDetails bd = (BlockDetails)entity;
            try
            {
                AcadUtils.DwgUtils Utils = new DwgUtils();
                string myBorderName = string.Empty;
                string MyTitle = string.Empty;
                Document TempDoc = Application.DocumentManager.MdiActiveDocument;
                Database TempDB = TempDoc.Database;


                if (bd.lbDwgs.Count > 0)
                {
                    bd.DrawingFileName = (string)bd.lbDwgs[bd.StartNo];
                }
                else
                {
                    bd.DrawingFileName = "";
                }

                if (bd.DrawingFileName != "")
                {
                    TextWriter tw = null;
                    tw = new StreamWriter(bd.ErrorTextFile, false);
                    DocumentCollection acDocMgr = Application.DocumentManager;

                    Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument = acDocMgr.Open(bd.DrawingFileName, false);
                    Document doc = Application.DocumentManager.MdiActiveDocument;
                    Database db = doc.Database;
                    bd.SpaceID = Utils.GetMSpaceID(db);
                    myBorderName = GetBorderName(db, bd.SpaceID);

                    if (myBorderName == string.Empty)
                    {
                        bd.SpaceID = Utils.GetPSpaceID(db);
                        myBorderName = GetBorderName(db, bd.SpaceID);
                    }

                    MyTitle = Utils.ReadAttributesInBlock(db, bd.SpaceID, myBorderName, "TITLE1") + " " +
                    Utils.ReadAttributesInBlock(db, bd.SpaceID, myBorderName, "TITLE2") + " " +
                    Utils.ReadAttributesInBlock(db, bd.SpaceID, myBorderName, "TITLE3") + " " +
                    Utils.ReadAttributesInBlock(db, bd.SpaceID, myBorderName, "TITLE4") + " " +
                    Utils.ReadAttributesInBlock(db, bd.SpaceID, myBorderName, "TITLE5");
                    MyTitle = MyTitle.Trim();
                    if (MyTitle == "")
                    {
                        MyTitle = "Title information not found";
                        tw.WriteLine(bd.DrawingFileName + " " + MyTitle);
                    }


                    doc.CloseAndSave(bd.DrawingFileName);

                    int PersistReply = 0;

                    SharePointPersist MyPersist = new SharePointPersist();
                    FileInfo fi = new FileInfo(bd.DrawingFileName);
                    byte[] DwgBytes = StreamDwgFile(bd.DrawingFileName);
                    PersistReply = MyPersist.UploadDwg(fi.Name, MyTitle, DwgBytes);
                    //Helper.InfoMessageBox(PersistReply.ToString());
                    if (PersistReply < 1)
                    {
                        tw.WriteLine(bd.DrawingFileName + " Was not saved to the SharePoint File Store");
                    }
                    else
                    {
                        tw.WriteLine(bd.DrawingFileName + " Was uploaded to SharePoint File Store correctly");
                    }

                    tw.Flush();
                    tw.Close();
                }

                bd.ChangeState(IncrementDWG.Instance);
            }
            catch (System.Exception ex)
            {
                bd.DidItOk = false;
                bd.ErrorMessage = ex.ToString();
                //Helper.InfoMessageBox(bd.ErrorMessage);
                bd.CriticalError = 3;
                bd.ChangeState(ErrorState.Instance);
                //Class1.log.Error("Possible error in DB.Save.", ex);
            }
        }

        public void Exit(IEntity entity)
        {

            //TODO: code to do something as we exit this state
            //Console.WriteLine ("Exiting the GetDrawing state");
        }

        #endregion



        /// <summary>
        /// Streams the DWG file to a byte array.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        private byte[] StreamDwgFile(string filename)
        {

            Byte[] CadFileData = null;
            FileStream fileStream = null;


            {
                fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                int length = (int)fileStream.Length;  // get file length
                CadFileData = new byte[length];            // create buffer
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(CadFileData, sum, length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading

                fileStream.Flush();
                fileStream.Close();

                return CadFileData;
            }
        }


        /// <summary>
        /// Updates the attributes in block.
        /// </summary>
        /// <param name="db">The db.</param>
        /// <param name="btrId">The BTR id.</param>
        /// <param name="blockName">Name of the block.</param>
        /// <param name="attbName">Name of the attb.</param>
        /// <param name="attbValue">The attb value.</param>
        /// <returns></returns>
        public string GetBorderName(Database db, ObjectId btrId)
        {
            string blockName = string.Empty;
            try
            {
                Transaction tr = db.TransactionManager.StartTransaction();
                using (tr)
                {
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(btrId, OpenMode.ForRead);

                    // Test each entity in the container...
                    foreach (ObjectId entId in btr)
                    {
                        Entity ent = tr.GetObject(entId, OpenMode.ForRead) as Entity;

                        if (ent != null)
                        {
                            BlockReference br = ent as BlockReference;

                            if (br != null)
                            {

                                BlockTableRecord bd = (BlockTableRecord)tr.GetObject(br.BlockTableRecord, OpenMode.ForRead);

                                // ... to see whether it's a block with the name we're after

                                // Check each of the attributes...

                                foreach (ObjectId arId in br.AttributeCollection)
                                {
                                    DBObject obj = tr.GetObject(arId, OpenMode.ForRead);

                                    AttributeReference ar = obj as AttributeReference;

                                    if (ar != null)
                                    {
                                        // ... to see whether it has the tag we're after

                                        if (ar.Tag.ToUpper() == "TITLE1")
                                        {
                                            blockName = br.Name;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    tr.Commit();

                }
            }
            catch (System.Exception)
            {

            }

            return blockName;
        }
    }
}
