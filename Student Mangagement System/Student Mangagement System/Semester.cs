using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Mangagement_System
{
    internal class Semester
    {
        public String Code { get; set; }
        public String Year { get; set; }
        public List<Course> courses { get; set; }
        public Semester()
        {
            courses = new List<Course>();
        }
        public Semester(string code, string year, List<Course> courses)
        {
            this.Code = code;
            Year = year;
            this.courses = courses;
        }
        public String showDetaila()
        {
            return Code;
        }
    }
}
