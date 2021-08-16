using KnowledgeSpace.BackendServer.Data;
using KnowledgeSpace.ViewModel.Systems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeSpace.BackendServer.Controllers
{
    public class CommandsController : BaseController
    {
        private readonly ApplicationDbContext _context;
        public CommandsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCommands()
        {
            var command = _context.Commands;
            var commandVm = await command.Select( c => new CommandVm()
            {
                Id = c.Id,
                Name = c.Name
            }).ToListAsync();
            return Ok(commandVm);
        }

    }
}
