using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace WebsiteBanXeMay___Coding.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        public string? Address { get; set; }
        public string? Age { get; set; }

        // Quan hệ 1-1 với NhanVien
        public virtual NhanVien? NhanVien { get; set; }
    }
}
