using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TwilioCallsImporter.Data;

namespace TwilioCallsImporter.Services
{
    public class CallData 
        : ICallData
    {
        private string _dbConn;
        public CallData(IConfiguration dbConn)
        {
            _dbConn = dbConn.GetConnectionString("DefaultConnection");
        }
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_dbConn);
            }
        }
        public void Add(IEnumerable<Call> callsToImport)
        {
            string sQuery = "INSERT INTO dbo.Calls (StartTime, EndTime, IpAddress, TrunkId, CallId, SourceNumber, DestinationNumber, Duration, CallRate, CallCost, InsertDate)"
                                          + "VALUES(@StartTime, @EndTime, @IpAddress, @TrunkId, @CallId, @SourceNumber, @DestinationNumber, @Duration, @CallRate, @CallCost, getdate())";
            using (IDbConnection dbConn = Connection)
            {
                dbConn.Open();
                dbConn.Execute(sQuery, callsToImport);
            }
        }
    }
}
