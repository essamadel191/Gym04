using Microsoft.AspNetCore.Mvc;
using GymManagement.DAL.Context;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DAL.Models;

namespace Gym04.Controllers
{
    public class PlanController : Controller
    {
        private readonly IGenericRepository<Plan> _planRepository;
        public PlanController(IGenericRepository<Plan> planRepository)
        {
            _planRepository = planRepository;
        }

        // GET :: BaseUrl /Plan/Index
        public async Task<IActionResult> Index(CancellationToken token)
        {
            var plans = await _planRepository.GetAllAsync(tk: token);

            return View(plans);
        }

        // GET :: BaseUrl /Plan/Details/{id}
        public async Task<IActionResult> Details(int id, CancellationToken token) 
        {
            var plan = await _planRepository.GetByIdAsync(id, token);
            if(plan == null) 
            {
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
    }
}
