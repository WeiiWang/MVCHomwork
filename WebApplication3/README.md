1.var 客戶銀行資訊 = db.客戶銀行資訊.Include(客 => 客.客戶資料);
->Repository不知道為何無法用Include API所以用All()取代不知會否影響效能

2.實作客戶聯絡人底下Email不能重複時，用輸入驗證可以做到新增時判斷有重複，但修改時沒有改的話會被當成Email重複擋住，救命

3.ClosedXML有一個方法 Worksheets.Add 其中一種可以帶 DataTable進去，就可以直接寫好檔案，有IQueryable轉DataTable的方法嗎?

4.ClosedXML一定要用Select指定欄位才能成功，有可以全部欄位匯出的方法嗎?

5.用ajax去觸發下載檔案必須用window.location.href，感覺同一個網址開兩次，不知道對不對
$("#Export").click(function () {
	$('@Url.Action("Export")', function (){
		window.location.href = '@Url.Action("Export")';
	});
});

6.做下拉選單的時候發現沒有預設的"請選擇"選項，雖然後來有查到@Html.DropDownList可以加預設選項，但是原本想法是在查出來的資料Insert一筆資料在IQueryable最頂端，有辦法嗎?

7.做Jsonresult時因為導覽屬性導致發生錯誤，最後用匿名型別躲掉導覽屬性，有其他做法嗎?

大約花費時間10hr10min

