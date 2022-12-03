﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Mangagement_System
{
    internal class Department
    {

        public string Name { get; set; }
        public string Code { get; set; }
        public List<Course> Courses { get; set; }

        public Department()
        {
            Courses = new List<Course>();
        }

        public string showDetails()
        {
            string details = "Dept name: " + Name + "\n" + "Dept Code: " + Code + "\n";
            //string details = "";
            foreach (Course course in Courses)
            {
                details += course.showDetails() + "\n";
            }
            return details;
        }
    }
}
