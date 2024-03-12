
using LinkBuilding.Repository.Interfaces;
using LinkBuildingProject.Domain;
using LinkBuildingProject.Web.Models;
using LinkBuildingProject.Web.ViewModels.LinkViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using System.Diagnostics;

namespace LinkBuildingProject.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILinkRepo _linkRepo;

        public HomeController(ILogger<HomeController> logger, ILinkRepo linkRepo)
        {
            _logger = logger;
            _linkRepo = linkRepo;
        }

        public async Task<IActionResult> Index()
        {
            List<DashBoard> list = new List<DashBoard>();
            var links = await _linkRepo.GetAll();
            var filteredLinks =  links.Where(x=>x.Status==LinkStatus.Success).ToList();
            foreach (var link in filteredLinks)
            {
                list.Add(new DashBoard { Id=link.Id,
                    Title=link.LinkTitle,
                    LinkUrl=link.LinkUrl,
                Category = link.Category.Title});

            }
            return View(list);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
