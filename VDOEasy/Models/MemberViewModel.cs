using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VDOEasy.ViewModels
{
    public class MemberViewModel
    {
        [Display(Name = "ประเภทสมาชิก")]
        public int MemberTypeId { get; set; }

        [Display(Name = "สาขา")]
        public int BranchId { get; set; }

        [Display(Name = "ชื่อ")]
        [Required(ErrorMessage = "กรุณากรอกชื่อ")]
        public string FirstName { get; set; }

        [Display(Name = "นามสกุล")]
        [Required(ErrorMessage = "กรุณากรอกนามสกุล")]
        public string LastName { get; set; }

        [Display(Name = "วันเกิด")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "กรุณากรอกวันเกิด")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "ที่อยู่")]
        [Required(ErrorMessage = "กรุณากรอกที่อยู่")]
        public string Address { get; set; }

        [Display(Name = "หมายเลขบัตรประชาชน")]
        [Required(ErrorMessage = "กรุณากรอกหมายเลขบัตรประชาชน")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "หมายเลขบัตรประชาชนต้องมี 13 หลัก")]
        public string IdCardNumber { get; set; }

        [Display(Name = "ประเภทภาพยนตร์ที่ชอบ")]
        public List<int> SelectedMovieTypeIds { get; set; }
    }
}
