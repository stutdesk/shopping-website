using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication13.Models;
using PagedList;
namespace WebApplication13.Controllers
{
    public class HomeController : Controller
    {
        dbShoppingCarEntities db = new dbShoppingCarEntities();
        //===============Home Page=====================================
        public ActionResult Index(int page = 1)
        {
            var paged = 1 > page ? 1 : page;
            var product = db.tProduct.ToList();
            var result = product.ToPagedList(paged, 6);
            if (Session["Member"] == null)
            {
                return View("index", "_LayOut", result);
            }
            else if ((Session["Member"] as tMember).fUserId == "Admin")
            {
                return RedirectToAction("Adminindex");
            }
            return View("index", "_LayOutMember", result);
        }
        //public ActionResult Index(string kind)
        //{
        //    List<tProduct> product;
        //    if(!string.IsNullOrEmpty(kind))
        //    {
        //        product = db.tProduct.Where(m => m.kind.Contains(kind)).ToList();
        //    }
        //    else
        //    {
        //        product = db.tProduct.ToList();
        //    }
        //    if(Session["Member"] == null)
        //    {
        //        return View("index", "_LayOut", product);
        //    }
        //    else if((Session["Member"] as tMember).fUserId == "Admin")
        //    {
        //        return RedirectToAction("Adminindex");
        //    }
        //    return View("index","_LayOutMember",product);
        //}
        //============================================================
        public ActionResult Adminindex(int page = 1)
        {
            var paged = 1 > page ? 1 : page;
            var product = db.tProduct.ToList();
            var result = product.ToPagedList(paged, 15);
            return View("Adminindex", "_LayOutAdmin", result);
        }
        //============================================================
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string fUserId, string fPwd)
        {
            var Member = db.tMember.Where(m => m.fUserId == fUserId && m.fPwd == fPwd).FirstOrDefault();
            if (Member == null)
            {
                ViewBag.message = "帳密錯誤";
                return View();
            }
            else
            {
                Session["Member"] = Member;
                Session["Welcome"] = Member.fName + "Welcome";
                return RedirectToAction("index", "home");
            }
        }
        //============================================================
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("index");
        }
        //============================================================
        public ActionResult ShoppingCar(int page = 1)
        {
            var paged = 1 > page ? 1 : page;
            string name = (Session["Member"] as tMember).fUserId;
            var shoppingcar = db.tOrderDetail.Where(m => m.fUserId == name && m.fIsApproved == "未下定").ToList();
            var result = shoppingcar.ToPagedList(paged, 10);
            return View("ShoppingCar", "_LayOutMember", result);
        }
        [HttpPost]
        public ActionResult ShoppingCar(int[] fId)
        {

            var total = 0;
            List<tOrderDetail> list = new List<tOrderDetail>();
            string UserId = (Session["Member"] as tMember).fUserId;
            if (fId == null)
            {
                TempData["error"] = "至少選取一樣商品";
                return RedirectToAction("ShoppingCar");
            }
            foreach (var item in fId)
            {
                var order = db.tOrderDetail.Where(m => m.fUserId == UserId && m.fId == item).FirstOrDefault();
                total += Convert.ToInt32(order.fPrice * order.fQty);
                list.Add(order);
            }
            ViewBag.fId = fId;
            ViewBag.total = total;
            return View("CheckOut", "_LayOutMember", list);
        }
        //============================================================
        public ActionResult Regis()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Regis(tMember member)
        {
            if (ModelState.IsValid != true)
            {
                return View();
            }
            else
            {
                var members = db.tMember.Where(m => m.fUserId == member.fUserId).FirstOrDefault();
                if (members == null)
                {
                    db.tMember.Add(member);
                    db.SaveChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.Message = "帳號已有人使用";
                    return View();
                }
            }
        }
        //============================================================
        [HttpPost]
        public ActionResult AddCar(string fPId, int fQty)
        {
            string fUserId = (Session["Member"] as tMember).fUserId;
            var currentcar = db.tOrderDetail.Where(m => m.fUserId == fUserId && m.fPId == fPId && m.fIsApproved == "未下定").FirstOrDefault();
            if (currentcar == null)
            {
                var product = db.tProduct.Where(m => m.fPId == fPId).FirstOrDefault();
                tOrderDetail orderDetail = new tOrderDetail();
                orderDetail.fUserId = fUserId;
                orderDetail.fQty = fQty;
                orderDetail.fPrice = product.fPrice;
                orderDetail.fPId = product.fPId;
                orderDetail.fName = product.fName;
                orderDetail.fIsApproved = "未下定";
                db.tOrderDetail.Add(orderDetail);
            }
            else
            {
                currentcar.fQty += fQty;
            }
            db.SaveChanges();
            return RedirectToAction("ShoppingCar");
        }
        //============================================================
        public ActionResult DeleteCar(int fId)
        {
            string UserId = (Session["Member"] as tMember).fUserId;
            var product = db.tOrderDetail.Where(m => m.fUserId == UserId && m.fId == fId).FirstOrDefault();
            if (product == null)
            {
                ViewBag.Error = "查無該筆資料";
                return RedirectToAction("ShoppingCar");
            }
            else
            {
                db.tOrderDetail.Remove(product);
                db.SaveChanges();
                return RedirectToAction("ShoppingCar");
            }
        }
        //============================================================
        public ActionResult CheckOut()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CheckOut(string fReceiver, string fAddress, string fEmail, int[] fId)
        {
            List<tOrderDetail> list = new List<tOrderDetail>();
            string fUserId = (Session["Member"] as tMember).fUserId;
            string guid = Guid.NewGuid().ToString();
            tOrder order = new tOrder()
            {
                fAddress = fAddress,
                fEmail = fEmail,
                fDate = DateTime.Now,
                fOrderGuid = guid,
                fReceiver = fReceiver,
                fUserId = fUserId,
            };
            foreach (var item in fId)
            {
                var orderdetail = db.tOrderDetail.Where(m => m.fIsApproved == "未下定" && m.fUserId == fUserId && m.fId == item).FirstOrDefault();
                orderdetail.fIsApproved = "已下定";
                orderdetail.fOrderGuid = guid;
                orderdetail.fdate = DateTime.Now;
                var product = db.tProduct.Where(m => m.fName == (orderdetail.fName)).FirstOrDefault();
                product.fselled = Convert.ToInt32(orderdetail.fQty);
                list.Add(orderdetail);
            }
            var orderlist = db.tOrder.Where(m => m.fOrderGuid == guid).FirstOrDefault();
            ViewBag.Order = orderlist;
            db.tOrder.Add(order);
            db.SaveChanges();
            return View("BuyerDetail", "_LayOutMember", list);
        }

        //============================================================
        public ActionResult BuyerDetail()
        {
            return View();
        }
        //============================================================
        public ActionResult OrderList(string keyword, string searchkind, int page = 1)
        {
            var paged = 1 > page ? 1 : page;
            string UserId = (Session["Member"] as tMember).fUserId;
            ViewBag.keyword = keyword; ViewBag.searchkind = searchkind;
            if (!string.IsNullOrEmpty(keyword))
            {
                switch (searchkind)
                {
                    case "fOrderGuid":
                        var resault = db.tOrderDetail.Where(m => m.fOrderGuid.Contains(keyword) && m.fIsApproved == "已下定" && m.fUserId == UserId).ToList();
                        var finalresult = resault.ToPagedList(paged, 10);
                        return View("OrderList", "_LayOutMember", finalresult);
                    case "fPId":
                        var resault2 = db.tOrderDetail.Where(m => m.fPId.Contains(keyword) && m.fIsApproved == "已下定" && m.fUserId == UserId).ToList();
                        var finalresult2 = resault2.ToPagedList(paged, 10);
                        return View("OrderList", "_LayOutMember", finalresult2);
                    case "fName":
                        var resault3 = db.tOrderDetail.Where(m => m.fName.Contains(keyword) && m.fIsApproved == "已下定" && m.fUserId == UserId).ToList();
                        var finalresult3 = resault3.ToPagedList(paged, 10);
                        return View("OrderList", "_LayOutMember", finalresult3);
                }
            }
            var orderlist = db.tOrderDetail.Where(m => m.fUserId == UserId && m.fIsApproved == "已下定").ToList();
            var orderlistresult = orderlist.ToPagedList(paged, 15);
            return View("OrderList", "_LayOutMember", orderlistresult);
        }
        //============================================================
        [HttpPost]
        public ActionResult search(string keyword1, string searchkind1)
        {
            string userid = (Session["Member"] as tMember).fUserId;
            return RedirectToAction("OrderList", new { keyword = keyword1, searchkind = searchkind1 });
        }
        //============================================================
        //public ActionResult productsearch()
        //{
        //    return red
        //}
        [HttpPost]
        public ActionResult productsearch(string productname, string kind)
        {
            return RedirectToAction("productkind", new { kind, pname = productname });
        }
        //============================================================
        public ActionResult OrderDetail(string fGuid)
        {
            var detail = db.tOrderDetail.Where(m => m.fOrderGuid == fGuid).ToList();
            var time = db.tOrder.Where(m => m.fOrderGuid == fGuid).FirstOrDefault().fDate;
            ViewBag.time = time;
            return View("OrderDetail", "_LayOutMember", detail);
        }
        //============================================================
        public ActionResult Delete(int fId)
        {
            var delete = db.tProduct.Where(m => m.fId == fId).FirstOrDefault();
            string filename = delete.fImg;
            if (!string.IsNullOrEmpty(filename))
            {
                System.IO.File.Delete(Server.MapPath("~/Images") + "/" + filename);
            }
            db.tProduct.Remove(delete);

            db.SaveChanges();
            return RedirectToAction("Adminindex");
        }
        //============================================================
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(string fPId, string fName, int fPrice, HttpPostedFileBase fImg, string fkind)
        {
            try
            {
                var x = db.tProduct.Where(m => m.fPId == fPId).FirstOrDefault();
                if (x == null)
                {
                    string filename = "";
                    if (fImg != null)
                    {
                        if (fImg.ContentLength > 0)
                        {
                            filename = System.IO.Path.GetFileName(fImg.FileName);
                            var path = System.IO.Path.Combine(Server.MapPath("~/Images"), filename);
                            fImg.SaveAs(path);
                        }
                    }
                    tProduct product = new tProduct() { fPId = fPId, fName = fName, fPrice = fPrice, fImg = filename, kind = fkind };
                    db.tProduct.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Adminindex");
                }
                else
                    ViewBag.error = "產品編號重複";
            }
            catch (Exception ex)
            {
                ViewBag.error = "上傳失敗";
            }
            return View();
        }
        //============================================================
        public ActionResult Edit(int fId)
        {
            var product = db.tProduct.Where(m => m.fId == fId).FirstOrDefault();
            return View(product);
        }
        [HttpPost]
        public ActionResult Edit(int fId, string fPId, string fName, int fPrice, HttpPostedFileBase fImg, string fkind, string Oimg)
        {
            string filename = "";
            if (fImg != null)
            {
                if (fImg.ContentLength > 0)
                {
                    filename = System.IO.Path.GetFileName(fImg.FileName);
                    var path = System.IO.Path.Combine(Server.MapPath("~/Images"), filename);
                    fImg.SaveAs(path);
                }
            }
            else
            {
                filename = Oimg;
            }
            var product = db.tProduct.Where(m => m.fId == fId).FirstOrDefault();
            product.fPId = fPId;
            product.fName = fName;
            product.fPrice = fPrice;
            product.fImg = filename;
            product.kind = fkind;
            db.SaveChanges();
            return RedirectToAction("Adminindex");
        }
        //============================================================
        public ActionResult MemberManager(int page = 1)
        {
            var paged = 1 > page ? 1 : page;
            var member = db.tMember.ToList();
            var result = member.ToPagedList(paged, 15);
            return View(result);
        }
        public ActionResult MemberDelete(int fId)
        {
            var member = db.tMember.Where(m => m.fId == fId).FirstOrDefault();
            db.tMember.Remove(member);
            db.SaveChanges();
            return RedirectToAction("MemberManager");
        }
        //============================================================   
        public ActionResult productkind(string kind, string pname, int page = 1)
        {

            List<tProduct> product;
            var paged = 1 > page ? 1 : page;
            ViewBag.pname = pname;
            ViewBag.kind = kind;
            if (string.IsNullOrEmpty(pname))
            {
                product = db.tProduct.Where(m => m.kind.Contains(kind)).ToList();

            }
            else
            {
                product = db.tProduct.Where(m => m.fName.Contains(pname)).ToList();
            }
            var result = product.ToPagedList(paged, 6);
            if (Session["Member"] == null)
            {
                return View("productkind", "_LayOut", result);
            }
            return View("productkind", "_LayOutMember", result);
        }
    }
}
        //============================================================
//        public ActionResult productdetail(string productname)
//        {
//            var product = db.tProduct.Where(m => m.fName == productname).FirstOrDefault();
//                if (Session["Member"] == null)
//                {
//                    return View("productdetail", "_LayOut", product);
//                }
//                else 
//                return View("productdetail", "_LayOutMember", product);

//        }
//    }
//}