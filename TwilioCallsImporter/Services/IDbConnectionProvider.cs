﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace TwilioCallsImporter.Services
{
    interface IDbConnectionProvider
    {
        IDbConnection Connection { get; }
    }
}