using System;
using System.IO;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;

namespace CableUpdater
{
    public class Class1
    {
        private static BlockDetails bd;



        /// <summary>
        /// Entry point of the Cable Block Updater FSM.
        /// Updates the cable blocks with new attributes.
        /// </summary>
        /// <param name="args">The args.</param>
        [CommandMethod("CU", CommandFlags.Session)]
        public static void UpdateCableBlockAtts(string[] args)
        {
            try
            {
                //create the initial 'BloclDetails' object and set some of the variables
                bd = new BlockDetails();
                //Open the dialog box
                DrawingSelection ds = new DrawingSelection();
                System.Windows.Forms.DialogResult dsResult = ds.ShowDialog();
                
                //'Update Cables' button set as the dialog 'ok' dialog result button
                if (dsResult == System.Windows.Forms.DialogResult.OK)
                {
                    //Copy the data from the 'ds' variables where the data was saved by the 'update cables' button in 'DrawingSelection.cs'
                    bd.lbDwgs = ds.lbo;
                    bd.CableList = ds.StringList;

                    bd.ErrorPath = ds.ErrorPath;
                    bd.DataPath = ds.DataPath;
                    bd.FullPath = ds.DWGFolderPath;
                    //Close the dialog and go to the 'Idle' state 
                    ds.Close();
                    bd.KeepRunning = true;
                    bd.ChangeState(Idle.Instance);
                }
                //Go here if 'cancel button is pushed
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
            //If there is an error at this stage it is critical. We go to the 'ErrorState' to process the error
            catch (System.Exception ex)
            {
                bd.DidItOk = false;
                bd.ErrorMessage = "Error during the dialog open, processing and close operation. " + ex.ToString();
                bd.CriticalError = 3;
                bd.ChangeState(ErrorState.Instance);

            }
        }
    }
}
