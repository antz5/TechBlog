using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TechBlogWebAPI.Models
{
    public class NewUserModels
    {                
        public string  EmailId { get; set; }        
        public string  ChoosePassword { get; set; }        
    }
}