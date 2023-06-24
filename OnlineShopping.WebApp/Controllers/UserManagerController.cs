
﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineShopping.Utility;
using OnlineShopping.WebApp.Models;
using OnlineShopping.WebApp.Models.Dto;
using OnlineShopping.WebApp.Services;
using OnlineShopping.WebApp.Services.IServices;

namespace OnlineShopping.WebApp.Controllers
{
    public class UserManagerController : Controller
    {
        private readonly IUserManagerService _userManagerService;
        private readonly IMapper _mapper;
        public  UserManagerController(IUserManagerService userManagerService, IMapper mapper)
        {
            _userManagerService = userManagerService;
            _mapper = mapper;
        }
		
        [Authorize(Roles = "Admin")]
		public async Task<IActionResult> IndexUsers()
        {
            List<UserManagerDto> list = new();

            var response = await _userManagerService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<UserManagerDto>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
       
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(int Id)
        {
            var response = await _userManagerService.GetAsync<APIResponse>(Id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                UserManagerDto model = JsonConvert.DeserializeObject<UserManagerDto>(Convert.ToString((response.Result)));
                return View(_mapper.Map<UserManagerDto>(model));
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(UserManagerDto model)
        {
            var response = await _userManagerService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "User updated successfully";

                return RedirectToAction(nameof(IndexUsers));
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        
        
    }
}