using Dou.Misc.Attr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IS.Models.Prj
{
    [Table("ConsultRecordLog")]
    public class ConsultRecordLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ColumnDef(Visible = false, VisibleEdit = false, VisibleView = false)]
        public int Seq { set; get; }

        [Index]
        //[ForeignKey("ConsultRecord")]
        public int No { set; get; }


        public DateTime LogDatettime { set; get; }

        [ColumnDef(VisibleEdit = false, Visible = false)]
        [StringLength(24)]
        [Display(Name = "回覆同仁", Order = 24)]
        public string ReplyEmpId { set; get; }

        [NotMapped]
        public string ReplyEmpName { get {
                var emp = FtisHelper.DB.Hepler.GetEmployee(ReplyEmpId);
                return emp == null ? ReplyEmpId : emp.Name;
            } 
        }

        [StringLength(512)]
        [Display(Name = "回覆描敘", Order = 26)]
        [ColumnDef(EditType = EditType.TextArea)]
        public string ReplyContent { set; get; }

        [Display(Name = "狀態", Order = 90)]
        [ColumnDef(Filter = true, Sortable = true, EditType = EditType.Select, SelectItems = "{\"1\":\"不需追蹤\",\"2\":\"待追蹤\",\"3\":\"已簽約\"}")]
        public int Status { set; get; }

        [Display(Name = "不需追蹤說明", Order = 90)]
        [ColumnDef( Sortable = true, EditType = EditType.Select, SelectItems = "{\"1\":\"查驗費用不符經濟誘因\",\"2\":\"綠色工廠\",\"3\":\"已有合作廠商\",\"4\":\"推估報價過高\",\"5\":\"多數取水使用地下水\",\"6\":\"其他\"}")]
        public int? StatusReason { set; get; }

        [System.Web.Script.Serialization.ScriptIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public ConsultRecord ConsultRecord { set; get; }
    }
}