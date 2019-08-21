using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CableUpdater
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
                bd.DwgCounter++;
                if (bd.DwgCounter < bd.lbDwgs.Count)
                {
                    bd.ChangeState(UpdateCableBlocks.Instance);
                }
                else
                {
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
