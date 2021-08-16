using KnowledgeSpace.BackendServer.Data;
using KnowledgeSpace.BackendServer.Data.Entities;
using KnowledgeSpace.ViewModel;
using KnowledgeSpace.ViewModel.Systems;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeSpace.BackendServer.Controllers
{
    public class UsersController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        public UsersController(UserManager<User> userManager, 
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var user = await _userManager.Users
                .Select(u => new GetUserVm()
                {
                    Id = u.Id,
                    Dob = u.Dob,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    UserName = u.UserName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber
                })
                .ToListAsync();
            return Ok(user);               
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            var userVm = new GetUserVm()
            {
                Id = user.Id,
                Dob = user.Dob,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
            return Ok(userVm);
        }


        [HttpGet("filter")]
        public async Task<IActionResult> GetUser(string filter, int pageIndex, int pageSize)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(
                    x => x.Email.Contains(filter) 
                    || x.UserName.Contains(filter) || x.PhoneNumber.Contains(filter));
            }
            var totalRecords = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1 * pageSize))
                .Take(pageSize)
                .Select(u => new GetUserVm()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Dob = u.Dob,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                })
                .ToListAsync();

            var pagination = new Pagination<GetUserVm>
            {
                Items = items,
                TotalRecords = totalRecords,
            };
            return Ok(pagination);
        }


        [HttpPost]
        public async Task<IActionResult> PostUser(PostUserVm userVm)
        {
            var user = new User()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = userVm.FirstName,
                LastName = userVm.LastName,
                UserName = userVm.UserName,
                Dob = userVm.Dob,              
                Email = userVm.Email,
                PhoneNumber = userVm.PhoneNumber
               
            };
            var result = await _userManager.CreateAsync(user, userVm.Password);
            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(GetById), new { id = user.Id }, userVm);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, [FromBody] PostUserVm userVm)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            user.FirstName = userVm.FirstName;
            user.LastName = userVm.LastName;
            user.Dob = userVm.Dob;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return NoContent();
            }
            return BadRequest(result.Errors);
        }

        [HttpPut("{id}/change-password")]
        public async Task<IActionResult> PutPasswordUser(string id, [FromBody] PutPasswordUserVm request)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (result.Succeeded)
            {
                return NoContent();
            }
            return BadRequest(result.Errors);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                var userVm = new GetUserVm()
                {
                    Id = user.Id,
                    Dob = user.Dob,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                };
                return Ok(userVm);
            }
            return BadRequest(result.Errors);
        }

        [HttpGet("{userId}/menu")]
        public async Task<IActionResult> GetMenuByUserPermission(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            var query = from f in _context.Functions
                        join p in _context.Permissions
                            on f.Id equals p.FunctionId
                        join r in _roleManager.Roles on p.RoleId equals r.Id
                        join a in _context.Commands
                            on p.CommandId equals a.Id
                        where roles.Contains(r.Name) && a.Id == "VIEW"
                        select new FunctionVm
                        {
                            Id = f.Id,
                            Name = f.Name,
                            Url = f.Url,
                            ParentId = f.ParentId,
                            SortOrder = f.SortOrder,
                        };
            var data = await query.Distinct()
                .OrderBy(x => x.ParentId)
                .ThenBy(x => x.SortOrder)
                .ToListAsync();
            return Ok(data);
        }
    }
}
