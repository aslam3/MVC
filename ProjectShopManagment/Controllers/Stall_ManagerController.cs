using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;

using ProjectShopManagment.Models;

namespace ProjectShopManagment.Controllers
{
    public class Stall_ManagerController : Controller
    {
        private DbShoppingMallEntities db = new DbShoppingMallEntities();
       [Authorize(Roles ="admin")]
        // GET: Stall_Manager
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            
                
                ViewBag.NameSortParam = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.StallSortParam = sortOrder == "stall" ? "stall_desc" : "stall";
                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }
                ViewBag.CurrentFilter = searchString;
                var manager = from s in db.Stall_Manager select s;

                if (!String.IsNullOrEmpty(searchString))
                {
                    manager = manager.Where(s => s.ManagerName.ToUpper().Contains(searchString.ToUpper()));
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        manager = manager.OrderByDescending(s => s.ManagerName);
                        break;
                    case "stall_desc":
                        manager = manager.OrderByDescending(s => s.Stall.StallName);
                        break;
                    case "stall":
                        manager = manager.OrderBy(s => s.Stall.StallName);
                        break;
                    default:
                        manager = manager.OrderBy(s => s.ManagerName);
                        break;
                }
                ViewBag.CurrentSort = sortOrder;
                int pageSize = 5;
                int pageNumber = (page ?? 1);
          
            var stall_Manager = db.Stall_Manager.Include(s => s.Stall);
            return View(manager.ToPagedList(pageNumber, pageSize));
           
        }

        // GET: Stall_Manager/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stall_Manager stall_Manager = db.Stall_Manager.Find(id);
            if (stall_Manager == null)
            {
                return HttpNotFound();
            }
            return View(stall_Manager);
        }

        // GET: Stall_Manager/Create
        public ActionResult Create()
        {
            ViewBag.M_Stall = new SelectList(db.Stalls, "ID", "StallName");
            return View();
        }

        // POST: Stall_Manager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "ID,ManagerName,Email,M_Stall,Image")]*/ Stall_Manager stall_Manager)
        {
           
            if (ModelState.IsValid)
            {
                if (stall_Manager.Imgfile != null)
                {
                    string ext = Path.GetExtension(stall_Manager.Imgfile.FileName).ToLower();
                    if (ext == ".jpg" || ext == ".png" || ext == ".jpeg")
                    {
                        string fname = Path.GetFileNameWithoutExtension(stall_Manager.Imgfile.FileName) + "_" + Guid.NewGuid().ToString() + ext;
                        stall_Manager.Imgfile.SaveAs(Path.Combine(Server.MapPath("~/Upload"), fname));
                        stall_Manager.Image = "~/Upload/" + fname;
                    }
                }
                db.Stall_Manager.Add(stall_Manager);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.M_Stall = new SelectList(db.Stalls, "ID", "StallName", stall_Manager.M_Stall);
            return View(stall_Manager);
        }

        // GET: Stall_Manager/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stall_Manager stall_Manager = db.Stall_Manager.Find(id);
            if (stall_Manager == null)
            {
                return HttpNotFound();
            }
            ViewBag.M_Stall = new SelectList(db.Stalls, "ID", "StallName", stall_Manager.M_Stall);
            return View(stall_Manager);
        }

        // POST: Stall_Manager/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(/*[Bind(Include = "ID,ManagerName,Email,M_Stall,Image")]*/ Stall_Manager stall_Manager)
        {
            
            if (ModelState.IsValid)
            {
                if (stall_Manager.Imgfile != null)
                {
                    string ext = Path.GetExtension(stall_Manager.Imgfile.FileName).ToLower();
                    if (ext == ".jpg" || ext == ".png" || ext == ".jpeg")
                    {
                        string fname = Path.GetFileNameWithoutExtension(stall_Manager.Imgfile.FileName) + "_" + Guid.NewGuid().ToString() + ext;
                        stall_Manager.Imgfile.SaveAs(Path.Combine(Server.MapPath("~/Upload"), fname));
                        stall_Manager.Image = "~/Upload/" + fname;
                    }
                }
                db.Entry(stall_Manager).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.M_Stall = new SelectList(db.Stalls, "ID", "StallName", stall_Manager.M_Stall);
            return View(stall_Manager);
        }

        // GET: Stall_Manager/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stall_Manager stall_Manager = db.Stall_Manager.Find(id);
            if (stall_Manager == null)
            {
                return HttpNotFound();
            }
            return View(stall_Manager);
        }

        // POST: Stall_Manager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Stall_Manager stall_Manager = db.Stall_Manager.Find(id);
            db.Stall_Manager.Remove(stall_Manager);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
