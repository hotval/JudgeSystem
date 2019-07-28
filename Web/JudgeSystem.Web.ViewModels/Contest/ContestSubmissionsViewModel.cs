﻿using JudgeSystem.Web.Dtos.Submission;
using System.Collections.Generic;

namespace JudgeSystem.Web.ViewModels.Contest
{
    public class ContestSubmissionsViewModel
    {
        public IEnumerable<SubmissionResult> Submissions { get; set; }

        public string ProblemName { get; set; }

        public int LessonId { get; set; }

        public string UrlPlaceholder { get; set; }
    }
}