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
    /// 訊息來源
    /// </summary>
    //[Table("InfoSource")]
    public class InfoSource
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ColumnDef(Visible = false,VisibleEdit =false)]
        public int No { set; get; }

        [StringLength(12)]
        [ColumnDef(Required = true)]
        [Display(Name = "訊息來源")]
        public string Name { set; get; }


        [StringLength(24)]
        [Display(Name = "備註")]
        public string Remark { set; get; }
        [Display(Name = "順序")]
        public int? Order { set; get; }
    }
    public class InfoSourceSelectItemsClassImp : SelectItemsClass
    {
        public const string AssemblyQualifiedName = "IS.Models.Base.InfoSourceSelectItemsClassImp, IS";

        static IEnumerable<InfoSource> _sources;
        static IEnumerable<InfoSource> SOURCES
        {
            get
            {
                if (_sources == null || _sources.Count() == 0)
                {
                    using (var db = new IS.Models.DouImpModelContext())
                    {
                        _sources = db.InfoSource.OrderBy(s => s.Order).ToArray();
                    }
                }
                return _sources;
            }
        }


        public static void Reset()
        {
            _sources = null;
        }
        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            return SOURCES.Select(s => new KeyValuePair<string, object>(s.No + "", s.Name + "@@" + s.Order)); //加@@，後面是排序用Dou.js
        }
    }
}