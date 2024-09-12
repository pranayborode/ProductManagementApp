using ProductManagementApp.Models;

namespace ProductManagementApp.Services.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();

        int GetTotalProductCount();
        Product GetProductById(int id);
        int AddProduct(Product product);
        int EditProduct(Product product);
        int DeleteProduct(int id);
    }
}
