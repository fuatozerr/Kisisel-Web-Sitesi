using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
    public class Test
    {

        private Repository<Category> repo_category = new Repository<Category>();
        private Repository<EverNoteUser> repo_user = new Repository<EverNoteUser>();
        private Repository<Note> repo_note = new Repository<Note>();
        private Repository<Comment> repo_comment = new Repository<Comment>();

        public Test()
        {
             List<Category> categories= repo_category.List();

        }
      
        public void InsertTest()
        {
            int result = repo_user.Insert(new EverNoteUser()
            {
                Name = "Ecrin",
                Surname = "Özer",
                Email = "ecrozerr23@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                Username = "ecrin",
                Password = "123456",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "fuatozer"

            });
        }

        public void UpdateTest()
        {

            EverNoteUser user = repo_user.Find(x=> x.Username=="fuatozerr");

            if(user!= null)
            {
                user.Username = "fuatozerrr";
                int result = repo_user.Update(user);
            }
        }

        public void DeleteTest()
        {
            EverNoteUser user = repo_user.Find(x => x.Username == "fuatozerr");
            if (user != null)
            {
                user.Username = "fuatozerrr";
                int result = repo_user.Delete(user);
            }

        }

        public void CommentTest()
        {
            EverNoteUser user = repo_user.Find(x => x.Username == "fuatozerr");
            Note note = repo_note.Find(x => x.Id == 3);

            Comment comment = new Comment()
            {
                Text = "bu bir testtir",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUsername = "futozerr",
                Note = note,
                Owner = user
            };

            repo_comment.Insert(comment);

        }
    }
}
