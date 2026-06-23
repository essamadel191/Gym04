using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagementBLL.ViewModels.PlanViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IPlanService
    {
        //GET All
        Task<IEnumerable<PlanViewModel>> GetAllAsync(CancellationToken tk = default);


        // Get Plan Details
        Task<PlanViewModel> GetPlanDetailsByIdAsync(int planId, CancellationToken tk = default);

        //Update Plan
        Task<UpdatePlanViewModel> GetPlanToUpdateAsync(int planId, CancellationToken tk = default);
        Task<bool> UpdatePlanAsync(int planId, UpdatePlanViewModel model, CancellationToken tk = default);

        //Change Plan Status
        Task<bool> ChangePlanStatusAsync(int planId, CancellationToken tk = default);
    }
}
