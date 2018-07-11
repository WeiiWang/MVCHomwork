using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WebApplication3.Models.InputValidations
{
    public class CellPhoneAttribute:DataTypeAttribute
    {
        private 客戶資料Entities db = new 客戶資料Entities();
        public CellPhoneAttribute():base(DataType.Text)
        {
            ErrorMessage = "請輸入正確格式 (e.g. 0911-111111)";
        }
        public override bool IsValid(object value)
        {
            string str = (string)value;

            Regex regex = new Regex(@"^\d{4}-\d{6}$");

            return regex.IsMatch(str);
        }
    }
}