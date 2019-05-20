﻿using System.ComponentModel.DataAnnotations;

namespace JudgeSystem.Data.Models
{
	public class Test
	{
		public Test()
		{
			IsTrialTest = false;
		}

		public int Id { get; set; }

		public Problem Problem { get; set; }
		public int ProblemId { get; set; }

		[Required]
		public string InputData { get; set; }

		[Required]
		public string OutputData { get; set; }

		public bool IsTrialTest { get; set; }
	}
}
