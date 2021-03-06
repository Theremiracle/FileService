﻿using Common.Contract.BookMgr;
using Common.Infrastructure.Entities;
using Microsoft.Practices.Unity;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.WpfApp.ViewModels
{
    class WebApiDataViewModel : ViewModelBase
    {
        private readonly IBookService _bookService;
        public WebApiDataViewModel(IBookService bookService, IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _bookService = bookService;
        }

        [InjectionMethod]
        public void InjectionMethod()
        {
            ReloadData();
        }

        private async void ReloadData()
        {
            var books = await _bookService.GetAllBooksAsync();

            Books = new ObservableCollection<Book>(books);
            RaisePropertyChanged(nameof(Books));
        }

        public ObservableCollection<Book> Books { get; private set; }

        private Book _selectedBook;
        public Book SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                SetProperty(ref _selectedBook, value);
            }
        }
    }
}