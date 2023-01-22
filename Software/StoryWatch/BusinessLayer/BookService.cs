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

        public bool AddBook(Book book)
        {
            bool saved;
            using(var db = new BookRepository())
            {
                var addedBook = db.Add(book);
                saved = addedBook > 0;
            }
            return saved;
        }

        public bool AddBookToList(BookListItem newBookList)
        {
            bool saved;
            using (var db = new BookRepository())
            {
                var addedBook = db.AddMovieToList(newBookList);
                saved = addedBook > 0;
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
    }
}
