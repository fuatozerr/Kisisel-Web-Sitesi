using MyEvernote.BusinessLayer.Abstract;
using MyEvernote.BusinessLayer.Results;
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
   public class EvernoteUserManager : ManagerBase<EverNoteUser>
    {


        public BusinessLayerResult<EverNoteUser> RegisterUser(RegisterViewModel data)
        {
            //kullanıcı adı kontrolü
            //kullanıcı e-posta kontrolü
            //kayıt işlemi
            //aktivcasyon kodu gönderimi
            EverNoteUser user = Find(x => x.Username == data.Username || x.Email==data.Email);
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
               int dbResult= Insert(new EverNoteUser()
                {
                    Username=data.Username,
                    Email=data.Email,
                    Password=data.Password,
                    ActivateGuid=Guid.NewGuid(),
                   ProfileImageFilename = "User_boy.png",
                    IsActive=false,
                    IsAdmin=false

                });

                if(dbResult>0)
                {
                   res.Result=  Find(x => x.Email == data.Email && x.Username == data.Username);

                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{ siteUri}/Home/UserActivate/{res.Result.ActivateGuid}";
                    string body = $"{res.Result.Name}; <br><br> Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayınız </a>.";
                    MailHelper.SendMail(body,res.Result.Email,"Hesap Aktifleştir");
                }
            }
            return res;    
        } //kullanıcı kayıt işlemi

        public BusinessLayerResult<EverNoteUser> RemoveUserById(int id)
        {
            BusinessLayerResult<EverNoteUser> res = new BusinessLayerResult<EverNoteUser>();

            EverNoteUser user =  Find(x => x.Id == id);
            if(user!=null)
            {
                if( Delete(user)==0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı Silinemedi");
                }
                else
                {
                    res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı Bulunamadı");
                }
            }
            return res;

        }

        public BusinessLayerResult<EverNoteUser> LoginUser(LoginViewModel data)
        {
            //giriş kontrolü
            //aktif mi?
            BusinessLayerResult<EverNoteUser> res = new BusinessLayerResult<EverNoteUser>();
 
            res.Result =  Find(x => x.Username == data.Username && x.Password == data.Password);


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

            res.Result =  Find(x => x.ActivateGuid == activateId);

            if(res.Result !=null )
            {
                if(res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı Zaten Aktif");
                    return res;
                }

                res.Result.IsActive = true;
                 Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNoteExist, "Aktif Kullanıcı Bulunamamıştır");
                return res;
            }
            return res;
        } //dogrulama kodu gönderme işlemi

        public BusinessLayerResult<EverNoteUser> UpdateProfile(EverNoteUser data)  //update işlemi
        {
            EverNoteUser db_user =  Find(x => x.Id != data.Id && (x.Username == data.Username || x.Email == data.Email));
            BusinessLayerResult<EverNoteUser> res = new BusinessLayerResult<EverNoteUser>();
            
            if(db_user != null && db_user.Id != data.Id)
            {
                if(db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UserNameAlreadyExists, "Kullanıcı adı kayıtlı");
                }

                if(db_user.Email ==data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "Mail adresi kayıtlı");
                }

                return res;
            }


            res.Result =  Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Username = data.Username;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;

            if(string.IsNullOrEmpty(data.ProfileImageFilename) ==false)
            {
                res.Result.ProfileImageFilename = data.ProfileImageFilename;
            }

            if( Update(res.Result)==0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Profil Güncellenmedi");
            }
            return res;
                
        }

        public BusinessLayerResult<EverNoteUser> GetUserById(int id)
        {
            BusinessLayerResult<EverNoteUser> res = new BusinessLayerResult<EverNoteUser>();
            res.Result =  Find(x => x.Id == id);
            if(res.Result==null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı Bulunamadı");

            }
            return res;

        }


    }
}
