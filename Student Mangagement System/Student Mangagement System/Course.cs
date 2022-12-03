using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Mangagement_System
{
    internal class Course
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string InstructorName { get; set; }

        public double Credit { get; set; }
        public Course() { }
        public Course(string id, string name, string instructorName, double credit)
        {
            Id = id;
            Name = name;
            InstructorName = instructorName;
            Credit = credit;
        }

        public string showDetails()
        {
            return "Course ID: " + Id + ", Course Name: " + Name + ", Credit: " + Credit;
            //return "";
        }
    }
}
