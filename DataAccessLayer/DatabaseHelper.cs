using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;

namespace DataAccessLayer
{
    public class DatabaseHelper
    {
        // When using Supabase's transaction pooler it's best to disable client-side pooling
        // to avoid double-pooling and stale/closed connections being reused. Also set
        // a TCP keepalive so idle connections are detected and reset by the driver.
        private static string connectionString =
            "Host=aws-0-ap-northeast-1.pooler.supabase.com;" +
            "Port=6543;" +
            "Database=postgres;" +
            "Username=postgres.uwgkhxuyahxywppgwvqc;" +
            "Password=966@$tudents966;" +
            "SSL Mode=Require;" +
            "Trust Server Certificate=true;" +
            "Pooling=false;" +
            "Keepalive=10";

        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(connectionString);
        }
    }
}
