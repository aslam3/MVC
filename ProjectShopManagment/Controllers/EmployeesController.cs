using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectShopManagment.Models;
using ProjectShopManagment.ViewModel;

namespace ProjectShopManagment.Controllers
{
    public class EmployeesController : Controller
    {
        private DbShoppingMallEntities db = new DbShoppingMallEntities();

        // GET: Employees
        public ActionResult Index()
        {
            
            ViewBag.Desig = new SelectList(db.Salaries, "ID", "Designation");
            return View();
        }
        public JsonResult AllEmployee()
        {
            var emp = db.Employees.Include(s => s.Salary);
            
           
            List<Employee> em = db.Employees.OrderBy(e => e.Name).ToList();
            var cobj = em.Select(s => new { Name = s.Name, Gender = s.Gender, DesigID=s.DesigID,Join_Date=s.Join_Date, ID = s.ID});
            return Json(new { data = cobj }, JsonRequestBehavior.AllowGet);
        }
       
        public JsonResult Save(EmployeeVm em)
        {
            if (ModelState.IsValid)
            {
                Employee e = new Employee();
                //if (em.ID > 0)
                //{

                //}
                if (em.Imgfile != null)
                {
                    string fname = em.Imgfile.FileName;
                    string ext = Path.GetExtension(fname).ToLower();
                    if(ext==".jpg"|| ext == ".png"|| ext == ".jpeg")
                    {
                        string file = Path.GetFileNameWithoutExtension(fname) + "_" + Guid.NewGuid() + ext;
                        em.Imgfile.SaveAs(Path.Combine(Server.MapPath("~/EmUpload"), file));
                        em.Picture = "~/EmUpload/" + file;
                    }
                }
                e.Name = em.Name;
                e.Gender = Enum.GetName(typeof(Gender),em.Gender); 
                e.DesigID = em.DesigID;
                e.Join_Date = em.Join_Date;
                

                db.Employees.Add(e);
                if (db.SaveChanges() > 0)
                {
                    return Json(new { result = em });
                }
                else
                {
                    return Json(new { result = "Failed" });
                }
            }
            return Json(new { result = "Invalid Object" });
        }
        public JsonResult GetById(int? id)
        {
            var em = db.Employees.Find(id);
            return Json(em, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(EmployeeVm em)
        {
            if (ModelState.IsValid)
            {
                Employee e = new Employee();
                if (em.ID > 0)
                {
                    e.Name = em.Name;
                    e.Gender = Enum.GetName(typeof(Gender), em.Gender);
                    e.DesigID = em.DesigID;
                    e.Join_Date = em.Join_Date;
                    e.ID = em.ID;
                    db.Entry(e).State = System.Data.Entity.EntityState.Modified;
                }
                if (db.SaveChanges() > 0)
                {
                    return Json(new { result = em });
                }
                else
                {
                    return Json(new { result = "Failed" });
                }
            }
            return Json(new { result = "Invalid Object" });
        }
        public JsonResult DeleteEm(int id)
        {
            var em = db.Employees.Find(id);
            db.Employees.Remove(em);
            if (db.SaveChanges() > 0)
            {
                return Json(new { result = "Success" },JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = "Failed" });
            }
        }
        // GET: Employees/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Employee employee = db.Employees.Find(id);
        //    if (employee == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(employee);
        //}

        //// GET: Employees/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Employees/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,Name,DesigID,TotalSalary,Join_Date")] Employee employee)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Employees.Add(employee);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(employee);
        //}

        //// GET: Employees/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Employee employee = db.Employees.Find(id);
        //    if (employee == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(employee);
        //}

        //// POST: Employees/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID,Name,DesigID,TotalSalary,Join_Date")] Employee employee)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(employee).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(employee);
        //}

        //// GET: Employees/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Employee employee = db.Employees.Find(id);
        //    if (employee == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(employee);
        //}

        //// POST: Employees/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Employee employee = db.Employees.Find(id);
        //    db.Employees.Remove(employee);
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
