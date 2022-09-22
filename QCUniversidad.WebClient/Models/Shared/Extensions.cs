﻿using QCUniversidad.WebClient.Models.Accounts;
using SmartB1t.Security.WebSecurity.Local;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.Shared
{
    public static class Extensions
    {
        public static User GetModel(this CreateUserViewModel viewModel)
            => new()
            {
                Fullname = viewModel.Fullname,
                Department = viewModel.Department,
                Position = viewModel.Position,
                Email = viewModel.Email,
                Active = true
            };

        public static EditUserViewModel GetEditViewModel(this User user)
            => new()
            {
                Id = user.Id.ToString(),
                Fullname = user.Fullname,
                Email = user.Email,
                Department = user.Department,
                Position = user.Position,
                RolesSelected = user.Roles.Select(ur => ur.Role.Id.ToString()).ToArray(),
                ProfilePictureId = user.Id.ToString()
            };
    }
}