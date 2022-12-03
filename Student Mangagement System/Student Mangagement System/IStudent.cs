using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Mangagement_System
{
    internal interface IStudent
    {
        String FullName();
        void Add_semester();
        void Show_courses();
    }
}
