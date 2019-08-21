using System;
using System.Drawing;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.IO;
using acadApp = Autodesk.AutoCAD.ApplicationServices.Application;
using System.Windows.Data;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Windows;
using Autodesk.AutoCAD.Interop;
using GPCLServiceCentral;
using CadLogixUtil;
using System.Runtime.InteropServices;

[assembly: ExtensionApplication(typeof(AcadUtils.DwgUtils))]
namespace AcadUtils
{
    public class ReferenceObject
    {
        public string ReferenceNumber { get; set; }
        public string ReferenceTitle { get; set; }
        public string ErrorMessage { get; set; }
        public Boolean Result { get; set; }

    }
    public class DeletionObject
    {
        public string BlockName { get; set; }
        public Boolean DeleteResult { get; set; }
        public int BlockCount { get; set; }
        public string DeleteError { get; set; }
        public ObjectId BlockId { get; set; }
    }
    public class AttributeObject
    {
        public string AttName { get; set; }
        public string AttPrompt { get; set; }
        public string AttValue { get; set; }
        public string Blockname { get; set; }
        public string BorderName { get; set; }
    }
    public class ValidationObject
    {
        public string XmlString { get; set; } //XML parsed to a string
        public string schemaFile { get; set; } //String that is the Schema file name
        public Boolean ValidateResult { get; set; }
        public string ValidateError { get; set; }
    }
    public class DrawingImageObject
    {
        public string SaveLocation { get; set; }
        public string ErrorMessage { get; set; }
        public Boolean ImageResult { get; set; }
        public ImageType MyImage { get; set; }
        public uint SizeX { get; set; }
        public uint SizeY { get; set; }
        public uint BackGColor { get; set; }
    }
    public enum ImageType
    {
        Bitmap,
        Jpeg,
        Icon,
        WMF,
        Tiff,
        Gif
    }
    public class SetTextWidth
    {
        public string BlockName { get; set; }
        public string AttributeName { get; set; }
        public Boolean Result { get; set; }
        public double TextWidth { get; set; }
        public ObjectId SpaceId { get; set; }

    }
    public class ReturnMessage
    {
        public bool Result { get; set; }
        public string ErrorMessage { get; set; }
        public int CriticalError { get; set; }

    }
    public class Metrics : ReturnMessage
    {
        public int BorderClass { get; set; }
        public double BorderRevision { get; set; }
        public string BorderName { get; set; }
        public int RevisionQty { get; set; }
        public int RevisionDescription { get; set; }
        public int Drawn { get; set; }
        public int ReferenceDescription { get; set; }
        public int Title { get; set; }

    }
    public class DwgUtils : IExtensionApplication
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DwgUtils"/> class.
        /// </summary>
        public DwgUtils()
        {
        }
        //Parameters for XML Validation
        static string ErrorMessage = "";
        static int ErrorsCount = 0;
        public static string[] Borders { get; set; }        //

        public Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        /// <summary>
        /// Terminates this instance.
        /// </summary>
        public void Terminate()
        {
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            try
            {
                //TODO 
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        /// <summary>
        /// The AttributeData method returns the tag and text data of a specific attribute in a specific block.
        /// </summary>
        /// <param name="SpaceId">The drawing space id model or paper.</param>
        /// <param name="AO">The AttributeObject.</param>
        /// <returns></returns>
        public AttributeObject AttributeData(ObjectId SpaceId, AttributeObject AO)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            try
            {
                Transaction tr = doc.TransactionManager.StartTransaction();
                CadLogixUtil.Meta clum = new Meta();

                using (tr)
                {
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(SpaceId, OpenMode.ForRead);

                    // Test each entity in the container...

                    foreach (ObjectId entId in btr)
                    {

                        Entity ent = tr.GetObject(entId, OpenMode.ForRead)

                          as Entity;

                        if (ent != null)
                        {
                            BlockReference br = ent as BlockReference;

                            if (br != null)
                            {
                                BlockTableRecord bd = (BlockTableRecord)tr.GetObject(br.BlockTableRecord, OpenMode.ForRead);

                                // ... to see whether it's a block with

                                // the name we're after
                                if (bd.Name.ToUpper() == AO.Blockname.ToUpper())
                                {
                                    if (bd.HasAttributeDefinitions)
                                    {
                                        foreach (ObjectId id in bd)
                                        {
                                            DBObject obj2 = tr.GetObject(id, OpenMode.ForRead);
                                            AttributeDefinition ad = obj2 as AttributeDefinition;
                                            if (ad != null)
                                            {
                                                if (ad.Tag.ToUpper() == AO.AttName)
                                                {
                                                    AO.AttPrompt = ad.Prompt;
                                                }
                                            }
                                        }
                                    }
                                    // Check each of the attributes...
                                    int theCount = br.AttributeCollection.Count;

                                    foreach (ObjectId arId in br.AttributeCollection)
                                    {

                                        DBObject obj = tr.GetObject(arId, OpenMode.ForRead);

                                        AttributeReference ar = obj as AttributeReference;

                                        if (ar != null)
                                        {

                                            // ... to see whether it has the tag we're after
                                            if ((ar.Tag.ToUpper() == AO.AttName))
                                            {

                                                // If so, update the value

                                                AO.AttValue = ar.TextString;
                                                AO.AttName = ar.Tag;

                                            }

                                        }

                                    }
                                    return AO;
                                }

                                AttributeData(br.BlockTableRecord, AO);
                            }

                        }

                    }

                    tr.Commit();
                }
                return AO;
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage(ex.ToString());
                return AO;
            }
        }

        /// <summary>
        /// Counts the blocks in ModelSpase and if not found there
        /// looks for them in PaperSpace. It returns the ObjectId of the 
        /// 'Space' where the block was found.
        /// </summary>
        /// <param name="blockName">Name of the block.</param>
        /// <returns></returns>
        public ObjectId CountBorderBlock(string blockName)
        {
            ObjectId noId = ObjectId.Null;
            try
            {
                Document doc = Application.DocumentManager.MdiActiveDocument;
                Database db = doc.Database;
                ObjectId psId;
                ObjectId msId = GetMSpaceID(db);

                if (CountBlocks(msId, blockName) >= 1)
                {
                    return msId;
                }

                else
                {
                    psId = GetPSpaceID(db);
                }
                if (CountBlocks(psId, blockName) >= 1)
                {
                    return psId;
                }
                return noId;
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage(ex.ToString());
                return noId;

            }
        }

        /// <summary>
        /// Counts the blocks in ModelSpase and if not found there
        /// looks for them in PaperSpace. It returns the ObjectId of the 
        /// 'Space' where the block was found.
        /// </summary>
        /// <param name="blockName">Name of the block.</param>
        /// <returns></returns>
        public ObjectId CountBorderBlock(Database db, string blockName)
        {
            ObjectId noId = ObjectId.Null;
            try
            {
                ObjectId psId;
                ObjectId msId = GetMSpaceID(db);

                if (CountBlocks(db, msId, blockName) >= 1)
                {
                    return msId;
                }

                else
                {
                    psId = GetPSpaceID(db);
                }
                if (CountBlocks(db, psId, blockName) >= 1)
                {
                    return psId;
                }
                return noId;
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage(ex.ToString());
                return noId;

            }
        }

        /// <summary>
        /// Counts the blocks of a given name in ModelSpace and PaperSpace by 
        /// searching each of the blocktablerecords.
        /// </summary>
        /// <param name="btrId">The BlockTableRecord id.</param>
        /// <param name="blockName">Name of the block.</param>
        /// <returns></returns>
        public int CountBlocks(ObjectId btrId, string blockName)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            int changedCount = 0;

            try
            {
                Transaction tr = doc.TransactionManager.StartTransaction();

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

                                if (bd.Name.ToUpper() == blockName.ToUpper())
                                {
                                    // Recurse for nested blocks

                                    changedCount++;

                                }
                            }

                        }

                    }

                    tr.Commit();

                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage(ex.ToString());
            }

            return changedCount;
        }
        public int CountBlocks(Database db, ObjectId btrId, string blockName)
        {
            int changedCount = 0;

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

                                if (bd.Name.ToUpper() == blockName.ToUpper())
                                {
                                    // Recurse for nested blocks

                                    changedCount++;

                                }
                            }

                        }

                    }

                    tr.Commit();

                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage(ex.ToString());
            }

            return changedCount;
        }

        /// <summary>
        /// Gets the ModelSpace BlockTableRecord AutoCad ObjectId.
        /// </summary>
        /// <param name="db">The database of the active document.</param>
        /// <returns></returns>
        public ObjectId GetMSpaceID(Database db)
        {
            ObjectId msId1;
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Transaction tr = db.TransactionManager.StartTransaction();

            using (tr)
            {
                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                msId1 = bt[BlockTableRecord.ModelSpace];
            }
            return msId1;
        }

        /// <summary>
        /// Gets the PaperSpace BlockTableRecord AutoCad ObjectId.
        /// </summary>
        /// <param name="db">The db.</param>
        /// <returns></returns>
        public ObjectId GetPSpaceID(Database db)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;

            Transaction tr = db.TransactionManager.StartTransaction();
            ObjectId psId1;
            using (tr)
            {

                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);

                psId1 = bt[BlockTableRecord.PaperSpace];

            }
            return psId1;
        }

        /// <summary>
        /// Updates the attributes in the specified block by searching the drawing database
        /// and matching the blockname with a block reference in the the BlockTableRecords.
        /// </summary>
        /// <param name="btrId">The BlockTableRecord object id.</param>
        /// <param name="blockName">Name of the block.</param>
        /// <param name="attbName">Name of the attb.</param>
        /// <param name="attbValue">The attb value.</param>
        /// <returns></returns>
        public int UpdateAttributesInBlock(ObjectId btrId, string blockName, string attbName, string attbValue)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;


            int changedCount = 0;

            try
            {
                Transaction tr = doc.TransactionManager.StartTransaction();

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

                                if (bd.Name.ToUpper() == blockName)
                                {
                                    // Check each of the attributes...

                                    foreach (ObjectId arId in br.AttributeCollection)
                                    {
                                        DBObject obj = tr.GetObject(arId, OpenMode.ForRead);

                                        AttributeReference ar = obj as AttributeReference;

                                        if (ar != null)
                                        {
                                            // ... to see whether it has the tag we're after

                                            if (ar.Tag.ToUpper() == attbName)
                                            {
                                                // If so, update the value and increment the counter

                                                ar.UpgradeOpen();

                                                ar.TextString = attbValue;

                                                ar.DowngradeOpen();

                                                changedCount++;
                                            }
                                        }
                                    }
                                }

                                // Recurse for nested blocks

                                changedCount += UpdateAttributesInBlock(br.BlockTableRecord, blockName, attbName, attbValue);
                            }
                        }
                    }

                    tr.Commit();

                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage(ex.ToString());
            }

            return changedCount;

        }
        public int UpdateAttributesInBlock(Database db, ObjectId btrId, string blockName, string attbName, string attbValue)
        {
            int changedCount = 0;
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

                                if (bd.Name.ToUpper() == blockName)
                                {
                                    // Check each of the attributes...

                                    foreach (ObjectId arId in br.AttributeCollection)
                                    {
                                        DBObject obj = tr.GetObject(arId, OpenMode.ForRead);

                                        AttributeReference ar = obj as AttributeReference;

                                        if (ar != null)
                                        {
                                            // ... to see whether it has the tag we're after

                                            if (ar.Tag.ToUpper() == attbName)
                                            {
                                                // If so, update the value and increment the counter

                                                ar.UpgradeOpen();

                                                ar.TextString = attbValue;

                                                ar.DowngradeOpen();

                                                changedCount++;
                                            }
                                        }
                                    }
                                }

                                // Recurse for nested blocks

                                changedCount += UpdateAttributesInBlock(db, br.BlockTableRecord, blockName, attbName, attbValue);
                            }
                        }
                    }

                    tr.Commit();

                }
            }
            catch (System.Exception ex)
            {

                ed.WriteMessage(ex.ToString());
            }
            return changedCount;

        }

        /// <summary>
        /// Reads the displayed text string of the attribute of a block
        /// by searching the drawing database blocktablerecords for block
        /// references that match the given name.
        /// </summary>
        /// <param name="btrId">The BlockTableRecord id.</param>
        /// <param name="blockName">Name of the block.</param>
        /// <param name="attbName">Name of the attb.</param>
        /// <returns></returns>
        public string ReadAttributesInBlock(ObjectId btrId, string blockName, string attbName)
        {
            string attbReadValue = "";
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            try
            {
                Transaction tr = doc.TransactionManager.StartTransaction();

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


                                if (bd.Name.ToUpper() == blockName.ToUpper())
                                {
                                    // Check each of the attributes...

                                    foreach (ObjectId arId in br.AttributeCollection)
                                    {
                                        DBObject obj = tr.GetObject(arId, OpenMode.ForRead);

                                        AttributeReference ar = obj as AttributeReference;

                                        if (ar != null)
                                        {
                                            // ... to see whether it has the tag we're after

                                            if (ar.Tag.ToUpper() == attbName.ToUpper())
                                            {

                                                // If so, update the value

                                                attbReadValue = ar.TextString;
                                            }
                                        }
                                    }

                                    return attbReadValue;
                                }
                                // Recurse for nested blocks
                                //ReadAttributesInBlock(br.BlockTableRecord, blockName, attbName);
                            }
                        }
                    }
                    tr.Commit();
                }
            }
            catch (System.Exception ex)
            {

                ed.WriteMessage(ex.ToString());
            }
            return null;
        }

        /// <summary>
        /// Reads the displayed text string of the attribute of a block
        /// by searching the drawing database blocktablerecords for block
        /// references that match the given name.
        /// </summary>
        /// <param name="btrId">The BlockTableRecord id.</param>
        /// <param name="blockName">Name of the block.</param>
        /// <param name="attbName">Name of the attb.</param>
        /// <returns></returns>
        public string ReadAttributesInBlock(Database db, ObjectId btrId, string blockName, string attbName)
        {
            string attbReadValue = "";

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


                                if (bd.Name.ToUpper() == blockName.ToUpper())
                                {
                                    // Check each of the attributes...

                                    foreach (ObjectId arId in br.AttributeCollection)
                                    {
                                        DBObject obj = tr.GetObject(arId, OpenMode.ForRead);

                                        AttributeReference ar = obj as AttributeReference;

                                        if (ar != null)
                                        {
                                            // ... to see whether it has the tag we're after

                                            if (ar.Tag.ToUpper() == attbName.ToUpper())
                                            {

                                                // If so, update the value

                                                attbReadValue = ar.TextString;
                                            }
                                        }
                                    }

                                    return attbReadValue;
                                }
                                // Recurse for nested blocks
                                ReadAttributesInBlock(db, br.BlockTableRecord, blockName, attbName);
                            }
                        }
                    }
                    tr.Commit();
                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage(ex.ToString());
            }
            return null;
        }

        public string GetConstantAttribute(Database db, string BlockName, string AttbName)
        {
            AcadUtils.DwgUtils Utils = new DwgUtils();

            BlockTableRecord blktblrec = new BlockTableRecord();
            string AttConst = string.Empty;

            try
            {
                Transaction tr = db.TransactionManager.StartTransaction();

                using (tr)
                {
                    BlockTable BlkTble = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                    if (BlkTble.Has(BlockName.ToUpper()))
                    {
                        //open the block table record of the block definition
                        //that we want to add attributes to.
                        blktblrec = (BlockTableRecord)tr.GetObject(BlkTble[BlockName], OpenMode.ForRead);

                        if (blktblrec.HasAttributeDefinitions)
                        {
                            foreach (ObjectId id in blktblrec)
                            {
                                AttributeDefinition AttDef = tr.GetObject(id, OpenMode.ForRead) as AttributeDefinition;

                                if (AttDef != null)
                                {
                                    if (AttDef.Tag.ToUpper() == AttbName.ToUpper())
                                    {
                                        AttConst = AttDef.TextString;
                                        //Helper.InfoMessageBox(AttbName + " String Value = " + AttConst);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    tr.Commit();
                }

                return AttConst;
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage(ex.ToString());
                return string.Empty;

            }
        }

        /// <summary>
        /// Gets the Class revision of the current border from the MetaDataClasses2.xml file.
        /// </summary>
        /// <param name="BorderName">Name of the border.</param>
        /// <returns></returns>
        public string GetTheClassRevision(string BorderName)
        {
            XmlDocument RetBordClasses = new XmlDocument();
            string ClassRevision;
            XmlDocument ClassesDoc = new XmlDocument();

            try
            {
                var MyIni = new IniFile("Setting.ini");
                string docPath = MyIni.Read("AcadUtilsMetaDataXMLPath", "AcadUtils"); // @"C:\DataX\Xml\MetaDataClasses2.xml";

                if (System.IO.File.Exists(docPath))
                {
                    //Create File Stream
                    FileStream docIn = new FileStream(docPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                    //Load Document
                    ClassesDoc.Load(docIn);

                    XmlNodeList ClassessList = ClassesDoc.GetElementsByTagName("Class");
                    int ClassesNodeCount = ClassessList.Count;

                    for (int i = 0; i < ClassessList.Count; i++)
                    {
                        XmlAttributeCollection attrColl = ClassessList[i].Attributes;

                        XmlNodeList ClassesChildNodes = ClassessList[i].ChildNodes;

                        for (int j = 0; j < ClassesChildNodes.Count; j++)
                        {
                            if (ClassesChildNodes[j].InnerText == BorderName)
                            {
                                for (int l = 0; l < attrColl.Count; l++)
                                {
                                    if (attrColl[l].Name == "ClassRevision")
                                    {
                                        ClassRevision = attrColl[l].Value;
                                        return ClassRevision;
                                    }
                                }
                            }
                        }

                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(docPath + " does not exist");
                }

            }
            catch (System.Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
                ed.WriteMessage(ex.ToString());
            }

            return null;
        }

        /// <summary>
        /// Gets the ClassId of the border from 'MetaDataClasses2.xml'.
        /// </summary>
        /// <param name="BorderName">Name of the border.</param>
        /// <returns>string</returns>
        public string GetTheClassID(string BorderName)
        {
            XmlDocument RetBordClasses = new XmlDocument();
            string ClassID = "";
            XmlDocument ClassesDoc = new XmlDocument();

            try
            {
                var MyIni = new IniFile("Setting.ini");
                string docPath = MyIni.Read("AcadUtilsMetaDataXMLPath", "AcadUtils"); //"C:\\DataX\\Xml\\MetaDataClasses2.xml";

                if (System.IO.File.Exists(docPath))
                {
                    //Create File Stream
                    FileStream docIn = new FileStream(docPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                    //Load Document
                    ClassesDoc.Load(docIn);
                    XmlNodeList ClassessList = ClassesDoc.GetElementsByTagName("Class");
                    int ClassesNodeCount = ClassessList.Count;

                    for (int i = 0; i < ClassessList.Count; i++)
                    {
                        XmlAttributeCollection attrColl = ClassessList[i].Attributes;

                        XmlNodeList ClassesChildNodes = ClassessList[i].ChildNodes;

                        for (int j = 0; j < ClassesChildNodes.Count; j++)
                        {
                            if (ClassesChildNodes[j].InnerText == BorderName)
                            {
                                for (int l = 0; l < attrColl.Count; l++)
                                {
                                    if (attrColl[l].Name == "ClassId")
                                    {
                                        ClassID = attrColl[l].Value;
                                        return ClassID;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(docPath + " does not exist");
                }

            }
            catch (System.Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
                ed.WriteMessage(ex.ToString());
            }

            return null;
        }

        /// <summary>
        /// Gets the Metrics of the border from 'MetaDataClasses2.xml'.
        /// This infirmation is used to 'squeeze' the drawing metadata into the 
        /// border fields
        /// </summary>
        /// <returns>The Metrics object</returns>

        public Metrics GetTheMetrics()
        {
            Metrics metrix = new Metrics();
            Meta clum = new Meta();
            metrix.BorderName = clum.ReadMetaDataBorder();
            metrix.BorderClass = clum.ReadMetaDataClass();
            metrix.BorderRevision = clum.ReadMetaDataRevision();
            metrix.Result = false;

            try
            {
                var MyIni = new IniFile("Setting.ini");
                string docPath = MyIni.Read("AcadUtilsMetaDataXMLPath", "AcadUtils"); //"C:\\DataX\\Xml\\MetaDataClasses2.xml";
                if (System.IO.File.Exists(docPath))
                {
                    XElement Classes = XElement.Load(docPath);
                    IEnumerable<XElement> MyClass = from node in Classes.Descendants("Metrics")
                                                    where (string)node.Parent.Attribute("ClassId") == metrix.BorderClass.ToString() && (string)node.Parent.Attribute("ClassRevision") == metrix.BorderRevision.ToString()
                                                    select node;

                    foreach (XNode node in MyClass)
                    {
                        if (node is XElement)
                        {
                            IEnumerable<XObject> MyMetrics = from xobj in (node as XElement).Descendants() select (XObject)xobj;
                            foreach (XObject xo in MyMetrics)
                            {
                                if (((XElement)xo).Name == "RevisionQty")
                                {
                                    metrix.RevisionQty = Convert.ToInt32(((XElement)xo).Value);
                                }
                                else if (((XElement)xo).Name == "RevisionDescription")
                                {
                                    metrix.RevisionDescription = Convert.ToInt32(((XElement)xo).Value);
                                }
                                else if (((XElement)xo).Name == "ReferenceDescription")
                                {
                                    metrix.ReferenceDescription = Convert.ToInt32(((XElement)xo).Value);
                                }
                                else if (((XElement)xo).Name == "Drawn")
                                {
                                    metrix.Drawn = Convert.ToInt32(((XElement)xo).Value);
                                }
                                else if (((XElement)xo).Name == "Title")
                                {
                                    metrix.Title = Convert.ToInt32(((XElement)xo).Value);
                                }

                            }
                        }
                    }

                    metrix.Result = true;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(docPath + " does not exist");
                }

            }
            catch (System.Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
                ed.WriteMessage(ex.ToString());
            }


            return metrix;
        }

        /// <summary>
        /// Gets both the ModelSpace and PaperSpace container ObjectID's then parses ID and blockName
        /// to CountBlocks to get the total number of blocks present in the drawing.
        /// </summary>
        /// <param name="blockName">Name of the block.</param>
        /// <returns>ReturnedBlockCount</returns>

        public int BlockCount(string blockName)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            AcadUtils.DwgUtils Utils = new DwgUtils();
            Database db = doc.Database;

            int ReturnedBlockCount = 0;
            try
            {
                ObjectId msId = Utils.GetMSpaceID(db);

                ObjectId psId = Utils.GetPSpaceID(db);

                int MSBlockCount = Utils.CountBlocks(msId, blockName);

                int PSBlockCount = Utils.CountBlocks(psId, blockName);

                ReturnedBlockCount = MSBlockCount + PSBlockCount;
            }
            catch (System.Exception)
            {

            }


            return ReturnedBlockCount;
        }



        /// <summary>
        /// Gets both the ModelSpace and PaperSpace container ObjectID's then parses ID and blockName
        /// </summary>
        /// <param name="db">The draing database</param>
        /// <param name="blockName">Name of the block.</param>
        /// <returns></returns>
        public int BlockCount(Database db, string blockName)
        {
            AcadUtils.DwgUtils Utils = new DwgUtils();

            int ReturnedBlockCount = 0;
            try
            {
                ObjectId msId = Utils.GetMSpaceID(db);

                ObjectId psId = Utils.GetPSpaceID(db);

                int MSBlockCount = Utils.CountBlocks(db, msId, blockName);

                int PSBlockCount = Utils.CountBlocks(db, psId, blockName);

                ReturnedBlockCount = MSBlockCount + PSBlockCount;
            }
            catch (System.Exception)
            {

            }


            return ReturnedBlockCount;
        }


        /// <summary>
        /// Reads the Border Names contained in 'MetaDataClasses2.xml' and checks
        /// for a displayed instance in the drawing.
        /// </summary>
        /// <returns></returns>
        public string GetTheBorderName()
        {
            //Document doc = Application.DocumentManager.MdiActiveDocument;

            XmlDocument RetBordClasses = new XmlDocument();
            string ClassID = "";
            XmlDocument ClassesDoc = new XmlDocument();
            string blkName = string.Empty;

            try
            {
                var MyIni = new IniFile("Setting.ini");
                string docPath = MyIni.Read("AcadUtilsMetaDataXMLPath", "AcadUtils"); //"C:\\DataX\\Xml\\MetaDataClasses2.xml";
                if (System.IO.File.Exists(docPath))
                {
                    //Create File Stream
                    FileStream docIn = new FileStream(docPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                    //Load Document
                    ClassesDoc.Load(docIn);
                    XmlNodeList ClassessList = ClassesDoc.GetElementsByTagName("Class");

                    int ReturnedBlockCount = 0;

                    for (int i = 0; i < ClassessList.Count; i++)
                    {
                        XmlAttributeCollection attrColl = ClassessList[i].Attributes;
                        ClassID = attrColl[0].Value;
                        //Get the Child nodes of the current 'Class'
                        XmlNodeList ClassesChildNodes = ClassessList[i].ChildNodes;

                        for (int j = 0; j < ClassesChildNodes.Count; j++)
                        {
                            if (ClassesChildNodes[j].Name == "Border")
                            {
                                blkName = ClassesChildNodes[j].InnerText;

                                ReturnedBlockCount = BlockCount(blkName);
                                if (ReturnedBlockCount == 1)
                                {
                                    //ed.WriteMessage("\nThe Border is " + blkName);
                                    return blkName;
                                }
                            }
                        }

                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(docPath + " does not exist");
                }

            }
            catch (System.Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
                ed.WriteMessage(ex.ToString());
            }


            blkName = string.Empty;
            return blkName;
        }


        public string GetTheBorderName(Database db)
        {

            AcadUtils.DwgUtils Utils = new DwgUtils();
            XmlDocument RetBordClasses = new XmlDocument();
            string ClassID = "";
            XmlDocument ClassesDoc = new XmlDocument();
            string blkName = string.Empty;

            try
            {
                var MyIni = new IniFile("Setting.ini");
                string docPath = MyIni.Read("AcadUtilsMetaDataXMLPath", "AcadUtils"); //"C:\\DataX\\Xml\\MetaDataClasses2.xml";

                if (System.IO.File.Exists(docPath))
                {
                    //Create File Stream
                    FileStream docIn = new FileStream(docPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                    //Load Document
                    ClassesDoc.Load(docIn);
                    XmlNodeList ClassessList = ClassesDoc.GetElementsByTagName("Class");

                    int ReturnedBlockCount = 0;

                    for (int i = 0; i < ClassessList.Count; i++)
                    {
                        XmlAttributeCollection attrColl = ClassessList[i].Attributes;
                        ClassID = attrColl[0].Value;
                        //Get the Child nodes of the current 'Class'
                        XmlNodeList ClassesChildNodes = ClassessList[i].ChildNodes;

                        for (int j = 0; j < ClassesChildNodes.Count; j++)
                        {
                            if (ClassesChildNodes[j].Name == "Border")
                            {
                                blkName = ClassesChildNodes[j].InnerText;

                                ObjectId msId = Utils.GetMSpaceID(db);

                                ObjectId psId = Utils.GetPSpaceID(db);

                                int MSBlockCount = Utils.CountBlocks(db, msId, blkName);

                                int PSBlockCount = Utils.CountBlocks(db, psId, blkName);

                                ReturnedBlockCount = MSBlockCount + PSBlockCount;

                                if (ReturnedBlockCount == 1)
                                {
                                    //ed.WriteMessage("\nThe Border is " + blkName);
                                    return blkName;
                                }
                            }
                        }

                    }
                    blkName = string.Empty;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(docPath + " does not exist");
                }

            }
            catch (System.Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
                ed.WriteMessage(ex.ToString());
            }


            return blkName;
        }

        /// <summary>
        /// Adds multiple reference drawings to the current drawing attributes.
        /// </summary>
        /// <param name="SpaceID">The drawing 'space' ObjectId(Model or Paper).</param>
        /// <param name="BorderBlockName">Name of the border block.</param>
        /// <returns></returns>
        public string AddNewReference(ObjectId SpaceID, string BorderBlockName)
        {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            string DrawingNumber = string.Empty;
            string AddResult = string.Empty;
            string ReferenceDesc = string.Empty;
            try
            {
                Addreference addRef = new Addreference();
                System.Windows.Forms.DialogResult RefResult = addRef.ShowDialog();
                if (RefResult == System.Windows.Forms.DialogResult.Cancel)
                {
                    AddResult = "Add Reference Cancelled";
                    addRef.Close();
                }
                else
                {
                    addRef.Close();
                    const char Comma = ',';
                    char[] delimiters = new char[] { Comma };
                    String[] resultArray = addRef.DwgNumbers.Split(delimiters);
                    foreach (string SubString in resultArray)
                    {
                        DrawingNumber = SubString;
                        AddResult = AddNewReference(DrawingNumber);
                        Helper.InfoMessageBox(AddResult);
                    }
                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage(ex.ToString());
            }

            return AddResult;
        }



        /// <summary>
        /// Gets the title of a drawing from the Legacy data 
        /// via the LegacyTitle webservice.
        /// </summary>
        /// <param name="RO">The RO.</param>
        /// <returns></returns>
        public ReferenceObject GetRefTitle(ReferenceObject RO)
        {
            try
            {
                GPCLServiceCentral.ServiceStation Service = new ServiceStation();
                string[] RefTitle;
                RefTitle = Service.LegacyTitle(RO.ReferenceNumber);
                if (RefTitle[0] == "data ok")
                {
                    RO.ReferenceTitle = RefTitle[2] + " " + RefTitle[3];
                    RO.Result = true;
                }
                else
                {
                    RO.ErrorMessage = "Drawing " + " " + RO.ReferenceNumber + " does not exist, or its Title information is not available";
                    RO.Result = false;
                }
            }
            catch (System.Exception)
            {

            }

            return RO;
        }


        /// <summary>
        /// Adds a single reference to the current drawing .
        /// </summary>
        /// <param name="RefDrawingNumber">Drawing number of the reference drawing.</param>
        /// <returns>AddResult</returns>
        public string AddNewReference(string DrawingNumber)
        {
            string RefDescription = string.Empty;
            string[] RefTitle;
            string AddResult = string.Empty;
            string AttBlock = string.Empty;
            try
            {
                GPCLServiceCentral.ServiceStation Service = new ServiceStation();
                RefTitle = Service.LegacyTitle(DrawingNumber);

                Document doc = Application.DocumentManager.MdiActiveDocument;

                DocumentLock dl = doc.LockDocument(DocumentLockMode.ProtectedAutoWrite, null, null, true);
                using (dl)
                {
                    if (RefTitle[0] == "data ok")
                    {
                        RefDescription = RefTitle[2] + " " + RefTitle[3];
                        Meta data = new CadLogixUtil.Meta();

                        //initialize reference object
                        CadLogixUtil.MetaDataReference newRef = new MetaDataReference();
                        newRef.RefDwgNumber = DrawingNumber;
                        newRef.ReferenceDesc = RefDescription;

                        //add reference to matadata
                        newRef = data.AddNewReference(newRef);
                        if (newRef.Result)
                        {
                            Boolean RefreshResult = RefreshReferences();

                            if (RefreshResult)
                            {

                                AddResult = "Drawing " + " " + DrawingNumber + " Reference added to drawing";
                                ed.Regen();
                            }
                            else
                            {
                                AddResult = "Drawing " + " " + DrawingNumber + " Reference could not be added to the drawing.\n It was added to the metadata";
                            }
                        }
                    }
                    else
                    {
                        AddResult = "Drawing " + " " + DrawingNumber + " does not exist, or its Title information is not available";
                        return AddResult;
                    }
                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage(ex.ToString());
            }
            return AddResult;
        }

        /// <summary>
        /// Refreshes the reference drawing section of the drawing border.
        /// This is done after adding a reference or when the border is edited.
        /// </summary>
        /// <returns></returns>
        public Boolean RefreshReferences()
        {
            Boolean RefreshResult = false;
            CadLogixUtil.Meta clum = new Meta();
            string BorderBlockName = string.Empty;
            ObjectId SpaceID = ObjectId.Null;
            string AttBlock = string.Empty;
            int RefCount = 0;
            try
            {
                Metrics metrix = GetTheMetrics();

                //Get the border name and return the 'Space' it is in
                BorderBlockName = clum.ReadMetaDataBorder();
                SpaceID = CountBorderBlock(BorderBlockName);

                //get the attribute block name from the metadata block
                AttBlock = clum.ReadMetaDataBorderAttributes();

                //read the matadata from the matadata block
                CadLogixMetaData TitleData = clum.ReadMetaDataXML();

                //Count the references
                if (TitleData.References != null)
                {
                    RefCount = TitleData.References.Reference.Count;

                    for (int m = 1; m < metrix.RevisionQty; m++)
                    {
                        try
                        {
                            string RefTag = "REFDWG" + m.ToString();
                            string Title = "REFTITLE" + m.ToString();
                            int ua1 = UpdateAttributesInBlock(SpaceID, AttBlock, RefTag, "");
                            int ua2 = UpdateAttributesInBlock(SpaceID, AttBlock, Title, "");
                        }
                        catch (System.Exception)
                        {
                            RefreshResult = false;

                        }
                    }

                    if (TitleData.References.Reference.Count > metrix.RevisionQty)
                    {
                        RefCount = metrix.RevisionQty;
                    }
                    else
                    {
                        RefCount = TitleData.References.Reference.Count;
                    }
                    //iterate through the metadata references and populate the border
                    for (int k = 0; k < RefCount; k++)
                    {
                        string RefTag = "REFDWG" + (k + 1).ToString();
                        string Title = "REFTITLE" + (k + 1).ToString();
                        int ua1 = UpdateAttributesInBlock(SpaceID, AttBlock, RefTag, TitleData.References.Reference[k].ReferenceDrawingNumber);
                        int ua2 = UpdateAttributesInBlock(SpaceID, AttBlock, Title, TitleData.References.Reference[k].ReferenceDrawingTitle);

                        if (ua1 > 0 && ua2 > 0)
                        {
                            //Check the length
                            SetTextWidth stw = new SetTextWidth();
                            stw.BlockName = AttBlock;
                            stw.TextWidth = metrix.ReferenceDescription;
                            stw.AttributeName = Title;
                            stw.SpaceId = SpaceID;
                            Boolean Result = SetAttStringLength(SpaceID, stw);
                            RefreshResult = true;
                            ed.Regen();
                        }
                        else
                        {
                            RefreshResult = false;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage(ex.ToString());
            }

            return RefreshResult;
        }


        /// <summary>
        /// Changes the length of the attribute string to fit into the border fields.
        /// </summary>
        /// <param name="SpaceId">The spaceid where the border is inserted.</param>
        /// <param name="stw">The STW is the object that holds the data.</param>
        /// <returns></returns>
        public Boolean SetAttStringLength(ObjectId SpaceId, SetTextWidth stw)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            try
            {
                Transaction tr = doc.TransactionManager.StartTransaction();
                Point3d p1 = new Point3d();
                Point3d p2 = new Point3d();
                double TextWidth = 0;

                using (tr)
                {
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(SpaceId, OpenMode.ForRead);

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

                                if (bd.Name.ToUpper() == stw.BlockName.ToUpper())
                                {
                                    // Check each of the attributes...


                                    foreach (ObjectId arId in br.AttributeCollection)
                                    {
                                        DBObject obj = tr.GetObject(arId, OpenMode.ForRead);

                                        AttributeReference ar = obj as AttributeReference;

                                        if (ar != null)
                                        {
                                            // ... to see whether it has the tag we're after
                                            if (ar.Tag.ToUpper() == stw.AttributeName.ToUpper())
                                            {
                                                // If so, update the value and increment the counter

                                                ar.UpgradeOpen();
                                                ar.WidthFactor = 0.8;
                                                p1 = ar.GeometricExtents.MinPoint;
                                                p2 = ar.GeometricExtents.MaxPoint;
                                                double wf = ar.WidthFactor;
                                                Double DwgScale = br.ScaleFactors.X;
                                                TextWidth = Math.Abs(p1.X - p2.X);
                                                if (stw.TextWidth * DwgScale < TextWidth)
                                                {
                                                    //Helper.InfoMessageBox(stw.AttributeName + ":" + stw.TextWidth);
                                                    //Helper.InfoMessageBox(Convert.ToString((ar.WidthFactor * ((stw.TextWidth * DwgScale) / TextWidth)) - .01));
                                                    ar.WidthFactor = (ar.WidthFactor * ((stw.TextWidth * DwgScale) / TextWidth)) - .01;
                                                }

                                                ar.DowngradeOpen();

                                                stw.Result = true;
                                            }
                                        }
                                    }
                                }

                                SetAttStringLength(br.BlockTableRecord, stw);
                            }
                        }
                    }

                    tr.Commit();

                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage(ex.ToString());
            }
            return stw.Result;

        }



        /// <summary>
        /// Refreshes the revision attributes by reading the revision data from the matadata block.
        /// The data is passed to the Revision Writer.
        /// The revisions are written from the current revision at the top, down to the earliest rev.
        /// </summary>
        /// <returns>String with the result message</returns>

        /*
        
        public PopulateBorder.RefreshObject RefreshRevisionAttributes(PopulateBorder.RefreshObject ro)
        {
            //Metrics metrix = GetTheMetrics();
            CadLogixUtil.Meta clum = new Meta();
            ro.RevisionResult = false;
            int i;
            int RevCount = 0;
            CadLogixUtil.MetaDataRevision MDRev = new MetaDataRevision();
            //int RevisionQty = ClassLibrary1.Properties.Settings.Default.RevisionQty;

            try
            {
                CadLogixMetaData TitleData = clum.ReadMetaDataXML();
                string AttBlockName = clum.ReadMetaDataBorderAttributes();
                ObjectId SpaceID = CountBorderBlock(clum.ReadMetaDataBorder());

                if (TitleData.Revisions != null)
                {

                    for (int m = 1; m < ro.metrix.RevisionQty+1; m++)
                    {
                        try
                        {
                            string RefTag = "REFDWG" + m.ToString();
                            string Title = "REFTITLE" + m.ToString();
                            UpdateAttributesInBlock(SpaceID, AttBlockName, "REV" + (m).ToString(), "");
                            UpdateAttributesInBlock(SpaceID, AttBlockName, "REVDESC" + (m).ToString(),"");
                            UpdateAttributesInBlock(SpaceID, AttBlockName, "REVDATE" + (m).ToString(), "");
                            UpdateAttributesInBlock(SpaceID, AttBlockName, "DRAWN" + (m).ToString(), "");
                            UpdateAttributesInBlock(SpaceID, AttBlockName, "DWNDATE" + (m).ToString(), "");
                            UpdateAttributesInBlock(SpaceID, AttBlockName, "CHKD" + (m).ToString(), "");
                            UpdateAttributesInBlock(SpaceID, AttBlockName, "CHKDATE" + (m).ToString(), "");
                            UpdateAttributesInBlock(SpaceID, AttBlockName, "APPD" + (m).ToString(), "");
                            UpdateAttributesInBlock(SpaceID, AttBlockName, "APPDDATE" + (m).ToString(), "");
                        }
                        catch (System.Exception)
                        {
                        }
                    }
                    int startIndex = 0;
                    RevCount = TitleData.Revisions.Revision.Count;
                    if (RevCount > ro.metrix.RevisionQty)
                    {
                        startIndex = RevCount - ro.metrix.RevisionQty;
                    }
                    int IntRevCount = 0;
                    string MyCustomFormat = "dd/MM/yy";
                    for (i = startIndex; i < (RevCount); i++)
                    {
                        MDRev.RevisionID = TitleData.Revisions.Revision[i].RevisionID;
                        MDRev.RevisionLog = TitleData.Revisions.Revision[i].RevisionLog;
                        MDRev.RevisionTimestamp = TitleData.Revisions.Revision[i].RevisionTimeStamp.ToLocalTime().ToString(MyCustomFormat);
                        MDRev.Drawn = TitleData.Revisions.Revision[i].RevisionWorkflow.Drawn;
                        MDRev.DrawnTimestamp = TitleData.Revisions.Revision[i].RevisionWorkflow.DrawnTimeStamp.ToLocalTime().ToString(MyCustomFormat);
                        MDRev.Checked = TitleData.Revisions.Revision[i].RevisionWorkflow.Checked;
                        MDRev.CheckedTimestamp = TitleData.Revisions.Revision[i].RevisionWorkflow.CheckedTimeStamp.ToLocalTime().ToString(MyCustomFormat);
                        MDRev.Approved = TitleData.Revisions.Revision[i].RevisionWorkflow.Approved;
                        MDRev.ApprovedTimestamp = TitleData.Revisions.Revision[i].RevisionWorkflow.ApprovedTimeStamp.ToLocalTime().ToString(MyCustomFormat);
                        ro.RevisionResult = WriteRevision(SpaceID, AttBlockName, MDRev, IntRevCount);
                        IntRevCount++;
                    }
                    UpdateAttributesInBlock(SpaceID, AttBlockName, "DBREV", TitleData.Revisions.Revision[RevCount - 1].RevisionID);
                }
            }
            catch (System.Exception ex)
            {
                Helper.InfoMessageBox(ex.ToString());
                ro.RevisionResult = false;
            }
            ed.Regen();
            ed.Regen();
            return ro;
            
        }

        */





        /// <summary>
        /// Writes the revision into the border attribute block. This occurs after the border is edited.
        /// </summary>
        /// <param name="SpaceID">The spaceID where the border was inserted.</param>
        /// <param name="AttBlockName">Name of the attribute block nested in the border.</param>
        /// <param name="MDRev">The MDrev is the revision object that carries the data.</param>
        /// <param name="i">The i, the counter that points to the correct attribute</param>
        /// <returns></returns>
        private Boolean WriteRevision(ObjectId SpaceID, string AttBlockName, CadLogixUtil.MetaDataRevision MDRev, int i)
        {

            AcadUtils.DwgUtils Utils = new DwgUtils();
            int ua1 = 0;
            Boolean AddResult = false;
            SetTextWidth stw = new SetTextWidth();
            stw.BlockName = AttBlockName;
            Metrics metrix = GetTheMetrics();

            try
            {

                ua1 += UpdateAttributesInBlock(SpaceID, AttBlockName, "REV" + (i + 1).ToString(), MDRev.RevisionID);
                ua1 += UpdateAttributesInBlock(SpaceID, AttBlockName, "REVDESC" + (i + 1).ToString(), MDRev.RevisionLog);

                //checks the length of the revision description
                stw.TextWidth = metrix.RevisionDescription;
                stw.AttributeName = "REVDESC" + (i + 1).ToString();
                SetAttStringLength(SpaceID, stw);

                ua1 += UpdateAttributesInBlock(SpaceID, AttBlockName, "REVDATE" + (i + 1).ToString(), MDRev.RevisionTimestamp);
                ua1 += UpdateAttributesInBlock(SpaceID, AttBlockName, "DRAWN" + (i + 1).ToString(), MDRev.Drawn);

                //checks the length of the Artist 'Name'.
                stw.TextWidth = metrix.Drawn;
                stw.AttributeName = "DRAWN" + (i + 1).ToString();
                SetAttStringLength(SpaceID, stw);

                ua1 += UpdateAttributesInBlock(SpaceID, AttBlockName, "DWNDATE" + (i + 1).ToString(), MDRev.DrawnTimestamp);
                ua1 += UpdateAttributesInBlock(SpaceID, AttBlockName, "CHKD" + (i + 1).ToString(), MDRev.Checked);
                ua1 += UpdateAttributesInBlock(SpaceID, AttBlockName, "CHKDATE" + (i + 1).ToString(), MDRev.CheckedTimestamp);
                ua1 += UpdateAttributesInBlock(SpaceID, AttBlockName, "APPD" + (i + 1).ToString(), MDRev.Approved);
                ua1 += UpdateAttributesInBlock(SpaceID, AttBlockName, "APPDDATE" + (i + 1).ToString(), MDRev.ApprovedTimestamp);

                if (ua1 > 8)
                {
                    AddResult = true;
                }
                else
                {
                    AddResult = false;
                }
            }
            catch (System.Exception ex)
            {
                Helper.InfoMessageBox(ex.ToString());
                ed.WriteMessage(ex.ToString());

            }
            return AddResult;
        }


        /// <summary>
        /// Gets a list of the current approved drawing border names via the
        /// myservice.GetBorderNemes webservice The names are then extracted from
        /// the XML fragment and saved to a string array.
        /// </summary>
        /// <returns></returns>
        public BordersObject GetBorderNames()
        {
            XmlNodeList BordersNodeList = null;
            ServiceStation myservice = new ServiceStation();
            BordersObject bo = new BordersObject();
            try
            {
                bo = myservice.GetBorderNames(bo);

            }
            catch (System.Exception ex)
            {
                bo.Result = false;
                bo.ErrorMessage = ex.ToString();
                ed.WriteMessage(ex.ToString());
            }
            try
            {
                BordersNodeList = bo.BordersNode.SelectNodes("StandardBorders");
                int i;
                for (i = 0; i < BordersNodeList.Count; i++)
                {
                    if (BordersNodeList[i].HasChildNodes)
                    {
                        for (int j = 0; j < BordersNodeList[i].ChildNodes.Count; j++)
                        {
                            if (BordersNodeList[i].ChildNodes[j].Name == "Filename")
                            {
                                bo.Borders[i] = BordersNodeList[i].ChildNodes[j].InnerText;

                            }
                        }
                    }

                }
            }
            catch (System.Exception)
            {

            }

            bo.Result = true;
            bo.ErrorMessage = "Borders list returned OK";
            return bo;
        }



        /*
        /// <summary>
        /// Allows the user to select the drawing border from a list provided from server.
        /// </summary>
        /// <returns></returns>
        public ReturnMessage SelectBorder()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            UpgradeBorder.BlockDetails bd = new UpgradeBorder.BlockDetails();
            ReturnMessage rm = new ReturnMessage();
            BordersObject bo = new BordersObject();

            try
            {
                bo = GetBorderNames();
                bo.Result = true;
            }
            catch (System.Exception ex)
            {
                bo.Result = false;
                bo.ErrorMessage = ex.ToString();
                rm.Result = bo.Result;
                rm.ErrorMessage = bo.ErrorMessage;
                return rm;
            }
            if (bo.Result)
            {
                int i;
                Borders = new string[bo.Borders.Count()];
                for (i = 0; i < bo.Borders.Count(); i++)
                {
                    Borders[i] = bo.Borders[i];
                }

                try
                {
                    InsertBorderForm ibf = new InsertBorderForm();
                    System.Windows.Forms.DialogResult RefResult = ibf.ShowDialog();
                    if (RefResult == System.Windows.Forms.DialogResult.Cancel)
                    {
                        ibf.Close();
                        rm.ErrorMessage = "Could not get a list of drawings from server";
                        rm.Result = false;
                        return rm;
                    }
                    else
                    {
                        bd.BlockName = ibf.BorderName;


                        switch (bd.BlockName)
                        {
                            case "A0_BORDER.DWG":
                                bd.DrawingFileName = @"C:\Datax\A0_Border.dwg";
                                bd.AttBlkName = "GPCLA0ATTRIBUTES";
                                bd.NewBorder = "A0_BORDER";
                                break;

                            case "A1_BORDER.DWG":
                                bd.NewBorderFilename = @"C:\Datax\A1_Border.dwg";
                                bd.AttBlkName = "GPCLA1ATTRIBUTES";
                                bd.NewBorder = "A1_BORDER";
                                break;

                            case "A2_BORDER.DWG":
                                bd.NewBorderFilename = @"C:\Datax\A2_Border.dwg";
                                bd.AttBlkName = "GPCLA2ATTRIBUTES";
                                bd.NewBorder = "A2_BORDER";
                                break;

                            case "A3_BORDER.DWG":
                                bd.NewBorderFilename = @"C:\Datax\A3_Border.dwg";
                                bd.AttBlkName = "GPCLA3ATTRIBUTES";
                                bd.NewBorder = "A3_BORDER";
                                break;

                            case "A4_BORDER.DWG":
                                bd.NewBorderFilename = @"C:\Datax\A4_Border.dwg";
                                bd.AttBlkName = "GPCLA4ATTRIBUTES";
                                bd.NewBorder = "A4_BORDER";
                                break;

                            default:
                                bd.NewBorderFilename = @"C:\Datax\A2_Border.dwg";
                                bd.AttBlkName = "GPCLA2ATTRIBUTES";
                                bd.NewBorder = "A2_BORDER";
                                break;
                        }

                        bd.SpaceID = GetMSpaceID(db);
                        bd.BlockScale = new Scale3d(1, 1, 1);
                        bd.BlockRotation = 0;
                        bd.BlockInsertPoint = new Point3d(0, 0, 0);

                        
                        
                        
                        
                        bd = DownLoadBorder(bd);
                        bd = InsertDrawingBorder(bd);

                        Application.DocumentManager.MdiActiveDocument.Editor.Regen();

                        int count = BlockCount("MetaData");
                        if (count < 1)
                        {
                            bd = InsertMetaData(bd);
                        }
                        else
                        {
                            CadLogixUtil.Meta clum = new Meta();
                            clum.WriteMetaDataBorder(bd.NewBorder);
                            clum.WriteMetaDataBorderAttributes(bd.AttBlkName);
                            clum.WriteMetaDataClass(Convert.ToInt32(GetTheClassID(bd.NewBorder)));
                            clum.WriteMetaDataRevision(double.Parse(GetTheClassRevision(bd.NewBorder)));
                        }

                        //string TempFolder = po.FullPath;
                        //if(System.IO.Directory.Exists(TempFolder))
                        //{
                        //    System.IO.Directory.Delete(TempFolder,true);
                        //}


                    }

                    doc.Window.WindowState = Window.State.Maximized;
                    CommandLine.Command("._zoom", "_e");
                    ed.Regen();
                    rm.Result = true;
                    rm.ErrorMessage = "No Errors";
                    return rm;
                }
                catch (System.Exception ex)
                {
                    rm.Result = false;
                    rm.ErrorMessage = ex.ToString();
                    return rm;
                }
            }
            rm.Result = true;
            rm.ErrorMessage = "All Good";
            return rm;
        }

         */

        /// <summary>
        /// Downloads the nominated drawing border from the CAD server by calling the 'DownloadBorder' webservice
        /// It is saved to a temporary location where it is then inserted into the drawing.
        /// </summary>
        /// <param name="bd">The bd.</param>
        /// <returns></returns>


        /* 
         public UpgradeBorder.BlockDetails DownLoadBorder(UpgradeBorder.BlockDetails bd)
         {
             //Get border from Server
             try
             {
                 GPCLServiceCentral.PersistObject po = new PersistObject();
                 GPCLServiceCentral.ServiceStation Service = new ServiceStation();
                 po.DwgFileName = bd.NewBorder;
                 po.FullPath = @"C:\KKTemp";
                 po = Service.DownloadBorder(po);
                 if (po.Result)
                 {
                     bd.NewBorderFilename = System.IO.Path.Combine(po.FullPath, po.DwgFileName) + ".dwg";
                     bd.DidItOk = true;
                     bd.ErrorMessage = "Border downloaded OK";
                     return bd;
                 }
                 else
                 {
                     bd.DidItOk = false;
                     bd.ErrorMessage = "Failed to download drawing border from server";
                     return bd;
                 }
             }
             catch (System.Exception ex)
             {
                     bd.DidItOk = false;
                     bd.ErrorMessage = "Failed to download drawing border from server\n " + ex.ToString();
                     return bd;
             }


         }




         /// <summary>
         /// Inserts the nominated drawing border into the current active document.
         /// </summary>
         /// <param name="BkDetails">The bk details.</param>
         /// <returns></returns>
        public UpgradeBorder.BlockDetails InsertDrawingBorder(UpgradeBorder.BlockDetails BkDetails)
         {

            BkDetails.DidItOk = false;
             Document doc = Application.DocumentManager.MdiActiveDocument;
             Database db = doc.Database;
             Editor ed = doc.Editor;
             Transaction Trans = db.TransactionManager.StartTransaction();
             ObjectId msId = GetMSpaceID(db);
             try
             {
                 ObjectId BlockExistID = CountBorderBlock(BkDetails.NewBorder);
                 if (BlockExistID == ObjectId.Null)
                 {

                     Database Newdb = new Database(false, true);
                     using (Newdb)
                     {
                         //Read the .dwg file into a new temporary drawing database
                         Newdb.ReadDwgFile(BkDetails.NewBorderFilename, System.IO.FileShare.Read, false, "");

                         //Insert the temporary database record into the active document database
                         //with the name held by the 'BkDetails.NewBorder' string variable. This
                         //creates a block definition.
                         ObjectId BlockID = db.Insert(BkDetails.NewBorder, Newdb, false);

                         //BlockTable bt = (BlockTable)(Trans.GetObject(db.BlockTableId, OpenMode.ForRead));

                         //Open the ModelSpace BlockTableRecord (BTR), create a new block reference from the 
                         //block definition and append it to the BTR. Effectively creating a visible object instance
                         //of the block definition.
                         BlockTableRecord btr = (BlockTableRecord)Trans.GetObject(msId, OpenMode.ForWrite);
                         BlockReference br = new BlockReference(BkDetails.BlockInsertPoint, BlockID);
                         btr.AppendEntity(br);
                         //HyperLink hl = new HyperLink();
                         //hl.Description = "Cable";
                         //hl.Name = "http://rgtctweb/scripts/electrical/getcable.idc?TheCableID=CD2679";
                         //br.Hyperlinks.Add(hl);

                         //Complete the persistence data, the rotation and scale.
                         br.Rotation = BkDetails.BlockRotation;
                         br.ScaleFactors = BkDetails.BlockScale;
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
                                 if (AttDef != null)
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
                     BkDetails.DidItOk = true;
                 }
                 else
                 {
                     BkDetails.ErrorMessage = "Block " + BkDetails.NewBorder + " already exists";
                 }
             }
             catch (System.Exception ex)
             {
                 BkDetails.ErrorMessage = ex.ToString();

             }
             return BkDetails;
         }


        /// <summary>
        /// Requests the CadLogixUtils CreateMetaDataDefinition method to create
        /// a metadata block definition within the drawing. It then follows the
        /// same procedure as the insertDrawingBorder method to create an instance
        /// of the metadata block in modelspace. It is however invisible to the user.
        /// </summary>
        /// <param name="bd">The bd.</param>
        /// <returns></returns>
        public UpgradeBorder.BlockDetails InsertMetaData(UpgradeBorder.BlockDetails bd)
        {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            //Transaction Trans = db.TransactionManager.StartTransaction();
            CadLogixUtil.Meta clum = new Meta();
            ObjectId NewBlkId;
            using (Transaction Trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    string blkName = "METADATA";
                    int MetaDataBlockCount = BlockCount(blkName);

                    if (MetaDataBlockCount != 0)
                    {
                        bd.DidItOk = true;
                        return bd;
                    }
                    else
                    {



                        //Create the block reference...use the return from CreateMetaDataBlockDefinition directly!
                        NewBlkId = clum.CreateMetaDataDefinition();
                        BlockTable bt = (BlockTable)(Trans.GetObject(db.BlockTableId, OpenMode.ForRead));
                        BlockTableRecord btr = (BlockTableRecord)Trans.GetObject(bd.SpaceID, OpenMode.ForWrite);
                        BlockReference br = new BlockReference(new Point3d(0, -5, 0), NewBlkId);

                        //Add the reference to ModelSpace
                        btr.AppendEntity(br);

                        //Let the transaction know about it
                        Trans.AddNewlyCreatedDBObject(br, true);
                        BlockTableRecord BlkTblRec = Trans.GetObject(NewBlkId, OpenMode.ForRead) as BlockTableRecord;
                        if (BlkTblRec.HasAttributeDefinitions)
                        {
                            foreach (ObjectId objId in BlkTblRec)
                            {
                                AttributeDefinition AttDef = Trans.GetObject(objId, OpenMode.ForRead) as AttributeDefinition;
                                if (AttDef != null)
                                {
                                    AttributeReference AttRef = new AttributeReference();
                                    AttRef.SetAttributeFromBlock(AttDef, br.BlockTransform);
                                    br.AttributeCollection.AppendAttribute(AttRef);
                                    Trans.AddNewlyCreatedDBObject(AttRef, true);
                                }
                            }
                        }
                        // Commit is always required to indicate success
                        Trans.Commit();
                        bd.DidItOk = true;
                        CadLogixMetaData TitleData = new CadLogixMetaData();
                        TitleData = clum.GetSampleXML();
                        clum.WriteMetaDataXML(TitleData);
                        clum.WriteMetaDataBorder(bd.NewBorder);
                        clum.WriteMetaDataBorderAttributes(bd.AttBlkName);
                        clum.WriteMetaDataClass(Convert.ToInt32(GetTheClassID(bd.NewBorder)));
                        clum.WriteMetaDataRevision(double.Parse(GetTheClassRevision(bd.NewBorder)));
                    }
                    //
                    return bd;
                }
                catch (System.Exception ex)
                {
                    Helper.InfoMessageBox(ex.ToString());
                    bd.DidItOk = false;
                    return bd;
                }
            }
            }

         */


        /// <summary>
        /// Deletes the Acad object, all visible instances and purges block from drawing.
        /// It returns the object with the result and any errors.
        /// </summary>
        /// <param name="delObj">The del obj.</param>
        /// <returns>delObj</returns>
        public DeletionObject DeleteObject(DeletionObject delObj)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            try
            {
                Transaction tr = doc.TransactionManager.StartTransaction();
                ObjectId SpaceID = CountBorderBlock(delObj.BlockName);
                if (SpaceID != ObjectId.Null)
                {
                    delObj.BlockCount = CountBlocks(SpaceID, delObj.BlockName);
                    ObjectId ErasedBTId = ObjectId.Null;

                    if (delObj.BlockCount > 0)
                    {
                        using (tr)
                        {
                            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(SpaceID, OpenMode.ForRead);

                            // Test each entity in the container...
                            foreach (ObjectId entId in btr)
                            {
                                Entity ent = tr.GetObject(entId, OpenMode.ForWrite) as Entity;

                                if (ent != null)
                                {
                                    BlockReference br = ent as BlockReference;

                                    if (br != null)
                                    {
                                        BlockTableRecord btr2 = (BlockTableRecord)tr.GetObject(br.BlockTableRecord, OpenMode.ForWrite);

                                        // ... to see whether it's a block with the name we're after

                                        if (btr2.Name.ToUpper() == delObj.BlockName.ToUpper())
                                        {
                                            ErasedBTId = br.BlockTableRecord;
                                            br.Erase(true);

                                        }
                                    }

                                }

                            }

                            BlockTable bt = tr.GetObject(db.BlockTableId, OpenMode.ForWrite) as BlockTable;
                            ObjectIdCollection idColl = new ObjectIdCollection();
                            foreach (ObjectId MyId in bt)
                            {
                                if (MyId == ErasedBTId)
                                {
                                    idColl.Add(MyId);
                                }
                            }
                            db.Purge(idColl);
                            foreach (ObjectId id in idColl)
                            {
                                BlockTableRecord btr3 = (BlockTableRecord)tr.GetObject(id, OpenMode.ForWrite);
                                btr3.Erase(true);
                            }

                            tr.Commit();
                            tr.Dispose();
                        }
                        ed.Regen();
                    }
                }
                delObj.DeleteError = delObj.BlockCount.ToString() + " block(s) deleted";
                delObj.DeleteResult = true;
                return delObj;
            }
            catch (System.Exception ex)
            {
                delObj.DeleteError = "Could not delete blocks because:-\n" + ex.ToString();
                delObj.DeleteResult = false;
                ed.WriteMessage(ex.ToString());
                return delObj;
            }

        }


        /// <summary>
        /// Validates the XML against the schema provided and returns the same object.
        /// including the validation result and the error if any.
        /// </summary>
        /// <param name="XmlValObj">The XML validation object.</param>
        /// <returns>ValidationObject</returns>
        public ValidationObject ValidateXml(ValidationObject XmlValObj)
        {
            try
            {

                XmlReader xsd = new XmlTextReader(XmlValObj.schemaFile);
                XmlSchemaSet schema = new XmlSchemaSet();
                schema.Add(null, xsd);

                XmlReaderSettings xmlReadeSettings = new XmlReaderSettings();
                xmlReadeSettings.ValidationType = ValidationType.Schema;
                xmlReadeSettings.Schemas.Add(schema);
                xmlReadeSettings.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);

                StringReader sReader = new StringReader(XmlValObj.XmlString);
                XmlReader xmlReader = XmlReader.Create(sReader, xmlReadeSettings);


                XmlValObj.ValidateResult = true;

                XmlValObj.ValidateError = "";
                ErrorMessage = "";
                ErrorsCount = 0;

                while (xmlReader.Read()) ;
                xmlReader.Close();
                xsd.Close();

                // Raise exception, if XML validation fails
                if (ErrorsCount > 0)
                {
                    XmlValObj.ValidateResult = false;
                    XmlValObj.ValidateError = ErrorMessage;
                    //throw new System.Exception(ErrorMessage);
                    return XmlValObj;
                }
            }
            catch (System.Exception ex)
            {
                XmlValObj.ValidateResult = false;
                XmlValObj.ValidateError = ex.ToString();
                ed.WriteMessage(ex.ToString());
            }

            return XmlValObj;
        }


        /// <summary>
        /// Handles the event caused by an XML validation error.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.Xml.Schema.ValidationEventArgs"/> instance containing the event data.</param>
        public static void ValidationHandler(object sender, ValidationEventArgs args)
        {
            ErrorMessage = ErrorMessage + args.Message + "\r\n";
            ErrorsCount++;
        }

        /// <summary>
        /// Creates the drawing image.
        /// </summary>
        /// <param name="ImOb">The im ob.</param>
        /// <returns></returns>
        public DrawingImageObject CreateDrawingImage(DrawingImageObject ImOb)
        {
            try
            {
                DocumentCollection dm = Application.DocumentManager;
                Document Doc = Application.DocumentManager.MdiActiveDocument;
                Database db = Doc.Database;
                ViewTableRecord vr = new ViewTableRecord();
                string ImageExt = string.Empty;
                // default Image Format
                System.Drawing.Imaging.ImageFormat IFormat = IFormat = System.Drawing.Imaging.ImageFormat.Bmp;

                //Get drawig extents
                AcadApplication acadApp = (AcadApplication)Application.AcadApplication;
                acadApp.ZoomExtents();
                //Change background color
                AcadPreferences pref = acadApp.Preferences as AcadPreferences;
                var oldcolor = pref.Display.GraphicsWinModelBackgrndColor;
                pref.Display.GraphicsWinModelBackgrndColor = ImOb.BackGColor;
                ed.Regen();
                //pref.Display.GraphicsWinModelBackgrndColor = 16777215;

                Bitmap thumb = Doc.CapturePreviewImage(ImOb.SizeX, ImOb.SizeY);
                Doc.Database.ThumbnailBitmap = thumb;

                switch (ImOb.MyImage)
                {
                    case ImageType.Bitmap:
                        ImageExt = ".bmp";
                        IFormat = System.Drawing.Imaging.ImageFormat.Bmp;
                        break;

                    case ImageType.Gif:
                        ImageExt = ".gif";
                        IFormat = System.Drawing.Imaging.ImageFormat.Gif;
                        break;

                    case ImageType.Tiff:
                        ImageExt = ".Tif";
                        IFormat = System.Drawing.Imaging.ImageFormat.Tiff;
                        break;

                    case ImageType.WMF:
                        ImageExt = ".wmf";
                        IFormat = System.Drawing.Imaging.ImageFormat.Bmp;
                        break;

                    case ImageType.Icon:
                        ImageExt = ".ico";
                        IFormat = System.Drawing.Imaging.ImageFormat.Icon;
                        break;

                    case ImageType.Jpeg:
                        ImageExt = ".jpg";
                        IFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                        break;
                }

                thumb.Save(ImOb.SaveLocation + System.IO.Path.GetFileNameWithoutExtension(Doc.Name) + "_" + ImOb.BackGColor.ToString() + ImageExt, IFormat);
                ImOb.ErrorMessage = ImOb.SaveLocation + System.IO.Path.GetFileNameWithoutExtension(Doc.Name) + ImageExt + " created sucessfully";
                ImOb.ImageResult = true;
                //pref.Display.GraphicsWinModelBackgrndColor = oldcolor;
            }
            catch (System.Exception ex)
            {
                ImOb.ErrorMessage = ex.ToString();
                ImOb.ImageResult = false;
                ed.WriteMessage(ex.ToString());
            }

            return ImOb;
        }

        /// <summary>
        /// Calls GetRevAtts to read the textstrings from the Revisions 'AmendText' block if it exists.
        /// If successful the text strings are searched using regular expressions, concatenated etc and
        /// formatted into the new revision format.
        /// </summary>
        //[CommandMethod ("Revs")]
        public Boolean RevisionReader()
        {
            Boolean rResult = false;
            Document doc = Application.DocumentManager.MdiActiveDocument;
            try
            {
                CadLogixUtil.Meta clum = new CadLogixUtil.Meta();
                
                CadLogixUtil.MetaDataRevision NewRev = new CadLogixUtil.MetaDataRevision();
                AcadUtils.DwgUtils Utils = new DwgUtils();
                string RevBlkName = "AMENDTXT";
                string AttbName = "AMENDTEXT";
                ObjectId RevId = ObjectId.Null;
                string[] revstring = new string[10];
                string artist = "";
                string cdate = "";
                string cdesc = "";
                string crev = "";

                //int RevBlkCnt = Utils.BlockCount(RevBlkName);

                if (BlockCount(RevBlkName) != 1)
                {
                    NewRev.RevisionID = "-";
                    NewRev.RevisionLog = "-";
                    NewRev.RevisionTimestamp = DateTime.UtcNow.ToString();
                    NewRev.Drawn = "-";
                    NewRev.DrawnTimestamp = DateTime.UtcNow.ToString();
                    NewRev.Checked = "-";
                    NewRev.CheckedTimestamp = DateTime.UtcNow.ToString();
                    NewRev.Approved = "-";
                    NewRev.ApprovedTimestamp = DateTime.UtcNow.ToString();
                    clum.AddNewRevision(NewRev);

                    //Helper.InfoMessageBox("RevisionReader:Revision block 'AMENDTXT' could not be found\n or more that 1 block exists");
                    rResult = true;
                    return rResult;
                }

                else if (BlockCount(RevBlkName) == 1)
                {
                    RevId = CountBorderBlock(RevBlkName);
                    revstring = GetRevAtts(RevId, RevBlkName, AttbName);
                }


                Regex MyExp = new Regex("^(--)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                int StartIndex = 0;
                int EndIndex = 0;


                for (int i = 0; i < revstring.Length; i++)
                {
                    if (revstring[i] != null)
                    {
                        MatchCollection MatchList = MyExp.Matches(revstring[i]);

                        if (MatchList.Count == 1)
                        {
                            //Helper.InfoMessageBox("MyExp: " + MyExp + "\nString: " + revstring[i] + "\nrevMatch.Count: " + MatchList.Count + "\nIndex: " + i);

                            EndIndex = i;


                            //Regex DateExp = new Regex("(\\s[0]?[1-9]|[12][0-9]|3[01])[-/.]([0]?[1-9]|1[012])[- /.]([0-9]{4}|[0-9]{2})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                            //MatchCollection DateMatch = DateExp.Matches(revstring[EndIndex - 1]);
                            //GroupCollection dategroup = DateMatch[0].Groups;
                            //cdate = dategroup[0].Value.ToString();
                            //artist = (revstring[EndIndex - 1].Substring(0, dategroup[0].Index - 1).Trim());

                            cdate = revstring[i - 1].Substring(revstring[i - 1].LastIndexOf(" ") + 1);
                            artist = revstring[i - 1].Substring(0, revstring[i - 1].LastIndexOf(" ")).Trim();

                            Regex RevExp = new Regex("^([A-Z]+)\\s*[:.\\s]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                            MatchCollection RevMatch = RevExp.Matches(revstring[StartIndex]);

                            //Helper.InfoMessageBox("RevExp: " + RevExp + "\nString: " + revstring[StartIndex] + "\nrevMatch.Count: " + RevMatch.Count + "\nIndex: " + StartIndex.ToString());
                            if (RevMatch.Count == 1)
                            {
                                GroupCollection revgroup = RevMatch[0].Groups;


                                crev = revgroup[0].Value.Substring(0, revgroup[0].Length - 1);

                                cdesc = revstring[StartIndex].Substring(revgroup[0].Length).Trim();

                                for (int j = StartIndex + 1; j < EndIndex - 1; j++)
                                {
                                    cdesc = cdesc + " " + revstring[j].ToString();
                                }
                            }
                            else
                            {
                                crev = "X";
                                cdesc = "XXXX";
                            }
                            //Helper.InfoMessageBox("Rev: " + crev + "\nDate: " + cdate + "\nArtist: " + artist + "\nDesc: " + cdesc);
                            DateTime dt = DateTimeConvert(cdate).ToUniversalTime();
                            NewRev.RevisionID = crev;
                            NewRev.RevisionLog = cdesc;
                            NewRev.RevisionTimestamp = dt.ToString();
                            NewRev.Drawn = artist;
                            NewRev.DrawnTimestamp = dt.ToString();
                            NewRev.Checked = "-";
                            NewRev.CheckedTimestamp = dt.ToString();
                            NewRev.Approved = "-";
                            NewRev.ApprovedTimestamp = dt.ToString();

                            clum.AddNewRevision(NewRev);
                            StartIndex = EndIndex + 1;

                        }

                    }

                }

            }
            catch (System.Exception ex)
            {
                string ErrorString = "RevisionReader:Unexpected Error at end:" + doc.Name + ":" + ex;
                rResult = false;
                ed.WriteMessage(ex.ToString());

            }
            return rResult;
        }


        /// <summary>
        /// Calls GetRevAtts to read the textstrings from the Revisions 'AmendText' block if it exists.
        /// If successful the text strings are searched using regular expressions, concatenated etc and
        /// formatted into the new revision format.
        /// </summary>
        //[CommandMethod ("Revs")]
        public bool RevisionReader(Database db)
        {
            Boolean rResult = false;
            Document doc = Application.DocumentManager.MdiActiveDocument;

            try
            {
                CadLogixUtil.Meta clum = new CadLogixUtil.Meta();               
                CadLogixUtil.MetaDataRevision NewRev = new CadLogixUtil.MetaDataRevision();
                AcadUtils.DwgUtils Utils = new DwgUtils();
                string RevBlkName = "AMENDTXT";
                string AttbName = "AMENDTEXT";
                ObjectId RevId = ObjectId.Null;
                string[] revstring = new string[10];
                string artist = "";
                string cdate = "";
                string cdesc = "";
                string crev = "";

                //int RevBlkCnt = Utils.BlockCount(RevBlkName);

                if (BlockCount(db, RevBlkName) != 1)
                {
                    NewRev.RevisionID = "-";
                    NewRev.RevisionLog = "-";
                    NewRev.RevisionTimestamp = DateTime.UtcNow.ToString();
                    NewRev.Drawn = "-";
                    NewRev.DrawnTimestamp = DateTime.UtcNow.ToString();
                    NewRev.Checked = "-";
                    NewRev.CheckedTimestamp = DateTime.UtcNow.ToString();
                    NewRev.Approved = "-";
                    NewRev.ApprovedTimestamp = DateTime.UtcNow.ToString();
                    clum.AddNewRevision(db, NewRev);

                    //Helper.InfoMessageBox("RevisionReader:Revision block 'AMENDTXT' could not be found\n or more that 1 block exists");
                    rResult = true;
                    return rResult;
                }

                else if (BlockCount(db, RevBlkName) == 1)
                {
                    RevId = CountBorderBlock(db, RevBlkName);
                    revstring = GetRevAtts(db, RevId, RevBlkName, AttbName);
                }


                Regex MyExp = new Regex("^(--)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                int StartIndex = 0;
                int EndIndex = 0;
                for (int i = 0; i < revstring.Length; i++)
                {
                    if (revstring[i] != null)
                    {
                        MatchCollection MatchList = MyExp.Matches(revstring[i]);

                        if (MatchList.Count == 1)
                        {
                            //Helper.InfoMessageBox("MyExp: " + MyExp + "\nString: " + revstring[i] + "\nrevMatch.Count: " + MatchList.Count + "\nIndex: " + i);

                            EndIndex = i;


                            //Regex DateExp = new Regex("(\\s[0]?[1-9]|[12][0-9]|3[01])[-/.]([0]?[1-9]|1[012])[- /.]([0-9]{4}|[0-9]{2})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                            //MatchCollection DateMatch = DateExp.Matches(revstring[EndIndex - 1]);
                            //GroupCollection dategroup = DateMatch[0].Groups;
                            //cdate = dategroup[0].Value.ToString();
                            //artist = (revstring[EndIndex - 1].Substring(0, dategroup[0].Index - 1).Trim());

                            cdate = revstring[i - 1].Substring(revstring[i - 1].LastIndexOf(" ") + 1);
                            artist = revstring[i - 1].Substring(0, revstring[i - 1].LastIndexOf(" ")).Trim();

                            Regex RevExp = new Regex("^([A-Z]+)\\s*[:.\\s]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                            MatchCollection RevMatch = RevExp.Matches(revstring[StartIndex]);

                            //Helper.InfoMessageBox("RevExp: " + RevExp + "\nString: " + revstring[StartIndex] + "\nrevMatch.Count: " + RevMatch.Count + "\nIndex: " + StartIndex.ToString());
                            if (RevMatch.Count == 1)
                            {
                                GroupCollection revgroup = RevMatch[0].Groups;


                                crev = revgroup[0].Value.Substring(0, revgroup[0].Length - 1);

                                cdesc = revstring[StartIndex].Substring(revgroup[0].Length).Trim();

                                for (int j = StartIndex + 1; j < EndIndex - 1; j++)
                                {
                                    cdesc = cdesc + " " + revstring[j].ToString();
                                }
                            }
                            else
                            {
                                crev = "X";
                                cdesc = "XXXX";
                            }
                            //Helper.InfoMessageBox("Rev: " + crev + "\nDate: " + cdate + "\nArtist: " + artist + "\nDesc: " + cdesc);
                            DateTime dt = DateTimeConvert(cdate).ToUniversalTime();
                            NewRev.RevisionID = crev;
                            NewRev.RevisionLog = cdesc;
                            NewRev.RevisionTimestamp = dt.ToString();
                            NewRev.Drawn = artist;
                            NewRev.DrawnTimestamp = dt.ToString();
                            NewRev.Checked = "-";
                            NewRev.CheckedTimestamp = dt.ToString();
                            NewRev.Approved = "-";
                            NewRev.ApprovedTimestamp = dt.ToString();

                            clum.AddNewRevision(db, NewRev);
                            StartIndex = EndIndex + 1;

                        }

                    }

                }
                rResult = true;
                return rResult;
            }
            catch (System.Exception ex)
            {
                string ErrorString = "RevisionReader:Unexpected Error at end:" + doc.Name + ":" + ex;
                rResult = false;
                ed.WriteMessage(ex.ToString());
                return rResult;
                //WriteToFile(ErrorString);

            }

        }


        /// <summary>
        /// Converts the '25/12/75' date format to '25/12/1975' if the year component is greater than 50.
        /// Converts the '25/12/10' date format to '25/12/2010' if the year component is less than 50.
        /// </summary>
        /// <param name="cdate">The cdate.</param>
        /// <returns></returns>
        public DateTime DateTimeConvert(string cdate)
        {
            DateTime dt = new DateTime();
            try
            {
                const char slash = '/';
                const char dash = '-';
                const char stop = '.';
                char[] delimiters = new char[] { slash, dash, stop };
                String[] resultArray = cdate.Split(delimiters);
                if ((Convert.ToInt32(resultArray[2]) > 50))
                {
                    dt = new DateTime(Convert.ToInt32("19" + resultArray[2]), Convert.ToInt32(resultArray[1]), Convert.ToInt32(resultArray[0]));
                }
                else
                {
                    dt = new DateTime(Convert.ToInt32("20" + resultArray[2]), Convert.ToInt32(resultArray[1]), Convert.ToInt32(resultArray[0]));
                }
            }
            catch (System.Exception ex)
            {
                dt = DateTime.UtcNow;
                ed.WriteMessage(ex.ToString());
                return dt;
            }

            return dt;
        }


        public class DateTimeConverter : IValueConverter
        {
            public object Convert(object value,
                               Type targetType,
                               object parameter,
                               CultureInfo culture)
            {
                DateTime date = (DateTime)value;
                return date.ToShortDateString();
            }

            public object ConvertBack(object value,
                                      Type targetType,
                                      object parameter,
                                      CultureInfo culture)
            {
                string strValue = value.ToString();
                DateTime resultDateTime;
                if (DateTime.TryParse(strValue, out resultDateTime))
                {
                    return resultDateTime;
                }
                return value;
            }
        }



        /// <summary>
        /// Reads the attributes in the 'AmendText" revision block.
        /// It returns an array of strings to be processed into the new revision format.
        /// </summary>
        /// <param name="btrId">The BTR id.</param>
        /// <param name="blockName">Name of the block.</param>
        /// <param name="attbName">Name of the attb.</param>
        /// <returns></returns>
        public string[] GetRevAtts(ObjectId btrId, string blockName, string attbName)
        {
            //List<string> revAttString = new List<string>();
            string[] attbReadValue = new string[50];
            try
            {
                Document doc = Application.DocumentManager.MdiActiveDocument;
                Database db = doc.Database;
                Editor ed = doc.Editor;
                Transaction tr = doc.TransactionManager.StartTransaction();
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
                                if (bd.Name.ToUpper() == blockName.ToUpper())
                                {

                                    // Check each of the attributes...
                                    int Counter = 0;
                                    foreach (ObjectId arId in br.AttributeCollection)
                                    {
                                        DBObject obj = tr.GetObject(arId, OpenMode.ForRead);
                                        AttributeReference ar = obj as AttributeReference;
                                        if (ar != null)
                                        {
                                            // ... to see whether it has the tag we're after
                                            if ((ar.Tag.ToUpper() == attbName) && (ar.TextString != ""))
                                            {
                                                // If so, update the value
                                                attbReadValue[Counter] = ar.TextString;
                                                Counter++;
                                            }
                                        }
                                    }
                                    return attbReadValue;
                                }
                            }
                        }
                    }

                    tr.Commit();

                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage(ex.ToString());
            }

            return null;
        }

        /// <summary>
        /// Reads the attributes in the 'AmendText" revision block.
        /// It returns an array of strings to be processed into the new revision format.
        /// </summary>
        /// <param name="btrId">The BTR id.</param>
        /// <param name="blockName">Name of the block.</param>
        /// <param name="attbName">Name of the attb.</param>
        /// <returns></returns>
        public string[] GetRevAtts(Database db, ObjectId btrId, string blockName, string attbName)
        {
            //List<string> revAttString = new List<string>();
            string[] attbReadValue = new string[50];

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
                                if (bd.Name.ToUpper() == blockName.ToUpper())
                                {

                                    // Check each of the attributes...
                                    int Counter = 0;
                                    foreach (ObjectId arId in br.AttributeCollection)
                                    {
                                        DBObject obj = tr.GetObject(arId, OpenMode.ForRead);
                                        AttributeReference ar = obj as AttributeReference;
                                        if (ar != null)
                                        {
                                            // ... to see whether it has the tag we're after
                                            if ((ar.Tag.ToUpper() == attbName) && (ar.TextString != ""))
                                            {
                                                // If so, update the value
                                                attbReadValue[Counter] = ar.TextString;
                                                Counter++;
                                            }
                                        }
                                    }
                                    return attbReadValue;
                                }
                            }
                        }
                    }

                    tr.Commit();

                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage(ex.ToString());
            }
            return null;
        }

        //Add methods above here
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
