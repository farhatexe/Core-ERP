# Core
Cognitivo Core is a package that aims to help developers quickly create business applications (desktop, mobile, or web) with as little work as possible. Core is designed to handle all major business logic, allowing you to focus on the user experience.

Why use Core:
- Agnostic Database: You can configure core to work with SQL Server, LocalDB, MySQL, Postgres, Oracle, and SQL Lite.
- Extensible Data Structure: You can inherit Core’s table structure to add missing columns or tables.
- Business Logic Included: All logic related to finance, stock, invoices are included, this means that all you need to do is focus on the user experience, and leave the rest to us.
- Online Services: Integration with Cognitivo and services such as Accounting, Currency Exchange Rates, and others.
- Multi Platform: Core is based on .Net Core, which can be fully integrated into Desktop (Windows & Mac), Mobile (Android & iOS), and Web (ASP.Net) projects in minutes.

Installation & Configuration
- To Install follow these steps:
	1. Create a new .Net Core 2 (or .Net Standard 2) project within your solution
	2. Add reference to Core through Nuget.
	2. Create a class, call it: Context and inherit Core.Models.Context
	3. Add OnModelCreation method
	4. Choose the Database of your liking.
	5. Run the Migration
	6. And your all set!
