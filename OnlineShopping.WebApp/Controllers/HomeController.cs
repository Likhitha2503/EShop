using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using OnlineShopping.Utility;
using OnlineShopping.WebApp.Models;
using OnlineShopping.WebApp.Models.Dto;
using OnlineShopping.WebApp.Models.VM;
using OnlineShopping.WebApp.Services;
using OnlineShopping.WebApp.Services.IServices;
using System.Diagnostics;

namespace OnlineShopping.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;
        private readonly ICartService _cartService;
        public HomeController(IProductService productService, IMapper mapper, ICategoryService categoryService, ICartService cartService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _mapper = mapper;
            _cartService = cartService;

        }

        public async Task<IActionResult> Index()
        {
            List<ProductDto> list = new();

            var response = await _productService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            return View(list);
        }


        [Authorize]
        public async Task<IActionResult> ProductDetails(int productId)
        {
            ProductdetailsVM productVM = new();

            var response = await _productService.GetAsync<APIResponse>(productId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                ProductDto model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString((response.Result)));
                productVM.Product = _mapper.Map<UpdateProductDto>(model);
            }
            return View(productVM);
        }
        [Authorize]
        [HttpPost]
        [ActionName("ProductDetails")]
        public async Task<IActionResult> ProductDetails(ProductdetailsVM productVM)
        {
            CartDto cartDto = new CartDto()
            {
                CartHeader = new CartHeaderDto
                {
                    UserId = HttpContext.Session.GetString(SD.SessionUserId)
                }
            };

            CartDetailsDto cartDetails = new CartDetailsDto()
            {
                Count = productVM.Count,
                ProductId = productVM.Product.ProductId,
            };

            List<CartDetailsDto> cartDetailsDtos = new() { cartDetails };
            cartDto.CartDetails = cartDetailsDtos;

            APIResponse? response = await _cartService.CreateAsync<APIResponse>(cartDto);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Item has been added to the Shopping Cart";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Error encountered.";
            }

            return View(productVM);
        }
    }
}