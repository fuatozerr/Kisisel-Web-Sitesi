using MyEvernote.BusinessLayer;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using MyEvernote.Entities.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyEvernote.WebApp.ViewModels;

namespace MyEvernote.WebApp.Controllers
{
  
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //BusinessLayer.Test test = new BusinessLayer.Test();
            //test.CommentTest();

           

            NoteManager nm = new NoteManager();

           return View(nm.GetAllNote().OrderByDescending(x => x.ModifiedOn).ToList());
         //return View(nm.GetAllNoteQuryable().OrderByDescending(x => x.ModifiedOn).ToList());

        }


        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CategoryManager cm = new CategoryManager();

            Category cat = cm.GetCategoryById(id.Value);

            if (cat == null)
            {
                return new HttpNotFoundResult();
            }

            return View("Index",cat.Notes.OrderByDescending(x=>x.ModifiedOn).ToList());
        }

        public ActionResult MostLiked()
        {
            NoteManager nm = new NoteManager();


            return View("Index", nm.GetAllNote().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult ShowProfile()
        {
            EverNoteUser currentUser = Session["login"] as EverNoteUser;
            EvernoteUserManager eum = new EvernoteUserManager();
            BusinessLayerResult<EverNoteUser> res= eum.GetUserById(currentUser.Id);
            if(res.Errors.Count>0)
            {
                ErrorViewModel errorNotifObj = new ErrorViewModel()
                {
                    Title="Hata Oluştu",
                    Items=res.Errors
                };
            }
            return View(res.Result);
        }

        public ActionResult DeleteProfile()
        {
            EverNoteUser currentUser = Session["login"] as EverNoteUser;
            EvernoteUserManager eum = new EvernoteUserManager();
            BusinessLayerResult<EverNoteUser> res = eum.RemoveUserById(currentUser.Id);

            if(res.Errors.Count>0)
            {
                ErrorViewModel messages = new ErrorViewModel()
                {
                    Title = "Silme İşlemi BAŞARISIZ",
                    Items = res.Errors,
                    RedirectingUrl = "/Home/ShowProfile"
                };
                return View("Error", messages);

            }
            Session.Clear();
            return RedirectToAction("Index");

        }

        public ActionResult EditProfile()
        {
            EverNoteUser currentUser = Session["login"] as EverNoteUser;
            EvernoteUserManager eum = new EvernoteUserManager();
            BusinessLayerResult<EverNoteUser> res = eum.GetUserById(currentUser.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };
            }
            return View(res.Result);
        }

        [HttpPost]
        public ActionResult EditProfile(EverNoteUser model,HttpPostedFileBase ProfileImage)
        {
            if(ProfileImage !=null &&
               ( ProfileImage.ContentType=="image/jpeg" ||
                 ProfileImage.ContentType == "image/jpg" ||
                 ProfileImage.ContentType == "image/png"))
            {
                string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";
                ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                model.ProfileImageFileName = filename;

            }

            EvernoteUserManager eum = new EvernoteUserManager();
            BusinessLayerResult<EverNoteUser> res = eum.UpdateProfile(model);
            if(res.Errors.Count>0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = res.Errors,
                    Title = "Profil Güncellenmedi",
                    RedirectingUrl = "/Home/EditProfile"
                };
                return View("error", errorNotifyObj);
            }

            Session["login"] = res.Result;
                

                return RedirectToAction("ShowProfile");
        }

       
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
           

            if (ModelState.IsValid)
            {
                EvernoteUserManager eum = new EvernoteUserManager();
                BusinessLayerResult<EverNoteUser> res = eum.LoginUser(model);

                if (res.Errors.Count > 0)
                {
                    if (res.Errors.Find(x => x.Code == ErrorMessageCode.UserIsNotActive) != null)
                    {
                        ViewBag.SetLink = "http://Home/Activate/1234-5678-91230";
                    }
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                Session["login"] = res.Result;
                return RedirectToAction("Index");
                //session olcak
                //yönlendirme 
            }

                return View(model);
        }   

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        { 
            //kullanıcı adı kontrolü
            //kullanıcı e-posta kontrolü
            //kayıt işlemi
            //aktivcasyon kodu gönderimi

            if(ModelState.IsValid)
            {
                EvernoteUserManager eum = new EvernoteUserManager();
                BusinessLayerResult<EverNoteUser> res= eum.RegisterUser(model);
                if(res.Errors.Count>0)
                {
                 

                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));


                    return View();
                }

                /*EverNoteUser user = null;
                try
                {
                   user= eum.RegisterUser(model);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    
                }*/





                /*
                if(model.Username=="aacc")
                {
                    ModelState.AddModelError("", "Kullanıcı Kullanılıyor");
                }

                if(model.Email=="eeaf@hot.com")
                {
                    ModelState.AddModelError("", "eposta Kullanılıyor");

                }

                foreach (var item in ModelState)
                {
                    if(item.Value.Errors.Count>0)
                    {
                        return View(model);
                    }

                }

                return RedirectToAction("RegisterOk");
                if(user==null)
                {
                    return View(model);

                }*/

                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/Login",

                };

                notifyObj.Items.Add("Epostaya gelen linki tıklayın. ");

                return View("Ok",notifyObj);
            }
            return View(model);

        }

        

        public ActionResult UserActivate(Guid id)
        {
            EvernoteUserManager eum = new EvernoteUserManager();

           BusinessLayerResult<EverNoteUser> res= eum.ActivateUser(id);

            if(res.Errors.Count>0)
            {
                ErrorViewModel notifyObj = new ErrorViewModel()
                {
                    Title = "Geçersiz işlem",
                    Items = res.Errors
                };

                return View("Error", notifyObj);
            }

            OkViewModel okNotifyObj = new OkViewModel()
            {
                Title="Hesap Aktifleştirildi",
                RedirectingUrl="/Home/Login",
                
                
            };

            okNotifyObj.Items.Add("Hesabınız aktifleştirildi");

            return View("ok",okNotifyObj);
        }
         
    }
}