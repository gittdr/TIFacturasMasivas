<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Adenda.aspx.cs" Inherits="TIFacturasMasivas.Adenda" %>

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
    <asp:HyperLink ID="linkBusqueda" runat="server" NavigateUrl="http://172.16.137.33:1501/Index.aspx" Target="_parent">Busqueda</asp:HyperLink>&nbsp;
    <br />
    <center>   
        <h1>
            <asp:Label ID="lblTipo" runat="server" Text="Label"></asp:Label>
            <asp:Label ID="lblFact" runat="server" Text="Label"></asp:Label>
    <br />
    <br />
    <br />
            <asp:PlaceHolder ID="estatusFactura" runat="server"></asp:PlaceHolder></h1>
        <p>
            &nbsp;&nbsp;<asp:Panel ID="AdendaPanel" runat="server" Height="134px" Visible="False">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table style="border-left-color: #000000; border-bottom-color: #000000; width: 644px;
                            border-top-style: solid; border-top-color: #000000; border-right-style: solid;
                            border-left-style: solid; height: 100px; border-right-color: #000000; border-bottom-style: solid">
                            <tr>
                                <td align="center" colspan="4" style="height: 24px">
                                    Complemento Liverpool</td>
                            </tr>
                            <tr>
                                <td colspan="4" style="height: 24px" align="left">
                                    <div>
                                        <asp:CheckBox ID="checkAdenda" runat="server" Text="Agregar Adenda" />&nbsp;
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 81px; height: 24px">
                                    Pedido &nbsp;</td>
                                <td align="left" style="height: 24px">
                                    <div>
                                        &nbsp;
                                        <asp:TextBox ID="txtPedido" runat="server" Width="186px">0</asp:TextBox></div>
                                </td>
                                <td align="right" style="width: 105px; height: 24px">
                                    Hoja de Entrada</td>
                                <td align="left" style="width: 174px; height: 24px">
                                    <div>
                                        &nbsp;&nbsp;
                                        <asp:TextBox ID="txtHojaEntrada" runat="server" Width="186px">0</asp:TextBox></div>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 81px; height: 26px">
                                    Proveedor &nbsp;</td>
                                <td align="left" style="width: 141px; height: 26px">&nbsp;
                                    <asp:DropDownList ID="lstProveedor" runat="server">
                                        <asp:ListItem Value="OPO">Oportunidad-4310107</asp:ListItem>
                                        <asp:ListItem Value="DED">Dedicado -135508 </asp:ListItem>
                                    </asp:DropDownList></td>
                                <td align="right" style="width: 79px; height: 26px">
                                    &nbsp;</td>
                                <td align="right">
                                    <asp:Button ID="btnBloquear" runat="server" OnClick="btnBloquear_Click" Text="Actualizar" Visible="False" /></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            &nbsp;&nbsp;</p>
            </center>
    <div>
        <asp:UpdatePanel runat="server" ID="up">
            <ContentTemplate>
                 <br />
        <center>
            <p>
                <table style="width: 413px">
                    <tr>
                        <th align="left">
                            Factura</th>
                            <td colspan = '5' align="left">
                                <asp:TextBox ID="txtFactura" runat="server" CssClass="readOnlyTextBox"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th align="left" colspan="6">
                            Receptor Del Comprobante Fiscal</th>
                    </tr>
                    <tr>
                        <td colspan="6" align="left" style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid">
                            <table style="width: 389px">
                                <tr>
                                    <td align="left" style="width: 211px">
                                        <asp:Image ID="imgCliente" runat="server" ImageUrl="~/img/alerta.png" ToolTip="Campo vacío"
                                            Visible="False" /></td>
                                    <td align="left" style="width: 211px">
                                        &nbsp;<asp:TextBox ID="txtNombre" runat="server" CssClass="readOnlyTextBox" ReadOnly="True"
                                            TextMode="MultiLine" Width="340px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 211px; height: 21px">
                                        <asp:Image ID="imgDir" runat="server" ImageUrl="~/img/alerta.png" ToolTip="Vacio Campo"
                                            Visible="False" />
                                    </td>
                                    <td align="left" style="width: 211px; height: 21px">
                                        <asp:TextBox ID="txtDirección" runat="server" CssClass="readOnlyTextBox" ReadOnly="True"
                                            TextMode="MultiLine" Width="340px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 211px; height: 25px">
                                        <asp:Image ID="imgRFC" runat="server" ImageUrl="~/img/alerta.png" ToolTip="Vacio Campo"
                                            Visible="False" /></td>
                                    <td align="left" style="width: 211px; height: 25px">
                                        <asp:TextBox ID="txtRFC" runat="server" CssClass="readOnlyTextBox" ReadOnly="True"></asp:TextBox></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <th style="width: 41px">
                            Cantidad</th>
                        <th style="width: 116px">
                            Unidad</th>
                        <th  style="width: 457px">
                            Descripción</th>
                        <th>
                            Precio</th>
                        <th>
                            Importe</th>
                    </tr>
                    <tr>
                        <td colspan="5" style="border-left: black thin solid" align="left">
                            <asp:PlaceHolder ID="conceptosLiv" runat="server"></asp:PlaceHolder>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 41px; border-left: black thin solid; height: 98px;">
                            </td>
                        <td style="width: 116px; height: 98px">
                            </td>
                        <td  style="width: 457px; height: 98px;" align="left" >
                            <asp:TextBox ID="txtDescripcion" runat="server" Width="341px" Height="51px" TextMode="MultiLine" CssClass="readOnlyTextBox"></asp:TextBox><br />
                            <br />
                            </td>
                        <td style="height: 98px">
                            </td>
                        <td style="border-right: black thin solid; height: 98px;">
                            </td>
                    </tr>
                    <tr>
                        <th colspan="3" style="height: 22px">
                        </th>
                        <th colspan="3" style="height: 22px">
                            Importe</th>
                    </tr>
                    <tr>
                        <td colspan="3" style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid">
                            <table style="width: 503px">
                                <tr>
                                    <td align="left" style="width: 129px; height: 20px">
                                        Forma de Pago:</td>
                                    <td align="left" style="height: 20px">
                                        <asp:TextBox ID="txtFormaPago" runat="server" Width="217px" CssClass="readOnlyTextBox"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 129px; height: 20px">
                                        Método de Pago:</td>
                                    <td align="left" style="height: 20px">
                                        <asp:TextBox ID="txtMetodoPago" runat="server" Width="215px" CssClass="readOnlyTextBox"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 129px">
                                        Condiciones de Pago:</td>
                                    <td align="left">
                                        <asp:TextBox ID="txtCondicionesPago" runat="server" Width="117px" CssClass="readOnlyTextBox"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 129px">
                                        No. Cuenta de Pago:</td>
                                    <td align="left">
                                        <asp:TextBox ID="txtCuenta" runat="server" Width="71px" CssClass="readOnlyTextBox"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 129px">
                                        Importe con letra:</td>
                                    <td align="left">
                                        <asp:TextBox ID="txtCantidadLetra" runat="server" Width="357px" CssClass="readOnlyTextBox"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 129px">
                                        Lugar de Expedición:</td>
                                    <td align="left">
                                        <asp:TextBox ID="TextBox11" runat="server" CssClass="readOnlyTextBox"></asp:TextBox></td>
                                </tr>
                            </table>
                        </td>
                        <td colspan="2" style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid">
                            <table style="width: 223px; height: 112px">
                                <tr>
                                    <td align="left" style="width: 89px; height: 42px;">
                                        Subtotal</td>
                                    <td align="right" style="height: 42px">
                                        <asp:TextBox ID="txtSubtotal" runat="server" Width="107px" CssClass="readOnlyTextBox" OnTextChanged="txtSubtotal_TextChanged"> </asp:TextBox></td>
                                    <td align="right" style="height: 42px">
                                        <asp:Image ID="imgSubtotal" runat="server" ImageUrl="~/img/alerta.png" ToolTip="Campo vacío"
                                            Visible="False" /></td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 89px; height: 41px;">
                                        IVA 16%</td>
                                    <td align="right" style="height: 41px">
                                        <asp:TextBox ID="txtIVA" runat="server" Width="107px" CssClass="readOnlyTextBox"></asp:TextBox></td>
                                    <td align="right" style="height: 41px">
                                        <asp:Image ID="imgIva" runat="server" ImageUrl="~/img/alerta.png" ToolTip="Campo vacío"
                                            Visible="False" /></td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 89px; border-bottom: black thin solid; height: 49px;">
                                        IVA RETENIDO</td>
                                    <td align="right" style="border-bottom: black thin solid; height: 49px">
                                        <asp:TextBox ID="txtRetencion" runat="server" Width="107px" CssClass="readOnlyTextBox"></asp:TextBox></td>
                                    <td align="right" style="border-bottom: black thin solid; height: 49px">
                                        <asp:Image ID="imgRetencion" runat="server" ImageUrl="~/img/alerta.png" ToolTip="Campo vacío"
                                            Visible="False" /></td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 89px; height: 21px;">
                                        Total</td>
                                    <td align="right" style="height: 21px">
                                        <asp:TextBox ID="txtTotal" runat="server" Width="107px" CssClass="readOnlyTextBox"></asp:TextBox></td>
                                    <td align="right" style="height: 21px">
                                        <asp:Image ID="imgTotal" runat="server" ImageUrl="~/img/alerta.png" ToolTip="Campo vacío"
                                            Visible="False" /></td>
                                </tr>
                            </table>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </p>
            <p>
                &nbsp;</p>
        
        
    <asp:Button ID="btnEdit" runat="server" Text="Editar" OnClick="btnEdit_Click" CausesValidation="False" UseSubmitBehavior="False" />
    <asp:Button ID="btnGuardar" runat="server" Text="Facturar CFDi" OnClick="btnGuardar_Click" /><br />  
    </center>
              
             <center>
                 &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                 
                 &nbsp;&nbsp;</center>
                
                 
            </ContentTemplate>
            
            <Triggers>
        <asp:AsyncPostBackTrigger ControlID= "btnGuardar" />
        <asp:AsyncPostBackTrigger ControlID = "btnEdit" />
        
    </Triggers>
        </asp:UpdatePanel>
    </div>
    
    <center>
        &nbsp;<asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                 <Msg:PopUpMsg ID='PopupMsg' runat='server'/>
                &nbsp;
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID = "btnGuardar" />
                <asp:AsyncPostBackTrigger ControlID = "btnEdit" />
            </Triggers>
        </asp:UpdatePanel>
        &nbsp;
    </center>
    <p>
        
        &nbsp;</p>
        </asp:Content>

        </div>
    </form>
</body>
</html>
