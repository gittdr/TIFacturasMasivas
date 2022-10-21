<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Inicial.aspx.cs" Inherits="TIFacturasMasivas.Inicial" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>TDR | Generación Facturación Masiva</title>
    <link rel="shortcut icon" href="images/icono-tdr-soluciones-logisticas.ico" />
	<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" />
	<link rel="preconnect" href="https://fonts.googleapis.com">
<link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
     <script src="https://kit.fontawesome.com/789a3ce2b4.js" crossorigin="anonymous"></script>
<link href="https://fonts.googleapis.com/css2?family=Open+Sans:wght@300;400;500&family=Roboto:ital,wght@0,400;1,300&display=swap" rel="stylesheet">
    <style>
        @font-face {
		font-family: 'Sucrose Bold Two';
		src: url('https://s3-us-west-2.amazonaws.com/s.cdpn.io/4273/sucrose.woff2') format('woff2');
		}
@font-face {
    font-family: 'IM Fell French Canon Pro';
    src: url('https://s3-us-west-2.amazonaws.com/s.cdpn.io/4273/im-fell-french-canon-pro.woff2') format('woff2');
}
* {
  box-sizing: border-box;
}
body {
  margin: 0;
}
header { 
	/*background: url(https://s3-us-west-2.amazonaws.com/s.cdpn.io/4273/mountain-range.jpg) no-repeat;*/
	background: url(https://media-exp1.licdn.com/dms/image/C4E1BAQGA1cWuVr4JTw/company-background_10000/0/1612830472883?e=2147483647&v=beta&t=nYmnTbV2bKdoFsLYrmN-3SjNtlA7rH96uyBEnN7VY8M) no-repeat;
	padding-top: 61.93333333%;
	background-size: cover;
  font-family: 'Sucrose Bold Two';
}
header img {
	position: absolute;
	top: 0;
	right: 0;
	width: 45.8%;
}

main { 
  background: #fff;
  position: relative;
  border: 1px solid #fff;
  font-family: 'IM Fell French Canon Pro';
  font-size: 1.4rem;
  padding: 2rem 25%;
  line-height: 1.6;
}
h1{ 
	color:#f2c43e;
  font-size: 10vw;
  line-height: .8;
  margin-top: 0;
  text-align: center;
  font-family: 'Sucrose Bold Two';
  z-index:999999;
  
}

h1 span {
  display: block;
  font-size: 8.75vw;
  background:linear-gradient(top, red, gold);
}
        

@media (max-width: 400px) {
  main { padding: 2rem; }
}
.divider:after,
.divider:before {
content: "";
flex: 1;
height: 1px;
background: #eee;
}

.h-custom {
height: calc(100% - 73px);
}
@media (max-width: 2850px) {
.h-custom {
height: 100%;
}

}
html, body{
      height:100%;
      margin: 0;
      display: flex;
      flex-direction: column;
    }
    #div1{
      height: 80px;
      width: 100%;
      background-color: red;
      justify-content: center;
      align-content: center;
    }

    #div2{
      
      width: 100%;
      background-color: rgba(0, 8, 20, 0.9)!important;
    }
    </style>
</head>
<body>
    
<div id="div2">
    <div class="container-fluid h-custom">
        <div class="row d-flex justify-content-center align-items-center" style="border-radius:1rem;background-color: rgba(255, 255, 255, 0.2)!important;margin:30px 20px 20px 20px;min-height:90vh;-webkit-box-shadow: 0px -33px 101px -48px rgba(237,237,9,1);
-moz-box-shadow: 0px -33px 101px -48px rgba(237,237,9,1);
box-shadow: 0px -33px 101px -48px rgba(237,237,9,1);">
            <div class="col-md-12 bg-red" style="height:100%!important;margin-top:10px">
                <table style="width:100%" border="0">
                    <tr>
                        <td><img src="img/logo.png" /></td>
                        <td style="text-align:right;color:white;font-size:20px"> <i class="fa fa-calendar" aria-hidden="true"></i> <asp:Label Style="font-weight:700" ID="lblFecha" runat="server" Text="Label"></asp:Label></td>
                    </tr>

                </table>
                 
                
            </div>
            <div class="col-md-12 bg-red" style="height:100%!important;margin-bottom:80px">
                 <h1>Facturación Masiva</h1>
            </div><br />
            <div class="col-md-12 bg-red" style="height:100%!important;text-align:center;margin-bottom:50px;margin-top:80px;padding:0px">
                <%--<img src="images/alala.png" class="img-fluid rounded mx-auto d-block" width="30%" alt="Sample image"/>--%>
               <%-- <img src="images/kisspng-small-business-management-company-businessman-fingers-download-to-a-report-picture-5a6f78666b15f9.1147671415172547584386.png" class="img-fluid" alt="Sample image"/>--%>
                <img src="img/pngwing.com (1).png" width="100%" class="img-fluid" alt="Sample image"  />
               <%-- <img src="images/transparent-logo-font-line-symbol-yellow-61dbb412c176d6.9459805016417884347924.png" width="200px" class="img-fluid" alt="Sample image" />--%>
                
                <%--<img src="images/favpng_magnifying-glass-invoice.png"
          class="img-fluid" alt="Sample image">--%>
            </div>
            <div class="col-md-12 bg-red" style="height:100%!important;">
                <form id="form1" runat="server">
          
         

          <div class="text-center text-lg-start mt-4 pt-2 pb-5">
            <asp:HyperLink ID="HyperLink1" CssClass="btn btn-lg btn-block  mt-5 shadow-lg" Style="font-family: 'Open Sans', sans-serif;color:black;background-color:#f2c43e;box-shadow: 1px 1px 82px 19px rgba(0,0,0,0.75);-webkit-box-shadow: 1px 1px 82px 19px rgba(0,0,0,0.75);-moz-box-shadow: 1px 1px 82px 19px rgba(0,0,0,0.75);" runat="server" NavegateUrl="Main.aspx" NavigateUrl="~/Main.aspx"><b><i class="fa fa-sign-in" aria-hidden="true"></i> Ingresar</b></asp:HyperLink>
              
            
          </div>

        </form>
            </div>
        </div>
  </div>
  
</div>
   

   
	 <footer id="sticky-footer" class="flex-shrink-0 py-4 bg-dark text-white-50" style="position: relative;background:rgba(0, 8, 20, 0.9) !important;">
    <div class="container text-center text-white">
      <small>2022 Copyright &copy; TDR Soluciones Logísticas</small>
    </div>
  </footer>
</body>
</html>
