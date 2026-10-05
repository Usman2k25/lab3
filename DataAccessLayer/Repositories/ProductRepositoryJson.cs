using System.Text.Json;
using DataAccessLayer.Interfaces;
using Shared.Models;

namespace DataAccessLayer.Repositories;

public class ProductRepositoryJson : IProductRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    private static string FilePath => Path.Combine(AppContext.BaseDirectory, "Data", "products.json");

    public async Task<List<ProductModel>> GetAllAsync() => await ReadProductsAsync();

    public async Task<ProductModel?> GetByIdAsync(Guid id)
    {
        var products = await ReadProductsAsync();
        return products.FirstOrDefault(product => product.Id == id);
    }

    public async Task AddAsync(ProductModel product)
    {
        var products = await ReadProductsAsync();
        products.Add(product);
        await WriteProductsAsync(products);
    }

    public async Task UpdateAsync(ProductModel product)
    {
        var products = await ReadProductsAsync();
        var existing = products.FirstOrDefault(item => item.Id == product.Id);
        if (existing is null)
        {
            throw new KeyNotFoundException($"Product with ID '{product.Id}' was not found.");
        }

        existing.Name = product.Name;
        existing.Price = product.Price;
        existing.Category = product.Category;
        existing.Description = product.Description;
        existing.IsActive = product.IsActive;

        await WriteProductsAsync(products);
    }

    public async Task DeleteAsync(Guid id)
    {
        var products = await ReadProductsAsync();
        var product = products.FirstOrDefault(item => item.Id == id);
        if (product is null)
        {
            return;
        }

        products.Remove(product);
        await WriteProductsAsync(products);
    }

    public async Task<List<ProductModel>> SearchAsync(string keyword)
    {
        var products = await ReadProductsAsync();
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return products;
        }

        keyword = keyword.Trim();
        return products.Where(product =>
            (product.Name?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (product.Category?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (product.Description?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false)).ToList();
    }

    private static async Task<List<ProductModel>> ReadProductsAsync()
    {
        if (!File.Exists(FilePath))
        {
            return [];
        }

        var json = await File.ReadAllTextAsync(FilePath);
        if (string.IsNullOrWhiteSpace(json))
        {
            return [];
        }

        var products = JsonSerializer.Deserialize<List<ProductModel>>(json, JsonOptions);
        return products ?? [];
    }

    private static async Task WriteProductsAsync(List<ProductModel> products)
    {
        var directory = Path.GetDirectoryName(FilePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var json = JsonSerializer.Serialize(products, JsonOptions);
        await File.WriteAllTextAsync(FilePath, json);
    }
}
