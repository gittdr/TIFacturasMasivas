using FactMasiva.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TIFacturasMasivas
{
    public partial class Main : System.Web.UI.Page
    {
        public FactLabControler facLabControler = new FactLabControler();
        Hashtable detalle = new Hashtable();
        public SearchControler busquedaControler = new SearchControler();
        public GridViewControl gridControl = new GridViewControl();
        GridView ItemsGrid;
        public string queryGeneral, url;
        public string moneda, tipoCambio, MetodoPago33, tipocomprobante, usocdfi, confirmacion, relacion, uuidrel, lugarexpedicion, idreceptor, pais, calle,
            noExt, noInt, colonia, localidad, referencia, municipio, estado, cp, estatus, paisresidencia, numtributacion, mailenvio, serie, tipofactor, tasatras, codirete, tasarete, coditrans;
        public string cuota, valorComercial, remision, condicionesPago, contiene, comentarios, amparaRemisiones,
            operador, licencia, tratorEco, tractorPlacas, remol1Eco, remol1Placas, remol2Eco, remol2Placas;
        string flete, maniobras, otros, rentaEquipo, cpac, cargoXSeguro, autopistas,
                    fechaPagare, interesesPagare, clientePagare, fpago, metpago, cuentaRef, factur, fecha, nombre, folio;
        string rfc, origen, origenRemitente, domicilioOrigen, rfcOrigen, destino, destinatario, domicilioDestino, rfcDestino, peso, orden, master,
                    movimiento, factura, subtotal, iva, retencion, documento, cantidadLetra, total, bandera, ultinvoice, hecha;
        public string clienteId;



        protected void Page_Load(object sender, EventArgs e)
        {
            string client = Request.QueryString["cliente"];
            string hecha = Request.QueryString["hecha"];
            int posicion;
            if (!IsPostBack)
            {
                //CARGAR DROPDOWN              
                DataTable td_FACTS = facLabControler.facturasClientes();
                lstCliente.DataSource = td_FACTS;
                lstCliente.DataTextField = "idreceptor";                            // FieldName of Table in DataBase
                lstCliente.DataValueField = "idreceptor";
                lstCliente.DataBind();
            }
            if (IsPostBack)
            {

                queryGeneral = generaQuery();
                if (!queryGeneral.Equals(""))
                {
                    createGrid(queryGeneral);
                    int count = ItemsGrid.Rows.Count;
                    if (count == 0)
                    {
                        Label mensaje = new Label();
                        mensaje.CssClass = "labelStile";
                        mensaje.Text = "No se obtuvieron resultados de la búsqueda";
                        gridPlace.Controls.Add(mensaje);
                    }
                }
            }
        }

        //Methods
        public string generaQuery()
        {
            lblError.Text = "";
            ArrayList empty = new ArrayList();

            int vacio = 0;
            int cont = 0;
            bool error = false;
            string query = "select * from vista_fe_header where ";

            if (!lstCliente.SelectedValue.Equals(""))
            {
                query = query + "idreceptor = '" + lstCliente.SelectedValue + "'";
            }
            else { vacio = 1; cont++; empty.Add(1); }

            if (!lstElaborada.SelectedValue.Equals(""))
            {
                if (empty.Count == 1) { query += "  hecha = '" + lstElaborada.SelectedValue + "'"; }
                else { query += " and hecha = '" + lstElaborada.SelectedValue + "'"; }
            }
            else { vacio = 2; cont++; empty.Add(2); }


            // query += "  and idreceptor != 'LIVERPOL' and idreceptor != 'ALMLIVER' and idreceptor != 'LIVERTIJ' and idreceptor != 'SETRALIV' and idreceptor != 'SFERALIV' and idreceptor != 'GLOBALIV' and idreceptor != 'FACTUMLV' order by folio";

            if (empty.Count == 2)
            {
                lblError.Text = "Selecciona algún criterio de búsqueda.";
                error = true;
                query = "";
            }

            if (error) { query = ""; }

            return query;
        }

        public void createGrid(string query)
        {
            gridPlace.Controls.Clear();
            // Create a DataGrid control.
            ItemsGrid = new GridView();


            // Set the properties of the DataGrid.

            //ItemsGrid.DataKeyNames = new string[] { "serie" };
            ItemsGrid.AutoGenerateColumns = false;
            ItemsGrid.CssClass = "mGrid";
            ItemsGrid.PagerStyle.CssClass = "pgr";
            ItemsGrid.AlternatingRowStyle.CssClass = "alt";
            ItemsGrid.AutoGenerateSelectButton = false;
            ItemsGrid.SelectedRowStyle.BackColor = System.Drawing.Color.LightYellow;
            ItemsGrid.SelectedRowStyle.ForeColor = System.Drawing.Color.Black;
            ItemsGrid.SelectedRowStyle.Font.Bold = true;

            ItemsGrid.RowDataBound += new GridViewRowEventHandler(itemsGrid_rowDataBound);

            ItemsGrid.Columns.Add(gridControl.check());
            ItemsGrid.Columns.Add(gridControl.CreateBoundColumn("nmaster", "Master", 80));
            ItemsGrid.Columns.Add(gridControl.CreateBoundColumn("folio", "Invoice", 80));
            ItemsGrid.Columns.Add(gridControl.CreateBoundColumn("idreceptor", "Cliente", 80));
            ItemsGrid.Columns.Add(gridControl.CreateBoundColumn("total", "Cantidad", 80));
            ItemsGrid.Columns.Add(gridControl.CreateBoundColumn("fhemision", "Fecha", 120));
            DataTable datos = busquedaControler.search(query);

            ItemsGrid.DataSource = datos;
            ItemsGrid.DataBind();

            gridPlace.Controls.Add(ItemsGrid);

            chkSelectAll.Text = " " + "Seleccionar: " + datos.Rows.Count + "";
        }
        //Metodo para obtener los datos de la factura
        private void iniciaDatos(string fact)
        {
            DataTable td = facLabControler.detalleFacturas(fact);

            foreach (DataRow row in td.Rows)
            {
                hecha = row["hecha"].ToString();
                ultinvoice = row["ultinvoice"].ToString();
                bandera = row["bandera"].ToString();
                folio = row["folio"].ToString();
                serie = row["serie"].ToString();
                moneda = row["moneda"].ToString();
                tipoCambio = row["tipocambio"].ToString();
                idreceptor = row["idreceptor"].ToString();

                //Obtengo el cliente para poder validar la denda de liverpool.
                clienteId = idreceptor;


                factur = row["idcomprobante"].ToString();
                DateTime dt = DateTime.Parse(row["fhemision"].ToString());
                fecha = dt.ToString("yyyy'/'MM'/'dd HH:mm:ss");
                //fecha = row["fhemision"].ToString();
                nombre = row["nombrecliente"].ToString();
                calle = row["calle"].ToString();
                noExt = row["numext"].ToString();
                noInt = row["numint"].ToString();
                colonia = row["colonia"].ToString();
                municipio = row["municdeleg"].ToString();
                estado = row["estado"].ToString();
                cp = row["cp"].ToString();
                pais = row["pais"].ToString();

                rfc = row["rfc"].ToString();
                origen = row["origen"].ToString();
                origenRemitente = row["remitente"].ToString();
                domicilioOrigen = row["domicilioorigen"].ToString();
                rfcOrigen = row["rfcorigen"].ToString();
                destino = row["destino"].ToString();
                destinatario = row["destinatario"].ToString();
                domicilioDestino = row["domiciliodestino"].ToString();
                rfcDestino = row["rfcdestino"].ToString();
                cuota = row["cuotaconv"].ToString();
                valorComercial = row["valorcomer"].ToString();
                remision = row["remision"].ToString();
                condicionesPago = row["condpago"].ToString();
                contiene = row["contiene"].ToString();
                comentarios = row["comentarios"].ToString();
                amparaRemisiones = row["ampararemisiones"].ToString();

                peso = row["pesoestimado"].ToString();
                orden = row["orden"].ToString();
                master = row["nmaster"].ToString();
                factura = row["invoice"].ToString();
                movimiento = row["movimiento"].ToString();
                operador = row["operador"].ToString();
                licencia = row["operadorlicenicia"].ToString();
                tratorEco = row["tractoeco"].ToString();
                tractorPlacas = row["tractoplaca"].ToString();
                remol1Eco = row["remolque1Eco"].ToString();
                remol1Placas = row["remolque1Placa"].ToString();
                remol2Eco = row["remolque2Eco"].ToString();
                remol2Placas = row["remolque2Placa"].ToString();

                subtotal = row["subtotal"].ToString().Remove(row["subtotal"].ToString().Length - 2);
                iva = row["imptras"].ToString().Remove(row["imptras"].ToString().Length - 2);
                retencion = row["imprete"].ToString().Remove(row["imprete"].ToString().Length - 2);
                total = row["total"].ToString().Remove(row["total"].ToString().Length - 2);


                if (retencion == "0.00")
                {
                    retencion = "";

                }



                documento = row["documento"].ToString();
                cantidadLetra = row["totalletra"].ToString();

                Hashtable datosReales = generaActualizacion(fact);

                if (datosReales.ContainsKey("FLETE")) { flete = datosReales["FLETE"].ToString(); } else { flete = ""; }
                if (datosReales.ContainsKey("MANIOBRAS")) { maniobras = datosReales["MANIOBRAS"].ToString(); } else { maniobras = ""; }
                if (datosReales.ContainsKey("OTROS")) { otros = datosReales["OTROS"].ToString(); } else { otros = ""; }
                if (datosReales.ContainsKey("RENTA EQUIPO")) { rentaEquipo = datosReales["RENTA EQUIPO"].ToString(); } else { rentaEquipo = ""; }
                if (datosReales.ContainsKey("CPAC")) { cpac = datosReales["CPAC"].ToString(); } else { cpac = ""; }
                if (datosReales.ContainsKey("CARGO POR SEGURO")) { cargoXSeguro = datosReales["CARGO POR SEGURO"].ToString(); } else { cargoXSeguro = ""; }
                if (datosReales.ContainsKey("AUTOPISTAS")) { autopistas = datosReales["AUTOPISTAS"].ToString(); } else { autopistas = ""; }

                fechaPagare = row["fechapagare"].ToString();
                interesesPagare = row["interesespagare"].ToString();
                clientePagare = row["clientepagare"].ToString();
                fpago = row["fpago"].ToString();
                condicionesPago = row["condpago"].ToString();
                metpago = row["metpago"].ToString();
                cuentaRef = row["cuentaref"].ToString();

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


            }
        }

        private Hashtable generaActualizacion(string fact)
        {
            detalle.Clear();
            Hashtable campos = new Hashtable();
            DataTable td = facLabControler.detalleFacturas(fact);
            int i = 0;

            foreach (DataRow registro33 in td.Rows)
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

                detalle.Add(i, lst33);
                i++;
            }
            return campos;
        }

        public void generaTXT()
        {
            string path = System.Web.Configuration.WebConfigurationManager.AppSettings["dir"] + folio.ToString() + ".txt";
            using (System.IO.StreamWriter escritor = new System.IO.StreamWriter(path))
            {

                //----------------------------------------Seccion De Datos Generales del CFDI-----------------------------------------------------------------------------------------

                //01 INFORMACION GENERAL DEL CFDI (1:1)

                escritor.WriteLine(
                "01"                                                //1.-Tipo De Registro
                + "|" + factur.Trim()                               //2-ID Comprobante
                + "|" + serie                                       //3-Serie
                + "|" + folio.Trim().Substring(1)                   //4-Foliio 
                + "|" + fecha.Trim()                                //5-Fecha y Hora De Emision
                + "|" + subtotal.Trim()                             //6-Subtotal
                + "|" + iva.Trim()                                  //7-Total Impuestos Trasladados
                + "|" + retencion.Trim()                            //8-Total Impuestos Retenidos
                + "|"                                               //9-Descuentos
                + "|" + total.Trim()                                //10-Total
                + "|" + cantidadLetra.Trim()                        //11-Total Con Letra
                + "|" + metpago.Trim()                                //12-Forma De Pago
                + "|" + condicionesPago.Trim()                      //13-Condiciones De Pago
                + "|" + MetodoPago33                                //14-Metodo de Pago
                + "|" + moneda.Trim()                               //15-Moneda
                + "|" + tipoCambio.Trim()                           //16-Tipo De Cambio
                + "|" + tipocomprobante                             //17-Tipo De Comprobante
                + "|" + lugarexpedicion                             //18-Lugar De Expedicion                                        
                + "|" + usocdfi                                     //19-Uso CFDI
                + "|" + confirmacion                                //20-Confirmacion
                + "|"
                );

                //----------------------------------------Seccion de los datos del receptor del CFDI -------------------------------------------------------------------------------------

                //02 INFORMACION DEL RECEPTOR (1:1)

                escritor.WriteLine(
                "02"                                                   //1-Tipo De Registro
                + "|" + idreceptor                                     //2-Id Receptor
                + "|" + rfc.Trim()                                     //3-RFC
                + "|" + nombre.Trim()                                  //4-Nombre
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

                //----------------------------------------Seccion de detalles de la factura leidos desde vista_fe_detail-------------------------------------------------------------------

                int j = 1;  //Se declara la varible que se incrementara para recorrer el Hashtable detalle33
                foreach (int item in detalle.Keys) //Se recorren los registros del Hashtable Detalle33
                {
                    ArrayList lista = (ArrayList)detalle[item];

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
                + "|" + iva.Trim()                                     //5-Importe
                + "|"                                                  //Fin Del Registro
                );

                //Si el valor del importe de la retencion es diferente de 0.00 se escribe el registro de datos de los impuestos retenidos

                if (retencion.Trim() != "")
                {

                    //07 DATOS DE LOS IMPUESTOS RETENIDOS (0:N)
                    escritor.WriteLine(
                    "07"                                                   //1-Tipo de Registro
                    + "|" + codirete                                       //2-Impuesto
                    + "|" + tasarete                                       //3-Tasa o Cuota
                    + "|" + retencion.Trim()                               //4-Importe
                    + "|"                                                  //Fin Del Registro
                    );
                }

                //----------------------------------------Seccion de datos extra para PDF CFDI-------------------------------------------------------------------------------------------

                //08 INFORMACION ADICIONAL COMPROBANTE (0:N)
                escritor.WriteLine("08|IVA|16.00|" + iva.Trim() + "|");
                escritor.WriteLine("08|IVA|4.00|" + retencion.Trim() + "|");
                escritor.WriteLine("08|Origen|" + origen.Trim() + "|");
                escritor.WriteLine("08|OrigenRemitente|" + origenRemitente.Trim() + "|");
                escritor.WriteLine("08|DomicilioOrigen|" + domicilioOrigen.Trim() + "|");
                escritor.WriteLine("08|RfcOrigen|" + rfcOrigen.Trim() + "|");
                escritor.WriteLine("08|Destino|" + rfcDestino.Trim() + "|");
                escritor.WriteLine("08|Destinatario|" + destinatario.Trim() + "|");
                escritor.WriteLine("08|Domicilio|" + domicilioDestino.Trim() + "|");
                escritor.WriteLine("08|RfcDestino|" + rfcDestino.Trim() + "|");
                escritor.WriteLine("08|CuotaConvenida|" + cuota.Trim() + "|");
                escritor.WriteLine("08|ValorComercial|" + valorComercial.Trim() + "|");
                escritor.WriteLine("08|Remision|" + remision.Trim() + "|");
                escritor.WriteLine("08|Contiene|" + contiene.Trim() + "|");
                escritor.WriteLine("08|Comentario|" + comentarios.Trim() + "|");
                escritor.WriteLine("08|AmparaRemisiones|" + amparaRemisiones.Trim() + "|");
                escritor.WriteLine("08|PesoEstimado|" + peso.Trim() + "|");
                escritor.WriteLine("08|Orden|" + orden.Trim() + "|");
                escritor.WriteLine("08|Master|" + master.Trim() + "|");
                escritor.WriteLine("08|Invoice|" + factura.Trim() + "|");
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
                escritor.WriteLine("08|InteresesPagare|" + interesesPagare.Trim() + "|");
                escritor.WriteLine("08|ClientePagare|" + clientePagare.Trim() + "|");
                escritor.WriteLine("08|Flete|" + flete.Trim() + "|");
                escritor.WriteLine("08|Seguro|" + cargoXSeguro.Trim() + "|");
                escritor.WriteLine("08|Maniobras|" + maniobras.Trim() + "|");
                escritor.WriteLine("08|Pistas|" + autopistas.Trim() + "|");
                escritor.WriteLine("08|RentaEquipo|" + rentaEquipo.Trim() + "|");
                escritor.WriteLine("08|Cpac|" + cpac.Trim() + "|");
                escritor.WriteLine("08|Otros|" + otros.Trim() + "|");

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

                }

                //----------------------------------------Seccion Adenda para HOME DEPOT-----------------------------------------------------------------------------------------------

                if (clienteId.Equals("HOMEDEP"))
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

                    DateTime hfFactura = Convert.ToDateTime(fecha.Trim());

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

                    escritor.WriteLine("08|Otros|" + otros.Trim() + "|");
                    escritor.WriteLine("08|RFPT|SimpleInvoiceType|");
                    escritor.WriteLine("08|RFPCV|1.3.1|");
                    escritor.WriteLine("08|RFPDSV|AMC7.1|");
                    escritor.WriteLine("08|RFPDS|ORIGINAL|");
                    escritor.WriteLine("08|RFPDD|" + hfechaTxt + "|");   //texto que tenga la hora y fecha de emision. ********LISTO
                    escritor.WriteLine("08|RFPIET|" + ttcompre + "|"); //texto que tenga si es INVOICE o  CREDIT_NOTE ********LISTO
                    escritor.WriteLine("08|RFPIUCI|" + serie.Trim() + folio.Trim() + "|"); //texto que tenga la serie mas el folio. ********LISTO
                    escritor.WriteLine("08|OIRI|" + comentarios.Trim() + "|"); //texto que tenga el num de la orden de compra. ********LISTO
                    escritor.WriteLine("08|OIT|ON|");
                    escritor.WriteLine("08|AIRI|" + comentarios.Trim() + "|"); //el num de la orden de compra. es el de comentarios********LISTO
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
                    escritor.WriteLine("044|1|TIIG|" + remision.Trim() + "|"); //num de la referencia. buscar cual es ********LISTO
                    escritor.WriteLine("044|1|ATII|" + remision.Trim() + "|"); //num de la referencia. buscar cual es ********LISTO
                    escritor.WriteLine("044|1|ATIIT|BUYER_ASSIGNED|");
                    escritor.WriteLine("044|1|TIDIL|ES|");
                    escritor.WriteLine("044|1|TIDILLT|FLETE|");
                    escritor.WriteLine("044|1|IQ|1.00|");
                    escritor.WriteLine("044|1|IQUOM|AU|");
                    escritor.WriteLine("044|1|GPA|" + String.Format("{0:0.00}", double.Parse(subtotal.Trim())) + "|"); //texto que tenga COSTO SIN IVA IMPUESTOS ***EN PRUEBA
                    escritor.WriteLine("044|1|PIPQ|1|");
                    escritor.WriteLine("044|1|PID|EACH|");
                    escritor.WriteLine("044|1|PIT|EXCHANGE_PALLETS|");
                    escritor.WriteLine("044|1|PITRMOP|PREPAID_BY_SELLER|");
                    escritor.WriteLine("044|1|TITITTD|VAT|");
                    escritor.WriteLine("044|1|TITITITATP|16.00|"); //texto que tenga EL % DE IVA A APLICAR ***EN PRUEBA
                    escritor.WriteLine("044|1|TITITITATA|" + String.Format("{0:0.00}", double.Parse(iva.Trim())) + "|"); //texto que tenga EL MONTO DE IVA A APLICAR ***EN PRUEBA
                    escritor.WriteLine("044|1|TLANAA|" + String.Format("{0:0.00}", double.Parse(subtotal.Trim())) + "|"); // COSTO SIN IVA IMPUESTOS (COSTO UNIT * UNIDADES)***EN PRUEBA


                    escritor.WriteLine("08|TACAOCT|ALLOWANCE|");
                    escritor.WriteLine("08|TACAOCTA|0.00|");
                    escritor.WriteLine("08|BAA|" + String.Format("{0:0.00}", double.Parse(subtotal.Trim())) + "|"); //subtotal (SUMA DE TODAS LAS LINEAS)********LISTO

                    escritor.WriteLine("13|VAT|4.00|" + String.Format("{0:0.00}", double.Parse(retencion.Trim())) + "|RETENIDO|"); //texto que tenga EL TOTAL DE LAS RETENCIONES ********LISTO
                    escritor.WriteLine("13|VAT|16.00|" + String.Format("{0:0.00}", double.Parse(iva.Trim())) + "|TRANSFERIDO|"); //texto que tenga EL TOTAL DEL IVA ********LISTO

                    escritor.Write("08|PAA|" + String.Format("{0:0.00}", double.Parse(total.Trim())) + "|"); //texto que tenga COSTO TOTAL DE TODO CON IVA Y RETENCION. ********LISTO


                }

                if (clienteId.Equals("ENVASESU"))
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

                    DateTime hfFactura = Convert.ToDateTime(fecha.Trim());

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
                    escritor.WriteLine("08|IdTransaccion|Con_Pedido|");
                    escritor.WriteLine("08|Transaccion|" + folio.Trim() + "|"); //texto que tenga el folio. ********LISTO
                    escritor.WriteLine("08|SecuenciaConsec|1|");
                    escritor.WriteLine("08|IdPedido|" + remision.Trim() + "|");
                    escritor.WriteLine("08|Albaran|" + comentarios.Trim() + "|");

                }



                if (clienteId.Equals("MABE"))
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
                    escritor.WriteLine("08|REF1|" + comentarios.Trim() + "|"); //el num de la referencia. es el de comentarios********LISTO
                    escritor.WriteLine("08|REF2|NA|"); //el num de la referencia 2 siempre va en NA********LISTO
                    escritor.WriteLine("08|NPROV|3000078|"); //el num de proveedor 2 siempre va en 3000078*******LISTO
                    escritor.WriteLine("08|NPLANT|D004|"); //el num de la planta siempre va en D004*******LISTO
                    escritor.WriteLine("08|DPLANT|PASEO DE LAS PALMAS|"); //la direccion de la planta*******LISTO 
                    escritor.WriteLine("08|NEXTPLANT|100|"); //siempre en na*******LISTO
                    escritor.WriteLine("08|NINPLANT|NA|"); //siempre en na*******LISTO
                    escritor.WriteLine("08|CPLANT|11000|"); //el codigo postal de la planta*******LISTO
                    escritor.WriteLine("08|MONEDA|MXN|"); //el codigo postal de la planta*******LISTO

                    //for (int i = 1; i <= concept.Count; i++)
                    //{
                    //    escritor.WriteLine("05|" + i.ToString() + "|NLINEA|" + i.ToString() + "|");
                    //    escritor.WriteLine("05|1|" + txtFactura.Text.Trim().Substring(0, 1) + "|" + String.Format("{0:0.00}", double.Parse(txtSubtotal.Text.Trim())) + "|"); //texto que tenga COSTO SIN IVA IMPUESTOS ***EN PRUEBA
                    //    escritor.WriteLine("05|1|PCONIVA|" + String.Format("{0:0.00}", double.Parse(txtTotal.Text.Trim())) + "|"); // COSTO CON IVA IMPUESTOS ***EN PRUEBA
                    //    escritor.WriteLine("05|1|SSINIVA|" + String.Format("{0:0.00}", double.Parse(txtSubtotal.Text.Trim())) + "|"); //texto que tenga subtotal SIN IVA IMPUESTOS ***EN PRUEBA
                    //    escritor.WriteLine("05|1|SCONIVA|" + String.Format("{0:0.00}", double.Parse(txtTotal.Text.Trim())) + "|"); // COSTO subtotal con IVA IMPUESTOS ***EN PRUE
                    //}

                    int p = 1;
                    foreach (int item in detalle.Keys)
                    {
                        ArrayList lista = (ArrayList)detalle[item];
                        double monto = 0;

                        if (lista[3].ToString().Trim() == "FLETE") { monto = double.Parse(lista[5].ToString().Trim()) * 1.12; } else { monto = double.Parse(lista[5].ToString().Trim()) * 1.16; }

                        escritor.WriteLine("044|" + p.ToString() + "|NLINEA|" + p.ToString() + "|");
                        escritor.WriteLine("044|" + p.ToString() + "|" + lista[2].ToString().Trim().Substring(0, 1) + "|" + String.Format("{0:0.00}", double.Parse(lista[5].ToString().Trim())) + "|"); //texto que tenga COSTO SIN IVA IMPUESTOS ***EN PRUEBA
                        escritor.WriteLine("044|" + p.ToString() + "|PCONIVA|" + String.Format("{0:0.00}", monto) + "|"); // COSTO CON IVA IMPUESTOS ***EN PRUEBA
                        escritor.WriteLine("044|" + p.ToString() + "|PSINIVA|" + String.Format("{0:0.00}", double.Parse(lista[5].ToString().Trim())) + "|");
                        escritor.WriteLine("044|" + p.ToString() + "|SSINIVA|" + String.Format("{0:0.00}", double.Parse(lista[5].ToString().Trim())) + "|"); //texto que tenga subtotal SIN IVA IMPUESTOS ***EN PRUEBA
                        escritor.WriteLine("044|" + p.ToString() + "|SCONIVA|" + String.Format("{0:0.00}", monto) + "|"); // COSTO subtotal con IVA IMPUESTOS ***EN PRUE
                        escritor.WriteLine("044|" + p.ToString() + "|COD|" + lista[2].ToString().Trim().Substring(0, 1) + "|");

                        p++;
                    }

                }

            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {

        }

        protected void itemsGrid_rowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                if (chkSelectAll.Checked == true)
                {
                    ((CheckBox)e.Row.Cells[0].FindControl("check")).Checked = true;
                }
                string serieFact = (string)DataBinder.Eval(e.Row.DataItem, "serie");
                if (serieFact != null)
                {
                    if (serieFact.Equals("NCT"))
                    {
                        e.Row.Cells[4].ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            string cliente = lstCliente.SelectedValue;
            if (cliente.Equals("LIVERPOL") || cliente.Equals("ALMLIVER") || cliente.Equals("LIVERTIJ") || cliente.Equals("SFERALIV") || cliente.Equals("GLOBALIV") || cliente.Equals("SETRALIV") || cliente.Equals("FACTUMLV") || cliente.Equals("MERCANLV") || cliente.Equals("LIVERDED"))
            {
                //ve a adenda
                Response.Redirect("AdendaMasive.aspx?billto=" + lstCliente.SelectedValue);
            }
            else
            {
                //ve a detalle factura
                Response.Redirect("DetallesFacturasMasivas.aspx?billto=" + lstCliente.SelectedValue);
            }

        }
        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            //createGrid(queryGeneral);


            foreach (GridViewRow gvr in ItemsGrid.Rows)
            {

                if (chkSelectAll.Checked)
                {
                    ((CheckBox)gvr.FindControl("check")).Checked = true;
                }
                else
                {
                    ((CheckBox)gvr.FindControl("check")).Checked = false;
                }

            }

        }
    }
}