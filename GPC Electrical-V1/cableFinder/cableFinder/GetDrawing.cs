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
using acadApp = Autodesk.AutoCAD.ApplicationServices.Application;

namespace cableFinder
{
    public class BorderDetails
    {
        public Database DwgDb { get; set; }
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

            //TODO: code to do actions while we are in this state
            BlockDetails bd = (BlockDetails)entity;
            try
            {
                BorderDetails bdet = new BorderDetails();

                AcadUtils.DwgUtils Utils = new DwgUtils();

                Document TempDoc = Application.DocumentManager.MdiActiveDocument;
                Database TempDB = TempDoc.Database;
                //Database db = new Database(false, true);


                if (bd.lbDwgs.Count > 0)
                {
                    bd.DrawingFileName = (string)bd.lbDwgs[bd.StartNo];

                    DocumentCollection acDocMgr = Application.DocumentManager;

                    Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument = acDocMgr.Open(bd.DrawingFileName, false);
                    Document doc = Application.DocumentManager.MdiActiveDocument;

                    Database db = doc.Database;
                    bd.SpaceID = Utils.GetMSpaceID(db);
                    ObjectId msId;
                    DocumentLock dl = doc.LockDocument(DocumentLockMode.ProtectedAutoWrite, null, null, true);
                    Transaction tr = doc.TransactionManager.StartTransaction();
                    using (dl)
                    {
                        using (tr)
                        {
                            using (db)
                            {

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

                                            // Check each of the attributes...

                                            foreach (ObjectId arId in br.AttributeCollection)
                                            {
                                                DBObject obj = tr.GetObject(arId, OpenMode.ForRead);

                                                AttributeReference ar = obj as AttributeReference;

                                                if (ar.Tag.ToUpper() == "CABLEID".ToUpper())
                                                {
                                                    for (int i = 0; i < bd.Cable_Numbers.Count; i++)
                                                    {
                                                        //Helper.InfoMessageBox(bd.Cable_Numbers[i]);
                                                        if (ar.TextString == bd.Cable_Numbers[i])
                                                        {
                                                            bd.Drawings[i] = bd.Drawings[i] + "," + Path.GetFileNameWithoutExtension(bd.DrawingFileName);
                                                        }
                                                    }

                                                }
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }

                    tr.Dispose();

                    doc.CloseAndDiscard();

                }  

                //Helper.InfoMessageBox(bdet.BannerFilename + "Exists");

                //string TempFile = Path.Combine(Path.GetDirectoryName(bd.DrawingFileName), Path.GetFileName(bd.DrawingFileName).Replace(".dwg",".tmp"));
                //Helper.InfoMessageBox(TempFile);
                //db.SaveAs(TempFile, DwgVersion.Current);
                //File.Replace(bd.DrawingFileName, TempFile, Path.GetFullPath(bd.DrawingFileName).Replace(".dwg", ".bak"));
                //File.Delete(TempFile);

                bd.ChangeState(IncrementDWG.Instance);
            }
            catch (System.Exception ex)
            {
                bd.DidItOk = false;
                bd.ErrorMessage = ex.ToString();
                bd.CriticalError = 3;
                bd.ChangeState(ErrorState.Instance);

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
            
            try
            {
                Transaction Trans = db.TransactionManager.StartTransaction();
                AcadUtils.DwgUtils Utils = new AcadUtils.DwgUtils();
                Point3d p1 = db.Extmin;
                Point3d p2 = db.Extmax;
                double XDistance = p2.X - p1.X;
                BkDetails.BannerInsertionPoint = new Point3d(p2.X / 2, p2.Y / 2, 0);
                BkDetails.BannerScale = new Scale3d(XDistance / 397, XDistance / 397, 1);
                BkDetails.BannerRotation = 0.34906585;
                ObjectId msId = Utils.GetMSpaceID(db);

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
                    // if (!bt.Has("cableFinder"))
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
                //ed.Regen();
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


    }
}
