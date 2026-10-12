using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;

namespace DataAccessLayer
{
    public class DatabaseHelper
    {
        private static string connectionString =
            "Host=aws-0-ap-northeast-1.pooler.supabase.com;" +
            "Port=6543;" +
            "Database=postgres;" +
            "Username=postgres.uwgkhxuyahxywppgwvqc;" +
            "Password=966@$tudents966;" +
            "SSL Mode=Require;" +
            "Trust Server Certificate=true";

        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(connectionString);
        }
    }
}
