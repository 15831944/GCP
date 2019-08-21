using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Banner
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
                    //Class1.log.Error("An Error occured during Upgrade routine because of:- " + bd.ErrorMessage);
                    System.Windows.Forms.MessageBox.Show("This drawing could not be upgraded because: " + bd.ErrorMessage);
                    bd.KeepRunning = false;
                    bd.ChangeState(Idle.Instance);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("An email has been sent to Cad Support with the error information" + bd.ErrorMessage);
                   // Class1.log.Fatal("There was a serious error during the 'AttributeInsert' routine because of:- " + bd.ErrorMessage);
                    bd.KeepRunning = false;
                    bd.ChangeState(Idle.Instance);
                }

                ////Get the current identity and put it into an identity object.
                //WindowsIdentity MyIdentity = WindowsIdentity.GetCurrent();

                ////Put the previous identity into a principal object.
                //WindowsPrincipal MyPrincipal = new WindowsPrincipal(MyIdentity);

                //// build the mail message
                //int myind = MyPrincipal.Identity.Name.IndexOf(@"\");
                //string temp =  (MyPrincipal.Identity.Name.Substring(myind+1) + "@gpcl.com.au");
                //MailMessage msg = new MailMessage(temp, "klupfelk@gpcl.com.au");
                ////msg.To.Add("krisnkaren@gmail.com");
                //msg.Subject = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                //msg.Body = bd.ErrorMessage;
                //msg.IsBodyHtml = false;

                //// build the smtp client
                //SmtpClient smtp = new SmtpClient("CQPAMAIL.gpa.org.au");
                //smtp.UseDefaultCredentials = true;
                //smtp.Send(msg);

                //Helper.InfoMessageBox("An email has been sent to Cad Support with the error information");
                //bd.KeepRunning = false;
                //bd.ChangeState(Idle.Instance);

            }
            catch (Exception ex)
            {

                Helper.InfoMessageBox(ex.ToString());
                bd.KeepRunning = false;
                bd.ChangeState(Idle.Instance);
                //Class1.log.Error("Error occurred during the 'Error State.", ex);

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
