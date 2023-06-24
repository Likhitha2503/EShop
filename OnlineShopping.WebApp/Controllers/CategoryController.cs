
﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineShopping.Utility;
using OnlineShopping.WebApp.Models;
using OnlineShopping.WebApp.Services;
using OnlineShopping.WebApp.Services.IServices;

namespace OnlineShopping.WebApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }
        //[Authorize]

        public async Task<IActionResult> IndexCategory()
        {
            List<CategoryDto> list = new();

            var response = await _categoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CategoryDto>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory(CategoryDto model)
        {
            List<CategoryDto> list = new();

            var response = await _categoryService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Category created successfully";
                return RedirectToAction(nameof(IndexCategory));
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(int categoryId)
        {
            var response = await _categoryService.GetAsync<APIResponse>(categoryId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                CategoryDto model = JsonConvert.DeserializeObject<CategoryDto>(Convert.ToString((response.Result)));
                return View(_mapper.Map<CategoryDto>(model));
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(CategoryDto model)
        {
            var response = await _categoryService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Category updated successfully";

                return RedirectToAction(nameof(IndexCategory));
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int categoryId)

        {
            var response = await _categoryService.GetAsync<APIResponse>(categoryId, HttpContext.Session.GetString(SD.SessionToken));

            if (response != null && response.IsSuccess)

            {
                CategoryDto model = JsonConvert.DeserializeObject<CategoryDto>(Convert.ToString((response.Result)));

                return View(model);

            }
            return NotFound();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(CategoryDto model)

        {

            var response = await _categoryService.DeleteAsync<APIResponse>(model.CategoryId, HttpContext.Session.GetString(SD.SessionToken));

            if (response != null && response.IsSuccess)

            {

                TempData["success"] = "Category deleted successfully";
                return RedirectToAction(nameof(IndexCategory));
            }
            TempData["error"] = "Error encountered.";
            return View(model);

        }
    }
}