using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClientManager.Infrastructure;
using ClientManager.Models;
using DBOperation;

namespace ClientManager.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private ClientManagerEntities db = new ClientManagerEntities();


        // GET: Admin
        [CustomAuthorize("Admin", "Manager")]
        public ActionResult List()
        {
            var user = db.Users;
            return View(user.ToList());
        }

        [CustomAuthorize("Admin")]
        public ActionResult Activate(int? id)
        {
            UserDetails currentUser = (UserDetails)Session["UserDetails"];
            JsonReponse jsonRes = null;
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                User user = db.Users.Find(id);

                if (user == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    user.IsActive = true;
                    user.ModifiedBy = currentUser.Id;
                    user.ModifiedOn = DateTime.Now;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();

                    jsonRes = new JsonReponse { message = "User Activated successfully!", status = "Success", redirectURL = "/Admin/User/List?" + DateTime.Now.Ticks };
                }
            }
            catch (Exception ex)
            {
                jsonRes = new JsonReponse { message = ex.Message, status = "Error", redirectURL = "" };
            }

            return Json(jsonRes, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize("Admin")]
        public ActionResult DeActivate(int? id)
        {
            UserDetails currentUser = (UserDetails)Session["UserDetails"];
            JsonReponse jsonRes = null;
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                User user = db.Users.Find(id);

                if (user == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    user.IsActive = false;
                    user.ModifiedBy = currentUser.Id;
                    user.ModifiedOn = DateTime.Now;

                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();

                    jsonRes = new JsonReponse { message = "User De-Activated successfully!", status = "Success", redirectURL = "/Admin/User/List?" + DateTime.Now.Ticks };
                }
            }
            catch (Exception ex)
            {
                jsonRes = new JsonReponse { message = ex.Message, status = "Error", redirectURL = "" };
            }

            return Json(jsonRes, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize("Admin")]
        // GET: Admin/User/Edit/5
        public ActionResult Edit(int? id)
        {
            var currentUser = (UserDetails)Session["UserDetails"];
            ViewBag.ReportingManager = new SelectList(db.Users, "Id", "FullName");

            List<SelectListItem> statusList = new List<SelectListItem>();
            statusList.Insert(0, (new SelectListItem { Text = "Active", Value = "0" }));
            statusList.Insert(1, (new SelectListItem { Text = "De-Active", Value = "1" }));
            
            ViewBag.Status = new SelectList(statusList, "Value", "Text", Convert.ToInt16((currentUser.IsActive))).ToList();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        // POST: Admin/User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [CustomAuthorize("Admin", "Manager")]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,SaleDate,Status,ClientName,ClientEmail,ClientPhoneNo,ProductId,Capacity,Unit,RecentCallDate,AnticipatedClosingDate,NoOfFollowUps,Remarks,SalesRepresentativeId,InvoiceNo,InvoiceAmount,DateOfClosing,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy")] SaleActivity saleActivity)
        public ActionResult Edit(UserDetails userData)
        {
            JsonReponse jsonRes = null;

            try
            {
                var currentUser = (UserDetails)Session["UserDetails"];

                var user = db.Users.FirstOrDefault(wh => wh.Id == userData.Id);
                int lastSavedId = 0;
                if (user == null)
                {
                    jsonRes = new JsonReponse { message = "There is no record for given Id", status = "Failed", redirectURL = "" };
                }
                else
                {
                    if (!true)
                    {
                        jsonRes = new JsonReponse { message = "Enter all required fields.", status = "Failed", redirectURL = "" };
                    }
                    else
                    {
                        db.Users.Add(new User());
                        db.Entry(user).State = EntityState.Modified;
                        lastSavedId = db.SaveChanges();
                    }


                    if (lastSavedId > 0)
                    {
                        jsonRes = new JsonReponse { message = "User updated successfully!", status = "Success", redirectURL = "/Admin/User/Edit/" + user.Id };
                    }
                    else
                    {
                        jsonRes = new JsonReponse { message = "User update not completed, try again after sometime.", status = "Failed", redirectURL = "" };
                    }
                }
            }
            catch (Exception ex)
            {
                jsonRes = new JsonReponse { message = ex.Message, status = "Error", redirectURL = "" };
            }

            return Json(jsonRes, JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/User/Delete/5
        [CustomAuthorize("Admin")]
        public ActionResult Delete(int? id)
        {
            JsonReponse jsonRes = null;
            User user = new User();
            try
            {
                var currentUser = (UserDetails)Session["UserDetails"];

                user = db.Users.FirstOrDefault(wh => wh.Id == id);

                if (user == null)
                {
                    jsonRes = new JsonReponse { message = "There is no record for given Id", status = "Failed", redirectURL = "" };
                }
                else
                {
                    db.Users.Remove(user);
                    db.SaveChanges();

                    jsonRes = new JsonReponse { message = "user deleted successfully!", status = "Success", redirectURL = "/Admin/User/List/" };

                }
            }
            catch (Exception ex)
            {
                jsonRes = new JsonReponse { message = ex.Message, status = "Error", redirectURL = "" };
            }

            return Json(jsonRes, JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //// GET: Admin/User
        //public ActionResult Index()
        //{
        //    var users = db.Users.Include(u => u.User1).Include(u => u.User2).Include(u => u.User3);
        //    return View(users.ToList());
        //}

        //// GET: Admin/User/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    User user = db.Users.Find(id);
        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(user);
        //}

        //// GET: Admin/User/Create
        //public ActionResult Create()
        //{
        //    ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Password");
        //    ViewBag.ReportingManager = new SelectList(db.Users, "Id", "Password");
        //    ViewBag.CreatedBy = new SelectList(db.Users, "Id", "Password");
        //    return View();
        //}

        //// POST: Admin/User/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Password,FullName,Email,IsActive,ReportingManager,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,DateOfBirth,EmployeeId,AddressLine1,AddressLine2,State,City,Pincode")] User user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Users.Add(user);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Password", user.ModifiedBy);
        //    ViewBag.ReportingManager = new SelectList(db.Users, "Id", "Password", user.ReportingManager);
        //    ViewBag.CreatedBy = new SelectList(db.Users, "Id", "Password", user.CreatedBy);
        //    return View(user);
        //}

        //// GET: Admin/User/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    User user = db.Users.Find(id);
        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Password", user.ModifiedBy);
        //    ViewBag.ReportingManager = new SelectList(db.Users, "Id", "Password", user.ReportingManager);
        //    ViewBag.CreatedBy = new SelectList(db.Users, "Id", "Password", user.CreatedBy);
        //    return View(user);
        //}

        //// POST: Admin/User/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Password,FullName,Email,IsActive,ReportingManager,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,DateOfBirth,EmployeeId,AddressLine1,AddressLine2,State,City,Pincode")] User user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(user).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Password", user.ModifiedBy);
        //    ViewBag.ReportingManager = new SelectList(db.Users, "Id", "Password", user.ReportingManager);
        //    ViewBag.CreatedBy = new SelectList(db.Users, "Id", "Password", user.CreatedBy);
        //    return View(user);
        //}

        //// GET: Admin/User/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    User user = db.Users.Find(id);
        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(user);
        //}

        //// POST: Admin/User/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    User user = db.Users.Find(id);
        //    db.Users.Remove(user);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
