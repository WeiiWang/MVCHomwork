using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models.InputValidations
{
    public class EmailDuplicateAttribute:DataTypeAttribute
    {
        private 客戶資料Entities db = new 客戶資料Entities();
        public EmailDuplicateAttribute():base(DataType.EmailAddress)
        {
            ErrorMessage = "Email重複";
        }
        public override bool IsValid(object value)
        {
            string str = (string)value;

            var data = db.客戶聯絡人.FirstOrDefault(x => x.Email.Equals(str));

            return data == null ? true : false;
        }
    }
}