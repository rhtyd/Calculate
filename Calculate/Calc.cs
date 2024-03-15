using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculate
{
     class Calc
    {
      
       public int id { get; set; }
        public string name { get; set; }
        public string form { get; set; }
       
        public Calc() { }


        public Calc(  string name, string form)
        {
           
            this.name = name;
           this.form = form;
           
        }

    }
}
