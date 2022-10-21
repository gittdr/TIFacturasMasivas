using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace FactMasiva.Models
{
    public class ModelFact
    {
        private const string facturas = "select folio as Folio,fhemision as Fecha, nombrecliente as Cliente, idreceptor from VISTA_fe_Header";
        private const string facturasClientes = "select distinct  idreceptor from vista_fe_header";
        private const string facturasPorProcesar = "select * from VISTA_fe_Header where  idreceptor not in ('liverpol','GLOBALIV','LIVERTIJ','SFERALIV','FACTUMLV')";
        private const string facturasPorProcesarLivepool = "select * from VISTA_fe_Header where idreceptor in ('liverpol','GLOBALIV','LIVERTIJ','SFERALIV','FACTUMLV')";
        private const string facturaAdendaReferencia = "select ref_number, ref_type from referencenumber where ord_hdrnumber = @orden and (ref_type = 'ADEHOJ' or ref_type = 'ADEPED' or ref_type = 'LPROV')";
        private const string datosFactura = "select * from VISTA_fe_Header where folio = @factura";
        private const string detalle = "select * from vista_Fe_detail where folio = @factura";
        private const string detalle33 = "select * from vista_Fe_detail where folio = @factura";
        private const string invoice = "select ivh_invoicestatus,ivh_mbnumber,ivh_ref_number from invoiceheader where ivh_invoicenumber = @factura";
        private const string updateTrans = "update invoiceheader set ivh_ref_number = @idComprobante where ivh_invoicenumber = @fact";
        private const string updateTransMaster = "update invoiceheader set ivh_ref_number = @idComprobante where ivh_mbnumber  = @master";
        private const string insertaGeneradas = "insert into VISTA_Fe_generadas (nmaster,invoice,serie,idreceptor,fhemision,total,moneda,rutapdf,rutaxml,imaging,bandera,\r\n            provfact,status,ultinvoice,hechapor,orden,rfc) values (@master,@factura,@serie,@idreceptor,@fhemision,@total,@moneda,\r\n            @rutapdf,@rutaxml,@imaging,@bandera,@provfactura,@status,@ultinvoice,@hechapor,@orden,@rfc)";
        private const string parmFactura = "( select case when(select ivh_mbnumber from invoiceheader with (nolock) where ivh_invoicenumber = @factura) = 0 then @factura else (select max(ivh_invoicenumber) from invoiceheader with (nolock) where ivh_mbnumber = (select ivh_mbnumber from invoiceheader with (nolock) where ivh_mbnumber != 0 and ivh_invoicenumber = @factura)) end)";
        private const string masterFactura = "select * from vista_fe_header where ultinvoice = @parmFact";
        private const string minInvoice = "select invoice from vista_fe_header where ultinvoice = @parmFact";
        private const string P_fact = "@factura";
        private const string P_idComprobante = "@idComprobante";
        private const string P_master = "@master";
        private const string P_fact2 = "@fact";
        private const string P_pfact = "@parmFact";
        private const string P_invoice = "@lastInvoice";
        private const string P_serie = "@serie";
        private const string P_idReceptor = "@idreceptor";
        private const string P_fhemision = "@fhemision";
        private const string P_total = "@total";
        private const string P_moneda = "@moneda";
        private const string P_rutaPdf = "@rutapdf";
        private const string P_rutaXML = "@rutaxml";
        private const string P_imaging = "@imaging";
        private const string P_bandera = "@bandera";
        private const string P_provFact = "@provfactura";
        private const string P_status = "@status";
        private const string P_ultinvoice = "@ultinvoice";
        private const string P_hechapor = "@hechapor";
        private const string P_orden = "@orden";
        private const string P_rfc = "@rfc";
        private string _ConnectionString;

        public ModelFact()
        {
            this._ConnectionString = new Connection().connectionString;
        }

        public DataTable getFacturas()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select folio as Folio,fhemision as Fecha, nombrecliente as Cliente, idreceptor from VISTA_fe_Header", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 1000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }

        public DataTable getFacturasClientes()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select distinct  idreceptor from vista_fe_header", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 1000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }

        public DataTable getFacturasPorProcesar(string billto)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select * from VISTA_fe_Header where idreceptor = @idreceptor and  idreceptor not in ('liverpol','GLOBALIV','LIVERTIJ','SFERALIV','FACTUMLV','MERCANLV')", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 1000;
                    selectCommand.Parameters.AddWithValue("@idreceptor", (object)billto);
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }

        public DataTable getFacturasPorProcesarLivepool()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select * from VISTA_fe_Header where idreceptor in ('liverpol','GLOBALIV','LIVERTIJ','SFERALIV','FACTUMLV','MERCANLV')", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 1000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }

        public DataTable getFacturaAdendaReferencia(string Ord)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select ref_number, ref_type from referencenumber where ord_hdrnumber = @orden and (ref_type = 'ADEHOJ' or ref_type = 'ADEPED' or ref_type = 'LPROV')", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 1000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            selectCommand.Parameters.AddWithValue("@orden", (object)Ord);
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }

        public DataTable getDatosFacturas(string fact)
        {
            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = new DataTable();
            string str = "";
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("( select case when(select ivh_mbnumber from invoiceheader with (nolock) where ivh_invoicenumber = @factura) = 0 then @factura else (select max(ivh_invoicenumber) from invoiceheader with (nolock) where ivh_mbnumber = (select ivh_mbnumber from invoiceheader with (nolock) where ivh_mbnumber != 0 and ivh_invoicenumber = @factura)) end)", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.Parameters.AddWithValue("@factura", (object)fact);
                    selectCommand.CommandTimeout = 1000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable1);
                            selectCommand.Connection.Close();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                            selectCommand.Connection.Close();
                        }
                    }
                }
                if (dataTable1.Rows.Count != 0 && dataTable1 != null)
                    str = dataTable1.Rows[0].ItemArray[0].ToString();
                using (SqlCommand selectCommand = new SqlCommand("select * from vista_fe_header where ultinvoice = @parmFact", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.Parameters.AddWithValue("@parmFact", (object)str);
                    selectCommand.CommandTimeout = 1000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable2);
                            selectCommand.Connection.Close();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                            selectCommand.Connection.Close();
                        }
                    }
                }
            }
            return dataTable2;
        }

        public DataTable getDetalle(string p)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select * from vista_Fe_detail where folio = @factura", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.Parameters.AddWithValue("@factura", (object)p);
                    selectCommand.CommandTimeout = 1000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                            selectCommand.Connection.Close();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }

        public DataTable getDetalle33(string p)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select * from vista_Fe_detail where folio = @factura", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.Parameters.AddWithValue("@factura", (object)p);
                    selectCommand.CommandTimeout = 1000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                            selectCommand.Connection.Close();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }

        public DataTable getInvoice(string fact)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select ivh_invoicestatus,ivh_mbnumber,ivh_ref_number from invoiceheader where ivh_invoicenumber = @factura", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.Parameters.AddWithValue("@factura", (object)fact);
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                            selectCommand.Connection.Close();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }

        public void updateFactura(string fact, string comprobante, int mbnumber)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand(mbnumber != 0 ? "update invoiceheader set ivh_ref_number = @idComprobante where ivh_mbnumber  = @master" : "update invoiceheader set ivh_ref_number = @idComprobante where ivh_invoicenumber = @fact", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.Parameters.AddWithValue("@idComprobante", (object)comprobante);
                    if (mbnumber == 0)
                        selectCommand.Parameters.AddWithValue("@fact", (object)fact);
                    else
                        selectCommand.Parameters.AddWithValue("@master", (object)mbnumber);
                    selectCommand.Parameters.AddWithValue("@factura", (object)fact);
                    using (new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            selectCommand.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
        }

        public DataTable getLastInvoice(string ivh)
        {
            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = new DataTable();
            string str = "";
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("( select case when(select ivh_mbnumber from invoiceheader with (nolock) where ivh_invoicenumber = @factura) = 0 then @factura else (select max(ivh_invoicenumber) from invoiceheader with (nolock) where ivh_mbnumber = (select ivh_mbnumber from invoiceheader with (nolock) where ivh_mbnumber != 0 and ivh_invoicenumber = @factura)) end)", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.Parameters.AddWithValue("@factura", (object)ivh);
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable1);
                            selectCommand.Connection.Close();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                            selectCommand.Connection.Close();
                        }
                    }
                }
                if (dataTable1.Rows.Count != 0 && dataTable1 != null)
                    str = dataTable1.Rows[0].ItemArray[0].ToString();
                using (SqlCommand selectCommand = new SqlCommand("select invoice from vista_fe_header where ultinvoice = @parmFact", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.Parameters.AddWithValue("@parmFact", (object)str);
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable2);
                            selectCommand.Connection.Close();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                            selectCommand.Connection.Close();
                        }
                    }
                }
            }
            return dataTable2;
        }

        public void actualizaGeneradas(
          string master,
          string fact,
          string serie,
          string idReceptor,
          string fhemision,
          string total,
          string moneda,
          string rutaPdf,
          string rutaXML,
          string imaging,
          string bandera,
          string provfact,
          string status,
          string ultinvoice,
          string hechapor,
          string orden,
          string rfc)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("insert into VISTA_Fe_generadas (nmaster,invoice,serie,idreceptor,fhemision,total,moneda,rutapdf,rutaxml,imaging,bandera,\r\n            provfact,status,ultinvoice,hechapor,orden,rfc) values (@master,@factura,@serie,@idreceptor,@fhemision,@total,@moneda,\r\n            @rutapdf,@rutaxml,@imaging,@bandera,@provfactura,@status,@ultinvoice,@hechapor,@orden,@rfc)", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.Parameters.AddWithValue("@master", (object)master);
                    selectCommand.Parameters.AddWithValue("@factura", (object)fact);
                    selectCommand.Parameters.AddWithValue("@serie", (object)serie);
                    selectCommand.Parameters.AddWithValue("@idreceptor", (object)idReceptor);
                    selectCommand.Parameters.AddWithValue("@fhemision", (object)fhemision);
                    selectCommand.Parameters.AddWithValue("@total", (object)total);
                    selectCommand.Parameters.AddWithValue("@moneda", (object)moneda);
                    selectCommand.Parameters.AddWithValue("@rutapdf", (object)rutaPdf);
                    selectCommand.Parameters.AddWithValue("@rutaxml", (object)rutaXML);
                    selectCommand.Parameters.AddWithValue("@imaging", (object)imaging);
                    selectCommand.Parameters.AddWithValue("@bandera", (object)bandera);
                    selectCommand.Parameters.AddWithValue("@provfactura", (object)provfact);
                    selectCommand.Parameters.AddWithValue("@status", (object)status);
                    selectCommand.Parameters.AddWithValue("@ultinvoice", (object)ultinvoice);
                    selectCommand.Parameters.AddWithValue("@hechapor", (object)hechapor);
                    selectCommand.Parameters.AddWithValue("@orden", (object)orden);
                    selectCommand.Parameters.AddWithValue("@rfc", (object)rfc);
                    using (new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            selectCommand.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
        }
    }
}