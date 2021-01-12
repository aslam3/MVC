using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ProjectShopManagment.Models;
using ProjectShopManagment.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectShopManagment.Controllers
{
    [Authorize]
    public class ProfilesController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        // GET: Profiles
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var store = new UserStore<ApplicationUser>(context);
            var uManager = new UserManager<ApplicationUser>(store);
            var user = uManager.FindById(userId);
            ProfileVM profileVm = new ProfileVM
            {
                UserName = user.Email,
                Phonenumber = user.PhoneNumber,
                Age = user.Age,
                Address = user.Address,
                Fullname = user.Fullname,
                UserId = user.Id,
                Picture = user.Picture

            };
            return View(profileVm);
        }
        public ActionResult Edit(string uid)
        {
            var userId = User.Identity.GetUserId();
            var store = new UserStore<ApplicationUser>(context);
            var uManager = new UserManager<ApplicationUser>(store);
            var user = uManager.FindById(userId);
            ProfileVM profileVm = new ProfileVM
            {
                UserName = user.UserName,
                Email = user.Email,
                Phonenumber = user.PhoneNumber,
                Age = user.Age,
                Address = user.Address,
                Fullname = user.Fullname,
                UserId = user.Id,
                Pass = user.PasswordHash,
                SecStamp = user.SecurityStamp,
                Picture=user.Picture

            };
            return View(profileVm);
        }
       
        public PartialViewResult pic()
        {
            var store = new UserStore<ApplicationUser>(context);
            var uManager = new UserManager<ApplicationUser>(store);
            ApplicationUser user = new ApplicationUser();
            ProfileVM profileVm = new ProfileVM
            {
                Picture = user.Picture
            };
            return PartialView("pic", profileVm);
        }
        [HttpPost]
        public ActionResult Edit(ProfileVM profile)
        {
            var userId = User.Identity.GetUserId();
            var store = new UserStore<ApplicationUser>(context);
            var uManager = new UserManager<ApplicationUser>(store);
            ApplicationUser user = new ApplicationUser();
            user.Id = profile.UserId;
            user.UserName = profile.UserName;
            user.PhoneNumber = profile.Phonenumber;
            user.Address = profile.Address;
            user.Age = profile.Age;
            user.Email = profile.Email;
            user.Fullname = profile.Fullname;
            user.PasswordHash = profile.Pass;
            user.SecurityStamp = profile.SecStamp;
            
            if (profile.ImgFile != null)
            {
                string ext = Path.GetExtension(profile.ImgFile.FileName).ToLower();
                if (ext == ".jpg" || ext == ".png" || ext == ".jpeg")
                {
                    string fname = Path.GetFileNameWithoutExtension(profile.ImgFile.FileName) + "_" + Guid.NewGuid().ToString() + ext;
                    profile.ImgFile.SaveAs(Path.Combine(Server.MapPath("~/Upload"), fname));
                    user.Picture = "~/Upload/" + fname;
                }
            }
            else
            {
                var oldUser = uManager.FindById(userId);
                user.Picture = profile.Picture;
            }

            context.Entry(user).State = System.Data.Entity.EntityState.Modified;
            IdentityResult result = uManager.Update(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            if (result.Errors.Count() > 0)
            {
                foreach (string e in result.Errors)
                {
                    ModelState.AddModelError("", e + ",");
                }

            }
            return View(profile);
        }
    }
}