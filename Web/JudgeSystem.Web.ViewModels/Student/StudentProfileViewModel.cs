﻿namespace JudgeSystem.Web.ViewModels.Student
{
	using JudgeSystem.Services.Mapping;
	using JudgeSystem.Data.Models;
	using AutoMapper;

	public class StudentProfileViewModel : IMapFrom<Student>, IHaveCustomMappings
	{
		public string Id { get; set; }

		public string FullName { get; set; }

		public string Email { get; set; }

        public string UserId { get; set; }

        public int NumberInCalss { get; set; }

        public string SchoolClassName { get; set; }

		public void CreateMappings(IProfileExpression configuration)
		{
			configuration.CreateMap<Student, StudentProfileViewModel>()
				.ForMember(x => x.SchoolClassName, y => y.MapFrom(s => $"{s.SchoolClass.ClassNumber} {s.SchoolClass.ClassType}"));
		}
	}
}
