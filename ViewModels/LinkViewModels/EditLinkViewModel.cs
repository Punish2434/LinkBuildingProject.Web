using LinkBuildingProject.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkBuildingProject.Web.ViewModels.LinkViewModels
{
    public class EditLinkViewModel
    {
        public int Id { get; set; }
        public required string LinkTitle { get; set; }
        public int CategoryId { get; set; }   
        

        [DataType(DataType.Url)]
        public required string LinkUrl { get; set; }
       

    }
}
