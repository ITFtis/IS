using Dou.Misc.Attr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IS.Models.Base
{
    /// <summary>
    /// 諮詢方式
    /// </summary>
    [Table("ConsultMethod")]
    public class ConsultMethod
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public int No { set; get; }

        [StringLength(12)]
        [ColumnDef(Required = true)]
        [Display(Name = "諮詢方式")]
        public string Name { set; get; }


        [StringLength(24)]
        [Display(Name = "備註")]
        public string Remark { set; get; }
        [Display(Name = "順序")]
        public int? Order { set; get; }
    }

    public class ConsultMethodSelectItemsClassImp : SelectItemsClass
    {
        public const string AssemblyQualifiedName = "IS.Models.Base.ConsultMethodSelectItemsClassImp, IS";

        static IEnumerable<ConsultMethod> _methods;
        static IEnumerable<ConsultMethod> METHODS
        {
            get
            {
                if (_methods == null || _methods.Count() == 0)
                {
                    using (var db = new IS.Models.DouImpModelContext())
                    {
                        _methods = db.ConsultMethod.OrderBy(s => s.Order).ToArray();
                    }
                }
                return _methods;
            }
        }


        public static void Reset()
        {
            _methods = null;
        }
        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            return METHODS.Select(s => new KeyValuePair<string, object>(s.No + "", s.Name + "@@" + s.Order)); //加@@，後面是排序用Dou.js
        }
    }
}