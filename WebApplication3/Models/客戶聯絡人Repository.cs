using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Collections.Generic;
	
namespace WebApplication3.Models
{   
	public  class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
	{
        public override IQueryable<客戶聯絡人> All()
        {
            return base.All().Where(x => x.客戶資料.是否已刪除 != true && x.是否已刪除 != true);
        }

        public IQueryable DisplayDAll()
        {
            return base.All().Where(x => x.客戶資料.是否已刪除 == true || x.是否已刪除 == true).Select(x => new { x.職稱, x.姓名, x.Email, x.手機, x.電話, x.客戶資料.客戶名稱 });
        }

        public 客戶聯絡人 Find(int id)
        {
            return All().FirstOrDefault(x => x.Id.Equals(id));
        }

        public IQueryable<客戶聯絡人> Search(string Keyword, string classification)
        {
            var data = All();
           
            if (!string.IsNullOrEmpty(Keyword))
            {
                data = data.Where(x => x.Email.Contains(Keyword) || x.姓名.Contains(Keyword) || x.手機.Contains(Keyword) || x.電話.Contains(Keyword));
            }
            if (!string.IsNullOrEmpty(classification))
            {
                data = data.Where(x => x.職稱.Equals(classification));
            }

            return data;
        }
        public IQueryable<客戶聯絡人> Sort(string condition, string orderby)
        {
            //return base.All().Where(x => x.是否已刪除 != true);
            return All().OrderBy(string.Format("{0} {1}", condition, orderby));
        }

        public override void Delete(客戶聯絡人 entity)
        {
            entity.是否已刪除 = true;
        }

        public IQueryable<String> DropDownList()
        {
            return All().Select(x=>x.職稱).Distinct();
        }
    }

	public  interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
	{

	}
}