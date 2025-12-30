using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudentRegistrationSystem.Domain.Entities;

namespace StudentRegistrationSystem.Domain.Interfaces.Repositories;

public interface IRegistrationRepository
    {
    Task<Registration?> GetByIdAsync(string id);

    Task<IEnumerable<Registration>> GetByStudentIdAsync(string studentId);

    Task<IEnumerable<Registration>> GetByCourseIdAsync(string courseId);

    Task<IEnumerable<Registration>> GetActiveByStudentIdAsync(string studentId);

    Task<bool> IsRegisteredAsync(string studentId, string courseId);

    Task<string> CreateAsync(Registration registration);

    Task<bool> UpdateAsync(Registration registration);

    Task<bool> DeleteAsync(string id);

    Task<int> GetRegistrationCountAsync(string courseId);
}
