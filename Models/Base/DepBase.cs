using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IS.Models.Base
{
    [Table("DepBase")]
    public class DepBase
    {
        /// <summary>
        /// 部門編碼
        /// </summary>
        [Key]
        [Display(Name = "部門編號")]
        public string DCode { set; get; }
        /// <summary>
        /// 部門名稱
        /// </summary>
        [Display(Name = "部門名稱")]
        public string DName { set; get; }
    }
}