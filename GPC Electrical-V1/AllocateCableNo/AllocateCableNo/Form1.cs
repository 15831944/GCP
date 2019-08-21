using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Principal;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;

namespace AllocateCableNo
{
    public partial class Form1 : Form
    {
        public string SelectedCablePrefix { get; set; }
        public int SelectedCableId { get; set; }
        public string CableNumber { get; set; }


        public class CablePrefixs
        {
            public XmlNode CablePrefixNode { get; set; }
            public Int32 PrefixID { get; set; }
            public string Prefix { get; set; }
        }

        public class CableNumbers
        {
            public XmlNode CableNos { get; set; }
            public string CableID { get; set; }
            public string FromRoute { get; set; }
            public string ToRoute { get; set; }
            public string Type { get; set; }
            public string Grade { get; set; }
        }

        public List<CablePrefixs> Cables = new List<CablePrefixs>();


        public Form1()
        {
            InitializeComponent();
            string settingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Setting.ini";
            if (!File.Exists(settingPath))
            {
                MessageBox.Show("Setting.ini data does not exist..!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            cadservices.CadARX CableService1 = new cadservices.CadARX();
            var MyIni = new IniFile("Setting.ini");
            CableService1.Url = MyIni.Read("AllocateCableNo_cadservices_CadARX", "AllocateCableNo"); //http://cadservices/cadarx/CadARX.asmx     
            try
            {
                CablePrefixs cp = new CablePrefixs();
                CableService1.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                //CableService1.Credentials = new System.Net.NetworkCredential("USERNAME", "PASSWORD");
                cp.CablePrefixNode = CableService1.ShowCablePrefix();

                XmlNodeList CablePrefixList = cp.CablePrefixNode.SelectNodes("Record");

                for (int i = 0; i < CablePrefixList.Count; i++)
                {
                    XmlNode recnode = CablePrefixList[i];

                    //Add all of  the Cable Prefix objects to the 'Facilities' list
                    Cables.Add(new Form1.CablePrefixs
                    {
                        PrefixID = Convert.ToInt32(recnode.SelectSingleNode("ID").InnerText),
                        Prefix = recnode.SelectSingleNode("Prefix").InnerText,
                    });

                    //Add the facility names from the added object to the combobox list
                    CablePrefix.Items.Add(Cables[i].Prefix);

                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Cannot connect to " + CableService1.Url + Environment.NewLine + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {

        }

        private void AllocateButton_Click(object sender, EventArgs e)
        {
            string settingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Setting.ini";
            if (!File.Exists(settingPath))
            {
                MessageBox.Show("Setting.ini data does not exist..!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            cadservices.CadARX ReqCableNo = new cadservices.CadARX();
            var MyIni = new IniFile("Setting.ini");
            ReqCableNo.Url = MyIni.Read("AllocateCableNo_cadservices_CadARX", "AllocateCableNo"); //http://cadservices/cadarx/CadARX.asmx       

            try
            { //clear the text box
                CableNoTextBox.Text = string.Empty;

                //get the 2 variables from the dialog
                if(Cables.Count>0)
                {
                    SelectedCablePrefix = Cables[CablePrefix.SelectedIndex].Prefix;
                    string Project = ProjectDesc.Text;

                    CableNumbers ReturnedCable = new CableNumbers();
                    ReqCableNo.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                    //ReqCableNo.Credentials = new System.Net.NetworkCredential("USERNAME", "PASSWORD");
                    //Send the data to the web service
                    ReturnedCable.CableNos = ReqCableNo.CreateCableSimple(SelectedCablePrefix, Project);
                    XmlNode RecordNode = ReturnedCable.CableNos.SelectSingleNode("Record");

                    ReturnedCable.CableID = RecordNode.SelectSingleNode("CableID").InnerText;

                    //Display the cable number
                    CableNoTextBox.Text = ReturnedCable.CableID;
                    //Test the result for a valid cable number
                }

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Cannot connect to " + ReqCableNo.Url + Environment.NewLine + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnBulkRegister_Click(object sender, EventArgs e)
        {
            string settingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Setting.ini";
            if (!File.Exists(settingPath))
            {
                MessageBox.Show("Setting.ini data does not exist..!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var MyIni = new IniFile("Setting.ini");
            string docPath = MyIni.Read("AllocateCableNoRegisterDataPath", "AllocateCableNo"); //"C:\\Vault\\CabeBulkResults.csv";

            if (System.IO.File.Exists(docPath))
            {
                StreamWriter csvWrite = new StreamWriter(docPath);

                for (int i = 0; i < spnCableCount.Value; i++)
                {
                    //clear the text box
                    CableNoTextBox.Text = string.Empty;

                    //get the 2 variables from the dialog
                    SelectedCablePrefix = Cables[CablePrefix.SelectedIndex].Prefix;
                    string Project = ProjectDesc.Text;

                    cadservices.CadARX ReqCableNo = new cadservices.CadARX();
                    ReqCableNo.Url = MyIni.Read("AllocateCableNo_cadservices_CadARX", "AllocateCableNo"); //http://cadservices/cadarx/CadARX.asmx      

                    try
                    {
                        CableNumbers ReturnedCable = new CableNumbers();
                        ReqCableNo.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                        //ReqCableNo.Credentials = new System.Net.NetworkCredential("USERNAME", "PASSWORD");
                        //Send the data to the web service
                        ReturnedCable.CableNos = ReqCableNo.CreateCableSimple(SelectedCablePrefix, Project);
                        XmlNode RecordNode = ReturnedCable.CableNos.SelectSingleNode("Record");

                        ReturnedCable.CableID = RecordNode.SelectSingleNode("CableID").InnerText;

                        //Display the cable number
                        CableNoTextBox.Text = ReturnedCable.CableID;

                        //Test the result for a valid cable number

                        csvWrite.WriteLine(ReturnedCable.CableID);
                        csvWrite.Flush();
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show("Cannot connect to " + ReqCableNo.Url + Environment.NewLine + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                csvWrite.Close();
                csvWrite.Dispose();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(docPath + " does not exist");
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
