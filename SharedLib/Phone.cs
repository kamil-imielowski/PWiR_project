using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLib
{
    public class Phone
    {
        public int ID { get; set; }
        public Brand Brand { get; set; }
        public string Model { get; set; }
        public string Processor { get; set; }
        public string Ram { get; set; }
        public string Memory { get; set; }
        public string Graphic { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public DateTime Premiere { get; set; }
    }
}
