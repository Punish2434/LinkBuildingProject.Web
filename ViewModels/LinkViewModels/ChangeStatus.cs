using LinkBuildingProject.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkBuildingProject.Web.ViewModels.LinkViewModels
{
    public class ChangeStatus
    {
        public int Id { get; set; }
        public LinkStatus Status { get; set; }

    }
}
