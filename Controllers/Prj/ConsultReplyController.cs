using Dou.Controllers;
using Dou.Misc;
using Dou.Misc.Attr;
using Dou.Models.DB;
using IS.Controllers.Manager;
using IS.Models;
using IS.Models.Prj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IS.Controllers.Prj
{
    [MenuDef(Name = "諮詢回覆", MenuPath = "諮詢資料", Action = "Index", Index = 2, Func = FuncEnum.Update, AllowAnonymous = false)]
    [AutoLogger(Content = AutoLoggerAttribute.LogContent.AssignContent, AssignContent = "編號:{No},狀態:{Status},新增回覆")]
    public class ConsultReplyController : ConsultBaseController
    {
        // GET: ConsultReply
        public ActionResult Index()
        {
            return View();
        }
        

        public override DataManagerOptions GetDataManagerOptions()
        {
            var options = base.GetDataManagerOptions();
            options.viewable = false;
            return options;
        }

        //只能刪除最新一筆而且是自己的回覆
        public bool Del(int seq)
        {
            DouImpModelContext db = new DouImpModelContext();
            var deleteOrderDetails =    (from data in db.ConsultRecordLog 
                                        where data.Seq == seq && data.ReplyEmpId== UserController.CurrentFtisEmployee.Mno
                                         select data).FirstOrDefault();
            db.ConsultRecordLog.Remove(deleteOrderDetails);
            db.SaveChanges();
            return true;
        }

        //確認最新一筆是自己的回覆
        public bool AdminOrMe(int seq)
        {
            DouImpModelContext db = new DouImpModelContext();
            var deleteOrderDetails = from data in db.ConsultRecordLog
                                      where data.Seq == seq && data.ReplyEmpId == UserController.CurrentFtisEmployee.Mno
                                      select data;
            if (deleteOrderDetails.Count() > 0)
            {
                return true;
            }
            else
            { return false; }
        }
    }
}