using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.Models
{
    public class Member : GymUser
    {
        public string? Photo  { get; set; }

        // JoinDate => CreatedAt In Fluent API

        #region Relationships
        // 1 -> 1
        public HealthRecord HealthRecord { get; set; } = default!;
        public ICollection<Membership> MemberPlans { get; set; } = default!;
        public ICollection<Booking> MemberSession { get; set; } = default!;

        #endregion
    }
}
