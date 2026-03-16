🎓 Student Grading System








A C# console-based application that allows users to manage student records, enter marks, calculate averages, and determine pass/fail results.

This project demonstrates Object-Oriented Programming, File Handling, and Input Validation using .NET 8.

📌 Features

✨ Create New Student

Generates a unique 8-digit student ID

Stores student name

Saves student record

📊 Enter Marks

Enter 6 subject marks

Automatically calculates average

Determines Pass or Fail

✏️ Update Marks

Modify previously entered marks

System stores updated results

📄 View Student Records

Displays all saved records

Shows marks, average, and status

💾 Persistent Storage

Data stored in a text file

Records remain after closing the program
===================================
     STUDENT GRADING SYSTEM
===================================

1. Create New Student
2. Enter Marks
3. Update Marks
4. View Student Record
5. Quit

⚙️ How the System Works
1️⃣ Create Student

The system generates a random 8-digit ID and stores the student.

Example:

Student ID : 12345678
Name       : John Smith
2️⃣ Enter Marks

Users must enter 6 subject marks (0–100).

Example input:

Enter mark 1: 65
Enter mark 2: 70
Enter mark 3: 55
Enter mark 4: 80
Enter mark 5: 60
Enter mark 6: 75

The system calculates:

Average: 67.5
Status : Pass
3️⃣ Update Marks

Marks can be modified using the Update Marks option.

Updated records are marked as:

[Updated]
4️⃣ View Student Record

Displays the student's stored data:

+-----------------------------------+
| ID     : 12345678                 |
| Name   : John Smith               |
| Marks  : 65,70,55,80,60,75        |
| Avg    : 67.5                     |
| Status : Pass                     |
+-----------------------------------+
📂 Data Storage

Student records are saved in a file:

students.txt

Example record format:

12345678|John Smith|[65,70,55,80,60,75]|Avg:67.5|Pass

If marks are not entered yet:

12345678|John Smith|NO_MARKS
🛠️ Technologies Used

C#

.NET 8

Console Application

File Handling

LINQ

Object-Oriented Programming

🚀 How to Run the Project
1️⃣ Clone the Repository
git clone https://github.com/yourusername/student-grading-system.git
2️⃣ Open in Visual Studio

Open the project folder in Visual Studio 2022 or later.

3️⃣ Build the Project
Build → Build Solution
4️⃣ Run the Program

Press:

F5

or

Ctrl + F5
📊 Project Structure
StudentGradingSystem
│
├── Program.cs
├── Student.cs
├── GradingApp.cs
├── students.txt
└── README.md
📚 Concepts Demonstrated

This project demonstrates:

Object-Oriented Programming

Class Design

File Storage

Exception Handling

Input Validation

Menu-based Console Systems

👨‍💻 Author

Rojla Manandhar

BSc Computer Systems Engineering
ISMT College

🖥️ Program Menu

When the application starts, the following menu appears:
