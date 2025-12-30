using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudentRegistrationSystem.Domain.Entities;

namespace StudentRegistrationSystem.Domain.Interfaces.Repositories;

public interface IStudentRepository
{
    Task<Student?> GetByIdAsync(string id);

    Task<Student?> GetByUserIdAsync(string userId);

    Task<IEnumerable<Student>> GetAllAsync();

    Task<string> CreateAsync(Student student);

    Task<bool> UpdateAsync(Student student);

    Task<bool> ExistsByUserIdAsync(string userId);
}

