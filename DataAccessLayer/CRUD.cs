
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

            string query = @"SELECT ""Id"", ""Name"", ""Price"", ""Category"",
                                    ""Description"", ""IsActive""
                             FROM ""Products""";

            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    int idIndex = reader.GetOrdinal("Id");
                    int nameIndex = reader.GetOrdinal("Name");
                    int priceIndex = reader.GetOrdinal("Price");
                    int categoryIndex = reader.GetOrdinal("Category");
                    int descriptionIndex = reader.GetOrdinal("Description");
                    int activeIndex = reader.GetOrdinal("IsActive");

                    while (reader.Read())
                    {
                        ProductModel product = new ProductModel();

                        product.Id = reader.GetGuid(idIndex);
                        product.Name = reader.GetString(nameIndex);
                        product.Price = reader.IsDBNull(priceIndex)
                            ? 0m : reader.GetDecimal(priceIndex);
                        product.Category = reader.IsDBNull(categoryIndex)
                            ? string.Empty : reader.GetString(categoryIndex);
                        product.Description = reader.IsDBNull(descriptionIndex)
                            ? null : reader.GetString(descriptionIndex);
                        product.IsActive = !reader.IsDBNull(activeIndex)
                            && reader.GetBoolean(activeIndex);

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

                string query = @"INSERT INTO ""Products""
                    (""Id"", ""Name"", ""Price"", ""Category"", ""Description"", ""IsActive"")
                    VALUES (@id, @name, @price, @category, @description, @active)";

                using (var cmd = new NpgsqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", p.Id);
                    cmd.Parameters.AddWithValue("@name", p.Name);
                    cmd.Parameters.AddWithValue("@price", p.Price);
                    cmd.Parameters.AddWithValue("@category", p.Category);
                    cmd.Parameters.AddWithValue("@description",
                        (object?)p.Description ?? DBNull.Value);
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

                string query = @"UPDATE ""Products""
                    SET ""Name"" = @name,
                        ""Price"" = @price,
                        ""Category"" = @category,
                        ""Description"" = @description,
                        ""IsActive"" = @active
                    WHERE ""Id"" = @id";

                using (var cmd = new NpgsqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", p.Id);
                    cmd.Parameters.AddWithValue("@name", p.Name);
                    cmd.Parameters.AddWithValue("@price", p.Price);
                    cmd.Parameters.AddWithValue("@category", p.Category);
                    cmd.Parameters.AddWithValue("@description",
                        (object?)p.Description ?? DBNull.Value);
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

                string query = @"DELETE FROM ""Products"" WHERE ""Id"" = @id";

                using (var cmd = new NpgsqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
