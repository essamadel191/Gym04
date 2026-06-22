using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.BLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<PlanViewModel>> GetAllAsync(CancellationToken tk = default)
        {
            var plans = await _unitOfWork.GetRepository<Plan>().GetAllAsync();

            if (!plans.Any()) return [];

            List<PlanViewModel> planViewModel = new List<PlanViewModel>();

            foreach (var plan in plans)
            {
                PlanViewModel planViewModelItem = new PlanViewModel()
                {
                    Id = plan.Id,
                    Name = plan.Name,
                    Description = plan.Description,
                    DurationDays = plan.DurationDays,
                    Price = plan.Price,
                    IsActive = plan.IsActive,
                };
                planViewModel.Add(planViewModelItem);
            }

            return planViewModel;
        }

        public async Task<PlanViewModel> GetPlanDetailsByIdAsync(int planId, CancellationToken tk = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(planId, tk);
            if (plan is null) return null;

            var planViewModel = new PlanViewModel()
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                IsActive = plan.IsActive,
                Price = plan.Price,
            };

            return planViewModel;
        }
        //public Task<bool> CreatePlanAsync(CreateMemberViewModel Plan, CancellationToken tk)
        //{
        //    throw new NotImplementedException();
        //}
        public async Task<UpdatePlanViewModel> GetPlanToUpdateAsync(int planId, CancellationToken tk = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(planId, tk);
            if (plan == null) return null;

            var updatePlanViewModel = new UpdatePlanViewModel()
            {
                PlanName = plan.Name,
                Description = plan.Description,
                Price = plan.Price,
                DurationDays = plan.DurationDays,
            };

            return updatePlanViewModel;
        }

        public async Task<bool> UpdatePlanAsync(int planId, UpdatePlanViewModel model, CancellationToken tk = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(planId, tk);
            if (plan == null) return false;

            var hasMembership = await _unitOfWork.GetRepository<Membership>().AnyAsync(x => x.PlanId == plan.Id);
            if (hasMembership) return false;

            plan.Description = model.Description;
            plan.DurationDays = model.DurationDays;
            plan.Price = model.Price;

            var result = await _unitOfWork.SaveChangesAsync(tk);
            return result > 0;
        }


        public async Task<bool> ChangePlanStatusAsync(int planId, CancellationToken tk = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(planId,tk);
            if(plan == null) return false;
        
            if(plan.IsActive == true)
            {
                var hasMembership = await _unitOfWork.GetRepository<Membership>().AnyAsync(x => x.PlanId == planId);
                if(hasMembership) return false;
            }
            plan.IsActive = !plan.IsActive;

            var result = await _unitOfWork.SaveChangesAsync(tk);
            return result > 0;
        }
    }
}
