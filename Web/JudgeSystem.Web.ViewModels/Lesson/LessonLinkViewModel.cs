﻿namespace JudgeSystem.Web.ViewModels.Lesson
{
	using Services.Mapping;
	using Data.Models;
	using AutoMapper;

	public class LessonLinkViewModel : IMapFrom<Lesson>, IHaveCustomMappings
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public int ProblemsCount { get; set; }

		public void CreateMappings(IMapperConfigurationExpression configuration)
		{
			configuration.CreateMap<Lesson, LessonLinkViewModel>()
				.ForMember(x => x.ProblemsCount, y => y.MapFrom(s => s.Problems.Count));
		}
	}
}
