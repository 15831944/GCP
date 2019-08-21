using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using RegisteredDrawingUpload;
using System.Linq;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Windows.Forms;

namespace TestRegisteredDrawingUpload
{
    class Program
    {
        static void Main(string[] args)
        {
            string settingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Setting.ini";
            if (!File.Exists(settingPath))
            {
                MessageBox.Show("Setting.ini data does not exist..!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                SharePointPersist MyPersist = new SharePointPersist();
                //107-19553     c:/dump/107-19553.dwg      R.G.TANNA COAL TERMINAL FIBRE SYSTEM SITE WIDE CAMERAS TOPOLOGY CABLE BLOCK DIAGRAM
                var MyIni = new IniFile("Setting.ini");
                string docPath = MyIni.Read("Upload2SPErrorLogFilePath", "Upload2SP"); //"c:/dump/107-19553.dwg";

                byte[] DwgBytes = StreamDwgFile(docPath);
                var PersistReply = MyPersist.UploadDwg(docPath, "R.G.TANNA COAL TERMINAL FIBRE SYSTEM SITE WIDE CAMERAS TOPOLOGY CABLE BLOCK DIAGRAM", DwgBytes);
                //Helper.InfoMessageBox(PersistReply.ToString());
                if (PersistReply < 1)
                {
                    Console.WriteLine(" {0} was not saved to the SharePoint File Store", docPath);
                }
                else
                {
                    Console.WriteLine(" {0} Was uploaded to SharePoint File Store correctly", docPath);
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(" Trouble with test : {0} ", ex.Message);
            }

            Console.ReadLine();
        }



        /// <summary>
        /// Streams the DWG file to a byte array.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        private static byte[] StreamDwgFile(string filename)
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
