﻿using System.Collections.Generic;
using System.Threading.Tasks;

using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Web.Dtos.Student;
using JudgeSystem.Web.InputModels.Student;
using JudgeSystem.Web.ViewModels.Student;

namespace JudgeSystem.Services.Data
{
	public interface IStudentService
	{
		Task<StudentDto> Create(StudentCreateInputModel model, string activationKey);

		Task<StudentDto> GetStudentProfileByActivationKey(string activationKey);

		Task SetStudentProfileAsActivated(string id);

		Task<StudentProfileViewModel> GetStudentInfo(string studentId);

		IEnumerable<StudentProfileViewModel> SearchStudentsByClass(int? classNumber, SchoolClassType? classType);

		Task<T> GetById<T>(string id);

		Task Delete(string id);

		Task<SchoolClassDto> GetStudentClass(string id);

        Task<StudentDto> Update(StudentEditInputModel model);
    }
}
