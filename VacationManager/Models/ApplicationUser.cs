using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace VacationManager.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        
        public int? TeamId { get; set; }
        public Team? Team { get; set; }

        public ICollection<VacationRequest> VacationRequests { get; set; } = new List<VacationRequest>();
    }
}
