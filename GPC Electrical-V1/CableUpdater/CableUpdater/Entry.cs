using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using System.Windows.Forms;

namespace CableUpdater
{
    //see Rohan to explain a state machine or google it
        public interface IEntity
        {
            IState CurrentState { get; set; }
            IState PreviousState { get; set; }
            void ChangeState(IState newState);
        }


        public class EntityBase : IEntity
        {
            private IState mCurrentState;
            private IState mPreviousState;

            #region IEntity Members

            public IState CurrentState
            {
                get
                { return mCurrentState; }
                set
                { mCurrentState = value; }
            }

            public IState PreviousState
            {
                get
                { return mPreviousState; }
                set
                { mPreviousState = value; }
            }

            public void ChangeState(IState newState)
            {
                if (CurrentState != null)
                    CurrentState.Exit(this);

                PreviousState = CurrentState;
                CurrentState = newState;

                newState.Enter(this);
            }

            #endregion
        }


        public interface IState
        {
            void Enter(IEntity entity);
            void Execute(IEntity entity);
            void Exit(IEntity entity);
        }

        public class fsm : EntityBase
        {
            public fsm()
            { }
            public int StepCount = 0;

        }
        public class BlockDetails : EntityBase
        {
            //setup the object to carry data from state to state
            public BlockDetails()
            { }
            public string DrawingFileName { get; set; }
            public ListBox.ObjectCollection lbDwgs { get; set; }//A collection of the lines from the drawings listbox
            public List<string> CableList = new List<string>();//a collection of the lines in the datagrid(cable numbers). Note that it is initialized differently
            public int DwgCounter { get; set; }//Counter for keeping track of drawings processed
            public string FullPath { get; set; }//Full path of the drawing folder
            public string ErrorPath { get; set; }
            public string DataPath { get; set; }//the path of the cable text file
            public string ErrorLog { get; set; }
            public ObjectId SpaceID { get; set; }//Model space 
            public bool KeepRunning { get; set; }
            public bool DidItOk { get; set; }
            public string ErrorMessage { get; set; }
            public int CriticalError { get; set; }
        }
    
}
