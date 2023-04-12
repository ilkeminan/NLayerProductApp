using Microsoft.AspNetCore.Mvc;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.MVC.Filters;
using NLayer.MVC.Services;

namespace NLayer.MVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly CategoryApiService _categoryApiService;

        public CategoriesController(CategoryApiService categoryApiService)
        {
            _categoryApiService = categoryApiService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _categoryApiService.GetAllAsync());
        }

        [HttpGet]
        public IActionResult Save()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(CategoryDTO categoryDTO)
        {
            if (ModelState.IsValid)
            {
                await _categoryApiService.SaveAsync(categoryDTO);
                return RedirectToAction(nameof(Index));
            }

            return View(categoryDTO);
        }

        [ServiceFilter(typeof(NotFoundFilter<Category>))]
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var category = await _categoryApiService.GetByIdAsync(id);
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CategoryDTO categoryDTO)
        {
            if (ModelState.IsValid)
            {
                await _categoryApiService.UpdateAsync(categoryDTO);
                return RedirectToAction(nameof(Index));
            }

            return View(categoryDTO);
        }

        public async Task<IActionResult> Remove(int id)
        {
            await _categoryApiService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetProductsFromCategoryId(int id)
        {
            var categoryWithProductsDto = await _categoryApiService.GetSingleCategoryByIdWithProducts(id);
            return View(categoryWithProductsDto);
        }
    }
}
