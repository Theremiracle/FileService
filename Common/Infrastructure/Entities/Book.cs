using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infrastructure.Entities
{
    public class Book : EntityBase
    {
        public Book()
        {
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
            Discount = 0;
        }

        public string Author { get; set; }
        public DateTime PublishDate { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public int Quantity { get; set; }

        public double Amount => Price * (1 - Discount) * Quantity;

        public Byte[] ImageData { get; set; }
    }
}
