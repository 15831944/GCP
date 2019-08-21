using System.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;

namespace Banner
{

/* Any class that implements our interface will support the notion of getting and setting the Current State
as well as getting and setting the previous state. We need also to be able to handle a transition (change of state)
and do some worthwhile processing as we are changing from one state to the next */


        public interface IEntity
        {
            IState CurrentState { get; set; }
            IState PreviousState { get; set; }
            void ChangeState(IState newState);
        }

        /* Here is our class that handles the boring implementation of getting and setting stuff
            as well as the change of state details...The non boring bit is that it does all this
            boring stuff via the passing of an interface of type IState... IState is our friend...
            We like IState because it is simple ... With just a few IState we can do some pretty
            complex logic and not get much of a sweat up doing it either. */

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


        /* Any class that implements our interface will support the notion of entering Current State as well as exiting the current state
           We need also to be able to handle an execute method in which we carry out some worthwhile actions appropriate to the current state
        */

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
            public BlockDetails()
            { }
            public string DrawingFileName { get; set; }
            public string FullPath { get; set; }
            public int StartNo { get; set; }
            public string Area { get; set; }
            public int finishNo { get; set; }
            public string ErrorLog { get; set; }
            public string BlockName { get; set; }
            public ObjectId BlockID { get; set; }
            public ObjectId SpaceID { get; set; }//Model space 
            public string CurrentBorder { get; set; } //Current border block name
            public string AttBlkName { get; set; } //new border attribute name

            public string BannerLine1 { get; set; }
            public string BannerLine2 { get; set; }
            public string BannerLine3 { get; set; }
            public string ReservedText { get; set; }
            public bool UploadDwg { get; set; }
            
            
            public string Drawer { get; set; } //Drawer name
            public System.DateTime DwnDate { get; set; }
            public ListBox.ObjectCollection lbDwgs { get; set; }

            public string ErrorTextFile { get; set; }
            public bool HasErrors { get; set; }
            public bool KeepRunning { get; set; }
            public bool DidItOk { get; set; }
            public string ErrorMessage { get; set; }
            public int CriticalError { get; set; }
        }
}
