using System;
using System.Collections.Generic;
using System.Text;

namespace TeaBagBot.DataAccess
{
    public interface IMongoDbSettings
    {
        string DatabaseName { get; set; }
        string ConnectionString { get; set; }
    }
}
