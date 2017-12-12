using Common.Infrastructure.Entities;
using Common.Infrastructure.Utilities;
using Server.AspWebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.AspWebApi.Models
{
    public class BookFactory
    {
        public static readonly string ImageDataFolder = BookMgrController.AppDataFolder + @"\BookImages";
        private readonly List<Book> _books = new List<Book>
        {
            new Book
            {
                Id = 1, Name="C# 7.0 Pocket Reference: Instant Help for C# 7.0 Programmers",
                DateCreated =DateTime.Now.AddYears(-1),
                Author ="Joseph Albahari ", Price=12.5, Quantity=3,
                ImageData = ImageUtil.ConvertImageFileToBytes(ImageDataFolder + @"\C# 7.0 Pocket Reference.jpg")
            },
            new Book
            {
                Id = 2, Name="C# 6.0 and the .NET 4.6 Framework",
                DateCreated =DateTime.Now.AddYears(-2),
                Author ="ANDREW TROELSEN", Price=12.5, Quantity=3,
                ImageData = ImageUtil.ConvertImageFileToBytes(ImageDataFolder + @"\C# 6.0 and the .NET 4.6 Framework.jpg")
            },
            new Book
            {
                Id = 3, Name="CLR via C#",
                DateCreated =DateTime.Now.AddYears(-4),
                Author ="Jeffrey Richter", Price=12.5, Quantity=3,
                ImageData = ImageUtil.ConvertImageFileToBytes(ImageDataFolder + @"\CLR via C#.jpg")
            },
            new Book
            {
                Id = 4, Name="C# 7.0 in a Nutshell: The Definitive Reference",
                DateCreated =DateTime.Now.AddYears(-1),
                Author ="Joseph Albahari", Price=12.5, Quantity=3,
                ImageData = ImageUtil.ConvertImageFileToBytes(ImageDataFolder + @"\C# 7.0 in a Nutshell.jpg")
            },
            new Book
            {
                Id = 5, Name="Essential Angular for ASP.NET Core MVC",
                DateCreated =DateTime.Now.AddYears(-2),
                Author ="Adam Freeman", Price=12.5, Quantity=3,
                ImageData = ImageUtil.ConvertImageFileToBytes(ImageDataFolder + @"\Essential Angular for ASP.NET Core MVC.jpg")
            }
        };

        public IList<Book> GetAllBooks()
        {
            return _books.ToList();
        }

        public void AddBooks(IEnumerable<Book> books)
        {
            foreach (var book in books)
            {
                AddBook(book);
            }
        }

        public void AddBook(Book book)
        {
            if (book.Id > 0) throw new Exception("Invalid new book entity");

            book.Id = _books.Count + 1;
            _books.Add(book);
        }

        public void UpdateBooks(IEnumerable<Book> books)
        {
            foreach (var book in books)
            {
                UpdateBook(book);
            }
        }

        public void UpdateBook(Book book)
        {
            var bookToUpdate = _books.FirstOrDefault(x => x.Id == book.Id);
            if (bookToUpdate != null)
            {
                bookToUpdate.Name = book.Name;
                bookToUpdate.Author = book.Author;
                bookToUpdate.PublishDate = book.PublishDate;
                bookToUpdate.Price = book.Price;
                bookToUpdate.Discount = book.Discount;
                bookToUpdate.Quantity = book.Quantity;

                bookToUpdate.DateModified = DateTime.Now;
            }
        }

        public void DeleteBooks(IEnumerable<Book> books)
        {
            foreach (var book in books)
            {
                DeleteBook(book);
            }
        }

        public void DeleteBook(Book book)
        {
            var bookToDelete = _books.FirstOrDefault(x => x.Id == book.Id);
            if (bookToDelete != null)
            {
                _books.Remove(bookToDelete);
            }
        }
    }
}