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
using System.Reflection;

namespace Banner
{
    public class BorderDetails
    {
        public string BannerName { get; set; }
        public Scale3d BannerScale { get; set; }
        public double BannerRotation { get; set; }
        public Point3d BannerInsertionPoint { get; set; }
        public Boolean DoneOk { get; set; }
        public string BannerFilename { get; set; }
        public string ErrorMessage { get; set; }
    }


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
            string settingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Setting.ini";
            if (!File.Exists(settingPath))
            {
                System.Windows.Forms.MessageBox.Show("Setting.ini data does not exist..!!", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            //TODO: code to do actions while we are in this state
            BlockDetails bd = entity as BlockDetails;
            try
            {
                BorderDetails bdet = new BorderDetails();


                AcadUtils.DwgUtils Utils = new DwgUtils();
                string myBorderName = string.Empty;
                string MyTitle = string.Empty;
                Document TempDoc = Application.DocumentManager.MdiActiveDocument;
                Database TempDB = TempDoc.Database;
                TextWriter tw = null;
                tw = new StreamWriter(bd.ErrorTextFile, false);
                int UpdatedAttributes = 0;

                bd.DrawingFileName = (string)bd.lbDwgs[bd.StartNo];
                DocumentCollection acDocMgr = Application.DocumentManager;

                Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument = acDocMgr.Open(bd.DrawingFileName, false);
                Document doc = Application.DocumentManager.MdiActiveDocument;
                Database db = doc.Database;
                bd.SpaceID = Utils.GetMSpaceID(db);

                myBorderName = GetBorderName(db, bd.SpaceID);
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

                DocumentLock dl = doc.LockDocument(DocumentLockMode.ProtectedAutoWrite, null, null, true);

                using (dl)
                {

                    using (db)
                    {
                        var MyIni = new IniFile("Setting.ini");
                        string BannerFileFullPath = MyIni.Read("BannerFileFullPath", "Banner"); //@"c:\Datax\Banner.dwg";
                        bdet.BannerFilename = BannerFileFullPath;
                        string BannerFilename = System.IO.Path.GetFileName(bdet.BannerFilename);
                        bdet.BannerName = BannerFilename.Replace(System.IO.Path.GetExtension(BannerFilename), "");
                        bdet = InsertBanner(bdet);

                        // Helper.InfoMessageBox(bdet.BannerName + ":" + bd.BannerLine1);

                        UpdatedAttributes += Utils.UpdateAttributesInBlock(db, bd.SpaceID, bdet.BannerName.ToUpper(), "LINE1", bd.BannerLine1);
                        UpdatedAttributes += Utils.UpdateAttributesInBlock(db, bd.SpaceID, bdet.BannerName.ToUpper(), "LINE2", bd.BannerLine2);
                        UpdatedAttributes += Utils.UpdateAttributesInBlock(db, bd.SpaceID, bdet.BannerName.ToUpper(), "LINE3", bd.BannerLine3);
                    }
                }
                if (UpdatedAttributes < 3)
                {
                    tw.WriteLine("Drawing " + bd.DrawingFileName + " failed to update all banner attributes");
                }

                doc.CloseAndSave(bd.DrawingFileName);

                if (bd.UploadDwg)
                {

                    int PersistReply = 0;

                    SharePointPersist MyPersist = new SharePointPersist();
                    FileInfo fi = new FileInfo(bd.DrawingFileName);
                    byte[] DwgBytes = StreamDwgFile(bd.DrawingFileName);
                    PersistReply = MyPersist.UploadDwg(fi.Name, MyTitle, DwgBytes);

                    if (PersistReply < 1)
                    {
                        tw.WriteLine(bd.DrawingFileName + " Was not saved to the SharePoint File Store");
                    }

                    //Helper.InfoMessageBox(PersistReply.ToString());

                }
                tw.Flush();
                tw.Close();
                bd.ChangeState(IncrementDWG.Instance);
            }
            catch (System.Exception ex)
            {
                bd.DidItOk = false;
                bd.ErrorMessage = ex.ToString();
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



        public BorderDetails InsertBanner(BorderDetails BkDetails)
        {

            BkDetails.DoneOk = false;
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            Transaction Trans = db.TransactionManager.StartTransaction();
            AcadUtils.DwgUtils Utils = new AcadUtils.DwgUtils();
            Point3d p1 = db.Extmin;
            Point3d p2 = db.Extmax;
            double XDistance = p2.X - p1.X;
            BkDetails.BannerInsertionPoint = new Point3d(p2.X / 2, p2.Y / 2, 0);
            BkDetails.BannerScale = new Scale3d(XDistance / 397, XDistance / 397, 1);
            BkDetails.BannerRotation = 0.34906585;
            ObjectId msId = Utils.GetMSpaceID(db);
            try
            {

                ObjectId BlockID = ObjectId.Null;
                Database Newdb = new Database(false, true);
                using (Newdb)
                {

                    if (File.Exists(BkDetails.BannerFilename))
                    {


                        //Read the .dwg file into a new temporary drawing database
                        Newdb.ReadDwgFile(BkDetails.BannerFilename, System.IO.FileShare.Read, false, "");

                    }


                    BlockTable bt = (BlockTable)(Trans.GetObject(db.BlockTableId, OpenMode.ForRead));
                    // if (!bt.Has("Banner"))
                    // {


                    //Insert the temporary database record into the active document database
                    //with the name held by the 'BkDetails.NewBorder' string variable. This
                    //creates a block definition.
                    BlockID = db.Insert(BkDetails.BannerName, Newdb, false);
                    //}
                    //else
                    //{

                    //}

                    //Open the ModelSpace BlockTableRecord (BTR), create a new block reference from the 
                    //block definition and append it to the BTR. Effectively creating a visible object instance
                    //of the block definition.

                    BlockTableRecord btr = (BlockTableRecord)Trans.GetObject(msId, OpenMode.ForWrite);
                    ObjectIdCollection OIC = btr.GetBlockReferenceIds(true, false);

                    BlockReference br = new BlockReference(BkDetails.BannerInsertionPoint, BlockID);
                    btr.AppendEntity(br);


                    //Complete the persistence data, the rotation and scale.
                    br.Rotation = BkDetails.BannerRotation;
                    br.ScaleFactors = BkDetails.BannerScale;
                    Trans.AddNewlyCreatedDBObject(br, true);
                    BlockTableRecord BlkTblRec = Trans.GetObject(BlockID, OpenMode.ForWrite) as BlockTableRecord;

                    //Set the block so that it cannot be exploded by normal AutoCad commands
                    //Then, if the block definition has attributes, copy them to the block reference.

                    BlkTblRec.Explodable = false;
                    if (BlkTblRec.HasAttributeDefinitions)
                    {
                        foreach (ObjectId objId in BlkTblRec)
                        {
                            AttributeDefinition AttDef = Trans.GetObject(objId, OpenMode.ForRead) as AttributeDefinition;
                            if (AttDef != null && !AttDef.Constant)
                            {
                                AttributeReference AttRef = new AttributeReference();
                                AttRef.SetAttributeFromBlock(AttDef, br.BlockTransform);
                                br.AttributeCollection.AppendAttribute(AttRef);
                                Trans.AddNewlyCreatedDBObject(AttRef, true);
                            }
                        }
                    }
                    //Finally commit the changes to the drawing database.
                    Trans.Commit();


                }
                Trans.Dispose();
                ed.Regen();
                BkDetails.DoneOk = true;
                // }
                // else
                // {
                BkDetails.ErrorMessage = "Block " + BkDetails.BannerName + " already exists";

            }
            catch (System.Exception ex)
            {
                BkDetails.ErrorMessage = ex.ToString();

            }
            return BkDetails;
        }

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

            Transaction tr = db.TransactionManager.StartTransaction();
            string blockName = string.Empty;

            try
            {
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
