using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface ITrainerService
    {
        //GET All
        Task<IEnumerable<TrainerViewModel>> GetAllAsync(CancellationToken tk = default);

        // Get Trainer Details
        Task<TrainerViewModel> GetTrainerDetailsByIdAsync(int trainerId, CancellationToken tk = default);

        // Create Member
        Task<bool> CreateTrainerAsync(CreateTrainerViewModel trainer, CancellationToken tk = default);

        //Update Member
        Task<TrainerToUpdateViewModel> GetTrainerToUpdateAsync(int trainerId, CancellationToken tk = default);
        Task<bool> UpdateTrainerAsync(int trainerId, TrainerToUpdateViewModel model, CancellationToken tk = default);

        //Delete Trainer
        Task<bool> DeleteTrainerAsync(int trainerId, CancellationToken tk = default);
    }
}
