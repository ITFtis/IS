using Dou.Misc.Attr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IS.Models.Base
{
    [Table("City")]
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] //key是int，ef6預設視乎會是DatabaseGeneratedOption.Identity，會造成insert CityCode 是NULL問題
        [Display(Name = "縣市代碼")]

        public int CityCode { get; set; }
        [Required]
        [StringLength(8)]
        [Display(Name = "縣市名稱")]
        public string CityName { get; set; }
        public virtual List<Town> Towns { get; set; }

        //using (System.IO.StreamReader sr = new System.IO.StreamReader(@"D:\CVS_SRC\SourceCode\水利署\水利署重大水災情\Disaster\DisasterEditorMapWeb\kml\Town.json"))
        //    {
        //        var jarrays=Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JArray>(sr.ReadToEnd());
        //        List<Models.Base.City> clis = new List<Models.Base.City>();
        //        List<Models.Base.Town> tlis = new List<Models.Base.Town>();
        //        foreach (var jo in jarrays)
        //        {
        //            var c = jo.Value<int>("CityCode");
        //            var n = jo.Value<string>("CityName");

        //            var tc = jo.Value<int>("TownCode");
        //            var tn = jo.Value<string>("Town");

        //            tlis.Add(new Models.Base.Town { CityCode = c, TownCode = tc, TownName = tn });
        //            if (clis.Exists(s => s.CityCode == c))
        //                continue;
        //            clis.Add(new Models.Base.City { CityCode = c, CityName = n }); ;
        //        }
        //        var dd = clis.Where(s => s.CityCode == null);
        //        using ( var db = new IS.Models.DouImpModelContext()){
        //            db.Database.Log = (log) => Debug.WriteLine(log);
        //            db.City.AddRange(clis);
        //            db.Town.AddRange(tlis);
        //            db.SaveChanges();
        //        }
        //    }
    }


    [Table("Town")]
    public class Town
    {
        [Display(Name = "縣市代碼")]

        public int CityCode { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "鄉鎮區代碼")]

        public int TownCode { get; set; }
        [Required]
        [StringLength(8)]
        [Display(Name = "縣市名稱")]
        public string TownName { get; set; }
    }

    public class CitySelectItemsClassImp : SelectItemsClass
    {
        public const string AssemblyQualifiedName = "IS.Models.Base.CitySelectItemsClassImp, IS";

        static IEnumerable<City> _cites;
        static IEnumerable<City> CITIES
        {
            get
            {
                if (_cites == null || _cites.Count()==0)
                {
                    using (var db =new IS.Models.DouImpModelContext())
                    {
                        _cites =db.City.ToArray();
                    }
                }
                return _cites;
            }
        }


        public static void Reset()
        {
            _cites = null;
        }
        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            return CITIES.Select(s => new KeyValuePair<string, object>(s.CityCode+"", s.CityName));
        }
    }
}