using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace TechBlogWebAPI.Models
{
    public class ProfileModels
    {
        public string   firstName         { get; set; }
        public string   lastName          { get; set; }
        public string   DOB               { get; set; }
        public string   Mobile            { get; set; }
        public string   Organization      { get; set; }
        public string   Country           { get; set; }        
        public string   AboutMe           { get; set; }        
        public string   Sex               { get; set; }
        public string   EmailId           { get; set; }
        public string   City              { get; set; }
        public string   Website           { get; set; }
        public byte[]   ProfilePicture  { get; set; }
    }
}