using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValconLibrary.DTOs;
using ValconLibrary.Entities;

namespace ValconLibrary.Test.Models
{
    public class UserObjectMother
    {
        public static RegisterDto CreateUserRegisterDto()
        {
            return new RegisterDto
            {
                UserName = "test",
                Name = "Test",
                LastName = "Test",
                Email = "test@email.com",
                Password = "TestPwd123!",
                DateOfBirth = DateOnly.Parse("2000-05-03")
            };
        }

        public static UserIdentity CreateUserIdentity()
        {
            return new UserIdentity
            {
                UserName = "test",
                Email = "test@email.com"
            };
        }

        public static UpdateUserDto CreateUpdateUserDto()
        {
            return new UpdateUserDto
            {
                Name = "Test1",
                LastName= "Test1",
                DateOfBirth = DateOnly.Parse("1999-05-03")
            };
        }

        public static PasswordUpdateDto CreatePasswordUpdateDto()
        {
            return new PasswordUpdateDto
            {
                OldPassword = "OldPwd123!",
                NewPassword = "NewPwd123!"
            };
        }

        public static List<UserIdentity> CreateUsersList()
        {
            return new List<UserIdentity>
        {
            new UserIdentity
            {
                UserName = "User1",
                Name = "First",
                LastName = "User",
                Email = "first.user@example.com",
                DateOfBirth = new DateOnly(1990, 1, 1)
            },
            new UserIdentity
            {
                UserName = "User2",
                Name = "Second",
                LastName = "User",
                Email = "second.user@example.com",
                DateOfBirth = new DateOnly(1992, 2, 2)
            }
        };
        }
    }
}
