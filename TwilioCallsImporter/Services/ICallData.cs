using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwilioCallsImporter.Data;

namespace TwilioCallsImporter.Services
{
    public interface ICallData
    {
        void Add(IEnumerable<Call> callToImport);
    }
}
