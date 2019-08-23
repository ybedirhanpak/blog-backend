﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog_Project.Dtos.CategoryDtos;
using Blog_Project.Helpers;
using Blog_Project.Models;
using Blog_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog_Project.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MainCategoriesController : ControllerBase
    {
        private readonly Repository<MainCategory> _mainCategoryRepository;
        private readonly IMapper _mapper;

        public MainCategoriesController(Repository<MainCategory> mainCategoryRepository, IMapper mapper)
        {
            _mainCategoryRepository = mainCategoryRepository;
            _mapper = mapper;
        }

        /**
         * Gives response including all main categories with their subcategories
         */
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<List<MainCategory>> GetAll()
        {
            return Ok(_mainCategoryRepository.All().Include(mc => mc.SubCategories).ToList());
        }

        [HttpPost]
        public ActionResult<MainCategory> Create([FromBody] MainCategoryCreateDto mainCategoryIn)
        {

            //Check if request is sent by admin.
            if (!AuthorizationHelpers.IsAdmin(HttpContext.User))
            {
                return BadRequest(new Message("Unauthorized user."));
            }

            //Check if request is valid.
            if (string.IsNullOrEmpty(mainCategoryIn.Name))
            {
                return BadRequest(new Message("Please give a valid name."));
            }

            if (_mainCategoryRepository.Where(mc => mc.Name == mainCategoryIn.Name).Any())
            {
                return BadRequest(new Message("There is already a main category with name: " + mainCategoryIn.Name));
            }

            var mainCategory = _mapper.Map<MainCategory>(mainCategoryIn);

            if (_mainCategoryRepository.Add(mainCategory))
            {
                return Ok(mainCategory);
            }

            return BadRequest(new Message("Error when creating main category"));
        }

        [HttpPost("{id}")]
        public ActionResult<MainCategory> Update(string id, [FromBody] MainCategoryCreateDto mainCategoryIn)
        {

            //Check if request is sent by admin.
            if (!AuthorizationHelpers.IsAdmin(HttpContext.User))
            {
                return BadRequest(new Message("Unauthorized user."));
            }

            var mainCategory = _mainCategoryRepository.GetById(Guid.Parse(id));

            if (mainCategory == null)
            {
                return NotFound(new Message("There is no main category with id: " + id));
            }

            if (!string.IsNullOrEmpty(mainCategoryIn.Name))
            {
                mainCategory.Name = mainCategoryIn.Name;
            }

            if (_mainCategoryRepository.Update(mainCategory))
            {
                return Ok(mainCategory);
            }

            return BadRequest(new Message("Error when updating main category with id: " + id));
        }

        [HttpPost("{id}")]
        public ActionResult<MainCategory> Delete(string id)
        {
            //Check if request is sent by admin.
            if (!AuthorizationHelpers.IsAdmin(HttpContext.User))
            {
                return BadRequest(new Message("Unauthorized user."));
            }

            var mainCategory = _mainCategoryRepository.GetById(Guid.Parse(id));

            if (mainCategory == null)
            {
                return NotFound(new Message("There is no main category with id: " + id));
            }

            mainCategory.IsDeleted = true;

            if (_mainCategoryRepository.Update(mainCategory))
            {
                return Ok("Main category with name: " + mainCategory.Name + " and with id: " + mainCategory.Id +
                          " is deleted successfully");
            }

            return BadRequest(new Message("Error when updating main category with id: " + id));
        }
    }
}
