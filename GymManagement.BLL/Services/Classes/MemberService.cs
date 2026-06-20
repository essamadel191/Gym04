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
        private readonly IGenericRepository<Membership> _membershipRepo;
        private readonly IGenericRepository<Plan> _planRepo;
        private readonly IGenericRepository<HealthRecord> _healthRepo;
        private readonly IGenericRepository<Booking> _bookingRepo;

        public MemberService(IGenericRepository<Member> memberRepo, IGenericRepository<Membership> membershipRepo
            , IGenericRepository<Plan> planRepo, IGenericRepository<HealthRecord> healthRepo, IGenericRepository<Booking> bookingRepo)
        {
            _memberRepo = memberRepo;
            _membershipRepo = membershipRepo;
            _planRepo = planRepo;
            _healthRepo = healthRepo;
            _bookingRepo = bookingRepo;
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
                    Id = member.Id,
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

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken tk)
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

        public async Task<MemberViewModel> GetMemberDetailsByIdAsync(int memberId, CancellationToken tk = default)
        {
            var member = await _memberRepo.GetByIdAsync(memberId, tk);

            if (member == null) return null;

            var model = new MemberViewModel()
            {
                Id = member.Id,
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                Gender = member.Gender.ToString(),
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Address = $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}",
            };


            var ActiveMembership = await _membershipRepo.FirstOrDefaultAsync(x => x.MemberId == memberId && x.EndDate > DateTime.Now, tk: tk);

            if (ActiveMembership != null)
            {
                // Plan Name
                var plan = await _planRepo.GetByIdAsync(ActiveMembership.PlanId, tk);
                model.PlanName = plan.Name;
                model.MembershipStartDate = ActiveMembership.CreatedAt.ToString();
                model.MembershipEndDate = ActiveMembership.EndDate.ToString();
            }

            return model;
        }

        public async Task<HealthRecordViewModel> GetMemberHealthRecordAsync(int memberId, CancellationToken tk = default)
        {
            var record = await _healthRepo.FirstOrDefaultAsync(x => x.MemberId == memberId, tk: tk);

            if (record is null) return null;

            return new HealthRecordViewModel()
            {
                Weight = record.Weight,
                Height = record.Height,
                BloodType = record.BloodType,
                Note = record.Note
            };

        }

        public async Task<MemberToUpdateViewMode> GetMemberToUpdateAsync(int memberId, CancellationToken tk = default)
        {
            var member = await _memberRepo.GetByIdAsync(memberId, tk);
            if (member == null) return null;
            return new MemberToUpdateViewMode()
            {
                Photo = member.Photo,
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                City = member.Address.City,
                Street = member.Address.Street,
                BuildingNumber = member.Address.BuildingNumber
            };

        }

        public async Task<bool> UpdateMemberAsync(int memberId, MemberToUpdateViewMode model, CancellationToken tk = default)
        {
            var member = await _memberRepo.GetByIdAsync(memberId, tk);

            var emailExist = await _memberRepo.AnyAsync(x => x.Email == model.Email && x.Id != memberId);
            var phoneExist = await _memberRepo.AnyAsync(x => x.Phone == model.Phone && x.Id != memberId);

            if (emailExist || phoneExist) return false;

            member.Email = model.Email;
            member.Phone = model.Phone;
            member.Address.BuildingNumber = model.BuildingNumber;
            member.Address.City = model.City;
            member.Address.Street = model.Street;
            member.UpdatedAt = DateTime.Now;

            var result = await _memberRepo.UpdateAsync(member);

            return result > 0;
        }

        public async Task<bool> DeleteMemberAsync(int memberId, CancellationToken tk = default)
        {
            // If member has active booking
            var member = await _memberRepo.GetByIdAsync(memberId, tk);
            if (member is null) return false;

            var HasActiveBooking = await _bookingRepo.AnyAsync(x => x.MemberId == memberId && x.Session.StartDate > DateTime.Now);//exception

            if (HasActiveBooking) return false;

            var result = await _memberRepo.DeleteAsync(member);

            return result > 0;

        }
    }
}
