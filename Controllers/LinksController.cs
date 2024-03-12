
using LinkBuilding.Repository.Implementations;
using LinkBuilding.Repository.Interfaces;
using LinkBuildingProject.Domain;
using LinkBuildingProject.Web.ViewModels.CategoryViewModels;
using LinkBuildingProject.Web.ViewModels.LinkViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace LinkBuildingProject.Web.Controllers
{
    
    public class LinksController : Controller
    {
        private readonly ILinkRepo _linkRepo;
        private readonly ICategoryRepo _categoryRepo;

        public LinksController(ILinkRepo linkRepo, ICategoryRepo categoryRepo)
        {
            _linkRepo = linkRepo;
            _categoryRepo = categoryRepo;
        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Index(string sortOrder=null)
        {
            
            List<LinkViewModel> list = new List<LinkViewModel>();
            var links = await _linkRepo.GetAll();           

            foreach (var link in links)
            {
                list.Add(new LinkViewModel
                {
                    Id = link.Id,
                    LinkTitle = link.LinkTitle,
                    LinkUrl = link.LinkUrl,
                    CategoryName = link.Category.Title,
                    Status = link.Status.ToString(),
                    ApplicationUser =  link.ApplicationUser.Email
                   
                });

            }

            switch(sortOrder)
            {
               
                case "Failed": list =  list.Where(x=>x.Status== "Failed").ToList(); break;  
                case "Pending": list =  list.Where(x=>x.Status== "Pending").ToList(); break;  
                case "Success": list =  list.Where(x=>x.Status== "Success").ToList(); break;
                default: list.ToList(); break;
            }

            return View(list);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditLinks(int id)
        {
            var link =  await _linkRepo.GetById(id);
            ChangeStatus vm = new ChangeStatus();
            vm.Id = link.Id;
            vm.Status = link.Status;
            return View(vm);

        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditLinks(ChangeStatus vm)
        {
            var link = await _linkRepo.GetById(vm.Id);
            link.Status = vm.Status;
            await _linkRepo.Edit(link);
            return RedirectToAction("Index");

        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var CategoryList =  await _categoryRepo.GetAll();
            ViewBag.Categories = new SelectList(CategoryList, "Id", "Title");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateLinkViewModel vm)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId =  claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
           
            if (ModelState.IsValid)
            {
                var link = new Link
                {
                    LinkTitle =  vm.LinkTitle,
                    LinkUrl = vm.LinkUrl,
                    CategoryId =  vm.CategoryId,
                    Status = LinkStatus.Pending,
                    ApplicationUserId = userId

                };
                await _linkRepo.Save(link);
                return View("success");
            }
            return RedirectToAction("Create");
        }
        [Authorize]
        public async Task<IActionResult> GetUserLinks()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var links = await _linkRepo.GetAllLinks(userId);

            List<LinkViewModel> vm = new List<LinkViewModel>();
            foreach (var link in links)
            {
                vm.Add(new LinkViewModel
                {
                    Id = link.Id,
                    ApplicationUserid = link.ApplicationUserId,
                    CategoryName =  link.Category.Title,
                    LinkUrl = link.LinkUrl,
                    LinkTitle = link.LinkTitle,
                    Status = link.Status.ToString()
                });
            }
            return View(vm);


        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (_linkRepo.IsLinkIdExists(id, userId))
            {

                var CategoryList = await _categoryRepo.GetAll();
                ViewBag.Categories = new SelectList(CategoryList, "Id", "Title");

                var link = await _linkRepo.GetById(id);
                if (link == null) { return NotFound(); }
                var vm = new EditLinkViewModel
                {
                    LinkTitle = link.LinkTitle,
                    LinkUrl = link.LinkUrl,
                    Id = link.Id,
                    CategoryId = link.CategoryId

                };
                return View(vm);

            }
            return RedirectToAction("GetUserLinks");
            

            
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(EditLinkViewModel vm)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var link = new Link
            {
                Id =  vm.Id,
                LinkTitle =  vm.LinkTitle,
                LinkUrl = vm.LinkUrl,
                CategoryId =  vm.CategoryId,
                Status =  LinkStatus.Pending,
                ApplicationUserId = userId 
            };
            await _linkRepo.Edit(link);
            return RedirectToAction("GetUserLinks");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (_linkRepo.IsLinkIdExists(id, userId))
            {
                var link = await _linkRepo.GetById(id);
                if (link == null) { return NotFound();}
                await _linkRepo.RemoveData(link);
            }
               
            return RedirectToAction("GetUserLinks");
        }
    }
}
