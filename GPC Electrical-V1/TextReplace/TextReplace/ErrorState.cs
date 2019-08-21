using System;
using System.Net.Mail;
using System.Collections.Generic;
using System.Security.Principal;
using System.Security.Cryptography;

namespace TextReplace
{

    class ErrorState : IState
    {
        private ErrorState()
        { }

        private static ErrorState _instance;
        public static ErrorState Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ErrorState();
                }
                return _instance;
            }
        }

        #region IState Members

        public void Enter(IEntity entity)
        {
            //TODO: code to do something as we enter this state
            //Helper.InfoMessageBox("Entering the ErrorState state");

        }

        public void Execute(IEntity entity)
        {

            //TODO: code to do actions while we are in this state
            BlockDetails bd = entity as BlockDetails;

            try
            {
                if (bd.CriticalError < 3)
                {

                    System.Windows.Forms.MessageBox.Show("The text replace process failed because: " + bd.ErrorMessage);
                    bd.KeepRunning = false;
                    bd.ChangeState(Idle.Instance);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("An Error occured during the text replacement routine because of:- " + bd.ErrorMessage);
                    
                    bd.KeepRunning = false;
                    bd.ChangeState(Idle.Instance);
                }

            }
            catch (Exception ex)
            {

                Helper.InfoMessageBox(ex.ToString());
                bd.KeepRunning = false;
                bd.ChangeState(Idle.Instance);


            }


        }

        public void Exit(IEntity entity)
        {

            //TODO: code to do something as we exit this state
            //Helper.InfoMessageBox("Exiting the ErrorState state");

        }

        #endregion
    }
}
