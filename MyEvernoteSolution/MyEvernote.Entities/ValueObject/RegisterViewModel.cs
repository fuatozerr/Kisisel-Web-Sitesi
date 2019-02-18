using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyEvernote.Entities.ValueObject
{
    public class RegisterViewModel
    {
        [DisplayName("Kullanıcı Adı"), Required(ErrorMessage = "{0} alanı boş geçilemez"), StringLength(25)]
        public string Username { get; set; }

        [DisplayName("Mail Adresiniz"), Required(ErrorMessage = "{0} alanı boş geçilemez"), StringLength(25),EmailAddress]
        public string Email { get; set; }

        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı boş geçilemez"), StringLength(25), DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Şifre Tekrarı"), Required(ErrorMessage = "{0} alanı boş geçilemez"), StringLength(25), DataType(DataType.Password),
            Compare("Password",ErrorMessage ="Şifreler Uyuşmuyor")]
        public string RePassword { get; set; }

    }
}