<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DetallesFacturasMasivas.aspx.cs" Inherits="TIFacturasMasivas.DetallesFacturasMasivas" %>

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
    <asp:HyperLink ID="linkGeneracionMasiva" runat="server" NavigateUrl="GeneracionMasiva.aspx" Target="_parent">Generación Masiva de CFDi</asp:HyperLink>&nbsp;
    <br />
    <center>   
            
        <h1>
            <asp:Label ID="lblTipo" runat="server" Text="Label"></asp:Label>
            <asp:Label ID="lblFact" runat="server" Text="Label"></asp:Label>
    <br />
    <br />
    <br />
            <asp:PlaceHolder ID="estatusFactura" runat="server"></asp:PlaceHolder></h1>
            </center>
            <center>
     <asp:Panel ID="Adenda" runat="server" Height="134px" Visible="False">
     
         <asp:UpdatePanel ID="UpdatePanel1" runat="server">
             <ContentTemplate>
                 <table style="border-left-color: #000000; border-bottom-color: #000000; width: 644px;
                     border-top-style: solid; border-top-color: #000000; border-right-style: solid;
                     border-left-style: solid; border-right-color: #000000; border-bottom-style: solid; height: 100px;">
                     <tr>
                         <td colspan="4" style="height: 24px">
                             <div>
                                 &nbsp;Complemento Liverpool</div>
                         </td>
                     </tr>
                     <tr>
                         <td align="right" style="width: 81px; height: 24px">
                             Pedido &nbsp;</td>
                         <td align="left" style="height: 24px">
                             <div>
                                 &nbsp;
                             <asp:TextBox ID="txtPedido" runat="server" Width="186px"></asp:TextBox></div>
                         </td>
                         <td align="right" style="width: 105px; height: 24px">
                             Hoja de Entrada</td>
                         <td align="left" style="width: 174px; height: 24px">
                             <div>
                                 &nbsp;&nbsp;
                             <asp:TextBox ID="txtHojaEntrada" runat="server" Width="186px"></asp:TextBox></div>
                         </td>
                     </tr>
                     <tr>
                         <td align="right" style="width: 81px; height: 26px">
                         </td>
                         <td align="left" style="width: 141px; height: 26px">
                         </td>
                         <td align="right" style="width: 79px; height: 26px">
                             &nbsp;</td>
                         <td align="right"><asp:Button ID="btnBloquear" runat="server" Text="Bloquear" OnClick="btnBloquear_Click" /></td>
                     </tr>
                 </table>
             </ContentTemplate>
         </asp:UpdatePanel>
         
         </asp:Panel>
         </center>
         
    <asp:Panel ID="detalle" runat="server" Visible="false">
     <div id="content">
            <asp:Panel ID="Panel2" runat="server" CssClass="collapsePanelHeader" Height="30px">
                <div style="padding-right: 5px; padding-left: 5px; padding-bottom: 5px; vertical-align: middle;
                    cursor: pointer; padding-top: 5px">
                    <div style="float: left">
                        Detalle Conceptos Factura</div>
                    <div style="float: left; margin-left: 20px">
                        <asp:Label ID="Label1" runat="server">(Show Details...)</asp:Label>
                    </div>
                    <div style="float: right; vertical-align: middle">
                        <asp:ImageButton ID="Image1" runat="server" AlternateText="(Show Details...)" ImageUrl="~/img/expand_blue.jpg" />
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="Panel1" runat="server" CssClass="collapsePanel" Height="0">
                <br />
                <p>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID= "btnEditaDetalle" />
                        
                    </Triggers>
                    </asp:UpdatePanel>
                    <p>
                    </p>
                    <center>
                        <asp:Button ID="btnEditaDetalle" runat="server" CausesValidation="False" OnClick="btnEditaDetalle_Click" Text="Editar" UseSubmitBehavior="False" />
                        <asp:Button ID="btnGuardaDetalle" runat="server" CausesValidation="False" OnClick="btnGuardaDetalle_Click" Text="Guardar" UseSubmitBehavior="False" />
                    </center>
                    <p>
                    </p>
                    <p>
                    </p>
                    <p>
                    </p>
                </p>
            </asp:Panel>
            <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server"
            TargetControlID="Panel1"
        ExpandControlID="Panel2"
        CollapseControlID="Panel2" 
        Collapsed="True"
        TextLabelID="Label1"
        ImageControlID="Image1"    
        ExpandedText="(Ocultar detalles...)"
        CollapsedText="(Mostrar detalles...)"
        ExpandedImage="~/img/collapse_blue.jpg"
        CollapsedImage="~/img/expand_blue.jpg"
        SuppressPostBack="true">
            </cc1:CollapsiblePanelExtender>
            </div>
     </asp:Panel>

    <div>
        <asp:UpdatePanel runat="server" ID="up">
            <ContentTemplate>
                <br />
        <center>
            <p>
                <table style="position: relative"  >
                    <tr>
                    <th >Factura/Carta Porte</th> <td style="border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid"> 
                        <asp:TextBox ID="txtFactura" runat="server" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                    <th >Fecha Expedición</th> <td style="border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid; width: 187px;"> 
                        <asp:TextBox ID="txtFecha" runat="server" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th colspan="2" style="border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid;" align="left">
                            Cliente</th>
                        <th style="border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid;" align="left" >Folio Fiscal</th>
                        <td style="border-right: #000000 thin solid; border-top: #000000 thin solid; width: 187px;">  </td>
                    </tr>
                    <tr>
                        <td colspan="2" rowspan = "5" align="left" style="border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid"> 
                         <table style="width: 389px">
                            <tr>
                                <td align="left" style="width: 211px"><asp:Image ID="imgCliente" runat="server" ToolTip = "Campo vacío" ImageUrl="~/img/alerta.png" Visible="False" /></td>
                                <td align="left" style="width: 211px">
                                    &nbsp;<asp:TextBox ID="txtNombre" runat="server" TextMode="MultiLine" Width="340px" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 211px; height: 21px">
                                    <asp:Image ID="imgDir" runat="server" ToolTip = "Vacio Campo" ImageUrl="~/img/alerta.png" Visible="False" /> </td>
                                <td style="width: 211px; height: 21px;" align="left">
                                    <asp:TextBox ID="txtDirección" runat="server" TextMode="MultiLine" Width="340px" ReadOnly="True" CssClass = "readOnlyTextBox"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 211px; height: 25px"><asp:Image ID="imgRFC" runat="server" ToolTip = "Vacio Campo" ImageUrl="~/img/alerta.png" Visible="False" /></td>
                                <td style="width: 211px; height: 25px" align="left">
                                    <asp:TextBox ID="txtRFC" runat="server" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                            </tr>
                        </table>                     
                        <br />
                        </td>
                        <th align="left" style="border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid">Fecha de Certificación</th>
                        <td style="border-right: #000000 thin solid; width: 187px;">   </td>
                    </tr>
                    <tr>
                        <th  align="left" style="border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid" > No. Certificado</th>
                        <td style="border-right: #000000 thin solid; width: 187px;">    </td>
                    </tr>
                    <tr>
                        <th  align="left" style="border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid" > No. Certificado SAT</th>
                        <td style="border-right: #000000 thin solid; width: 187px;" >    </td>
                    </tr>
                    <tr>
                        <th align="left" style="border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid" >Regimen Fiscal</th>
                        <td style="border-right: #000000 thin solid; width: 187px;">    </td>
                    </tr>
                     <tr>
                        <th  align="left" style="border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid;" >Lugar de Expedición</th>
                        <td style="border-right: #000000 thin solid; border-bottom:#000000 thin solid; width: 187px;">    </td>
                    </tr>
                    <tr>
                        <th colspan = "2" style="border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid;" align="left">Origen</th>
                        <th colspan = "2" align="left" style="border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid">Destino</th>
                    </tr>
                    <tr>
                    <td colspan = "2">
                    <table style="height: 140px; border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid; width: 211px">
                        <tr>
                        <td style="width: 366px">
                            <asp:TextBox ID="txtOrigen" runat="server" Width="350px" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                        </tr>
                        <tr>
                        <td style="width: 366px">
                            <asp:TextBox ID="txtOrigenRemitente" runat="server" Width="350px" TextMode="MultiLine" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                        </tr>
                        <tr>
                        <td style="width: 366px">
                            <asp:TextBox ID="txtDomicilioOrigen" runat="server" Width="350px" TextMode="MultiLine" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                        </tr>
                        <tr>
                        <td style="width: 366px; height: 26px;">
                            <asp:TextBox ID="txtRFCOrigen" runat="server" Width="350px" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                        </tr>
                    </table>
                    <br />
                    </td>
                    <td colspan = "2">
                     <table style="height: 140px; border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid;">
                        <tr>
                        <td style="width: 3px" align="left">
                            <asp:TextBox ID="txtDestino" runat="server" Width="350px" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                        </tr>
                        <tr>
                        <td style="width: 3px" align="left">
                            <asp:TextBox ID="txtDestinatario" runat="server" Width="350px" TextMode="MultiLine" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                        </tr>
                        <tr>
                        <td style="width: 3px" align="left">
                            <asp:TextBox ID="txtDomicilioDestino" runat="server" Width="350px" TextMode="MultiLine" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                        </tr>
                        <tr>
                        <td style="width: 3px;" align="left">
                            <asp:TextBox ID="txtRFCDestino" runat="server" Width="350px" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                        </tr>
                    </table>
                    <br />
                    </td>
                    </tr>
                    <tr>
                    <th style="width: 187px">Valor Unitario, Couta convenida por Tonelada o Carga Fraccionada</th>
                    <th style="width: 174px">Valor comercial Declarado</th>
                    <th>No. Remisión</th>
                    <th style="width: 187px">Condiciones de Pago</th>
                    </tr>
                    <tr>
                    <td align="center" style="border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid; width: 187px;">
                        <asp:TextBox ID="txtCuotaConvenida" runat="server" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                    <td align="center" style="border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid; width: 174px;">
                        <asp:TextBox ID="txtValorComercial" runat="server" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                    <td align="center" style="border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid;">
                        <asp:TextBox ID="txtRemisión" runat="server" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                    <td align="center" style="border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid; width: 187px;">
                        <asp:TextBox ID="txtCondicionesPago" runat="server" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                    </tr>
                    <tr>
                    <td colspan = "4">
                    <br />
                    <table style="width: 728px; height: 442px;">
                    <tr>
                    <td style="width: 520px">
                    
                    <table style="width: 524px; height: 204px">
                    <tr>
                        <th colspan = "2">Bultos</th>
                        <th rowspan = "2" style="width: 204px">Se Dice Que Contienen</th>
                        <th rowspan = "2">Peso</th>
                        <th colspan = "2">Volumen</th>                      
                    </tr>
                    <tr>
                        <th style="width: 42px">No.</th>
                        <th style="width: 26px">Embalaje</th>
                        <th style="width: 35px">Mtrs3</th>
                        <th style="width: 32px">P Estimado</th>
                    </tr>
                    <tr>
                        <td style="width: 42px; height: 140px; border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid;"></td>
                        <td style="width: 26px; height: 140px; border-bottom: #000000 thin solid;"></td>
                        <td style="width: 204px; height: 140px; border-right: #000000 thin solid; border-top: #000000 thin solid; border-bottom: #000000 thin solid;" >
                        <br />
                            <table style="width: 260px; height: 98px">
                                <tr>
                                    <td style="width: 5px; height: 26px;">
                                        <asp:TextBox ID="txtContine" runat="server" Width="250px" TextMode="MultiLine" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 5px">
                                        <asp:TextBox ID="txtComentarios" runat="server" Width="250px" ReadOnly="True" CssClass="readOnlyTextBox" TextMode="MultiLine"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 5px; height: 26px;">
                                        <asp:TextBox ID="txtAmparaRemisiones" runat="server" Width="250px" ReadOnly="True" CssClass="readOnlyTextBox" TextMode="MultiLine"></asp:TextBox></td>
                                </tr>
                            </table>
                        </td>
                        <td colspan = "2" align="center" style="height: 140px; border-bottom: #000000 thin solid;">
                            <asp:TextBox ID="txtPeso" runat="server" Width="65px" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                        <td style="width: 32px; height: 140px;border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid;"></td>
                    </tr>
                    </table>
                    <br />
                    
                    </td>
                    <td rowspan = "3">
                        <table style="height: 433px; width: 205px;" >
                            <tr><th style="width: 80px; height: 27px;">Concepto</th><th style="width: 83px; height: 27px;" align="center">Importe</th>
                                <td align="center" style="width: 83px; height: 27px">
                                </td>
                            </tr>
                            <tr><th style="width: 80px">Flete</th><td style="width: 83px; border-right: #000000 thin solid;" align="center">
                                <asp:TextBox ID="txtFlete" runat="server" Width="70px" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                                <td align="center" style="width: 83px">
                                </td>
                            </tr>
                            <tr><th style="width: 80px">Carga por Seguro</th><td style="width: 83px; border-right: #000000 thin solid;" align="center">
                                <asp:TextBox ID="txtCargaXSeguro" runat="server" Width="70px" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                                <td align="center" style=" width: 83px">
                                </td>
                            </tr>
                            <tr><th style="width: 80px">Maniobras</th><td style="width: 83px; border-right: #000000 thin solid;" align="center">
                                <asp:TextBox ID="txtManiobras" runat="server" Width="70px" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                                <td align="center" ></td>
                            </tr>
                            <tr><th style="width: 80px; height: 49px;">Autopistas Libramientos Transbordadores</th><td style="width: 83px; border-right: #000000 thin solid; height: 49px;" align="center">
                                <asp:TextBox ID="txtAutopistas" runat="server" Width="70px" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                                <td align="center" style="width: 83px; height: 49px">
                                </td>
                            </tr>                         
                            <tr><th style="width: 80px">Renta Equipo</th><td style="width: 83px; border-right: #000000 thin solid;" align="center">
                                <asp:TextBox ID="txtEquipo" runat="server" Width="70px" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                                <td align="center" style=" width: 83px">
                                </td>
                            </tr>        
                            <tr><th style="width: 80px">CPAC</th><td style="width: 83px; border-right: #000000 thin solid;" align="center">
                                <asp:TextBox ID="txtCPAC" runat="server" Width="70px" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                                <td align="center" style=" width: 83px">
                                </td>
                            </tr>
                            <tr><th style="width: 80px">Otros</th><td style="width: 83px; border-right: #000000 thin solid;" align="center">
                                <asp:TextBox ID="txtOtros" runat="server" Width="70px" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                                <td align="center" style=" width: 83px">
                                </td>
                            </tr>
                            <tr><th style="width: 80px">Subtotal</th><td style="width: 83px; border-right: #000000 thin solid;" align="center">
                                <asp:TextBox ID="txtSubtotal" runat="server" Width="70px" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                                <td align="center" style=" width: 83px">
                                    <asp:Image ID="imgSubtotal" runat="server" ToolTip = "Vacio Campo" ImageUrl="~/img/alerta.png" Visible="False" /></td>
                            </tr>
                            <tr><th style="width: 80px">IVA%16.0</th><td style="width: 83px; border-right: #000000 thin solid;" align="center">
                                <asp:TextBox ID="txtIVA" runat="server" Width="70px" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                                <td align="center" style=" width: 83px">
                                    <asp:Image ID="imgIva" runat="server" ToolTip = "Vacio Campo" ImageUrl="~/img/alerta.png" Visible="False" /></td>
                            </tr>
                            <tr><th style="width: 80px">Retención 4%</th><td style="width: 83px; border-right: #000000 thin solid; " align="center">
                                <asp:TextBox ID="txtRetencion" runat="server" Width="70px" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                                <td align="center" style=" width: 83px">
                                    <asp:Image ID="imgRetencion" runat="server" ToolTip = "Vacio Campo" ImageUrl="~/img/alerta.png" Visible="False" /></td>
                            </tr>
                            <tr><th style="width: 80px; height: 28px;">Total</th><td style="width: 83px; height: 28px; border-right: #000000 thin solid; border-bottom: #000000 thin solid;" align="center">
                                <asp:TextBox ID="txtTotal" runat="server" Width="70px" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                                <td align="center" style=" width: 83px; height: 28px">
                                    <asp:Image ID="imgTotal" runat="server" ToolTip = "Vacio Campo" ImageUrl="~/img/alerta.png" Visible="False" /></td>
                            </tr>
                        </table>
                    </td>
                    </tr>
                    <tr>
                    <td style="width: 520px">
                        <table>
                            <tr>
                                <td style="width: 133px; border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid;">Orden:<asp:TextBox ID="txtOrden" runat="server" Width="50px" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                                <td style="width: 124px; border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid;">MB:<asp:TextBox ID="txtMb" runat="server" Width="50px" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                                <td style="width: 140px; border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid;">Invoice:<asp:TextBox ID="txtInvoice" runat="server" Width="50px" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                                <td style="width: 141px; border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid;">Movimiento:<asp:TextBox ID="txtMovimiento" runat="server" Width="50px" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                            </tr>
                        </table>
                    </td>
                    </tr>
                    <tr>
                    <td style="width: 520px; height: 169px;">
                        <br />
                        <table style="height: 134px">
                            <tr>
                                <th colspan = "3">
                                Nombre Operador
                                </th>
                                <th colspan = "3">
                                Número Licencia
                                </th>
                            </tr>
                            <tr>
                            <td colspan = "3" style="border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid;">
                                <asp:TextBox ID="txtOperador" runat="server" ReadOnly="True" CssClass="readOnlyTextBox" Width="217px"></asp:TextBox></td>
                            <td colspan = "3" style="border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid;">
                                <asp:TextBox ID="txtLicencia" runat="server" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <th colspan = "2">Tractor</th>
                                <th colspan = "2">Remolque 1</th>
                                <th colspan = "2">Remolque 2</th>
                            </tr>
                            <tr>
                                <th style="width: 86px">No. Eco.</th>
                                <th style="width: 86px">Placas</th>
                                <th style="width: 86px">No. Eco.</th>
                                <th style="width: 86px">Placas</th>
                                <th style="width: 86px">No. Eco.</th>
                                <th style="width: 86px">Placas</th>
                            </tr>
                            <tr>
                                <td style="border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid;" align="center">
                                    <asp:TextBox ID="txtTractorEco" runat="server" Width="50px" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                                <td style="border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid;" align="center">
                                    <asp:TextBox ID="txtTractorPlacas" runat="server" Width="50px" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                                <td style="border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid;" align="center">
                                    <asp:TextBox ID="txtRemol1Eco" runat="server" Width="50px" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                                <td style="border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid;" align="center">
                                    <asp:TextBox ID="txtRemolque1Placas" runat="server" Width="50px" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                                <td style="border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid;" align="center">
                                    <asp:TextBox ID="txtRemolque2Eco" runat="server" Width="50px" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                                <td style="border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid;" align="center">
                                    <asp:TextBox ID="txtRemolque2Placas" runat="server" Width="50px" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                            </tr>
                            
                        </table>
                    </td>
                    </tr>
                    </table>
                    
                    </td>
                    </tr>
                    <tr>
                    <th colspan = "2">Documento</th>
                    <th colspan = "2">Recibí de Conformidad</th>
                    </tr>
                    <tr>
                    <td colspan = "2" style="border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid;" align="center">
                        <asp:TextBox ID="txtDocumento" runat="server" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                    <td colspan = "2" style="border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid;" align="center"></td>
                    </tr>
                    <tr>
                    <td colspan = "4" style="border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid; height: 23px;" align="left">Cantidad con Letra:<asp:TextBox ID="txtCantidadLetra" runat="server" Width="605px" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox></td>
                    </tr>
                     <tr>
                     <td colspan = "4" align="left">
                     <br /><br />
                     DEBO(EMOS) Y PAGARE(EMOS) INCONDICIONALMENTE SIN PRETEXTO, ESTE PAGARE A LA ORDEN DE TDR TRANSPORTES, S.A. DE C.V. EN LA PLAZA DE EMISION DEL DIA
                         <asp:TextBox ID="txtFechaPagare" runat="server" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox>LA CANTIDAD DE
                         <asp:TextBox ID="txtTotal2" runat="server" ReadOnly="True" CssClass="readOnlyTextBox" Width="161px"></asp:TextBox>
                         IMPORTE CON LETRA
                         <asp:TextBox ID="txtCantidadLetra2" runat="server" ReadOnly="True" CssClass="readOnlyTextBox" Width="417px"></asp:TextBox>
                         VALOR DECLARADO A MI (NUESTRA) ENTERA SATISFACCION. 170. 1/3 Y RELATIVOS CAUSANDO
                         UN INTERES MORATORIO DE
                         <asp:TextBox ID="txtInteres" runat="server" ReadOnly="True" CssClass="readOnlyTextBox"></asp:TextBox>
                         MENSUAL<br />
                         (DEUDOR)<asp:TextBox ID="txtClientePagare" runat="server" ReadOnly="True" CssClass="readOnlyTextBox" Width="385px"></asp:TextBox>
                         &nbsp; &nbsp; &nbsp; &nbsp; FIRMA ______________________</td>
                     </tr>
                     <tr>
                     <td colspan = "4" style="height: 26px" align="left">
                         <asp:TextBox ID="txtFormaPago" runat="server" ReadOnly="True" CssClass="readOnlyTextBox" Width="209px"></asp:TextBox>&nbsp;
                         <asp:TextBox ID="txtCondicionesPago2" runat="server" ReadOnly="True" CssClass="readOnlyTextBox" Visible="False" Width="1px"></asp:TextBox>
                         Metodo de Pago:
                         <asp:TextBox ID="txtMetodoPago" runat="server" ReadOnly="True" CssClass="readOnlyTextBox" Width="233px" OnTextChanged="txtMetodoPago_TextChanged"></asp:TextBox>
                         &nbsp;<br />
                         <br />
                         <br />
                         UUID Relacionado:
                         <asp:TextBox ID="txtUUIDREL" runat="server" ReadOnly="True" Width="261px" CssClass="readOnlyTextBox"></asp:TextBox>&nbsp;Tipo Relación:
                         <asp:TextBox ID="txtRelacion" runat="server" CssClass="readOnlyTextBox" ReadOnly="True" Width="261px"></asp:TextBox>
                         </td>
                     </tr>
                     
                   
                    
                </table>
            </p>
            <p>
                &nbsp;</p>
        
        
    <asp:Button ID="btnEdit" runat="server" Text="Editar" OnClick="btnEdit_Click" CausesValidation="False" UseSubmitBehavior="False" />
    <asp:Button ID="btnGuardar" runat="server" Text="Facturar CFDi" OnClick="btnGuardar_Click"  /></center>
              
             <center>
                 
                 &nbsp;&nbsp;</center>
                
                 
            </ContentTemplate>
            
            <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnEdit" />
        <asp:AsyncPostBackTrigger ControlID= "btnGuardaDetalle" />
        <asp:AsyncPostBackTrigger ControlID= "btnGuardar" />
        
    </Triggers>
        </asp:UpdatePanel>
    </div>
    
    <center>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                 <Msg:PopUpMsg ID='PopupMsg' runat='server'/>
            </ContentTemplate>
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
