using System.Collections.Generic;

namespace StudentRegistrationSystem.Web.ViewModels.Registrations;

public class MyCoursesViewModel
{
    public IEnumerable<RegistrationViewModel> Registrations { get; set; } = new List<RegistrationViewModel>();
}
