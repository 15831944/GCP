using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CableUpdater
{
    class Idle : IState
    {
        private Idle()
        { }

        private static Idle _instance;
        public static Idle Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Idle();
                }
                return _instance;
            }
        }

        #region IState Members

        public void Enter(IEntity entity)
        {
            //TODO: code to do something as we enter this state
            //Helper.InfoMessageBox("Entering the  Idle state" );
        }

        public void Execute(IEntity entity)
        {

            //TODO: code to do actions while we are in this state
            BlockDetails bd = entity as BlockDetails;

            try
            {
                bd.CriticalError = 0;
                bd.DwgCounter = 0;
                
                bd.ChangeState(UpdateCableBlocks.Instance);

            }
            catch (Exception ex)
            {
                bd.DidItOk = false;
                bd.ErrorMessage = ex.ToString();
                bd.CriticalError = 3;
                bd.ChangeState(ErrorState.Instance);

            }



        }

        public void Exit(IEntity entity)
        {

            //TODO: code to do something as we exit this state
            //Console.WriteLine ("Exiting the  Idle state");
        }

        #endregion
    }
}
