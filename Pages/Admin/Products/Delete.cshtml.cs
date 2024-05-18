using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Services;

namespace WebApplication1.Pages.Admin.Products
{
    public class DeleteModel : PageModel
    {

        private readonly IWebHostEnvironment environment;
        private readonly ApplicationDbContext context;
        public string errorMessage = "";
        public string successMesssage = "";
        public DeleteModel(IWebHostEnvironment environment, ApplicationDbContext context)
        {
            this.environment = environment;
            this.context = context;
        }
        public void OnGet(int? id)
        {
            if (id == null)
            {
                successMesssage = "Product updated successfully1";
                Response.Redirect("/Admin/Products/Index");
                return;
            }
            var product = context.Products.Find(id);
            if (product == null)
            {
                successMesssage = "Product updated successfully2";
                Response.Redirect("/Admin/Products/Index");
                return;
            }
            successMesssage = "Product updated successfully3";
            string imageFullPath = Path.Combine(environment.WebRootPath + "/products/" + product.ImageFileName);
            System.IO.File.Delete(imageFullPath);

            context.Products.Remove(product);
            context.SaveChanges();
            Response.Redirect("/Admin/Products/Index");

        }
    }
}
