using Dou.Misc.Attr;
using FtisHelper.DB.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IS.Models.Prj
{
    [Table("ConsultRecord")]
    public class ConsultRecord
    {
        public ConsultRecord()
        {
            this.ReplyLogs = new HashSet<ConsultRecordLog>();
        }
        //const var cc = new ccc();
        //public bool cc(object o)
        //{
        //    return true;
        //}
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ColumnDef(Visible = false, VisibleEdit = false, VisibleView = false)]
        public int No { set; get; }

        [ColumnDef(VisibleEdit = false, Sortable = true, Filter = true, FilterAssign = FilterAssignType.Between)]
        [Display(Name = "諮詢時間", Order = 1)]
        public DateTime RecordDatetime { set; get; }

        [StringLength(24)]
        [ColumnDef(VisibleEdit = false, Visible =false, VisibleView =false)]
        [Display(Name = "接洽者員編", Order = 3)]
        public string RecordEmpId { set; get; }

        [ColumnDef(VisibleEdit = false)]//, Filter = true, FilterAssign = FilterAssignType.Contains)] 
        [Display(Name = "接洽者", Order = 3)]
        [NotMapped] //NotMapped 做Filter Dou平台會濾掉(無作用)，可於BeforeIQueryToPagedList做，但會有page問題
        public string RecordEmpName { get {
            var emp =FtisHelper.DB.Hepler.GetEmployee(RecordEmpId);
            return emp==null ?RecordEmpId:emp.Name;
        } }

        [StringLength(8)]
        [ColumnDef(VisibleEdit = false, Sortable = true, Filter = true, EditType = EditType.Select, SelectItemsClassNamespace = DepartmentSelectItemsClassImpForConsultRecord.AssemblyQualifiedName)]
        [Display(Name = "接洽單位", Order = 5)]
        public string RecordDep { set; get; } //為了查詢不用nomap欄位

       
        [Required]
        [StringLength(50)]
        [Display(Name = "諮詢單位", Order = 9)]
        [ColumnDef(Filter =true, FilterAssign = FilterAssignType.Contains)]
        public string ConsultUnit { set; get; }
        
        [Required]
        [StringLength(12)]
        [Display(Name = "所在縣市", Order = 11)]
        [ColumnDef(Sortable = true, Filter = true, EditType = EditType.Select, SelectItemsClassNamespace = Base.CitySelectItemsClassImp.AssemblyQualifiedName)]
        public string City { set; get; }


        [StringLength(24)]
        [Required]
        [ColumnDef(Visible = false)]
        [Display(Name = "聯絡人", Order = 13)]
        public string ContactPerson { set; get; }

        [ColumnDef(Visible = false)]
        [StringLength(80)]
        [Display(Name = "聯絡地址", Order = 14)]
        public string ContactAddress { set; get; }

        [ColumnDef(Visible = false)]
        [StringLength(24)]
        [Display(Name = "聯絡電話", Order = 16)]
        public string ContactTel { set; get; }

        [ColumnDef(Visible = false)]
        [StringLength(80)]
        [Display(Name = "聯絡E-Mail", Order = 18)]
        public string ContactMail { set; get; }

        [Required]
        [ColumnDef(EditType = EditType.Select, Filter = false, SelectItemsClassNamespace = Base.ConsultMethodSelectItemsClassImp.AssemblyQualifiedName)]
        [Display(Name = "諮詢方式", Order =20)]
        public int ConsultMethod { set; get; }

        [Required]
        [ColumnDef(EditType = EditType.Select, Filter = false, SelectItemsClassNamespace = Base.InfoSourceSelectItemsClassImp.AssemblyQualifiedName)]
        [Display(Name = "訊息來源", Order = 22)]
        public int InfoSource { set; get; }


        [Required]
        [ColumnDef(EditType = EditType.Select, Filter = false, SelectItemsClassNamespace = Base.ConsultAttributeSelectItemsClassImp.AssemblyQualifiedName)]
        [Display(Name = "諮詢屬性", Order = 24)]
        public int ConsultAttribute { set; get; }

        [Required]
        [ColumnDef(EditType = EditType.Select, Filter = false, SelectItemsClassNamespace = Base.ConsultTypeSelectItemsClassImp.AssemblyQualifiedName)]

        [Display(Name = "諮詢分類", Order = 26)]
        public int ConsultType { set; get; }

        [StringLength(512)]
        [ColumnDef(EditType = EditType.TextArea)]
        [Display(Name = "諮詢內容描敘", Order =80)]
        public string ContactContent { set; get; }


        //[ColumnDef(VisibleEdit = false, Visible =false)]
        //[StringLength(24)]
        //[Display(Name = "回覆同仁", Order = 24)]
        //public string ReplyStaffId { set; get; }

        //[ColumnDef(VisibleEdit = false)]
        //[Display(Name = "回覆同仁", Order = 24)]
        //[NotMapped]
        //public string ReplyStaffName
        //{
        //    get
        //    {
        //        var emp = FtisHelper.DB.Hepler.GetEmployee(ReplyStaffId);
        //        return emp == null ? ReplyStaffId : emp.Name;
        //    }
        //}

        //[StringLength(512)]
        //[Display(Name = "回覆描敘", Order = 26)]
        //[ColumnDef(EditType = EditType.TextArea)]
        //public string ReplyContent { set; get; }
        
        public virtual ICollection<ConsultRecordLog> ReplyLogs { set; get; }

        [Display(Name = "狀態", Order =90)]
        [ColumnDef(Filter = true, Sortable = true,EditType = EditType.Select, SelectItems = "{\"0\":\"未回覆\",\"1\":\"不需追蹤\",\"2\":\"待追蹤\",\"3\":\"已簽約\"}")]
        public int Status { set; get; }

        [Display(Name = "不需追蹤說明", Order = 90)]
        [ColumnDef( Sortable = true, EditType = EditType.Select, SelectItems = "{\"1\":\"查驗費用不符經濟誘因\",\"2\":\"綠色工廠\",\"3\":\"已有合作廠商\",\"4\":\"推估報價過高\",\"5\":\"多數取水使用地下水\",\"6\":\"其他\"}")]
        public int? StatusReason { set; get; }

        [MaxLength]
        [Display(Name = "備註", Order =99)]
        [ColumnDef(EditType = EditType.TextArea)]
        public string Remark { set; get; }
    }
}