using MyEvernote.Common.Helpers;
using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using MyEvernote.Entities.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
   public class EvernoteUserManager
    {
        private Repository<EverNoteUser> repo_user = new Repository<EverNoteUser>();


        public BusinessLayerResult<EverNoteUser> RegisterUser(RegisterViewModel data)
        {
            //kullanıcı adı kontrolü
            //kullanıcı e-posta kontrolü
            //kayıt işlemi
            //aktivcasyon kodu gönderimi
            EverNoteUser user = repo_user.Find(x => x.Username == data.Username || x.Email==data.Email);
            BusinessLayerResult<EverNoteUser> res = new BusinessLayerResult<EverNoteUser>();

            

            if (user!=null)
            {
                if(user.Username==data.Username)
                {
                    res.AddError(Entities.Messages.ErrorMessageCode.UserNameAlreadyExists,"Kullanıcı Adı kayıtlı");

                }
                if(user.Email==data.Email)
                {
                    res.AddError(Entities.Messages.ErrorMessageCode.EmailAlreadyExists, "Mail Kayıtlı");
                }
            }
            else
            {
               int dbResult= repo_user.Insert(new EverNoteUser()
                {
                    Username=data.Username,
                    Email=data.Email,
                    Password=data.Password,
                    ActivateGuid=Guid.NewGuid(),
                    
                    IsActive=false,
                    IsAdmin=false

                });

                if(dbResult>0)
                {
                   res.Result= repo_user.Find(x => x.Email == data.Email && x.Username == data.Username);

                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{ siteUri}/Home/UserActivate/{res.Result.ActivateGuid}";
                    string body = $"{res.Result.Name}; <br><br> Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayınız </a>.";
                    MailHelper.SendMail(body,res.Result.Email,"Hesap Aktifleştir");
                }
            }
            return res;    
        } //kullanıcı kayıt işlemi


        public BusinessLayerResult<EverNoteUser> LoginUser(LoginViewModel data)
        {
            //giriş kontrolü
            //aktif mi?
            BusinessLayerResult<EverNoteUser> res = new BusinessLayerResult<EverNoteUser>();
 
            res.Result = repo_user.Find(x => x.Username == data.Username && x.Password == data.Password);


            if (res.Result != null)
            {
                if (!res.Result.IsActive)
                {
                    res.AddError(Entities.Messages.ErrorMessageCode.UserIsNotActive, "Kullanıcı aktif değil. ");

                    res.AddError(Entities.Messages.ErrorMessageCode.CheckYourMail, " Maili kontrol edin");
                }
            }
            else
            {
                res.AddError(Entities.Messages.ErrorMessageCode.UsernameOrPassWrong, "Kullanıcı ya da şifre uyuşmuyor");
            }
            return res;
        }   //kullanıcı giriş işlemi


        public BusinessLayerResult<EverNoteUser> ActivateUser(Guid activateId)
        {
            BusinessLayerResult<EverNoteUser> res = new BusinessLayerResult<EverNoteUser>();

            res.Result = repo_user.Find(x => x.ActivateGuid == activateId);

            if(res.Result !=null )
            {
                if(res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı Zaten Aktif");
                    return res;
                }

                res.Result.IsActive = true;
                repo_user.Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNoteExist, "Aktif Kullanıcı Bulunamamıştır");
                return res;
            }
            return res;
        } //dogrulama kodu gönderme işlemi


    }
}
