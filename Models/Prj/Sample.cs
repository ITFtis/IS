using Dou.Misc.Attr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IS.Models.Prj
{
    public class Sample
    {
        [Key]
        [ColumnDef(VisibleEdit =false)]
        [Display(Name = "編碼")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int No { get; set; }

        [ColumnDef(Required =true,Filter =true, FilterAssign =  FilterAssignType.Contains)]
        [Display(Name = "名稱")]
        [StringLength(8)]
        public string Name { get; set; }

        [ColumnDef(Required = true)]
        [Display(Name = "數值")]
        public int F_Number { get; set; }

        [ColumnDef(EditType = EditType.Select, Filter = false, SelectSourceDbContextNamespace = "IS.Models.DouImpModelContext,IS",
           SelectSourceModelNamespace = "IS.Models.User,IS", SelectSourceModelValueField = "Id", SelectSourceModelDisplayField = "Name")]
        [Display(Name = "選單1")]
        [StringLength(50)]
        public string F_Select1 { get; set; }

        [ColumnDef(EditType = EditType.Select,Filter =true, SelectItems = "{\"1\":\"item1\",\"2\":\"item2\",\"3\":\"item3\",\"4\":\"item4\"}")]
        [Display(Name = "選單2")]
        [StringLength(50)]
        public string F_Select2 { get; set; }

        [ColumnDef(Required = true, Filter =true, FilterAssign = FilterAssignType.Between )]
        [Display(Name = "日期1")]
        public DateTime F_Datetime { get; set; }

        [ColumnDef(Required = true, EditType = EditType.Date)]
        [Display(Name = "日期2")]
        public DateTime F_Date { get; set; }

        [ColumnDef(EditType = EditType.Checkbox, DefaultValue = "1", Filter = false, SelectItems = "{'1':'是','0':'否'}")]
        [Display(Name = "布林值")]
        public bool F_Bool { get; set; }

        [ColumnDef(EditType = EditType.Image, ImageMaxHeight =100, ImageMaxWidth =150)]
        [Display(Name = "圖檔")]
        public string F_Image { get; set; }
        //[Column("F_Image", TypeName = "image")]
        //public byte[] F_Image { get; set; }

        //[ColumnDef()]
        [Display(Name = "文字")]
        public string F_Textarea { get; set; }
    }
}