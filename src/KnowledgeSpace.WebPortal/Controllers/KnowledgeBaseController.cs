﻿using KnowledgeSpace.ViewModel.Contents;
using KnowledgeSpace.WebPortal.Extensions;
using KnowledgeSpace.WebPortal.Models;
using KnowledgeSpace.WebPortal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace KnowledgeSpace.WebPortal.Controllers
{
    public class KnowledgeBaseController : Controller
    {
        private readonly IKnowledgeBaseApiClient _knowledgeBaseApiClient;
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly IConfiguration _configuration;
        private readonly ILabelApiClient _labelApiClient;
        private readonly IUserApiClient _userApiClient;

        public KnowledgeBaseController(IKnowledgeBaseApiClient knowledgeBaseApiClient,
            ICategoryApiClient categoryApiClient,
            IConfiguration configuration,
            IUserApiClient userApiClient,
            ILabelApiClient labelApiClient)
        {
            _knowledgeBaseApiClient = knowledgeBaseApiClient;
            _categoryApiClient = categoryApiClient;
            _configuration = configuration;
            _labelApiClient = labelApiClient;
            _userApiClient = userApiClient;
        }

        public async Task<IActionResult> ListByCategoryId(int id, int page = 1)
        {
            var pageSize = int.Parse(_configuration["PageSize"]);
            var category = await _categoryApiClient.GetCategoryById(id);
            var data = await _knowledgeBaseApiClient.GetKnowledgeBasesByCategoryId(id, page, pageSize);
            var viewModel = new ListByCategoryViewModel()
            {
                Data = data,
                Category = category
            };
            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var knowledgeBase = await _knowledgeBaseApiClient.GetKnowledgeBaseDetail(id);
            var category = await _categoryApiClient.GetCategoryById(knowledgeBase.CategoryId);
            var label = await _knowledgeBaseApiClient.GetLabelsByKnowledgeBaseId(id);
            var viewModel = new KnowledgeBaseDetailViewModel()
            {
                Detail = knowledgeBase,
                Category = category,
                Labels = label
            };
            if (User.Identity.IsAuthenticated)
            {
                viewModel.CurrentUser = await _userApiClient.GetById(User.GetUserId());
            }
            await _knowledgeBaseApiClient.UpdateViewCount(id);
            return View(viewModel);
        }

        public async Task<IActionResult> Search(string keyword, int page = 1)
        {
            var pageSize = int.Parse(_configuration["PageSize"]);
            var data = await _knowledgeBaseApiClient.SearchKnowledgeBase(keyword, page, pageSize);
            var viewModel = new SearchKnowledgeBaseViewModel()
            {
                Data = data,
                Keyword = keyword
            };
            return View(viewModel);
        }

        public async Task<IActionResult> ListByTag(string tagId, int page = 1)
        {
            var pageSize = int.Parse(_configuration["PageSize"]);
            var data = await _knowledgeBaseApiClient.GetKnowledgeBasesByTagId(tagId, page, pageSize);
            var label = await _labelApiClient.GetLabelById(tagId);
            var viewModel = new ListByTagIdViewModel()
            {
                Data = data,
                LabelVm = label
            };
            return View(viewModel);
        }

        #region AJAX Methods
        public async Task<IActionResult> GetCommentByKnowledgeBaseId(int knowledgeBaseId)
        {
            var data = await _knowledgeBaseApiClient.GetCommentsTree(knowledgeBaseId);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewComment([FromForm] PostCommentVm request)
        {
            var result = await _knowledgeBaseApiClient.PostComment(request);
            if (result != null)
                return Ok(result);
            return BadRequest();
        }


        [HttpPost]
        public async Task<IActionResult> PostVote(int knowledgeBaseId)
        {
            var result = await _knowledgeBaseApiClient.PostVote(knowledgeBaseId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostReport([FromForm] PostReportVm request)
        {
            var result = await _knowledgeBaseApiClient.PostReport(request);
            return Ok(result);
        }
        #endregion AJAX Methods
    }
}
