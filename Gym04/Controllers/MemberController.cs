using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Gym04.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;
        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        #region Get Memebers

        // GET :: BaseUrl/Memebr/Index
        public async Task<IActionResult> Index(CancellationToken tk)
        {
            var member = await _memberService.GetAllAsync(tk);
            return View(member);
        }
        // GET :: BaseUrl/Member/Details/{id} => Get Specific Memeber
        public async Task<IActionResult> MemberDetails(int id, CancellationToken tk)
        {
            var member = await _memberService.GetMemberDetailsByIdAsync(id, tk);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member Not Found!";

            }
            return View(member);
        }

        // GET :: BaseUrl/Member/HealthRecordDetials/{id} => Get Data of Specific Memeber with Health Record

        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken tk)
        {
            var healthRecord = await _memberService.GetMemberHealthRecordAsync(id, tk);
            if (healthRecord == null)
            {
                TempData["ErrorMessage"] = "Health Record Not Found!";
            }
            return View(healthRecord);
        }

        #endregion
        #region Create Member

        // GET :: BaseUrl/Memeber/Create => Show Empty Form
        [HttpGet]
        public IActionResult Create() => View();

        // Post :: BaseUrl/Member/CreateMember/{Member} => Submit Form
        //CreateMember
        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model, CancellationToken tk)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Create), model);
            }

            var result = await _memberService.CreateMemberAsync(model, tk);

            if (result) TempData["SuccessMessage"] = "Member Created Sucessfully";
            else TempData["ErrorMessage"] = "Failed To Create !";

            return RedirectToAction(nameof(Index));

        }

        #endregion
        #region Edit Member

        // GET :: BaseUrl/Member/Edit/{id} => Show Edit Form
        [HttpGet]
        public async Task<IActionResult> EditMember(int id, CancellationToken tk)
        {
            var member = await _memberService.GetMemberToUpdateAsync(id, tk);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found !";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        // Post :: BaseUrl/Member/Edit/{Member} => Submit Edit Form
        [HttpPost]
        public async Task<IActionResult> EditMember([FromRoute] int id, MemberToUpdateViewMode model, CancellationToken tk)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _memberService.UpdateMemberAsync(id, model, tk);
            if (result) TempData["SuccessMessage"] = "Member Updated Successfully !";
            else TempData["ErrorMessage"] = "Failed to Updated Member";

            return RedirectToAction(nameof(Index));
        }

        #endregion
        #region Delete Memeber

        // GET :: BaseUrl/Memeber/Delete/{id} => Show Validation Page
        public async Task<IActionResult> Delete(int id, CancellationToken tk)
        {
            var member = await _memberService.GetMemberDetailsByIdAsync(id, tk);
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
            var result = await _memberService.DeleteMemberAsync(id, tk);

            if (result) TempData["SuccessMessage"] = "Member Deleted Successfully";
            else TempData["ErrorMessage"] = "Failed To Delete Member !";

            return RedirectToAction(nameof(Index));
        }

        #endregion


    }
}
