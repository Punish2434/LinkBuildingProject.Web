using LinkBuildingProject.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkBuildingProject.Web.ViewModels.LinkViewModels
{
    public class LinkViewModel
    {
        public int Id { get; set; }
        public required string LinkTitle { get; set; }
        public string CategoryName { get; set; }       
        public string Status { get; set; }
        public required string LinkUrl { get; set; }
        public string? ApplicationUser { get; set; }
        public string? ApplicationUserid { get; set; }
        

    }
}
