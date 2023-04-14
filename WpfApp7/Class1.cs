using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp7
{
    public class Note
    {
        public int id;
        public string name { get; set; }
        public string type { get; set; }
        private double setter_price;
        public double price
        {
            get
            {
                return setter_price;
            }
            set
            {
                setter_price = value;
                incoming = value > 0 ? true : false;
                if (!incoming) setter_price *= -1;
            }
        }
        public bool incoming { get; set; }
        public DateTime date;
        public DateTime GetDate() { return date; }

        public Note(int id, string name, string type, double price, DateTime date)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.price = price;
            this.date = date;
        }
    }
}
