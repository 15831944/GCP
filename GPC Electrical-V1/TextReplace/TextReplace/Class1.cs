using System;
using System.IO;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;



namespace TextReplace
{
    public class Class1
    {

        private static BlockDetails bd;

        // Create a logger for use in this class

        // NOTE that using System.Reflection.MethodBase.GetCurrentMethod().DeclaringType
        // is equivalent to typeof(StartHere) but is more portable
        // i.e. you can copy the code directly into another class without
        // needing to edit the code.

        /// <summary>
        /// Entry point of the Border Update FSM.
        /// Updates the legacy border to the current version and adds metadata.
        /// </summary>
        /// <param name="args">The args.</param>
        [CommandMethod("fr", CommandFlags.Session)]
        public static void GetAlltext(string[] args)
        {
            try
            {

                bd = new BlockDetails();

                DrawingSelection ds = new DrawingSelection();
                System.Windows.Forms.DialogResult dsResult = ds.ShowDialog();
                if (dsResult == System.Windows.Forms.DialogResult.OK)
                {
                    bd.lbtext = ds.tlbo;
                    bd.lbDwgs = ds.lbo;
                    bd.DataPath = ds.DataPath;
                    bd.FullPath = ds.DWGFolderPath;
                    ds.Close();
                    bd.KeepRunning = true;
                    bd.ChangeState(ReplaceBodyText.Instance);
                }
                else
                {
                    ds.Close();
                    bd.KeepRunning = false;
                    bd.ChangeState(Idle.Instance);

                }

                //bd.KeepRunning = true;
                //bd.ChangeState(Idle.Instance);

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
            }
        }
    }
}
