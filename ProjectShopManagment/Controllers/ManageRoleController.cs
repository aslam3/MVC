using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ProjectShopManagment.Models;
using ProjectShopManagment.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectShopManagment.Controllers
{
    public class ManageRoleController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: ManageRole
        public ActionResult Index()
        {
            return View(db.Roles.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(RolesVM roles)
        {
            try
            {
                var role = new IdentityRole
                {
                    Name = roles.Name
                };
                db.Roles.Add(role);
                if (db.SaveChanges() > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
            {
                ViewBag.result = ex.Message;
            }
            return View(roles);
        }
        [HttpGet]
        public ActionResult AssignRole()
        {
            ViewBag.Userlist = db.Users.Select(s => new SelectListItem
            { Text = s.UserName, Value = s.Id });
            ViewBag.RoleList = db.Roles.Select(s => new SelectListItem
            { Text = s.Name, Value = s.Name });
            return View();
        }
        [HttpPost]
        public ActionResult AssignRole(string Userlist,string RoleList)
        {
            ViewBag.Userlist = db.Users.Select(s => new SelectListItem
            { Text = s.UserName, Value = s.Id });
            ViewBag.RoleList = db.Roles.Select(s => new SelectListItem
            { Text = s.Name, Value = s.Name });

            var store = new UserStore<ApplicationUser>(db);
            var uManager = new UserManager<ApplicationUser>(store);
            IdentityResult result = uManager.AddToRole(Userlist, RoleList);
            if (result.Succeeded)
            {
                ViewBag.result = "Successfully assigned role";
            }
            else
            {
                ViewBag.result = "Failed";
            }
            return View();
        }
        public ActionResult UserRole()
        {
            ViewBag.Userlist = db.Users.Select(s => new SelectListItem
            { Text = s.UserName, Value = s.Id });
            return View();
        }
        [HttpPost]
        public ActionResult UserRole(string Userlist)
        {
            ViewBag.Userlist = db.Users.Select(s => new SelectListItem
            { Text = s.UserName, Value = s.Id });

            var store = new UserStore<ApplicationUser>(db);
            var uManager = new UserManager<ApplicationUser>(store);
            if (!String.IsNullOrEmpty(Userlist))
            {
                var roleList = uManager.GetRoles(Userlist);
                ViewBag.roles = new SelectList(roleList);
            }
            else
            {
                ViewBag.result = "Provide User id";
            }
            return View();
        }

        public ActionResult GetUsersRole()
        {
            IEnumerable<UsersRole> all = new List<UsersRole>();
            try
            {
                all = (from u in db.Users
                       select new
                       {
                           UserId = u.Id,
                           UserName = u.UserName,
                           RoleNames = (from ur in u.Roles
                                        join r in db.Roles
                                        on ur.RoleId equals r.Id
                                        select r.Name
                                      )
                       }).ToList().Select(s => new UsersRole
                       {
                           UserID = s.UserId,
                           UserName = s.UserName,
                           RoleName = string.Join(",", s.RoleNames)
                       }

                     );
            }
            catch(Exception ex)
            {
                ViewBag.result = ex.Message;
            }
            return View(all);
        }
        public ActionResult RemoveUsersRole()
        {
            ViewBag.Userlist = db.Users.Select(s => new SelectListItem
            {
                Text = s.UserName,
                Value = s.Id
            });
            ViewBag.RoleList = db.Roles.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id
            });
            return View();
        }
        [HttpPost]
        public ActionResult RemoveUsersRole(string Userlist,string RoleList)
        {
            ViewBag.Userlist = db.Users.Select(s => new SelectListItem
            {
                Text = s.UserName,
                Value = s.Id
            });
            ViewBag.RoleList = db.Roles.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id
            });
            var store = new UserStore<ApplicationUser>(db);
            var uManager = new UserManager<ApplicationUser>(store);
            IdentityResult result = uManager.RemoveFromRole(Userlist, RoleList);
            if (result.Succeeded)
            {
                ViewBag.result = "Remove successfully";
            }
            if (result.Errors.Count() > 0)
            {
                
            }
            return View();
        }
    }
}