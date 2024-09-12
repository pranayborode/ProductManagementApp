using ProductManagementApp.Models;

namespace ProductManagementApp.Repository.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllProducts();
        int GetTotalProductCount();
        Product GetProductById(int id);
        int AddProduct(Product product);
        int EditProduct(Product product);
        int DeleteProduct(int id);
    }
}
