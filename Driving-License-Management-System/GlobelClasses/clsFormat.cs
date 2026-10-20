using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driving_License_Management_System.GlobelClasses
{
    public  class clsFormat
    {
        public static string DateToShort(DateTime dt1)
        {
            return dt1.ToString("dd/MMM/yyyy");
        }
    }
}
