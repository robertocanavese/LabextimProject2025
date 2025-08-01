using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UILabExtim
{
    public class PageTemplateBaseController : Page
    {
        public enum LabExtimErrorType
        {
            UpdateFailed
        }

        public void ToggleSuccessMessage(bool result, Label logLabel, LabExtimErrorType errorType)
        {
            switch (errorType)
            {
                case LabExtimErrorType.UpdateFailed:
                    logLabel.Text = result
                        ? "Operazione eseguita con successo alle " + DateTime.Now.ToLongTimeString() + " di " +
                          DateTime.Now.ToLongDateString()
                        : "Operazione fallita alle " + DateTime.Now.ToLongTimeString() + " di " +
                          DateTime.Now.ToLongDateString() +
                          "</br>Per poter cancellare una voce è necessario che siano eliminate tutte le voci che ne fanno uso! </br>(riprovare o consultare il log errori dell'applicazione)";
                    break;

                default:
                    break;
            }
        }
    }
}