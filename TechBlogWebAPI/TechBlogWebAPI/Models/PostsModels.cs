using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TechBlogWebAPI.Models
{
    public class PostsModels
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string  EmailId { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string PostTitle { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string PostContent { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string PostedBy { get; set; }

      
        [DataType(DataType.DateTime)]
        public string PostedDate { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public char Status { get; set; }

       
        [DataType(DataType.DateTime)]
        public DateTime LastEditedDate { get; set; }
                
        public string CategoryId { get; set; }
    }
}