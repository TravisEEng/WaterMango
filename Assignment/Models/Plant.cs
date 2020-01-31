using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment.Models
{
    public class Plant
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Last_Watered { get; set; }
        public int Watered_Status { get; set; }
    }


}