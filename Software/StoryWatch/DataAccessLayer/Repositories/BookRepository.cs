using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class BookRepository : Repository<Book>
    {
        public BookRepository() { }

        public List<Book> GetBooksForListBox(BookListCategory bookListCategory, User loggedUser)
        {
            var user = Context.Users.Where(u => u.Id == loggedUser.Id).Single();
            var book_ids = user.BookListItems.Where(l => l.Id_BookListCategories == bookListCategory.Id).Select(l => l.Id_Books).ToList();

            List<Book> books = new List<Book>();

            foreach (var book in Entities)
                if (book_ids.Exists(id => id == book.Id))
                    books.Add(book);

            return books;
        }

        public int AddBookToList(BookListItem bookListItem) 
        {
            Context.BookListItems.Add(bookListItem);
            return Context.SaveChanges();
        }

        public override int Add(Book entity, bool saveChanges = true)
        {
            var genre = Context.Genres.SingleOrDefault(g => g.Id == entity.Genre.Id);

            entity.Genre = genre;

            Entities.Add(entity);

            if (saveChanges)
            {
                return SaveChanges();
            }
            else
            {
                return 0;
            }

        }

        public IQueryable<Book> GetBookByName(string title) 
        {
            var query = from m in Entities
                        where m.Title == title
                        select m;

            return query;
        }

        public int DeleteBookFromList(BookListItem bookListItem)
        {
            int isSuccessful;
            Context.BookListItems.Attach(bookListItem);
            Context.BookListItems.Remove(bookListItem);
            isSuccessful = Context.SaveChanges();

            if (Context.BookListItems.Count(m => m.Id_Books == bookListItem.Id_Books) == 0)
            {
                var unusedBook = Entities.SingleOrDefault(m => m.Id == bookListItem.Id_Books);
                Delete(unusedBook);
            }
            return isSuccessful;
        }

        public int Update(Book updateBook)
        {
            Book book = Entities.SingleOrDefault(e => e.Id == updateBook.Id);
            book.Title = updateBook.Title;
            book.Author = updateBook.Author;
            book.Summary = updateBook.Summary;
            book.Pages = updateBook.Pages;
            book.Genre = updateBook.Genre;
            book.Genre = Context.Genres.SingleOrDefault(g => g.Id == book.Genre.Id);
            return SaveChanges();
        }

        public IQueryable<Book> GetBookById(int id)
        {
            var query = from b in Entities.Include("Genre")
                        where b.Id == id
                        select b;
            return query;
        }

        public int UpdateBookListItem(BookListItem bookListItem, int idNewList, bool saveChanges = true)
        {

            BookListItem bookListItemDB = Context.BookListItems.Where(ml =>
                                        ml.Id_Books == bookListItem.Id_Books &&
                                        ml.Id_Users == bookListItem.Id_Users &&
                                        ml.Id_BookListCategories == bookListItem.Id_BookListCategories).SingleOrDefault();

            Context.BookListItems.Remove(bookListItemDB);
            Context.BookListItems.Add(new BookListItem
            {
                Id_Books = bookListItem.Id_Books,
                Id_BookListCategories = idNewList,
                Id_Users = bookListItem.Id_Users
            });

            if (saveChanges)
            {
                return Context.SaveChanges();
            }
            else
            {
                return 0;
            }
        }
    }
}
