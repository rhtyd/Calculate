using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Calculate
{
    internal class UpdateData
    {
        ApplicationContext db;
        public List<string> Updates() {

            db = new ApplicationContext();
            List<Calc> calcs = db.Calcs.ToList();

            List<string> str = new List<string>();
            foreach (Calc calc in calcs)
            {
                str.Add(calc.name);
            }
            return str;

        } 

    }
}
