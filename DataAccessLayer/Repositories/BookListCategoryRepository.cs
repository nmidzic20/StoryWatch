﻿using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    /// <summary>
    /// Function: database communication for book list management
    /// Author: David Kajzogaj
    /// </summary>
    public class BookListCategoryRepository : Repository<BookListCategory>
    {
        public void DeleteListForUser(BookListCategory bookListCategory, User loggedUser)
        {
            var bookRepo = new BookRepository();
            var booksOnThisList = bookRepo.GetBooksForListBox(bookListCategory, loggedUser);
            foreach (var book in booksOnThisList)
            {
                //since books are deleted with the list
                //check if any list for any user, still references these books
                //if not, no sense in keeping the book in DB - delete it as well
                if (Context.BookListItems.Count(b => b.Id_Books == book.Id) == 1)
                {
                    var unusedBook = Context.Books.SingleOrDefault(b => b.Id == book.Id);
                    Context.Books.Remove(unusedBook);
                    SaveChanges();
                }
            }
            var user = Context.Users.FirstOrDefault(u => u.Id == loggedUser.Id);
            var bookList = user.BookListCategories
                .FirstOrDefault(ml => ml.Id == bookListCategory.Id);

            user.BookListCategories.Remove(bookList);
            SaveChanges();

            //additionally, check if this list category still used by any user and delete it if not
            bool bookListIsReferencedByAnotherUser = false;
            foreach (var u in Context.Users)
            {
                if (u.BookListCategories.Count(bl => bl.Id == bookList.Id) > 0)
                {
                    bookListIsReferencedByAnotherUser = true;
                    break;
                }
            }

            if (!bookListIsReferencedByAnotherUser)
            {
                Entities.Remove(bookList);
                SaveChanges();
            }
        }

        public int UpdateListForUser(BookListCategory entity, User loggedUser, bool saveChanges = true)
        {
            var bookListCategory = Entities.SingleOrDefault(e => e.Id == entity.Id);
            bookListCategory.Title = entity.Title;
            bookListCategory.Color = entity.Color;

            if (saveChanges)
            {
                return SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public IQueryable<BookListCategory> GetBookListCategories()
        {
            var query = from l in Entities
                        select l;

            return query;
        }
    }
}
