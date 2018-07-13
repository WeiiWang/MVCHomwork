using System;
using System.Linq;
using System.Collections.Generic;

namespace WebApplication3.Models
{
    public class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
    {
        public override IQueryable<客戶資料> All()
        {
            return base.All().Where(x => x.是否已刪除 != true);
        }

        public IQueryable<客戶資料ViewModel> 客戶VM()
        {
            return All().Select(x => new 客戶資料ViewModel
            {
                Id = x.Id,
                客戶名稱 = x.客戶名稱,
                聯絡人數量 = x.客戶聯絡人.Count(),
                銀行帳戶數量 = x.客戶銀行資訊.Count()
            }); ;
        }

        public 客戶資料 Find(int id)
        {
            return All().FirstOrDefault(x => x.Id.Equals(id));
        }

        public IQueryable<客戶資料> Search(string Keyword, string classification)
        {
            var data = All();

            if (!string.IsNullOrEmpty(Keyword))
            {
                data = data.Where(x => x.客戶名稱.Contains(Keyword) || x.統一編號.Contains(Keyword) || x.電話.Contains(Keyword) || x.Email.Equals(Keyword));
            }
            if (!string.IsNullOrEmpty(classification))
            {
                data = data.Where(x => x.客戶分類.Equals(classification));
            }
            return data;
        }
        public IQueryable<String> DropDownList()
        {
            return All().Select(x => x.客戶分類).Distinct();
        }
    }

    public interface I客戶資料Repository : IRepository<客戶資料>
    {

    }
}