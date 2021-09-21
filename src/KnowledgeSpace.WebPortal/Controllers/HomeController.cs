﻿using KnowledgeSpace.WebPortal.Models;
using KnowledgeSpace.WebPortal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace KnowledgeSpace.WebPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IKnowledgeBaseApiClient _knowledgeBaseApiClient;
        private readonly ILabelApiClient _labelApiClient;

        public HomeController(ILogger<HomeController> logger,
             ILabelApiClient labelApiClient,
            IKnowledgeBaseApiClient knowledgeBaseApiClient)
        {
            _logger = logger;
            _knowledgeBaseApiClient = knowledgeBaseApiClient;
            _labelApiClient = labelApiClient;
        }

        public async Task<IActionResult> Index()
        {
            var latestKbs = await _knowledgeBaseApiClient.GetLatestKnowledgeBases(6);
            var popularKbs = await _knowledgeBaseApiClient.GetPopularKnowledgeBases(6);
            var labels = await _labelApiClient.GetPopularLabels(20);
            var viewModel = new HomeViewModel()
            {
                LatestKnowledgeBases = latestKbs,
                PopularKnowledgeBases = popularKbs,
                PopularLabels = labels
            };

            return View(viewModel);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
