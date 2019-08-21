using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.Windows;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Windows.Media.Imaging;
using System.Xml;

#region Apache License
//// Configure log4net using the .config file
//[assembly: log4net.Config.XmlConfigurator(ConfigFile = @"C:\Datax\Assembly\cableFinder.dll.Config", Watch = true)]
//// This will cause log4net to look for a configuration file
//// called AttributeInsert.dll.Config in the Autocad .dll application folder
//// directory from which AutoCad 'netloads 'AttributeInsert.dll' )
#endregion

namespace cableFinder
{
    public class Class1
    {

        private static BlockDetails bd;


        /// <summary>
        /// Entry point of the text string extraction FSM.
        /// This FSM extracts the Title, current rev and registration from the border.
        /// Then extracts text strings from text objects, block attribd znd mline text.
        /// </summary>
        /// <param name="args">The args.</param>

        [CommandMethod("cf", CommandFlags.Session)]
        public static void GetAlltext(string[] args)
        {
            try
            {

                bd = new BlockDetails();
                bd.HasErrors = false;

                //bd.ErrorTextFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UpdateErrors.txt");
                //if (File.Exists(bd.ErrorTextFile))
                //{
                //    File.Delete(bd.ErrorTextFile);
                //}

                Banner_Form ds = new Banner_Form();

                System.Windows.Forms.DialogResult dsResult = ds.ShowDialog();

                if (dsResult == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        bd.NewCableTextFile = ds.NewDrawingsFile;
                        bd.Cable_Numbers = ds.CableNumbers;
                        bd.Drawings = new List<string>();
                        foreach (string Cableno in bd.Cable_Numbers)
                        {
                            bd.Drawings.Add(null);
                        }
                        bd.lbDwgs = ds.lbo;
                        bd.StartNo = 0;
                        if (bd.lbDwgs.Count > 0)
                        {
                            bd.DrawingFileName = (string)bd.lbDwgs[bd.StartNo];
                            bd.KeepRunning = true;
                        }
                        else
                        {
                            bd.KeepRunning = false;
                            bd.DrawingFileName = "";
                        }

                        ds.Close();
                    }
                    catch (System.Exception ex)
                    {
                        Helper.InfoMessageBox(ex.ToString());

                    }                   
                    bd.ChangeState(Idle.Instance);
                }
                else
                {
                    ds.Close();
                    bd.KeepRunning = false;
                    bd.ChangeState(Idle.Instance);

                }

                // Hint: Declare a bool eg: KeepRunning Set KeepRunning to true before the while loop 
                // and set it to false in the Exit method of your final IState implementation 
                // replacing while(true) with while([namespace].[class].KeepRunning)

                while (bd.KeepRunning)
                {
                    // Our thread of execution retrieves the CurrentState property and calls the Execute method of the returned state object
                    bd.CurrentState.Execute(bd);
                }
            }
            catch (System.Exception ex)
            {
                bd.DidItOk = false;
                bd.ErrorMessage = ex.ToString();
                bd.CriticalError = 3;
                bd.ChangeState(ErrorState.Instance);
                //log.Error("Did not even start the upgrade process.", ex);
            }
        }

        public class ExtApp : IExtensionApplication
        {

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
                    //CreateRibbon();
                }

            }
            //public void CreateRibbon()
            //{
            //    RibbonControl RibCtrl = Autodesk.Windows.ComponentManager.Ribbon;
            //    RibbonTab RibTab = null;
            //    RibTab = new RibbonTab();

            //    for (int j = 0; j < RibCtrl.Tabs.Count; j++)
            //    {
            //        if (RibCtrl.Tabs[j].Title == "GPCL Electric")
            //        {
            //            RibTab = RibCtrl.Tabs[j];
            //            RibTab.Id = "GPCL_TAB02";
            //            RibTab.KeyTip = "Key01";
            //            break;
            //        }
            //    }

            //    RibbonButton RibButton = new RibbonButton();
            //    RibButton.Text = "cableFinder";
            //    RibButton.ShowText = true;
            //    RibButton.Id = "GPCL_03";
            //    RibButton.CommandParameter = "cf ";
            //    RibButton.Description = "Use this button to add a banner to multiple drawings";
            //    RibButton.Image = Images.getBitmap(Properties.Resources.small);
            //    RibButton.LargeImage = Images.getBitmap(Properties.Resources.Large);

            //    RibButton.ShowImage = true;
            //    RibButton.ShowText = true;
            //    RibButton.Size = RibbonItemSize.Large;
            //    RibButton.Orientation = System.Windows.Controls.Orientation.Vertical;
            //    RibButton.KeyTip = "Add cableFinder";
            //    RibButton.CommandHandler = new AdskCommandHandler();

            //    RibbonRowPanel RibRow = new RibbonRowPanel();
            //    RibRow.Items.Add(RibButton);

            //    RibbonPanelSource RibSource = new RibbonPanelSource();
            //    for (int m = 0; m < RibTab.Panels.Count; m++)
            //    {
            //        if (RibTab.Panels[m].Source.Name == "GPCL Electric")
            //        {
            //            RibSource = RibTab.Panels[m].Source;
            //            if (RibSource.Items.Count > 0)
            //            {
            //                RibSource.Items.Add(new RibbonRowBreak());
            //            }
            //            RibSource.Items.Add(RibRow);
            //        }
            //    }


            //    if (RibTab.FindPanel("GPCL_Pan04") == null)
            //    {
            //        RibSource.Title = "cableFinder";
            //        RibSource.KeyTip = "Keytip1";
            //        RibSource.Id = "GPCL_Pan04";
            //        RibSource.Name = "cableFinder";
            //        RibSource.Items.Add(RibRow);
            //        RibbonPanel RibPanel = new RibbonPanel();
            //        RibPanel.Source = RibSource;
            //        RibTab.Panels.Add(RibPanel);
            //    }
            //}

            void ComponentManager_ItemInitialized(object sender, RibbonItemEventArgs e)
            {
                //now one Ribbon item is initialized, but the Ribbon control
                //may not be available yet, so check if before
                if (Autodesk.Windows.ComponentManager.Ribbon != null)
                {
                    //ok, create Ribbon

                    //CreateRibbon();
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

            public class Images
            {
                public static BitmapImage getBitmap(Bitmap image)
                {
                    MemoryStream stream = new MemoryStream();
                    image.Save(stream, ImageFormat.Png);
                    BitmapImage bmp = new BitmapImage();
                    bmp.BeginInit(); bmp.StreamSource = stream; bmp.EndInit();
                    return bmp;
                }
            }

        }

        public class AdskCommandHandler : System.Windows.Input.ICommand
        {
            public Boolean CanExecute(Object parameter)
            {
                return true;
            }
            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                RibbonButton ribBtn = parameter as RibbonButton;

                if (ribBtn != null)
                {
                    Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.SendStringToExecute(ribBtn.CommandParameter.ToString(), true, false, true);
                }
            }
        }



        // A class to fire commands to AutoCAD

        public class AutoCADCommandHandler : System.Windows.Input.ICommand
        {
            private string _command = "";

            public AutoCADCommandHandler(string cmd)
            {

                _command = cmd;

            }

#pragma warning disable 67

            public event EventHandler CanExecuteChanged;

#pragma warning restore 67

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                if (!String.IsNullOrEmpty(_command))
                {
                    Document doc = Application.DocumentManager.MdiActiveDocument;
                    doc.SendStringToExecute(_command + " ", false, false, false);
                }
            }
        }
    }
}

