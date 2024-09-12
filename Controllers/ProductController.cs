using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagementApp.Models;
using ProductManagementApp.Services.Interfaces;
using System.Drawing.Printing;

namespace ProductManagementApp.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private const int PageSize = 10;
        public ProductController(IProductService _productService)
        {
            this._productService = _productService;
        }

        public ActionResult Index(int page = 1)
        {
            var products = _productService.GetAllProducts(); 

            var paginatedProducts = products
                .Skip((page - 1) * PageSize)
                .Take(PageSize) 
                .ToList(); 

            var totalProducts = products.Count();

            var model = new ProductListViewModel
            {
                Products = paginatedProducts,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalProducts / (double)PageSize)
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_ProductListPartial", model);
            }

            return View(model);
        }


        // GET: Create Product
        public ActionResult Create()
        {
            return PartialView("_CreateProductPartial");
        }

        // POST: Create Product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                int result = _productService.AddProduct(product);
                if (result >= 1)
                {
                    TempData["SuccessMessage"] = "Product created successfully.";
                    return Json(new { success = true });
                }
            }
            TempData["ErrorMessage"] = "Failed to create product.";
            return Json(new { success = false });
        }

        // GET: Edit Product
        public ActionResult Edit(int id)
        {
            var product = _productService.GetProductById(id);
            return PartialView("_EditProductPartial", product);
        }

        // POST: Edit Product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                int result = _productService.EditProduct(product);
                if (result >= 1)
                {
                    TempData["SuccessMessage"] = "Product updated successfully.";
                    return Json(new { success = true });
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);
            var errorMessages = errors.Select(e => e.ErrorMessage).ToArray();

            TempData["ErrorMessage"] = "Failed to update product.";
            return Json(new { success = false, errors = errorMessages });
        }


        // GET: Delete Product
        public ActionResult Delete(int id)
        {
            var product = _productService.GetProductById(id);
            return PartialView("_DeleteProductPartial", product);
        }

        // POST: Delete Product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                var errorMessages = errors.Select(e => e.ErrorMessage).ToArray();
                TempData["ErrorMessage"] = "Validation failed.";
                return Json(new { success = false, errors = errorMessages });
            }

            int result = _productService.DeleteProduct(id);
            if (result >= 1)
            {
                TempData["SuccessMessage"] = "Product deleted successfully.";
                return Json(new { success = true });
            }
            TempData["ErrorMessage"] = "Failed to delete product.";
            return Json(new { success = false });
        }





    }
}
