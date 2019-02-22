﻿using MyEvernote.BusinessLayer.Abstract;
using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
   public class CategoryManager : ManagerBase<Category>
    {

        public override int Delete(Category category)
        {
            NoteManager noteManager = new NoteManager();
            LikedManager likedManager = new LikedManager();
            CommentManager commentManager = new CommentManager();
            //kategoride ilişkili notlar silincek
            foreach (Note note in category.Notes.ToList())
            {
                //kategoride ilişkili beğeni silincek

                foreach (Liked like in note.Likes.ToList())
                {
                    likedManager.Delete(like);
                }
                //kategoride ilişkili yorumlar silincek

                foreach (Comment comment in note.Comments.ToList())
                {
                    commentManager.Delete(comment);
                }

                noteManager.Delete(note);
            }

            return base.Delete(category);
        }


    }
}
