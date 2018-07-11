using System;
using System.Linq;
using System.Collections.Generic;
	
namespace WebApplication3.Models
{   
	public  class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
	{
        public override IQueryable<客戶聯絡人> All()
        {
            return base.All().Where(x => x.客戶資料.是否已刪除 != true && x.是否已刪除 != true);
        }

        public 客戶聯絡人 Find(int id)
        {
            return All().FirstOrDefault(x => x.Id.Equals(id));
        }

        public IQueryable<客戶聯絡人> Search(string Keyword)
        {
            var data = All();
           
            if (!string.IsNullOrEmpty(Keyword))
            {
                data = data.Where(x => x.Email.Contains(Keyword) || x.姓名.Contains(Keyword) || x.手機.Contains(Keyword) || x.電話.Contains(Keyword));
            }

            return data;
        }

        public override void Delete(客戶聯絡人 entity)
        {
            entity.是否已刪除 = true;
        }
    }

	public  interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
	{

	}
}