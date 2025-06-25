using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace StudentGradingApp
{
    // The Subject and Student classes
    public class Subject
    {
        public string? SubjectName { get; set; }
        public int Score { get; set; }
        public string Grade
        {
            get
            {
                if (Score >= 70) return "A: EXCELLENT";
                else if (Score >= 60 && Score < 70) return "B: GOOD";
                else if (Score >= 50 && Score < 60) return "C: AVERAGE";
                else if (Score >= 40 && Score < 50) return "D: BELOW AVERAGE";
                else if (Score < 40) return "F: FAIL";
                return "Score not set";
            }
        }
    }

    // The Student class contains a list of subjects
    public class Student
    { 
        public string? Name { get; set; }
        public List<Subject> Subjects { get; set; } = new List<Subject>();
    }
    public class Program
    {
        static void Main(string[] args)
        {
            InterfaceMenu();
        }

        public static void InterfaceMenu()
        {
            Console.WriteLine("🎓 WHAT DO YOU WISH TO DO TODAY? 🙂");
            Console.WriteLine("To input student scores, press 1");
            Console.WriteLine("To view student summary, press 2");

            Console.Write("Enter your choice:");

            string? input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    Console.Clear();
                    StudentDetailsHandler();
                    break;
                case "2":
                    Console.Clear();
                    StudentSummary();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("❌ Invalid input. Please try again.\n");
                    InterfaceMenu(); // Re-show the menu
                    break;
            }

        }

        /*---------------This method handles the input of student details, including their subjects and scores.------------------*/
        public static void StudentDetailsHandler()
        {
            List<string> subjectNames = new List<string>();

            Console.WriteLine("Only Enter the five(5) core subject For SS1:");
            for (int j = 0; j < 5; j++)
            {
                Console.Write($"Enter Subject {j + 1}: ");
                string? SubjectName = Console.ReadLine()?.Trim().ToUpper();
                if (SubjectValidation(SubjectName, subjectNames) && SubjectName != null)
                {
                    // Create a new subject and add to the list
                    Subject subject = new Subject { SubjectName = SubjectName };

                    // Add the subject to the list of subjects
                    subjectNames.Add(subject.SubjectName);
                }
                else if (SubjectName != null && !subjectNames.Contains(SubjectName.ToUpper()))
                {
                    // Add the subject name to the list
                    subjectNames.Add(SubjectName);
                }

                else
                {
                    Console.WriteLine("Subject not added. Please try again.");
                    j--; // Decrement j to repeat the input for the same subject
                }

            }
            /*--------------------------------------End of Subject Name Input ---------------------------------------------*/


            /*---------------------------------------handles subjects and scores.-------------------------------------------*/

            List<Student> students = new List<Student>();
            int i = 1;

            do
            {

                Console.Write($"\n Enter Student {i} Name: ");
                string? studentName = Console.ReadLine()?.Trim().ToUpper();

                if (StudentNameValidation(studentName, students))
                {
                    // Create a new student and add to the list
                    Student student = new Student { Name = studentName };
                    students.Add(student);

                    // Add subjects
                    foreach (var subjectName in subjectNames)
                    {
                        Console.Write($"Enter {subjectName} score: ");
                        if (int.TryParse(Console.ReadLine(), out int score) && score >= 0 && score <= 100)
                        {
                            student.Subjects.Add(new Subject { SubjectName = subjectName, Score = score, });
                        }
                        else
                        {
                            Console.WriteLine("Invalid score. Please enter a number between 0 and 100.");
                            Console.WriteLine("You will need to re-enter the student and their subjects.");
                            students.Remove(student); // Remove the student if score is invalid
                            break;
                        }
                        i++;
                    }

                    foreach (var studentd in students)
                    {
                        Console.WriteLine("\nStudent Details:");
                        Console.WriteLine($"👤 Name: {studentd.Name}");

                        foreach (var subject in studentd.Subjects)
                        {
                            Console.WriteLine($"   📘 {subject.SubjectName}: {subject.Score}: {subject.Grade}");
                        }

                        Console.WriteLine(); // 
                    }

                    Console.WriteLine("Press 1 to add another student, or any other key to exit:");
                    string? choice = Console.ReadLine();
                    if (choice == "1")
                    {
                        Console.Clear();
                        continue;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Exiting student input............................");
                        break;
                    }

                }

                else
                {
                    Console.WriteLine("Student not added. Please try again.");
                }

            }

            while (true);


        }
        /*---------------------End of Student Details Input----------------------------------*/

        /*---------------This method display summary of the students and their grade------------------*/
        public static void StudentSummary()
        {
           
        }
        /*---------------------End of Student Summary----------------------------------*/



        /*---------------------This method validates the subject name input------------------*/
        public static bool SubjectValidation(string? userInput, List<string> objectName)
        {
            // Check if the subject name is not null or empty
            if (string.IsNullOrEmpty(userInput))
            {
                Console.WriteLine("Subject name cannot be empty. Please try again.");
                return false;
            }

            // Check if the subject name already exists in the list
            else if (objectName.Contains(userInput.ToUpper()))
            {
                Console.WriteLine("Subject name already exists. Please enter a different subject.");
                return false;
            }
            else if (!userInput.ToUpper().All(char.IsLetter))
            {
                Console.WriteLine("Subject name can only be letters. Please try again.");
                return false;
            }

            return true;
        }
        //---------------------End of Subject Name Validation----------------------------------*/




        /*---------------------This method validates the student name input------------------*/

        public static bool StudentNameValidation(string? userInput, List<Student> students)
        {
            // 1. Check if input is empty
            if (string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine("Student name cannot be empty. Please try again.");
                return false;
            }

            // 2. Check if name already exists in the list (case-insensitive)
            if (students.Any(s => s.Name.Equals(userInput, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("This student already exists. Please enter a different name.");
                return false;
            }

            // 3. Check if name contains only letters
            if (!userInput.All(char.IsLetter))
            {
                Console.WriteLine("Student name must contain only letters. Please try again.");
                return false;
            }

            return true;
        }
        //---------------------End of Student Name Validation----------------------------------*/

    }
}