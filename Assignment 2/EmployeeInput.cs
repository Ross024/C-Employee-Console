using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_2
{
    class EmployeeInput
    {
        EmployeeDetailEntities db = new EmployeeDetailEntities();

        public void EmployeeNameInput()
        {
            Console.Clear();
            Console.WriteLine("\nEMPLOYEE DATA ENTRY\n");
            Console.Write("Enter the first name:");
            string employeeFName = Console.ReadLine();
            Console.Write("Enter the last name:");
            string employeeLName = Console.ReadLine();
            OrganizationInput(employeeFName, employeeLName);
        }
        public void OrganizationInput(string firstName, string lastName)
        {
            bool quit = false;

            while (!quit)
            {
                Console.Clear();
                Console.WriteLine("\nORGANIZATION ENTRY\n");

                IList<Organization> orgs = db.Organizations.ToList();

                for (int i = 0; i < orgs.Count(); i++)
                {
                    int questionNum = i + 1;
                    Console.WriteLine(questionNum + " )" + orgs[i].organizationName);
                }

                Console.Write("\nPlease enter an organization for {0} {1}:", firstName, lastName);
                string empOrgNum = Console.ReadLine();

                try
                {
                    int choiceNum = Convert.ToInt32(empOrgNum) - 1;
                    string empOrg = orgs[choiceNum].organizationName;
                    DepartmentInput(firstName, lastName, empOrg);
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Invalid entry");
                }
                Console.ReadLine();
            }
        }
        public void DepartmentInput(string firstName, string lastName, string empOrg)
        {
            bool quit = false;

            var depList = (from d in db.Departments
                           where d.organizationName == empOrg
                           select new { d.departmentName, d.departmentID }).ToList();

            // NEED TO RETURN DEPARTMENT ID
            while (!quit)
            {
                Console.Clear();
                Console.WriteLine("\nDEPARTMENT ENTRY");
                for (int i = 0; i < depList.Count(); i++)
                {
                    int questionNum = i + 1;
                    Console.WriteLine(questionNum + " )" + depList[i].departmentName);
                }
                Console.Write("\nPlease enter a department for {0} {1}:", firstName, lastName);
                var depSel = Console.ReadLine();
                try
                {
                    int choiceNum = Convert.ToInt32(depSel) - 1;
                    int resultDepId = depList[choiceNum].departmentID;
                    CertificateInput(firstName, lastName, empOrg, resultDepId);
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Invalid input");
                }
                Console.ReadLine();
            }
        }
        public void CertificateInput(string firstName, string lastName, string empOrg, int depID)
        {
            bool quit = false;

            IList<Certification> certList = db.Certifications.ToList();
            List<string> empCerts = new List<string>();

            while (!quit)
            {                
                int totalChoices = 1;

                Console.Clear();
                Console.WriteLine("\nCERTIFICATION ENTRY\n");

                for (int i = 0; i < certList.Count(); i++)
                {
                    int questionNum = i + 1;
                    Console.WriteLine(questionNum + ") " + certList[i].certificate);
                    totalChoices += 1;
                }
                Console.WriteLine(totalChoices + ") " + "Done");

                // Displays total certificates if you have already made valid choices
                if (empCerts.Count() > 0)
                {
                    Console.WriteLine("\nTotal certificates: " + empCerts.Count());
                }
                Console.Write("Please enter a certificate for {0} {1}:", firstName, lastName);
                string certChoice = Console.ReadLine();

                try
                {
                    int choiceNum = Convert.ToInt32(certChoice) - 1;
                    if (choiceNum == certList.Count()) //fix this so it's dynamic
                    {
                        //Put employee overview function here
                        //quit = true;
                        var distEmpCerts = empCerts.Distinct().ToList();
                        NewEmployeeConfirm(firstName, lastName, empOrg, depID, distEmpCerts);
                        Console.ReadLine();
                        break;
                    }
                    string empCert = certList[choiceNum].certificate;
                    empCerts.Add(empCert);

                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine("Invalid entry");
                }
            }
        }
        public void NewEmployeeConfirm(string firstName, string lastName, string empOrg, int depID, IList<string> empCerts)
        {
            IList<Organization> orgs = db.Organizations.ToList();
            IList<Department> deps = db.Departments.ToList();

            var orgQuery = from r in db.Regions
                           from o in db.Organizations
                           where r.city == o.city && o.organizationName == empOrg
                           select new { o.organizationName, r.state, r.city, r.timezone };

            var depQuery = from d in db.Departments
                           where d.departmentID == depID
                           select new { d.departmentName };

            Console.Clear();
            Console.WriteLine("\nEMPLOYEE CREATION CONFIRMATION");
            Console.WriteLine("\nPlease verify the employee details:\n");
            //Print employee Organization details
            foreach (var org in orgQuery)
            {
                Console.WriteLine("============================================");
                Console.WriteLine("Organization: " + org.organizationName);
                Console.WriteLine("State:        " + org.state);
                Console.WriteLine("City:         " + org.city);
                Console.WriteLine("Timezone:     " + org.timezone);
                Console.WriteLine("============================================\n");
            }
            Console.WriteLine("    Employee Name: {0} {1}\n", firstName, lastName);
            //Print employee Department
            foreach (var dep in depQuery) 
            {
                Console.WriteLine("    Department:    " + dep.departmentName);
            }
            //Dynamically print Certificates
            for (int i = 0; i < empCerts.Count(); i++)
            {
                if (i == 0)
                {
                    Console.WriteLine("\n    Certifications:-{0}", Convert.ToString(empCerts[i]));
                }
                else
                {
                    Console.WriteLine("                   -{0}", empCerts[i]);
                }
            }
            Console.WriteLine("1) Save entry");
            Console.WriteLine("2) Cancel entry");
            Console.Write("\nPlease enter your selection:");
            string saveCancel = Console.ReadLine();

            Menu menu = new Menu();
            switch(saveCancel)
            {
                case "1":
                    SaveEmployee(firstName, lastName, depID, empCerts);
                    Console.WriteLine("Entry saved.");
                    Console.Write("\nPress ENTER to continue");
                    break;
                case "2":
                    Console.WriteLine("Entry cancelled.");
                    Console.Write("\nPress ENTER to continue");
                    break;
                default:
                    Console.Write("Invalid entry");
                    NewEmployeeConfirm(firstName, lastName, empOrg, depID, empCerts);
                    break;
            }
        }
        public void SaveEmployee(string firstName, string lastName, int depID, IList<string> empCerts)
        {
            Employee emp = new Employee();
            emp.firstName    = firstName;
            emp.lastName     = lastName;
            emp.departmentID = depID;
            db.Employees.Add(emp);

            foreach(var certs in empCerts)
            {
                Certification certification = (from c in db.Certifications
                                               where c.certificate == certs
                                               select c).FirstOrDefault();
                certification.Employees.Add(emp);
            }
            db.SaveChanges();
        }
    }
}
