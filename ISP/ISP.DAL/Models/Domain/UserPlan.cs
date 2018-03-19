﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ISP.DAL.Enums;
using ISP.DAL.Models.Identity;

namespace ISP.DAL.Models.Domain
{
    public class UserPlan : CommonEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public int PlanId { get; set; }

        public virtual Plan Plan { get; set; }

        public DateTime LastPaidDate { get; set; }

        public ServiceStatus Status { get; set; } = ServiceStatus.Deactivated;

        public bool WillUnsubscribe { get; set; }
    }
}
