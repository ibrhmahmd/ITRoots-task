using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudentRegistrationSystem.Domain.Entities;

namespace StudentRegistrationSystem.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(string id);

    
    Task<User?> GetByUsernameAsync(string username);

   
    Task<User?> GetByEmailAsync(string email);

    Task<User?> GetByEmailVerificationTokenAsync(string token);

    Task<string> CreateAsync(User user);

    Task<bool> UpdateAsync(User user);

    Task<bool> UsernameExistsAsync(string username);

    Task<bool> EmailExistsAsync(string email);
}
