﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contract.BookMgr
{
    public class DeleteBookRequest
    {
        public IList<int> BookIds { get; set; }
    }
}
