using GymManagement.BLL.Services.Classes;
using GymManagement.BLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Gym04.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;
        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }
        // GET :: BaseUrl/Trainer/Index
        public async Task<IActionResult> Index(CancellationToken tk)
        {
            var triner = await _trainerService.GetAllAsync(tk);
            return View(triner);
        }

        public async Task<IActionResult> Details(int id,CancellationToken tk)
        {
            var trainer = await _trainerService.GetTrainerDetailsByIdAsync(id);
            if (trainer == null) return RedirectToAction(nameof(Index));

            return View(trainer);
        }

        // GET :: BaseUrl/Trainer/Create
        [HttpGet]
        public IActionResult Create() => View();

        // Post :: BaseUrl/Trainer/Create
        [HttpPost]
        public async Task<IActionResult> Create(CreateTrainerViewModel model, CancellationToken tk)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Create), model);
            }

            var result = await _trainerService.CreateTrainerAsync(model, tk);

            if (result) TempData["SuccessMessage"] = "Trainer Created Sucessfully";
            else TempData["ErrorMessage"] = "Failed To Create Trainer!";

            return RedirectToAction(nameof(Index));
        }

        // Get :: BaseUrl/Trainer/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken tk)
        {
            var trainer = await _trainerService.GetTrainerToUpdateAsync(id, tk);
            if (trainer == null) return View(nameof(Index));


            return View(trainer);
        }

        // Post :: BaseUrl/Trainer/Edit/{model}
        [HttpPost]
        public async Task<IActionResult> Edit(int id, TrainerToUpdateViewModel model , CancellationToken tk)
        {
            if(!ModelState.IsValid) return View(nameof(Index));

            var result = await _trainerService.UpdateTrainerAsync(id, model, tk);

            if (result) TempData["SuccessMessage"] = "Trainer Updated Successfully !";
            else TempData["ErrorMessage"] = "Failed to Updated Trainer";

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id, CancellationToken tk)
        {
            var member = await _trainerService.GetTrainerDetailsByIdAsync(id, tk);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id, CancellationToken tk)
        {
            var result = await _trainerService.DeleteTrainerAsync(id, tk);

            if (result) TempData["SuccessMessage"] = "Member Deleted Successfully";
            else TempData["ErrorMessage"] = "Failed To Delete Member !";

            return RedirectToAction(nameof(Index));
        }
    }
}
