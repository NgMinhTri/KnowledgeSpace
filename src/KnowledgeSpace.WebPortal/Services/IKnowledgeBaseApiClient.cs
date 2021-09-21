﻿using KnowledgeSpace.ViewModel;
using KnowledgeSpace.ViewModel.Contents;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnowledgeSpace.WebPortal.Services
{
    public interface IKnowledgeBaseApiClient
    {
        Task<List<KnowledgeBaseQuickVm>> GetPopularKnowledgeBases(int take);
        Task<List<KnowledgeBaseQuickVm>> GetLatestKnowledgeBases(int take);
        Task<List<LabelVm>> GetPopularLabels(int take);
        Task<Pagination<KnowledgeBaseQuickVm>> GetKnowledgeBasesByCategoryId(int categoryId, int pageIndex, int pageSize);
        Task<KnowledgeBaseVm> GetKnowledgeBaseDetail(int id);
        Task<List<LabelVm>> GetLabelsByKnowledgeBaseId(int id);
        Task<Pagination<KnowledgeBaseQuickVm>> SearchKnowledgeBase(string keyword, int pageIndex, int pageSize);

    }
}