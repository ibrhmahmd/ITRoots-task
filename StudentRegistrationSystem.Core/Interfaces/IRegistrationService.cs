using System.Collections.Generic;
using System.Threading.Tasks;
using StudentRegistrationSystem.Core.DTOs;

namespace StudentRegistrationSystem.Core.Interfaces;

public interface IRegistrationService
{
    Task<IEnumerable<RegistrationDto>> GetByStudentIdAsync(string studentId);

    Task<IEnumerable<RegistrationDto>> GetActiveByStudentIdAsync(string studentId);

    Task<RegistrationDto> RegisterAsync(string studentId, string courseId);

    Task<bool> UnregisterAsync(string studentId, string courseId);
    Task<bool> IsRegisteredAsync(string studentId, string courseId);
}

