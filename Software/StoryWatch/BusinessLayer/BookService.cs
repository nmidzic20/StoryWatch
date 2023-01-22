﻿using EntitiesLayer.Entities;
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
                        Book bookAdd = new Book { Title = title, Summary = summary, Author = author };
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

        public void clearList()
        {
            bookInfo.Clear();
        }
    }
}
