using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace FactMasiva.Models
{
    public sealed class Connection
    {
        private const string database = "TDR";
        public string connectionString;

        public string ConnectionString
        {
            get
            {
                return this.connectionString;
            }
            private set
            {
                this.connectionString = value;
            }
        }

        public Connection()
        {
            this.ConnectionString = WebConfigurationManager.ConnectionStrings["TDR"].ConnectionString;
        }
    }
}