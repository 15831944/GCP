using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;


namespace TextReplace
{
 
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
        public BlockDetails()
        { }
        public string DrawingFileName { get; set; }

        public ListBox.ObjectCollection lbtext { get; set; }
        public ListBox.ObjectCollection lbDwgs { get; set; }
        public List<Autodesk.AutoCAD.DatabaseServices.MText> MTextList = new List<MText>();
        public List<Autodesk.AutoCAD.DatabaseServices.DBText> DBTextList = new List<DBText>();
        public int DwgCounter { get; set; }
        public bool extractionState { get; set; }
        public string FullPath { get; set; }
        public string DataPath { get; set; }
        public string BodyText { get; set; }
        public string BorderText { get; set; }
        public string ErrorLog { get; set; }
        public string AddTextBoxString { get; set; }
        public int OldattCount { get; set; }
        public string BlockName { get; set; }
        public ObjectId BlockID { get; set; }
        public ObjectId SpaceID { get; set; }//Model space 
        public bool KeepRunning { get; set; }
        public bool DidItOk { get; set; }
        public string ErrorMessage { get; set; }
        public int CriticalError { get; set; }
    }

}

