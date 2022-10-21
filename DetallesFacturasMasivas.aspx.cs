using FactMasiva.Controllers;
using FactMasiva.Models;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace TIFacturasMasivas
{
    public partial class DetallesFacturasMasivas : System.Web.UI.Page
    {
        //Properties
        public FactLabControler facLabControler = new FactLabControler();
        public SearchControler busquedaControler = new SearchControler();
        public string fDesde, fHasta, concepto, tipoCobro;
        public Hashtable campos = new Hashtable();
        Hashtable detalle33 = new Hashtable();
        HtmlTable table = new HtmlTable();
        public string moneda, tipoCambio, MetodoPago33, tipocomprobante, usocdfi, confirmacion, relacion, uuidrel, lugarexpedicion, idreceptor, pais, calle, noExt, noInt, colonia, localidad, referencia, municipio,
            estado, cp, estatus, paisresidencia, numtributacion, mailenvio, serie, tipofactor, tasatras, codirete, tasarete, coditrans, uuidre, relac;
        public int mbnumber;
        public string numidentificacion, idConcepto, claveunidad, impuestoiva, tipofactoriva, tasa_iva, iva_monto, impuestoretencion, tipofactorret, tasa_retv, ret_monto;
        public string flete1, maniobras1, seguro1, pista1, cpac1, equipo1, otros1;
        public string fact, bill_to;
        public string cuota, valorComercial, remision, condicionesPago, contiene, comentarios, amparaRemisiones,
            operador, licencia, tratorEco, tractorPlacas, remol1Eco, remol1Placas, remol2Eco, remol2Placas, bandera, ultinvoice, hecha;
        public string clienteId, peso;
        public bool adenda;

        public string escrituraFactura = "", idSucursal = "", idTipoFactura = "", jsonFactura = "", IdApiEmpresa = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            bill_to = Request.QueryString["billto"];
            DataTable td_facturas = facLabControler.FacturasPorProcesar(bill_to);

            foreach (DataRow row in td_facturas.Rows)
            {
                if (bill_to.ToUpper() == row["idreceptor"].ToString().ToUpper())
                {
                    //todo select 
                    fact = row["Folio"].ToString();
                    lblFact.Text = facLabControler.facturaValida(fact);
                    fact = lblFact.Text;

                    //Verifica que la factura este en PRN
                    if (verificaStatus(fact))
                    {
                        //Obtiene el valor de cada uno de los detalles 
                        detalle.Visible = true;
                        flete1 = txtFlete.Text.ToString();
                        maniobras1 = txtManiobras.Text.ToString();
                        seguro1 = txtCargaXSeguro.Text.ToString();
                        pista1 = txtAutopistas.Text;
                        cpac1 = txtCPAC.Text;
                        equipo1 = txtEquipo.Text;
                        otros1 = txtOtros.Text;

                        //Si se realizó algún cambio obtiene los nuevos valores de los campos
                        if (IsPostBack)
                        {
                            //cuota = txtCuotaConvenida.Text;
                            //valorComercial = txtValorComercial.Text;
                            //remision = txtRemisión.Text;
                            //condicionesPago = txtCondicionesPago.Text;
                            //contiene = txtContine.Text;
                            //comentarios = txtComentarios.Text;
                            //amparaRemisiones = txtAmparaRemisiones.Text;
                            //operador = txtOperador.Text;
                            //licencia = txtLicencia.Text;
                            //tratorEco = txtTractorEco.Text;
                            //tractorPlacas = txtTractorPlacas.Text;
                            //remol1Eco = txtRemol1Eco.Text;
                            //remol1Placas = txtRemolque1Placas.Text;
                            //remol2Eco = txtRemolque2Eco.Text;
                            //remol2Placas = txtRemolque2Placas.Text;
                            //peso = txtPeso.Text;
                            //uuidrel = txtUUIDREL.Text;
                            //relac = txtRelacion.Text;
                        }

                        //Genera el detalle de la factura e inicia los campos
                        generaDetalle();
                        iniciaDatos();

                        if (Adenda.Visible == false)
                        {
                            //Verifica que el cliente se Liverpool o almacenadora
                            if (clienteId.Equals("LIVERPOL") || clienteId.Equals("ALMLIVER") || clienteId.Equals("LIVERTIJ") || clienteId.Equals("SFERALIV") || clienteId.Equals("GLOBALIV") || clienteId.Equals("SETRALIV") || clienteId.Equals("FACTUMLV") || clienteId.Equals("LIVERDED"))
                            // if (clienteId.Equals("LIVERPOL") || clienteId.Equals("ALMLIVER") || clienteId.Equals("LIVERTIJ"))
                            {

                                //Response.Redirect("Adenda.aspx?factura=" + fact);
                            }

                        }
                        else
                        {
                            //Verifica si los campos de la adenda han sido capturados
                            if (!txtPedido.Text.Equals("") && !txtHojaEntrada.Text.Equals(""))
                            {
                                Adenda.Enabled = false;
                                btnGuardar.Enabled = true;
                            }
                            else
                            {
                                Adenda.Enabled = true;
                                btnGuardar.Enabled = false;
                            }
                        }
                        //Verifica los campos de las facturas
                        bool valida = validaCampos();
                    }
                    else
                    {
                        //Si el estatus de la factura es diferente de PRN, redirecciona a la busqueda
                        Response.Redirect("Busqueda.aspx?factura=" + lblFact.Text + "&estatus=" + estatus);
                    }
                    GenerarCFDI();
                }
            }
            Response.Redirect("GeneracionMasiva.aspx");
            //string close = @"<script type='text/javascript'>
            //                    window.open('', '_self', ''); window.close();
            //                    </script>";
            //base.Response.Write(close);
        }

        //Verifica el estatus de la factura. Parámetro la factura
        public bool verificaStatus(string fact)
        {
            string referencia = "";
            DataTable td = facLabControler.estatus(fact);
            //Valida que la consulta contenga elemento
            if (td.Rows.Count != 0)
            {
                //Para cada resultado obtener el estatus y la referencia
                foreach (DataRow row in td.Rows)
                {
                    estatus = row["ivh_invoicestatus"].ToString();
                    referencia = row["ivh_ref_number"].ToString();

                }
                //Se separa la referencia "TDRT-2540" para obtener la factura y la serie

                string[] reference = referencia.Split('-');

                if (estatus.Equals("PRN"))
                {
                    return true;
                }

            }

            return false;
        }

        public void iniciaDatos()
        {

            DataTable td = facLabControler.detalleFacturas(lblFact.Text);


            foreach (DataRow row in td.Rows)
            {
                mbnumber = int.Parse(row["nmaster"].ToString());
                serie = row["serie"].ToString();
                if (mbnumber == 0)
                {

                    if (serie.Equals("NCT"))
                    {
                        lblTipo.Text = "Nota de Credito";
                    }
                    else
                    {
                        lblTipo.Text = "Factura";
                    }
                }

                else
                {
                    lblTipo.Text = "MasterBill";
                    lblFact.Text = mbnumber.ToString();
                }
                hecha = row["hecha"].ToString();
                bandera = row["bandera"].ToString();
                ultinvoice = row["ultinvoice"].ToString();
                moneda = row["moneda"].ToString();
                tipoCambio = row["tipocambio"].ToString();
                idreceptor = row["idreceptor"].ToString();

                //Obtengo el cliente para poder validar la denda de liverpool.
                clienteId = idreceptor;

                txtFactura.Text = row["idcomprobante"].ToString();
                DateTime dt = DateTime.Parse(row["fhemision"].ToString());
                txtFecha.Text = dt.ToString("yyyy'/'MM'/'dd HH:mm:ss");
                txtNombre.Text = row["nombrecliente"].ToString();
                calle = row["calle"].ToString();
                noExt = row["numext"].ToString();
                noInt = row["numint"].ToString();
                colonia = row["colonia"].ToString();
                municipio = row["municdeleg"].ToString();
                estado = row["estado"].ToString();
                cp = row["cp"].ToString();
                pais = row["pais"].ToString();

                txtDirección.Text = calle + ", " + noExt + ", " + noInt + ", " + colonia + ", " + municipio + ", " + estado + ", " +
                    cp + ", " + pais;
                txtRFC.Text = row["rfc"].ToString();
                txtOrigen.Text = row["origen"].ToString();
                txtOrigenRemitente.Text = row["remitente"].ToString();
                //txtDomicilioOrigen.Text = row["domicilioorigen"].ToString();
                txtDomicilioOrigen.Text = "";
                txtRFCOrigen.Text = row["rfcorigen"].ToString();
                txtDestino.Text = row["destino"].ToString();
                txtDestinatario.Text = row["destinatario"].ToString();
                //txtDomicilioDestino.Text = row["domiciliodestino"].ToString();
                txtDomicilioDestino.Text = "";
                txtRFCDestino.Text = row["rfcdestino"].ToString();



                if (cuota == null || cuota.Equals(row["cuotaconv"].ToString())) { txtCuotaConvenida.Text = row["cuotaconv"].ToString(); }
                else { txtCuotaConvenida.Text = cuota; }

                if (valorComercial == null || valorComercial.Equals(row["valorcomer"].ToString())) { txtValorComercial.Text = row["valorcomer"].ToString(); }
                else { txtValorComercial.Text = valorComercial; }

                if (remision == null || remision.Equals(row["remision"].ToString())) { txtRemisión.Text = row["remision"].ToString(); }
                else { txtRemisión.Text = remision; }

                if (condicionesPago == null || condicionesPago.Equals(row["condpago"].ToString())) { txtCondicionesPago.Text = row["condpago"].ToString(); }
                else { txtCondicionesPago.Text = condicionesPago; }

                if (contiene == null || contiene.Equals(row["contiene"].ToString())) { txtContine.Text = row["contiene"].ToString(); }
                else { txtContine.Text = contiene; }

                if (comentarios == null || comentarios.Equals(row["comentarios"].ToString())) { txtComentarios.Text = row["comentarios"].ToString(); }
                else { txtComentarios.Text = comentarios; }

                DataSet ds = new DataSet();
                //dbConnector objConn = new dbConnector();
                string cadena2 = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
                //DataTable dataTable = new DataTable();

                SqlConnection Conn = new SqlConnection(cadena2);
                //SqlConnection Conn = objConn.GetConnection;
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    try
                    {
                        da.SelectCommand = new SqlCommand("sp_Comentarios_cfdi_orden", Conn);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.Parameters.AddWithValue("@orden", row["orden"].ToString());

                        DataSet dss = new DataSet();
                        da.Fill(dss, "result_name");

                        DataTable dt_result = dss.Tables["result_name"];
                        foreach (DataRow rowComent in dt_result.Rows)
                        {
                            txtComentarios.Text = rowComent["cc_comentarios"].ToString();
                        }
                        //for example          

                    }
                    catch (SqlException ex)
                    {
                        // error here
                        string error = ex.Message;

                    }
                    finally
                    {
                        Conn.Close();
                    }
                }


                if (amparaRemisiones == null || amparaRemisiones.Equals(row["ampararemisiones"].ToString())) { txtAmparaRemisiones.Text = row["ampararemisiones"].ToString(); }
                else { txtAmparaRemisiones.Text = amparaRemisiones; }


                if (peso == null || peso.Equals(row["pesoestimado"].ToString())) { txtPeso.Text = row["pesoestimado"].ToString(); }
                else { txtPeso.Text = peso; }
                //txtPeso.Text = row["pesoestimado"].ToString();
                txtOrden.Text = row["orden"].ToString();
                txtMb.Text = row["nmaster"].ToString();
                txtInvoice.Text = row["invoice"].ToString();
                txtMovimiento.Text = row["movimiento"].ToString();

                if (operador == null || operador.Equals(row["operador"].ToString())) { txtOperador.Text = row["operador"].ToString(); }
                else { txtOperador.Text = operador; }

                if (licencia == null || licencia.Equals(row["operadorlicenicia"].ToString())) { txtLicencia.Text = row["operadorlicenicia"].ToString(); }
                else { txtLicencia.Text = licencia; }

                if (tratorEco == null || tratorEco.Equals(row["tractoeco"].ToString())) { txtTractorEco.Text = row["tractoeco"].ToString(); }
                else { txtTractorEco.Text = tratorEco; }

                if (tractorPlacas == null || tractorPlacas.Equals(row["tractoplaca"].ToString())) { txtTractorPlacas.Text = row["tractoplaca"].ToString(); }
                else { txtTractorPlacas.Text = tractorPlacas; }

                if (remol1Eco == null || remol1Eco.Equals(row["remolque1Eco"].ToString())) { txtRemol1Eco.Text = row["remolque1Eco"].ToString(); }
                else { txtRemol1Eco.Text = remol1Eco; }

                if (remol1Placas == null || remol1Placas.Equals(row["remolque1Placa"].ToString())) { txtRemolque1Placas.Text = row["remolque1Placa"].ToString(); }
                else { txtRemolque1Placas.Text = remol1Placas; }

                if (remol2Eco == null || remol2Eco.Equals(row["remolque2Eco"].ToString())) { txtRemolque2Eco.Text = row["remolque2Eco"].ToString(); }
                else { txtRemolque2Eco.Text = remol2Eco; }

                if (remol2Placas == null || remol2Placas.Equals(row["remolque2Placa"].ToString())) { txtRemolque2Placas.Text = row["remolque2Placa"].ToString(); }
                else { txtRemolque2Placas.Text = remol2Placas; }
                //todo cambiar al servico web

                if (uuidrel == null || uuidrel.Equals(row["uuidrel"].ToString())) { txtUUIDREL.Text = row["uuidrel"].ToString(); uuidrel = row["uuidrel"].ToString(); }
                else { txtUUIDREL.Text = uuidrel; }

                if (relac == null || relac.Equals(row["relacion"].ToString())) { txtRelacion.Text = row["relacion"].ToString(); relac = row["relacion"].ToString(); }
                else { txtRelacion.Text = relac; }

                if (clienteId == "SAYER")
                {
                    txtRelacion.Text = "04";
                }


                txtSubtotal.Text = row["subtotal"].ToString().Remove(row["subtotal"].ToString().Length - 2);
                txtIVA.Text = row["imptras"].ToString().Remove(row["imptras"].ToString().Length - 2);
                txtRetencion.Text = row["imprete"].ToString().Remove(row["imprete"].ToString().Length - 2);

                if (txtRetencion.Text == "0.00")
                {
                    txtRetencion.Text = "";

                }

                txtTotal.Text = row["total"].ToString().Remove(row["total"].ToString().Length - 2);

                txtDocumento.Text = row["documento"].ToString();
                txtCantidadLetra.Text = row["totalletra"].ToString();


                Hashtable datosReales = generaActualizacion();

                if (datosReales.ContainsKey("FLETE")) { txtFlete.Text = datosReales["FLETE"].ToString(); }
                else { txtFlete.Text = ""; }

                if (datosReales.ContainsKey("MANIOBRAS")) { txtManiobras.Text = datosReales["MANIOBRAS"].ToString(); }
                else { txtManiobras.Text = ""; }

                if (datosReales.ContainsKey("OTROS")) { txtOtros.Text = datosReales["OTROS"].ToString(); }
                else { txtOtros.Text = ""; }

                if (datosReales.ContainsKey("RENTA EQUIPO")) { txtEquipo.Text = datosReales["RENTA EQUIPO"].ToString(); }
                else { txtEquipo.Text = ""; }

                if (datosReales.ContainsKey("CPAC")) { txtCPAC.Text = datosReales["CPAC"].ToString(); }
                else { txtCPAC.Text = ""; }

                if (datosReales.ContainsKey("CARGO POR SEGURO")) { txtCargaXSeguro.Text = datosReales["CARGO POR SEGURO"].ToString(); }
                else { txtCargaXSeguro.Text = ""; }

                if (datosReales.ContainsKey("AUTOPISTAS")) { txtAutopistas.Text = datosReales["AUTOPISTAS"].ToString(); }
                else { txtAutopistas.Text = ""; }

                txtFechaPagare.Text = row["fechapagare"].ToString();
                txtCantidadLetra2.Text = row["totalletra"].ToString();
                txtTotal2.Text = row["total"].ToString();
                txtInteres.Text = row["interesespagare"].ToString();
                txtClientePagare.Text = row["clientepagare"].ToString();
                txtFormaPago.Text = row["fpago"].ToString();
                txtCondicionesPago2.Text = row["condpago"].ToString();
                txtMetodoPago.Text = row["metpago"].ToString();

                MetodoPago33 = row["metodopago33"].ToString();
                tipocomprobante = row["tipocomprobante"].ToString();
                lugarexpedicion = row["lugarexpedicion"].ToString();
                usocdfi = row["usocfdi"].ToString();
                confirmacion = row["confirmacion"].ToString();



                referencia = row["referencia"].ToString();
                localidad = row["localidad"].ToString();
                paisresidencia = row["paisresidencia"].ToString();
                numtributacion = row["numtributacion"].ToString();
                mailenvio = row["mailenvio"].ToString();



                tipofactor = row["tipofactor"].ToString();
                tasatras = row["tasatras"].ToString();
                codirete = row["codirete"].ToString();
                tasarete = row["tasarete"].ToString();
                coditrans = row["coditrans"].ToString();

                try
                { //try TLS 1.3
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)12288
                                                         | (SecurityProtocolType)3072
                                                         | (SecurityProtocolType)768
                                                         | SecurityProtocolType.Tls;
                }
                catch (NotSupportedException)
                {
                    try
                    { //try TLS 1.2
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072
                                                             | (SecurityProtocolType)768
                                                             | SecurityProtocolType.Tls;
                    }
                    catch (NotSupportedException)
                    {
                        try
                        { //try TLS 1.1
                            ServicePointManager.SecurityProtocol = (SecurityProtocolType)768
                                                                 | SecurityProtocolType.Tls;
                        }
                        catch (NotSupportedException)
                        { //TLS 1.0
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                        }
                    }
                }

                if (row["RevType4"].ToString() != "CNV")
                {

                    IdApiEmpresa = "bf2e1036-ba47-49a0-8cd9-e04b36d5afd4";
                    var request_ = (HttpWebRequest)WebRequest.Create("https://canal1.xsa.com.mx:9050/" + IdApiEmpresa + "/tiposCfds");
                    var response_ = (HttpWebResponse)request_.GetResponse();
                    var responseString_ = new StreamReader(response_.GetResponseStream()).ReadToEnd();

                    string[] separadas_ = responseString_.Split('}');

                    foreach (string dato in separadas_)
                    {
                        if (dato.Contains(serie))
                        {
                            string[] separadasSucursal_ = dato.Split(',');
                            foreach (string datoSuc in separadasSucursal_)
                            {
                                if (datoSuc.Contains("idSucursal"))
                                {
                                    idSucursal = datoSuc.Replace(dato.Substring(0, 8), "").Replace("\"", "").Split(':')[1];

                                }

                                if (datoSuc.Contains("id") && !datoSuc.Contains("idSucursal"))
                                {
                                    idTipoFactura = datoSuc.Replace(dato.Substring(0, 8), "").Replace("\"", "").Split(':')[1];

                                }
                            }
                        }
                    }
                }
                else if (row["RevType4"].ToString() == "CNV")
                {
                    if (mbnumber != 0)
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter())
                        {
                            try
                            {
                                da.SelectCommand = new SqlCommand("Sp_Fact_conceptosComodity", Conn);
                                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                                da.SelectCommand.Parameters.AddWithValue("@ord_mb", mbnumber);

                                DataSet dss = new DataSet();
                                da.Fill(dss, "result_name");

                                DataTable dt_result = dss.Tables["result_name"];
                                foreach (DataRow rowComent in dt_result.Rows)
                                {
                                    if (txtRemisión.Text.Length > 7)
                                    {
                                        txtComentarios.Text = txtRemisión.Text.Replace(" ", "").Substring(0, 7) + rowComent[0].ToString() + "/" + row["ampararemisiones"].ToString();
                                    }
                                    else
                                    {
                                        txtComentarios.Text = txtRemisión.Text.Replace(" ", "") + "/" + row["ampararemisiones"].ToString();
                                    }
                                    //txtRemisión.Text = "";
                                    txtContine.Text = "";
                                    txtAmparaRemisiones.Text = "";
                                }

                            }
                            catch (SqlException ex)
                            {
                                // error here
                                string error = ex.Message;

                            }
                            finally
                            {
                                Conn.Close();
                            }
                        }
                    }
                    else
                    {
                        txtComentarios.Text = txtRemisión.Text.Replace(" ", "") + "/" + row["contiene"].ToString() + "/" + row["ampararemisiones"].ToString();
                        txtContine.Text = "";
                        txtAmparaRemisiones.Text = "";
                    }
                    //cambiar key a convoy
                    IdApiEmpresa = "87d0d022-9dc3-4712-9bac-b0a9f6371e7c";
                    if (lblTipo.Text != "Nota de Credito")
                    {
                        serie = "TDRCON";
                        var request_ = (HttpWebRequest)WebRequest.Create("https://canal1.xsa.com.mx:9050/" + IdApiEmpresa + "/tiposCfds");
                        var response_ = (HttpWebResponse)request_.GetResponse();
                        var responseString_ = new StreamReader(response_.GetResponseStream()).ReadToEnd();

                        string[] separadas_ = responseString_.Split('}');

                        foreach (string dato in separadas_)
                        {
                            if (dato.Contains(serie))
                            {
                                string[] separadasSucursal_ = dato.Split(',');
                                foreach (string datoSuc in separadasSucursal_)
                                {
                                    if (datoSuc.Contains("idSucursal"))
                                    {
                                        idSucursal = datoSuc.Replace(dato.Substring(0, 8), "").Replace("\"", "").Split(':')[1];

                                    }

                                    if (datoSuc.Contains("id") && !datoSuc.Contains("idSucursal"))
                                    {
                                        idTipoFactura = datoSuc.Replace("\"", "").Split(':')[1];
                                    }
                                }
                            }
                        }
                        serie = "CONV";
                    }
                    else
                    {
                        serie = "NTC";
                        var request_ = (HttpWebRequest)WebRequest.Create("https://canal1.xsa.com.mx:9050/" + IdApiEmpresa + "/tiposCfds");
                        var response_ = (HttpWebResponse)request_.GetResponse();
                        var responseString_ = new StreamReader(response_.GetResponseStream()).ReadToEnd();

                        string[] separadas_ = responseString_.Split('}');

                        foreach (string dato in separadas_)
                        {
                            if (dato.Contains(serie))
                            {
                                string[] separadasSucursal_ = dato.Split(',');
                                foreach (string datoSuc in separadasSucursal_)
                                {
                                    if (datoSuc.Contains("idSucursal"))
                                    {
                                        idSucursal = datoSuc.Replace(dato.Substring(0, 8), "").Replace("\"", "").Split(':')[1];

                                    }

                                    if (datoSuc.Contains("id") && !datoSuc.Contains("idSucursal"))
                                    {
                                        idTipoFactura = datoSuc.Replace(dato.Substring(0, 8), "").Replace("\"", "").Split(':')[1];
                                    }
                                }
                            }
                        }
                    }


                }


                //postbak antiguo
                //cuota = txtCuotaConvenida.Text;
                //valorComercial = txtValorComercial.Text;
                //remision = txtRemisión.Text;
                //condicionesPago = txtCondicionesPago.Text;
                //contiene = txtContine.Text;
                //comentarios = txtComentarios.Text;
                //amparaRemisiones = txtAmparaRemisiones.Text;
                //operador = txtOperador.Text;
                //licencia = txtLicencia.Text;
                //tratorEco = txtTractorEco.Text;
                //tractorPlacas = txtTractorPlacas.Text;
                //remol1Eco = txtRemol1Eco.Text;
                //remol1Placas = txtRemolque1Placas.Text;
                //remol2Eco = txtRemolque2Eco.Text;
                //remol2Placas = txtRemolque2Placas.Text;
                //peso = txtPeso.Text;
                //uuidrel = txtUUIDREL.Text;
                //relac = txtRelacion.Text;


            }
        }

        //Se escribe el archivo txt con los datos para generar la factura.
        public void generaTXT()
        {
            string path = System.Web.Configuration.WebConfigurationManager.AppSettings["dir"] + lblFact.Text + ".txt";
            escrituraFactura = "";
            //@"C:\Users\J. Desarrollo\Downloads\E1247198.txt"
            //using (System.IO.StreamWriter escritor = new System.IO.StreamWriter(@"C:\Users\J. Desarrollo\Downloads\E1247198.txt"))
            using (System.IO.StreamWriter escritor = new System.IO.StreamWriter(path))
            {

                //----------------------------------------Seccion De Definición de Serie de Folios CFDI------------------------------------------------------------------------------------

                //Se define la serie TDRL para el complemento detallista de Liverpool y Almacenadora
                if (clienteId.Equals("LIVERPOL") || clienteId.Equals("ALMLIVER") || clienteId.Equals("LIVERTIJ") || clienteId.Equals("SFERALIV") || clienteId.Equals("GLOBALIV") || clienteId.Equals("SETRALIV") || clienteId.Equals("FACTUMLV") || clienteId.Equals("LIVERDED"))
                {
                    string tipoFactura = "";
                    //Dependiendo del tipo de factura inicializa la variable de "tipoFactura"
                    if (serie.Equals("NCT"))
                    {
                        tipoFactura = "NCR";
                    }
                    else if (serie.Equals("TDRT"))
                    {

                        tipoFactura = "FAC";
                    }
                    serie = "TDRL";
                    string generaIdComprobante = serie + "-" + fact;
                    txtFactura.Text = generaIdComprobante;

                    //----------------------------------------Seccion De Datos Generales del CFDI-----------------------------------------------------------------------------------------

                    //01 INFORMACION GENERAL DEL CFDI (1:1)

                    escritor.WriteLine(
                    "01"                                                //1.-Tipo De Registro
                    + "|" + generaIdComprobante                         //2-ID Comprobante
                    + "|" + serie                                       //3-Serie
                    + "|" + fact.Substring(1)                           //4-Foliio 
                    + "|" + txtFecha.Text.Trim()                        //5-Fecha y Hora De Emision
                    + "|" + txtSubtotal.Text.Trim()                     //6-Subtotal
                    + "|" + txtIVA.Text.Trim()                          //7-Total Impuestos Trasladados
                    + "|" + txtRetencion.Text.Trim()                    //8-Total Impuestos Retenidos
                    + "|"                                               //9-Descuentos
                    + "|" + txtTotal.Text.Trim()                        //10-Total
                    + "|" + txtCantidadLetra.Text.Trim()                //11-Total Con Letra
                    + "|" + txtMetodoPago.Text.Trim()                   //12-Forma De Pago
                    + "|" + txtCondicionesPago2.Text.Trim()             //13-Condiciones De Pago
                    + "|" + MetodoPago33                                //14-Metodo de Pago
                    + "|" + moneda.Trim()                               //15-Moneda
                    + "|" + tipoCambio.Trim()                           //16-Tipo De Cambio
                    + "|" + tipocomprobante                             //17-Tipo De Comprobante
                    + "|" + lugarexpedicion                             //18-Lugar De Expedicion                                        
                    + "|" + usocdfi                                     //19-Uso CFDI
                    + "|" + confirmacion                                //20-Confirmacion
                    + "|"
                    );

                    escrituraFactura += "01"                                                //1.-Tipo De Registro
                    + "|" + generaIdComprobante                         //2-ID Comprobante
                    + "|" + serie                                       //3-Serie
                    + "|" + fact.Substring(1)                           //4-Foliio 
                    + "|" + txtFecha.Text.Trim()                        //5-Fecha y Hora De Emision
                    + "|" + txtSubtotal.Text.Trim()                     //6-Subtotal
                    + "|" + txtIVA.Text.Trim()                          //7-Total Impuestos Trasladados
                    + "|" + txtRetencion.Text.Trim()                    //8-Total Impuestos Retenidos
                    + "|"                                               //9-Descuentos
                    + "|" + txtTotal.Text.Trim()                        //10-Total
                    + "|" + txtCantidadLetra.Text.Trim()                //11-Total Con Letra
                    + "|" + txtMetodoPago.Text.Trim()                   //12-Forma De Pago
                    + "|" + txtCondicionesPago2.Text.Trim()             //13-Condiciones De Pago
                    + "|" + MetodoPago33                                //14-Metodo de Pago
                    + "|" + moneda.Trim()                               //15-Moneda
                    + "|" + tipoCambio.Trim()                           //16-Tipo De Cambio
                    + "|" + tipocomprobante                             //17-Tipo De Comprobante
                    + "|" + lugarexpedicion                             //18-Lugar De Expedicion                                        
                    + "|" + usocdfi                                     //19-Uso CFDI
                    + "|" + confirmacion                                //20-Confirmacion
                    + "|";
                }
                else
                {

                    //01 INFORMACION GENERAL DEL CFDI (1:1)

                    escritor.WriteLine(
                    "01"                                                 //1-Tipo De Registro
                    + "|" + txtFactura.Text                              //2-ID Comprobante
                    + "|" + serie                                        //3-Serie
                    + "|" + fact.Substring(1)                            //4-Foliio 
                    + "|" + txtFecha.Text.Trim()                         //5-Fecha y Hora De Emision
                    + "|" + txtSubtotal.Text.Trim()                      //6-Subtotal
                    + "|" + txtIVA.Text.Trim()                           //7-Total Impuestos Trasladados
                    + "|" + txtRetencion.Text.Trim()                     //8-Total Impuestos Retenidos
                    + "|"                                                //9-Descuentos
                    + "|" + txtTotal.Text.Trim()                         //10-Total
                    + "|" + txtCantidadLetra.Text.Trim()                 //11-Total Con Letra
                    + "|" + txtMetodoPago.Text.Trim()                    //12-Forma de Pago
                    + "|" + txtCondicionesPago2.Text.Trim()              //13-Condiciones de Pago
                    + "|" + MetodoPago33                                 //14-Metodo de Pago
                    + "|" + moneda.Trim()                                //15-Moneda
                    + "|" + tipoCambio.Trim()                            //16-Tipo De Cambio
                    + "|" + tipocomprobante                              //17-Tipo de Comprobante
                    + "|" + lugarexpedicion                              //18-Lugar de Expedicion
                    + "|" + usocdfi                                      //19-Uso CFDI
                    + "|" + confirmacion                                 //20-Confirmacion
                    + "|"                                                //Fin Del Registro
                    );

                    escrituraFactura += "01"                                                 //1-Tipo De Registro
                    + "|" + txtFactura.Text                              //2-ID Comprobante
                    + "|" + serie                                        //3-Serie
                    + "|" + fact.Substring(1)                            //4-Foliio 
                    + "|" + txtFecha.Text.Trim()                         //5-Fecha y Hora De Emision
                    + "|" + txtSubtotal.Text.Trim()                      //6-Subtotal
                    + "|" + txtIVA.Text.Trim()                           //7-Total Impuestos Trasladados
                    + "|" + txtRetencion.Text.Trim()                     //8-Total Impuestos Retenidos
                    + "|"                                                //9-Descuentos
                    + "|" + txtTotal.Text.Trim()                         //10-Total
                    + "|" + txtCantidadLetra.Text.Trim()                 //11-Total Con Letra
                    + "|" + txtMetodoPago.Text.Trim()                    //12-Forma de Pago
                    + "|" + txtCondicionesPago2.Text.Trim()              //13-Condiciones de Pago
                    + "|" + MetodoPago33                                 //14-Metodo de Pago
                    + "|" + moneda.Trim()                                //15-Moneda
                    + "|" + tipoCambio.Trim()                            //16-Tipo De Cambio
                    + "|" + tipocomprobante                              //17-Tipo de Comprobante
                    + "|" + lugarexpedicion                              //18-Lugar de Expedicion
                    + "|" + usocdfi                                      //19-Uso CFDI
                    + "|" + confirmacion                                 //20-Confirmacion
                    + "|";
                }

                //----------------------------------------Seccion de los datos del receptor del CFDI -------------------------------------------------------------------------------------

                //02 INFORMACION DEL RECEPTOR (1:1)

                escritor.WriteLine(
                "02"                                                   //1-Tipo De Registro
                + "|" + idreceptor                                     //2-Id Receptor
                + "|" + txtRFC.Text.Trim()                             //3-RFC
                + "|" + txtNombre.Text.Trim()                          //4-Nombre
                + "|" + pais                                           //5-Pais
                + "|" + calle                                          //6-Calle
                + "|" + noExt                                          //7-Numero Exterior
                + "|" + noInt                                          //8-Numero Interior
                + "|" + colonia                                        //9-Colonia
                + "|" + localidad                                      //10-Localidad
                + "|" + referencia                                     //11-Referencia
                + "|" + municipio                                      //12-Municio/Delegacion
                + "|" + estado                                         //13-EStado
                + "|" + cp                                             //14-Codigo Postal
                + "|" + paisresidencia                                 //15-Pais de Residecia Fiscal Cuando La Empresa Sea Extrajera
                + "|" + numtributacion                                 //16-Numero de Registro de ID Tributacion 
                + "|" + mailenvio                                      //17-Correo de envio                                                    
                + "|"                                                  //Fin Del Registro 
                );
                escrituraFactura += "\\n02"                                                   //1-Tipo De Registro
                + "|" + idreceptor                                     //2-Id Receptor
                + "|" + txtRFC.Text.Trim()                             //3-RFC
                + "|" + txtNombre.Text.Trim()                          //4-Nombre
                + "|" + pais                                           //5-Pais
                + "|" + calle                                          //6-Calle
                + "|" + noExt                                          //7-Numero Exterior
                + "|" + noInt                                          //8-Numero Interior
                + "|" + colonia                                        //9-Colonia
                + "|" + localidad                                      //10-Localidad
                + "|" + referencia                                     //11-Referencia
                + "|" + municipio                                      //12-Municio/Delegacion
                + "|" + estado                                         //13-EStado
                + "|" + cp                                             //14-Codigo Postal
                + "|" + paisresidencia                                 //15-Pais de Residecia Fiscal Cuando La Empresa Sea Extrajera
                + "|" + numtributacion                                 //16-Numero de Registro de ID Tributacion 
                + "|" + mailenvio                                      //17-Correo de envio                                                    
                + "|";
                //----------------------------------------Seccion de detalles de la factura leidos desde vista_fe_detail-------------------------------------------------------------------

                Hashtable concept = conceptosFinales();  //Hastabla de la version 3.2, se toma suposicion que se utiliza para calcular el agrupamiento de conceptos.

                int j = 1;                          //Se declara la varible que se incrementara para recorrer el Hashtable detalle33
                ArrayList lista = new ArrayList();  //Se declara el arraylist que contendra lo valores del hashtable detalle33
                int sinImpuestos = 0;
                Console.WriteLine(moneda + idreceptor);
                foreach (int item in detalle33.Keys) //Se recorren los registros del Hashtable Detalle33
                {
                    lista = (ArrayList)detalle33[item];  //Se agrega al Array Lista lo contenido en el HashTabla detalle33


                    //04 INFORMACION DE LOS CONCEPTOS (1:N)
                    escritor.WriteLine(
                    "04"                                                   //1-Tipo De Registro
                    + "|" + j.ToString()                                   //2-Consecutivo Concepto
                    + "|" + lista[7].ToString().Trim()                     //3-Clave Producto o Servicio SAT
                    + "|" + lista[2].ToString().Trim()                     //4-Numero Identificacion TDR
                    + "|" + double.Parse(lista[0].ToString()).ToString()   //5-Cantidad
                    + "|" + lista[6].ToString().Trim()                     //6-Clave Unidad SAT
                    + "|" + lista[1].ToString().Trim()                     //7-Unidad de Medida
                    + "|" + lista[3].ToString().Trim()                     //8-Descripcion
                    + "|" + lista[4].ToString().Trim()                     //9-Valor Unitario
                    + "|" + lista[5].ToString().Trim()                     //10-Importe
                    + "|"                                                  //11-Descuento                                                  
                                                                           //12 Importe con iva si el rfc es XAXX010101000 y XEXX010101000 OPCIONAL
                    + "|"                                                  //Fin Del Registro
                     );
                    escrituraFactura += "\\n04"                                                   //1-Tipo De Registro
                    + "|" + j.ToString()                                   //2-Consecutivo Concepto
                    + "|" + lista[7].ToString().Trim()                     //3-Clave Producto o Servicio SAT
                    + "|" + lista[2].ToString().Trim()                     //4-Numero Identificacion TDR
                    + "|" + double.Parse(lista[0].ToString()).ToString()   //5-Cantidad
                    + "|" + lista[6].ToString().Trim()                     //6-Clave Unidad SAT
                    + "|" + lista[1].ToString().Trim()                     //7-Unidad de Medida
                    + "|" + lista[3].ToString().Trim()                     //8-Descripcion
                    + "|" + lista[4].ToString().Trim()                     //9-Valor Unitario
                    + "|" + lista[5].ToString().Trim()                     //10-Importe
                    + "|"                                                  //11-Descuento                                                  
                                                                           //12 Importe con iva si el rfc es XAXX010101000 y XEXX010101000 OPCIONAL
                    + "|";
                    //041 DATOS DE LOS IMPUESTOS TRASLADADOS CONCEPTOS Dolares
                    //if (moneda == "USD" && (idreceptor == "WERGLOBA" || idreceptor == "SAYER" || idreceptor == "SAYAPA" || idreceptor == "GOLTOR" || idreceptor == "TRPLACE" || idreceptor == "TRPLACE"))
                    if (moneda == "USD" && (idreceptor == "WERGLOBA" || idreceptor == "SAYER" || idreceptor == "SAYAPA1" || idreceptor == "GOLTOR" || idreceptor == "TRPLACE" || idreceptor == "TRPLACE"))
                    {
                        //041 DATOS DE LOS IMPUESTOS TRASLADADOS CONCEPTOS (0:N) 

                        escritor.WriteLine(
                        "041"                                                  //1-Tipo De Registro
                        + "|" + j.ToString()                                    //2-Consecutivo Concepto     
                        + "|" + lista[9].ToString().Trim()                     //3-Impuesto
                        + "|" + lista[10].ToString().Trim()                    //4-Tipo Factor
                        + "|" + (Math.Round((Convert.ToDecimal(lista[11].ToString().Trim()) / Convert.ToDecimal(lista[5].ToString().Trim())), 2)).ToString() + "0000"                //5-Tasa o Cuota
                        + "|" + lista[11].ToString().Trim()                    //7-Importe
                        + "|" + lista[5].ToString().Trim()                     //8-Base Calculo
                        + "|"                                                  //Fin del Registro
                        );
                        escrituraFactura += "\\n041"                                                  //1-Tipo De Registro
                        + "|" + j.ToString()                                    //2-Consecutivo Concepto     
                        + "|" + lista[9].ToString().Trim()                     //3-Impuesto
                        + "|" + lista[10].ToString().Trim()                    //4-Tipo Factor
                        + "|" + (Math.Round((Convert.ToDecimal(lista[11].ToString().Trim()) / Convert.ToDecimal(lista[5].ToString().Trim())), 2)).ToString() + "0000"                           //5-Tasa o Cuota
                        + "|" + lista[11].ToString().Trim()                    //7-Importe
                        + "|" + lista[5].ToString().Trim()                     //8-Base Calculo
                        + "|";
                    }

                    else if (moneda == "USD")
                    {
                        escritor.WriteLine(
                        "041"                                                  //1-Tipo De Registro
                        + "|" + j.ToString()                                    //2-Consecutivo Concepto     
                        + "|" + lista[9].ToString().Trim()                     //3-Impuesto
                        + "|Exento"                   //4-Tipo Factor
                        + "|"                      //5-Tasa o Cuota
                        + "|"                    //7-Importe
                        + "|" + lista[5].ToString().Trim()                     //8-Base Calculo
                        + "|"                                                  //Fin del Registro
                        );
                        escrituraFactura += "\\n041"                                                  //1-Tipo De Registro
                        + "|" + j.ToString()                                    //2-Consecutivo Concepto     
                        + "|" + lista[9].ToString().Trim()                     //3-Impuesto
                        + "|Exento"                   //4-Tipo Factor
                        + "|"                      //5-Tasa o Cuota
                        + "|"                    //7-Importe
                        + "|" + lista[5].ToString().Trim()                     //8-Base Calculo
                        + "|";
                    }
                    else if (lista[15].ToString().Trim() == "0.00" && idreceptor == "CHROBMEX" && lista[3].ToString().Trim() != "Otros" && lista[3].ToString().Trim() != "Maniobras de  Descarga" && lista[3].ToString().Trim() != "refact" || lista[15].ToString().Trim() == "0.00" && idreceptor == "CHROBMEX" && sinImpuestos > 0)
                    {
                        sinImpuestos++;
                        escritor.WriteLine(
                        "041"                                                  //1-Tipo De Registro
                        + "|" + j.ToString()                                    //2-Consecutivo Concepto     
                        + "|" + lista[9].ToString().Trim()                     //3-Impuesto
                        + "|Exento"                   //4-Tipo Factor
                        + "|"                      //5-Tasa o Cuota
                        + "|"                    //7-Importe
                        + "|" + lista[5].ToString().Trim()                     //8-Base Calculo
                        + "|"                                                  //Fin del Registro
                        );
                        escrituraFactura += "\\n041"                                                  //1-Tipo De Registro
                        + "|" + j.ToString()                                    //2-Consecutivo Concepto     
                        + "|" + lista[9].ToString().Trim()                     //3-Impuesto
                        + "|Exento"                   //4-Tipo Factor
                        + "|"                      //5-Tasa o Cuota
                        + "|"                    //7-Importe
                        + "|" + lista[5].ToString().Trim()                     //8-Base Calculo
                        + "|";
                    }

                    else
                    {
                        //041 DATOS DE LOS IMPUESTOS TRASLADADOS CONCEPTOS (0:N) 


                        escritor.WriteLine(
                        "041"                                                  //1-Tipo De Registro
                        + "|" + j.ToString()                                    //2-Consecutivo Concepto     
                        + "|" + lista[9].ToString().Trim()                     //3-Impuesto
                        + "|" + lista[10].ToString().Trim()                    //4-Tipo Factor
                        + "|" + lista[8].ToString().Trim()                     //5-Tasa o Cuota
                        + "|" + lista[11].ToString().Trim()                    //7-Importe
                        + "|" + lista[5].ToString().Trim()                     //8-Base Calculo
                        + "|"                                                  //Fin del Registro
                        );
                        escrituraFactura += "\\n041"                                                  //1-Tipo De Registro
                        + "|" + j.ToString()                                    //2-Consecutivo Concepto     
                        + "|" + lista[9].ToString().Trim()                     //3-Impuesto
                        + "|" + lista[10].ToString().Trim()                    //4-Tipo Factor
                        + "|" + lista[8].ToString().Trim()                     //5-Tasa o Cuota
                        + "|" + lista[11].ToString().Trim()                    //7-Importe
                        + "|" + lista[5].ToString().Trim()                     //8-Base Calculo
                        + "|";

                    }
                    //Si el valor del monto de la retencion es diferente de 0.00 se escribe el registro de datos de los impuestos retenidos
                    if (lista[15].ToString().Trim() != "0.00")
                    {


                        //042 Datos de los impuestos retinidos (RET) en conceptos(0:N) crear por concepto (retencion)
                        if (idreceptor == "PEÑAFIEL" && lista[7].ToString().Trim() == "78121601")
                        {
                            escritor.WriteLine(
                           "042"                                                  //1-Tipo De Registro
                           + "|" + j.ToString()                                   //2-Consecutivo Concepto
                           + "|" + lista[13].ToString().Trim()                    //3-Impuesto
                           + "|" + lista[14].ToString().Trim()                    //4-Tipo Factor
                           + "|" + "0.060000"                //5-Tasa o Cuota
                           + "|" + (Convert.ToDecimal(lista[5].ToString().Trim()) * Convert.ToDecimal("0.06")).ToString()                  //7-Importe
                           + "|" + lista[5].ToString().Trim()                     //8-Base Calculo
                           + "|"                                                  //Fin del Registro
                           );
                            escrituraFactura += "\\n042"                                                  //1-Tipo De Registro
                            + "|" + j.ToString()                                   //2-Consecutivo Concepto
                            + "|" + lista[13].ToString().Trim()                    //3-Impuesto
                            + "|" + lista[14].ToString().Trim()                    //4-Tipo Factor
                            + "|" + "0.060000"                //5-Tasa o Cuota
                            + "|" + (Convert.ToDecimal(lista[5].ToString().Trim()) * Convert.ToDecimal("0.06")).ToString()                  //7-Importe
                            + "|" + lista[5].ToString().Trim()                     //8-Base Calculo
                            + "|";

                        }
                        else
                        {
                            escritor.WriteLine(
                            "042"                                                  //1-Tipo De Registro
                            + "|" + j.ToString()                                   //2-Consecutivo Concepto
                            + "|" + lista[13].ToString().Trim()                    //3-Impuesto
                            + "|" + lista[14].ToString().Trim()                    //4-Tipo Factor
                            + "|" + lista[12].ToString().Trim()                    //5-Tasa o Cuota
                            + "|" + lista[15].ToString().Trim()                    //7-Importe
                            + "|" + lista[5].ToString().Trim()                     //8-Base Calculo
                            + "|"                                                  //Fin del Registro
                            );
                            escrituraFactura += "\\n042"                                                  //1-Tipo De Registro
                            + "|" + j.ToString()                                   //2-Consecutivo Concepto
                            + "|" + lista[13].ToString().Trim()                    //3-Impuesto
                            + "|" + lista[14].ToString().Trim()                    //4-Tipo Factor
                            + "|" + lista[12].ToString().Trim()                    //5-Tasa o Cuota
                            + "|" + lista[15].ToString().Trim()                    //7-Importe
                            + "|" + lista[5].ToString().Trim()                     //8-Base Calculo
                            + "|";
                        }
                    }

                    j++;   //Incremento del foreach que recorre la tabla Vista_fe_detail
                }

                //----------------------------------------Seccion de detalles de la factura leidos desde vista_fe_detail-------------------------------------------------------------------

                //06 DATOS DE LOS IMPUESTOS TRASLADADOS (0:N)
                //if (moneda == "USD" && (idreceptor == "WERGLOBA" || idreceptor == "SAYER"|| idreceptor == "GOLTOR" || idreceptor == "TRPLACE" || idreceptor == "TRPLACE"))

                escritor.WriteLine(
                "06"                                                   //1-Tipo de Registro
                + "|" + coditrans                                        //2-Impuesto
                + "|" + tipofactor                                     //3-Tipo Factor
                + "|" + tasatras                                       //4-Tasa o Cuota
                + "|" + txtIVA.Text.Trim()                             //5-Importe
                + "|"                                                  //Fin Del Registro
                );
                escrituraFactura += "\\n06"                                                   //1-Tipo de Registro
                   + "|" + coditrans                                      //2-Impuesto
                   + "|" + tipofactor                                     //3-Tipo Factor
                   + "|" + tasatras                                       //4-Tasa o Cuota
                   + "|" + txtIVA.Text.Trim()                             //5-Importe
                   + "|";


                //Si el valor del importe de la retencion es diferente de 0.00 se escribe el registro de datos de los impuestos retenidos
                if (txtRetencion.Text.Trim() != "")
                {

                    //07 DATOS DE LOS IMPUESTOS RETENIDOS (0:N)
                    escritor.WriteLine(
                  "07"                                                   //1-Tipo de Registro
                  + "|" + codirete                                       //2-Impuesto
                  + "|" + tasarete                                       //3-Tasa o Cuota
                  + "|" + txtRetencion.Text.Trim()                               //4-Importe
                  + "|"                                                  //Fin Del Registro
                  );
                    escrituraFactura += "\\n07"                                                   //1-Tipo de Registro
                  + "|" + codirete                                       //2-Impuesto
                  + "|" + tasarete                                       //3-Tasa o Cuota
                  + "|" + txtRetencion.Text.Trim()                               //4-Importe
                  + "|";
                }



                //----------------------------------------Seccion de datos extra para PDF CFDI-------------------------------------------------------------------------------------------

                //08 INFORMACION ADICIONAL COMPROBANTE (0:N)
                escritor.WriteLine("08|Origen|" + txtOrigen.Text.Trim() + "|");
                escritor.WriteLine("08|OrigenRemitente|" + txtOrigenRemitente.Text.Trim() + "|");
                //escritor.WriteLine("08|DomicilioOrigen|" + txtDomicilioOrigen.Text.Trim() + "|");
                escritor.WriteLine("08|DomicilioOrigen|" + "" + "|");
                escritor.WriteLine("08|RfcOrigen|" + txtRFCOrigen.Text.Trim() + "|");
                escritor.WriteLine("08|Destino|" + txtDestino.Text.Trim() + "|");
                escritor.WriteLine("08|Destinatario|" + txtDestinatario.Text.Trim() + "|");
                //escritor.WriteLine("08|Domicilio|" + txtDomicilioDestino.Text.Trim() + "|");
                escritor.WriteLine("08|Domicilio|" + "" + "|");
                escritor.WriteLine("08|RfcDestino|" + txtRFCDestino.Text.Trim() + "|");
                escritor.WriteLine("08|CuotaConvenida|" + txtCuotaConvenida.Text.Trim() + "|");
                escritor.WriteLine("08|ValorComercial|" + txtValorComercial.Text.Trim() + "|");
                escritor.WriteLine("08|Remision|" + txtRemisión.Text.Trim() + "|");
                escritor.WriteLine("08|Contiene|" + txtContine.Text.Trim() + "|");
                escritor.WriteLine("08|Comentario|" + txtComentarios.Text.Trim() + "|");
                escritor.WriteLine("08|AmparaRemisiones|" + txtAmparaRemisiones.Text.Trim() + "|");
                escritor.WriteLine("08|PesoEstimado|" + txtPeso.Text.Trim() + "|");
                escritor.WriteLine("08|Orden|" + txtOrden.Text.Trim() + "|");
                escritor.WriteLine("08|Master|" + txtMb.Text.Trim() + "|");
                escritor.WriteLine("08|Invoice|" + txtInvoice.Text.Trim() + "|");
                escritor.WriteLine("08|Movimiento|" + txtMovimiento.Text.Trim() + "|");
                escritor.WriteLine("08|Operador|" + txtOperador.Text.Trim() + "|");
                escritor.WriteLine("08|OperadorLicencia|" + txtLicencia.Text.Trim() + "|");
                escritor.WriteLine("08|TractoEco|" + txtTractorEco.Text.Trim() + "|");
                escritor.WriteLine("08|TractoPlaca|" + txtTractorPlacas.Text.Trim() + "|");
                escritor.WriteLine("08|Remolque1Eco|" + txtRemol1Eco.Text.Trim() + "|");
                escritor.WriteLine("08|Remolque1Placa|" + txtRemolque1Placas.Text.Trim() + "|");
                escritor.WriteLine("08|Remolque2Eco|" + txtRemolque2Eco.Text.Trim() + "|");
                escritor.WriteLine("08|Remolque2Placa|" + txtRemolque2Placas.Text.Trim() + "|");
                escritor.WriteLine("08|Documento|" + txtDocumento.Text.Trim() + "|");
                escritor.WriteLine("08|FechaPagare|" + txtFechaPagare.Text.Trim() + "|");
                escritor.WriteLine("08|InteresesPagare|" + txtInteres.Text.Trim() + "|");
                escritor.WriteLine("08|ClientePagare|" + txtClientePagare.Text.Trim() + "|");
                escritor.WriteLine("08|Flete|" + txtFlete.Text.Trim() + "|");
                escritor.WriteLine("08|Seguro|" + txtCargaXSeguro.Text.Trim() + "|");
                escritor.WriteLine("08|Maniobras|" + txtManiobras.Text.Trim() + "|");
                escritor.WriteLine("08|Pistas|" + txtAutopistas.Text.Trim() + "|");
                escritor.WriteLine("08|RentaEquipo|" + txtEquipo.Text.Trim() + "|");
                escritor.WriteLine("08|Cpac|" + txtCPAC.Text.Trim() + "|");


                escrituraFactura += "\\n08|Origen|" + txtOrigen.Text.Trim() + "|";
                escrituraFactura += "\\n08|OrigenRemitente|" + txtOrigenRemitente.Text.Trim() + "|";
                //escrituraFactura +="\\n08|DomicilioOrigen|" + txtDomicilioOrigen.Text.Trim() + "|";
                escrituraFactura += "\\n08|DomicilioOrigen|" + "" + "|";
                escrituraFactura += "\\n08|RfcOrigen|" + txtRFCOrigen.Text.Trim() + "|";
                escrituraFactura += "\\n08|Destino|" + txtDestino.Text.Trim() + "|";
                escrituraFactura += "\\n08|Destinatario|" + txtDestinatario.Text.Trim() + "|";
                //escrituraFactura +="\\n08|Domicilio|" + txtDomicilioDestino.Text.Trim() + "|";
                escrituraFactura += "\\n08|Domicilio|" + "" + "|";
                escrituraFactura += "\\n08|RfcDestino|" + txtRFCDestino.Text.Trim() + "|";
                escrituraFactura += "\\n08|CuotaConvenida|" + txtCuotaConvenida.Text.Trim() + "|";
                escrituraFactura += "\\n08|ValorComercial|" + txtValorComercial.Text.Trim() + "|";
                escrituraFactura += "\\n08|Remision|" + txtRemisión.Text.Trim() + "|";
                escrituraFactura += "\\n08|Contiene|" + txtContine.Text.Trim() + "|";
                escrituraFactura += "\\n08|Comentario|" + txtComentarios.Text.Trim() + "|";
                escrituraFactura += "\\n08|AmparaRemisiones|" + txtAmparaRemisiones.Text.Trim() + "|";
                escrituraFactura += "\\n08|PesoEstimado|" + txtPeso.Text.Trim() + "|";
                escrituraFactura += "\\n08|Orden|" + txtOrden.Text.Trim() + "|";
                escrituraFactura += "\\n08|Master|" + txtMb.Text.Trim() + "|";
                escrituraFactura += "\\n08|Invoice|" + txtInvoice.Text.Trim() + "|";
                escrituraFactura += "\\n08|Movimiento|" + txtMovimiento.Text.Trim() + "|";
                escrituraFactura += "\\n08|Operador|" + txtOperador.Text.Trim() + "|";
                escrituraFactura += "\\n08|OperadorLicencia|" + txtLicencia.Text.Trim() + "|";
                escrituraFactura += "\\n08|TractoEco|" + txtTractorEco.Text.Trim() + "|";
                escrituraFactura += "\\n08|TractoPlaca|" + txtTractorPlacas.Text.Trim() + "|";
                escrituraFactura += "\\n08|Remolque1Eco|" + txtRemol1Eco.Text.Trim() + "|";
                escrituraFactura += "\\n08|Remolque1Placa|" + txtRemolque1Placas.Text.Trim() + "|";
                escrituraFactura += "\\n08|Remolque2Eco|" + txtRemolque2Eco.Text.Trim() + "|";
                escrituraFactura += "\\n08|Remolque2Placa|" + txtRemolque2Placas.Text.Trim() + "|";
                escrituraFactura += "\\n08|Documento|" + txtDocumento.Text.Trim() + "|";
                escrituraFactura += "\\n08|FechaPagare|" + txtFechaPagare.Text.Trim() + "|";
                escrituraFactura += "\\n08|InteresesPagare|" + txtInteres.Text.Trim() + "|";
                escrituraFactura += "\\n08|ClientePagare|" + txtClientePagare.Text.Trim() + "|";
                escrituraFactura += "\\n08|Flete|" + txtFlete.Text.Trim() + "|";
                escrituraFactura += "\\n08|Seguro|" + txtCargaXSeguro.Text.Trim() + "|";
                escrituraFactura += "\\n08|Maniobras|" + txtManiobras.Text.Trim() + "|";
                escrituraFactura += "\\n08|Pistas|" + txtAutopistas.Text.Trim() + "|";
                escrituraFactura += "\\n08|RentaEquipo|" + txtEquipo.Text.Trim() + "|";
                escrituraFactura += "\\n08|Cpac|" + txtCPAC.Text.Trim() + "|";

                //----------------------------------------Seccion de datos de relacion de CFDI-------------------------------------------------------------------------------------------

                //SI CONTAMOS CON UN UUID DE RELACION SE ESCRIBE EL REGISTRO
                if ((txtUUIDREL.Text != ("")) && (txtRelacion.Text != ("")))
                {
                    //09 DATOS DE CFDIS RELACIOnADOS(0:N)
                    escritor.WriteLine(
                    "09"                                                      //1-Tipo de Registro
                    + "|" + relac                                             //2-Tipo de Relacion
                    + "|" + uuidrel                                           //3-UUID
                    + "|"                                                     //Fin del Registro
                    );

                    escrituraFactura += "\\n09"                                                      //1-Tipo de Registro
                    + "|" + relac                                             //2-Tipo de Relacion
                    + "|" + uuidrel                                           //3-UUID
                    + "|";

                    relac = null;
                    uuidrel = null;
                    txtUUIDREL.Text = "";
                    txtRelacion.Text = "";

                }

                //----------------------------------------Seccion Adenda para billtos del grupo LIVERPOOL-------------------------------------------------------------------------------

                if (clienteId.Equals("LIVERPOL") || clienteId.Equals("ALMLIVER") || clienteId.Equals("LIVERTIJ") || clienteId.Equals("SFERALIV") || clienteId.Equals("GLOBALIV") || clienteId.Equals("SETRALIV") || clienteId.Equals("FACTUMLV") || clienteId.Equals("LIVERDED"))
                //if (clienteId.Equals("LIVERPOL") || clienteId.Equals("ALMLIVER") || clienteId.Equals("LIVERTIJ"))
                {
                    escritor.WriteLine("08|Otros|" + txtOtros.Text.Trim() + "|");
                    escrituraFactura += "\\n08|Otros|" + txtOtros.Text.Trim() + "|";


                    //if (true)
                    //{
                    DateTime fFactura = Convert.ToDateTime(txtFecha.Text.Trim());
                    string fechaTxt = fFactura.Year + "-" + fFactura.Month + "-" + fFactura.Day;

                    escritor.WriteLine("20|ORIGINAL||ZZZ|" + txtCantidadLetra.Text.Trim() + "|||||||" + fechaTxt.Trim() + "|" + fechaTxt.Trim() + "||||||");
                    escritor.WriteLine("21|" + txtPedido.Text.Trim() + "|");
                    escritor.WriteLine("22|4310107|SELLER_ASSIGNED_IDENTIFIER_FOR_A_PARTY|4310107||||||||");
                    escritor.WriteLine("24|" + moneda.Trim() + "|BILLING CURRENCY|" + String.Format("{0:0.00}", double.Parse(tipoCambio.Trim())) + "|");
                    escritor.WriteLine("25|ALLOWANCE_GLOBAL|BILL_BACK||AJ|0.00|0.00");

                    escrituraFactura += "\\n20|ORIGINAL||ZZZ|" + txtCantidadLetra.Text.Trim() + "|||||||" + fechaTxt.Trim() + "|" + fechaTxt.Trim() + "||||||";
                    escrituraFactura += "\\n21|" + txtPedido.Text.Trim() + "|";
                    escrituraFactura += "\\n22|4310107|SELLER_ASSIGNED_IDENTIFIER_FOR_A_PARTY|4310107||||||||";
                    escrituraFactura += "\\n24|" + moneda.Trim() + "|BILLING CURRENCY|" + String.Format("{0:0.00}", double.Parse(tipoCambio.Trim())) + "|";
                    escrituraFactura += "\\n25|ALLOWANCE_GLOBAL|BILL_BACK||AJ|0.00|0.00";

                    for (int i = 1; i <= concept.Count; i++)
                    {
                        escritor.WriteLine("26|" + i.ToString() + "|" + txtFactura.Text.Trim() + "|" + txtFactura.Text.Trim() + "|SUPPLIER_ASSIGNED|ES|" + String.Format("{0:0.00}", double.Parse(txtSubtotal.Text.Trim())) +
                            "|" + String.Format("{0:0.00}", double.Parse(txtTotal.Text.Trim())) + "|" + String.Format("{0:0.00}", double.Parse(txtSubtotal.Text.Trim())) + "|" + String.Format("{0:0.00}", double.Parse(txtTotal.Text.Trim())) + "|SRV|1||PAID_BY_BUYER||");
                        escrituraFactura += "\\n26|" + i.ToString() + "|" + txtFactura.Text.Trim() + "|" + txtFactura.Text.Trim() + "|SUPPLIER_ASSIGNED|ES|" + String.Format("{0:0.00}", double.Parse(txtSubtotal.Text.Trim())) +
                            "|" + String.Format("{0:0.00}", double.Parse(txtTotal.Text.Trim())) + "|" + String.Format("{0:0.00}", double.Parse(txtSubtotal.Text.Trim())) + "|" + String.Format("{0:0.00}", double.Parse(txtTotal.Text.Trim())) + "|SRV|1||PAID_BY_BUYER||";

                    }

                    escritor.WriteLine("29|ALLOWANCE||" + String.Format("{0:0.00}", double.Parse(txtSubtotal.Text.Trim())) + "|");
                    escritor.WriteLine("30|174542|ATZ|");
                    escritor.WriteLine("31|" + txtHojaEntrada.Text.Trim() + "|");
                    escritor.Write("32|7504000107903|0101|");

                    escrituraFactura += "\\n29|ALLOWANCE||" + String.Format("{0:0.00}", double.Parse(txtSubtotal.Text.Trim())) + "|";
                    escrituraFactura += "\\n30|174542|ATZ|";
                    escrituraFactura += "\\n31|" + txtHojaEntrada.Text.Trim() + "|";
                    escrituraFactura += "\\n32|7504000107903|0101|";
                }

                //----------------------------------------Seccion Adenda MABE------------------------------------------------------------------------------------------------------------

                else if (clienteId.Equals("MABE"))
                {

                    string ttcompre = "";

                    //Dependiendo del tipo de factura inicializa la variable de "tipocompro" 
                    if (serie.Equals("NCT"))
                    {
                        ttcompre = "NOTA DE CREDITO";
                    }
                    else if (serie.Equals("TDRT"))
                    {

                        ttcompre = "FACTURA";
                    }

                    //escritor.WriteLine("");
                    escritor.WriteLine("08|VER|1.0|");   //texto que contiene la version de la addenda MABE
                    escritor.WriteLine("08|TDF|" + ttcompre + "|"); //texto que tenga si es FACTURA O NOTA DE CREDITO********LISTO
                    escritor.WriteLine("08|ORDC|NA|"); //numero de la orden de compra siempre va en NA********LISTO
                    escritor.WriteLine("08|REF1|" + txtComentarios.Text.Trim() + "|"); //el num de la referencia. es el de comentarios********LISTO
                    escritor.WriteLine("08|REF2|NA|"); //el num de la referencia 2 siempre va en NA********LISTO
                    escritor.WriteLine("08|NPROV|3000078|"); //el num de proveedor 2 siempre va en 3000078*******LISTO
                    escritor.WriteLine("08|NPLANT|D004|"); //el num de la planta siempre va en D004*******LISTO
                    escritor.WriteLine("08|DPLANT|PASEO DE LAS PALMAS|"); //la direccion de la planta*******LISTO 
                    escritor.WriteLine("08|NEXTPLANT|100|"); //siempre en na*******LISTO
                    escritor.WriteLine("08|NINPLANT|NA|"); //siempre en na*******LISTO
                    escritor.WriteLine("08|CPLANT|11000|"); //el codigo postal de la planta*******LISTO
                    escritor.WriteLine("08|MONEDA|MXN|"); //el codigo postal de la planta*******LISTO

                    escrituraFactura += "\\n08|VER|1.0|";   //texto que contiene la version de la addenda MABE
                    escrituraFactura += "\\n08|TDF|" + ttcompre + "|"; //texto que tenga si es FACTURA O NOTA DE CREDITO********LISTO
                    escrituraFactura += "\\n08|ORDC|NA|"; //numero de la orden de compra siempre va en NA********LISTO
                    escrituraFactura += "\\n08|REF1|" + txtComentarios.Text.Trim() + "|"; //el num de la referencia. es el de comentarios********LISTO
                    escrituraFactura += "\\n08|REF2|NA|"; //el num de la referencia 2 siempre va en NA********LISTO
                    escrituraFactura += "\\n08|NPROV|3000078|"; //el num de proveedor 2 siempre va en 3000078*******LISTO
                    escrituraFactura += "\\n08|NPLANT|D004|"; //el num de la planta siempre va en D004*******LISTO
                    escrituraFactura += "\\n08|DPLANT|PASEO DE LAS PALMAS|"; //la direccion de la planta*******LISTO 
                    escrituraFactura += "\\n08|NEXTPLANT|100|"; //siempre en na*******LISTO
                    escrituraFactura += "\\n08|NINPLANT|NA|"; //siempre en na*******LISTO
                    escrituraFactura += "\\n08|CPLANT|11000|"; //el codigo postal de la planta*******LISTO
                    escrituraFactura += "\\n08|MONEDA|MXN|"; //el codigo postal de la planta*******LISTO



                    //for (int i = 1; i <= concept.Count; i++)
                    //{
                    //    double monto = 0;

                    //    if (txtFactura.ToString().Trim() == "FLETE") { monto = double.Parse(txtTotal.ToString().Trim()) * 1.12; } else { monto = double.Parse(txtTotal.ToString().Trim()) * 1.16; }

                    //    escritor.WriteLine("05|" + i.ToString() + "|NLINEA|" + i.ToString() + "|");
                    //    escritor.WriteLine("05|" + i.ToString() + "|" + txtFactura.Text.Trim().Substring(0, 1) + "|" + String.Format("{0:0.00}", double.Parse(txtSubtotal.Text.Trim())) + "|"); //texto que tenga COSTO SIN IVA IMPUESTOS ***EN PRUEBA
                    //    escritor.WriteLine("05|" + i.ToString() + "|PCONIVA|" + String.Format("{0:0.00}", monto) + "|"); // COSTO CON IVA IMPUESTOS ***EN PRUEBA
                    //    escritor.WriteLine("05|" + i.ToString() + "|SSINIVA|" + String.Format("{0:0.00}", double.Parse(txtSubtotal.Text.Trim())) + "|"); //texto que tenga subtotal SIN IVA IMPUESTOS ***EN PRUEBA
                    //    escritor.WriteLine("05|" + i.ToString() + "|SCONIVA|" + String.Format("{0:0.00}", monto) + "|"); // COSTO subtotal con IVA IMPUESTOS ***EN PRUE
                    //}

                    int p = 1;
                    foreach (int item in concept.Keys)
                    {
                        ArrayList listas = (ArrayList)concept[item];
                        double monto = 0;

                        if (listas[3].ToString().Trim() == "FLETE") { monto = double.Parse(listas[5].ToString().Trim()) * 1.12; } else { monto = double.Parse(listas[5].ToString().Trim()) * 1.16; }

                        escritor.WriteLine("044|" + p.ToString() + "|NLINEA|" + p.ToString() + "|");
                        escritor.WriteLine("044|" + p.ToString() + "|" + listas[2].ToString().Trim().Substring(0, 1) + "|" + String.Format("{0:0.00}", double.Parse(listas[5].ToString().Trim())) + "|"); //texto que tenga COSTO SIN IVA IMPUESTOS ***EN PRUEBA
                        escritor.WriteLine("044|" + p.ToString() + "|PCONIVA|" + String.Format("{0:0.00}", monto) + "|"); // COSTO CON IVA IMPUESTOS ***EN PRUEBA
                        escritor.WriteLine("044|" + p.ToString() + "|PSINIVA|" + String.Format("{0:0.00}", double.Parse(listas[5].ToString().Trim())) + "|"); // COSTO CON IVA IMPUESTOS ***EN PRUEBA
                        escritor.WriteLine("044|" + p.ToString() + "|SSINIVA|" + String.Format("{0:0.00}", double.Parse(listas[5].ToString().Trim())) + "|"); //texto que tenga subtotal SIN IVA IMPUESTOS ***EN PRUEBA
                        escritor.WriteLine("044|" + p.ToString() + "|SCONIVA|" + String.Format("{0:0.00}", monto) + "|"); // COSTO subtotal con IVA IMPUESTOS ***EN PRUE
                        escritor.WriteLine("044|" + p.ToString() + "|COD|" + listas[2].ToString().Trim().Substring(0, 1) + "|");

                        escrituraFactura += "\\n044|" + p.ToString() + "|NLINEA|" + p.ToString() + "|";
                        escrituraFactura += "\\n044|" + p.ToString() + "|" + listas[2].ToString().Trim().Substring(0, 1) + "|" + String.Format("{0:0.00}", double.Parse(listas[5].ToString().Trim())) + "|"; //texto que tenga COSTO SIN IVA IMPUESTOS ***EN PRUEBA
                        escrituraFactura += "\\n044|" + p.ToString() + "|PCONIVA|" + String.Format("{0:0.00}", monto) + "|"; // COSTO CON IVA IMPUESTOS ***EN PRUEBA
                        escrituraFactura += "\\n044|" + p.ToString() + "|PSINIVA|" + String.Format("{0:0.00}", double.Parse(listas[5].ToString().Trim())) + "|"; // COSTO CON IVA IMPUESTOS ***EN PRUEBA
                        escrituraFactura += "\\n044|" + p.ToString() + "|SSINIVA|" + String.Format("{0:0.00}", double.Parse(listas[5].ToString().Trim())) + "|"; //texto que tenga subtotal SIN IVA IMPUESTOS ***EN PRUEBA
                        escrituraFactura += "\\n044|" + p.ToString() + "|SCONIVA|" + String.Format("{0:0.00}", monto) + "|"; // COSTO subtotal con IVA IMPUESTOS ***EN PRUE
                        escrituraFactura += "\\n044|" + p.ToString() + "|COD|" + listas[2].ToString().Trim().Substring(0, 1) + "|";

                        p++;
                    }



                }

                //----------------------------------------Seccion Adenda Envases Universales------------------------------------------------------------------------------------------------------------

                else if (clienteId.Equals("ENVASESU"))
                {



                    string ttcompre = "";
                    string diafecha = "";
                    string mesfecha = "";
                    //Dependiendo del tipo de factura inicializa la variable de "tipocompro" 
                    if (serie.Equals("NCT"))
                    {
                        ttcompre = "CREDIT_NOTE";
                    }
                    else if (serie.Equals("TDRT"))
                    {

                        ttcompre = "INVOICE";
                    }

                    DateTime hfFactura = Convert.ToDateTime(txtFecha.Text.Trim());

                    //validamos si el dia y el mes son menores a 10 para que les agrege un 0 ya que pide 2 digitos.

                    if (hfFactura.Day < 10)
                    {
                        diafecha = "0" + hfFactura.Day;
                    }
                    else
                    {
                        diafecha = "" + hfFactura.Day;
                    }



                    if (hfFactura.Month < 10)
                    {
                        mesfecha = "0" + hfFactura.Month;
                    }
                    else
                    {
                        mesfecha = "" + hfFactura.Month;
                    }


                    //construimos el texto completo con el formato de fecha solicitado
                    string hfechaTxt = hfFactura.Year + "-" + mesfecha + "-" + diafecha;


                    escritor.WriteLine("08|Version|1.0|");
                    escritor.WriteLine("08|IdFactura|Factura|");
                    escritor.WriteLine("08|FechaMensaje|" + hfechaTxt + "|");   //texto que tenga la hora y fecha de emision. ********LISTO
                    escritor.WriteLine("08|IdTransaccion|Con_Pedido");
                    escritor.WriteLine("08|Transaccion|" + fact + "|"); //texto que tenga el folio. ********LISTO
                    escritor.WriteLine("08|SecuenciaConsec|1|");
                    escritor.WriteLine("08|IdPedido|" + txtRemisión.Text.Trim() + "|");
                    escritor.WriteLine("08|Albaran|" + txtComentarios.Text.Trim() + "|");

                    escrituraFactura += "\\n08|Version|1.0|";
                    escrituraFactura += "\\n08|IdFactura|Factura|";
                    escrituraFactura += "\\n08|FechaMensaje|" + hfechaTxt + "|";   //texto que tenga la hora y fecha de emision. ********LISTO
                    escrituraFactura += "\\n08|IdTransaccion|Con_Pedido";
                    escrituraFactura += "\\n08|Transaccion|" + fact + "|"; //texto que tenga el folio. ********LISTO
                    escrituraFactura += "\\n08|SecuenciaConsec|1|";
                    escrituraFactura += "\\n08|IdPedido|" + txtRemisión.Text.Trim() + "|";
                    escrituraFactura += "\\n08|Albaran|" + txtComentarios.Text.Trim() + "|";

                }


                //----------------------------------------Seccion Adenda Home Depot------------------------------------------------------------------------------------------------------------

                else if (clienteId.Equals("HOMEDEP"))
                {



                    string ttcompre = "";
                    string diafecha = "";
                    string mesfecha = "";
                    //Dependiendo del tipo de factura inicializa la variable de "tipocompro" 
                    if (serie.Equals("NCT"))
                    {
                        ttcompre = "CREDIT_NOTE";
                    }
                    else if (serie.Equals("TDRT"))
                    {

                        ttcompre = "INVOICE";
                    }

                    DateTime hfFactura = Convert.ToDateTime(txtFecha.Text.Trim());

                    //validamos si el dia y el mes son menores a 10 para que les agrege un 0 ya que pide 2 digitos.

                    if (hfFactura.Day < 10)
                    {
                        diafecha = "0" + hfFactura.Day;
                    }
                    else
                    {
                        diafecha = "" + hfFactura.Day;
                    }



                    if (hfFactura.Month < 10)
                    {
                        mesfecha = "0" + hfFactura.Month;
                    }
                    else
                    {
                        mesfecha = "" + hfFactura.Month;
                    }


                    //construimos el texto completo con el formato de fecha solicitado
                    string hfechaTxt = hfFactura.Year + "-" + mesfecha + "-" + diafecha;


                    escritor.WriteLine("08|Otros|" + txtOtros.Text.Trim() + "|");
                    escritor.WriteLine("08|RFPT|SimpleInvoiceType|");
                    escritor.WriteLine("08|RFPCV|1.3.1|");
                    escritor.WriteLine("08|RFPDSV|AMC7.1|");
                    escritor.WriteLine("08|RFPDS|ORIGINAL|");
                    escritor.WriteLine("08|RFPDD|" + hfechaTxt + "|");   //texto que tenga la hora y fecha de emision. ********LISTO
                    escritor.WriteLine("08|RFPIET|" + ttcompre + "|"); //texto que tenga si es INVOICE o  CREDIT_NOTE ********LISTO
                    escritor.WriteLine("08|RFPIUCI|" + serie + fact + "|"); //texto que tenga la serie mas el folio. ********LISTO
                    escritor.WriteLine("08|OIRI|" + txtComentarios.Text.Trim() + "|"); //texto que tenga el num de la orden de compra. ********LISTO
                    escritor.WriteLine("08|OIT|ON|");
                    escritor.WriteLine("08|AIRI|" + txtComentarios.Text.Trim() + "|"); //el num de la orden de compra. es el de comentarios********LISTO
                    escritor.WriteLine("08|AIT|ON|");
                    escritor.WriteLine("08|BG|0007504005499|");
                    escritor.WriteLine("08|SG|0000081008775|");
                    escritor.WriteLine("08|SAPI|81008775|");
                    escritor.WriteLine("08|SAPIT|SELLER_ASSIGNED_IDENTIFIER_FOR_A_PARTY|");
                    escritor.WriteLine("08|CCIC|MXN|");
                    escritor.WriteLine("08|CCF|BILLING_CURRENCY|");
                    escritor.WriteLine("08|CROC|1.00|");

                    escritor.WriteLine("044|1|LIT|SimpleInvoiceLineItemType|");
                    escritor.WriteLine("044|1|LIN|1|");
                    escritor.WriteLine("044|1|TIIG|" + txtRemisión.Text.Trim() + "|"); //num de la referencia. buscar cual es ********LISTO
                    escritor.WriteLine("044|1|ATIIT|SUPPLIER_ASSIGNED|" + txtRemisión.Text.Trim() + "|"); //num de la referencia. buscar cual es ********LISTO // SUPPLIER_ASSIGNED added cambios 9/18/2018 by erik
                                                                                                          //escritor.WriteLine("044|ATIIT|BUYER_ASSIGNED|");
                    escritor.WriteLine("044|1|TIDIL|ES|");
                    escritor.WriteLine("044|1|TIDILLT|FLETE|");
                    escritor.WriteLine("044|1|IQ|1.00|");
                    escritor.WriteLine("044|1|IQUOM|AP|");    //AU cambios 9/18/2018 by erik
                    escritor.WriteLine("044|1|GPA|" + String.Format("{0:0.00}", double.Parse(txtSubtotal.Text.Trim())) + "|"); //texto que tenga COSTO SIN IVA IMPUESTOS ***EN PRUEBA
                    escritor.WriteLine("044|1|PIPQ|1|");
                    escritor.WriteLine("044|1|PID|EACH|");
                    escritor.WriteLine("044|1|PIT|EXCHANGE_PALLETS|");
                    escritor.WriteLine("044|1|PITRMOP|PREPAID_BY_SELLER|");
                    escritor.WriteLine("044|1|TITITTD|VAT|");
                    escritor.WriteLine("044|1|TITITITATP|16.00|"); //texto que tenga EL % DE IVA A APLICAR ***EN PRUEBA
                    escritor.WriteLine("044|1|TITITITATA|" + String.Format("{0:0.00}", double.Parse(txtIVA.Text.Trim())) + "|"); //texto que tenga EL MONTO DE IVA A APLICAR ***EN PRUEBA
                    escritor.WriteLine("044|1|TLANAA|" + String.Format("{0:0.00}", double.Parse(txtSubtotal.Text.Trim())) + "|"); // COSTO SIN IVA IMPUESTOS (COSTO UNIT * UNIDADES)***EN PRUEBA


                    escritor.WriteLine("08|TACAOCT|ALLOWANCE|");
                    escritor.WriteLine("08|TACAOCTA|0.00|");
                    escritor.WriteLine("08|BAA|" + String.Format("{0:0.00}", double.Parse(txtSubtotal.Text.Trim())) + "|"); //subtotal (SUMA DE TODAS LAS LINEAS)********LISTO
                    if (txtRetencion.Text.Trim() == "")
                    {
                        txtRetencion.Text = "0";
                    }
                    escritor.WriteLine("13|VAT|4.00|" + String.Format("{0:0.00}", double.Parse(txtRetencion.Text.Trim())) + "|RETENIDO|"); //texto que tenga EL TOTAL DE LAS RETENCIONES ********LISTO
                    escritor.WriteLine("13|VAT|16.00|" + String.Format("{0:0.00}", double.Parse(txtIVA.Text.Trim())) + "|TRANSFERIDO|"); //texto que tenga EL TOTAL DEL IVA ********LISTO

                    escritor.Write("08|PAA|" + String.Format("{0:0.00}", double.Parse(txtTotal.Text.Trim())) + "|"); //texto que tenga COSTO TOTAL DE TODO CON IVA Y RETENCION. ********LISTO

                    // texto en variable
                    escrituraFactura += "\\n08|Otros|" + txtOtros.Text.Trim() + "|";
                    escrituraFactura += "\\n08|RFPT|SimpleInvoiceType|";
                    escrituraFactura += "\\n08|RFPCV|1.3.1|";
                    escrituraFactura += "\\n08|RFPDSV|AMC7.1|";
                    escrituraFactura += "\\n08|RFPDS|ORIGINAL|";
                    escrituraFactura += "\\n08|RFPDD|" + hfechaTxt + "|";   //texto que tenga la hora y fecha de emision. ********LISTO
                    escrituraFactura += "\\n08|RFPIET|" + ttcompre + "|"; //texto que tenga si es INVOICE o  CREDIT_NOTE ********LISTO
                    escrituraFactura += "\\n08|RFPIUCI|" + serie + fact + "|"; //texto que tenga la serie mas el folio. ********LISTO
                    escrituraFactura += "\\n08|OIRI|" + txtComentarios.Text.Trim() + "|"; //texto que tenga el num de la orden de compra. ********LISTO
                    escrituraFactura += "\\n08|OIT|ON|";
                    escrituraFactura += "\\n08|AIRI|" + txtComentarios.Text.Trim() + "|"; //el num de la orden de compra. es el de comentarios********LISTO
                    escrituraFactura += "\\n08|AIT|ON|";
                    escrituraFactura += "\\n08|BG|0007504005499|";
                    escrituraFactura += "\\n08|SG|0000081008775|";
                    escrituraFactura += "\\n08|SAPI|81008775|";
                    escrituraFactura += "\\n08|SAPIT|SELLER_ASSIGNED_IDENTIFIER_FOR_A_PARTY|";
                    escrituraFactura += "\\n08|CCIC|MXN|";
                    escrituraFactura += "\\n08|CCF|BILLING_CURRENCY|";
                    escrituraFactura += "\\n08|CROC|1.00|";

                    escrituraFactura += "\\n044|1|LIT|SimpleInvoiceLineItemType|";
                    escrituraFactura += "\\n044|1|LIN|1|";
                    escrituraFactura += "\\n044|1|TIIG|" + txtRemisión.Text.Trim() + "|"; //num de la referencia. buscar cual es ********LISTO
                    escrituraFactura += "\\n044|1|ATIIT|SUPPLIER_ASSIGNED|" + txtRemisión.Text.Trim() + "|"; //num de la referencia. buscar cual es ********LISTO // SUPPLIER_ASSIGNED added cambios 9/18/2018 by erik
                                                                                                             //escrituraFactura +="\\n044|ATIIT|BUYER_ASSIGNED|";
                    escrituraFactura += "\\n044|1|TIDIL|ES|";
                    escrituraFactura += "\\n044|1|TIDILLT|FLETE|";
                    escrituraFactura += "\\n044|1|IQ|1.00|";
                    escrituraFactura += "\\n044|1|IQUOM|AP|";    //AU cambios 9/18/2018 by erik
                    escrituraFactura += "\\n044|1|GPA|" + String.Format("{0:0.00}", double.Parse(txtSubtotal.Text.Trim())) + "|"; //texto que tenga COSTO SIN IVA IMPUESTOS ***EN PRUEBA
                    escrituraFactura += "\\n044|1|PIPQ|1|";
                    escrituraFactura += "\\n044|1|PID|EACH|";
                    escrituraFactura += "\\n044|1|PIT|EXCHANGE_PALLETS|";
                    escrituraFactura += "\\n044|1|PITRMOP|PREPAID_BY_SELLER|";
                    escrituraFactura += "\\n044|1|TITITTD|VAT|";
                    escrituraFactura += "\\n044|1|TITITITATP|16.00|"; //texto que tenga EL % DE IVA A APLICAR ***EN PRUEBA
                    escrituraFactura += "\\n044|1|TITITITATA|" + String.Format("{0:0.00}", double.Parse(txtIVA.Text.Trim())) + "|"; //texto que tenga EL MONTO DE IVA A APLICAR ***EN PRUEBA
                    escrituraFactura += "\\n044|1|TLANAA|" + String.Format("{0:0.00}", double.Parse(txtSubtotal.Text.Trim())) + "|"; // COSTO SIN IVA IMPUESTOS (COSTO UNIT * UNIDADES)***EN PRUEBA


                    escrituraFactura += "\\n08|TACAOCT|ALLOWANCE|";
                    escrituraFactura += "\\n08|TACAOCTA|0.00|";
                    escrituraFactura += "\\n08|BAA|" + String.Format("{0:0.00}", double.Parse(txtSubtotal.Text.Trim())) + "|"; //subtotal (SUMA DE TODAS LAS LINEAS)********LISTO

                    escrituraFactura += "\\n13|VAT|4.00|" + String.Format("{0:0.00}", double.Parse(txtRetencion.Text.Trim())) + "|RETENIDO|"; //texto que tenga EL TOTAL DE LAS RETENCIONES ********LISTO
                    escrituraFactura += "\\n13|VAT|16.00|" + String.Format("{0:0.00}", double.Parse(txtIVA.Text.Trim())) + "|TRANSFERIDO|"; //texto que tenga EL TOTAL DEL IVA ********LISTO

                    escrituraFactura += "\\n08|PAA|" + String.Format("{0:0.00}", double.Parse(txtTotal.Text.Trim())) + "|"; //texto que tenga COSTO TOTAL DE TODO CON IVA Y RETENCION. ********LISTO


                }
                else
                {
                    escritor.Write("\\n08|Otros|" + txtOtros.Text.Trim() + "|");
                    escrituraFactura += "\\n08|Otros|" + txtOtros.Text.Trim() + "|";
                }
            }
        }

        //----------------------------------------Seccion Funciones ---------------------------------------------------------------------------------------------------------------------

        public void generaDetalle()
        {
            detalle33.Clear();
            Hashtable campos33 = new Hashtable();
            table = new HtmlTable();
            DataTable td33 = facLabControler.detalle(lblFact.Text);
            int uy = 0;

            foreach (DataRow registro33 in td33.Rows)
            {
                //agregado por emolvera para campos nuevos cfdi ver 3.3
                string cantidad33 = registro33["cantidad"].ToString();
                string unidadmedida33 = registro33["unidadmedida33"].ToString();
                string idconcepto33 = registro33["idconcepto"].ToString();
                string descripcion33 = registro33["descripcion"].ToString();

                string valorunitario33 = registro33["valorunitario"].ToString().Remove(registro33["valorunitario"].ToString().Length - 2);
                string importe33 = registro33["Importe"].ToString().Remove(registro33["Importe"].ToString().Length - 2);
                string claveunidad33 = registro33["claveunidad"].ToString();
                string numidentificacion33 = registro33["numidentificacion"].ToString();
                string tasa_iva33 = registro33["tasa_iva"].ToString();
                string impuestoiva33 = registro33["impuestoiva"].ToString();
                string tipofactoriva33 = registro33["tipofactoriva"].ToString();
                string iva_monto33 = registro33["iva_monto"].ToString().Remove(registro33["iva_monto"].ToString().Length - 2);
                string tasa_ret33 = registro33["tasa_ret"].ToString();
                string impuestoret33 = registro33["impuestoret"].ToString();
                string tipofactorret33 = registro33["tipofactorret"].ToString();
                string ret_monto33 = registro33["ret_monto"].ToString().Remove(registro33["ret_monto"].ToString().Length - 2);

                ArrayList lst33 = new ArrayList();
                lst33.Add(cantidad33);
                lst33.Add(unidadmedida33);
                lst33.Add(descripcion33);   //por mal nombramiento en BD  idconcepto  es descripcion 
                lst33.Add(idconcepto33);    //por mal nombramiento en BD idconcepto es descripcion - lon invertimos aqui para escribir bien el registro en orden
                lst33.Add(valorunitario33);
                lst33.Add(importe33);
                lst33.Add(claveunidad33);
                lst33.Add(numidentificacion33);
                lst33.Add(tasa_iva33);
                lst33.Add(impuestoiva33);
                lst33.Add(tipofactoriva33);
                lst33.Add(iva_monto33);
                lst33.Add(tasa_ret33);
                lst33.Add(impuestoret33);
                lst33.Add(tipofactorret33);
                lst33.Add(ret_monto33);

                detalle33.Add(uy, lst33);

                uy++;

            }

            DataTable td = facLabControler.detalle(lblFact.Text);


            if (td.Rows.Count == 0)
            {
                LiteralControl texto = new LiteralControl("<b>No hay detalles.</b>");
                PlaceHolder1.Controls.Add(texto);
            }
            else
            {

                table.Border = 1;
                HtmlTableRow encabezado = new HtmlTableRow();
                HtmlTableCell cantidad = new HtmlTableCell();
                HtmlTableCell concepto = new HtmlTableCell();
                HtmlTableCell desc = new HtmlTableCell();
                HtmlTableCell valor = new HtmlTableCell();
                HtmlTableCell importe = new HtmlTableCell();
                HtmlTableCell unidadmedida = new HtmlTableCell();



                cantidad.Controls.Add(new LiteralControl("<b>Cantidad</b>"));
                encabezado.Cells.Add(cantidad);
                unidadmedida.Controls.Add(new LiteralControl("<b>Unidad Medida</b>"));
                encabezado.Cells.Add(unidadmedida);
                concepto.Controls.Add(new LiteralControl("<b>Concepto</b>"));
                encabezado.Cells.Add(concepto);
                desc.Controls.Add(new LiteralControl("<b>Descripción</b>"));
                encabezado.Cells.Add(desc);
                valor.Controls.Add(new LiteralControl("<b>Valor Unitario</b>"));
                encabezado.Cells.Add(valor);
                importe.Controls.Add(new LiteralControl("<b>Importe</b>"));
                encabezado.Cells.Add(importe);


                table.Rows.Add(encabezado);
                int j = 0;

                //AQUI ES DONDE RECORRE LA VISTA_FE_DETAILS
                foreach (DataRow registro in td.Rows)
                {


                    if (campos.ContainsKey(registro["descripcion"].ToString()))
                    {
                        double val = double.Parse(campos[registro["descripcion"].ToString()].ToString());
                        campos[registro["descripcion"].ToString()] = val + double.Parse(registro["importe"].ToString());
                    }
                    else
                    {
                        campos.Add(registro["descripcion"].ToString(), registro["importe"].ToString());
                    }
                    HtmlTableRow row = new HtmlTableRow();
                    int size = registro.ItemArray.Length;
                    int i;
                    for (i = 0; i < size; i++)
                    {
                        HtmlTableCell cell = new HtmlTableCell();
                        if (i == 3)
                        {
                            DropDownList list = new DropDownList();
                            list.ID = "" + j + "" + i;
                            list.CssClass = "readOnlyTextBox";
                            list.Enabled = false;
                            ListItem item = new ListItem();
                            if (registro.ItemArray[i].ToString().Equals("AUTOPISTAS"))
                            {
                                item = new ListItem("AUTOPISTAS", "AUTOPISTAS");
                                item.Enabled = true;
                                item.Selected = true;
                                list.Items.Add(item);
                                list.Items.Add(new ListItem("CARGO POR SEGURO", "CARGO POR SEGURO"));
                                list.Items.Add(new ListItem("CPAC", "CPAC"));
                                list.Items.Add(new ListItem("FLETE", "FLETE"));
                                list.Items.Add(new ListItem("MANIOBRAS", "MANIOBRAS"));
                                list.Items.Add(new ListItem("OTROS", "OTROS"));
                                list.Items.Add(new ListItem("RENTA EQUIPO", "RENTA EQUIPO"));

                            }
                            else if (registro.ItemArray[i].ToString().Equals("CARGO POR SEGURO"))
                            {
                                item = new ListItem("CARGO POR SEGURO", "CARGO POR SEGURO");
                                item.Enabled = true;
                                item.Selected = true;
                                list.Items.Add(item);
                                list.Items.Add(new ListItem("CARGO POR SEGURO", "CARGO POR SEGURO"));
                                list.Items.Add(new ListItem("CPAC", "CPAC"));
                                list.Items.Add(new ListItem("FLETE", "FLETE"));
                                list.Items.Add(new ListItem("MANIOBRAS", "MANIOBRAS"));
                                list.Items.Add(new ListItem("OTROS", "OTROS"));
                                list.Items.Add(new ListItem("RENTA EQUIPO", "RENTA EQUIPO"));
                            }
                            else if (registro.ItemArray[i].ToString().Equals("CPAC"))
                            {
                                item = new ListItem("CPAC", "CPAC");
                                item.Enabled = true;
                                item.Selected = true;
                                list.Items.Add(item);
                                list.Items.Add(new ListItem("CARGO POR SEGURO", "CARGO POR SEGURO"));
                                list.Items.Add(new ListItem("CPAC", "CPAC"));
                                list.Items.Add(new ListItem("FLETE", "FLETE"));
                                list.Items.Add(new ListItem("MANIOBRAS", "MANIOBRAS"));
                                list.Items.Add(new ListItem("OTROS", "OTROS"));
                                list.Items.Add(new ListItem("RENTA EQUIPO", "RENTA EQUIPO"));
                            }
                            else if (registro.ItemArray[i].ToString().Equals("FLETE"))
                            {
                                item = new ListItem("FLETE", "FLETE");
                                item.Enabled = true;
                                item.Selected = true;
                                list.Items.Add(item);
                                list.Items.Add(new ListItem("CARGO POR SEGURO", "CARGO POR SEGURO"));
                                list.Items.Add(new ListItem("CPAC", "CPAC"));
                                list.Items.Add(new ListItem("FLETE", "FLETE"));
                                list.Items.Add(new ListItem("MANIOBRAS", "MANIOBRAS"));
                                list.Items.Add(new ListItem("OTROS", "OTROS"));
                                list.Items.Add(new ListItem("RENTA EQUIPO", "RENTA EQUIPO"));
                            }
                            else if (registro.ItemArray[i].ToString().Equals("MANIOBRAS"))
                            {
                                item = new ListItem("MANIOBRAS", "MANIOBRAS");
                                item.Enabled = true;
                                item.Selected = true;
                                list.Items.Add(item);
                                list.Items.Add(new ListItem("CARGO POR SEGURO", "CARGO POR SEGURO"));
                                list.Items.Add(new ListItem("CPAC", "CPAC"));
                                list.Items.Add(new ListItem("FLETE", "FLETE"));
                                list.Items.Add(new ListItem("MANIOBRAS", "MANIOBRAS"));
                                list.Items.Add(new ListItem("OTROS", "OTROS"));
                                list.Items.Add(new ListItem("RENTA EQUIPO", "RENTA EQUIPO"));
                            }

                            else if (registro.ItemArray[i].ToString().Equals("OTROS"))
                            {
                                item = new ListItem("OTROS", "OTROS");
                                item.Enabled = true;
                                item.Selected = true;
                                list.Items.Add(item);
                                list.Items.Add(new ListItem("CARGO POR SEGURO", "CARGO POR SEGURO"));
                                list.Items.Add(new ListItem("CPAC", "CPAC"));
                                list.Items.Add(new ListItem("FLETE", "FLETE"));
                                list.Items.Add(new ListItem("MANIOBRAS", "MANIOBRAS"));
                                list.Items.Add(new ListItem("OTROS", "OTROS"));
                                list.Items.Add(new ListItem("RENTA EQUIPO", "RENTA EQUIPO"));
                            }

                            else if (registro.ItemArray[i].ToString().Equals("RENTA EQUIPO"))
                            {
                                item = new ListItem("RENTA EQUIPO", "RENTA EQUIPO");
                                item.Enabled = true;
                                item.Selected = true;
                                list.Items.Add(item);
                                list.Items.Add(new ListItem("CARGO POR SEGURO", "CARGO POR SEGURO"));
                                list.Items.Add(new ListItem("CPAC", "CPAC"));
                                list.Items.Add(new ListItem("FLETE", "FLETE"));
                                list.Items.Add(new ListItem("MANIOBRAS", "MANIOBRAS"));
                                list.Items.Add(new ListItem("OTROS", "OTROS"));
                                list.Items.Add(new ListItem("RENTA EQUIPO", "RENTA EQUIPO"));
                            }

                            cell.Controls.Add(list);
                        }
                        else
                        {
                            TextBox box = new TextBox();
                            box.ID = "" + j + "" + i;
                            box.CssClass = "readOnlyTextBox";
                            box.Text = registro.ItemArray[i].ToString();
                            box.ReadOnly = true;
                            if (i == 0)
                            {
                                box.Width = 50;
                            }
                            else if (i == 1)
                            {
                                box.Width = 60;
                            }
                            else if (i == 5 || i == 4) { box.Width = 60; }
                            else if (i == 2) { box.Width = 200; }

                            cell.Controls.Add(box);
                        }
                        row.Cells.Add(cell);
                    }
                    table.Rows.Add(row);


                    j++;
                }
                PlaceHolder1.Controls.Add(table);

            }
        }

        public Hashtable conceptosFinales()
        {
            table = new HtmlTable();
            Hashtable datos = new Hashtable();
            for (int i = 0; i < table.Rows.Count - 1; i++)
            {
                TextBox cant = (TextBox)table.FindControl("" + i + "1");
                TextBox unidad = (TextBox)table.FindControl("" + i + "1");
                TextBox concepto = (TextBox)table.FindControl("" + i + "2");
                DropDownList tmp = (DropDownList)table.FindControl("" + i + "3");
                TextBox valor = (TextBox)table.FindControl("" + i + "4");
                TextBox importe = (TextBox)table.FindControl("" + i + "5");

                double cantidad = Math.Abs(double.Parse(cant.Text));

                //double cantidad = Double.Parse(cant.Text);





                ArrayList list = new ArrayList();
                list.Add(cantidad.ToString());
                list.Add(unidad.Text);
                list.Add(concepto.Text);
                list.Add(tmp.SelectedValue);
                list.Add(valor.Text);
                list.Add(importe.Text);



                if (datos.ContainsKey(tmp.Text))
                {
                    datos[i] = list;
                }
                else
                {
                    datos.Add(i, list);
                }
            }
            return datos;
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            bool readOnly = false;
            string stilo = "editTextBox";
            string textoBoton = "Editar";
            if (btnEdit.Text.Equals("Editar"))
            {
                readOnly = false;
                stilo = "editTextBox";
                textoBoton = "Guardar";
            }
            else
            {
                readOnly = true;
                stilo = "readOnlyTextBox";
                textoBoton = "Editar";
            }
            //Se habilita el campo
            txtCuotaConvenida.ReadOnly = readOnly;
            txtValorComercial.ReadOnly = readOnly;
            txtRemisión.ReadOnly = readOnly;
            txtCondicionesPago.ReadOnly = readOnly;
            txtContine.ReadOnly = readOnly;
            txtComentarios.ReadOnly = readOnly;
            txtAmparaRemisiones.ReadOnly = readOnly;
            txtOperador.ReadOnly = readOnly;
            txtLicencia.ReadOnly = readOnly;
            txtTractorEco.ReadOnly = readOnly;
            txtTractorPlacas.ReadOnly = readOnly;
            txtRemol1Eco.ReadOnly = readOnly;
            txtRemolque1Placas.ReadOnly = readOnly;
            txtRemolque2Eco.ReadOnly = readOnly;
            txtRemolque2Placas.ReadOnly = readOnly;
            txtPeso.ReadOnly = readOnly;
            txtUUIDREL.ReadOnly = readOnly;
            txtRelacion.ReadOnly = readOnly;

            //Se elimina el estilo
            txtCuotaConvenida.CssClass = stilo;
            txtValorComercial.CssClass = stilo;
            txtRemisión.CssClass = stilo;
            txtCondicionesPago.CssClass = stilo;
            txtContine.CssClass = stilo;
            txtComentarios.CssClass = stilo;
            txtAmparaRemisiones.CssClass = stilo;
            txtOperador.CssClass = stilo;
            txtLicencia.CssClass = stilo;
            txtTractorEco.CssClass = stilo;
            txtTractorPlacas.CssClass = stilo;
            txtRemol1Eco.CssClass = stilo;
            txtRemolque1Placas.CssClass = stilo;
            txtRemolque2Eco.CssClass = stilo;
            txtRemolque2Placas.CssClass = stilo;
            txtPeso.CssClass = stilo;
            txtUUIDREL.CssClass = stilo;
            txtRelacion.CssClass = stilo;

            btnEdit.Text = textoBoton;


        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            bool campoIncorrecto = false;

            if (txtNombre.Text.Equals(""))
            {
                imgCliente.Visible = true;
                imgCliente.ToolTip = "El cliente no está capturado";
                campoIncorrecto = true;
            }

            if (calle.Equals(""))
            {
                imgDir.Visible = true;
                imgDir.ToolTip = "La calle no está capturada";
                campoIncorrecto = true;
            }

            if (noExt.Equals(""))
            {
                imgDir.Visible = true;
                imgDir.ToolTip = "El No. Ext. no está capturado";
                campoIncorrecto = true;
            }

            // if (noInt.Equals (""))
            //{
            //    imgDir.Visible = true;
            //    imgDir.ToolTip = "El No. Int. no esta capturado";
            //    campoIncorrecto = true;
            //}


            if (colonia.Equals(""))
            {
                imgDir.Visible = true;
                imgDir.ToolTip = "La colonia no está capturada";
                campoIncorrecto = true;
            }

            if (municipio.Equals(""))
            {
                imgDir.Visible = true;
                imgDir.ToolTip = "El municipio no está capturado";
                campoIncorrecto = true;
            }

            if (estado.Equals(""))
            {
                imgDir.Visible = true;
                imgDir.ToolTip = "El Estado. no está capturado";
                campoIncorrecto = true;
            }

            if (pais.Equals(""))
            {
                imgDir.Visible = true;
                imgDir.ToolTip = "El País no está capturado";
                campoIncorrecto = true;
            }

            if (cp.Equals(""))
            {
                imgDir.Visible = true;
                imgDir.ToolTip = "El CP. no está capturado";
                campoIncorrecto = true;
            }

            if (txtSubtotal.Text.Equals(""))
            {
                imgSubtotal.Visible = true;
                imgSubtotal.ToolTip = "El subtotal no está capturado";
                campoIncorrecto = true;
            }
            else if (double.Parse(txtSubtotal.Text) == 0)
            {
                imgSubtotal.Visible = true;
                imgSubtotal.ToolTip = "El subtotal no puede ser 0";
                campoIncorrecto = true;
            }

            if (txtIVA.Text.Equals("") && moneda.Equals("MXN"))
            {
                imgIva.Visible = true;
                imgIva.ToolTip = "El IVA no está capturado";
                campoIncorrecto = true;
            }
            else if (double.Parse(txtIVA.Text) == 0 && moneda.Equals("MXN"))
            {
                imgIva.Visible = true;
                imgIva.ToolTip = "El IVA no puede ser 0";
                campoIncorrecto = true;
            }


            if (txtTotal.Text.Equals(""))
            {
                imgTotal.Visible = true;
                imgTotal.ToolTip = "El total no está capturado";
                campoIncorrecto = true;
            }
            else if (double.Parse(txtTotal.Text) == 0)
            {
                imgTotal.Visible = true;
                imgTotal.ToolTip = "El total no puede ser 0";
                campoIncorrecto = true;
            }

            if (!campoIncorrecto)
            {
                //Se elimina el estilo
                txtCuotaConvenida.CssClass = "readOnlyTextBox";
                txtValorComercial.CssClass = "readOnlyTextBox";
                txtRemisión.CssClass = "readOnlyTextBox";
                txtCondicionesPago.CssClass = "readOnlyTextBox";
                txtContine.CssClass = "readOnlyTextBox";
                txtComentarios.CssClass = "readOnlyTextBox";
                txtAmparaRemisiones.CssClass = "readOnlyTextBox";
                txtOperador.CssClass = "readOnlyTextBox";
                txtLicencia.CssClass = "readOnlyTextBox";
                txtTractorEco.CssClass = "readOnlyTextBox";
                txtTractorPlacas.CssClass = "readOnlyTextBox";
                txtRemol1Eco.CssClass = "readOnlyTextBox";
                txtRemolque1Placas.CssClass = "readOnlyTextBox";
                txtRemolque2Eco.CssClass = "readOnlyTextBox";
                txtRemolque2Placas.CssClass = "readOnlyTextBox";
                txtUUIDREL.CssClass = "readOnlyTextBox";
                txtRelacion.CssClass = "readOnlyTextBox";

                generaTXT();


                jsonFactura = "{\r\n\r\n  \"idTipoCfd\":" + "\"" + idTipoFactura + "\"";
                jsonFactura += ",\r\n\r\n  \"nombre\":" + "\"" + lblFact.Text + ".txt" + "\"";
                jsonFactura += ",\r\n\r\n  \"idSucursal\":" + "\"" + idSucursal + "\"";
                jsonFactura += ", \r\n\r\n  \"archivoFuente\":" + "\"" + escrituraFactura + "\"" + "\r\n\r\n}";

                string folioFactura = "", serieFactura = "", uuidFactura = "", pdf_xml_descargaFactura = "", pdf_descargaFactura = "", xlm_descargaFactura = "", cancelFactura = "", error = "";
                string salida = "";
                try
                {
                    var client = new RestClient("https://canal1.xsa.com.mx:9050/" + IdApiEmpresa + "/cfdis");
                    var request = new RestRequest(Method.PUT);

                    request.AddHeader("cache-control", "no-cache");

                    request.AddHeader("content-length", "834");
                    request.AddHeader("accept-encoding", "gzip, deflate");
                    request.AddHeader("Host", "canal1.xsa.com.mx:9050");
                    request.AddHeader("Postman-Token", "b6b7d8eb-29f2-420f-8d70-7775701ec765,a4b60b83-429b-4188-98d4-7983acc6742e");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Accept", "*/*");
                    request.AddHeader("User-Agent", "PostmanRuntime/7.13.0");

                    request.AddParameter("application/json", jsonFactura, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);


                    string[] separadaFactura = response.Content.ToString().Split(',');
                    try
                    { //try TLS 1.3
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)12288
                                                             | (SecurityProtocolType)3072
                                                             | (SecurityProtocolType)768
                                                             | SecurityProtocolType.Tls;
                    }
                    catch (NotSupportedException)
                    {
                        try
                        { //try TLS 1.2
                            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072
                                                                 | (SecurityProtocolType)768
                                                                 | SecurityProtocolType.Tls;
                        }
                        catch (NotSupportedException)
                        {
                            try
                            { //try TLS 1.1
                                ServicePointManager.SecurityProtocol = (SecurityProtocolType)768
                                                                     | SecurityProtocolType.Tls;
                            }
                            catch (NotSupportedException)
                            { //TLS 1.0
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                            }
                        }
                    }
                    foreach (string factura in separadaFactura)
                    {
                        if (factura.Contains("errors"))
                        {
                            error += factura.Replace(factura.Substring(0, 6), "").Replace("\"", "").Split('[')[1] + "\n";
                            error = error.Replace("\\n", "").Replace("]}", "");
                            salida = "FALLA AL SUBIR";
                        }
                        else
                        {
                            if (factura.Contains("folio"))
                            {
                                folioFactura = factura.Replace(factura.Substring(0, 5), "").Replace("\"", "").Split(':')[1];
                            }

                            if (factura.Contains("serie"))
                            {
                                serieFactura = factura.Replace(factura.Substring(0, 5), "").Replace("\"", "").Split(':')[1];
                            }

                            if (factura.Contains("uuid"))
                            {
                                uuidFactura = factura.Replace(factura.Substring(0, 4), "").Replace("\"", "").Split(':')[1];
                            }

                            if (factura.Contains("pdfAndXmlDownload"))
                            {
                                pdf_xml_descargaFactura = factura.Replace(factura.Substring(0, 17), "").Replace("\"", "").Split(':')[1];
                            }

                            if (factura.Contains("pdfDownload"))
                            {
                                pdf_descargaFactura = "https://canal1.xsa.com.mx:9050" + factura.Replace(factura.Substring(0, 11), "").Replace("\"", "").Split(':')[1];
                            }

                            if (factura.Contains("xmlDownload") && !factura.Contains("pdfAndXmlDownload"))
                            {
                                xlm_descargaFactura = "https://canal1.xsa.com.mx:9050" + factura.Replace(factura.Substring(0, 11), "").Replace("\"", "").Split(':')[1];
                            }

                            if (factura.Contains("cancellCfdi"))
                            {
                                cancelFactura = factura.Replace(factura.Substring(0, 11), "").Replace("\"", "").Split(':')[1];
                            }
                        }
                    }
                }

                catch (Exception ex)
                {
                    string error1 = ex.Message;
                }



                string ftp = System.Web.Configuration.WebConfigurationManager.AppSettings["ftp"];
                if (ftp.Equals("Si"))
                {
                    string path = System.Web.Configuration.WebConfigurationManager.AppSettings["dir"] + lblFact.Text + ".txt";
                    UploadFiles file = new UploadFiles();
                    //file.Upload(@"C:\Users\fidele gutember\Prueba.txt", serie);
                    //salida = file.prubeftp(lblFact.Text + ".txt", path, serie);
                    //subir factura directamente

                }
                if (salida != "FALLA AL SUBIR")
                {
                    if (System.Web.Configuration.WebConfigurationManager.AppSettings["activa"].Equals("Si"))
                    {
                        //Modifica referencia
                        //facLabControler.actualizaFactura(lblFact.Text, txtFactura.Text, mbnumber);

                        //Inserta en la tabla de generadas
                        //string rutaPDF = "https://" + System.Web.Configuration.WebConfigurationManager.AppSettings["servidord"] + "/xsamanager/downloadCfdWebView?serie=" + serie + "&folio=" + fact + "&tipo=PDF&rfc=TTR931201KJ6&key=" + System.Web.Configuration.WebConfigurationManager.AppSettings["llave"];
                        //string rutaXML = "https://" + System.Web.Configuration.WebConfigurationManager.AppSettings["servidord"] + "/xsamanager/downloadCfdWebView?serie=" + serie + "&folio=" + fact + "&tipo=XML&rfc=TTR931201KJ6&key=" + System.Web.Configuration.WebConfigurationManager.AppSettings["llave"];
                        string imaging = "http://172.16.136.34/cgi-bin/img-docfind.pl?reftype=ORD&refnum=" + txtOrden.Text.ToString().Trim();

                        DateTime fecha1 = Convert.ToDateTime(txtFecha.Text.ToString());
                        string fechaFinal = fecha1.Year + "-" + fecha1.Month + "-" + fecha1.Day + " " + fecha1.Hour + ":" + fecha1.Minute + ":" + fecha1.Second + "." + fecha1.Millisecond;

                        facLabControler.generadas(mbnumber.ToString(), fact, serie, idreceptor, fechaFinal, txtTotal.Text.ToString().Trim(),
                           moneda, pdf_descargaFactura, xlm_descargaFactura, imaging, bandera, "Tralix", "VAL", ultinvoice, hecha, txtOrden.Text.Trim(), txtRFCDestino.Text.Trim());
                    }

                    string msg = "Se ha generado correctamente el CFDi, será enviado a tu correo electrónico o podrás encontrarlo en el portal de búsqueda.";
                    ScriptManager.RegisterStartupScript(this, GetType(), "swal", "swal('" + msg + "', 'Correcto ', 'success');setTimeout(function(){window.location.href ='Main.aspx'}, 10000)", true);
                    //PopupMsg.Message1 = "Se ha generado correctamente el CFDi, será enviado a tu correo electrónico o podrás encontrarlo en el portal de búsqueda.";
                    //PopupMsg.ShowPopUp(0);
                }
                else
                {
                    string msg = "Error al conectar al servicio XSA";
                    ScriptManager.RegisterStartupScript(this, GetType(), "swal", "swal('" + msg + "', 'Error ', 'error');setTimeout(function(){window.location.href ='Main.aspx'}, 10000)", true);
                    //PopupMsg.Message1 = "Error al conectar al servicio XSA";
                    //PopupMsg.ShowPopUp(0);
                }
            }

            else
            {

                ClientScript.RegisterStartupScript(this.GetType(), "myScriptExiste", "<script>javascript:setExiste(true);</script>");
            }

        }
        protected void btnEditaDetalle_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < table.Rows.Count - 1; i++)
            {
                Control ctrl = table.FindControl("" + i + "3");
                DropDownList tmp = (DropDownList)table.FindControl("" + i + "3");
                tmp.Enabled = true;
                tmp.CssClass = "";
            }
        }
        protected void btnGuardaDetalle_Click(object sender, EventArgs e)
        {
            Hashtable conceptos = new Hashtable();
            for (int i = 0; i < table.Rows.Count - 1; i++)
            {
                TextBox cant = (TextBox)table.FindControl("" + i + "0");
                TextBox concepto = (TextBox)table.FindControl("" + i + "1");

                DropDownList tmp = (DropDownList)table.FindControl("" + i + "3");
                tmp.Enabled = false;
                tmp.CssClass = "readOnlyTextBox";

                TextBox valor = (TextBox)table.FindControl("" + i + "4");

                TextBox importe = (TextBox)table.FindControl("" + i + "5");
                if (conceptos.ContainsKey(tmp.Text))
                {
                    double value = (double)conceptos[tmp.SelectedValue];
                    conceptos[tmp.SelectedValue] = value + double.Parse(importe.Text);
                }
                else
                {
                    conceptos.Add(tmp.SelectedValue, double.Parse(importe.Text));
                }
            }

            if (conceptos.ContainsKey("FLETE")) { txtFlete.Text = conceptos["FLETE"].ToString(); }
            else { txtFlete.Text = ""; }

            if (conceptos.ContainsKey("MANIOBRAS")) { txtManiobras.Text = conceptos["MANIOBRAS"].ToString(); }
            else { txtManiobras.Text = ""; }

            if (conceptos.ContainsKey("OTROS")) { txtOtros.Text = conceptos["OTROS"].ToString(); }
            else { txtOtros.Text = ""; }

            if (conceptos.ContainsKey("RENTA EQUIPO")) { txtEquipo.Text = conceptos["RENTA EQUIPO"].ToString(); }
            else { txtEquipo.Text = ""; }

            if (conceptos.ContainsKey("CPAC")) { txtCPAC.Text = conceptos["CPAC"].ToString(); }
            else { txtCPAC.Text = ""; }

            if (conceptos.ContainsKey("CARGO POR SEGURO")) { txtCargaXSeguro.Text = conceptos["CARGO POR SEGURO"].ToString(); }
            else { txtCargaXSeguro.Text = ""; }

            if (conceptos.ContainsKey("AUTOPISTAS")) { txtAutopistas.Text = conceptos["AUTOPISTAS"].ToString(); }
            else { txtAutopistas.Text = ""; }

            conceptos.Clear();
        }

        public Hashtable generaActualizacion()
        {
            Hashtable datosTabla = this.conceptosFinales();
            Hashtable actualiza = new Hashtable();

            foreach (int item in datosTabla.Keys)
            {
                ArrayList list = (ArrayList)datosTabla[item];
                string tipoConcepto = list[3].ToString();
                double total = double.Parse(list[5].ToString());
                if (actualiza.ContainsKey(tipoConcepto))
                {
                    double val = double.Parse(actualiza[tipoConcepto].ToString());
                    actualiza[tipoConcepto] = val + total;
                }
                else
                {
                    actualiza.Add(tipoConcepto, total);
                }
            }
            return actualiza;
        }

        public bool validaCampos()
        {
            bool campoIncorrecto = false;
            if (txtNombre.Text.Equals(""))
            {
                imgCliente.Visible = true;
                imgCliente.ToolTip = "El cliente no está capturado";
                campoIncorrecto = true;
            }

            if (calle.Equals(""))
            {
                imgDir.Visible = true;
                imgDir.ToolTip = "La calle no está capturada";
                campoIncorrecto = true;
            }

            if (noExt.Equals(""))
            {
                imgDir.Visible = true;
                imgDir.ToolTip = "El No. Ext. no está capturado";
                campoIncorrecto = true;
            }

            // if (noInt.Equals (""))
            //{
            //    imgDir.Visible = true;
            //    imgDir.ToolTip = "El No. Int. no esta capturado";
            //    campoIncorrecto = true;
            //}


            if (colonia.Equals(""))
            {
                imgDir.Visible = true;
                imgDir.ToolTip = "La colonia no está capturada";
                campoIncorrecto = true;
            }

            if (municipio.Equals(""))
            {
                imgDir.Visible = true;
                imgDir.ToolTip = "El municipio no está capturado";
                campoIncorrecto = true;
            }

            if (estado.Equals(""))
            {
                imgDir.Visible = true;
                imgDir.ToolTip = "El Estado. no está capturado";
                campoIncorrecto = true;
            }

            if (pais.Equals(""))
            {
                imgDir.Visible = true;
                imgDir.ToolTip = "El País no está capturado";
                campoIncorrecto = true;
            }

            if (cp.Equals(""))
            {
                imgDir.Visible = true;
                imgDir.ToolTip = "El CP. no está capturado";
                campoIncorrecto = true;
            }

            if (txtSubtotal.Text.Equals(""))
            {
                imgSubtotal.Visible = true;
                imgSubtotal.ToolTip = "El subtotal no está capturado";
                campoIncorrecto = true;
            }
            else if (double.Parse(txtSubtotal.Text) == 0)
            {
                imgSubtotal.Visible = true;
                imgSubtotal.ToolTip = "El subtotal no puede ser 0";
                campoIncorrecto = true;
            }

            if (txtIVA.Text.Equals("") && moneda.Equals("MXN"))
            {
                imgIva.Visible = true;
                imgIva.ToolTip = "El IVA no está capturado";
                campoIncorrecto = true;
            }
            else if (double.Parse(txtIVA.Text) == 0 && moneda.Equals("MXN"))
            {
                imgIva.Visible = true;
                imgIva.ToolTip = "El IVA no puede ser 0";
                campoIncorrecto = true;
            }



            if (txtTotal.Text.Equals(""))
            {
                imgTotal.Visible = true;
                imgTotal.ToolTip = "El total no está capturado";
                campoIncorrecto = true;
            }
            else if (double.Parse(txtTotal.Text) == 0)
            {
                imgTotal.Visible = true;
                imgTotal.ToolTip = "El total no puede ser 0";
                campoIncorrecto = true;
            }

            return campoIncorrecto;
        }
        protected void btnBloquear_Click(object sender, EventArgs e)
        {

        }
        protected void txtMetodoPago_TextChanged(object sender, EventArgs e)
        {

        }

        protected void GenerarCFDI()
        {
            bool campoIncorrecto = false;

            if (txtNombre.Text.Equals(""))
            {
                imgCliente.Visible = true;
                imgCliente.ToolTip = "El cliente no está capturado";
                campoIncorrecto = true;
            }

            if (calle.Equals(""))
            {
                imgDir.Visible = true;
                imgDir.ToolTip = "La calle no está capturada";
                campoIncorrecto = true;
            }

            if (noExt.Equals(""))
            {
                imgDir.Visible = true;
                imgDir.ToolTip = "El No. Ext. no está capturado";
                campoIncorrecto = true;
            }

            // if (noInt.Equals (""))
            //{
            //    imgDir.Visible = true;
            //    imgDir.ToolTip = "El No. Int. no esta capturado";
            //    campoIncorrecto = true;
            //}


            if (colonia.Equals(""))
            {
                imgDir.Visible = true;
                imgDir.ToolTip = "La colonia no está capturada";
                campoIncorrecto = true;
            }

            if (municipio.Equals(""))
            {
                imgDir.Visible = true;
                imgDir.ToolTip = "El municipio no está capturado";
                campoIncorrecto = true;
            }

            if (estado.Equals(""))
            {
                imgDir.Visible = true;
                imgDir.ToolTip = "El Estado. no está capturado";
                campoIncorrecto = true;
            }

            if (pais.Equals(""))
            {
                imgDir.Visible = true;
                imgDir.ToolTip = "El País no está capturado";
                campoIncorrecto = true;
            }

            if (cp.Equals(""))
            {
                imgDir.Visible = true;
                imgDir.ToolTip = "El CP. no está capturado";
                campoIncorrecto = true;
            }

            if (txtSubtotal.Text.Equals(""))
            {
                imgSubtotal.Visible = true;
                imgSubtotal.ToolTip = "El subtotal no está capturado";
                campoIncorrecto = true;
            }
            else if (double.Parse(txtSubtotal.Text) == 0)
            {
                imgSubtotal.Visible = true;
                imgSubtotal.ToolTip = "El subtotal no puede ser 0";
                campoIncorrecto = true;
            }

            if (txtIVA.Text.Equals("") && moneda.Equals("MXN"))
            {
                imgIva.Visible = true;
                imgIva.ToolTip = "El IVA no está capturado";
                campoIncorrecto = true;
            }
            else if (double.Parse(txtIVA.Text) == 0 && moneda.Equals("MXN"))
            {
                imgIva.Visible = true;
                imgIva.ToolTip = "El IVA no puede ser 0";
                campoIncorrecto = true;
            }


            if (txtTotal.Text.Equals(""))
            {
                imgTotal.Visible = true;
                imgTotal.ToolTip = "El total no está capturado";
                campoIncorrecto = true;
            }
            else if (double.Parse(txtTotal.Text) == 0)
            {
                imgTotal.Visible = true;
                imgTotal.ToolTip = "El total no puede ser 0";
                campoIncorrecto = true;
            }

            if (!campoIncorrecto)
            {
                //Se elimina el estilo
                txtCuotaConvenida.CssClass = "readOnlyTextBox";
                txtValorComercial.CssClass = "readOnlyTextBox";
                txtRemisión.CssClass = "readOnlyTextBox";
                txtCondicionesPago.CssClass = "readOnlyTextBox";
                txtContine.CssClass = "readOnlyTextBox";
                txtComentarios.CssClass = "readOnlyTextBox";
                txtAmparaRemisiones.CssClass = "readOnlyTextBox";
                txtOperador.CssClass = "readOnlyTextBox";
                txtLicencia.CssClass = "readOnlyTextBox";
                txtTractorEco.CssClass = "readOnlyTextBox";
                txtTractorPlacas.CssClass = "readOnlyTextBox";
                txtRemol1Eco.CssClass = "readOnlyTextBox";
                txtRemolque1Placas.CssClass = "readOnlyTextBox";
                txtRemolque2Eco.CssClass = "readOnlyTextBox";
                txtRemolque2Placas.CssClass = "readOnlyTextBox";
                txtUUIDREL.CssClass = "readOnlyTextBox";
                txtRelacion.CssClass = "readOnlyTextBox";

                generaTXT();

                jsonFactura = "{\r\n\r\n  \"idTipoCfd\":" + "\"" + idTipoFactura + "\"";
                jsonFactura += ",\r\n\r\n  \"nombre\":" + "\"" + lblFact.Text + ".txt" + "\"";
                jsonFactura += ",\r\n\r\n  \"idSucursal\":" + "\"" + idSucursal + "\"";
                jsonFactura += ", \r\n\r\n  \"archivoFuente\":" + "\"" + escrituraFactura + "\"" + "\r\n\r\n}";

                string folioFactura = "", serieFactura = "", uuidFactura = "", pdf_xml_descargaFactura = "", pdf_descargaFactura = "", xlm_descargaFactura = "", cancelFactura = "", error = "";
                string salida = "";
                try
                {
                    try
                    { //try TLS 1.3
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)12288
                                                             | (SecurityProtocolType)3072
                                                             | (SecurityProtocolType)768
                                                             | SecurityProtocolType.Tls;
                    }
                    catch (NotSupportedException)
                    {
                        try
                        { //try TLS 1.2
                            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072
                                                                 | (SecurityProtocolType)768
                                                                 | SecurityProtocolType.Tls;
                        }
                        catch (NotSupportedException)
                        {
                            try
                            { //try TLS 1.1
                                ServicePointManager.SecurityProtocol = (SecurityProtocolType)768
                                                                     | SecurityProtocolType.Tls;
                            }
                            catch (NotSupportedException)
                            { //TLS 1.0
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                            }
                        }
                    }

                    var client = new RestClient("https://canal1.xsa.com.mx:9050/" + IdApiEmpresa + "/cfdis");
                    var request = new RestRequest(Method.PUT);

                    request.AddHeader("cache-control", "no-cache");

                    request.AddHeader("content-length", "834");
                    request.AddHeader("accept-encoding", "gzip, deflate");
                    request.AddHeader("Host", "canal1.xsa.com.mx:9050");
                    request.AddHeader("Postman-Token", "b6b7d8eb-29f2-420f-8d70-7775701ec765,a4b60b83-429b-4188-98d4-7983acc6742e");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Accept", "*/*");
                    request.AddHeader("User-Agent", "PostmanRuntime/7.13.0");

                    request.AddParameter("application/json", jsonFactura, ParameterType.RequestBody);
                    //AQUI COMENTE 2 LINEAS PARA EJECUTAR SIN CREAR
                    IRestResponse response = client.Execute(request);
                    string[] separadaFactura = response.Content.ToString().Split(',');
                    //String CAD = "";
                    //string[] separadaFactura = CAD.Split(',');
                    foreach (string factura in separadaFactura)
                    {
                        if (factura.Contains("error"))
                        {
                            error += factura.Replace(factura.Substring(0, 6), "").Replace("\"", "").Split('[')[1] + "\n";
                            error = error.Replace("\\n", "").Replace("]}", "");
                            //salida = "FALLA AL SUBIR";
                        }
                        else if (factura.Contains("Bad re"))
                        {
                            salida = "FALLA AL SUBIR";
                        }
                        else if (factura.Contains("Error"))
                        {
                            salida = "FALLA AL SUBIR";
                        }
                        else if (factura == "")
                        {
                            salida = "FALLA AL SUBIR";
                        }
                        else
                        {
                            if (factura.Contains("folio"))
                            {
                                folioFactura = factura.Replace(factura.Substring(0, 5), "").Replace("\"", "").Split(':')[1];
                            }

                            if (factura.Contains("serie"))
                            {
                                serieFactura = factura.Replace(factura.Substring(0, 5), "").Replace("\"", "").Split(':')[1];
                            }

                            if (factura.Contains("uuid"))
                            {
                                uuidFactura = factura.Replace(factura.Substring(0, 4), "").Replace("\"", "").Split(':')[1];
                            }

                            if (factura.Contains("pdfAndXmlDownload"))
                            {
                                pdf_xml_descargaFactura = factura.Replace(factura.Substring(0, 17), "").Replace("\"", "").Split(':')[1];
                            }

                            if (factura.Contains("pdfDownload"))
                            {
                                pdf_descargaFactura = "https://canal1.xsa.com.mx:9050" + factura.Replace(factura.Substring(0, 11), "").Replace("\"", "").Split(':')[1];
                            }

                            if (factura.Contains("xmlDownload") && !factura.Contains("pdfAndXmlDownload"))
                            {
                                xlm_descargaFactura = "https://canal1.xsa.com.mx:9050" + factura.Replace(factura.Substring(0, 11), "").Replace("\"", "").Split(':')[1];
                            }

                            if (factura.Contains("cancellCfdi"))
                            {
                                cancelFactura = factura.Replace(factura.Substring(0, 11), "").Replace("\"", "").Split(':')[1];
                            }
                        }
                    }
                }

                catch (Exception ex)
                {
                    string error1 = ex.Message;
                }



                string ftp = System.Web.Configuration.WebConfigurationManager.AppSettings["ftp"];
                if (ftp.Equals("Si"))
                {
                    string path = System.Web.Configuration.WebConfigurationManager.AppSettings["dir"] + lblFact.Text + ".txt";
                    UploadFiles file = new UploadFiles();
                    //file.Upload(@"C:\Users\fidele gutember\Prueba.txt", serie);
                    //salida = file.prubeftp(lblFact.Text + ".txt", path, serie);
                    //subir factura directamente

                }
                if (salida != "FALLA AL SUBIR")
                {
                    if (System.Web.Configuration.WebConfigurationManager.AppSettings["activa"].Equals("Si"))
                    {
                        //Modifica referencia
                        //facLabControler.actualizaFactura(lblFact.Text, txtFactura.Text, mbnumber);

                        //Inserta en la tabla de generadas
                        //string rutaPDF = "https://" + System.Web.Configuration.WebConfigurationManager.AppSettings["servidord"] + "/xsamanager/downloadCfdWebView?serie=" + serie + "&folio=" + fact + "&tipo=PDF&rfc=TTR931201KJ6&key=" + System.Web.Configuration.WebConfigurationManager.AppSettings["llave"];
                        //string rutaXML = "https://" + System.Web.Configuration.WebConfigurationManager.AppSettings["servidord"] + "/xsamanager/downloadCfdWebView?serie=" + serie + "&folio=" + fact + "&tipo=XML&rfc=TTR931201KJ6&key=" + System.Web.Configuration.WebConfigurationManager.AppSettings["llave"];
                        string imaging = "http://172.16.136.34/cgi-bin/img-docfind.pl?reftype=ORD&refnum=" + txtOrden.Text.ToString().Trim();

                        DateTime fecha1 = Convert.ToDateTime(txtFecha.Text.ToString());
                        string fechaFinal = fecha1.Year + "-" + fecha1.Month + "-" + fecha1.Day + " " + fecha1.Hour + ":" + fecha1.Minute + ":" + fecha1.Second + "." + fecha1.Millisecond;
                        //AQUI COMENTE DOS LINEAS PARA QUE NO SE GUARDE LA FACTURA EN DB
                        facLabControler.generadas(mbnumber.ToString(), fact, serie, idreceptor, fechaFinal, txtTotal.Text.ToString().Trim(),
                           moneda, pdf_descargaFactura, xlm_descargaFactura, imaging, bandera, "Tralix", "VAL", ultinvoice, hecha, txtOrden.Text.Trim(), txtRFCDestino.Text.Trim());
                    }

                    string msg = "Se ha generado correctamente el CFDi, será enviado a tu correo electrónico o podrás encontrarlo en el portal de búsqueda.";
                    ScriptManager.RegisterStartupScript(this, GetType(), "swal", "swal('" + msg + "', 'Correcto ', 'success');setTimeout(function(){window.location.href ='Main.aspx'}, 10000)", true);
                    //PopupMsg.Message1 = "Se ha generado correctamente el CFDi, será enviado a tu correo electrónico o podrás encontrarlo en el portal de búsqueda.";
                    //PopupMsg.ShowPopUp(0);
                }
                else
                {
                    string msg = "Error al conectar al servicio XSA";
                    ScriptManager.RegisterStartupScript(this, GetType(), "swal", "swal('" + msg + "', 'Error ', 'error');setTimeout(function(){window.location.href ='Main.aspx'}, 10000)", true);
                    //PopupMsg.Message1 = "Error al conectar al servicio XSA";
                    //PopupMsg.ShowPopUp(0);
                }
            }



            else
            {

                ClientScript.RegisterStartupScript(this.GetType(), "myScriptExiste", "<script>javascript:setExiste(true);</script>");
            }

        }


    }
}