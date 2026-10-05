using Business.Interfaces;
using DataAccessLayer.Interfaces;
using Shared.Models;

namespace Business.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public Task<List<ProductModel>> GetAllAsync() => _repository.GetAllAsync();

    public Task<ProductModel?> GetByIdAsync(Guid id) => _repository.GetByIdAsync(id);

    public async Task CreateAsync(ProductModel product)
    {
        ValidateProduct(product);
        product.Id = Guid.NewGuid();
        await _repository.AddAsync(product);
    }

    public async Task UpdateAsync(ProductModel product)
    {
        ValidateProduct(product);

        var existing = await _repository.GetByIdAsync(product.Id);
        if (existing is null)
        {
            throw new KeyNotFoundException("The selected product was not found.");
        }

        await _repository.UpdateAsync(product);
    }

    public Task DeleteAsync(Guid id) => _repository.DeleteAsync(id);

    public Task<List<ProductModel>> SearchAsync(string keyword) => _repository.SearchAsync(keyword);

    private static void ValidateProduct(ProductModel product)
    {
        if (product is null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        product.Name = product.Name?.Trim() ?? string.Empty;
        product.Category = product.Category?.Trim() ?? string.Empty;
        product.Description = product.Description?.Trim();

        if (string.IsNullOrWhiteSpace(product.Name))
        {
            throw new ArgumentException("Name is required.");
        }

        if (product.Name.Length < 3)
        {
            throw new ArgumentException("Name must contain at least 3 characters.");
        }

        if (product.Price <= 0)
        {
            throw new ArgumentException("Price must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(product.Category))
        {
            throw new ArgumentException("Category is required.");
        }

        if (product.Description is not null && product.Description.Length > 500)
        {
            throw new ArgumentException("Description cannot exceed 500 characters.");
        }
    }
}
