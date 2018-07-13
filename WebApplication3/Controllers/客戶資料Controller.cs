using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class 客戶資料Controller : Controller
    {
        private 客戶資料Repository repo = RepositoryHelper.Get客戶資料Repository();
        //private 客戶資料Entities db = new 客戶資料Entities();

        // GET: 客戶資料
        public ActionResult Index()
        {

            var data = repo.All();
            ViewBag.classification = new SelectList(repo.DropDownList());
            return View(data);
        }

        public ActionResult Index2()
        {
            var data = repo.客戶VM();
            return View(data);
        }
        public ActionResult Search(string Keyword, string classification)
        {
            var data = repo.Search(Keyword, classification);
            ViewBag.classification = new SelectList(repo.DropDownList());
            return View("Index", data);
        }
        public ActionResult Export()
        {

            using (XLWorkbook wb = new XLWorkbook())
            {
                //取得我要塞入Excel內的資料
                var data = repo.All().Select(x => new { x.客戶名稱, x.客戶分類, x.電話, x.地址 });

                //一個wrokbook內至少會有一個worksheet,並將資料Insert至這個位於A1這個位置上
                var ws = wb.Worksheets.Add("客戶資料");

                //注意官方文件上說明,如果是要塞入Query後的資料該資料一定要變成是data.AsEnumerable()
                //但是我查詢出來的資料剛好是IQueryable ,其中IQueryable有繼承IEnumerable 所以不需要特別寫
                ws.Cell(1, 1).Value = data;

                //因為是用Query的方式,這個地方要用串流的方式來存檔
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wb.SaveAs(memoryStream);
                    //請注意 一定要加入這行,不然Excel會是空檔
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    //注意Excel的ContentType,是要用這個"application/vnd.ms-excel" 不曉得為什麼網路上有的Excel ContentType超長,xlsx會錯 xls反而不會
                    return File(memoryStream.ToArray(), "application/vnd.ms-excel", "Download.xlsx");
                }
            }
        }

        // GET: 客戶資料/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: 客戶資料/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,客戶分類,統一編號,電話,傳真,地址,Email")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                repo.Add(客戶資料);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(客戶資料);
        }

        // GET: 客戶資料/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: 客戶資料/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,客戶分類,統一編號,電話,傳真,地址,Email")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                repo.UnitOfWork.Context.Entry(客戶資料).State = EntityState.Modified;
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: 客戶資料/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶資料 客戶資料 = repo.Find(id);
            客戶資料.是否已刪除 = true;
            repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
