﻿namespace JudgeSystem.Web.InputModels.Student
{
    using System.ComponentModel.DataAnnotations;

    using JudgeSystem.Services.Mapping;
	using JudgeSystem.Data.Models;
    using JudgeSystem.Common;

    public class StudentCreateInputModel : IMapTo<Student>
	{
		public const int FullnameMinLength = 5;
		public const int FullnameMaxLength = 100;

		[Required]
		[MinLength(FullnameMinLength), MaxLength(FullnameMaxLength)]
		public string FullName { get; set; }

		[EmailAddress]
		[Required]
		public string Email { get; set; }

		[Range(GlobalConstants.MinStudentsInClass, GlobalConstants.MaxStudentsInClass)]
		public int NumberInCalss { get; set; }

		public int SchoolClassId { get; set; }
	}
}
