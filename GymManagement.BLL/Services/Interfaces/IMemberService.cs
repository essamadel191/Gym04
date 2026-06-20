using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        //GET All
        Task<IEnumerable<MemberViewModel>> GetAllAsync(CancellationToken tk = default);

        // Create Member
        Task<bool> CreateMemberAsync(CreateMemberViewModel member, CancellationToken tk);

        // Get Member Details
        Task<MemberViewModel> GetMemberDetailsByIdAsync (int memberId,CancellationToken tk = default);

        //Get Member Health Record
        Task<HealthRecordViewModel> GetMemberHealthRecordAsync(int memberId, CancellationToken tk = default);

        //Update Member
        Task<MemberToUpdateViewMode> GetMemberToUpdateAsync(int memberId,CancellationToken tk = default);
        Task<bool> UpdateMemberAsync(int memberId, MemberToUpdateViewMode model, CancellationToken tk = default);

        //Delete Member
        Task<bool> DeleteMemberAsync(int memberId, CancellationToken tk = default);

    }
}
