using FactMasiva.Controllers;
using FactMasiva.Models;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace TIFacturasMasivas
{
    public partial class AdendaMasive : System.Web.UI.Page
    {
        //Properties
        public FactLabControler facLabControler = new FactLabControler();
        public SearchControler busquedaControler = new SearchControler();
        public string fDesde, fHasta, concepto, tipoCobro;
        public Hashtable campos = new Hashtable(); //Almacena el detalle de la factura y lo va sumando dependiendo de su clasificación
        Hashtable detalle33 = new Hashtable();
        public Hashtable camposFinales = new Hashtable();
        HtmlTable table = new HtmlTable();

        //Variables para almacenar el detalle de la factura
        public string moneda, tipoCambio, MetodoPago33, tipocomprobante, usocdfi, confirmacion, relacion, uuidrel, lugarexpedicion, idreceptor, pais, calle, noExt, noInt, colonia, localidad, referencia, municipio,
            estado, cp, estatus, paisresidencia, numtributacion, mailenvio, serie, tipofactor, tasatras, codirete, tasarete, coditrans, tipodetalle, proveedor, prov;

        public int mbnumber;
        public string flete1, maniobras1, seguro1, pista1, cpac1, equipo1, otros1;
        public string fact;
        public string cuota, valorComercial, remision, condicionesPago, contiene, comentarios, amparaRemisiones,
            operador, licencia, tratorEco, tractorPlacas, remol1Eco, remol1Placas, remol2Eco, remol2Placas, bandera, ultinvoice, hecha;
        public string clienteId;
        public bool adenda;
        string fecha, origen, origenRemitente, domicilioOrigen, rfcOrigen, destino, destinatario, domicilioDestino, rfcDestino, bill_to,
        cuotaConvenida, peso, orden, mb, invoice, movimiento, documento, fechaPagare, intereses, clientePagare, descripcion;
        string factTemporal;

        public string escrituraFactura = "", idSucursal = "", idTipoFactura = "", jsonFactura = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            bill_to = Request.QueryString["billto"];
            DataTable td_facturas = facLabControler.FacturasPorProcesarLiverpool();
            foreach (DataRow row in td_facturas.Rows)
            {
                if (bill_to.ToUpper() == row["idreceptor"].ToString().ToUpper())
                {
                    //Obtiene el número de factura
                    fact = row["Folio"].ToString();
                    factTemporal = fact;
                    lblFact.Text = facLabControler.facturaValida(fact);
                    fact = lblFact.Text;

                    DataTable referenciasAdenda = facLabControler.FacturaFacturaAdendaReferencia(row["orden"].ToString());
                    foreach (DataRow rowAdenda in referenciasAdenda.Rows)
                    {
                        if (rowAdenda["ref_type"].ToString() == "ADEPED")
                        {
                            txtPedido.Text = rowAdenda["ref_number"].ToString();
                        }
                        else if (rowAdenda["ref_type"].ToString() == "ADEHOJ")
                        {
                            txtHojaEntrada.Text = rowAdenda["ref_number"].ToString();
                        }
                        else if (rowAdenda["ref_type"].ToString() == "LPROV")
                        {
                            lstProveedor.SelectedValue = rowAdenda["ref_number"].ToString();
                        }
                        checkAdenda.Checked = true;
                    }

                    //Verifica que el estatus sea PRN
                    if (verificaStatus(fact))
                    {
                        if (IsPostBack)
                        {
                            descripcion = txtDescripcion.Text;
                        }

                        iniciaDatos();
                        generaDetalle();



                        if (AdendaPanel.Visible == false)
                        {

                            if (clienteId.Equals("LIVERPOL") || clienteId.Equals("ALMLIVER") || clienteId.Equals("LIVERTIJ") || clienteId.Equals("LIVERSUB") || clienteId.Equals("LIVERSUR") || clienteId.Equals("SFERALIV") || clienteId.Equals("GLOBALIV") || clienteId.Equals("SETRALIV") || clienteId.Equals("FACTUMLV") || clienteId.Equals("MERCANLV") || clienteId.Equals("LIVERDED"))
                            //if (clienteId.Equals("LIVERPOL") || clienteId.Equals("ALMLIVER") || clienteId.Equals("LIVERTIJ"))
                            {
                                adenda = true;
                                AdendaPanel.Visible = true;
                            }

                        }
                        else
                        {
                            if (!txtPedido.Text.Equals("") && !txtHojaEntrada.Text.Equals(""))
                            {
                                AdendaPanel.Enabled = false;
                                btnGuardar.Enabled = true;
                            }
                            else
                            {
                                AdendaPanel.Enabled = true;
                            }
                        }

                        bool valida = validaCampos();

                    }
                    else
                    {
                        //Response.Redirect("Busqueda.aspx?factura=" + lblFact.Text + "&estatus=" + estatus);
                    }
                    generarCFDI();
                }

            }
            Response.Redirect("GeneracionMasiva.aspx");
            //string close = @"<script type='text/javascript'>
            //                    window.open('', '_self', ''); window.close();
            //                    </script>";
            //base.Response.Write(close);

        }

        public bool verificaStatus(string fact)
        {
            string referencia = "";
            DataTable td = facLabControler.estatus(fact);
            if (td.Rows.Count != 0)
            {
                foreach (DataRow row in td.Rows)
                {
                    estatus = row["ivh_invoicestatus"].ToString();
                    referencia = row["ivh_ref_number"].ToString();
                }
                string[] reference = referencia.Split('-');

                if (estatus.Equals("PRN"))
                {
                    return true;
                }

            }

            return false;
        }

        public void generaDetalle()

        {
            detalle33.Clear();
            Hashtable campos33 = new Hashtable();
            DataTable td33 = facLabControler.detalle(factTemporal);
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


            //Solicita al controlador el detalle de la factura
            DataTable td = facLabControler.detalle(factTemporal);

            //Verifica el número de registros en el detalle de la factura
            int noDetalle = td.Rows.Count;


            //Si los registros son igual a 0 muestra la leyenda de que no hay detalles
            if (noDetalle == 0)
            {
                LiteralControl texto = new LiteralControl("<b>No hay detalles.</b>");
                conceptosLiv.Controls.Add(texto);
            }
            else
            {

                table.Border = 1;

                int j = 0;
                int k = 0;
                int sizeRegistro = noDetalle;
                foreach (DataRow registro in td.Rows)
                {
                    //Verifica si alguno de los cargos del detalle tiene un concepto generado por el Dedicated Billing
                    if (registro.ItemArray[2].ToString().Equals("Renta de Equipo Dedicado "))
                    {
                        //Si no contiene el concepto de RENTA EQUIPO lo agrego si no lo busco.
                        if (campos.ContainsKey("RENTA EQUIPO"))
                        {
                            //Obtiene el valor y lo agrega a la hash campos donde se van sumando dependiendo de su clasificación.
                            double val = double.Parse(campos["RENTA EQUIPO"].ToString());
                            campos["RENTA EQUIPO"] = val + double.Parse(registro["importe"].ToString());
                        }
                        else
                        {
                            campos.Add("RENTA EQUIPO", registro["importe"].ToString());
                        }
                    }
                    else
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
                    }
                    if (k == (noDetalle - 1))
                    {
                        HtmlTableRow row = new HtmlTableRow();
                        int size = registro.ItemArray.Length;
                        int i;
                        ArrayList detalleConcepto = new ArrayList();
                        for (i = 0; i < size; i++)
                        {
                            if (i != 2)
                            {
                                HtmlTableCell cell = new HtmlTableCell();
                                cell.BorderColor = "";
                                TextBox box = new TextBox();
                                box.ID = "" + j + "" + i;
                                box.CssClass = "readOnlyTextBox";

                                box.ReadOnly = true;

                                if (i == 0)
                                {
                                    box.Width = 50; box.Text = "1";
                                    detalleConcepto.Add("1");

                                }
                                else if (i == 1)
                                {
                                    box.Width = 85; box.Text = registro.ItemArray[i].ToString();
                                    detalleConcepto.Add(registro.ItemArray[i].ToString());
                                }
                                else if (i == 4)
                                {
                                    box.Width = 90; cell.Width = "111";

                                    box.Text = txtSubtotal.Text;
                                    detalleConcepto.Add(registro.ItemArray[5].ToString());
                                }
                                else if (i == 5)
                                {
                                    box.Width = 90; cell.Width = "111";

                                    box.Text = txtSubtotal.Text;
                                    detalleConcepto.Add(registro.ItemArray[i].ToString());
                                }
                                else if (i == 3)
                                {
                                    cell.Width = "380";
                                    box.Width = 100;
                                    box.Text = registro.ItemArray[i].ToString();
                                    detalleConcepto.Add(registro.ItemArray[i].ToString());
                                }

                                cell.Controls.Add(box);

                                row.Cells.Add(cell);

                            }
                            else
                            {
                                detalleConcepto.Add(registro.ItemArray[i].ToString());
                            }

                        }
                        table.Rows.Add(row);


                        j++;
                        camposFinales = new Hashtable();
                        camposFinales.Add(j, detalleConcepto);
                    }
                    k++;
                }
                conceptosLiv.Controls.Add(table);
            }
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
                fecha = dt.ToString("yyyy'/'MM'/'dd HH:mm:ss");
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
                origen = row["origen"].ToString();
                origenRemitente = row["remitente"].ToString();
                domicilioOrigen = row["domicilioorigen"].ToString();
                rfcOrigen = row["rfcorigen"].ToString();
                destino = row["destino"].ToString();
                destinatario = row["destinatario"].ToString();
                domicilioDestino = row["domiciliodestino"].ToString();
                rfcDestino = row["rfcdestino"].ToString();

                cuotaConvenida = row["cuotaconv"].ToString();
                valorComercial = row["valorcomer"].ToString();
                remision = row["remision"].ToString();


                if (condicionesPago == null || condicionesPago.Equals(row["condpago"].ToString())) { txtCondicionesPago.Text = row["condpago"].ToString(); }
                else { txtCondicionesPago.Text = condicionesPago; }

                contiene = row["contiene"].ToString();
                comentarios = row["comentarios"].ToString();

                //TODO: agregar los campos



                if (checkAdenda.Checked == true)
                {
                    if (descripcion == null || descripcion.Equals(comentarios + " Hoja de Entrada: " + txtHojaEntrada.Text + " Pedido: " + txtPedido.Text) || descripcion.Equals(comentarios)) { txtDescripcion.Text = comentarios + " Hoja de Entrada: " + txtHojaEntrada.Text + " Pedido: " + txtPedido.Text; }
                    else { txtDescripcion.Text = descripcion; }
                }
                else
                {
                    if (descripcion == null || descripcion.Equals(comentarios)) { txtDescripcion.Text = comentarios; }
                    else { txtDescripcion.Text = descripcion; }
                }
                amparaRemisiones = row["ampararemisiones"].ToString();

                peso = row["pesoestimado"].ToString();
                orden = row["orden"].ToString();
                mb = row["nmaster"].ToString();
                invoice = row["invoice"].ToString();
                movimiento = row["movimiento"].ToString();
                operador = row["operador"].ToString();
                licencia = row["operadorlicenicia"].ToString();
                tratorEco = row["tractoeco"].ToString();
                tractorPlacas = row["tractoplaca"].ToString();
                remol1Eco = row["remolque1Eco"].ToString();
                remol1Placas = row["remolque1Placa"].ToString();
                remol2Eco = row["remolque2Eco"].ToString();
                remol2Placas = row["remolque2Placa"].ToString();

                txtSubtotal.Text = row["subtotal"].ToString().Remove(row["subtotal"].ToString().Length - 2);
                txtIVA.Text = row["imptras"].ToString().Remove(row["imptras"].ToString().Length - 2);
                txtRetencion.Text = row["imprete"].ToString().Remove(row["imprete"].ToString().Length - 2);

                if (txtRetencion.Text == "0.00")
                {
                    txtRetencion.Text = "";

                }


                txtTotal.Text = row["total"].ToString().Remove(row["total"].ToString().Length - 2);

                documento = row["documento"].ToString();
                txtCantidadLetra.Text = row["totalletra"].ToString();

                fechaPagare = row["fechapagare"].ToString();

                intereses = row["interesespagare"].ToString();
                clientePagare = row["clientepagare"].ToString();
                txtFormaPago.Text = row["fpago"].ToString();
                txtMetodoPago.Text = row["metpago"].ToString();
                txtCuenta.Text = row["cuentaref"].ToString();


                MetodoPago33 = row["metodopago33"].ToString();
                tipocomprobante = row["tipocomprobante"].ToString();
                lugarexpedicion = row["lugarexpedicion"].ToString();
                usocdfi = row["usocfdi"].ToString();
                confirmacion = row["confirmacion"].ToString();
                relacion = row["relacion"].ToString();
                uuidrel = row["uuidrel"].ToString();
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
                tipodetalle = row["tipodetalle"].ToString();



            }
        }

        //public Hashtable generaActualizacion()
        //{
        //    Hashtable datosTabla = conceptosFinales();
        //    Hashtable actualiza = new Hashtable();

        //    foreach (int item in datosTabla.Keys)
        //    {
        //        ArrayList list = (ArrayList)datosTabla[item];
        //        string tipoConcepto = list[3].ToString();
        //        double total = double.Parse(list[5].ToString());
        //        if (actualiza.ContainsKey(tipoConcepto))
        //        {
        //            double val = double.Parse(actualiza[tipoConcepto].ToString());
        //            actualiza[tipoConcepto] = val + total;
        //        }
        //        else
        //        {
        //            actualiza.Add(tipoConcepto, total);
        //        }
        //    }
        //    return actualiza;
        //}

        public bool validaCampos()
        {
            bool campoIncorrecto = false;
            if (txtNombre.Text.Equals(""))
            {
                imgCliente.Visible = true;
                imgCliente.ToolTip = "El cliente no está capturado";
                campoIncorrecto = true;
            }
            try
            {
                if (calle.Equals(""))
                {
                    imgDir.Visible = true;
                    imgDir.ToolTip = "La calle no está capturada";
                    campoIncorrecto = true;
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            if (noExt.Equals(""))
            {
                imgDir.Visible = true;
                imgDir.ToolTip = "El No. Ext. no está capturado";
                campoIncorrecto = true;
            }

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

            if (txtRetencion.Text.Equals("") && moneda.Equals("MXN"))
            {
                imgRetencion.Visible = true;
                imgRetencion.ToolTip = "La retención no está capturada";
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

        //Review the correct lenght.
        public void idProveedor()
        {
            string prove = lstProveedor.SelectedValue;
            if (prove.Equals("DED"))
            {
                proveedor = "0000000135508";
                prov = "135508";

            }
            else if (prove.Equals("OPO"))
            {
                proveedor = "0000004310107";
                prov = "4310107";
            }

        }


        public void generaTXT()
        {
            //Assignt vendor number
            idProveedor();

            string path = System.Web.Configuration.WebConfigurationManager.AppSettings["dir"] + lblFact.Text + ".txt";
            escrituraFactura = "";
            using (System.IO.StreamWriter escritor = new System.IO.StreamWriter(path))
            {

                //----------------------------------------Seccion De Definición de Serie de Folios CFDI------------------------------------------------------------------------------------

                if (clienteId.Equals("LIVERPOL") || clienteId.Equals("ALMLIVER") || clienteId.Equals("LIVERTIJ") || clienteId.Equals("LIVERSUR") || clienteId.Equals("LIVERSUB") || clienteId.Equals("SFERALIV") || clienteId.Equals("GLOBALIV") || clienteId.Equals("SETRALIV") || clienteId.Equals("FACTUMLV") || clienteId.Equals("MERCANLV") || clienteId.Equals("LIVERDED"))
                //if (clienteId.Equals("LIVERPOL") || clienteId.Equals("ALMLIVER") || clienteId.Equals("LIVERTIJ"))
                {
                    string tipoFactura = "";
                    if (serie.Equals("NCT"))
                    {
                        serie = "NCT";
                        tipoFactura = "NCR";
                    }
                    else if (serie.Equals("TDRT"))
                    {
                        serie = (checkAdenda.Checked == true ? "TDRL" : "TDRL");
                        tipoFactura = "FAC";
                    }

                    string generaIdComprobante = serie + "-" + fact;
                    txtFactura.Text = generaIdComprobante;

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



                    //Open-Agregue esto
                    var request_ = (HttpWebRequest)WebRequest.Create("https://canal1.xsa.com.mx:9050/bf2e1036-ba47-49a0-8cd9-e04b36d5afd4/tiposCfds");
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
                    //Close-Agregue esto

                    //----------------------------------------Seccion De Datos Generales del CFDI-----------------------------------------------------------------------------------------

                    //01 INFORMACION GENERAL DEL CFDI (1:1)

                    escritor.WriteLine(
                    "01"                                                //1.-Tipo De Registro
                    + "|" + generaIdComprobante                         //2-ID Comprobante
                    + "|" + serie                                       //3-Serie
                    + "|" + fact.Substring(1)                           //4-Foliio 
                    + "|" + fecha.Trim()                                //5-Fecha y Hora De Emision
                    + "|" + txtSubtotal.Text.Trim()                     //6-Subtotal
                    + "|" + txtIVA.Text.Trim()                          //7-Total Impuestos Trasladados
                    + "|" + txtRetencion.Text.Trim()                    //8-Total Impuestos Retenidos
                    + "|"                                               //9-Descuentos
                    + "|" + txtTotal.Text.Trim()                        //10-Total
                    + "|" + txtCantidadLetra.Text.Trim()                //11-Total Con Letra
                    + "|" + txtMetodoPago.Text.Trim()                   //12-Forma De Pago
                    + "|" + txtCondicionesPago.Text.Trim()              //13-Condiciones De Pago
                    + "|" + MetodoPago33                                //14-Metodo de Pago
                    + "|" + moneda.Trim()                               //15-Moneda
                    + "|" + tipoCambio.Trim()                           //16-Tipo De Cambio
                    + "|" + tipocomprobante                             //17-Tipo De Comprobante
                    + "|" + lugarexpedicion                             //18-Lugar De Expedicion                                        
                    + "|" + usocdfi                                     //19-Uso CFDI
                    + "|" + confirmacion                                //20-Confirmacion
                    + "|" + tipodetalle                                 //21-Tipo complemento detallista
                    + "|"                                               //Fin del registro
                    );

                    escrituraFactura += "01"                                                //1.-Tipo De Registro
                    + "|" + generaIdComprobante                         //2-ID Comprobante
                    + "|" + serie                                       //3-Serie
                    + "|" + fact.Substring(1)                           //4-Foliio 
                    + "|" + fecha.Trim()                        //5-Fecha y Hora De Emision
                    + "|" + txtSubtotal.Text.Trim()                     //6-Subtotal
                    + "|" + txtIVA.Text.Trim()                          //7-Total Impuestos Trasladados
                    + "|" + txtRetencion.Text.Trim()                    //8-Total Impuestos Retenidos
                    + "|"                                               //9-Descuentos
                    + "|" + txtTotal.Text.Trim()                        //10-Total
                    + "|" + txtCantidadLetra.Text.Trim()                //11-Total Con Letra
                    + "|" + txtMetodoPago.Text.Trim()                   //12-Forma De Pago
                    + "|" + txtCondicionesPago.Text.Trim()             //13-Condiciones De Pago
                    + "|" + MetodoPago33                                //14-Metodo de Pago
                    + "|" + moneda.Trim()                               //15-Moneda
                    + "|" + tipoCambio.Trim()                           //16-Tipo De Cambio
                    + "|" + tipocomprobante                             //17-Tipo De Comprobante
                    + "|" + lugarexpedicion                             //18-Lugar De Expedicion                                        
                    + "|" + usocdfi                                     //19-Uso CFDI
                    + "|" + confirmacion                                //20-Confirmacion
                    + "|" + tipodetalle                                 //21-Tipo complemento detallista
                    + "|";
                }
                else
                {
                    //01 INFORMACION GENERAL DEL CFDI (1:1)

                    escritor.WriteLine(
                    "01"                                                 //1-Tipo De Registro
                    + "|" + txtFactura.Text                              //2-ID Comprobante
                    + "|" + serie                                        //3-Serie
                    + "|" + fact.Substring(1)                           //4-Foliio 
                    + "|" + fecha.Trim()                                 //5-Fecha y Hora De Emision
                    + "|" + txtSubtotal.Text.Trim()                      //6-Subtotal
                    + "|" + txtIVA.Text.Trim()                           //7-Total Impuestos Trasladados
                    + "|" + txtRetencion.Text.Trim()                     //8-Total Impuestos Retenidos
                    + "|"                                                //9-Descuentos
                    + "|" + txtTotal.Text.Trim()                         //10-Total
                    + "|" + txtCantidadLetra.Text.Trim()                 //11-Total Con Letra
                    + "|" + txtMetodoPago.Text.Trim()                    //12-Forma de Pago
                    + "|" + txtCondicionesPago.Text.Trim()               //13-Condiciones de Pago
                    + "|" + MetodoPago33                                 //14-Metodo de Pago
                    + "|" + moneda.Trim()                                //15-Moneda
                    + "|" + tipoCambio.Trim()                            //16-Tipo De Cambio
                    + "|" + tipocomprobante                              //17-Tipo de Comprobante
                    + "|" + lugarexpedicion                              //18-Lugar de Expedicion
                    + "|" + usocdfi                                      //19-Uso CFDI
                    + "|" + confirmacion                                 //20-Confirmacion
                    + "|" + tipodetalle                                  //21-Tipo complemento detallista
                    + "|"                                                //Fin Del Registro
                    );

                    escrituraFactura += "01"                                                 //1-Tipo De Registro
                    + "|" + txtFactura.Text                              //2-ID Comprobante
                    + "|" + serie                                        //3-Serie
                    + "|" + fact.Substring(1)                            //4-Foliio 
                    + "|" + fecha.Trim()                         //5-Fecha y Hora De Emision
                    + "|" + txtSubtotal.Text.Trim()                      //6-Subtotal
                    + "|" + txtIVA.Text.Trim()                           //7-Total Impuestos Trasladados
                    + "|" + txtRetencion.Text.Trim()                     //8-Total Impuestos Retenidos
                    + "|"                                                //9-Descuentos
                    + "|" + txtTotal.Text.Trim()                         //10-Total
                    + "|" + txtCantidadLetra.Text.Trim()                 //11-Total Con Letra
                    + "|" + txtMetodoPago.Text.Trim()                    //12-Forma de Pago
                    + "|" + txtCondicionesPago.Text.Trim()              //13-Condiciones de Pago
                    + "|" + MetodoPago33                                 //14-Metodo de Pago
                    + "|" + moneda.Trim()                                //15-Moneda
                    + "|" + tipoCambio.Trim()                            //16-Tipo De Cambio
                    + "|" + tipocomprobante                              //17-Tipo de Comprobante
                    + "|" + lugarexpedicion                              //18-Lugar de Expedicion
                    + "|" + usocdfi                                      //19-Uso CFDI
                    + "|" + confirmacion                                 //20-Confirmacion
                    + "|" + tipodetalle                                  //21-Tipo complemento detallista
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

                //----------------------------------------Seccion de detalles de la factura leidos desde vista_fe_detail-------------------------------------------------------------------

                Hashtable concept = camposFinales;

                int j = 1;
                ArrayList lista = new ArrayList();
                foreach (int item in detalle33.Keys)
                {
                    if (concept.Keys.Count == j)
                    {
                        lista = (ArrayList)detalle33[item];

                        //04 INFORMACION DE LOS CONCEPTOS (1:N)
                        escritor.WriteLine(
                        "04"                                                   //1-Tipo De Registro
                        + "|" + j.ToString()                                   //2-Consecutivo Concepto
                        + "|" + lista[7].ToString().Trim()                     //3-Clave Producto o Servicio SAT
                        + "|" + lista[2].ToString().Trim()                     //4-Numero Identificacion TDR
                        + "|" + double.Parse(lista[0].ToString()).ToString()   //5-Cantidad
                        + "|" + lista[6].ToString().Trim()                     //6-Clave Unidad SAT
                        + "|" + lista[1].ToString().Trim()                     //7-Unidad de Medida
                        + "|" + lista[2].ToString().Trim()                     //8-Descripcion
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
                    + "|" + lista[2].ToString().Trim()                     //8-Descripcion
                    + "|" + lista[4].ToString().Trim()                     //9-Valor Unitario
                    + "|" + lista[5].ToString().Trim()                     //10-Importe
                    + "|"                                                  //11-Descuento                                                  
                                                                           //12 Importe con iva si el rfc es XAXX010101000 y XEXX010101000 OPCIONAL
                    + "|"                                                  //Fin Del Registro
                     ;

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
                        + "|"                                                  //Fin del Registro
                        ;
                        //Si el valor del monto de la retencion es diferente de 0.00 se escribe el registro de datos de los impuestos retenidos
                        if (lista[15].ToString().Trim() != "0.00")
                        {


                            //042 Datos de los impuestos retinidos (RET) en conceptos(0:N) crear por concepto (retencion)

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
                        + "|";                                             //Fin del Registro

                        }

                        //044 INFORMACION ADICIONAL DE LOS CONCEPTOS (0:N) PARA ADENDA LIVERPOOL
                        escritor.WriteLine(
                        "044"                                                   //1-Tipo de Registro
                        + "|" + j.ToString()                                              //2-Consecutivo Concepto
                        + "|" + "0"                                              //3-Variable
                        + "|" + lista[2].ToString().Trim()                      //4-Valor
                        + "|"                                                   //Fin Del Registro
                        );
                        escrituraFactura += "\\n044"                                                   //1-Tipo de Registro
                        + "|" + j.ToString()                                              //2-Consecutivo Concepto
                        + "|" + "0"                                              //3-Variable
                        + "|" + lista[2].ToString().Trim()                      //4-Valor
                        + "|";                                                  //Fin Del Registro

                    }
                    else
                    {
                        lista = (ArrayList)detalle33[item];

                        //04 INFORMACION DE LOS CONCEPTOS (1:N)
                        escritor.WriteLine(
                        "04"                                                   //1-Tipo De Registro
                        + "|" + j.ToString()                                   //2-Consecutivo Concepto
                        + "|" + lista[7].ToString().Trim()                     //3-Clave Producto o Servicio SAT
                        + "|" + lista[2].ToString().Trim()                     //4-Numero Identificacion TDR
                        + "|" + double.Parse(lista[0].ToString()).ToString()   //5-Cantidad
                        + "|" + lista[6].ToString().Trim()                     //6-Clave Unidad SAT
                        + "|" + lista[1].ToString().Trim()                     //7-Unidad de Medida
                        + "|" + lista[2].ToString().Trim()                     //8-Descripcion
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
                    + "|" + lista[2].ToString().Trim()                     //8-Descripcion
                    + "|" + lista[4].ToString().Trim()                     //9-Valor Unitario
                    + "|" + lista[5].ToString().Trim()                     //10-Importe
                    + "|"                                                  //11-Descuento                                                  
                                                                           //12 Importe con iva si el rfc es XAXX010101000 y XEXX010101000 OPCIONAL
                    + "|"
                    ;
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

                        //Si el valor del monto de la retencion es diferente de 0.00 se escribe el registro de datos de los impuestos retenidos
                        if (lista[15].ToString().Trim() != "0.00")
                        {

                            //042 Datos de los impuestos retinidos (RET) en conceptos(0:N) crear por concepto (retencion)

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



                        // escritor.WriteLine("04|" + j.ToString() + "| " + lista[2] + "|" + double.Parse(lista[0].ToString()).ToString() + "|" + lista[3].ToString().Trim() + "|" +
                        // txtSubtotal.Text.Trim() + "|" + txtSubtotal.Text.Trim() + "|" + (checkAdenda.Checked == true ? "NO APLICA" : lista[1].ToString().Trim()) + "|");

                        //044 INFORMACION ADICIONAL DE LOS CONCEPTOS (0:N) PARA ADENDA LIVERPOOL
                        escritor.WriteLine(
                        "044"                                                   //1-Tipo de Registro
                        + "|" + j.ToString()                                              //2-Consecutivo Concepto
                        + "|" + "0"                                              //3-Variable
                        + "|" + lista[2].ToString().Trim()                      //4-Valor
                        + "|"                                                   //Fin Del Registro
                        );
                        escrituraFactura += "\\n044"                                                   //1-Tipo de Registro
                        + "|" + j.ToString()                                              //2-Consecutivo Concepto
                        + "|" + "0"                                              //3-Variable
                        + "|" + lista[2].ToString().Trim()                      //4-Valor
                        + "|"                                                   //Fin Del Registro
                        ;


                    }
                    j++;
                }

                //----------------------------------------Seccion de detalles de la factura leidos desde vista_fe_detail-------------------------------------------------------------------

                //06 DATOS DE LOS IMPUESTOS TRASLADADOS (0:N)
                escritor.WriteLine(
                "06"                                                   //1-Tipo de Registro
                + "|" + coditrans                                      //2-Impuesto
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
                escritor.WriteLine("08|Origen|" + origen.Trim() + "|");
                escritor.WriteLine("08|OrigenRemitente|" + origenRemitente.Trim() + "|");
                escritor.WriteLine("08|DomicilioOrigen|" + domicilioOrigen.Trim() + "|");
                escritor.WriteLine("08|RfcOrigen|" + rfcOrigen.Trim() + "|");
                escritor.WriteLine("08|Destino|" + destino.Trim() + "|");
                escritor.WriteLine("08|Destinatario|" + destinatario.Trim() + "|");
                escritor.WriteLine("08|Domicilio|" + domicilioDestino.Trim() + "|");
                escritor.WriteLine("08|RfcDestino|" + rfcDestino.Trim() + "|");
                escritor.WriteLine("08|CuotaConvenida|" + cuotaConvenida.Trim() + "|");
                escritor.WriteLine("08|ValorComercial|" + valorComercial.Trim() + "|");
                escritor.WriteLine("08|Remision|" + remision.Trim() + "|");
                escritor.WriteLine("08|Contiene|" + contiene.Trim() + "|");
                escritor.WriteLine("08|Comentario|" + txtDescripcion.Text.Trim() + "|");
                escritor.WriteLine("08|AmparaRemisiones|" + amparaRemisiones.Trim() + "|");
                escritor.WriteLine("08|PesoEstimado|" + peso.Trim() + "|");
                escritor.WriteLine("08|Orden|" + orden.Trim() + "|");
                escritor.WriteLine("08|Master|" + mb.Trim() + "|");
                escritor.WriteLine("08|Invoice|" + invoice.Trim() + "|");
                escritor.WriteLine("08|Movimiento|" + movimiento.Trim() + "|");
                escritor.WriteLine("08|Operador|" + operador.Trim() + "|");
                escritor.WriteLine("08|OperadorLicencia|" + licencia.Trim() + "|");
                escritor.WriteLine("08|TractoEco|" + tratorEco.Trim() + "|");
                escritor.WriteLine("08|TractoPlaca|" + tractorPlacas.Trim() + "|");
                escritor.WriteLine("08|Remolque1Eco|" + remol1Eco.Trim() + "|");
                escritor.WriteLine("08|Remolque1Placa|" + remol1Placas.Trim() + "|");
                escritor.WriteLine("08|Remolque2Eco|" + remol2Eco.Trim() + "|");
                escritor.WriteLine("08|Remolque2Placa|" + remol2Placas.Trim() + "|");
                escritor.WriteLine("08|Documento|" + documento.Trim() + "|");
                escritor.WriteLine("08|FechaPagare|" + fechaPagare.Trim() + "|");
                escritor.WriteLine("08|InteresesPagare|" + intereses.Trim() + "|");
                escritor.WriteLine("08|ClientePagare|" + clientePagare.Trim() + "|");

                escrituraFactura += "\\n08|Origen|" + origen.Trim() + "|";
                escrituraFactura += "\\n08|OrigenRemitente|" + origenRemitente.Trim() + "|";
                escrituraFactura += "\\n08|DomicilioOrigen|" + domicilioOrigen.Trim() + "|";
                escrituraFactura += "\\n08|RfcOrigen|" + rfcOrigen.Trim() + "|";
                escrituraFactura += "\\n08|Destino|" + destino.Trim() + "|";
                escrituraFactura += "\\n08|Destinatario|" + destinatario.Trim() + "|";
                escrituraFactura += "\\n08|Domicilio|" + domicilioDestino.Trim() + "|";
                escrituraFactura += "\\n08|RfcDestino|" + rfcDestino.Trim() + "|";
                escrituraFactura += "\\n08|CuotaConvenida|" + cuotaConvenida.Trim() + "|";
                escrituraFactura += "\\n08|ValorComercial|" + valorComercial.Trim() + "|";
                escrituraFactura += "\\n08|Remision|" + remision.Trim() + "|";
                escrituraFactura += "\\n08|Contiene|" + contiene.Trim() + "|";
                escrituraFactura += "\\n08|Comentario|" + txtDescripcion.Text.Trim() + "|";
                escrituraFactura += "\\n08|AmparaRemisiones|" + amparaRemisiones.Trim() + "|";
                escrituraFactura += "\\n08|PesoEstimado|" + peso.Trim() + "|";
                escrituraFactura += "\\n08|Orden|" + orden.Trim() + "|";
                escrituraFactura += "\\n08|Master|" + mb.Trim() + "|";
                escrituraFactura += "\\n08|Invoice|" + invoice.Trim() + "|";
                escrituraFactura += "\\n08|Movimiento|" + movimiento.Trim() + "|";
                escrituraFactura += "\\n08|Operador|" + operador.Trim() + "|";
                escrituraFactura += "\\n08|OperadorLicencia|" + licencia.Trim() + "|";
                escrituraFactura += "\\n08|TractoEco|" + tratorEco.Trim() + "|";
                escrituraFactura += "\\n08|TractoPlaca|" + tractorPlacas.Trim() + "|";
                escrituraFactura += "\\n08|Remolque1Eco|" + remol1Eco.Trim() + "|";
                escrituraFactura += "\\n08|Remolque1Placa|" + remol1Placas.Trim() + "|";
                escrituraFactura += "\\n08|Remolque2Eco|" + remol2Eco.Trim() + "|";
                escrituraFactura += "\\n08|Remolque2Placa|" + remol2Placas.Trim() + "|";
                escrituraFactura += "\\n08|Documento|" + documento.Trim() + "|";
                escrituraFactura += "\\n08|FechaPagare|" + fechaPagare.Trim() + "|";
                escrituraFactura += "\\n08|InteresesPagare|" + intereses.Trim() + "|";
                escrituraFactura += "\\n08|ClientePagare|" + clientePagare.Trim() + "|";


                //----------------------------------------Seccion de datos de relacion de CFDI-------------------------------------------------------------------------------------------

                //SI CONTAMOS CON UN UUID DE RELACION SE ESCRIBE EL REGISTRO
                if (uuidrel != (""))
                {
                    //09 DATOS DE CFDIS RELACIOnADOS(0:N)
                    escritor.WriteLine(
                    "09"                                                      //1-Tipo de Registro
                    + "|" + relacion                                          //2-Tipo de Relacion
                    + "|" + uuidrel                                           //3-UUID
                    + "|"                                                     //Fin del Registro
                    );

                    escrituraFactura += "\\n09"                                                      //1-Tipo de Registro
                    + "|" + relacion                                             //2-Tipo de Relacion
                    + "|" + uuidrel                                           //3-UUID
                    + "|";

                    relacion = null;
                    uuidrel = null;

                }

                //----------------------------------------Seccion Adenda para billtos del grupo LIVERPOOL-------------------------------------------------------------------------------


                if (clienteId.Equals("LIVERPOL") || clienteId.Equals("ALMLIVER") || clienteId.Equals("LIVERSUB") || clienteId.Equals("LIVERSUR") || clienteId.Equals("LIVERTIJ") || clienteId.Equals("SFERALIV") || clienteId.Equals("GLOBALIV") || clienteId.Equals("SETRALIV") || clienteId.Equals("FACTUMLV") || clienteId.Equals("MERCANLV") || clienteId.Equals("LIVERDED"))
                //if (clienteId.Equals("LIVERPOL") || clienteId.Equals("ALMLIVER") || clienteId.Equals("LIVERTIJ"))
                {
                    //escritor.WriteLine("12|Otros|" + otros1.Trim() + "|");


                    //if (true)
                    //{
                    DateTime fFactura = Convert.ToDateTime(fecha.Trim());
                    string mes = fFactura.Month < 10 ? "0" + fFactura.Month : fFactura.Month.ToString();
                    string fechaTxt = fFactura.Year + "-" + mes + "-" + fFactura.Day;

                    escritor.WriteLine("20|ORIGINAL||ZZZ|" + txtCantidadLetra.Text.Trim() + "|||||||" + fechaTxt.Trim() + "|" + fechaTxt.Trim() + "||||||" + txtSubtotal.Text.Trim() + "|");
                    escritor.WriteLine("21|" + txtPedido.Text.Trim() + "|");
                    escritor.WriteLine("22|" + proveedor + "|SELLER_ASSIGNED_IDENTIFIER_FOR_A_PARTY|" + prov + "||||||||");
                    //escritor.WriteLine("24|" + moneda.Trim() + "|BILLING CURRENCY|" + String.Format("{0:0.00}", double.Parse(tipoCambio.Trim())) + "|");
                    escritor.WriteLine("25|ALLOWANCE_GLOBAL|BILL_BACK||AJ|0.00|0.00|");

                    escrituraFactura += "\\n20|ORIGINAL||ZZZ|" + txtCantidadLetra.Text.Trim() + "|||||||" + fechaTxt.Trim() + "|" + fechaTxt.Trim() + "||||||" + txtSubtotal.Text.Trim() + "|";
                    escrituraFactura += "\\n21|" + txtPedido.Text.Trim() + "|";
                    escrituraFactura += "\\n22|" + proveedor + "|SELLER_ASSIGNED_IDENTIFIER_FOR_A_PARTY|" + prov + "||||||||";
                    //escrituraFactura += "\\n24|" + moneda.Trim() + "|BILLING CURRENCY|" + String.Format("{0:0.00}", double.Parse(tipoCambio.Trim())) + "|";
                    escrituraFactura += "\\n25|ALLOWANCE_GLOBAL|BILL_BACK||AJ|0.00|0.00|";

                    //Estoy mandando el 5

                    //for (int i = 1; i <= concept.Count; i++)
                    //{
                    escritor.WriteLine("26|1" + "|" + fact.ToString() + "|" + fact.ToString() + "|SUPPLIER_ASSIGNED|ES|" + String.Format("{0:0.00}", double.Parse(txtSubtotal.Text.Trim())) +
                            "|" + String.Format("{0:0.00}", double.Parse(txtTotal.Text.Trim())) + "|" + String.Format("{0:0.00}", double.Parse(txtSubtotal.Text.Trim())) + "|" + String.Format("{0:0.00}", double.Parse(txtTotal.Text.Trim())) + "|SRV|1||PAID_BY_BUYER|5400017710|NA|");
                    escrituraFactura += "\\n26|1" + "|" + fact.ToString() + "|" + fact.ToString() + "|SUPPLIER_ASSIGNED|ES|" + String.Format("{0:0.00}", double.Parse(txtSubtotal.Text.Trim())) +
                           "|" + String.Format("{0:0.00}", double.Parse(txtTotal.Text.Trim())) + "|" + String.Format("{0:0.00}", double.Parse(txtSubtotal.Text.Trim())) + "|" + String.Format("{0:0.00}", double.Parse(txtTotal.Text.Trim())) + "|SRV|1||PAID_BY_BUYER|5400017710|NA|";

                    //}

                    escritor.WriteLine("29|ALLOWANCE|AJ|" + String.Format("{0:0.00}", double.Parse(txtSubtotal.Text.Trim())) + "|");
                    escritor.WriteLine("30|A|IV|");
                    escritor.WriteLine("30|174542|ATZ|");
                    escritor.WriteLine("31|" + txtHojaEntrada.Text.Trim() + "|");
                    escritor.Write("32|7504000107903|0101|");

                    escrituraFactura += "\\n29|ALLOWANCE|AJ|" + String.Format("{0:0.00}", double.Parse(txtSubtotal.Text.Trim())) + "|";
                    escrituraFactura += "\\n30|A|IV|";
                    escrituraFactura += "\\n30|174542|ATZ|";
                    escrituraFactura += "\\n31|" + txtHojaEntrada.Text.Trim() + "|";
                    escrituraFactura += "\\n32|7504000107903|0101|";
                }

                else
                {
                    //escritor.Write("12|Otros|" + otros1.Trim() + "|");
                }
            }
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
            txtDescripcion.ReadOnly = readOnly;

            //Se elimina el estilo
            txtDescripcion.CssClass = stilo;

            btnEdit.Text = textoBoton;
        }
        protected void generarCFDI()
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

                    var client = new RestClient("https://canal1.xsa.com.mx:9050/bf2e1036-ba47-49a0-8cd9-e04b36d5afd4/cfdis");
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

                    foreach (string factura in separadaFactura)
                    {
                        if (factura.Contains("errors") && !factura.Contains("ya se encuentra utilizado"))
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
                        string imaging = "http://172.16.136.34/cgi-bin/img-docfind.pl?reftype=ORD&refnum=" + orden.ToString().Trim();

                        DateTime fecha1 = Convert.ToDateTime(fecha.ToString());
                        string fechaFinal = fecha1.Year + "-" + fecha1.Month + "-" + fecha1.Day + " " + fecha1.Hour + ":" + fecha1.Minute + ":" + fecha1.Second + "." + fecha1.Millisecond;

                        facLabControler.generadas(mbnumber.ToString(), fact, serie, idreceptor, fechaFinal, txtTotal.Text.ToString().Trim(),
                            moneda, pdf_descargaFactura, xlm_descargaFactura, imaging, bandera, "Tralix", "VAL", ultinvoice, hecha, orden.Trim(), rfcDestino.Trim());
                    }

                    //PopupMsg.Message1 = "Se ha generado correctamente el CFDi, será enviado a tu correo electrónico o podrás encontrarlo en el portal de búsqueda.";
                    //PopupMsg.ShowPopUp(0);
                }
                else
                {
                    //PopupMsg.Message1 = "Error al conectar al servicio XSA";
                    //PopupMsg.ShowPopUp(0);
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myScriptExiste", "<script>javascript:setExiste(true);</script>");
            }
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

                generaTXT();

                string ftp = System.Web.Configuration.WebConfigurationManager.AppSettings["ftp"];
                if (ftp.Equals("Si"))
                {
                    string path = System.Web.Configuration.WebConfigurationManager.AppSettings["dir"] + lblFact.Text + ".txt";
                    UploadFiles file = new UploadFiles();
                    file.prubeftp(lblFact.Text + ".txt", path, serie);
                }

                if (System.Web.Configuration.WebConfigurationManager.AppSettings["activa"].Equals("Si"))
                {
                    //Modifica referencia
                    facLabControler.actualizaFactura(lblFact.Text, txtFactura.Text, mbnumber);

                    //Inserta en la tabla de generadas
                    string rutaPDF = "https://" + System.Web.Configuration.WebConfigurationManager.AppSettings["servidord"] + "/xsamanager/downloadCfdWebView?serie=" + serie + "&folio=" + fact + "&tipo=PDF&rfc=TTR931201KJ6&key=" + System.Web.Configuration.WebConfigurationManager.AppSettings["llave"];
                    string rutaXML = "https://" + System.Web.Configuration.WebConfigurationManager.AppSettings["servidord"] + "/xsamanager/downloadCfdWebView?serie=" + serie + "&folio=" + fact + "&tipo=XML&rfc=TTR931201KJ6&key=" + System.Web.Configuration.WebConfigurationManager.AppSettings["llave"];
                    string imaging = "http://172.16.136.34/cgi-bin/img-docfind.pl?reftype=ORD&refnum=" + orden.ToString().Trim();

                    DateTime fecha1 = Convert.ToDateTime(fecha.ToString());
                    string fechaFinal = fecha1.Year + "-" + fecha1.Month + "-" + fecha1.Day + " " + fecha1.Hour + ":" + fecha1.Minute + ":" + fecha1.Second + "." + fecha1.Millisecond;

                    facLabControler.generadas(mbnumber.ToString(), fact, serie, idreceptor, fechaFinal, txtTotal.Text.ToString().Trim(),
                        moneda, rutaPDF, rutaXML, imaging, bandera, "Tralix", "VAL", ultinvoice, hecha, orden.Trim(), rfcDestino.Trim());
                }

                //ClientScript.RegisterStartupScript(this.GetType(), "myScriptExiste", "<script>javascript:closeWindow();</script>");

                //PopupMsg.Message1 = "Se ha generado correctamente el CFDi, será enviado a tu correo electrónico o podrás encontrarlo en el portal de búsqueda.";
                //PopupMsg.ShowPopUp(0);


            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myScriptExiste", "<script>javascript:setExiste(true);</script>");
            }
        }
        protected void btnBloquear_Click(object sender, EventArgs e)
        {

        }

        protected void txtSubtotal_TextChanged(object sender, EventArgs e)
        {

        }

    }
}