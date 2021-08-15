﻿using KnowledgeSpace.BackendServer.Data;
using KnowledgeSpace.BackendServer.Data.Entities;
using KnowledgeSpace.ViewModel;
using KnowledgeSpace.ViewModel.Contents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeSpace.BackendServer.Controllers
{
    public class CategoriesController : BaseController
    {
        private readonly ApplicationDbContext _context;
        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var category = await _context.Categories.Select(c => new CategoryVm()
            {
                Id = c.Id,
                Name = c.Name,
                SeoAlias = c.SeoAlias,
                SeoDescription = c.SeoDescription,
                SortOrder = c.SortOrder,
                ParentId = c.ParentId,
                NumberOfTickets = c.NumberOfTickets

            }).ToListAsync();
            return Ok(category);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();
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
        public async Task<IActionResult> GetCategories(string filter, int pageIndex, int pageSize)
        {
            var query = _context.Categories.AsQueryable();
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Name.Contains(filter));
            }
            var totalRecords = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1 * pageSize))
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
                return BadRequest();
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, [FromBody] PostCategoryVm request)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();
            if (id == request.ParentId)
            {
                return BadRequest("Category cannot be a child itself.");
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
                return NoContent();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
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