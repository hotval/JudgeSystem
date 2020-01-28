﻿using JudgeSystem.Common;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.Infrastructure.Pagination;
using JudgeSystem.Web.ViewModels.Practice;
using JudgeSystem.Web.Infrastructure.Routes;
using JudgeSystem.Web.Filters;

using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Controllers
{
    public class PracticeController : BaseController
    {
        public const int ResultsPerPage = 1;
        private readonly IPracticeService practiceService;

        public PracticeController(IPracticeService practiceService)
        {
            this.practiceService = practiceService;
        }

        public IActionResult Results(int id, int page = GlobalConstants.DefaultPage)
        {
            PracticeAllResultsViewModel model = practiceService.GetPracticeResults(id, page, ResultsPerPage);
            var routeString = new RouteString(nameof(PracticeController), nameof(Results));

            model.PaginationData = new PaginationData
            {
                Url = routeString.AppendId(id).AppendPaginationPlaceholder(),
                NumberOfPages = practiceService.GetPracticeResultsPagesCount(id, ResultsPerPage),
                CurrentPage = page
            };

            return View(model);
        }

        [EndpointExceptionFilter]
        public int ResultsPagesCount(int id) => practiceService.GetPracticeResultsPagesCount(id, ResultsPerPage);
    }
}