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
        // GET :: BaseUrl/Member/HealthRecordDetials/{id} => Get Data of Specific Memeber with Health Record

        #endregion
        #region Create Member

        // GET :: BaseUrl/Memeber/Create => Show Empty Form
        [HttpGet]
        public IActionResult Create() => View();

        // Post :: BaseUrl/Member/CreateMember/{Member} => Submit Form
        //CreateMember
        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model,CancellationToken tk)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Create) , model);
            }

            var result = await _memberService.CreateMemberAsync(model, tk);

            if (result) TempData["SuccessMessage"] = "Member Created Sucessfully";
            else TempData["ErrorMessage"] = "Failed To Create !";

            return RedirectToAction(nameof(Index)) ;
        
        }

        #endregion
        #region Edit Member

        // GET :: BaseUrl/Member/Edit/{id} => Show Edit Form
        // Post :: BaseUrl/Member/Edit/{Member} => Submit Edit Form

        #endregion
        #region Delete Memeber

        // GET :: BaseUrl/Memeber/Delete/{id} => Show Validation Page

        #endregion


    }
}
