using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace TwilioCallsImporter.Services
{
    public class DbConnectionProvider : IDbConnectionProvider
    {
        private string connectionString;
        public DbConnectionProvider(IConfiguration config)
        {
            connectionString = config.GetConnectionString("DefaultConnection");
        }
        public IDbConnection Connection
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
