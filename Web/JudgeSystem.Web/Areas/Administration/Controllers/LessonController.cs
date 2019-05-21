﻿using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Services.Data;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.Infrastructure.Extensions;
using JudgeSystem.Web.Utilites;
using JudgeSystem.Web.ViewModels.Lesson;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JudgeSystem.Web.Areas.Administration.Controllers
{
	public class LessonController : AdministrationBaseController
	{
		private readonly IResourceService resourceService;
		private readonly ILessonService lessonService;

		public LessonController(IResourceService resourseService, ILessonService lessonService)
		{
			this.resourceService = resourseService;
			this.lessonService = lessonService;
		}

		public IActionResult Create()
		{
			ViewData["lessonTypes"] = Utility.GetSelectListItems<LessonType>();

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(LessonInputModel model)
		{
			if (!ModelState.IsValid)
			{
				ViewData["lessonTypes"] = EnumExtensions.GetEnumValuesAsString<LessonType>().
				Select(t => new SelectListItem { Value = t, Text = t })
				.ToList();
				return View(model);
			}

			List<Resource> resources = new List<Resource>();

			foreach (var formFile in model.Resources.Where(f => f.Length > 0))
			{
				string fileOriginalName = formFile.FileName;
				var fileName = Path.GetRandomFileName() + fileOriginalName;
				var filePath = GlobalConstants.FileStorePath + fileName;

				Resource resource = resourceService.CreateResource(fileName, fileOriginalName);
				resources.Add(resource);
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await formFile.CopyToAsync(stream);
				}
			}

			//TODO: hash lesson password for more security
			Lesson newLesson = await lessonService.CreateLesson(model, resources);

			return RedirectToAction("Details", "Lesson", new { id = newLesson.Id });
		}

		public async Task<IActionResult> Edit(int id, string lessonType, int courseId)
		{
			Lesson lesson = await lessonService.GetById(id);
			if(lesson == null)
			{
				string errorMessage = string.Format(ErrorMessages.NotFoundEntityMessage, "lesson");
				return ShowError(errorMessage, "All", "Course", new { lessonType, courseId});
			}
			var model = lesson.To<Lesson, LessonEditInputModel>();
			ViewData["lessonTypes"] = Utility.GetSelectListItems<LessonType>();
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(LessonEditInputModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			Lesson lesson = await lessonService.GetById(model.Id);
			if(lesson == null)
			{
				string message = string.Format(ErrorMessages.NotFoundEntityMessage, "lesson");
				return ShowError(message, "Lessons", "Course", new { lessonType = lesson.Type, lesson.CourseId });
			}

			lesson.Name = model.Name;
			lesson.Type = model.Type;
			await lessonService.Update(lesson);

			return RedirectToAction("Lessons", "Course", new { lessonType = lesson.Type, lesson.CourseId });
		}

		public IActionResult AddPassword()
		{
			return View();
		}

		[HttpPost]
		public IActionResult AddPassword(LessonAddPasswordInputModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			return Json(model);
		}

		public IActionResult ChangePassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ChangePassword(LessonChangePasswordInputModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			Lesson lesson = await lessonService.GetById(model.Id);

			if(lesson.IsLocked && lesson.LessonPassword == model.OldPassword)
			{
				lesson.LessonPassword = model.NewPassword;
				await lessonService.Update(lesson);
				string infoMessage = string.Format(InfoMessages.ChangePasswordSuccessfully, lesson.Name);
				return this.ShowInfo(infoMessage, "Lessons", "Course", new { lessonType = lesson.Type, lesson.CourseId });

			}
			else
			{
				string errorMessage = ErrorMessages.DiffrentLessonPasswords;
				this.ModelState.AddModelError(string.Empty, errorMessage);
				return View(model);
			}
		}

	}
}
