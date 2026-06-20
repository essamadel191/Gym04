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
    }
}
