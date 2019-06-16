﻿namespace JudgeSystem.Web.InputModels.Lesson
{
	using System.ComponentModel.DataAnnotations;

	using Common;
	using Data.Models.Enums;
	using Services.Mapping;
	using Data.Models;

	public class LessonEditInputModel : IMapTo<Lesson>, IMapFrom<Lesson>
	{
		public int Id { get; set; }

		[Required]
		[MinLength(GlobalConstants.NameMinLength)]
		public string Name { get; set; }

		public LessonType Type { get; set; }

		public bool IsLocked { get; set; }
	}
}