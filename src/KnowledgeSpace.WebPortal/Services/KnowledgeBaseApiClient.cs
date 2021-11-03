﻿using KnowledgeSpace.ViewModel;
using KnowledgeSpace.ViewModel.Contents;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace KnowledgeSpace.WebPortal.Services
{
    public class KnowledgeBaseApiClient : BaseApiClient, IKnowledgeBaseApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public KnowledgeBaseApiClient(IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor): base(httpClientFactory, configuration, httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<KnowledgeBaseQuickVm>> GetPopularKnowledgeBases(int take)
        {
            return await GetListAsync<KnowledgeBaseQuickVm>($"/api/knowledgeBases/popular/{take}");
        }

        public async Task<List<KnowledgeBaseQuickVm>> GetLatestKnowledgeBases(int take)
        {
            return await GetListAsync<KnowledgeBaseQuickVm>($"/api/knowledgeBases/latest/{take}");
        }

        public async Task<Pagination<KnowledgeBaseQuickVm>> GetKnowledgeBasesByCategoryId(int categoryId, int pageIndex, int pageSize)
        {
            var apiUrl = $"/api/knowledgeBases/filter?categoryId={categoryId}&pageIndex={pageIndex}&pageSize={pageSize}";
            return await GetAsync<Pagination<KnowledgeBaseQuickVm>>(apiUrl);
        }

        //Hàm lấy ra chi tiết KnowledgeBase
        public async Task<KnowledgeBaseVm> GetKnowledgeBaseDetail(int id)
        {
            return await GetAsync<KnowledgeBaseVm>($"/api/knowledgeBases/{id}");
        }


        public async Task<List<LabelVm>> GetLabelsByKnowledgeBaseId(int id)
        {
            return await GetListAsync<LabelVm>($"/api/knowledgeBases/{id}/labels");
        }

        //Tìm kiếm bài viết theo từ khóa
        public async Task<Pagination<KnowledgeBaseQuickVm>> SearchKnowledgeBase(string keyword, int pageIndex, int pageSize)
        {
            var apiUrl = $"/api/knowledgeBases/filter?filter={keyword}&pageIndex={pageIndex}&pageSize={pageSize}";
            return await GetAsync<Pagination<KnowledgeBaseQuickVm>>(apiUrl);
        }

        public async Task<Pagination<KnowledgeBaseQuickVm>> GetKnowledgeBasesByTagId(string tagId, int pageIndex, int pageSize)
        {
            var apiUrl = $"/api/knowledgeBases/tags/{tagId}?pageIndex={pageIndex}&pageSize={pageSize}";
            return await GetAsync<Pagination<KnowledgeBaseQuickVm>>(apiUrl);
        }

        public async Task<List<CommentVm>> GetRecentComments(int take)
        {
            return await GetListAsync<CommentVm>($"/api/knowledgeBases/comments/recent/{take}");
        }

        public async Task<List<CommentVm>> GetCommentsTree(int knowledgeBaseId)
        {
            return await GetListAsync<CommentVm>($"/api/knowledgeBases/{knowledgeBaseId}/comments/tree");
        }

        public async Task<CommentVm> PostComment(PostCommentVm request)
        {
            return await PostAsync<PostCommentVm, CommentVm>($"/api/knowledgeBases/{request.KnowledgeBaseId}/comments", request);
        }

        public async Task<bool> PostKnowlegdeBase(PostKnowledgeBaseVm request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BackendApiUrl"]);
            using var requestContent = new MultipartFormDataContent();
            if (request.Attachments?.Count > 0)
            {
                foreach (var item in request.Attachments)
                {
                    byte[] data;
                    using (var br = new BinaryReader(item.OpenReadStream()))
                    {
                        data = br.ReadBytes((int)item.OpenReadStream().Length);
                    }
                    ByteArrayContent bytes = new ByteArrayContent(data);
                    requestContent.Add(bytes, "attachments", item.FileName);
                }
            }
            requestContent.Add(new StringContent(request.CategoryId.ToString()), "categoryId");
            requestContent.Add(new StringContent(request.Title.ToString()), "title");
            requestContent.Add(new StringContent(request.Problem.ToString()), "problem");
            requestContent.Add(new StringContent(request.Note.ToString()), "note");
            requestContent.Add(new StringContent(request.Description.ToString()), "description");
            requestContent.Add(new StringContent(request.Environment.ToString()), "environment");
            requestContent.Add(new StringContent(request.StepToReproduce.ToString()), "stepToReproduce");
            requestContent.Add(new StringContent(request.ErrorMessage.ToString()), "errorMessage");
            requestContent.Add(new StringContent(request.Workaround.ToString()), "workaround");
            if (request.Labels?.Length > 0)
            {
                foreach (var label in request.Labels)
                {
                    requestContent.Add(new StringContent(label), "labels");
                }
            }
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync($"/api/knowledgeBases/", requestContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PutKnowlegdeBase(int id, PostKnowledgeBaseVm request)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BackendApiUrl"]);
            using var requestContent = new MultipartFormDataContent();

            if (request.Attachments?.Count > 0)
            {
                foreach (var item in request.Attachments)
                {
                    byte[] data;
                    using (var br = new BinaryReader(item.OpenReadStream()))
                    {
                        data = br.ReadBytes((int)item.OpenReadStream().Length);
                    }
                    ByteArrayContent bytes = new ByteArrayContent(data);
                    requestContent.Add(bytes, "attachments", item.FileName);
                }
            }
            requestContent.Add(new StringContent(request.CategoryId.ToString()), "categoryId");
            requestContent.Add(new StringContent(request.Title.ToString()), "title");
            requestContent.Add(new StringContent(request.Problem.ToString()), "problem");
            requestContent.Add(new StringContent(request.Note.ToString()), "note");
            requestContent.Add(new StringContent(request.Description.ToString()), "description");
            requestContent.Add(new StringContent(request.Environment.ToString()), "environment");
            requestContent.Add(new StringContent(request.StepToReproduce.ToString()), "stepToReproduce");
            requestContent.Add(new StringContent(request.ErrorMessage.ToString()), "errorMessage");
            requestContent.Add(new StringContent(request.Workaround.ToString()), "workaround");
            if (request.Labels?.Length > 0)
            {
                foreach (var label in request.Labels)
                {
                    requestContent.Add(new StringContent(label), "labels");
                }
            }

            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PutAsync($"/api/knowledgeBases/{id}", requestContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateViewCount(int id)
        {
            return await PutAsync<object, bool>($"/api/knowledgeBases/{id}/view-count", null, false);
        }

        public async Task<int> PostVote(int knowledgeBaseId)
        {
            var result = await PostAsync<int, int>($"/api/knowledgeBases/{knowledgeBaseId}/votes", knowledgeBaseId);
            return result;
        }

        public async Task<ReportVm> PostReport(PostReportVm request)
        {
            return await PostAsync<PostReportVm, ReportVm>($"/api/knowledgeBases/{request.KnowledgeBaseId}/reports", request);
        }
    }
}
