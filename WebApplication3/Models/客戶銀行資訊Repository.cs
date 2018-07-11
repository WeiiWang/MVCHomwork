using System;
using System.Linq;
using System.Collections.Generic;

namespace WebApplication3.Models
{
    public class 客戶銀行資訊Repository : EFRepository<客戶銀行資訊>, I客戶銀行資訊Repository
    {
        public override IQueryable<客戶銀行資訊> All()
        {
            return base.All().Where(x => x.客戶資料.是否已刪除 != true && x.是否已刪除 != true);
        }

        public 客戶銀行資訊 Find(int id)
        {
            return All().FirstOrDefault(x => x.Id.Equals(id));
        }

        public IQueryable<客戶銀行資訊> Search(string Keyword)
        {
            var data = All();
            int i = 0;
            bool result = int.TryParse(Keyword, out i);
            if (!string.IsNullOrEmpty(Keyword))
            {
                if (result)
                    data = data.Where(x => x.銀行代碼.Equals(i));
                else
                    data = data.Where(x => x.帳戶名稱.Contains(Keyword) || x.銀行名稱.Contains(Keyword));
            }

            return data;
        }

        public override void Delete(客戶銀行資訊 entity)
        {
            entity.是否已刪除 = true;
        }
    }

    public interface I客戶銀行資訊Repository : IRepository<客戶銀行資訊>
    {

    }
}