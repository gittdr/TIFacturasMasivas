<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="TIFacturasMasivas.Main" %>

<!DOCTYPE html>

<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, shrink-to-fit=no">
     <title>TDR | Complementos de Pagos V2.0</title>
    <link rel="shortcut icon" href="images/icono-tdr-soluciones-logisticas.ico" />
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
        .mGrid {
    width: 100%;
    background-color: #fff;
    margin: 5px 0 10px 0;
    border: solid 1px #525252;
    border-collapse: collapse;
    text-align:center;
}

    .mGrid td {
        padding: 2px;
        border: solid 1px #c1c1c1;
        color: #717171;
    }

    .mGrid th {
        padding: 4px 2px;
        color: #fff;
        background: #424242 url(grd_head.png) repeat-x top;
        border-left: solid 1px #525252;
        font-size: 0.9em;
    }

    .mGrid .alt {
        background: #fcfcfc url(grd_alt.png) repeat-x top;
    }

    .mGrid .pgr {
        background: #424242 url(grd_pgr.png) repeat-x top;
    }

        .mGrid .pgr table {
            margin: 5px 0;
        }

        .mGrid .pgr td {
            border-width: 0;
            padding: 0 6px;
            border-left: solid 1px #666;
            font-weight: bold;
            color: #fff;
            line-height: 12px;
        }

        .mGrid .pgr a {
            color: #666;
            text-decoration: none;
        }

            .mGrid .pgr a:hover {
                color: #000;
                text-decoration: none;
            }
        html{
            scroll-behavior: smooth;
        }
        .mitabla {
            width :100%
        }
        body {
  height: 100%;
  
}

#page-content {
  flex: 1 0 auto;
}
        /*body {margin: 0; background: #181824; font-family: Arial; }
nav {
  position: fixed;
  width: 100%;
  max-width: 300px;
  bottom: 0; top: 0;
  display: block;
  min-height: 300px;
  height: 100%;
  color: #fff;
  opacity: 0.8;
  transition: all 300ms;
  -moz-transition: all 300ms;
  -webkit-transition: all 300ms;
}
nav .vertical-menu hr{opacity: 0.1; border-width: 0.5px;}
nav ul{width: 90%; padding-inline-start: 0; margin: 10px; height: calc(100% - 20px); }
nav .vertical-menu-logo{padding: 20px; font-size: 1.3em; position: relative}
nav .vertical-menu-logo .open-menu-btn{width: 30px; height: max-content; position: absolute; display: block; right: 20px; top: 0; bottom: 0; margin: auto; cursor: pointer;}
nav .vertical-menu-logo .open-menu-btn hr{margin: 5px 0}
nav li{list-style: none; padding: 10px 10px; cursor: pointer;}
nav li:hover{ color: rgba(75, 105, 176,1); }
nav li#user-info{position: absolute; bottom: 0; width: 80%;}
nav li#user-info > span{display: block; float: right; font-size: 0.9em; position: relative; opacity: 0.6;}
nav li#user-info > span:after{
  content: '';
  width: 12px;
  height: 12px;
  display: block;
  position: absolute;
  background: green;
  left: -20px; top: 0; bottom: 0;
  margin: auto;
  border-radius: 50%;
}
.content-wrapper{
  width: calc(100% - 300px);
  height: 100%;
  position: fixed;
  background: #fff;
  left: 300px;
  padding: 20px;
}
.closed-menu .content-wrapper{
  width: 100%;
  left: 50px;
}
.content-wrapper{
    transition: all 300ms;
}
.vertical-menu-wrapper .vertical-menu-logo div{transition: all 100ms;}
.closed-menu .vertical-menu-wrapper .vertical-menu-logo div{
  margin-left: -100px;
}
.vertical-menu-wrapper .vertical-menu-logo .open-menu-btn{transition: all 300ms;}
.closed-menu .vertical-menu-wrapper .vertical-menu-logo .open-menu-btn{
  left: 7px;
  right: 100%;
}

.closed-menu .vertical-menu-wrapper ul,.closed-menu .vertical-menu-wrapper hr{margin-left: -300px;}
.vertical-menu-wrapper ul, .vertical-menu-wrapper hr{transition: all 100ms;}*/
.content-wrapper{background: #ebebeb;}
.content{
  width: 90%;
  min-height: 90%;
  background: #fff;
  border-radius: 10px;
  padding: 30px;
  z-index: 1900;
}
#divLoading {
    -moz-animation: cssAnimation 0s ease-in 3s forwards;
    /* Firefox */
    -webkit-animation: cssAnimation 0s ease-in 3s forwards;
    /* Safari and Chrome */
    -o-animation: cssAnimation 0s ease-in 3s forwards;
    /* Opera */
    animation: cssAnimation 0s ease-in 3s forwards;
    -webkit-animation-fill-mode: forwards;
    animation-fill-mode: forwards;
}
@keyframes cssAnimation {
    to {
        width:0;
        height:0;
        overflow:hidden;
    }
}
@-webkit-keyframes cssAnimation {
    to {
        width:0;
        height:0;
        visibility:hidden;
    }
}

    </style>
</head>

<body style="background: rgb(238,244,247);">
     <form id="form1" runat="server">
    <nav class="navbar navbar-expand-lg navbar-dark bg-dark" style="background:rgba(0, 8, 20, 0.9) !important;">
  <a class="navbar-brand" href="#">
      <img src="img/logo.png" /></a>

  <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
    <span class="navbar-toggler-icon"></span>
  </button>
        
  <div class="collapse navbar-collapse" id="navbarNav">
     <ul class="navbar-nav mr-auto">
         <li class="nav-item active">
        <asp:HyperLink ID="HyperLink3" CssClass="text-white" Style="text-decoration:none; padding-right: 20px;"  runat="server" NavegateUrl="Main.aspx" NavigateUrl="~/Main.aspx"><b><i class="fa fa-check-circle" style="color:#f2c43e" aria-hidden="true"></i> Facturación masiva </b></asp:HyperLink>
      </li>
     
    </ul>
    <%--<ul class="navbar-nav mr-auto ml-auto">
       
      <li class="nav-item active m-auto">
        <a class="nav-link" href="#"><h3>Complementos de Pago</h3><span class="sr-only">(current)</span></a>
      </li>
    </ul>--%>
      <ul class="navbar-nav">
            
            <li class="nav-item active text-white">
                <asp:HyperLink ID="HyperLink2" CssClass="btn btn-outline-warning text-white"  runat="server" NavegateUrl="Inicial.aspx" NavigateUrl="~/Inicial.aspx"><b><i class="fa fa-chevron-circle-left" aria-hidden="true"></i> Regresar</b></asp:HyperLink>
              
            </li>
          </ul>
     
      
     
  </div>
             
</nav>
    <asp:ScriptManager ID="ScriptManager1" AsyncPostBackTimeOut="360000" runat="server"></asp:ScriptManager>
    
    <div class="highlight-blue">
        <div class="container-fluid mt-4">
                 <div class="card" style="box-shadow: 10px 10px 55px 0px rgba(5,5,5,0.36);-webkit-box-shadow: 10px 10px 55px 0px rgba(5,5,5,0.36);-moz-box-shadow: 10px 10px 55px 0px rgba(5,5,5,0.36);">
                  <div class="card-header">
                    <b>Generación Masiva de Facturas</b>
                  </div>
                  <div class="card-body">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="form-row">
                                <div class="form-group col-sm-12">
                                 
                                    <asp:UpdatePanel runat="server" ID="up">
                                        <ContentTemplate>
                                            <table class="table">
                                                <tr>
                                                    <td colspan="4">
                                                        <div>
                                                            <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width:10%">
                                                         <label for="FileUpload1"><b>Cliente</b></label>
                                                        
                                                    </td>
                                                    <td style="width:90%">
                                                        <div class="form-group">
                                                            <asp:DropDownList CssClass="form-control" ID="lstCliente" runat="server" >
                                                            </asp:DropDownList>
                                                           
                                                        </div>
                                                    </td>
                                                     <td style="width:10%">
                                                         <label for="FileUpload2"><b>Elaborada</b></label>
                                                        
                                                    </td>
                                                    <td style="width:90%">
                                                        <div class="form-group">
                                                            <asp:DropDownList ID="lstElaborada" CssClass="form-control" runat="server">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" style="text-align:right">
                                                        <asp:Button CssClass="btn btn-primary" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" /><asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                                            <ProgressTemplate>
                                                                <asp:Image ID="Image2" runat="server"  ImageUrl="img/indicator.gif" />
                                                                <asp:Label ID="lblProgress" runat="server" Text="Generando Busqueda....."></asp:Label>
                                                            </ProgressTemplate>
                                                        </asp:UpdateProgress>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                     <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                         <ContentTemplate>
                                             <table class="table">
                                                 <tr>
                                                     <td>
                                                         <div class="form-check">
                                                         <asp:CheckBox ID="chkSelectAll" CssClass="form-check-input" runat="server" Text=" Seleccionar todos" AutoPostBack="True" OnCheckedChanged="chkSelectAll_CheckedChanged" /><br />
                                                         </div>
                                                             <center>
                                                                <asp:PlaceHolder ID="gridPlace" runat="server"></asp:PlaceHolder>
                                                            </center>
                                                     </td>
                                                 </tr>
                                             </table>
                                         </ContentTemplate>
                                          <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnBuscar" /> 
                                                <asp:AsyncPostBackTrigger ControlID="chkSelectAll" />    
                                          </Triggers>
                                     </asp:UpdatePanel>
                                    <table class="table">
                                        <tr>
                                            <td style="text-align:center">
                                               
                                               <asp:Button ID="btnGuarda" CssClass="btn btn-success" runat="server" OnClick="Button1_Click" Text="Generar CFDi" CausesValidation="False"  /><br />
                                                    
                                            </td>
                                        </tr>
                                    </table>
                                     
                                </div>
                                <div class="form-group col-sm-2">
                                  
                                </div>
                            </div>
                        </div>
                        <hr />
                        
                            
                            
                        

                    </div>
                      
                   
                  </div>

                </div>
            <div runat="server" id="divLoading" style="background-image:url(img/loading.gif);position:absolute;top:0;left:0;width:100%;height:100%;background-repeat:no-repeat;background-position:center;z-index:2000"></div>
        </div>
       
    </div>
    
  
       
    
   
    
    <footer id="sticky-footer" class="flex-shrink-0 py-4 bg-dark text-white-50" style="position: relative;
    margin-top: 50vh;background:rgba(0, 8, 20, 0.9) !important;">
    <div class="container text-center text-white">
        <a href="#form1" style="font-size:28px;text-decoration:none;color:white"><i class="fa fa-arrow-circle-up" aria-hidden="true"></i></a><br />
      <small>2022 Copyright &copy; TDR Soluciones Logísticas</small>
    </div>
  </footer>
       
         </form>
</body>

</html>
