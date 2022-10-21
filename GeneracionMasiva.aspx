<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GeneracionMasiva.aspx.cs" Inherits="TIFacturasMasivas.GeneracionMasiva" MaintainScrollPositionOnPostback="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    
     <script src="https://code.jquery.com/jquery-3.2.1.slim.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js" ></script>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" />
    
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js"></script>
     <%--<script src="https://cdnjs.cloudflare.com/ajax/libs/bootbox.js/5.5.2/bootbox.min.js"></script>--%>
    <script type="text/javascript" src='https://cdn.jsdelivr.net/sweetalert2/6.3.8/sweetalert2.min.js'> </script>
        <link rel="stylesheet" href='https://cdn.jsdelivr.net/sweetalert2/6.3.8/sweetalert2.min.css'
            media="screen" />
    <script src="https://kit.fontawesome.com/789a3ce2b4.js" crossorigin="anonymous"></script>
    <style>
        .cimagen {
            width:100% !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <asp:ScriptManager ID="ScriptManager1" AsyncPostBackTimeOut="360000" runat="server"></asp:ScriptManager>
           
               <div class="row">
                   
                   <div class="col-md-12">
                       <div id = "ParentDiv">
        <asp:UpdatePanel runat="server" ID="up">
            <ContentTemplate>
                <br />
                <div id="master_headertop">
            
                <asp:Image runat="server" ID="HeaderImage" CssClass="cimagen" ImageUrl="img/banner4.png" AlternateText="Ajax Control Toolkit" />
            </asp:HyperLink>
        </div>
                <center>
                    <h1>
                        Generación Masiva de Facturas</h1>
                </center>
                <center>
                    <div>
                        <br />
                        <br />
                        <table class="table" > 
                            <tr>
                                <td align="center" colspan="4" style="height: 24px">
                                    <div>
                                        <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 81px; height: 24px">
                                Cliente: &nbsp;</td>
                                <td align="left" style="height: 24px">
                                    <div>
                                        &nbsp;<%--<asp:DropDownList ID="lstCliente" runat="server" DataSourceID="clientes" DataTextField="idreceptor" DataValueField="idreceptor">--%>
                                        <asp:DropDownList ID="lstCliente" runat="server" >
                                        </asp:DropDownList>
                                       <%-- <asp:SqlDataSource ID="clientes" runat="server" ConnectionString="<%$ ConnectionStrings:TDR %>"  SelectCommand="select distinct  idreceptor from vista_fe_header">
                                        </asp:SqlDataSource>--%>
                                    </div>
                                </td>
                                <td align="right" style="width: 79px; height: 24px">
                                        Elaborada por:</td>
                                <td align="left" style="width: 174px; height: 24px">
                                    <div>
                                        &nbsp;
                                        <%--<asp:DropDownList ID="lstElaborada" runat="server" DataSourceID="hechapor" DataTextField="hecha" DataValueField="hecha">
                                        </asp:DropDownList><asp:SqlDataSource ID="hechapor" runat="server" ConnectionString="<%$ ConnectionStrings:TDR %>"
                                    SelectCommand="select distinct hecha from vista_fe_header union (select '') order by hecha">--%>
                                        <asp:DropDownList ID="lstElaborada" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 81px; height: 26px">
                                </td>
                                <td align="left" style="width: 141px; height: 26px">
                                </td>
                                <td align="right" colspan="2" style="width: 79px; height: 26px">
                                        <asp:Button CssClass="btn btn-primary" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" /><asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                            <ProgressTemplate>
                                                <asp:Image ID="Image2" runat="server"  ImageUrl="img/indicator.gif" />
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
               
            </Triggers>
        </asp:UpdatePanel>
        <p>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:CheckBox ID="chkSelectAll" runat="server" Text="Seleccionar todos" AutoPostBack="True" OnCheckedChanged="chkSelectAll_CheckedChanged" /><br />
                <center>
                    <asp:PlaceHolder ID="gridPlace" runat="server"></asp:PlaceHolder>
                </center>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnBuscar" /> 
                <asp:AsyncPostBackTrigger ControlID="chkSelectAll" />    
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
                &nbsp;<asp:Button ID="btnGuarda" CssClass="btn btn-primary" runat="server" OnClick="Button1_Click" Text="Generar CFDi" CausesValidation="False"  /><br />
            </div>
        </center>
    </div>  
                   </div>
               </div>
           
           
    <script type="text/JavaScript" language="JavaScript"> 
    </script>
  </asp:Content>
        </div>
    </form>
</body>
</html>


