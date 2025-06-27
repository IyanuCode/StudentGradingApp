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
        public static readonly List<string> SubjectNames = new();
        public static readonly List<Student> Students = new();
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
              Console.Clear();
            int j = 1;
           Console.WriteLine("Start entering the subjects (type DONE when finished):");

while (true)
{
    Console.Write($"Enter Subject {j}: ");   // 
    string? subjectName = Console.ReadLine()?.Trim();

 
    if (string.Equals(subjectName, "DONE", StringComparison.OrdinalIgnoreCase))
    {
        Console.Clear();
        Console.WriteLine("Subject input completed.");
        Console.WriteLine("Now entering student name and scores.");
        StudentScoreHandler();
        break;                              
    }

   
    if (SubjectValidation(subjectName, SubjectNames) && subjectName != null)
    {
        SubjectNames.Add(subjectName.ToUpper());  
        j++;                                      
        continue;                                 
    }

    
        //Console.WriteLine(" Invalid subject. Please try again.");
  
}

        
        }
        /*---------------------End of Student Details Input----------------------------------*/


            /*---------------------------------------handles subjects and scores.-------------------------------------------*/

    public static void StudentScoreHandler(){
                int i = 1;

            do
            {

                Console.Write($"\n Enter Student {i} Name: ");
                string? studentName = Console.ReadLine()?.Trim().ToUpper();

                if (StudentNameValidation(studentName, Students))
                {
                    // Create a new student and add to the list
                    Student student = new Student { Name = studentName };
                    Students.Add(student);

                    // Add subjects
                    foreach (var subjectName in SubjectNames)
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
                            Students.Remove(student); // Remove the student if score is invalid
                            break;
                        }
                        
                    }
                    i++;

                    foreach (var studentd in Students)
                    {
                        Console.WriteLine("\nStudent Details:");
                        Console.WriteLine($"👤 Name: {studentd.Name}");

                        foreach (var subject in studentd.Subjects)
                        {
                            Console.WriteLine($"        📘 {subject.SubjectName}: {subject.Score}: {subject.Grade}");
                        }

                        Console.WriteLine(); // 
                    }

                    Console.WriteLine("Press 1 to add another student");
                    Console.WriteLine("Press 0 to go back to Main Menu");
                    Console.WriteLine("Press any other key to exit");
                    string? choice = Console.ReadLine();
                    if (choice == "1")
                    {
                        Console.Clear();
                        continue;
                    }
                    else if (choice == "0")
                    {
                        InterfaceMenu();
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








        /*---------------This method display summary of the students and their grade------------------*/
        public static void StudentSummary()
        {
            if (Students.Count == 0)
            {
                Console.WriteLine("❗ No student data available.");
                return;
            }

            Console.WriteLine("📚 Student Summary:");
            foreach (var studentd in Students)
            {
                Console.WriteLine("\nStudent Details:");
                Console.WriteLine($"👤 Name: {studentd.Name}");

                foreach (var subject in studentd.Subjects)
                {
                    Console.WriteLine($"   📘 {subject.SubjectName}: {subject.Score}: {subject.Grade}");
                }

                Console.WriteLine(); // 
            }
             AverageScore(Students);
        }
        /*---------------------End of Student Summary----------------------------------*/

        /*--------------------------------------Average Score --------------------------------*/

        public static double AverageScore(List<Student> students){
            if(students.Count == 0){
                Console.WriteLine("No student available to calculate average score.");
                return 0;
            }
            double totalScore = 0;
            int totalSubjects = 0;
            
            foreach (var student in students)
            {
                foreach (var subject in student.Subjects)
                {
                    totalScore += subject.Score;
                    totalSubjects++;
                }
            }
            if (totalSubjects == 0)
            {
                Console.WriteLine("No subjects available to calculate average score.");
                return 0;
            }
            double average = totalScore / totalSubjects;
            Console.WriteLine($"The average score of all students is: {average:F2}");
            return average;
        }
        /*---------------------------------------End of Average Score --------------------------------*/

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
                Console.WriteLine("Subject is entered already. Please enter a different subject.");
                return false;
            }
            else if (!userInput.Replace(" ", "").ToUpper().All(char.IsLetter))
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
