using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Services;
using WebApplication1.Models;

namespace WebApplication1.Pages.Admin.Products
{
    public class EditModel : PageModel
    {
        private readonly IWebHostEnvironment environment;
        private readonly ApplicationDbContext context;

        [BindProperty]
        public ProductDto ProductDto { get; set; } = new ProductDto();
        public Product Product { get; set; } = new Product();

        public string errorMessage = "";
        public string successMesssage = "";
        public EditModel(IWebHostEnvironment environment, ApplicationDbContext context)
        {
            this.environment = environment;
            this.context = context;
        }
        public void OnGet(int? id)
        {
            if (id == null)
            {
                Response.Redirect("/Admin/Products/Index");
                return;
            }
            var product = context.Products.Find(id);
            if (product == null)
            {
                Response.Redirect("/Admin/Products/Index");
                return;
            }

            ProductDto.Name = product.Name;
            ProductDto.Brand = product.Brand;
            ProductDto.Category = product.Category;
            ProductDto.Price = product.Price;
            ProductDto.Description = product.Description;

            Product = product;
        }

        public void OnPost(int? id)
        {
            if (id == null)
            {
                Response.Redirect("/Admin/Products/Index");
                return;
            }
            

            var product = context.Products.Find(id);

            if (product == null)
            {
                Response.Redirect("/Admin/Products/Index");
                return;
            }

            // Update the image if we have a new image file
            string newFileName = product.ImageFileName;

            if (ProductDto.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(ProductDto.ImageFile.FileName);

                string imageFullPath = Path.Combine(environment.WebRootPath + "/products/" + newFileName);
                using (var stream = new FileStream(imageFullPath, FileMode.Create))
                {
                    ProductDto.ImageFile.CopyTo(stream);
                }

                // Delete the old Image
                string oldImageFullPath = Path.Combine(environment.WebRootPath + "/products/" + product.ImageFileName);
                if (System.IO.File.Exists(oldImageFullPath))
                {
                    System.IO.File.Delete(oldImageFullPath);
                }
            }

            // Update the product in database
            product.Name = ProductDto.Name;
            product.Brand = ProductDto.Brand;
            product.Category = ProductDto.Category;
            product.Price = ProductDto.Price;
            product.Description = ProductDto.Description ?? "";
            product.ImageFileName = newFileName;

            context.SaveChanges();
            Product = product;

            successMesssage = "Product updated successfully";

            Response.Redirect("/Admin/Products/Index");
        }

    }
}