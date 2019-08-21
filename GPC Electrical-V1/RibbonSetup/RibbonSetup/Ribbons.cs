using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace RibbonSetup
{
    public class Ribbons
    {

    }

    public class ExtApp : IExtensionApplication
    {
        public static List<string> blockNames = new List<string>();

        public void Initialize()

        {

            if (Autodesk.Windows.ComponentManager.Ribbon == null)
            {
                //load the custom Ribbon on startup, but at this point
                //the Ribbon control is not available, so register for
                //an event and wait
                Autodesk.Windows.ComponentManager.ItemInitialized +=
                    new EventHandler<RibbonItemEventArgs>(ComponentManager_ItemInitialized);
            }
            else
            {
                //the assembly was loaded using NETLOAD, so the ribbon
                //is available and we just create the ribbon
                CreateRibbon();
            }

        }
        public void CreateRibbon()
        {
            RibbonControl RibCtrl = Autodesk.Windows.ComponentManager.Ribbon;
            RibbonTab RibTab = null;
            RibTab = new RibbonTab();
            RibTab.Name = "GPCL Electrical";
            RibTab.Title = "GPCL Electrical";
            RibTab.Id = "GPCL_TAB01";
            RibTab.KeyTip = "Key01";

            RibCtrl.Tabs.Add(RibTab);
            RibTab.IsActive = true;

        }

        void ComponentManager_ItemInitialized(object sender, RibbonItemEventArgs e)
        {
            //now one Ribbon item is initialized, but the Ribbon control
            //may not be available yet, so check if before
            if (Autodesk.Windows.ComponentManager.Ribbon != null)
            {
                //ok, create Ribbon

                CreateRibbon();
                //and remove the event handler
                Autodesk.Windows.ComponentManager.ItemInitialized -=
                    new EventHandler<RibbonItemEventArgs>(ComponentManager_ItemInitialized);
            }
        }

        public void Terminate()

        {

            // Assuming these events have fired, they have already
            // been removed
        }


        private System.Windows.Media.ImageSource GetIcon(string ico)

        {
            // We'll look for our icons in the folder of the assembly
            // (we could also use a resources, of course)


            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            // Check our .ico file exists

            string fileName = path + "\\" + ico;
            if (File.Exists(fileName))
            {

                // Get access to it via a stream
                Stream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                using (fs)
                {
                    // Decode the contents and return them
                    IconBitmapDecoder dec = new IconBitmapDecoder(fs, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    return dec.Frames[0];
                }
            }
            return null;
        }
        [CommandMethod("B2")]
        public void BlockBlock2()
        {
            try
            {
                Application.DocumentManager.MdiActiveDocument.CommandEnded += new CommandEventHandler(blkEdClosed);
                blockNames.Add("GPCLA2ATTRIBUTES");
                blockNames.Add("A3_BORDER");
                blockNames.Add("MetaData");

            }
            catch (System.Exception)
            {

            }

        }


        static void blkEdClosed(object sender, Autodesk.AutoCAD.ApplicationServices.CommandEventArgs e)
        {
            try
            {
                if (e.GlobalCommandName == "BCLOSE")
                {
                    Document doc = Application.DocumentManager.MdiActiveDocument;
                    Database db = doc.Database;

                    using (Transaction trx = db.TransactionManager.StartTransaction())
                    {
                        BlockTable bt = db.BlockTableId.GetObject(OpenMode.ForRead) as BlockTable;

                        foreach (string bName in blockNames)
                        {
                            if (bt.Has(bName))
                            {
                                ObjectId btrObjId = bt[bName].GetObject(OpenMode.ForRead).ObjectId;
                                BlockTableRecord btr = (BlockTableRecord)btrObjId.GetObject(OpenMode.ForRead);
                                if (btr.Explodable == true)
                                {
                                    btr.UpgradeOpen();
                                    btr.Explodable = false;
                                }

                            }
                        }

                        trx.Commit();
                    }
                }
            }
            catch (System.Exception)
            {

            }

        }


        static void BlockEditorOpen(object sender, Autodesk.AutoCAD.ApplicationServices.SystemVariableChangedEventArgs e)
        {
            try
            {
                if (e.Name == "BLOCKEDITOR" && e.Changed == true)
                {
                    if ((int)Application.GetSystemVariable("BLOCKEDITOR") == 0)
                    {
                        Document doc = Application.DocumentManager.MdiActiveDocument;
                        Database db = doc.Database;

                        using (Transaction trx = db.TransactionManager.StartTransaction())
                        {
                            BlockTable bt = db.BlockTableId.GetObject(OpenMode.ForRead) as BlockTable;

                            foreach (string bName in blockNames)
                            {
                                if (bt.Has(bName))
                                {
                                    ObjectId btrObjId = bt[bName].GetObject(OpenMode.ForRead).ObjectId;
                                    BlockTableRecord btr = (BlockTableRecord)btrObjId.GetObject(OpenMode.ForRead);
                                    if (btr.Explodable == true)
                                    {
                                        btr.UpgradeOpen();
                                        btr.Explodable = false;
                                    }

                                }
                            }

                            trx.Commit();
                        }
                    }

                }
            }
            catch (System.Exception)
            {

            }

        }

    }


}



