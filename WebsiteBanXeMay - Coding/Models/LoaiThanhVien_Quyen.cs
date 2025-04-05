using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebsiteBanXeMay___Coding.Models
{
    public class LoaiThanhVien_Quyen
    {
        [Key, Column(Order = 1)]
        public int LoaiThanhVienId { get; set; }

        [Key, Column(Order = 2)]
        public int QuyenId { get; set; }

        [ForeignKey("LoaiThanhVienId")]
        public virtual LoaiThanhVien LoaiThanhVien { get; set; }

        [ForeignKey("QuyenId")]
        public virtual Quyen Quyen { get; set; }
    }
}
