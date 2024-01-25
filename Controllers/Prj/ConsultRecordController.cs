using Dou.Controllers;
using Dou.Misc;
using Dou.Misc.Attr;
using Dou.Models.DB;
using IS.Models;
using IS.Models.Prj;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace IS.Controllers.Prj
{
    //[MenuDef(Name = "諮詢紀錄", MenuPath = "", Action = "Index", Index = 0, Func = FuncEnum.ALL, AllowAnonymous = false)]
    //[AutoLogger(Content = AutoLoggerAttribute.LogContent.All)]
    public class ConsultRecordController : APaginationModelController<ConsultRecord>
    {
        // GET: ConsultRecord
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<ConsultRecord> GetModelEntity()
        {
            return new ModelEntity<ConsultRecord>(new DouImpModelContext());
        }
        protected override void AddDBObject(IModelEntity<ConsultRecord> dbEntity, IEnumerable<ConsultRecord> objs)
        {
            foreach(var o in objs)
            {
                o.RecordDatetime = DateTime.Now;
                o.RecordEmpId = Dou.Context.CurrentUser<User>().Id;
                o.RecordDep = Dou.Context.CurrentIsAdminUser ? "99" : Dou.Context.CurrentUser<User>().Dep;
                o.Status = 0;
            }
            base.AddDBObject(dbEntity, objs);
        }
        protected override IEnumerable<ConsultRecord> GetDataDBObject(IModelEntity<ConsultRecord> dbEntity, params KeyValueParams[] paras)
        {
            List<KeyValueParams> ps = paras.ToList();
            if (!Dou.Context.CurrentIsAdminUser && !Dou.Context.CurrentUser<User>().Boss)
            {
                var ukv = new KeyValueParams { key = "RecordDep", value = Dou.Context.CurrentUser<User>().Dep };
                var cfs = ps.FirstOrDefault(s => s.key == "columnsFilter");
                if (cfs != null) //按條件查詢
                {
                    var cf=  (cfs.value as IEnumerable<KeyValueParams>).FirstOrDefault(s => s.key == "RecordDep");
                    if (cf == null) //發生在業務單位登入RecordDep.filter=false
                    {
                       var new_cfsvalue= (cfs.value as IEnumerable<KeyValueParams>).ToList();
                        new_cfsvalue.Add(ukv);
                        cfs.value = new_cfsvalue;
                    }
                    else
                        cf.value = ukv.value;//GetDataManagerOptions設 RecordDep.filter=false應該不會有這現象
                }
                else //功能進來預設的查詢
                {
                    cfs = new KeyValueParams() { key = "columnsFilter", value = new KeyValueParams[] { ukv } };
                    ps.Add(cfs);
                }
            }

            //var sdd =GetModelEntity().GetAll().Include(s => s.ReplyLogs).ToArray();
            return base.GetDataDBObject(dbEntity, ps.ToArray()); 
        }
        public override DataManagerOptions GetDataManagerOptions()
        {
            var  r= base.GetDataManagerOptions();
            if (!Dou.Context.CurrentIsAdminUser && !Dou.Context.CurrentUser<User>().Boss)
            {
                var d = r.GetFiled("RecordDep");
                r.GetFiled("RecordDep").filter = false;
            }
            r.GetFiled("RecordDep").align = r.GetFiled("ConsultMethod").align = r.GetFiled("InfoSource").align = r.GetFiled("ConsultAttribute").align= r.GetFiled("ConsultType").align = "center";
            r.viewable = true;
            r.ctrlFieldAlign = "left";
            r.tableOptions.detailView = true;
            r.tableOptions.iconsPrefix = "glyphicon";
            r.tableOptions.icons = new ExpandoObject();
            r.tableOptions.icons.detailOpen = "glyphicon-eye-open icon-plus";
            r.tableOptions.icons.detailClose = "glyphicon-eye-close icon-minus";
            //r.tableOptions.detailViewByClick = true;
            //r.tableOptions.detailViewAlign = "right";
            //r.tableOptions.mobileResponsive = true;
            //r.editformLabelWidth = 4;
            return r;
        }
    }
}