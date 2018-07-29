using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication3.Models;
using X.PagedList;
//using System.Linq.Dynamic;

namespace WebApplication3.Controllers
{
    [LogActionTime]
    public class 客戶資料Controller : Controller
    {
        private 客戶資料Repository repo;
        private 客戶聯絡人Repository repox;

        public 客戶資料Controller()
        {
            repo = RepositoryHelper.Get客戶資料Repository();
            repox = RepositoryHelper.Get客戶聯絡人Repository(repo.UnitOfWork);
        }
        //private 客戶資料Entities db = new 客戶資料Entities();

        // GET: 客戶資料
        public ActionResult Index(int? page)
        {

            var pageNumber = page ?? 1;
            var data = repo.All().OrderBy(x => x.Id).ToPagedList(pageNumber, 1);
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
        public ActionResult Sort(string condition)
        {
            ViewBag.classification = new SelectList(repo.DropDownList());
            var orderby = (string)Session["orderby"];
            ViewBag.orderby = orderby != "asc" ? "asc" : "desc";
            Session["orderby"] = ViewBag.orderby;

            var data = repo.Sort(condition, ViewBag.orderby);
            return View("Index", data);
        }

        public ActionResult SettingAccount(int id)
        {

            if (!User.Identity.IsAuthenticated)
            {
                return Content("未登入");
            }
            return PartialView(repo.Find(id));
        }

        public ActionResult EditAccount(客戶帳號VM item)
        {
            if (ModelState.IsValid)
            {
                SHA512 sha512 = new SHA512CryptoServiceProvider();
                string resultSha512 = Convert.ToBase64String(sha512.ComputeHash(Encoding.Default.GetBytes(item.密碼)));

                var data = repo.Find(item.Id);
                data.密碼 = resultSha512;
                data.傳真 = item.傳真;
                data.地址 = item.地址;
                data.電話 = item.電話;
                data.Email = item.Email;
                repo.UnitOfWork.Commit();
            }


            return PartialView();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(string account, string password)
        {
            SHA512 sha512 = new SHA512CryptoServiceProvider();
            string resultSha512 = Convert.ToBase64String(sha512.ComputeHash(Encoding.Default.GetBytes(password)));

            var data = repo.All().FirstOrDefault(x => x.帳號.Equals(account));
            if (data == null || !data.密碼.Equals(resultSha512))
            {
                RedirectToAction("Index");
            }
            FormsAuthentication.RedirectFromLoginPage(account, false);
            //FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
            //    account,
            //    DateTime.Now,
            //    DateTime.Now.AddMinutes(30),
            //    false,
            //    account,
            //    FormsAuthentication.FormsCookiePath);

            //// Encrypt the ticket.
            //string encTicket = FormsAuthentication.Encrypt(ticket);

            //// Create the cookie.
            //Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

            return RedirectToAction("SettingAccount", new { id = data.Id });
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
        public ActionResult DetailCollection(int id)
        {
            ViewData.Model = repo.FindDetialCollection(id);
            return PartialView("DetailCollection");
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

        [HandleError(ExceptionType = typeof(NullReferenceException), View = "Error_NullReferenceException")]
        public ActionResult BatchUpdate(客戶聯絡人VM[] data, int id)
        {
            if (ModelState.IsValid)
            {
                foreach (var vm in data)
                {
                    var item = repox.Find(vm.Id);
                    item.職稱 = vm.職稱;
                    item.手機 = vm.手機;
                    item.電話 = vm.電話;
                }
                repox.UnitOfWork.Commit();
            }
            return RedirectToAction("Details", new { id = id });

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
