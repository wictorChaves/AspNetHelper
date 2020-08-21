using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreOfBuild.Domain;
using StoreOfBuild.Domain.Products;
using StoreOfBuild.Web.Models;
using StoreOfBuild.Web.ViewsModels;

namespace StoreOfBuild.Web.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly CategoryStorer categoryStorer;
        private readonly IRepository<Category> categoryRepository;

        public CategoryController(CategoryStorer categoryStorer, IRepository<Category> categoryRepository)
        {
            this.categoryStorer = categoryStorer;
            this.categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            var categories = this.categoryRepository.All();

            var viewsModels = categories.Select(c => new CategoryViewModel { Id = c.Id, Name = c.Name });
            return View(viewsModels);
        }

        public IActionResult CreateOrEdit(int id)
        {
            if (id > 0)
            {
                var category = this.categoryRepository.GetById(id);
                var categoryViewModel = new CategoryViewModel { Id = category.Id, Name = category.Name };
                return View(categoryViewModel);
            }
            else
            {
                return View();
            }

        }

        [HttpPost]
        public IActionResult CreateOrEdit(CategoryViewModel viewModel)
        {
            this.categoryStorer.Store(viewModel.Id, viewModel.Name);
            return View();
        }
    }
}
