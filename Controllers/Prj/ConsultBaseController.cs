using Dou.Controllers;
using Dou.Misc;
using Dou.Models.DB;
using IS.Models;
using IS.Models.Prj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Dynamic;
using Dou.Misc.Attr;
using System.Web.Routing;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using Dou.Misc;
using System.Diagnostics;
using System.Data.Entity;
using Dou.Help;

namespace IS.Controllers.Prj
{
    [MenuDef(Name = "諮詢資料", MenuPath = "",IsOnlyPath =true, Index = 0)]
    public class ConsultBaseController : APaginationModelController<ConsultRecord>
    {
        //是否權責人員
        protected bool IsHeightPermissionUser
        {
            get
            {
                return Dou.Context.CurrentIsAdminUser || Dou.Context.CurrentUser<User>().Boss;
            }
        }
        protected override IModelEntity<ConsultRecord> GetModelEntity()
        {
            return new ModelEntity<ConsultRecord>(new DouImpModelContext());
        }

        protected override void AddDBObject(IModelEntity<ConsultRecord> dbEntity, IEnumerable<ConsultRecord> objs)
        {
            foreach (var o in objs)
            {
                o.RecordDatetime = DateTime.Now;
                o.RecordEmpId = CurrentFtisEmployee?.Mno;// Dou.Context.CurrentUser<User>().Id;
                o.RecordDep = Dou.Context.CurrentIsAdminUser ? "16" : CurrentFtisEmployee.DCode;// Dou.Context.CurrentUser<User>().Dep;
                o.Status = 0;
            }
            base.AddDBObject(dbEntity, objs);
        }
        protected override void DeleteDBObject(IModelEntity<ConsultRecord> dbEntity, IEnumerable<ConsultRecord> objs)
        {
            objs.First().ReplyLogs = null;
            base.DeleteDBObject(dbEntity, objs);
        }
        protected override void UpdateDBObject(IModelEntity<ConsultRecord> dbEntity, IEnumerable<ConsultRecord> objs)
        {
            base.UpdateDBObject(dbEntity, objs);
        }
        //轉成PageList前多抓ReplyLogs
        protected override IQueryable<ConsultRecord> BeforeIQueryToPagedList(IQueryable<ConsultRecord> iquery, params KeyValueParams[] paras)
        {
            if (!IsHeightPermissionUser) {
                var cdepid= Dou.Context.CurrentUser<User>().Dep;
                iquery = iquery.Where(s => s.RecordDep == cdepid);
            }

            //以下為RecordEmpName的filter，因是NotMapped非實體欄位，所以不能直接查詢
            //注意，此方法(iquery.ToList())可能會查詢所有資料(無條件)，造成嚴重效能問題，另一方法可先查出RecordEmpName(包含)的Id先過濾
            //KeyValueParams colsFilterKeyValues = paras.Where(s => s.key.Equals("columnsFilter")).Count() > 0 ? paras.Where(s => s.key.Equals("columnsFilter")).First() : null;
            //if (colsFilterKeyValues != null)
            //{
            //    var kv = (colsFilterKeyValues.value as IEnumerable<KeyValueParams>).First(s => s.key == "RecordEmpName");
            //    if (kv != null && !string.IsNullOrEmpty(kv.value + ""))
            //    {
            //        var ss = iquery.ToList();
            //        iquery = ss.Where(s => s.RecordEmpName.Contains(kv.value + "")).AsQueryable<ConsultRecord>();
            //    }
            //}
            iquery = iquery.Include(s=>s.ReplyLogs);
            return base.BeforeIQueryToPagedList(iquery,paras);
        }
        //新增回覆
        public async Task<ActionResult> AddReplyLog(IEnumerable<ConsultRecordLog> objs)
        {
            bool isSuccess = true;
            string errorDesc = "新增資料完成!!";
            try
            {
                var addo = objs.First();
                addo.LogDatettime = DateTime.Now;
                addo.ReplyEmpId = CurrentFtisEmployee?.Mno;//  Dou.Context.CurrentUser<User>().Id;
                using (var db = GetModelEntity())
                {
                    using (var dbt= (db as ModelEntity<ConsultRecord>)._context.Database.BeginTransaction())
                    {
                        var crecord = db.Find(new object[] { addo.No });
                        crecord.Status = addo.Status;
                        if (addo.Status == 1)
                        {
                            crecord.StatusReason = addo.StatusReason;
                        }
                        else
                        {
                            crecord.StatusReason = null;
                            objs.First().StatusReason = null;
                        }
                        
                        db.Update(crecord);
                        new ModelEntity<ConsultRecordLog>((db as ModelEntity<ConsultRecord>)._context).Add(objs);
                        dbt.Commit();

                        ConsultRecord temp = new ConsultRecord();// {No=crecord.No, Status = crecord.s  };
                        MiscHelper.CopyPropertiesTo(crecord, temp);
                        //因ef6直接用crecord會是namespace"System.Data.Entity.DynamicProxies"的物件??，所以用temp
                        await WriteLoggerAsync(new ConsultRecord[] { temp }, LoggerEntity.LoggerDataStatus.Update);
                    }
                }
                
            }
            catch (DbEntityValidationException ex)
            {
                var error = ex.EntityValidationErrors.First().ValidationErrors.First();
                isSuccess = false;
                errorDesc = "";
                foreach (DbEntityValidationResult dr in ex.EntityValidationErrors)
                    errorDesc += dr.ValidationErrors.First().ErrorMessage + ";";
            }
            catch (ModelException ex)
            {
                isSuccess = false;
                errorDesc = ex.Message;
            }
            catch (Exception ex)
            {
                isSuccess = false;
                errorDesc = ex.ToString();
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            return Json(new { Success = isSuccess, Desc = errorDesc, data = objs }, JsonRequestBehavior.AllowGet);
        }
        //目前員工
        internal static FtisHelper.DB.Model.Employee CurrentFtisEmployee
        {
            get { return Manager.UserController.CurrentFtisEmployee; }
        }
        public override DataManagerOptions GetDataManagerOptions()
        {
            

            var r = base.GetDataManagerOptions();
            if (!Dou.Context.CurrentIsAdminUser && !Dou.Context.CurrentUser<User>().Boss)
            {
                var d = r.GetFiled("RecordDep");
                r.GetFiled("RecordDep").filter = false;
            }
            r.GetFiled("RecordDep").align = r.GetFiled("ConsultMethod").align = r.GetFiled("InfoSource").align = r.GetFiled("ConsultAttribute").align = r.GetFiled("ConsultType").align = "center";
            r.viewable = true;
            r.ctrlFieldAlign = "left";
            return r;
        }
    }
}