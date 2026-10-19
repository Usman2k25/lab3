
using Npgsql;
using Shared.Models;
using System;
using System.Collections.Generic;

namespace DataAccessLayer
{
    public class CRUD
    {
        public static List<ProductModel> GetAllData()
        {
            List<ProductModel> products = new List<ProductModel>();

            string Query = "SELECT * FROM \"Products\"";
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand(Query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("Database query executed successfully");

                    while (reader.Read())
                    {
                        ProductModel product = new ProductModel();
                        product.Id = reader.GetGuid(0);
                        product.Name = reader.GetString(1);
                        // Price is stored as numeric/decimal in DB; read as decimal to match model
                        product.Price = reader.IsDBNull(2) ? 0m : reader.GetDecimal(2);
                        product.Category = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                        product.Description = reader.IsDBNull(4) ? null : reader.GetString(4);
                        product.IsActive = !reader.IsDBNull(5) && reader.GetBoolean(5);

                        products.Add(product);
                    }
                }
            }

            return products;
        }

        public static void CreateData(ProductModel p)
        {
            using (var con = DatabaseHelper.GetConnection())
            {
                con.Open();
            string q = @"INSERT INTO ""Products""
(""Id"", ""Name"", ""Price"", ""Category"", ""Description"", ""IsActive"")
VALUES (@id, @name, @price, @category, @description, @active)";

                using (var cmd = new NpgsqlCommand(q, con))
                {
                    cmd.Parameters.AddWithValue("@id", p.Id);
                    cmd.Parameters.AddWithValue("@name", p.Name);
                    cmd.Parameters.AddWithValue("@price", p.Price);
                    cmd.Parameters.AddWithValue("@category", p.Category);
                    cmd.Parameters.AddWithValue("@description", (object?)p.Description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@active", p.IsActive);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateData(ProductModel p)
        {
            using (var con = DatabaseHelper.GetConnection())
            {
                con.Open();
                // Update all editable fields including Description and Category
                string q = "UPDATE \"Products\" SET \"Name\"=@name, \"Price\"=@price, \"Category\"=@category, \"Description\"=@description, \"IsActive\"=@active WHERE \"Id\"=@id";

                using (var cmd = new NpgsqlCommand(q, con))
                {
                    cmd.Parameters.AddWithValue("@id", p.Id);
                    cmd.Parameters.AddWithValue("@name", p.Name);
                    cmd.Parameters.AddWithValue("@price", p.Price);
                    cmd.Parameters.AddWithValue("@category", (object?)p.Category ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@description", (object?)p.Description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@active", p.IsActive);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteData(Guid id)
        {
            using (var con = DatabaseHelper.GetConnection())
            {
                con.Open();

                string q = "DELETE FROM \"Products\" WHERE \"Id\"=@id";

                using (var cmd = new NpgsqlCommand(q, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
