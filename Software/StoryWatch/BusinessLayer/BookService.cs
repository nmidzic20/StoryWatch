using EntitiesLayer.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using DataAccessLayer.Repositories;
using TMDbLib.Objects.Movies;

namespace BusinessLayer
{
    public class BookService
    {
        List<Book> bookInfo;
        HttpClient bookClient = new HttpClient();
        public const string bookURL = "https://www.googleapis.com/books/v1/volumes/";
        public BookService()
        {
            bookClient.BaseAddress = new Uri(bookURL);
            bookInfo = new List<Book>();
        }

        public List<Book> findBookByName(string name)
        {
            HttpResponseMessage response;
            string urlParameters = "?q=" + name;
            response = bookClient.GetAsync(urlParameters).Result;

            if (response.IsSuccessStatusCode)
            {
                JObject bookJson = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                JArray books = (JArray)bookJson["items"];
                foreach (var book in books)
                {
                    JObject volumeInfoObject = (JObject)book["volumeInfo"];
                    JArray autor = (JArray)volumeInfoObject["authors"];
                    string title = (string)volumeInfoObject["title"];
                    string summary = (string)volumeInfoObject["description"];
                    if (autor != null)
                    {
                        string author = (string)autor[0];
                        Book bookAdd = new Book { Title = title, Summary = summary, Author = author};
                        bookInfo.Add(bookAdd);
                    }
                    else
                    {
                        Book bookAdd = new Book { Title = title, Summary = summary };
                        bookInfo.Add(bookAdd);
                    }
                }
            }
            return bookInfo;
        }

        public List<Book> returnCurrentBookList()
        {
            return bookInfo;
        }

        public Book AddBook(Book book)
        {
            bool saved;
            using(var db = new BookRepository())
            {
                List<Book> addBook = db.GetAll().ToList();
                Book foundBookTitleAndAuthor = addBook.FirstOrDefault(b => b.Title == book.Title && b.Author == book.Author);
                Book foundBookAuthorAndSummary = addBook.FirstOrDefault(b => b.Author == book.Author && b.Summary == book.Summary);
                Book foundBookSummaryAndTitle = addBook.FirstOrDefault(b => b.Summary == book.Summary && b.Title == book.Title);

                if (foundBookTitleAndAuthor != null)
                {
                    return foundBookTitleAndAuthor;
                }
                else if(foundBookAuthorAndSummary != null)
                {
                    return foundBookAuthorAndSummary;
                }
                else if(foundBookSummaryAndTitle != null)
                {
                    return foundBookSummaryAndTitle;
                }
                else
                {
                    var addedBook = db.Add(book);
                    saved = addedBook > 0;
                    return null;
                }
            }
            return null;
        }

        public bool AddBookToList(Book book, BookListItem newBookList, BookListCategory bookListCategory, User loggedUser)
        {
            bool saved = false;
            using (var db = new BookRepository())
            {
                List<Book> books = db.GetBooksForListBox(bookListCategory, loggedUser).ToList();
                Book bookTitleandAuthorExistsOnList = books.FirstOrDefault(b => b.Title == book.Title && b.Author == book.Author);
                Book bookTitleAndSummaryExistsOnList = books.FirstOrDefault(b => b.Author == book.Author && b.Summary == book.Summary);
                Book bookSummaryAndAuthorExistsOnList = books.FirstOrDefault(b => b.Summary == book.Summary && b.Author == b.Author);

                if (bookTitleandAuthorExistsOnList != null)
                {
                   saved = false;
                }
                else if(bookTitleAndSummaryExistsOnList != null)
                {
                    saved = false;
                }
                else if(bookSummaryAndAuthorExistsOnList != null)
                {
                    saved = false;
                }
                else
                {
                    int affectedRows = db.AddBookToList(newBookList);
                    saved = affectedRows > 0;
                }
            }
            return saved;
        }

        public void clearList()
        {
            bookInfo.Clear();
        }

        public List<Book> GetBooksForList(BookListCategory bookListCategory, User loggedUser)
        {
            using(var db = new BookRepository())
            {
                return db.GetBooksForListBox(bookListCategory, loggedUser);
            }
        }

        public List<Book> GetAll()
        {
            using(var db = new BookRepository())
            {
                return db.GetAll().ToList();
            }
        }

        public Book GetBookByTitle(string title)
        {
            using(var db = new BookRepository())
            {
                return db.GetBookByName(title).Single();
            }
        }

        public bool DeleteBookFromList(Book selectedBook, BookListCategory bookListCategory, User loggedUser)
        {
            bool isSuccessful = false;

            var bookListItem = new BookListItem
            {
                Id_BookListCategories = bookListCategory.Id,
                Id_Books = selectedBook.Id,
                Id_Users = loggedUser.Id
            };

            using (var db = new BookRepository())
            {
                int affectedRows = db.DeleteBookFromList(bookListItem);
                isSuccessful = affectedRows > 0;

                return isSuccessful;
            }
        }
        public int UpdateBook(Book book)
        {
            using (var db = new BookRepository())
            {
                return db.Update(book);
            }
        }
    }
}
