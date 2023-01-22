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

        public IQueryable<Book> GetBookByName(string title) 
        {
            var query = from m in Entities
                        where m.Title == title
                        select m;

            return query;
        }

        public int DeleteBookFromList(BookListItem bookListItem)
        {
            Context.BookListItems.Attach(bookListItem);
            Context.BookListItems.Remove(bookListItem);
            return Context.SaveChanges();
        }

        public int Update(Book updateBook)
        {
            Book book = Entities.SingleOrDefault(e => e.Id == updateBook.Id);
            book.Title = updateBook.Title;
            book.Author = updateBook.Author;
            book.Summary = updateBook.Summary;
            return SaveChanges();
        }
    }
}
