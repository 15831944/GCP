using System;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Windows;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Media.Imaging;
using System.Xml;

namespace AllocateCableNo
{
    public class Class1
    {

        [CommandMethod("Cables", CommandFlags.Session)]

        public static void AllocateCableNumbers()
        {
            Form1 ACForm = new Form1();

            try
            {                
                System.Windows.Forms.DialogResult acResult = ACForm.ShowDialog();

                if (acResult == System.Windows.Forms.DialogResult.Cancel)
                {
                    ACForm.Close();
                }
            }
            catch (System.Exception)
            {

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
                    CreateRibbon();
                }

            }
            public void CreateRibbon()
            {
                RibbonControl RibCtrl = Autodesk.Windows.ComponentManager.Ribbon;
                RibbonTab RibTab = null;
                RibTab = new RibbonTab();

                for (int j = 0; j < RibCtrl.Tabs.Count; j++)
                {
                    if (RibCtrl.Tabs[j].Title == "GPCL Electrical")
                    {
                        RibTab = RibCtrl.Tabs[j];
                        RibTab.Id = "GPCL_TAB01";
                        RibTab.KeyTip = "Key01";
                        break;
                    }
                }

                RibbonButton RibButton = new RibbonButton();
                RibButton.Text = "Allocate\nCable";
                RibButton.ShowText = true;
                RibButton.Id = "GPCL_08";
                RibButton.CommandParameter = "Cables ";
                RibButton.Description = "Use this button to allocate a new cable";
                RibButton.Image = Images.getBitmap(Properties.Resources.CableLarge);
                RibButton.LargeImage = Images.getBitmap(Properties.Resources.CableLarge);

                RibButton.ShowImage = true;
                RibButton.ShowText = true;
                RibButton.Size = RibbonItemSize.Large;
                RibButton.Orientation = System.Windows.Controls.Orientation.Vertical;
                RibButton.KeyTip = "Allocate Cable";
                RibButton.CommandHandler = new AdskCommandHandler();

                RibbonRowPanel RibRow = new RibbonRowPanel();
                RibRow.Items.Add(RibButton);

                RibbonPanelSource RibSource = new RibbonPanelSource();
                for (int m = 0; m < RibTab.Panels.Count; m++)
                {
                    if (RibTab.Panels[m].Source.Name == "GPCL Electric")
                    {
                        RibSource = RibTab.Panels[m].Source;
                        if (RibSource.Items.Count > 0)
                        {
                            RibSource.Items.Add(new RibbonRowBreak());
                        }
                        RibSource.Items.Add(RibRow);
                    }
                }


                if (RibTab.FindPanel("GPCL_Pan08") == null)
                {
                    RibSource.Title = "Allocate Cable";
                    RibSource.KeyTip = "Keytip1";
                    RibSource.Id = "GPCL_Pan08";
                    RibSource.Name = "Allocate Cable";
                    RibSource.Items.Add(RibRow);
                    RibbonPanel RibPanel = new RibbonPanel();
                    RibPanel.Source = RibSource;
                    RibTab.Panels.Add(RibPanel);
                }
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
