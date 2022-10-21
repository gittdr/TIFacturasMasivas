using FactMasiva.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FactMasiva.Controllers
{
    public class SearchControler
    {
        public ModelSearch modelFact = new ModelSearch();

        public DataTable clientes()
        {
            return this.modelFact.getClientes();
        }

        public DataTable search(string query)
        {
            return this.modelFact.busqueda(query);
        }

        public DataTable clientesLad()
        {
            return this.modelFact.getClientesLad();
        }

        public string minInvoice(string ivh)
        {
            DataTable lastInvoice = this.modelFact.getLastInvoice(ivh);
            if (lastInvoice.Rows.Count != 0 && lastInvoice != null)
                return lastInvoice.Rows[0].ItemArray[0].ToString();
            return "";
        }
    }
}