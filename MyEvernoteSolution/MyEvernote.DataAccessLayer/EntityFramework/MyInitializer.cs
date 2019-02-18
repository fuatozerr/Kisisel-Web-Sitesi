using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MyEvernote.Entities;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    public class MyInitializer : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            EverNoteUser admin = new EverNoteUser()
            {
                Name = "Fuat",
                Surname = "Özer",
                Email = "fuatozerr23@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                Username = "fuatozer",
                Password = "123456",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "fuatozer"

            };

            EverNoteUser standartUser = new EverNoteUser()
            {
                Name = "fatih",
                Surname = "Özer",
                Email = "fatihozerr23@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                Username = "fatihozerr",
                Password = "123456",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "fuatozer"

            };

            context.EvernoteUsers.Add(admin);
            context.EvernoteUsers.Add(standartUser);

            for (int i = 0; i < 8; i++)
            {
                EverNoteUser user = new EverNoteUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    Username = $"user{i}",
                    Password = "123",
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now.AddMinutes(5),
                    ModifiedUsername = "fuatozer"

                };
                context.EvernoteUsers.Add(user);

            }
            context.SaveChanges();

            List<EverNoteUser> userlist = context.EvernoteUsers.ToList();

            for (int i = 0; i < 10; i++)
            {
                Category cat = new Category()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUsername = "fuatozerr"
                };
                context.Categories.Add(cat);

                for (int k = 0; k < FakeData.NumberData.GetNumber(10,20); k++)
                {
                    EverNoteUser owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];
                    Note note = new Note()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        IsDraft=false,
                        LikeCount=FakeData.NumberData.GetNumber(10,50),
                        Owner=userlist[FakeData.NumberData.GetNumber(0,userlist.Count-1)],
                        CreatedOn=FakeData.DateTimeData.GetDatetime(),
                        ModifiedOn=DateTime.Now,
                        ModifiedUsername=owner.Username


                    };
                    cat.Notes.Add(note);

                    for (int j = 0; j < FakeData.NumberData.GetNumber(3,6); j++)
                    {
                        EverNoteUser owner_comment = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];

                        Comment comment = new Comment()
                        {

                            Text = FakeData.TextData.GetSentence(),
                            Owner = owner_comment,
                            CreatedOn = FakeData.DateTimeData.GetDatetime(),
                            ModifiedOn = DateTime.Now,
                            ModifiedUsername = owner_comment.Username



                        };
                        note.Comments.Add(comment);
                    }

                    for (int m = 0; m < FakeData.NumberData.GetNumber(1,3); m++)
                    {
                        Liked liked = new Liked()
                        {
                            LikedUser = userlist[m]

                        };
                        note.Likes.Add(liked);
                    }

                }


            }
            context.SaveChanges();

        }
    }
}
