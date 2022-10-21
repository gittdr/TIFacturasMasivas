<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Busqueda.aspx.cs" Inherits="TIFacturasMasivas.Busqueda" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" AsyncPostBackTimeOut="360000" runat="server"></asp:ScriptManager>
            <asp:Content ID="Content1" ContentPlaceHolderID="SampleContent" runat="Server">
    <div id = "ParentDiv">
        <asp:UpdatePanel runat="server" ID="up">
            <ContentTemplate>
                <br />
                <center>
                    <h1>
                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="Date1" PopupButtonID="Image1"></cc1:CalendarExtender>
                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="Date2" PopupButtonID="imgCalendar2"></cc1:CalendarExtender>
                        Búsqueda de Facturas
                    </h1>
                </center>
                <center>
                    <div>
                        <br />
                        <br />
                        <table style="border-left-color: #000000; border-bottom-color: #000000; width: 591px; border-top-style: solid; border-top-color: #000000; border-right-style: solid; border-left-style: solid; border-right-color: #000000; border-bottom-style: solid"> 
                            <tr>
                                <td align="center" colspan="4" style="height: 24px">
                                    <div>
                                        <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 81px; height: 24px">
                                    Serie:
                                </td>
                                <td align="left" style="height: 24px">
                                    <div>
                                        <asp:DropDownList ID="lstSerie" runat="server">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="TDRT">Factura</asp:ListItem>
                                            <asp:ListItem Value="NCT">Nota de Cr&#233;dito</asp:ListItem>
                                          
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td align="right" style="width: 79px; height: 24px">
                                Cliente:
                            </td>
                                <td align="left" style="width: 174px; height: 24px">
                                    <div>
                                        <asp:DropDownList ID="lstCliente" runat="server" DataSourceID="clientes" DataTextField="idreceptor" DataValueField="idreceptor">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="clientes" runat="server" ConnectionString="<%$ ConnectionStrings:TDR %>" SelectCommand="select distinct  idreceptor from vista_fe_generadas union (select '') order by idreceptor">
                                        </asp:SqlDataSource>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 81px; height: 42px">
                                    De:
                                </td>
                                <td align="left" style="width: 194px; height: 42px">
                                    <div>
                                        <asp:TextBox ID="Date1" runat="server"></asp:TextBox><asp:ImageButton
                                            ID="Image1" runat="Server" AlternateText="Click to show calendar" ImageUrl="~/img/Calendar_scheduleHS.png" /></div>
                                </td>
                                <td align="right" style="width: 79px; height: 42px">
                                    A: 
                                </td>
                                <td align="left" style="width: 174px; height: 42px">
                                    <div>
                                        <asp:TextBox ID="Date2" runat="server"></asp:TextBox>
                                        <asp:ImageButton ID="imgCalendar2" runat="Server" AlternateText="Click to show calendar" ImageUrl="~/img/Calendar_scheduleHS.png" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 81px; height: 26px">
                                    Invoice:
                                </td>
                                <td align="left" style="width: 141px; height: 26px">
                                    <div>
                                        <asp:TextBox ID="txtInvoice" runat="server"></asp:TextBox>
                                    </div>
                                </td>
                                <td align="right" style="width: 79px; height: 26px">
                                    Master:
                                </td>
                                <td align="left" style="width: 174px; height: 26px">
                                    <div>
                                        <asp:TextBox ID="txtMaster" runat="server"></asp:TextBox>
                                        
                                    </div>
                                </td>
                            </tr>
                            <tr>


                                <td align="right" style="width: 81px; height: 26px">
                                    Sistema Facturación
                                </td>
                                <td align="left" style="width: 141px; height: 26px">
                                    <div>
                                        <asp:DropDownList ID="lstSistema" runat="server">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="WF">WF</asp:ListItem>
                                            <asp:ListItem Value="TRALIX">Tralix</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td align="right" style="width: 79px; height: 26px">
                                    <div>
                                        Elaborada por:&nbsp;
                                    </div>
                                </td>
                                <td align="left"><asp:DropDownList ID="lstElaborada" runat="server" DataSourceID="hechapor" DataTextField="hechapor" DataValueField="hechapor">
                                        </asp:DropDownList><asp:SqlDataSource ID="hechapor" runat="server" ConnectionString="<%$ ConnectionStrings:TDR %>"
                                    SelectCommand="select distinct hechapor from vista_fe_generadas union (select '') order by hechapor">
                                </asp:SqlDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 81px; height: 26px">
                                    Referencia:</td>
                                <td align="left" style="width: 141px; height: 26px">
                                    <asp:TextBox ID="txtReferencia" runat="server"></asp:TextBox></td>
                                <td align="right" style="width: 79px; height: 26px">
                                    Orden:</td>
                                <td align="left">
                                    <asp:TextBox ID="txtOrden1" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 81px; height: 26px">
                                    </td>
                                <td align="left" style="width: 141px; height: 26px">
                                    </td>
                                <td align="right" colspan="2" style="width: 79px;">
                                        <asp:Button ID="btnBuscar" runat="server" OnClick="Button1_Click" Text="Buscar" /><asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                            <ProgressTemplate>
                                                <asp:Image ID="Image2" runat="server" ImageUrl="~/img/indicator.gif" />
                                                <asp:Label ID="lblProgress" runat="server" Text="Generando Busqueda....."></asp:Label>
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID = "btnDescargar" />
            </Triggers>
        </asp:UpdatePanel>
        <p>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelectAll_CheckedChanged" Text="Seleccionar todos" />
                <center>
                        <br />
                    <asp:PlaceHolder ID="gridPlace" runat="server"></asp:PlaceHolder>
                </center>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                <asp:AsyncPostBackTrigger ControlID="btnDescargar" />
            </Triggers>
        </asp:UpdatePanel>
        </p>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>   
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <center>
            <div>
                <asp:Button ID="btnDescargar" runat="server" OnClick="btnDescargar_Click" Text="Descargar" /><br />
            </div>
        </center>
    </div>         
    <script type="text/JavaScript" language="JavaScript"> 
    </script>
  </asp:Content>


        </div>
    </form>
</body>
</html>
