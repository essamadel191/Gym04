using GymManagement.BLL.Services.Interfaces;
using GymManagement.DAL.Models;
using GymManagement.DAL.Models.Enums;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.BLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<TrainerViewModel>> GetAllAsync(CancellationToken tk = default)
        {
            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync();
            if (!trainers.Any()) return [];

            List<TrainerViewModel> trainerViewModels = new List<TrainerViewModel>();
            foreach (var trainer in trainers)
            {
                var trainerViewModel = new TrainerViewModel()
                {
                    Id = trainer.Id,
                    Name = trainer.Name,
                    Email = trainer.Email,
                    Phone = trainer.Phone,
                    DateOfBirth = trainer.DateOfBirth.ToShortDateString(),
                    Gender = trainer.Gender.ToString(),
                    Speciality = trainer.Specility.ToString(),
                };
                trainerViewModels.Add(trainerViewModel);
            }

            return trainerViewModels;
        }
        public async Task<TrainerViewModel> GetTrainerDetailsByIdAsync(int trainerId, CancellationToken tk = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId,tk);
            if (trainer == null) return null;

            var trainerViewModel = new TrainerViewModel()
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                DateOfBirth = trainer.DateOfBirth.ToShortDateString(),
                Gender = trainer.Gender.ToString(),
                Speciality = trainer.Specility.ToString(),
                Address = $"{trainer.Address.BuildingNumber} - {trainer.Address.Street} - {trainer.Address.City}" 
            };

            return trainerViewModel;
        }
        public async Task<bool> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken tk = default)
        {
            var emailExist = await _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Email == model.Email);
            var phoneExist = await _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Phone == model.Phone);

            if (emailExist || phoneExist) return false;

            var trainer = new Trainer()
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                Address = new Address()
                {
                    BuildingNumber = model.BuildingNumber,
                    City = model.City,
                    Street = model.Street
                },
                Specility = model.Speciality
            };

            _unitOfWork.GetRepository<Trainer>().AddAsync(trainer);
            var result = await _unitOfWork.SaveChangesAsync(tk);

            return result > 0;
        }
        public async Task<TrainerToUpdateViewModel> GetTrainerToUpdateAsync(int trainerId, CancellationToken tk = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, tk);
            if (trainer == null) return null;

            var model = new TrainerToUpdateViewModel ()
            {
                Name=trainer.Name,
                Email=trainer.Email,
                Phone = trainer.Phone,
                BuildingNumber = trainer.Address.BuildingNumber,
                Street = trainer.Address.Street,
                City = trainer.Address.City,
                Specialties = trainer.Specility
            };
            return model;
        }
        public async Task<bool> UpdateTrainerAsync(int trainerId, TrainerToUpdateViewModel model, CancellationToken tk = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync (trainerId, tk);
            if(trainer == null) return false;

            var emailExist = await _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Email == model.Email && x.Id != trainerId);
            var phoneExist = await _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Phone == model.Phone && x.Id != trainerId);

            if (emailExist || phoneExist) return false;

            trainer.Email = model.Email;
            trainer.Phone = model.Phone;
            trainer.Address.BuildingNumber = model.BuildingNumber;
            trainer.Address.City = model.City;
            trainer.Address.Street = model.Street;
            trainer.Specility = model.Specialties;

            var result = await _unitOfWork.SaveChangesAsync(tk);
            return result > 0;
        }
        public async Task<bool> DeleteTrainerAsync(int trainerId, CancellationToken tk = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, tk);
            if (trainer == null) return false;

            var scheduleSessions = await _unitOfWork.GetRepository<Session>().AnyAsync(x => x.TrainerId == trainerId && x.EndDate > DateTime.Now,tk);
            if(scheduleSessions) return false;

            _unitOfWork.GetRepository<Trainer>().DeleteAsync(trainer, tk);
            var result = await _unitOfWork.SaveChangesAsync(tk);

            return result > 0;
        }
    }
}
