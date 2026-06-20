using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IGenericRepository<Member> _memberRepo;
        public MemberService(IGenericRepository<Member> memberRepo)
        {
            _memberRepo = memberRepo;
        }


        public async Task<IEnumerable<MemberViewModel>> GetAllAsync(CancellationToken tk = default)
        {
            var members = await _memberRepo.GetAllAsync(tk: tk);

            if (!members.Any())
            {
                return [];
            }

            List<MemberViewModel> memberVMs = new List<MemberViewModel>();
            foreach (var member in members)
            {
                var memberVM = new MemberViewModel()
                {
                    Name = member.Name,
                    Photo = member.Photo,
                    Email = member.Email,
                    Phone = member.Phone,
                    Gender = member.Gender.ToString()
                };
                memberVMs.Add(memberVM);
            }

            return memberVMs;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model,CancellationToken tk)
        {
            var emailExist = await _memberRepo.AnyAsync(x => x.Email == model.Email);
            var phoneExist = await _memberRepo.AnyAsync(x => x.Phone == model.Phone);

            if (emailExist || phoneExist) return false;

            var member = new Member()
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Address = new Address()
                {
                    BuildingNumber = model.BuildingNumber,
                    City = model.City,
                    Street = model.Street
                },
                HealthRecord = new HealthRecord()
                {
                    Height = model.HealthRecordViewModel.Height,
                    Weight = model.HealthRecordViewModel.Weight,
                    BloodType = model.HealthRecordViewModel.BloodType,
                    Note = model.HealthRecordViewModel.Note
                }
            };

            var result = await _memberRepo.AddAsync(member);

            // result > 0 if it's successfully added 
            return result > 0;
        }
    }
}
