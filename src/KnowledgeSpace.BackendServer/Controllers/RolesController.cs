﻿using KnowledgeSpace.ViewModel;
using KnowledgeSpace.ViewModel.Systems;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeSpace.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        // URL: http://localhost:5001/api/roles
        [HttpPost]
        public async Task<IActionResult> PostRole(RoleVm roleVm)
        {
            var role = new IdentityRole()
            {
                Id = roleVm.Id,
                Name = roleVm.Name,
                NormalizedName = roleVm.Name.ToUpper()
            };
            var result = await _roleManager.CreateAsync(role);
            if(result.Succeeded)
            {
                return CreatedAtAction(nameof(GetById), new { id = role.Id }, roleVm);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var role = await _roleManager.Roles
            .Select(r => new RoleVm()
            {
                Id = r.Id,
                Name = r.Name
            })
            .ToListAsync();
            return Ok(role);
        }

        //URL: GET: http://localhost:5001/api/roles/?filter={filter}&pageIndex=1&pageSize=10
        [HttpGet("filter")]
        public async Task<IActionResult> GetRoles(string filter, int pageIndex, int pageSize)
        {
            var query = _roleManager.Roles;
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Id.Contains(filter) || x.Name.Contains(filter));
            }
            var totalRecords = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1 * pageSize))
                .Take(pageSize)
                .Select(r => new RoleVm()
                {
                    Id = r.Id,
                    Name = r.Name
                })
                .ToListAsync();

            var pagination = new Pagination<RoleVm>
            {
                Items = items,
                TotalRecords = totalRecords,
            };
            return Ok(pagination);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound();
            var roleVm = new RoleVm()
            {
                Id = role.Id,
                Name = role.Name,
            };
            return Ok(roleVm);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(string id, [FromBody]RoleVm roleVm)
        {
            if (id != roleVm.Id)
                return BadRequest();

            var role = await _roleManager.FindByIdAsync(id);
            if(role == null)
                return NotFound();

            role.Name = roleVm.Name;
            role.Name = roleVm.Name.ToUpper();

            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return NoContent();
            }
            return BadRequest(result.Errors);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound();
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                var roleVm = new RoleVm()
                {
                    Id = role.Id,
                    Name = role.Name
                };
                return Ok(roleVm);
            }
            return BadRequest(result.Errors);
        }
    }
}
