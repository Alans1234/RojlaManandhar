using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace StudentGradingSystem
{
    public class Student
    {
        private string _studentNumber;
        private string _studentName;
        private int[] _marks = new int[6];
        private double _average;
        private string _result = "Not Set";

        public string StudentNumber => _studentNumber;
        public string StudentName => _studentName;
        public bool HasMarks { get; private set; } = false;

        public Student(string number, string name)
        {
            _studentNumber = number;
            _studentName = name;
        }

        public void SetMarks(int[] newMarks)
        {
            if (newMarks == null || newMarks.Length != 6)
                throw new ArgumentException("Exactly 6 marks are required.");

            _marks = (int[])newMarks.Clone();
            _average = _marks.Average();
            _result = (_average >= 40) ? "Pass" : "Fail";
            HasMarks = true;
        }

        public string GetFormattedRecord(bool isUpdate = false)
        {
            string marksJoined = string.Join(",", _marks);
            string tag = isUpdate ? " [Updated]" : "";
            return $"{_studentNumber}|{_studentName}|[{marksJoined}]|Avg:{_average:F1}|{_result}{tag}";
        }

        public void PrintDisplayRecord(bool isUpdate = false)
        {
            string marksJoined = string.Join(", ", _marks);
            string tag = isUpdate ? " [Updated]" : "";
            Console.WriteLine("\n  +-----------------------------------+");
            Console.WriteLine($"  |  Student ID : {_studentNumber,-21}|");
            Console.WriteLine($"  |  Name       : {_studentName,-21}|");
            Console.WriteLine($"  |  Marks      : {marksJoined,-21}|");
            Console.WriteLine($"  |  Average    : {_average:F1,-21}|");
            Console.WriteLine($"  |  Status     : {(_result + tag),-21}|");
            Console.WriteLine("  +-----------------------------------+");
        }
    }

    public class GradingApp
    {
        private static readonly string DataFile = Path.Combine(Path.GetTempPath(), "students.txt");
        private static readonly Random Rng = new Random();
        private string _lastCreatedId = "";

        public void Run()
        {
            string choice = "";
            while (choice != "5")
            {
                Console.Clear();
                Console.WriteLine("  ===================================");
                Console.WriteLine("      STUDENT GRADING SYSTEM        ");
                Console.WriteLine("  ===================================");

                if (!string.IsNullOrEmpty(_lastCreatedId))
                {
                    Console.WriteLine($"\n  Last Created ID : {_lastCreatedId}");
                }

                Console.WriteLine("\n  1. Create New Student");
                Console.WriteLine("  2. Enter Marks");
                Console.WriteLine("  3. Update Marks");
                Console.WriteLine("  4. View Student Record");
                Console.WriteLine("  5. Quit");
                Console.WriteLine("\n  -----------------------------------");
                Console.Write("  Selection: ");
                choice = Console.ReadLine()?.Trim() ?? "";

                switch (choice)
                {
                    case "1":
                        CreateStudent();
                        break;
                    case "2":
                        EnterMarks(false);
                        break;
                    case "3":
                        EnterMarks(true);
                        break;
                    case "4":
                        ViewRecord();
                        break;
                    case "5":
                        Console.Write("\n  Are you sure you want to quit? (y/n): ");
                        string confirm = Console.ReadLine()?.Trim().ToLower() ?? "";
                        if (confirm != "y") choice = "";
                        break;
                    default:
                        ShowError("Invalid option. Please select 1-5.");
                        Pause();
                        break;
                }
            }

            Console.Clear();
            Console.WriteLine("\n  ===================================");
            Console.WriteLine("    Goodbye! Have a great day!      ");
            Console.WriteLine("  ===================================\n");
        }

        private void CreateStudent()
        {
            Console.Clear();
            Console.WriteLine("  ===================================");
            Console.WriteLine("         CREATE NEW STUDENT         ");
            Console.WriteLine("  ===================================\n");

            string name = "";
            while (string.IsNullOrWhiteSpace(name))
            {
                Console.Write("  Enter Student Name: ");
                name = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(name))
                    ShowError("Name cannot be empty. Please try again.");
            }

            string id = GenerateUniqueId();
            File.AppendAllText(DataFile, $"{id}|{name}|NO_MARKS\n");
            _lastCreatedId = id;

            Console.WriteLine("\n  [SUCCESS] Student created successfully!");
            Console.WriteLine($"  Student ID : {id}");
            Console.WriteLine($"  Name       : {name}");
            Pause();
        }

        private void EnterMarks(bool isUpdate)
        {
            Console.Clear();
            string title = isUpdate ? "         UPDATE MARKS          " : "          ENTER MARKS          ";
            Console.WriteLine("  ===================================");
            Console.WriteLine(title);
            Console.WriteLine("  ===================================\n");

            if (!string.IsNullOrEmpty(_lastCreatedId))
                Console.WriteLine($"  Last Created ID : {_lastCreatedId}\n");

            Console.Write("  Enter 8-digit Student ID: ");
            string id = Console.ReadLine()?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(id) || id.Length != 8 || !id.All(char.IsDigit))
            {
                ShowError("Invalid ID format. Must be exactly 8 digits.");
                Pause();
                return;
            }

            string? record = FindLatestRecord(id);
            if (record == null)
            {
                ShowError("No student found with that ID.");
                Pause();
                return;
            }

            string[] parts = record.Split('|');
            if (parts.Length < 2)
            {
                ShowError("Student record is corrupted.");
                Pause();
                return;
            }

            string name = parts[1];

            if (!isUpdate && !record.Contains("NO_MARKS"))
            {
                ShowError("Marks already exist. Use option 3 to update.");
                Pause();
                return;
            }

            Console.WriteLine($"\n  Student: {name}");
            Console.WriteLine("  -----------------------------------");

            int[] marks = new int[6];
            for (int i = 0; i < 6; i++)
            {
                marks[i] = ReadMark(i + 1);
            }

            Student s = new Student(id, name);
            s.SetMarks(marks);

            File.AppendAllText(DataFile, s.GetFormattedRecord(isUpdate) + "\n");

            s.PrintDisplayRecord(isUpdate);
            Console.WriteLine($"\n  [SUCCESS] Marks {(isUpdate ? "updated" : "saved")} successfully.");
            Pause();
        }

        private void ViewRecord()
        {
            Console.Clear();
            Console.WriteLine("  ===================================");
            Console.WriteLine("        VIEW STUDENT RECORD         ");
            Console.WriteLine("  ===================================\n");

            if (!string.IsNullOrEmpty(_lastCreatedId))
                Console.WriteLine($"  Last Created ID : {_lastCreatedId}\n");

            Console.Write("  Enter Student ID: ");
            string id = Console.ReadLine()?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(id))
            {
                ShowError("ID cannot be empty.");
                Pause();
                return;
            }

            if (!File.Exists(DataFile))
            {
                ShowError("No student data file found.");
                Pause();
                return;
            }

            var lines = File.ReadAllLines(DataFile)
                            .Where(l => l.StartsWith(id + "|") && !l.Contains("NO_MARKS"))
                            .ToList();

            if (lines.Count == 0)
            {
                Console.WriteLine("\n  [INFO] No records with marks found for that ID.");
                Pause();
                return;
            }

            Console.WriteLine($"\n  Found {lines.Count} record(s):\n");
            foreach (var line in lines)
            {
                string[] p = line.Split('|');
                if (p.Length < 5) continue;

                string marksRaw = p[2].Trim('[', ']');
                string avg = p[3].Replace("Avg:", "");
                string status = p[4];

                Console.WriteLine("  +-----------------------------------+");
                Console.WriteLine($"  |  ID     : {p[0],-25}|");
                Console.WriteLine($"  |  Name   : {p[1],-25}|");
                Console.WriteLine($"  |  Marks  : {marksRaw,-25}|");
                Console.WriteLine($"  |  Avg    : {avg,-25}|");
                Console.WriteLine($"  |  Status : {status,-25}|");
                Console.WriteLine("  +-----------------------------------+\n");
            }

            Pause();
        }

        private int ReadMark(int index)
        {
            while (true)
            {
                Console.Write($"  Enter mark {index} (0-100): ");
                string input = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(input))
                {
                    ShowError("Mark cannot be empty.");
                    continue;
                }

                if (!int.TryParse(input, out int val) || val < 0 || val > 100)
                {
                    ShowError("Invalid mark. Enter a whole number between 0 and 100.");
                    continue;
                }

                return val;
            }
        }

        private string? FindLatestRecord(string id)
        {
            if (!File.Exists(DataFile))
                return null;

            return File.ReadAllLines(DataFile)
                       .LastOrDefault(l => l.StartsWith(id + "|"));
        }

        private string GenerateUniqueId()
        {
            HashSet<string> existing = new HashSet<string>();

            if (File.Exists(DataFile))
            {
                foreach (var line in File.ReadAllLines(DataFile))
                {
                    string[] p = line.Split('|');
                    if (p.Length > 0 && p[0].Length == 8)
                        existing.Add(p[0]);
                }
            }

            string id;
            do
            {
                id = Rng.Next(10000000, 99999999).ToString();
            } while (existing.Contains(id));

            return id;
        }

        private void ShowError(string message)
        {
            Console.WriteLine($"\n  [ERROR] {message}");
        }

        private void Pause()
        {
            Console.WriteLine("\n  Press any key to continue...");
            Console.ReadKey(true);
        }
    }

    class Program
    {
        static void Main()
        {
            new GradingApp().Run();
        }
    }
}