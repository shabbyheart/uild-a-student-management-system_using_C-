using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Mangagement_System
{
    //public delegate String MyDelegate();
    internal class Student : IStudent
    {
        public String FirstName { get; set; }
        public String MiddleName { get; set; }
        public String LastName { get; set; }
        public String StudentId { get; set; }
        public String JoiningBatch { get; set; }
        public Department Department { get; set; }
        public int Degree { get; set; }

        public List<Semester> Semesters { get; set; }   
        public Student(){
            Semesters = new List<Semester>();
        }
        public Student(string firstName, string middleName, string lastName, string studentId, string joiningBatch, Department department, int degree)
        {
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            StudentId = studentId;
            JoiningBatch = joiningBatch;
            Department = department;
            Degree = degree;
            Semesters = new List<Semester>();
        }
        //MyDelegate fullName = new MyDelegate(FullName);
        public String FullName()
        {
            return $"{FirstName} {MiddleName} {LastName}";
        }
        public void Add_semester()
        {
            Console.WriteLine("Press 1 for add semester or Press 0 for exit:");
            string press = Console.ReadLine();
            if (press == "0") return;
            string code, year;
            string courseId;
            Console.WriteLine("Enter Semester Code {Summer, Fall, Spring}");
            code = Console.ReadLine();
            Console.WriteLine("Enter Year");
            year = Console.ReadLine();

            Show_courses();
            Console.WriteLine("For add course in this semester");
            
            List<Course> courseList = new List<Course>();
            while (true)
            {
                Console.WriteLine("Enter Course id for adding course ");
                courseId = Console.ReadLine().Trim();
                if (courseId == "0") { break; }
                int check = 0;
                foreach (var course in Department.Courses)
                {
                    if (course.Id == courseId)
                    {
                        check = 1;
                        courseList.Add(course);
                    }
                }
                if (check == 1) Console.WriteLine("Course Added successfully!!!! Press 0 for exit Or");
                else Console.WriteLine("This course is not found! Please Enter Correct CourseId");
            }
            Semester semester1 = new Semester(code, year, courseList);
            Semesters.Add(semester1);
        }
        public  void Show_courses()
        {
            try
            {
                List<string> list = new List<string>();
                foreach (var semester in Semesters)
                {
                    foreach (var course in semester.courses)
                    {
                        list.Add(course.Id);
                    }
                }
                Console.WriteLine("courses that student did not taken yet");
                Department dpt = Department;
                Console.WriteLine("\t   Course_Id \tCourse_Name");
                Console.WriteLine("\t--------------------------------");
                int i = 1;
                foreach (var course in dpt.Courses)
                {
                    if (!list.Contains(course.Id))
                    {
                        Console.WriteLine($"\t{i++}. {course.Id}\t{course.Name}");
                    }
                }
            }
            catch
            {
                Console.WriteLine("There is no course in this department");
            }
            
        }
    }
}
