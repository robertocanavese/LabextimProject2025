<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
         CodeBehind="Login.aspx.cs" Inherits="LabExtim.Login"   %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <table width="100%" >
        <tr>
            <td align="center" >
                <asp:Login ID="Login1" runat="server" PasswordRequiredErrorMessage="La password è obbligatoria."
                           RememberMeText="Ricorda nome utente e password." UserNameLabelText="Nome Utente:"
                           UserNameRequiredErrorMessage="Il Nome Utente è obbligatorio." 
                           MembershipProvider="MyMembershipProvider" RememberMeSet="true" 
                           onloggedin="Login1_LoggedIn" CssClass="loginStyle" >
                    <TextBoxStyle Width="150px" CssClass="loginStyle" />
                    <LoginButtonStyle  CssClass="myButton" />
                </asp:Login>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:PasswordRecovery ID="PasswordRecovery1" runat="server" AnswerLabelText="Risposta:"
                                      AnswerRequiredErrorMessage="La risposta è obbligatoria." GeneralFailureText="Il tentativo di ricevere la password è fallito. Riprovare."
                                      QuestionFailureText="La risposta non corrisponde. Riprovare." QuestionInstructionText="Rispondere alla seguente domanda per ricevere la password."
                                      QuestionLabelText="Domanda:" QuestionTitleText="Verifica identità" SubmitButtonText="Ricevi e-mail"
                                      SuccessText="La password è stata inviata al tuo indirizzo e-mail." UserNameFailureText="Tentativo di verificare il nome utente fallito. Riprovare."
                                      UserNameInstructionText="Inserire il nome utente per riceverla via e-mail." UserNameLabelText="Nome Utente:"
                                      UserNameRequiredErrorMessage="Il nome utente è obbligatorio." UserNameTitleText="Dimenticata la password?" CssClass="loginStyle">
                    <TextBoxStyle Width="150px" CssClass="loginStyle" />
                    <SubmitButtonStyle  CssClass="myButton" />
                </asp:PasswordRecovery>
            </td>
        </tr>
    </table>
</asp:Content>