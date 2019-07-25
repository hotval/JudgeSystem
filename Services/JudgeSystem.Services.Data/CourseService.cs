﻿namespace JudgeSystem.Services.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	using JudgeSystem.Common;
	using JudgeSystem.Data.Common.Repositories;
	using JudgeSystem.Data.Models;
	using JudgeSystem.Services.Mapping;
	using JudgeSystem.Web.Dtos.Course;
    using JudgeSystem.Web.Infrastructure.Exceptions;
    using JudgeSystem.Web.InputModels.Course;
	using JudgeSystem.Web.ViewModels.Course;

	using Microsoft.EntityFrameworkCore;

	public class CourseService : ICourseService
	{
		private readonly IDeletableEntityRepository<Course> repository;

		public CourseService(IDeletableEntityRepository<Course> repository)
		{
			this.repository = repository;
		}

		public async Task Add(CourseInputModel model)
		{
			Course course = model.To<CourseInputModel, Course>();
			await this.repository.AddAsync(course);
			await this.repository.SaveChangesAsync();
		}

		public IEnumerable<CourseViewModel> All()
		{
			return this.repository.All().To<CourseViewModel>().ToList();
		}

		public string GetName(int courseId)
		{
			return repository.All().FirstOrDefault(r => r.Id == courseId)?.Name;
		}

		public async Task<Course> GetById(int courseId)
		{
            if(!this.Exists(courseId))
            {
                throw new EntityNotFoundException("course");
            }

			return await this.repository.All()
			.FirstOrDefaultAsync(c => c.Id == courseId);
		}

		public async Task Updade(CourseEditModel model)
		{
            if (!this.Exists(model.Id))
            {
                throw new EntityNotFoundException("course");
            }

            Course course = await GetById(model.Id);
			if(course == null)
			{
				throw new ArgumentException(string.Format(ErrorMessages.NotFoundEntityMessage, "course"));
			}
			course.Name = model.Name;
			repository.Update(course);
			await repository.SaveChangesAsync();
		}

		public async Task Delete(Course course)
		{
            if (!this.Exists(course.Id))
            {
                throw new EntityNotFoundException("course");
            }

            repository.Delete(course);
			await repository.SaveChangesAsync();
		}

		public IEnumerable<ContestCourseDto> GetAllCourses()
		{
			var courses = repository.All().To<ContestCourseDto>().ToList();
			return courses;
		}

        private bool Exists(int id)
        {
            return this.repository.All().Any(x => x.Id == id);
        }
	}
}
