using Dou.Misc.Attr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IS.Models
{
    [Table("Role")]
    public class Role : Dou.Models.RoleBase
    {

        //[ColumnDef()]
        //[System.ComponentModel.DataAnnotations.Display(Name = "其他", Order =99)]
        //public string Other { set; get; }
    }
}