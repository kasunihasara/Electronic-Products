﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class ProductDto
    {
        internal object Id;

        [Required, MaxLength(100)]
        public string Name { get; set; } = "";
        [Required, MaxLength(100)]
        public string Brand { get; set; } = "";
        [Required, MaxLength(100)]
        public string Category { get; set; } = "";
        [Required]
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string ImageFileName { get; internal set; }
    }
}