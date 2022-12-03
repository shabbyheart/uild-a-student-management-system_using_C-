using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Student_Mangagement_System
{
    internal class Program
    {
        enum Departmentt { ComputerScience = 0, BBA = 1, English = 2 }
        enum Degree { BSC = 0, BBA = 1, BA = 2, MSC = 3, MBA = 4, MA = 5 }
        public delegate dynamic FileReadWrite(List<Student> studentList, string filepath);
        public delegate List<Student> FileInitialSetup();
        public static string filepath;//= @"C:\Users\omarf\Desktop\C# Program\info.json";
        static void Main(string[] args)
        {
            filepath = Environment.CurrentDirectory;
            filepath = filepath.Substring(0, filepath.IndexOf("bin")) +"info.json";

            // variable received by params array;
            Add_department(0,1,2);
            FileInitialSetup fetchData = () =>
            {
                List<Student> students = new List<Student>();
                if (System.IO.File.Exists(filepath))
                {
                    string text = System.IO.File.ReadAllText(filepath);
                    students = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Student>>(text);
                }
                else
                {
                    Console.WriteLine("Old file is not Found!. new file is in this direcotry --> " + filepath);
                    var jsonFormattedContent = Newtonsoft.Json.JsonConvert.SerializeObject(students);
                    System.IO.File.WriteAllText(filepath, jsonFormattedContent);
                }
                return students;
            };
            List<Student> studentList = fetchData();   
            FileReadWrite file_read_write = new FileReadWrite(Convert_json_to_cs);
            file_read_write += convert_cs_to_json;
            while (true)
            {
                string press = Show_students_list(studentList);
                switch (press)
                {
                    case "0":
                        dynamic std = Add_new_student(studentList);
                        studentList.Add(std);
                        studentList = file_read_write(studentList, filepath);
                        break;
                    case "1":
                        Student s = Show_student_details(studentList);
                        s.Add_semester();
                        studentList = file_read_write(studentList, filepath);
                        break;
                    case "2":
                        Remove_student(studentList);
                        studentList = file_read_write(studentList, filepath);
                        break;
                    case "3":
                        return;
                        //break;
                    default:
                        Console.WriteLine("Please Enter correct digit");
                        break;
                }
            }
        }
        
        public static List<Student> convert_cs_to_json(List<Student> studentList, string filepath)
        {
            var jsonFormattedContent = Newtonsoft.Json.JsonConvert.SerializeObject(studentList);
            System.IO.File.WriteAllText(filepath, jsonFormattedContent);
            return studentList;
        }
        public static List<Student> Convert_json_to_cs(List<Student> studentList, string filepath)
        {
            if (System.IO.File.Exists(filepath))
            {
                string text = System.IO.File.ReadAllText(filepath);
                studentList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Student>>(text);
                return studentList;
            }
            return null;
        }
        public static void Remove_student(List<Student> students)
        {
            if (students == null)
                throw new NullReferenceException("Student List is null.");
            Console.WriteLine("Enter Student Id");
            string studentId = Console.ReadLine().Trim();
            int check = 0;
            foreach (Student student in students)
            {
                if (student.StudentId == studentId)
                {
                    check = 1;
                    students.Remove(student);
                    break;
                }
            }
            if (check == 1) Console.WriteLine("Student Remove successfully");
            else Console.WriteLine("This Student is not found. Please entere correct Student Id");
        }
        public static Student Add_new_student(List<Student> studentList)
        {
            if (studentList == null)
                throw new NullReferenceException("Student List is null.");
            string f_name, m_name, l_name, s_id;
            int dept, deg;
            Console.WriteLine("Enter First Name: ");
            f_name = Console.ReadLine();
            Console.WriteLine("Enter Middle Name: ");
            m_name = Console.ReadLine();
            Console.WriteLine("Enter Last Name: ");
            l_name = Console.ReadLine();
            Console.WriteLine("Enter Student Id (XXXX): ");
            s_id = Console.ReadLine()+ "0" + studentList.Count;
            Console.WriteLine("press {0,1,2} for corresponding {ComputerScience, BBA, English} department");
            dept = Convert.ToInt32(Console.ReadLine());
            Department deptt = Add_department(dept);
            Console.WriteLine("press {0,1,2,3,4,5} for corresponding {BSC, BBA, BA, MSC, MBA, MA} degree.");
            deg = Convert.ToInt32(Console.ReadLine());
            //Console.WriteLine(deptt.Courses.Count);
            Student student = new Student(f_name, m_name, l_name, s_id,"22", deptt, deg);
            return student;
        }
        public static Student Show_student_details(List<Student> students)
        {
            if (students == null)
                throw new NullReferenceException("Student List is null.");
            Console.WriteLine("Enter Student Id: ");
            string studentId = Console.ReadLine().Trim();
            Student std = new Student();
            int check = 0;
            foreach(Student student in students)
            {
                if(student.StudentId == studentId)
                {
                    check = 1;
                    std = student;
                    Console.WriteLine("Name: " + student.FullName());
                    Console.WriteLine("Student Id: {0} \nBatch: {1}", student.StudentId, student.JoiningBatch);
                    Console.WriteLine("Degree: " + (Degree)student.Degree);
                    //Console.WriteLine("{0}", student.Department.showDetails());
                    if(student.Semesters.Count <= 0)
                    {
                        Console.WriteLine("There is no Semester assigned yet for this student");
                        break;
                    }
                    Console.WriteLine("Semester List Shown below");
                    int i = 1;
                    foreach( var semester in student.Semesters)
                    {
                        Console.WriteLine($"\t{i++}." + " Code : " + semester.Code + "\n\t   Year : " + semester.Year);
                        Console.WriteLine("\t   Course Taken in this Semester");
                        int j = 1;
                        foreach(var course in semester.courses)
                        {
                            Console.WriteLine($"\t\t{j++}. Id: {course.Id} Name: {course.Name}");
                        }
                    }
                    break;
                }
            }
            if(check == 0)
            {
                Console.WriteLine("This student is not found!!!");
                Show_student_details(students);
            }
                
            return std;
        }
        public static string Show_students_list(List<Student> students)
        {
            
            Console.WriteLine("<------List of all students is shown below:------>");
            Console.WriteLine("_______________________________________________________________");
            Console.WriteLine("No  Student_Id    Student_Name");
            if (students.Count == 0)
                Console.WriteLine("\tThere is no Student. You can add student");
            try
            {
                for (int i = 0; i < students.Count; i++)
                {
                    Console.WriteLine($"{i + 1}.    {students[i].StudentId}      {students[i].FullName()}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: {0}", ex.Message);
            }
            Console.WriteLine("_______________________________________________________________");
            Console.WriteLine("-----------------------------*************-------------------------------------------------");
            Console.WriteLine("\tPress 0 -> Add Student, 1 -> View Student, 2 -> Delete Student, 3 -> Exit Program");
            Console.WriteLine("-----------------------------*************-------------------------------------------------");
            string value = Console.ReadLine();
            return value;
        }
        
        public static Department Add_department(params int[] departments)
        {
            //Console.WriteLine("Add department called");
            Department department = new Department();
            foreach (int dept in departments)
            {
                if (dept == 0)
                {
                    department.Name = "Computer Science & Engineering";
                    department.Code = "CSE";

                    department.Courses.Add(new Course("CSE-101", "Fundamentals of Computer System", "Faruq", 2.0));
                    department.Courses.Add(new Course()
                    {
                        Id = "CSE-102",
                        Name = "C Programming",
                        InstructorName = "Faruq",
                        Credit = 3.0
                    });
                    department.Courses.Add(new Course("CSE-103", "Data Structure", "Faruq", 2.0));
                    department.Courses.Add(new Course("CSE-104", "Introduction to Algorithms", "Faruq", 4.0));
                    department.Courses.Add(new Course("CSE-105", "nformation Systems Analysis & Design", "Faruq", 4.0));
                    department.Courses.Add(new Course("CSE-106", "Computer Networks", "Faruq", 4.0));
                    department.Courses.Add(new Course("CSE-107", "Compiler Designs", "Faruq", 4.0));
                    department.Courses.Add(new Course("CSE-108", "Electrical Devices & Instrumentations", "Faruq", 4.0));
                    department.Courses.Add(new Course("CSE-109", "Communication Engineering", "Faruq", 4.0));
                    department.Courses.Add(new Course("CSE-201", "Database Management Systems", "Faruq", 4.0));
                    department.Courses.Add(new Course("CSE-202", "Computer Architecture", "Faruq", 4.0));

                }
                if (dept == 1)
                {
                    department.Name = "Bachelor of Business Administration";
                    department.Code = "BBA";

                    department.Courses.Add(new Course("BBA-101", "ACCOUNTING ", "Mohi", 2.0));
                    department.Courses.Add(new Course("BBA-102", "HUMAN RESOURCE MANAGEMENT", "Mohi", 2.0));
                    department.Courses.Add(new Course("BBA-103", "INTERNATIONAL BUSINESS", "Mohi", 4.0));
                    department.Courses.Add(new Course("BBA-104", "OPERATIONS AND SUPPLY CHAIN MANAGEMENT", "Mohi", 4.0));
                    department.Courses.Add(new Course("BBA-105", "FINANCIAL MANAGEMENT", "Mohi", 4.0));
                    department.Courses.Add(new Course("BBA-106", "MANAGEMENT INFORMATION SYSTEMS", "Mohi", 4.0));
                    department.Courses.Add(new Course("BBA-107", "ADVANCED BUSINESS STATISTICS", "Mohi", 4.0));
                    department.Courses.Add(new Course("BBA-108", "BASICS IN SOCIAL SCIENCE", "Mohi", 4.0));
                    department.Courses.Add(new Course("BBA-109", "COMPUTING AND BUSINESS APPLICATIONS", "Mohi", 4.0));
                    department.Courses.Add(new Course("BBA-201", "ORGANIZATIONAL BEHAVIOUR", "Mohi", 4.0));
                    department.Courses.Add(new Course("BBA-203", "BUSINESS STATISTICS", "Mohi", 4.0));
                    department.Courses.Add(new Course("BBA-204", "MANAGERIAL ACCOUNTING", "Mohi", 4.0));
                    department.Courses.Add(new Course("BBA-205", "MACRO ECONOMICS", "Mohi", 4.0));
                }
                if (dept == 2)
                {
                    department.Name = "English";
                    department.Code = "English";

                    department.Courses.Add(new Course("ENG-1151", "English Reading Skills", "Omar", 2.0));
                    department.Courses.Add(new Course("ENG-1152", "English Writing Skills", "Omar", 2.0));
                    department.Courses.Add(new Course("ENG-1153", "Introduction to Poetry", "Omar", 4.0));
                    department.Courses.Add(new Course("ENG-1154", "Bangla Language and Literature", "Omar", 4.0));
                    department.Courses.Add(new Course("ENG-1155", "Elizabethan to Neo-Classical Tragedy", "Omar", 4.0));
                    department.Courses.Add(new Course("ENG-1156", "Elizabethan to Neo-Classical Comedy", "Omar", 4.0));
                    department.Courses.Add(new Course("ENG-1157", "Literary Theory and Criticism", "Omar", 4.0));
                    department.Courses.Add(new Course("ENG-1158", "Introduction to Sociolinguistics", "Omar", 4.0));
                    department.Courses.Add(new Course("ENG-1159", "16th to 18th Century Prose", "Omar", 4.0));
                    department.Courses.Add(new Course("ENG-1161", "Western Thought", "Omar", 4.0));
                    department.Courses.Add(new Course("ENG-1162", "History of Modern Europe", "Omar", 4.0));
                    department.Courses.Add(new Course("ENG-1163", "English for Business Communication", "Omar", 4.0));
                    department.Courses.Add(new Course("ENG-1164", "18th Century Literature", "Omar", 4.0));
                    department.Courses.Add(new Course("ENG-1165", "16th to 17th Century Poetry", "Omar", 4.0));
                    department.Courses.Add(new Course("ENG-1167", "Greek and Roman Classics in Translation", "Omar", 4.0));
                    department.Courses.Add(new Course("ENG-1168", "Introduction to Philosophy", "Omar", 4.0));
                    department.Courses.Add(new Course("ENG-1169", "Studies in English History", "Omar", 4.0));
                    department.Courses.Add(new Course("ENG-1170", "Introduction to Morphology and Syntax", "Omar", 4.0));
                }
            }
            return department;
        }
    }
}
