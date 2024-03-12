using LinkBuilding.Repository.Interfaces;
using LinkBuildingProject.Domain;
using LinkBuildingProject.Web.ViewModels.CategoryViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace TechnologyKeeda.UI.Controllers
{
    [Authorize(Roles ="Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepo _CategoryRepo;

        public CategoriesController(ICategoryRepo categoryRepo)
        {
            _CategoryRepo = categoryRepo;
        }

        public async Task<IActionResult> Index()
        {
          
                List<CategoryViewModel> vm = new List<CategoryViewModel>();

                var categories = await _CategoryRepo.GetAll();

                foreach (var category in categories)
                {
                    vm.Add(new CategoryViewModel { Id = category.Id, Title = category.Title
                        });
                }

                return View(vm);
           
        }
        [HttpGet]
        public IActionResult Create()
        {
           
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCaegoryViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    Title = vm.Title,
                };
                await _CategoryRepo.Save(category);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            
            var category = await _CategoryRepo.GetById(id);
            CategoryViewModel vm = new CategoryViewModel
            {
                Id=category.Id,
                Title = category.Title   
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CategoryViewModel vm)
        {
            var category = new Category
            {
                Id = vm.Id,
                Title = vm.Title,
            };
            await _CategoryRepo.Edit(category);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _CategoryRepo.GetById(id);
            await _CategoryRepo.RemoveData(category);
            return RedirectToAction("Index");
        }

    }
}
