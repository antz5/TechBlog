using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechBlogWebAPI.Models
{
    public class CategoryModels
    {
        public string emailId { get; set; }
        public string categoryName { get; set; }
        public string categoryDescription { get; set; }
        public bool isActive { get; set; }
    }
}