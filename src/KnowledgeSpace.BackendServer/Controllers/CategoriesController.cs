using KnowledgeSpace.BackendServer.Authorization;
using KnowledgeSpace.BackendServer.Constants;
using KnowledgeSpace.BackendServer.Data;
using KnowledgeSpace.BackendServer.Data.Entities;
using KnowledgeSpace.BackendServer.Helpers;
using KnowledgeSpace.BackendServer.Services;
using KnowledgeSpace.ViewModel;
using KnowledgeSpace.ViewModel.Contents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeSpace.BackendServer.Controllers
{
    public class CategoriesController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly ICacheService _cacheService;
        public CategoriesController(ApplicationDbContext context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategories()
        {
            var cachedData = await _cacheService.GetAsync<List<CategoryVm>>(CacheConstants.Categories);
            if (cachedData == null)
            {
                var categories = _context.Categories;
                var categoriesVms = await categories.Select(c => new CategoryVm()
                {
                    Id = c.Id,
                    Name = c.Name,
                    SortOrder = c.SortOrder,
                    ParentId = c.ParentId,
                    NumberOfTickets = c.NumberOfTickets,
                    SeoDescription = c.SeoDescription,
                    SeoAlias = c.SeoAlias
                }).ToListAsync();
                await _cacheService.SetAsync(CacheConstants.Categories, categoriesVms);
                cachedData = categoriesVms;
            }
            return Ok(cachedData);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound(new ApiNotFoundResponse($"Category with id: {id} is not found"));

            var categoryVm = new CategoryVm()
            {
                Id = category.Id,
                Name = category.Name,
                SeoAlias = category.SeoAlias,
                SeoDescription = category.SeoDescription,
                SortOrder = category.SortOrder,
                ParentId = category.ParentId,
                NumberOfTickets= category.NumberOfTickets
            };
            return Ok(categoryVm);
        }

        [HttpGet("filter")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategories(string filter, int pageIndex, int pageSize)
        {
            var query = _context.Categories.AsQueryable();
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Name.Contains(filter));
            }
            var totalRecords = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CategoryVm()
                {
                    Id = c.Id,
                    Name = c.Name,
                    SeoAlias = c.SeoAlias,
                    SeoDescription = c.SeoDescription,
                    ParentId = c.ParentId,
                    NumberOfTickets = c.NumberOfTickets
                })
                .ToListAsync();

            var pagination = new Pagination<CategoryVm>
            {
                Items = items,
                TotalRecords = totalRecords,
            };
            return Ok(pagination);
        }

        [HttpPost]
        [ClaimRequirement(FunctionCode.CONTENT_CATEGORY, CommandCode.CREATE)]
        [ApiValidationFilter]
        public async Task<IActionResult> PostCategory([FromBody] PostCategoryVm categoryVm)
        {
            var category = new Category()
            {
                Name = categoryVm.Name,
                SeoAlias = categoryVm.SeoAlias,
                SeoDescription = categoryVm.SeoDescription,
                ParentId = categoryVm.ParentId,
                SortOrder = categoryVm.SortOrder
            };
            _context.Categories.Add(category);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return CreatedAtAction(nameof(GetById), new { id = category.Id }, categoryVm);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse("Create category failed"));
            }
        }


        [HttpPut("{id}")]
        [ClaimRequirement(FunctionCode.CONTENT_CATEGORY, CommandCode.UPDATE)]
        [ApiValidationFilter]
        public async Task<IActionResult> PutCategory(int id, [FromBody] PostCategoryVm request)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound(new ApiNotFoundResponse($"Category with id: {id} is not found"));

            if (id == request.ParentId)
            {
                return BadRequest(new ApiBadRequestResponse("Category cannot be a child itself."));
            }

            category.Name = request.Name;
            category.SeoAlias = request.SeoAlias;
            category.SeoDescription = request.SeoDescription;
            category.SortOrder = request.SortOrder;
            category.ParentId = request.ParentId;

            _context.Categories.Update(category);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                await _cacheService.RemoveAsync(CacheConstants.Categories);
                return NoContent();
            }
            return BadRequest(new ApiBadRequestResponse("Update category failed"));
        }

        [HttpDelete("{id}")]
        [ClaimRequirement(FunctionCode.CONTENT_CATEGORY, CommandCode.DELETE)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound(new ApiNotFoundResponse($"Category with id: {id} is not found"));

            _context.Categories.Remove(category);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                await _cacheService.RemoveAsync("Categories");
                var categoryVm = new CategoryVm()
                {
                    Id = category.Id,
                    Name = category.Name,
                    SeoAlias = category.SeoAlias,
                    SeoDescription = category.SeoDescription,
                    SortOrder = category.SortOrder,
                    ParentId = category.ParentId,
                    NumberOfTickets = category.NumberOfTickets
                };
                return Ok(categoryVm);
            }
            return BadRequest();
        }

    }
}
