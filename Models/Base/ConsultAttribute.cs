using Dou.Misc.Attr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IS.Models.Base
{
    [Table("ConsultAttribute")]
    public class ConsultAttribute
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public int No { set; get; }

        [StringLength(12)]
        [ColumnDef(Required = true)]
        [Display(Name = "諮詢屬性")]
        public string Name { set; get; }


        [StringLength(24)]
        [Display(Name = "備註")]
        public string Remark { set; get; }
        [Display(Name = "順序")]
        public int? Order { set; get; }
    }

    public class ConsultAttributeSelectItemsClassImp : SelectItemsClass
    {
        public const string AssemblyQualifiedName = "IS.Models.Base.ConsultAttributeSelectItemsClassImp, IS";

        static IEnumerable<ConsultAttribute> _attrs;
        static IEnumerable<ConsultAttribute> ATTRS
        {
            get
            {
                if (_attrs == null || _attrs.Count() == 0)
                {
                    using (var db = new IS.Models.DouImpModelContext())
                    {
                        _attrs = db.ConsultAttribute.OrderBy(s=>s.Order).ToArray();
                    }
                }
                return _attrs;
            }
        }


        public static void Reset()
        {
            _attrs = null;
        }
        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            return ATTRS.Select(s => new KeyValuePair<string, object>(s.No + "", s.Name+"@@"+s.Order)); //加@@，後面是排序用Dou.js
        }
    }
}