using System;
using System.Collections.Generic;
using Npgsql;
using Shared.Models;

namespace DataAccessLayer
{
    public class CRUD
    {
        public static List<ProductModel> GetAllData()
        {
            List<ProductModel> products = new List<ProductModel>();

            string Query = "SELECT * FROM \"Products\"";
            NpgsqlConnection connection = DatabaseHelper.GetConnection();
            connection.Open();

            NpgsqlCommand cmd = new NpgsqlCommand(Query, connection);
            NpgsqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ProductModel product = new ProductModel();
                product.Id = reader.GetGuid(0);
                product.Name = reader.GetString(1);
                product.Price = Convert.ToInt32(reader["Price"]);
                product.Category = reader.GetString(3);
                product.Description = reader.IsDBNull(4) ? null : reader.GetString(4);
                product.IsActive = reader.GetBoolean(5);

                products.Add(product);
            }

            reader.Close();
            connection.Close();

            return products;
        }
    }
}
