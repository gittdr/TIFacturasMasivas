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
    public partial class Busqueda : System.Web.UI.Page
    {
        //Properties
        public SearchControler busquedaControler = new SearchControler();
        public GridViewControl gridControl = new GridViewControl();
        GridView ItemsGrid;
        public string queryGeneral, url;

        protected void Page_Load(object sender, EventArgs e)
        {
            string fact = Request.QueryString["factura"];

            string status = Request.QueryString["estatus"];
            txtInvoice.Text = busquedaControler.minInvoice(fact);
            //if (IsPostBack)
            //{
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
                    btnDescargar.Visible = false;
                }
            }
            //}
        }

        //Methods
        public string generaQuery()
        {
            lblError.Text = "";
            ArrayList empty = new ArrayList();

            int vacio = 0;
            int cont = 0;
            bool error = false;
            string query = "select * from vista_fe_generadas where ";

            if (!lstSerie.SelectedValue.Equals(""))
            {
                query += "serie = '" + lstSerie.SelectedValue + "'";

            }
            else { vacio = 1; cont++; empty.Add(1); }

            if (!lstCliente.SelectedValue.Equals("") && !lstSerie.SelectedValue.Equals(""))
            {
                query = query + " and idreceptor = '" + lstCliente.SelectedValue + "'";
            }
            else if (!lstCliente.SelectedValue.Equals(""))
            {
                query = query + "idreceptor = '" + lstCliente.SelectedValue + "'";
            }
            else { vacio = 2; cont++; empty.Add(2); }



            if (!Date1.Text.Equals(""))
            {
                System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
                dateInfo.ShortDatePattern = "MM/dd/yyyy";
                DateTime fecha1 = Convert.ToDateTime(Date1.Text, dateInfo);
                if (!Date2.Text.Equals(""))
                {
                    DateTime fecha2 = Convert.ToDateTime(Date2.Text, dateInfo);
                    if (fecha1.CompareTo(fecha2) == 1)
                    {
                        lblError.Text = "La Fecha inicial es mayor que la final";
                        error = true;
                    }
                    else
                    {
                        if (empty.Count == 2)
                        {
                            query += " fhemision between '" + fecha1.Year + "-" + fecha1.Month + "-" + fecha1.Day + " 00:00:00' and '" +
                                fecha2.Year + "-" + fecha2.Month + "-" + fecha2.Day + " 00:00:00'";
                        }
                        else
                        {
                            query += " and fhemision between '" + fecha1.Year + "-" + fecha1.Month + "-" + fecha1.Day + " 00:00:00' and '" +
                                fecha2.Year + "-" + fecha2.Month + "-" + fecha2.Day + " 00:00:00'";
                        }
                    }
                }
                else
                {
                    if (fecha1.CompareTo(DateTime.Today) == 1)
                    {
                        lblError.Text = "La Fecha inicial es mayor que la fecha actual";
                        error = true;
                    }
                    else
                    {
                        if (empty.Count == 2)
                        {
                            query += " fhemision between '" + fecha1.Year + "-" + fecha1.Month + "-" + fecha1.Day + " 00:00:00' and getdate()";
                        }
                        else
                        {
                            query += "  and fhemision between '" + fecha1.Year + "-" + fecha1.Month + "-" + fecha1.Day + " 00:00:00' and getdate()";
                        }
                    }
                }
            }
            else { vacio = 3; cont++; empty.Add(3); }

            if (Date1.Text.Equals("") && !Date2.Text.Equals(""))
            {
                lblError.Text = "Debe seleccionar la fecha inicial";
                error = true;
            }
            if (Date2.Text.Equals("")) { vacio = 4; cont++; empty.Add(4); }


            if (!txtInvoice.Text.Equals(""))
            {
                if (empty.Count == 4)
                {
                    query += "  invoice = '" + txtInvoice.Text + "'";
                }
                else
                {
                    query += " and invoice = '" + txtInvoice.Text + "'";
                }
            }
            else
            {
                vacio = 5;
                cont++;
                empty.Add(5);
            }

            if (!txtMaster.Text.Equals(""))
            {
                if (empty.Count == 5)
                {
                    query += "  nmaster = '" + txtMaster.Text + "'";
                }
                else
                {
                    query += " and nmaster = '" + txtMaster.Text + "'";
                }
            }
            else { vacio = 6; cont++; empty.Add(6); }


            if (!lstSistema.SelectedValue.Equals(""))
            {
                if (empty.Count == 6) { query += "  provfact = '" + lstSistema.SelectedValue + "'"; }
                else { query += " and provfact = '" + lstSistema.SelectedValue + "'"; }
            }
            else { vacio = 7; cont++; empty.Add(7); }

            if (!lstElaborada.SelectedValue.Equals(""))
            {
                if (empty.Count == 7) { query += "  hechapor = '" + lstElaborada.SelectedValue + "'"; }
                else { query += " and hechapor = '" + lstElaborada.SelectedValue + "'"; }
            }
            else { vacio = 8; cont++; empty.Add(8); }


            if (!txtReferencia.Text.Equals(""))
            {
                if (empty.Count == 8) { query += " bandera = '" + txtReferencia.Text + "'"; }
                else { query += " and bandera = '" + txtReferencia.Text + "'"; }
            }
            else { vacio = 9; cont++; empty.Add(9); }

            if (!txtOrden1.Text.Equals(""))
            {
                if (empty.Count == 9) { query += " orden = '" + txtOrden1.Text + "'"; }
                else { query += " and orden = '" + txtOrden1.Text + "'"; }
            }
            else { vacio = 10; cont++; empty.Add(10); }


            query += " order by invoice";

            if (empty.Count == 10)
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
            ItemsGrid.Columns.Add(gridControl.CreateBoundColumn("invoice", "Invoice", 80));
            ItemsGrid.Columns.Add(gridControl.CreateBoundColumn("orden", "Orden", 80));
            ItemsGrid.Columns.Add(gridControl.CreateBoundColumn("idreceptor", "Cliente", 80));
            ItemsGrid.Columns.Add(gridControl.CreateBoundColumn("total", "Cantidad", 80));
            ItemsGrid.Columns.Add(gridControl.CreateBoundColumn("moneda", "Moneda", 40));
            ItemsGrid.Columns.Add(gridControl.CreateBoundColumn("fhemision", "Fecha", 120));
            ItemsGrid.Columns.Add(gridControl.createHyperLink("rutapdf", "PDF", 40));
            ItemsGrid.Columns.Add(gridControl.createHyperLink("rutaxml", "XML", 40));
            ItemsGrid.Columns.Add(gridControl.createHyperLink("imaging", "Imaging", 40));
            DataTable datos = busquedaControler.search(query);
            ItemsGrid.DataSource = datos;
            ItemsGrid.DataBind();

            gridPlace.Controls.Add(ItemsGrid);
            chkSelectAll.Text = "Seleccionar: " + datos.Rows.Count + "";
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            //string queryResultante = generaQuery();
            //createGrid(queryResultante);
            //System.Threading.Thread.Sleep(3000);
        }

        protected void btnDescargar_Click(object sender, EventArgs e)
        {
            string invoice = "";
            foreach (GridViewRow row in ItemsGrid.Rows)
            {
                CheckBox accion = (CheckBox)row.Cells[0].FindControl("check");
                if (accion.Checked)
                {
                    string key = accion.ToolTip;
                    invoice += key + "-" + row.Cells[2].Text.ToString() + ",";
                }
            }
            invoice += "";

            //url = "http://173.205.254.88/xsamanager/DownloadExpedidosBloqueServlet?rfcEmisor=TTR931201KJ6&key=c3a0578bbe5801cecc89d30b48052652&serie-folio=" + invoice + "&tipo=PDF,XML";
            url = "https://" + System.Web.Configuration.WebConfigurationManager.AppSettings["servidord"] + "/xsamanager/DownloadExpedidosBloqueServlet?rfcEmisor=TTR931201KJ6&key=" + System.Web.Configuration.WebConfigurationManager.AppSettings["llave"] + "&serie-folio=" + invoice + "&tipo=PDF,XML";

            Response.Write("<script>window.open('" + url + "','_blank');</script>");

            //Response.Redirect(url);
            UpdatePanel2.Update();

        }

        protected void itemsGrid_rowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                if (chkSelectAll.Checked == true)
                {
                    ((CheckBox)e.Row.Cells[0].FindControl("check")).Checked = true;
                }

                string dato = (string)DataBinder.Eval(e.Row.DataItem, "provfact");
                if (dato != null)
                {
                    if (dato.Equals("WF"))
                    {
                        ((CheckBox)e.Row.Cells[0].FindControl("check")).Visible = false;
                    }
                }

                string serie = (string)DataBinder.Eval(e.Row.DataItem, "serie");
                if (serie != null)
                {
                    ((CheckBox)e.Row.Cells[0].FindControl("check")).ToolTip = serie;

                    if (serie.Equals("NCT"))
                    {
                        e.Row.Cells[5].ForeColor = System.Drawing.Color.Red;
                    }
                }

                if (e.Row.DataItem != null)
                {

                    string pdf = (string)DataBinder.Eval(e.Row.DataItem, "rutapdf");
                    if (pdf.Equals("CANCELADA"))
                    {

                        e.Row.Cells[9].Controls.Clear();
                        e.Row.Cells[9].ForeColor = System.Drawing.Color.Red;
                        e.Row.Cells[9].Text = "CANCELADA";
                        e.Row.Cells[8].ForeColor = System.Drawing.Color.Red;
                        e.Row.Cells[8].Controls.Clear();
                        e.Row.Cells[8].Text = "CANCELADA";

                    }
                }
            }

        }
        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            createGrid(queryGeneral);
        }

    }
}