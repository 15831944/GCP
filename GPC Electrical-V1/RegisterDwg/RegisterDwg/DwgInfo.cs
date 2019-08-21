using System;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Windows;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Windows.Forms;
using System.Xml.Linq;
using GPCLServiceCentral;
using Exception = System.Exception;
using System.Runtime.InteropServices;

namespace RegisterDwg
{

    public partial class DwgInfo : Form
    {
        string[,] FacilityCodes = new string[20, 5];
        string[,] UnitCodes = new string[300, 5];
        string[,] SelectedUnitCodes = new string[300, 5];
        string[,] DrawingCodes = new string[20, 5];
        string SelectedBorder = "";

        public class ReturnedRegistrationAttributes
        {
            public XmlNode DwgInfoNode { get; set; }
            public string DrawingNumber { get; set; }
            public string Facility { get; set; }
            public string Unit { get; set; }
            public string Equipment { get; set; }
            public string Device { get; set; }
            public string DrawingType { get; set; }
            public string RegistrationNumber { get; set; }
        }

        public class RegistrationAttributes
        {
            public string Prefix { get; set; }
            public int Facility { get; set; }
            public int Unit { get; set; }
            public string Equipment { get; set; }
            public string Device { get; set; }
            public int DrawingType { get; set; }
            public string RegistrationNumber { get; set; }
        }

        public class Facility
        {
            public XmlNode FacilityNode { get; set; }
            public Int32 FacilityID { get; set; }
            public string FacilityName { get; set; }
            public string FacilityChar0 { get; set; }
            public Int32 FacilityRegistrationArea { get; set; }
        }

        public class Unit
        {
            public XmlNode UnitNode { get; set; }
            public Int32 UnitID { get; set; }
            public string UnitName { get; set; }
            public string RegistrationAbbrev { get; set; }
            public Int32 FacilityRegistrationArea { get; set; }
        }

        public class DrawingType
        {
            public XmlNode DrawingTypeNode { get; set; }
            public Int32 DrawingTypeID { get; set; }
            public string DrawingTypeName { get; set; }
            public string DrawingTypeAbbrev { get; set; }
        }

        public class BorderDetails
        {
            public string BorderName { get; set; }
            public Scale3d BorderScale { get; set; }
            public double BorderRotation { get; set; }
            public Point3d BorderInsertionPoint { get; set; }
            public Boolean DoneOk { get; set; }
            public string NewBorderFilename { get; set; }
            public string ErrorMessage { get; set; }
        }

        public string SelectedBorderName;

        public List<Facility> Facilities = new List<Facility>();

        public List<Unit> Units = new List<Unit>();

        public List<Unit> SelectedUnits = new List<Unit>();

        public List<DrawingType> DrawingTypes = new List<DrawingType>();

        public string BorderScale;

        public Boolean Ready;

        //public DateTime Date; //Coles Counter

        public DwgInfo()
        {
            InitializeComponent();

            string settingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Setting.ini";
            if (!File.Exists(settingPath))
            {
                MessageBox.Show("Setting.ini data does not exist..!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                //System.Diagnostics.Debugger.Launch();
                Ready = false;
                RegisterButton.Enabled = false;
                cadservices.CadARX service = new cadservices.CadARX();
                var MyIni = new IniFile("Setting.ini");
                service.Url = MyIni.Read("RegisterDwg_cadservices_CadARX", "RegisterDwg"); //http://cadservices/cadarx/CadARX.asmx


                Facility f = new Facility();

                service.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;

                try
                {
                    f.FacilityNode = service.ShowCadFacility();
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Cannot connect to " + service.Url + Environment.NewLine + ex.Message);
                    return;
                }

                XmlNodeList FacilityList = f.FacilityNode.SelectNodes("Record");
                for (int i = 0; i < FacilityList.Count; i++)
                {
                    XmlNode recnode = FacilityList[i];

                    //Add all of  the facility objects to the 'Facilities' list
                    Facilities.Add(new DwgInfo.Facility
                    {
                        FacilityID = Convert.ToInt32(recnode.SelectSingleNode("ID").InnerText),
                        FacilityName = recnode.SelectSingleNode("Facility").InnerText,
                        FacilityChar0 = recnode.SelectSingleNode("RegistrationChar0").InnerText,
                        FacilityRegistrationArea = Convert.ToInt32(recnode.SelectSingleNode("RegistrationArea").InnerText),
                    });

                    //Add the facility names from the added object to the combobox list
                    Title1ListBox.Items.Add(Facilities[i].FacilityName);

                }

                //Create unit list
                Unit u = new Unit();
                try
                {
                    u.UnitNode = service.ShowCadUnit();
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Cannot connect to " + service.Url + Environment.NewLine + ex.Message);
                    return;
                }

                XmlNodeList UnitList = u.UnitNode.SelectNodes("Record");

                for (int i = 0; i < UnitList.Count; i++)
                {
                    XmlNode recnode = UnitList[i];
                    Units.Add(new Unit
                    {
                        UnitID = Convert.ToInt32(recnode.SelectSingleNode("ID").InnerText),
                        UnitName = recnode.SelectSingleNode("Unit").InnerText,
                        RegistrationAbbrev = recnode.SelectSingleNode("UnitRegistrationAbbrev").InnerText,
                        FacilityRegistrationArea = Convert.ToInt32(recnode.SelectSingleNode("RegistrationArea").InnerText)
                    });
                }


                DrawingType d = new DrawingType();
                try
                {
                    d.DrawingTypeNode = service.ShowCadDrawingType();
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Cannot connect to " + service.Url + Environment.NewLine + ex.Message);
                    return;
                }

                XmlNodeList DwgTypeList = d.DrawingTypeNode.SelectNodes("Record");

                for (int i = 0; i < DwgTypeList.Count; i++)
                {
                    XmlNodeList DwgTypeRecordNodes = DwgTypeList[i].ChildNodes;

                    XmlNode recnode = DwgTypeList[i];
                    DrawingTypes.Add(new DwgInfo.DrawingType
                    {
                        DrawingTypeID = Convert.ToInt32(recnode.SelectSingleNode("ID").InnerText),
                        DrawingTypeName = recnode.SelectSingleNode("DrawingType").InnerText,
                        DrawingTypeAbbrev = recnode.SelectSingleNode("Abbrev").InnerText
                    });

                    Title5ListBox.Items.Add(DrawingTypes[i].DrawingTypeName);
                }

                Title5ListBox.SelectedIndex = 0;
                Title1ListBox.SelectedIndex = 4;
                ScaleComboBox.SelectedIndex = 0;
                BorderComboBox.SelectedIndex = 0;
                if (Facilities[Title1ListBox.SelectedIndex].FacilityChar0 == "C")
                {
                    DwgPrefix.Text = "107-";
                }
                Ready = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }


        }
        //This is where the drawing info is returned and transferred to the border

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            string settingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Setting.ini";
            if (!File.Exists(settingPath))
            {
                MessageBox.Show("Setting.ini data does not exist..!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                //
                //Coles Code to Save and Create a new drawing for the border to be added to
                //
                SaveActiveDrawing(); //saves the current drawing
                NewDrawing(); //Create a new drawing

                AcadUtils.DwgUtils Utils = new AcadUtils.DwgUtils();
                Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                Database db = doc.Database;
                ObjectId msId = Utils.GetMSpaceID(db);
                cadservices.CadARX RegisterService = new cadservices.CadARX();
                var MyIni = new IniFile("Setting.ini");
                RegisterService.Url = MyIni.Read("RegisterDwg_cadservices_CadARX", "RegisterDwg"); //http://cadservices/cadarx/CadARX.asmx
                RegisterService.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;


                BorderDetails bd = new BorderDetails();
                bd.NewBorderFilename = SelectedBorder;
                bd.BorderName = SelectedBorderName;
                bd.BorderRotation = 0;
                bd.BorderInsertionPoint = new Point3d(0, 0, 0);

                if (ScaleComboBox.SelectedItem.ToString() == "NTS")
                {
                    bd.BorderScale = new Scale3d(1, 1, 1);
                    BorderScale = "NTS";
                }
                else
                {
                    bd.BorderScale = new Scale3d(Convert.ToInt64(ScaleComboBox.SelectedItem.ToString()));
                    BorderScale = "1 : " + ScaleComboBox.SelectedItem.ToString();
                }

                RegistrationAttributes ra = new RegistrationAttributes();

                //We will get a drawing number returned
                ra.Prefix = DwgPrefix.Text;

                ra.Facility = Facilities[Title1ListBox.SelectedIndex].FacilityID;
                ra.Unit = SelectedUnits[Title2ListBox.SelectedIndex].UnitID;
                ra.Equipment = Title3TextBox.Text;
                ra.Device = Title4TextBox.Text;
                ra.DrawingType = DrawingTypes[Title5ListBox.SelectedIndex].DrawingTypeID;
                ra.RegistrationNumber = "";

                ReturnedRegistrationAttributes rra = new ReturnedRegistrationAttributes();
                try
                {
                    rra.DwgInfoNode = RegisterService.CreateDrawing(ra.Prefix, ra.Facility, ra.Unit, ra.Equipment, ra.Device, ra.DrawingType);

                    // remember the xml returned maybe a newly created drawing or details of errors in your parameters ...ie Prefix, Facility etc etc

                }
                catch (System.Exception ex)
                {
                    // if we got an exception calling across the wire to a remote service then it's a waste of time going on.
                    // notfy user and return...

                    MessageBox.Show("Problem calling CreateDrawing Method on Service: " + ex.Message);
                    return;   //no use going on we have no data
                }

                // going on but have not verified that we have a new drawing or details of error in our xml    

                //MessageBox.Show(rra.DwgInfoNode.InnerXml);

                XmlNode RecordNode = null;
                if (rra.DwgInfoNode != null) // check if object returned from service call is not null before calling methods on it
                {


                    //assigments of expected values in an xml fragment are potential failure spots, so try catch is appropriate
                    try
                    {
                        RecordNode = rra.DwgInfoNode.SelectSingleNode("Record"); // try to retrieve the XML element called Record from the data returned from the service call and assign it to RecordNode
                    }
                    catch (System.Exception)
                    {
                        MessageBox.Show("No Record element in xml returned from CreateDrawing");
                        return;  // notice I have checked for the existence of a Record element, currently when it isn't there, we just exit...good practice would be to see if there is an Error element so we could show the user the error if it existed..
                    }

                }
                else
                {

                    // null object returned from call... no use going on tell user and return
                    MessageBox.Show("Null Object returned from call to CreateDrawing Method on CadArx Service");
                    return;
                }

                // RecordNode should exist at this time.. good practice dictates that we check it for null before consuming
                if (RecordNode != null)
                {
                    // problematic at the moment because we are assuming that the piece of xml actually contains the Drawing Number, it well may contain an Error Element only

                    try
                    {
                        rra.DrawingNumber = RecordNode.SelectSingleNode("DrawingNo").InnerText;
                        rra.Facility = rra.DwgInfoNode.SelectSingleNode("Record").SelectSingleNode("Facility").InnerText;
                        rra.Unit = rra.DwgInfoNode.SelectSingleNode("Record").SelectSingleNode("Unit").InnerText;
                        rra.Equipment = rra.DwgInfoNode.SelectSingleNode("Record").SelectSingleNode("Equipment").InnerText;
                        rra.Device = rra.DwgInfoNode.SelectSingleNode("Record").SelectSingleNode("Device").InnerText;
                        rra.DrawingType = rra.DwgInfoNode.SelectSingleNode("Record").SelectSingleNode("DrawingType").InnerText;
                        rra.RegistrationNumber = rra.DwgInfoNode.SelectSingleNode("Record").SelectSingleNode("Registration").InnerText;
                    }
                    catch (System.Exception ex)
                    {

                        MessageBox.Show("Problem assigning values from xml to ReturnedRegistrationAttributes type : " + ex.Message);
                        return;
                    }
                    string Artist = "";

                    try
                    {
                        //If the check box is not seleted don't Add Authors Name
                        if (ckbArtistName.Checked == false)
                            Artist = "";
                        else
                        {
                            string FirstName = System.DirectoryServices.AccountManagement.UserPrincipal.Current.GivenName;
                            char FirstIntial = FirstName[0];

                            string LastName = System.DirectoryServices.AccountManagement.UserPrincipal.Current.Surname;

                            Artist = (FirstIntial + ". " + LastName).ToUpper();
                        }
                    }
                    catch (System.Exception)
                    {

                        Artist = "";
                    }

                    DocumentLock dl = doc.LockDocument(DocumentLockMode.ProtectedAutoWrite, null, null, true);
                    bd = InsertDrawingBorder(bd);

                    using (dl)
                    {
                        string AttributeBlock = Utils.GetConstantAttribute(db, SelectedBorderName, "BORDERATTBLOCK");
                        if (SelectedBorderName == Utils.GetConstantAttribute(db, SelectedBorderName, "BORDERNAME"))
                        {

                            Utils.UpdateAttributesInBlock(msId, AttributeBlock, "DWGNO", rra.DrawingNumber);
                            Utils.UpdateAttributesInBlock(msId, AttributeBlock, "TITLE1", rra.Facility);
                            Utils.UpdateAttributesInBlock(msId, AttributeBlock, "TITLE2", rra.Unit);
                            Utils.UpdateAttributesInBlock(msId, AttributeBlock, "TITLE3", rra.Equipment);
                            Utils.UpdateAttributesInBlock(msId, AttributeBlock, "TITLE4", rra.Device);
                            Utils.UpdateAttributesInBlock(msId, AttributeBlock, "TITLE5", rra.DrawingType);
                            Utils.UpdateAttributesInBlock(msId, AttributeBlock, "CADNO", rra.RegistrationNumber);
                            Utils.UpdateAttributesInBlock(msId, AttributeBlock, "DWGSCALE", BorderScale);
                            Utils.UpdateAttributesInBlock(msId, AttributeBlock, "ARTIST", Artist);
                            //Add the Current Date to the Title Block
                            Utils.UpdateAttributesInBlock(msId, AttributeBlock, "DWGDATE", DateTime.Now.ToString("dd/MM/yyyy"));
                            //Add the Current Revision of A to the Title Block
                            Utils.UpdateAttributesInBlock(msId, AttributeBlock, "DBREV", "A");


                            //Boolean InsertMdataResult = InsertMetaDataBlock();
                            //CadLogixUtil.Meta data = new CadLogixUtil.Meta();

                            //data.WriteMetaDataBorder(Utils.GetConstantAttribute(db, SelectedBorder, "BORDERNAME"));
                            //data.WriteMetaDataBorderAttributes(Utils.GetConstantAttribute(db, SelectedBorder, "BORDERATTBLOCK"));
                            //data.WriteMetaDataClass(Convert.ToInt32(Utils.GetConstantAttribute(db, SelectedBorder, "BORDERCLASS")));
                            //data.WriteMetaDataRevision(Convert.ToDouble(Utils.GetConstantAttribute(db, SelectedBorder, "BORDERCLASSREV")));
                        }
                        else
                        {
                            MessageBox.Show("The selected Border and the current border are not the same!");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("A valid drawing has not been registered because: " + RecordNode.SelectSingleNode("Error").InnerText);
                }
                SaveToVault((Convert.ToString(DwgPrefix.Text) + Convert.ToString(DwgNo.Text) + ".dwg")); //Coles Code to Save to Vault DIR
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

        }

        public void SaveToVault(string DrawingNo)
        {
            string settingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Setting.ini";
            if (!File.Exists(settingPath))
            {
                MessageBox.Show("Setting.ini data does not exist..!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                Document acDoc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;

                var MyIni = new IniFile("Setting.ini");
                string docPath = MyIni.Read("RegisterDwgLocalVaultFileLocation", "RegisterDwg"); //"C:\\Vault\\Designs\\ElectricalDrawings\\"

                string SaveName = docPath + "\\" + DrawingNo; //Save Location in Local Vault
                acDoc.Database.SaveAs(SaveName, true, DwgVersion.Current, acDoc.Database.SecurityParameters); //Actually Save the File to the vault location
            }
            catch (System.Exception)
            {
                MessageBox.Show("Vault Save Failed");
            }
        }
        public void SaveActiveDrawing()
        {
            string settingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Setting.ini";
            if (!File.Exists(settingPath))
            {
                MessageBox.Show("Setting.ini data does not exist..!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                Document acDoc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
                string strDWGName = acDoc.Name;
                object obj = Autodesk.AutoCAD.ApplicationServices.Core.Application.GetSystemVariable("DWGTITLED");

                //Check to see if the Drawing has been Named

                if (System.Convert.ToInt16(obj) == 0)
                {
                    //if Drawing is using default name provide a new name

                    var MyIni = new IniFile("Setting.ini");
                    strDWGName = MyIni.Read("RegisterDwgActiveDrawingPath", "RegisterDwg"); // "c:\\Vault\\Drawing.dwg";  //  +Convert.ToString(DateTime.Now); To be added after test
                }

                // Save the Drawing

                acDoc.Database.SaveAs(strDWGName, true, DwgVersion.Current, acDoc.Database.SecurityParameters);
            }
            catch (SystemException e)
            {
                MessageBox.Show("Failed to Save Active Drawing " + e.Message);
                return;
            }

        }

        private void Title1ListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            string settingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Setting.ini";
            if (!File.Exists(settingPath))
            {
                MessageBox.Show("Setting.ini data does not exist..!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                cadservices.CadARX CheckNextDwgNo = new cadservices.CadARX();
                var MyIni = new IniFile("Setting.ini");
                CheckNextDwgNo.Url = MyIni.Read("RegisterDwg_cadservices_CadARX", "RegisterDwg"); //http://cadservices/cadarx/CadARX.asmx
                CheckNextDwgNo.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                RegoFacility.Text = Facilities[Title1ListBox.SelectedIndex].FacilityChar0;

                //switch (Facilities[Title1ListBox.SelectedIndex].FacilityChar0)
                //{
                //    case "C":
                //        DwgPrefix.Text = "107-";
                //        break;

                //    case "B":
                //        DwgPrefix.Text = "517-";
                //        break;

                //    case "A":
                //        DwgPrefix.Text = "016-";
                //        break;

                //    case "M":
                //        DwgPrefix.Text = "205-";
                //        break;

                //    case "O":
                //        DwgPrefix.Text = "807-";
                //        break;

                //    default:
                //        DwgPrefix.Text = null;
                //        DwgNo.Text = null;
                //        break;
                //}

                try
                {
                    XmlNode CADPrefixList = CheckNextDwgNo.ShowCadPrefix();
                    var xDoc = XDocument.Parse(CADPrefixList.OuterXml);

                    try
                    {
                        DwgPrefix.Text = xDoc.Element("CadPrefix")
                        .Elements("Record")
                        .FirstOrDefault(d => d.Element("CadFacilityID").Value == Facilities[Title1ListBox.SelectedIndex].FacilityID.ToString()).Element("Prefix").Value;
                    }
                    catch (NullReferenceException)
                    {
                        DwgPrefix.Text = null;
                        DwgNo.Text = null;

                    }

                    if (!string.IsNullOrEmpty(DwgPrefix.Text))
                    {
                        XmlNode NextDwgNode = CheckNextDwgNo.CheckNextDrawing(DwgPrefix.Text).SelectSingleNode("Record");
                        DwgNo.Text = NextDwgNode.SelectSingleNode("Number").InnerText;
                    }

                    SelectedUnits.Clear();

                    //MessageBox.Show(Title1ListBox.SelectedIndex.ToString() + ":" + Facilities[Title1ListBox.SelectedIndex].FacilityRegistrationArea);

                    for (int i = 0; i < Units.Count(); i++)
                    {
                        if (Units[i].FacilityRegistrationArea == Facilities[Title1ListBox.SelectedIndex].FacilityRegistrationArea)
                        {
                            SelectedUnits.Add(Units[i]);
                        }
                    }

                    Title2ListBox.Items.Clear();

                    for (int j = 0; j < SelectedUnits.Count(); j++)
                    {
                        Title2ListBox.Items.Add(SelectedUnits[j].UnitName);
                    }

                    if (Title2ListBox.Items.Count > 0)
                    {
                        Title2ListBox.SelectedIndex = 0;
                    }
                    else
                    {
                        RegoUnit.Text = "";
                    }


                    RegoFind(); //Run the find Rego script
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Cannot connect to " + CheckNextDwgNo.Url + Environment.NewLine + ex.Message);
                    return;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return;
            }


        }
        void RegoFind()
        {
            string settingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Setting.ini";
            if (!File.Exists(settingPath))
            {
                MessageBox.Show("Setting.ini data does not exist..!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //
            //Check if all the Rego values are populated prior to exe GetNextRego
            // Cole Fallon Code
            //


            if (Ready == true)
            {
                RegisterButton.Enabled = true;
                cadservices.CadARX CheckNextRego = new cadservices.CadARX();
                var MyIni = new IniFile("Setting.ini");
                CheckNextRego.Url = MyIni.Read("RegisterDwg_cadservices_CadARX", "RegisterDwg"); //http://cadservices/cadarx/CadARX.asmx
                CheckNextRego.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;


                try
                {
                    if ((RegoFacility.Text != null) && (RegoUnit.Text != null) && (RegoDwg.Text != null))
                    {
                        try
                        {
                            XmlNode NextRegoNode = CheckNextRego.CheckNextRegistration(RegoFacility.Text, RegoUnit.Text, RegoDwg.Text).SelectSingleNode("Record");
                            RegoNo.Text = NextRegoNode.SelectSingleNode("Number").InnerText;
                        }
                        catch (Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show("Cannot connect to " + CheckNextRego.Url + Environment.NewLine + ex.Message);
                            return;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Coles Code Failed to Run CheckNextRegistration " + ex.Message);
                    ActiveForm.Close();
                }

            }
        }

        private void Title5ListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                RegoDwg.Text = DrawingTypes[Title5ListBox.SelectedIndex].DrawingTypeAbbrev;

                RegoFind(); //Run the find Rego script
            }
            catch (Exception)
            {

            }

        }

        private void Title2ListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (Title2ListBox.Items.Count > 0)
                {
                    RegoUnit.Text = SelectedUnits[Title2ListBox.SelectedIndex].RegistrationAbbrev;
                    RegoFind(); //Run the find Rego script //Is there a problem with enabling this
                }
            }
            catch (Exception)
            {

            }

        }



        private void BorderComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string settingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Setting.ini";
            if (!File.Exists(settingPath))
            {
                MessageBox.Show("Setting.ini data does not exist..!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var MyIni = new IniFile("Setting.ini");


                if (BorderComboBox.SelectedIndex == 0)
                {
                    SelectedBorder = MyIni.Read("RegisterDwgBorderTemplateA3", "RegisterDwg");  // @"\\gldfs04\CAD\ACAD\AcadR2004\GPAELECTLIB\GPAEA3B.dwg";
                    SelectedBorderName = "GPAEA3B";
                }
                else if (BorderComboBox.SelectedIndex == 1)
                {
                    SelectedBorder = MyIni.Read("RegisterDwgBorderTemplateA2", "RegisterDwg");// @"\\gldfs04\CAD\ACAD\AcadR2004\GPAELECTLIB\GPAEA2B.dwg";
                    SelectedBorderName = "GPAEA2B";
                }
                else
                {
                    SelectedBorder = MyIni.Read("RegisterDwgBorderTemplateA1", "RegisterDwg");// @"\\gldfs04\CAD\ACAD\AcadR2004\GPAELECTLIB\GPAEA1B.dwg";
                    SelectedBorderName = "GPAEA1B";
                }
            }
            catch (Exception)
            {

            }

        }

        /// Inserts the meta data block with attributes.
        /// </summary>
        //[CommandMethod("IMB")]

        public Boolean InsertMetaDataBlock()
        {
            Boolean InsertResult = false;

            try
            {
                AcadUtils.DwgUtils Utils = new AcadUtils.DwgUtils();
                Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                Database db = doc.Database;
                Editor ed = doc.Editor;
                Transaction Trans = db.TransactionManager.StartTransaction();
                ObjectId msId = Utils.GetMSpaceID(db);
                ObjectId NewBlkId;
                string DwgBorder = string.Empty;

                CadLogixUtil.Meta data = new CadLogixUtil.Meta();
                DwgBorder = SelectedBorder;
                if (DwgBorder == string.Empty)
                {
                    return InsertResult;
                }
                else
                {

                    string blkName = "METADATA";
                    int MetaDataBlockCount = Utils.BlockCount(blkName);

                    if (MetaDataBlockCount != 0)
                    {
                        InsertResult = false;
                        return InsertResult;
                    }
                    else
                    {
                        BlockTable bt = (BlockTable)(Trans.GetObject(db.BlockTableId, OpenMode.ForRead));

                        BlockTableRecord btr = (BlockTableRecord)Trans.GetObject(msId, OpenMode.ForWrite);

                        //Create the block reference...use the return from CreateMetaDataBlockDefinition directly!
                        NewBlkId = data.CreateMetaDataDefinition();


                        BlockReference br = new BlockReference(new Point3d(0, -5, 0), NewBlkId);
                        btr.AppendEntity(br); //Add the reference to ModelSpace
                        Trans.AddNewlyCreatedDBObject(br, true); //Let the transaction know about it

                        BlockTableRecord BlkTblRec = Trans.GetObject(NewBlkId, OpenMode.ForRead) as BlockTableRecord;
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

                        Trans.Commit(); // Commit is always required to indicate success.
                        InsertResult = true;

                    }
                    Trans.Dispose();

                }
            }
            catch (Exception)
            {

            }
            return InsertResult;
        }



        /// <summary>
        /// Downloads the nominated drawing border from the CAD server by calling the 'DownloadBorder' webservice
        /// It is saved to a temporary location where it is then inserted into the drawing.
        /// </summary>
        /// <param name="bd">The bd.</param>
        /// <returns></returns>
        public BorderDetails DownLoadBorder(BorderDetails bd)
        {
            //Get border from Server
            try
            {
                GPCLServiceCentral.PersistObject po = new GPCLServiceCentral.PersistObject();
                GPCLServiceCentral.ServiceStation Service = new GPCLServiceCentral.ServiceStation();
                po.DwgFileName = bd.BorderName;
                po.FullPath = @"C:\KKTemp";
                po = Service.DownloadBorder(po);
                if (po.Result)
                {
                    bd.NewBorderFilename = System.IO.Path.Combine(po.FullPath, po.DwgFileName) + ".dwg";
                    bd.DoneOk = true;
                    bd.ErrorMessage = "Border downloaded OK";
                    return bd;
                }
                else
                {
                    bd.DoneOk = false;
                    bd.ErrorMessage = "Failed to download drawing border from server";
                    return bd;
                }
            }
            catch (System.Exception ex)
            {
                bd.DoneOk = false;
                bd.ErrorMessage = "Failed to download drawing border from server\n " + ex.ToString();
                return bd;
            }
        }


        /// <summary>
        /// Inserts the nominated drawing border into the current active document.
        /// </summary>
        /// <param name="BkDetails">The bk details.</param>
        /// <returns></returns>
        public BorderDetails InsertDrawingBorder(BorderDetails BkDetails)
        {
            BkDetails.DoneOk = false;

            try
            {
                Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                Database db = doc.Database;
                Editor ed = doc.Editor;
                Transaction Trans = db.TransactionManager.StartTransaction();
                AcadUtils.DwgUtils Utils = new AcadUtils.DwgUtils();

                ObjectId msId = Utils.GetMSpaceID(db);

                ComboBox.ObjectCollection co = ScaleComboBox.Items;

                //ObjectId BlockExistID = CountBorderBlock(BkDetails.BorderName);
                //ObjectId BlockExistID = msId;
                //if (BlockExistID == ObjectId.Null)
                //{

                Database Newdb = new Database(false, true);
                using (Newdb)
                {
                    //Read the .dwg file into a new temporary drawing database
                    Newdb.ReadDwgFile(BkDetails.NewBorderFilename, System.IO.FileShare.Read, false, "");

                    //Insert the temporary database record into the active document database
                    //with the name held by the 'BkDetails.BorderName' string variable. This
                    //creates a block definition.
                    ObjectId BlockID = db.Insert(BkDetails.BorderName, Newdb, false);

                    //BlockTable bt = (BlockTable)(Trans.GetObject(db.BlockTableId, OpenMode.ForRead));

                    //Open the ModelSpace BlockTableRecord (BTR), create a new block reference from the 
                    //block definition and append it to the BTR. Effectively creating a visible object instance
                    //of the block definition.
                    BlockTableRecord btr = (BlockTableRecord)Trans.GetObject(msId, OpenMode.ForWrite);
                    BlockReference br = new BlockReference(BkDetails.BorderInsertionPoint, BlockID);
                    btr.AppendEntity(br);
                    //HyperLink hl = new HyperLink();
                    //hl.Description = "Cable";
                    //hl.Name = "http://rgtctweb/scripts/electrical/getcable.idc?TheCableID=CD2679";
                    //br.Hyperlinks.Add(hl);

                    //Complete the persistence data, the rotation and scale.
                    br.Rotation = BkDetails.BorderRotation;
                    br.ScaleFactors = BkDetails.BorderScale;
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
                //}
                //else
                //{
                //    BkDetails.ErrorMessage = "Block " + BkDetails.BorderName + " already exists";
                //}
            }
            catch (System.Exception ex)
            {
                BkDetails.ErrorMessage = ex.ToString();

            }
            return BkDetails;
        }


        public string GetNextRego(string NextRego)
        {
            string settingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Setting.ini";
            if (!File.Exists(settingPath))
            {
                MessageBox.Show("Setting.ini data does not exist..!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
            try
            {
                cadservices.CadARX CheckNextRego = new cadservices.CadARX();
                var MyIni = new IniFile("Setting.ini");
                CheckNextRego.Url = MyIni.Read("RegisterDwg_cadservices_CadARX", "RegisterDwg"); //http://cadservices/cadarx/CadARX.asmx
                CheckNextRego.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;

                try
                {
                    XmlNode NextRegoNode = CheckNextRego.CheckNextDrawing(DwgPrefix.Text).SelectSingleNode("Record");
                    NextRego = NextRegoNode.SelectSingleNode("Number").InnerText;
                    return NextRego;
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Cannot connect to " + CheckNextRego.Url + Environment.NewLine + ex.Message);
                    return "";
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return "";
            }


        }

        private void NewDrawing()
        {
            string settingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Setting.ini";
            if (!File.Exists(settingPath))
            {
                MessageBox.Show("Setting.ini data does not exist..!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Specify the template to use, if the template is not found
            // the default settings are used.
            try
            {
                var MyIni = new IniFile("Setting.ini");
                string strTemplatePath = MyIni.Read("RegisterDwgNewTemplate", "RegisterDwg");// "\\\\gldfs04\\CAD\\gpa electrical application\\template\\gpa a3 acad.dwt";

                DocumentCollection acDocMgr = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager;
                Document acDoc = acDocMgr.Add(strTemplatePath);
                acDocMgr.MdiActiveDocument = acDoc;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);

            }

        }

        private void btnBulkReg_Click(object sender, EventArgs e)
        {

        }

        private void btnOpenCSV_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Displays an OpenFileDialog so the user can select a CSV or PCD point Cloud File.
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "CSV Files|*.csv";
                //openFileDialog1.Filter = "PCD Files|*.PCD";
                openFileDialog1.Title = "Select a CSV Registration File";
                openFileDialog1.ShowDialog();
                txtBulkPath.Text = openFileDialog1.FileName;
                //string CSVName = openFileDialog1.SafeFileName;
            }
            catch (Exception)
            {

            }

        }

        private void btnBulkReg_Click_1(object sender, EventArgs e)
        {
            string settingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Setting.ini";
            if (!File.Exists(settingPath))
            {
                MessageBox.Show("Setting.ini data does not exist..!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {


                StreamReader csvRead = new StreamReader(txtBulkPath.Text);
                StreamWriter csvWrite = new StreamWriter(txtBulkPath.Text + ".Results.csv");
                //MessageBox.Show("Opening Stream");
                while (!csvRead.EndOfStream)
                {

                    //SaveActiveDrawing();
                    string read = csvRead.ReadLine();
                    string[] string1 = read.Split(',');
                    //string DrawingNumber = string1[0];
                    //string RegoNumber = string1[1];
                    string Title1 = string1[0];
                    string Title2 = string1[1];
                    string Title3 = string1[2];
                    string Title4 = string1[3];
                    string Title5 = string1[4];
                    string Size = string1[5];
                    string Scale = string1[6];

                    //MessageBox.Show("read the stream " + Title1 + Title2 + Title3 + Title4 + Title5 + Size + Scale);
                    RegistrationAttributes ra = new RegistrationAttributes();
                    if (Size == "A3")
                    {
                        var MyIni1 = new IniFile("Setting.ini");
                        SelectedBorder = MyIni1.Read("RegisterDwgBorderTemplateA3"); //@"\\gldfs04\CAD\ACAD\AcadR2004\GPAELECTLIB\GPAEA3B.dwg";
                        SelectedBorderName = "GPAEA3B";
                    }
                    else if (Size == "A2")
                    {
                        var MyIni2 = new IniFile("Setting.ini");
                        SelectedBorder = MyIni2.Read("RegisterDwgBorderTemplateA2");  //@"\\gldfs04\CAD\ACAD\AcadR2004\GPAELECTLIB\GPAEA2B.dwg";
                        SelectedBorderName = "GPAEA2B";
                    }
                    else if (Size == "A1")
                    {
                        var MyIni3 = new IniFile("Setting.ini");
                        SelectedBorder = MyIni3.Read("RegisterDwgBorderTemplateA1");  //@"\\gldfs04\CAD\ACAD\AcadR2004\GPAELECTLIB\GPAEA1B.dwg";
                        SelectedBorderName = "GPAEA1B";
                    }
                    else
                    {
                        MessageBox.Show("Error in Border sizing");
                        continue;
                    }

                    //MessageBox.Show("Border is sized" + SelectedBorderName);

                    //MessageBox.Show(Title1);
                    for (int i = 0; i < Facilities.Count; i++)
                    {
                        if (Facilities[i].FacilityName.ToUpper().Contains(Title1.ToUpper()))
                        {
                            ra.Facility = Facilities[i].FacilityID;
                            //MessageBox.Show(Convert.ToString(ra.Facility));
                            switch (Facilities[i].FacilityChar0)
                            {
                                case "C":
                                    ra.Prefix = "107-";
                                    break;

                                case "B":
                                    ra.Prefix = "517-";
                                    break;

                                case "A":
                                    ra.Prefix = "016-";
                                    break;

                                case "M":
                                    ra.Prefix = "205-";
                                    break;

                                case "O":
                                    ra.Prefix = "807-";
                                    break;

                                default:
                                    DwgPrefix.Text = null;
                                    DwgNo.Text = null;
                                    break;
                            }
                        }
                    }

                    //MessageBox.Show("Prefix Number = " + ra.Prefix);

                    for (int i = 0; i < Units.Count; i++)
                    {

                        if (Units[i].UnitName == Title2)
                        {

                            ra.Unit = Units[i].UnitID;
                        }
                    }

                    //MessageBox.Show("Unit ID = " + ra.Unit);

                    for (int i = 0; i < DrawingTypes.Count; i++)
                    {
                        if (DrawingTypes[i].DrawingTypeName.ToUpper().Contains(Title5.ToUpper()))
                        {
                            ra.DrawingType = DrawingTypes[i].DrawingTypeID;
                        }
                    }

                    //MessageBox.Show("Drawing Type = " + ra.DrawingType);
                    NewDrawing();

                    AcadUtils.DwgUtils Utils = new AcadUtils.DwgUtils();
                    Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                    Database db = doc.Database;
                    ObjectId msId = Utils.GetMSpaceID(db);
                    cadservices.CadARX RegisterService = new cadservices.CadARX();
                    var MyIni = new IniFile("Setting.ini");
                    RegisterService.Url = MyIni.Read("RegisterDwg_cadservices_CadARX", "RegisterDwg"); //http://cadservices/cadarx/CadARX.asmx
                    RegisterService.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;


                    BorderDetails bd = new BorderDetails();
                    bd.NewBorderFilename = SelectedBorder;
                    bd.BorderName = SelectedBorderName;
                    bd.BorderRotation = 0;
                    bd.BorderInsertionPoint = new Point3d(0, 0, 0);

                    if (Scale == "NTS")
                    {
                        bd.BorderScale = new Scale3d(1, 1, 1);
                        BorderScale = "NTS";
                    }
                    else
                    {
                        bd.BorderScale = new Scale3d(Convert.ToInt64(Scale));
                        BorderScale = "1 : " + Scale;
                    }


                    //MessageBox.Show("Border is sized " + BorderScale);

                    // Insert regoAttributes here
                    //We will get a drawing number returned
                    //ra.Prefix = DwgPrefix.Text; //Need to Generaete that from the text file

                    // ra.Facility = Facilities[Title1ListBox.SelectedIndex].FacilityID; //Need to Generaete that from the text file
                    //ra.Unit = SelectedUnits[Title2ListBox.SelectedIndex].UnitID; //Need to Generaete that from the text file
                    ra.Equipment = Title3;
                    ra.Device = Title4;
                    //ra.DrawingType = DrawingTypes[Title5ListBox.SelectedIndex].DrawingTypeID; //Need to Generaete that from the text file
                    ra.RegistrationNumber = "";



                    //MessageBox.Show(ra.Equipment + ra.Device + ra.Facility + ra.Prefix + ra.Unit + BorderScale);

                    ReturnedRegistrationAttributes rra = new ReturnedRegistrationAttributes();
                    try
                    {
                        rra.DwgInfoNode = RegisterService.CreateDrawing(ra.Prefix, ra.Facility, ra.Unit, ra.Equipment, ra.Device, ra.DrawingType);

                        // remember the xml returned maybe a newly created drawing or details of errors in your parameters ...ie Prefix, Facility etc etc

                    }
                    catch (System.Exception ex)
                    {


                        // if we got an exception calling across the wire to a remote service then it's a waste of time going on.
                        // notfy user and return...

                        MessageBox.Show("Problem calling CreateDrawing Method on Service: " + ex.Message);
                        continue;   //no use going on we have no data
                    }

                    // going on but have not verified that we have a new drawing or details of error in our xml    

                    //MessageBox.Show(rra.DwgInfoNode.InnerXml);

                    XmlNode RecordNode = null;
                    if (rra.DwgInfoNode != null) // check if object returned from service call is not null before calling methods on it
                    {


                        //assigments of expected values in an xml fragment are potential failure spots, so try catch is appropriate
                        try
                        {
                            RecordNode = rra.DwgInfoNode.SelectSingleNode("Record"); // try to retrieve the XML element called Record from the data returned from the service call and assign it to RecordNode
                        }
                        catch (System.Exception)
                        {
                            MessageBox.Show("No Record element in xml returned from CreateDrawing");
                            continue;  // notice I have checked for the existence of a Record element, currently when it isn't there, we just exit...good practice would be to see if there is an Error element so we could show the user the error if it existed..
                        }

                    }
                    else
                    {

                        // null object returned from call... no use going on tell user and return
                        MessageBox.Show("Null Object returned from call to CreateDrawing Method on CadArx Service");
                        continue;
                    }

                    // RecordNode should exist at this time.. good practice dictates that we check it for null before consuming
                    if (RecordNode != null)
                    {

                        // problematic at the moment because we are assuming that the piece of xml actually contains the Drawing Number, it well may contain an Error Element only


                        try
                        {
                            rra.DrawingNumber = RecordNode.SelectSingleNode("DrawingNo").InnerText;
                            rra.Facility = rra.DwgInfoNode.SelectSingleNode("Record").SelectSingleNode("Facility").InnerText;
                            rra.Unit = rra.DwgInfoNode.SelectSingleNode("Record").SelectSingleNode("Unit").InnerText;
                            rra.Equipment = rra.DwgInfoNode.SelectSingleNode("Record").SelectSingleNode("Equipment").InnerText;
                            rra.Device = rra.DwgInfoNode.SelectSingleNode("Record").SelectSingleNode("Device").InnerText;
                            rra.DrawingType = rra.DwgInfoNode.SelectSingleNode("Record").SelectSingleNode("DrawingType").InnerText;
                            rra.RegistrationNumber = rra.DwgInfoNode.SelectSingleNode("Record").SelectSingleNode("Registration").InnerText;

                            //MessageBox.Show(rra.DrawingNumber + rra.DrawingType + rra.Device + rra.Facility + rra.Unit + rra.Equipment + rra.RegistrationNumber);

                        }
                        catch (System.Exception ex)
                        {

                            MessageBox.Show("Problem assigning values from xml to ReturnedRegistrationAttributes type : " + ex.Message);
                            continue;
                        }


                        csvWrite.WriteLine(rra.DrawingNumber + "," + rra.RegistrationNumber + "," + rra.Facility + "," + rra.Unit + "," + rra.Equipment + "," + rra.Device + "," + rra.DrawingType + "," + BorderScale + "," + Size);
                        csvWrite.Flush();

                        string Artist = "";

                        try
                        {
                            //If the check box is not seleted don't Add Authors Name
                            if (ckbArtistName.Checked == false)
                                Artist = "";
                            else
                            {
                                string FirstName = System.DirectoryServices.AccountManagement.UserPrincipal.Current.GivenName;
                                char FirstIntial = FirstName[0];

                                string LastName = System.DirectoryServices.AccountManagement.UserPrincipal.Current.Surname;

                                Artist = (FirstIntial + ". " + LastName).ToUpper();
                            }

                        }
                        catch (System.Exception)
                        {

                            Artist = "";
                        }


                        DocumentLock dl = doc.LockDocument(DocumentLockMode.ProtectedAutoWrite, null, null, true);
                        bd = InsertDrawingBorder(bd);

                        using (dl)
                        {
                            string AttributeBlock = Utils.GetConstantAttribute(db, SelectedBorderName, "BORDERATTBLOCK");
                            if (SelectedBorderName == Utils.GetConstantAttribute(db, SelectedBorderName, "BORDERNAME"))
                            {

                                Utils.UpdateAttributesInBlock(msId, AttributeBlock, "DWGNO", rra.DrawingNumber);
                                Utils.UpdateAttributesInBlock(msId, AttributeBlock, "TITLE1", rra.Facility);
                                Utils.UpdateAttributesInBlock(msId, AttributeBlock, "TITLE2", rra.Unit);
                                Utils.UpdateAttributesInBlock(msId, AttributeBlock, "TITLE3", rra.Equipment);
                                Utils.UpdateAttributesInBlock(msId, AttributeBlock, "TITLE4", rra.Device);
                                Utils.UpdateAttributesInBlock(msId, AttributeBlock, "TITLE5", rra.DrawingType);
                                Utils.UpdateAttributesInBlock(msId, AttributeBlock, "CADNO", rra.RegistrationNumber);
                                Utils.UpdateAttributesInBlock(msId, AttributeBlock, "DWGSCALE", BorderScale);

                                Utils.UpdateAttributesInBlock(msId, AttributeBlock, "ARTIST", Artist);
                                //Add the Current Date to the Title Block
                                Utils.UpdateAttributesInBlock(msId, AttributeBlock, "DWGDATE", DateTime.Now.ToString("dd/MM/yyyy"));
                                //Add the Current Revision of A to the Title Block
                                Utils.UpdateAttributesInBlock(msId, AttributeBlock, "DBREV", "A");

                                //Boolean InsertMdataResult = InsertMetaDataBlock();
                                //CadLogixUtil.Meta data = new CadLogixUtil.Meta();

                                //data.WriteMetaDataBorder(Utils.GetConstantAttribute(db, SelectedBorder, "BORDERNAME"));
                                //data.WriteMetaDataBorderAttributes(Utils.GetConstantAttribute(db, SelectedBorder, "BORDERATTBLOCK"));
                                //data.WriteMetaDataClass(Convert.ToInt32(Utils.GetConstantAttribute(db, SelectedBorder, "BORDERCLASS")));
                                //data.WriteMetaDataRevision(Convert.ToDouble(Utils.GetConstantAttribute(db, SelectedBorder, "BORDERCLASSREV")));
                            }
                            else
                            {
                                MessageBox.Show("The selected Border and the current border are not the same!");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("A valid drawing has not been registered because: " + RecordNode.SelectSingleNode("Error").InnerText);
                    }
                    SaveToVault(rra.DrawingNumber + ".dwg"); //Coles Code to Save to Vault DIR
                    doc.CloseAndDiscard();


                }

                csvWrite.Flush();
                csvWrite.Close();
                csvRead.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
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
