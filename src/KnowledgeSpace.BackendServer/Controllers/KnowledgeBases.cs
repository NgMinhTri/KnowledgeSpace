using KnowledgeSpace.BackendServer.Data;
using KnowledgeSpace.BackendServer.Data.Entities;
using KnowledgeSpace.BackendServer.Services;
using KnowledgeSpace.ViewModel;
using KnowledgeSpace.ViewModel.Contents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeSpace.BackendServer.Controllers
{
    public class KnowledgeBases : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly ISequenceService _sequenceService;
        public KnowledgeBases(ApplicationDbContext context, ISequenceService sequenceService)
        {
            _context = context;
            _sequenceService = sequenceService;
        }

        #region KnowledgeBase
        [HttpGet]
        public async Task<IActionResult> GetKnowledgeBases()
        {
            var knowledgeBase = _context.KnowledgeBases;
            var knowledgeBaseVm = await knowledgeBase.Select(k => new KnowledgeBaseQuickVm()
            {
                Id = k.Id,
                CategoryId = k.CategoryId,
                Description = k.Description,
                SeoAlias = k.SeoAlias,
                Title = k.Title
            }).ToListAsync();
            return Ok(knowledgeBaseVm);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetKnowledgeBaseById(int id)
        {
            var knowledgeBase = await _context.KnowledgeBases.FindAsync(id);
            if(knowledgeBase == null)
                return NotFound();

            var knowledgeBaseVm = new KnowledgeBaseVm()
            {
                Id = knowledgeBase.Id,

                CategoryId = knowledgeBase.CategoryId,

                Title = knowledgeBase.Title,

                SeoAlias = knowledgeBase.SeoAlias,

                Description = knowledgeBase.Description,

                Environment = knowledgeBase.Environment,

                Problem = knowledgeBase.Problem,

                StepToReproduce = knowledgeBase.StepToReproduce,

                ErrorMessage = knowledgeBase.ErrorMessage,

                Workaround = knowledgeBase.Workaround,

                Note = knowledgeBase.Note,

                OwnerUserId = knowledgeBase.OwnerUserId,

                Labels = knowledgeBase.Labels,

                CreateDate = knowledgeBase.CreateDate,

                LastModifiedDate = knowledgeBase.LastModifiedDate,

                NumberOfComments = knowledgeBase.CategoryId,

                NumberOfVotes = knowledgeBase.CategoryId,

                NumberOfReports = knowledgeBase.CategoryId,

            };
            return Ok(knowledgeBaseVm);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetKnowledgeBasesPaging(string filter, int pageIndex, int pageSize)
        {
            var query = _context.KnowledgeBases.AsQueryable();
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Title.Contains(filter));
            }
            var totalRecords = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1 * pageSize))
                .Take(pageSize)
                .Select(u => new KnowledgeBaseQuickVm()
                {
                    Id = u.Id,
                    CategoryId = u.CategoryId,
                    Description = u.Description,
                    SeoAlias = u.SeoAlias,
                    Title = u.Title
                })
                .ToListAsync();

            var pagination = new Pagination<KnowledgeBaseQuickVm>
            {
                Items = items,
                TotalRecords = totalRecords,
            };
            return Ok(pagination);
        }


        [HttpPost]
        public async Task<IActionResult> PostKnowledgeBase([FromBody] PostKnowledgeBaseVm request)
        {
            var knowledgeBase = new KnowledgeBase()
            {
                CategoryId = request.CategoryId,

                Title = request.Title,

                SeoAlias = request.SeoAlias,

                Description = request.Description,

                Environment = request.Environment,

                Problem = request.Problem,

                StepToReproduce = request.StepToReproduce,

                ErrorMessage = request.ErrorMessage,

                Workaround = request.Workaround,

                Note = request.Note,

                Labels = request.Labels,
            };
            knowledgeBase.Id = await _sequenceService.GetKnowledgeBaseNewId();
            _context.KnowledgeBases.Add(knowledgeBase);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return CreatedAtAction(nameof(GetKnowledgeBaseById), new { id = knowledgeBase.Id }, request);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutKnowledgeBase(int id, [FromBody] PostKnowledgeBaseVm request)
        {
            var knowledgeBase = await _context.KnowledgeBases.FindAsync(id);
            if (knowledgeBase == null)
                return NotFound();

            knowledgeBase.CategoryId = request.CategoryId;

            knowledgeBase.Title = request.Title;

            knowledgeBase.SeoAlias = request.SeoAlias;

            knowledgeBase.Description = request.Description;

            knowledgeBase.Environment = request.Environment;

            knowledgeBase.Problem = request.Problem;

            knowledgeBase.StepToReproduce = request.StepToReproduce;

            knowledgeBase.ErrorMessage = request.ErrorMessage;

            knowledgeBase.Workaround = request.Workaround;

            knowledgeBase.Note = request.Note;

            knowledgeBase.Labels = request.Labels;

            _context.KnowledgeBases.Update(knowledgeBase);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKnowledgeBase(string id)
        {
            var knowledgeBase = await _context.KnowledgeBases.FindAsync(id);
            if (knowledgeBase == null)
                return NotFound();

            _context.KnowledgeBases.Remove(knowledgeBase);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                var knowledgeBasevm = new KnowledgeBaseVm()
                {
                    Id = knowledgeBase.CategoryId,

                    CategoryId = knowledgeBase.CategoryId,

                    Title = knowledgeBase.Title,

                    SeoAlias = knowledgeBase.SeoAlias,

                    Description = knowledgeBase.Description,

                    Environment = knowledgeBase.Environment,

                    Problem = knowledgeBase.Problem,

                    StepToReproduce = knowledgeBase.StepToReproduce,

                    ErrorMessage = knowledgeBase.ErrorMessage,

                    Workaround = knowledgeBase.Workaround,

                    Note = knowledgeBase.Note,

                    OwnerUserId = knowledgeBase.OwnerUserId,

                    Labels = knowledgeBase.Labels,

                    CreateDate = knowledgeBase.CreateDate,

                    LastModifiedDate = knowledgeBase.LastModifiedDate,

                    NumberOfComments = knowledgeBase.CategoryId,

                    NumberOfVotes = knowledgeBase.CategoryId,

                    NumberOfReports = knowledgeBase.CategoryId,
                };
                return Ok(knowledgeBasevm);
            }
            return BadRequest();
        }
        #endregion

        #region Comment

        [HttpGet("{knowledgeBaseId}/comments/filter")]
        public async Task<IActionResult> GetCommentsPaging(int knowledgeBaseId, string filter, int pageIndex, int pageSize)
        {
            var query = _context.Comments.Where(x => x.KnowledgeBaseId == knowledgeBaseId).AsQueryable();
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Content.Contains(filter));
            }
            var totalRecords = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1 * pageSize))
                .Take(pageSize)
                .Select(c => new CommentVm()
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreateDate = c.CreateDate,
                    KnowledgeBaseId = c.KnowledgeBaseId,
                    LastModifiedDate = c.LastModifiedDate,
                    OwnwerUserId = c.OwnwerUserId
                })
                .ToListAsync();

            var pagination = new Pagination<CommentVm>
            {
                Items = items,
                TotalRecords = totalRecords,
            };
            return Ok(pagination);
        }

        [HttpGet("{knowledgeBaseId}/comments/{commentId}")]
        public async Task<IActionResult> GetCommentDetail(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null)
                return NotFound();

            var commentVm = new CommentVm()
            {
                Id = comment.Id,
                Content = comment.Content,
                CreateDate = comment.CreateDate,
                KnowledgeBaseId = comment.KnowledgeBaseId,
                LastModifiedDate = comment.LastModifiedDate,
                OwnwerUserId = comment.OwnwerUserId
            };
            return Ok(commentVm);
        }

        [HttpPost("{knowledgeBaseId}/comments")]
        public async Task<IActionResult> PostComment(int knowledgeBaseId, [FromBody] PostCommentVm request)
        {
            var comment = new Comment()
            {
                Content = request.Content,
                KnowledgeBaseId = request.KnowledgeBaseId,
                OwnwerUserId = string.Empty/*TODO: GET USER FROM CLAIM*/,
            };
            _context.Comments.Add(comment);

            var knowledgeBase = await _context.KnowledgeBases.FindAsync(knowledgeBaseId);
            if (knowledgeBase != null)
                return BadRequest();
            knowledgeBase.NumberOfComments = knowledgeBase.NumberOfVotes.GetValueOrDefault(0) + 1;
            _context.KnowledgeBases.Update(knowledgeBase);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return CreatedAtAction(nameof(GetCommentDetail), new { id = knowledgeBaseId, commentId = comment.Id }, request);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("{knowledgeBaseId}/comments/{commentId}")]
        public async Task<IActionResult> PutComment(int commentId, [FromBody] PostCommentVm request)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null)
                return NotFound();
            if (comment.OwnwerUserId != User.Identity.Name)
                return Forbid();

            comment.Content = request.Content;
            _context.Comments.Update(comment);

            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpDelete("{knowledgeBaseId}/comments/{commentId}")]
        public async Task<IActionResult> DeleteComment(int knowledgeBaseId, int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null)
                return NotFound();

            _context.Comments.Remove(comment);

            var knowledgeBase = await _context.KnowledgeBases.FindAsync(knowledgeBaseId);
            if (knowledgeBase != null)
                return BadRequest();
            knowledgeBase.NumberOfComments = knowledgeBase.NumberOfVotes.GetValueOrDefault(0) - 1;
            _context.KnowledgeBases.Update(knowledgeBase);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                var commentVm = new CommentVm()
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    CreateDate = comment.CreateDate,
                    KnowledgeBaseId = comment.KnowledgeBaseId,
                    LastModifiedDate = comment.LastModifiedDate,
                    OwnwerUserId = comment.OwnwerUserId
                };
                return Ok(commentVm);
            }
            return BadRequest();
        }
        #endregion
    }
}
