using LinkBuildingProject.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkBuildingProject.Web.ViewModels.LinkViewModels
{
    public class CreateLinkViewModel
    {
       
        public string LinkTitle { get; set; }
        [Display(Name ="Category")]
        public int CategoryId { get; set; }        
        public LinkStatus Status { get; set; }

        [DataType(DataType.Url)]
        public required string LinkUrl { get; set; }
        public string? ApplicationUserid { get; set; }
        

    }
}
