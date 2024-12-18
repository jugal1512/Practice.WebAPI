﻿using AutoMapper;
using Practice.Domain.Employees;
using Practice.Domain.RegisterUser;
using Practice.Domain.Skills;
using Practice.Domain.Student;
using Practice.WebAPI.Models;

namespace Practice.WebAPI.AutoMapper
{
    public class AutoMappingProfile : Profile 
    {
        public AutoMappingProfile()
        {
            CreateMap<RegisterUser, RegisterUserDto>().ReverseMap();
            CreateMap<Login, LoginDto>().ReverseMap();
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => string.Join(",", src.Skills.Select(s => s.SkillName).ToList())))
                .ForMember(dest => dest.ProfileImageName,opt => opt.MapFrom(src => src.ProfileImage)).ReverseMap();
            CreateMap<Skill, SkillDto>().ReverseMap();
            CreateMap<Student, StudentViewModal>().ReverseMap();
        }
    }
}