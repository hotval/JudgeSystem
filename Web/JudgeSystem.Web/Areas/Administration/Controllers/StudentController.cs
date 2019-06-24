﻿namespace JudgeSystem.Web.Areas.Administration.Controllers
{
	using System;
	using System.Collections.Generic;
    using System.IO;
    using System.Linq;
	using System.Threading.Tasks;

	using JudgeSystem.Common;
	using JudgeSystem.Data.Models;
    using JudgeSystem.Data.Models.Enums;
    using JudgeSystem.Services.Data;
	using JudgeSystem.Services.Mapping;
	using JudgeSystem.Services.Messaging;
	using JudgeSystem.Web.InputModels.Student;
    using JudgeSystem.Web.ViewModels.Student;

    using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.Rendering;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.Logging;

	public class StudentController : AdministrationBaseController
	{
		private readonly IStudentService studentService;
		private readonly ISchoolClassService schoolClassService;
		private readonly IConfiguration configuration;
		private readonly ILoggerFactory loggerFactory;

		public StudentController(IStudentService studentService, ISchoolClassService schoolClassService,
			IConfiguration configuration, ILoggerFactory loggerFactory)
		{
			this.studentService = studentService;
			this.schoolClassService = schoolClassService;
			this.configuration = configuration;
			this.loggerFactory = loggerFactory;
		}

		public IActionResult Create()
		{
			ViewData["classes"] = GetSchoolClassesAsSelectListItem();
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(StudentCreateInputModel model)
		{
			if (!ModelState.IsValid)
			{
				ViewData["classes"] = GetSchoolClassesAsSelectListItem();
				return View(model);
			}

			string activationKey = Guid.NewGuid().ToString();
			bool sendedSuccessfully = await SendActivationEmail(activationKey, model.Email);
			if (!sendedSuccessfully)
			{
				ModelState.AddModelError(string.Empty, ErrorMessages.NotValidEmail);
				return View(model);
			}

			Student student = model.To<Student>();
			student.ActivationKeyHash = activationKey;
			await studentService.Create(student);

			return Redirect("/");
		}

		public IActionResult StudentsByClass(int? classNumber, SchoolClassType? classType)
		{
			IEnumerable<StudentProfileViewModel> students = studentService.SearchStudentsByClass(classNumber, classType);
			return View(students);
		}

		public async Task<IActionResult> Edit(string id)
		{
			ViewData["classes"] = GetSchoolClassesAsSelectListItem();
			var model = await studentService.GetById<StudentEditInputModel>(id);
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(StudentEditInputModel model)
		{
			if(!ModelState.IsValid)
			{
				ViewData["classes"] = GetSchoolClassesAsSelectListItem();
				return View(model);
			}

			Student student = await studentService.UpdateAsync(model);
			SchoolClass schoolClass = await studentService.GetStudentClassAsync(student.Id);
			return RedirectToAction(nameof(StudentsByClass), 
				new { classNumber = schoolClass.ClassNumber, classType = schoolClass.ClassType });
		}

		public async Task<IActionResult> Delete(string id)
		{
			var model = await studentService.GetById<StudentProfileViewModel>(id);
			return View(model);
		}

		[HttpPost]
		[ActionName(nameof(Delete))]
		public async Task<IActionResult> DeletePost(string id)
		{
			Student student = await studentService.GetById(id);
			SchoolClass schoolClass = await studentService.GetStudentClassAsync(student.Id);
			await studentService.DeleteAsync(student);
			return RedirectToAction(nameof(StudentsByClass),
				new { classNumber = schoolClass.ClassNumber, classType = schoolClass.ClassType });
		}

		[NonAction]
		private async Task<bool> SendActivationEmail(string activationKey, string toAddress)
		{
			string fromName = configuration["App:Name"];
			string sendGridApiKey = configuration["SendGrid:ApiKey"];
			string fromAddress = configuration["Email:Username"];
			string subject = GlobalConstants.StudentProfileActivationEmailSubject;
			string message = await ReadEmailTemplateAsync();
			string activationKeyPlaceholder = "@{activationKey}";
			message = message.Replace(activationKeyPlaceholder, activationKey);

			try
			{
				SendGridEmailSender emailSender = new SendGridEmailSender(loggerFactory, sendGridApiKey, fromAddress, fromName);
				await emailSender.SendEmailAsync(toAddress, subject, message);
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}

		[NonAction]
		private async Task<string> ReadEmailTemplateAsync()
		{
			string activationTemplateName = "StudentProfileActivation.html";
			string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", 
				GlobalConstants.TemplatesFolder, 
				GlobalConstants.EmailTemplatesFolder, 
				activationTemplateName);

			string temaplte = await System.IO.File.ReadAllTextAsync(path);
			return temaplte;
		}

		[NonAction]
		private IEnumerable<SelectListItem> GetSchoolClassesAsSelectListItem()
		{
			var schoolClasses = schoolClassService.GetAllClasses()
				.Select(c => new SelectListItem
				{
					Text = $"{c.ClassNumber} {c.ClassType}",
					Value = c.Id.ToString()
				});

			return schoolClasses;
		}
	}
}
