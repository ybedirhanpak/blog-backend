﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog_Project.Dtos;
using Blog_Project.Dtos.PostDtos;
using Blog_Project.Dtos.UserDtos;
using Blog_Project.Models;

namespace Blog_Project.Settings
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            //User
            CreateMap<UserCreateDto, User>();
            CreateMap<User, UserAuthenticatedDto>();
            CreateMap<User, UserOutDto>();

            //Category
            CreateMap<CategoryInDto, Category>();

            //User-Category
            CreateMap<UserCategoryDto, UserCategory>();

            //Post
            CreateMap<PostCreateDto, Post>();
            CreateMap<Post, PostOutDto>();
            
            //Comment
            CreateMap<CommentCreateDto, Comment>();
        }

    }
}
