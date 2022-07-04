using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_2
{
    class Display
    {
        public void AllOrganizations()
        {
            Console.Clear();
            Console.WriteLine("\nALL ORGANIZATIONS\n");

            EmployeeDetailEntities db = new EmployeeDetailEntities();

            var organizations = db.Organizations;

            foreach(var org in organizations)
            {
                Console.WriteLine(org.organizationName + ": " + org.city + ", " + org.state);
            }
            Console.Write("\nPress ENTER to continue");
            Console.ReadLine();
        }
        public void AllEmployees()
        {
            Console.Clear();
            Console.WriteLine("\nALL EMPLOYEES\n");

            EmployeeDetailEntities db = new EmployeeDetailEntities();

            var query = (from e in db.Employees
                        from d in db.Departments
                        where d.departmentID == e.departmentID
                        select new { d.organizationName }).Distinct();

            var query2 = from d in db.Departments
                        from e in db.Employees
                        where d.departmentID == e.departmentID
                        orderby e.lastName
                        select new { d.departmentID, d.departmentName, d.organizationName, e.firstName, e.lastName };

            foreach ( var org in query)
            {
                Console.WriteLine(org.organizationName);
                foreach (var item in query2)
                {
                    if (item.organizationName == org.organizationName)
                    {
                        Console.WriteLine("    " + item.firstName + " " + item.lastName);
                    }
                }
            }
            Console.Write("\nPress ENTER to continue");
            Console.ReadLine();
        }
        public void OrganizationDetails(int choiceNum)
        {
            EmployeeDetailEntities db = new EmployeeDetailEntities();
            IList<Organization> orgs = db.Organizations.ToList();

            var selectOrg = orgs[choiceNum].organizationName;

            var orgQuery = from r in db.Regions
                           from o in db.Organizations
                           where r.city == o.city && o.organizationName == selectOrg
                           select new { o.organizationName, r.state, r.city, r.timezone };

            var certQuery = from e in db.Employees
                            from c in e.Certifications.DefaultIfEmpty()
                            select new { e.firstName, e.lastName, e.departmentID,
                                certificate = ((c.certificate != null)? c.certificate: "")};

            var empQuery = from d in db.Departments
                           from gc in certQuery
                           where d.departmentID == gc.departmentID && d.organizationName == selectOrg
                           select new { gc.firstName, gc.lastName, d.departmentName, gc.certificate };

            Console.Clear();
            foreach(var org in orgQuery)
            {
                Console.WriteLine("\n============================================");
                Console.WriteLine("Organization: " + org.organizationName);
                Console.WriteLine("State:        " + org.state);
                Console.WriteLine("City:         " + org.city);
                Console.WriteLine("Timezone:     " + org.timezone);
                Console.WriteLine("============================================\n");
            }
            Console.WriteLine("    EMPLOYEES\n");

            string prevLastName = "";
            foreach( var emp in empQuery)
            {
                if (emp.lastName != prevLastName)
                {
                    Console.WriteLine("\n    Employee Name: " + emp.lastName + ", " + emp.firstName);
                    Console.WriteLine("    Department:    " + emp.departmentName);
                    if (emp.certificate.Length != 0)
                    {
                        Console.Write("    Certification: ");
                        Console.WriteLine("-" + emp.certificate);                       
                    }
                }
                else
                {
                    Console.WriteLine("                   -" + emp.certificate);
                }
                prevLastName = emp.lastName;                              
            }
            Console.Write("\nPress ENTER to continue");
            Console.ReadLine();
        }
    }
}
