using Dou.Misc.Attr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IS.Models.Base
{
    [Table("ConsultType")]
    public class ConsultType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public int No { set; get; }

        [StringLength(12)]
        [ColumnDef(Required = true)]
        [Display(Name = "諮詢分類")]
        public string Name { set; get; }


        [StringLength(24)]
        [Display(Name = "備註")]
        public string Remark { set; get; }
        [Display(Name = "順序")]
        public int? Order { set; get; }
    }
    public class ConsultTypeSelectItemsClassImp : SelectItemsClass
    {
        public const string AssemblyQualifiedName = "IS.Models.Base.ConsultTypeSelectItemsClassImp, IS";

        static IEnumerable<ConsultType> _types;
        static IEnumerable<ConsultType> TYPES
        {
            get
            {
                if (_types == null || _types.Count() == 0)
                {
                    using (var db = new IS.Models.DouImpModelContext())
                    {
                        _types = db.ConsultType.OrderBy(s => s.Order).ToArray();
                    }
                }
                return _types;
            }
        }


        public static void Reset()
        {
            _types = null;
        }
        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            return TYPES.Select(s => new KeyValuePair<string, object>(s.No + "", s.Name + "@@" + s.Order)); //加@@，後面是排序用Dou.js
        }
    }
}