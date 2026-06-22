using Microsoft.AspNetCore.Mvc;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DAL.Models;
using GymManagement.BLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;

namespace Gym04.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;
        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }

        // GET :: BaseUrl /Plan/Index
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken token)
        {
            var plans = await _planService.GetAllAsync(tk: token);

            return View(plans);
        }

        // GET :: BaseUrl /Plan/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken token)
        {
            var plan = await _planService.GetPlanDetailsByIdAsync(id, token);
            if (plan == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        // GET :: BaseUrl /Plan/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken token)
        {
            var plan = await _planService.GetPlanToUpdateAsync(id, token);
            if(plan is null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        // Post :: BaseUrl /Plan/Edit/{plan}
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id,UpdatePlanViewModel model,CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _planService.UpdatePlanAsync(id, model, token);

            if (result) TempData["SuccessMessage"] = "Member Updated Successfully !";
            else TempData["ErrorMessage"] = "Failed to Updated Member";

            return RedirectToAction(nameof(Index));
        }

        //Post :: BaseUrl /Plan/Activate/{id}
        [HttpPost]
        public async Task<IActionResult> Activate([FromRoute] int id,CancellationToken tk)
        {
            var result = await _planService.ChangePlanStatusAsync(id, tk);

            if (result) TempData["SuccessMessage"] = "Member Updated Successfully !";
            else TempData["ErrorMessage"] = "Failed to Updated Member";

            return RedirectToAction(nameof(Index));
        }
    }
}
