<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SessionKeeper.aspx.cs" Inherits="LabExtim.SessionKeeper" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Mantiene la sessione attiva</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        File non utilizzato: serve solo per tenere la sessione attiva.
        Viene richiamato dall'iframe presente nella testata dell'applicativo.
        <!-- Vedere Site.Master per maggiori dettagli -->
        Caricato il <%= DateTime.Now %><br />
        Ultimo caricamento:<span id="UltimoCaricamento" runat="server"></span>
    </div>
    </form>
</body>
</html>
<script language="JavaScript" type="text/javascript">
	//esegue aggiornamento pagina ogni 5 minuti
	 setTimeout(function(){document.location.reload(true);}, (1*60*1000));
</script>
