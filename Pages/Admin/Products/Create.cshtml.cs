using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Pages.Admin.Products
{
    public class CreateModel : PageModel
    {
        private readonly IWebHostEnvironment environment;
        private readonly ApplicationDbContext context;

        [BindProperty]
        public ProductDto ProductDto { get; set; } = new ProductDto();
        public object DataTime { get; private set; }

        public CreateModel(IWebHostEnvironment environment, ApplicationDbContext context) 
        {
            this.environment = environment;
            this.context = context;
        }
        public void OnGet()
        {
        }

        public string errorMessage = "";
        public string sucessMessage = "";

        public object GetDataTime()
        {
            return DataTime;
        }

        public void OnPost(object dataTime)
        { 
            if (ProductDto.ImageFile == null) 
            {
                ModelState.AddModelError("ProductDto.ImageFile", "This Image File is Required");
            }
            if (!ModelState.IsValid) 
            {
                errorMessage = "Please provide the required felds";
                return;
            }

            //save the image file
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(ProductDto.ImageFile!.FileName);

            string imageFullPath = environment.WebRootPath +"/products/" + newFileName;
            using (var stream =System.IO.File.Create(imageFullPath))
            {
                ProductDto.ImageFile.CopyTo(stream);
            }

            //save the new product in the database
            Product product = new Product()
            {
                Name = ProductDto.Name,
                Brand = ProductDto.Brand,
                Category = ProductDto.Category,
                Price = ProductDto.Price,
                Description = ProductDto.Description ?? "",
                ImageFileName = newFileName,
                CreatedAt = DateTime.Now,
            };

            context.Products.Add(product);
            context.SaveChanges();

            //clear the form
            ProductDto.Name = "";
            ProductDto.Brand = "";
            ProductDto.Category = "";
            ProductDto.Price = 0;
            ProductDto.Description = "";
            ProductDto.ImageFile= null;

            ModelState.Clear();

            sucessMessage = "Product created successfully";

            Response.Redirect("/Admin/Products/Index");
        }
    }
}
