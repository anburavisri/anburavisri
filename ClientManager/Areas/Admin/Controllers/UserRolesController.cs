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
    public class UserRolesController : Controller
    {
        private ClientManagerEntities db = new ClientManagerEntities();

        [CustomAuthorize("Admin")]
        // GET: Admin/UserRoles
        public ActionResult Index()
        {
            var userRoles = db.UserRoles.Include(u => u.Role).Include(u => u.User).Include(u => u.User1).Include(u => u.User2).Include(u => u.User3);
            return View(userRoles.ToList());
        }

        // GET: Admin/UserRoles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DBOperation.UserRole userRole = db.UserRoles.Find(id);
            if (userRole == null)
            {
                return HttpNotFound();
            }
            return View(userRole);
        }

        [CustomAuthorize("Admin")]
        // GET: Admin/UserRoles/Create
        public ActionResult Create(int userId = 1)
        {
            var assignedRoles = db.UserRoles.Where(wh => wh.UserId == userId).Select(sel => new { RoleId = sel.RoleId, RoleName = sel.Role.RoleName }).ToList();
            var assignedRoleNames = assignedRoles.Select(sel => sel.RoleName);
            ViewBag.AssignedRoles = new SelectList(assignedRoles, "RoleId", "RoleName");
            ViewBag.AvailableRoles = new SelectList(db.Roles.Where(wh => !assignedRoleNames.Contains(wh.RoleName)), "Id", "RoleName");
            ViewBag.Users = new SelectList(db.Users, "Id", "FullName", userId);
            return View();
        }

        [HttpPost]
        [CustomAuthorize("Admin")]
        public ActionResult Create(UserRoleData userRoleData)
        {
            JsonReponse jsonRes = null;

            try
            {
                var currentUser = (UserDetails)Session["UserDetails"];

                var messagetext = "";
                int lastSavedId = 0;


                if (userRoleData.UserId <= 0 || userRoleData.SelectedRoles == null)
                {
                    jsonRes = new JsonReponse { message = "Enter all required fields.", status = "Failed", redirectURL = "" };
                }
                else
                {
                    if (currentUser.UserRoles.Any(wh => wh.RoleName.ToLower() == "admin"))
                    {
                        db.UserRoles.RemoveRange(db.UserRoles.Where(wh => wh.UserId == userRoleData.UserId));

                        int idx = 0;
                        foreach (var RoleId in userRoleData.SelectedRoles)
                        {

                            db.UserRoles.Add(new DBOperation.UserRole { UserId = userRoleData.UserId, RoleId = userRoleData.SelectedRoles[idx], CreatedOn = DateTime.Now, CreatedBy = currentUser.Id });
                            idx++;
                        }

                        messagetext = "User Roles updated ";
                        lastSavedId = db.SaveChanges();
                    }
                }


                if (lastSavedId > 0)
                {
                    jsonRes = new JsonReponse { message = messagetext + " successfully!", status = "Success", redirectURL = "/Admin/UserRoles/Create" };
                }
                else
                {
                    jsonRes = new JsonReponse { message = messagetext + " not completed, try again after sometime.", status = "Failed", redirectURL = "" };
                }

            }
            catch (Exception ex)
            {
                jsonRes = new JsonReponse { message = ex.Message, status = "Error", redirectURL = "" };
            }

            return Json(jsonRes, JsonRequestBehavior.AllowGet);
        }

        //// POST: Admin/UserRoles/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(UserRoleData userRoleData)
        //{
        //    JsonReponse jsonRes = null;
        //    var currentUser = (UserDetails)Session["UserDetails"];
        //    var assignedRoles = db.UserRoles.Where(wh => wh.UserId == 1).Select(sel => new { RoleId = sel.RoleId, RoleName = sel.Role.RoleName }).ToList();
        //    var assignedRoleNames = assignedRoles.Select(sel => sel.RoleName);
        //    ViewBag.AssignedRoles = new SelectList(assignedRoles, "RoleId", "RoleName");
        //    ViewBag.AvailableRoles = new SelectList(db.Roles.Where(wh => !assignedRoleNames.Contains(wh.RoleName)), "Id", "RoleName");
        //    ViewBag.Users = new SelectList(db.Users, "Id", "FullName");

        //    try
        //    {
        //        var userRoles = db.UserRoles.FirstOrDefault(wh => wh.Id == userRoleData.UserId);
        //        int lastSavedId = 0;

        //        if (userRoleData.UserId <= 0 || userRoleData.SelectedRoles == null)
        //        {
        //            jsonRes = new JsonReponse { message = "Enter all required fields.", status = "Failed", redirectURL = "" };
        //        }
        //        else
        //        {
        //            db.Users.Add(new User { FullName = userData.FullName, Email = userData.UserId, Password = userData.Password, AddressLine1 = userData.AddressLine1, AddressLine2 = userData.AddressLine2, City = userData.City, State = userData.State, Pincode = userData.PinCode, IsActive = userData.IsActive, DateOfBirth = userData.DateOfBirth, DateOfJoining = userData.DateofJoining, EmployeeId = userData.EmpId, ReportingManager = userData.ReportingManager, SaleTarget = userData.SaleTarget, CreatedBy = currentUser.Id, CreatedOn = DateTime.Now });
        //            lastSavedId = db.SaveChanges();
        //        }

        //        if (lastSavedId > 0)
        //        {
        //            jsonRes = new JsonReponse { message = "User created successfully!", status = "Success", redirectURL = "/Admin/User/List" };
        //        }
        //        else
        //        {
        //            jsonRes = new JsonReponse { message = "User creatikon not completed, try again after sometime.", status = "Failed", redirectURL = "" };
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        jsonRes = new JsonReponse { message = ex.Message, status = "Error", redirectURL = "" };
        //    }

        //    return Json(jsonRes, JsonRequestBehavior.AllowGet);
        //}

        //// GET: Admin/UserRoles/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserRole userRole = db.UserRoles.Find(id);
        //    if (userRole == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.RoleId = new SelectList(db.Roles, "Id", "RoleName", userRole.RoleId);
        //    ViewBag.CreatedBy = new SelectList(db.Users, "Id", "Password", userRole.CreatedBy);
        //    ViewBag.UserId = new SelectList(db.Users, "Id", "Password", userRole.UserId);
        //    ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Password", userRole.ModifiedBy);
        //    ViewBag.UserId = new SelectList(db.Users, "Id", "Password", userRole.UserId);
        //    return View(userRole);
        //}

        // POST: Admin/UserRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,UserId,RoleId,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy")] UserRole userRole)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(userRole).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.RoleId = new SelectList(db.Roles, "Id", "RoleName", userRole.RoleId);
        //    ViewBag.CreatedBy = new SelectList(db.Users, "Id", "Password", userRole.CreatedBy);
        //    ViewBag.UserId = new SelectList(db.Users, "Id", "Password", userRole.UserId);
        //    ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Password", userRole.ModifiedBy);
        //    ViewBag.UserId = new SelectList(db.Users, "Id", "Password", userRole.UserId);
        //    return View(userRole);
        //}

        //// GET: Admin/UserRoles/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserRole userRole = db.UserRoles.Find(id);
        //    if (userRole == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(userRole);
        //}

        //// POST: Admin/UserRoles/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    UserRole userRole = db.UserRoles.Find(id);
        //    db.UserRoles.Remove(userRole);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
{
    if (disposing)
    {
        db.Dispose();
    }
    base.Dispose(disposing);
}
    }
}
