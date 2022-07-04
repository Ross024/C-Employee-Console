using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_2
{
    class Menu
    {
        public void MainMenu()
        {
            bool quit = false;

            while (!quit)
            {
                Console.Clear();
                Console.WriteLine("\nMAIN MENU\n");
                Console.WriteLine("1) All Organizations");
                Console.WriteLine("2) All Employees");
                Console.WriteLine("3) Organization Detail");
                Console.WriteLine("4) Create New Employee");
                Console.WriteLine("5) Quit\n");
                Console.Write("Please enter your selection:");
                string myInput = Console.ReadLine();

                Display display = new Display();
                EmployeeInput employeeInput = new EmployeeInput();

                switch(myInput)
                {
                    case "1":
                        display.AllOrganizations();
                        break;
                    case "2":
                        display.AllEmployees();
                        break;
                    case "3":
                        OrganizationSelection();
                        break;
                    case "4":
                        employeeInput.EmployeeNameInput();
                        break;
                    case "5":
                        quit = true;
                        break;
                }
            }
        }
        public void OrganizationSelection()
        {
            bool quit = false;

            while(!quit)
            {
                Console.Clear();
                Console.WriteLine("\nORGANIZATION SELECTION\n");

                Display display = new Display();
                EmployeeDetailEntities db = new EmployeeDetailEntities();
                IList<Organization> orgs = db.Organizations.ToList();
                for (int i = 0; i < orgs.Count(); i++)
                {
                    int questionNum = i + 1;
                    Console.WriteLine(questionNum + ") " + orgs[i].organizationName);
                }
                Console.Write("\nPlease enter the Organization number:");
                string userOrgChoice = Console.ReadLine();
                try
                {
                    int choiceNum = Convert.ToInt32(userOrgChoice) - 1;
                    display.OrganizationDetails(choiceNum);
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Invalid entry");
                }
                Console.ReadLine();
            }
        }
    }
}
