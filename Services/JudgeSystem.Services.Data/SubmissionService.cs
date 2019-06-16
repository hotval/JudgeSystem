﻿namespace JudgeSystem.Services.Data
{
	using System.Text;
	using System.Threading.Tasks;

	using JudgeSystem.Data.Common.Repositories;
	using JudgeSystem.Data.Models;
	using JudgeSystem.Web.ViewModels.Submission;
	using JudgeSystem.Web.InputModels.Submission;

	public class SubmissionService : ISubmissionService
	{
		private readonly IRepository<Submission> repository;

		public SubmissionService(IRepository<Submission> repository)
		{
			this.repository = repository;
		}

		public async Task<Submission> Create(SubmissionInputModel model, string userId)
		{
			Submission submission = new Submission
			{
				Code = Encoding.UTF8.GetBytes(model.Code),
				ProblemId = model.ProblemId,
				UserId = userId
			};

			await repository.AddAsync(submission);
			await repository.SaveChangesAsync();
			return submission;
		}

		public async Task Update(Submission submission)
		{
			repository.Update(submission);
			await repository.SaveChangesAsync();
		}
	}
}