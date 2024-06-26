using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.Domain.Entities
{
    public class Account : IdentityUser<int>
    {
        public string PhoneNumber { get; set; }
        [ForeignKey("Photos")]
        public int PhotoId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedAt { get;set; }
        public virtual ICollection<string> RefreshTokens { get; set; }
    }
}
