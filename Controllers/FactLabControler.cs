using FactMasiva.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FactMasiva.Controllers
{
    public class FactLabControler
    {
        public ModelFact modelFact = new ModelFact();

        public DataTable facturas()
        {
            return this.modelFact.getFacturas();
        }

        public DataTable facturasClientes()
        {
            return this.modelFact.getFacturasClientes();
        }

        public DataTable FacturasPorProcesar(string billto)
        {
            return this.modelFact.getFacturasPorProcesar(billto);
        }

        public DataTable FacturasPorProcesarLiverpool()
        {
            return this.modelFact.getFacturasPorProcesarLivepool();
        }

        public DataTable detalleFacturas(string fact)
        {
            return this.modelFact.getDatosFacturas(fact);
        }

        public DataTable FacturaFacturaAdendaReferencia(string orden)
        {
            return this.modelFact.getFacturaAdendaReferencia(orden);
        }

        public DataTable detalle(string p)
        {
            return this.modelFact.getDetalle(p);
        }

        public DataTable detalle33p(string p)
        {
            return this.modelFact.getDetalle33(p);
        }

        public DataTable estatus(string fact)
        {
            return this.modelFact.getInvoice(fact);
        }

        public void actualizaFactura(string fact, string comprobante, int mbnumber)
        {
            this.modelFact.updateFactura(fact, comprobante, mbnumber);
        }

        public string minInvoice(string ivh)
        {
            DataTable lastInvoice = this.modelFact.getLastInvoice(ivh);
            if (lastInvoice.Rows.Count != 0 && lastInvoice != null)
                return lastInvoice.Rows[0].ItemArray[0].ToString();
            return "";
        }

        public string facturaValida(string ivh)
        {
            string str = this.minInvoice(ivh);
            if (str.Equals(""))
                return ivh;
            return str;
        }

        public void generadas(
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
            this.modelFact.actualizaGeneradas(master, fact, serie, idReceptor, fhemision, total, moneda, rutaPdf, rutaXML, imaging, bandera, provfact, status, ultinvoice, hechapor, orden, rfc);
        }
    }
}