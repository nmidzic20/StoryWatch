using EntitiesLayer.Entities;
using Goodreads;
using Goodreads.Models.Response;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class BookService
    {

        const string apiKey = "";
        const string apiSecret = "";
        IGoodreadsClient goodreads;
        public BookService() {
            goodreads = GoodreadsClient.Create(apiKey, apiSecret);
        }

        public async Task<string> GetBookTitleAsync(string title)
        {
            var book = await goodreads.Books.GetByTitle(title);
            return book.Title;
        }




    }
}
