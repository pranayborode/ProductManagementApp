using ProductManagementApp.Data;
using ProductManagementApp.Models;
using ProductManagementApp.Repository.Interfaces;

namespace ProductManagementApp.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext _context)
        {
            this._context = _context;
        }

        public int AddProduct(Product product)
        {
            _context.Add(product);
            int result = _context.SaveChanges();
            return result;
        }

        public int DeleteProduct(int id)
        {
            int result = 0;
            var product = _context.Products.Find(id);

            if (product != null)
            {
                _context.Remove(product);
                result = _context.SaveChanges();
                return result;
            }
            return result;
        }

        public int EditProduct(Product product)
        {
            var _product = _context.Products.Find(product.Id);

            if (_product != null)
            {
                _product.Name = product.Name;
                _product.Description = product.Description;
                _product.Price = product.Price;
                int result = _context.SaveChanges();
                return result;
            }
            else
            {
                return 0;
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public int GetTotalProductCount()
        {
            return _context.Products.Count();
        }

        public Product GetProductById(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                return product;
            }
            else
            {
                return null;
            }
        }
    }
}
