using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace cableFinder
{
    class IncrementDWG : IState
    {
        private IncrementDWG()
        { }

        private static IncrementDWG _instance;
        public static IncrementDWG Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new IncrementDWG();
                }
                return _instance;
            }
        }

        #region IState Members

        public void Enter(IEntity entity)
        {
            //TODO: code to do something as we enter this state
            //Helper.InfoMessageBox("Entering the IncrementDWG state");


        }

        public void Execute(IEntity entity)
        {

            BlockDetails bd = entity as BlockDetails;
            try
            {
                bd.CriticalError = 0;
                bd.StartNo++;
                if (bd.StartNo < bd.lbDwgs.Count)
                {
                    bd.ChangeState(GetDrawing.Instance);
                }
                else
                {
                    if (bd.HasErrors)
                    {
                        MessageBoxButtons buttons = MessageBoxButtons.YesNo;

                        DialogResult result = MessageBox.Show("there were errors during the Attribute update process.\nDo you want to view the file?"
                           , "Update Errors", buttons);
                        if (result == System.Windows.Forms.DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start("notepad.exe", bd.ErrorTextFile);

                        }

                    }

                    TextWriter tw = null;
                    if (File.Exists(bd.NewCableTextFile))
                    {
                    tw = new StreamWriter(bd.NewCableTextFile, true);
                    }
                    else
                    {
                        tw = new StreamWriter(Path.Combine(Path.GetDirectoryName(bd.DrawingFileName),"Cablerefs.txt"),false);
                    }
                    for (int i = 0; i<bd.Cable_Numbers.Count;i++)
                    {
                        tw.WriteLine(bd.Cable_Numbers[i] + ":" + bd.Drawings[i]);

                    }

                    tw.Flush();
                    tw.Close();
                    bd.KeepRunning = false;
                    bd.ChangeState(Idle.Instance);
                }


            }
            catch (Exception ex)
            {
                bd.DidItOk = false;
                bd.ErrorMessage = ex.ToString();
                bd.CriticalError = 2;
                bd.ChangeState(ErrorState.Instance);
                //Class1.log.Error("An error occured in the Increment state.", ex);
            }
        }

        public void Exit(IEntity entity)
        {

            //TODO: code to do something as we exit this state
            //Console.WriteLine ("Exiting the IncrementDWG state");



        }

        #endregion
    }
}
