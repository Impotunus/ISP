﻿namespace ISP.ViewModels.ProfileViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string UserName { get; set; }

        public string LastName { get; set; }

        public double Balance { get; set; }
        
        public string FirstName { get; set; }

        public string Address { get; set; }

        public string Role { get; set; }

        public bool AdminBanned { get; set; }
    }
}
