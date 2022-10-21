using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FactMasiva.Models
{
    public class ModelSearch
    {
        private const string clientes = "select distinct  idreceptor from vista_fe_generadas";
        private const string clientesLad = "select distinct cliente from vista_feproy_generadas";
        private const string minInvoice = "select invoice from vista_fe_generadas where ultinvoice = @lastInvoice";
        private const string P_invoice = "@lastInvoice";
        private string _ConnectionString;

        public ModelSearch()
        {
            this._ConnectionString = new Connection().connectionString;
        }

        public DataTable getClientes()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select distinct  idreceptor from vista_fe_generadas", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
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

        public DataTable busqueda(string query)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand(query, connection))
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

        public DataTable getClientesLad()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select distinct cliente from vista_feproy_generadas", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
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

        public DataTable getLastInvoice(string ivh)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select invoice from vista_fe_generadas where ultinvoice = @lastInvoice", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.Parameters.AddWithValue("@lastInvoice", (object)ivh);
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
    }
}