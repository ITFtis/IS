using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace IS.Models
{
    public partial class DouImpModelContext : Dou.Models.ModelContextBase<User, Role>
    {
        public DouImpModelContext()
            : base("name=DouImpModelContext")
        {
            Database.SetInitializer<DouImpModelContext>(null); //關閉給定內容類型(code first)初始化資料庫 migration in Entity Framework
        }

        //public virtual DbSet<Prj.Sample> Sample { get; set; }

        public virtual DbSet<Prj.ConsultRecord> ConsultRecord { get; set; }
        public virtual DbSet<Prj.ConsultRecordLog> ConsultRecordLog { get; set; }
        public virtual DbSet<Base.ConsultMethod> ConsultMethod { get; set; }
        public virtual DbSet<Base.InfoSource> InfoSource { get; set; }
        public virtual DbSet<Base.ConsultAttribute> ConsultAttribute { get; set; }
        public virtual DbSet<Base.ConsultType> ConsultType { get; set; }
        public virtual DbSet<Base.City> City { get; set; }
        public virtual DbSet<Base.Town> Town { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //資料表會跟model class 命名一致(不會再自動+s, 不用避免多s class要加TableAttribute)
            modelBuilder.Conventions.Remove< 
                 System.Data.Entity.ModelConfiguration.Conventions.
                 PluralizingTableNameConvention>();

            //modelBuilder.Entity<Prj.Sample>()
            //        .Property(c => c.No)
            //        .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
