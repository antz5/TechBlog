using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TechBlogWebAPI.Models
{
    public class LoginModels
    {
        
        public string EmailId { get; set; }

        
        public string Password { get; set; }        
            
    }
}