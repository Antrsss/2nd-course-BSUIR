using System.Diagnostics;
using System.Reflection;

namespace _353502_Zgirskaya_Lab6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Employee> employees = new List<Employee>
            {
                new Employee("Tom", 25, true),
                new Employee("Astrid", 20, true),
                new Employee("Toothless", 29, false),
                new Employee("Helen", 19, false),
                new Employee("Hiccup", 18, true)
            };

            var path = "C:\\Users\\zgdas\\OneDrive\\Documents\\2 курс\\ИСП\\353502_Zgirskaya_Lab6\\ClassLibrary\\obj\\Debug\\net8.0\\ClassLibrary.dll";
            Assembly assembly = Assembly.LoadFrom(path);
            Type fileServiceType = assembly.GetType("ClassLibrary.FileService`1")
                                          .MakeGenericType(typeof(Employee));

            var fileService = Activator.CreateInstance(fileServiceType);

            MethodInfo saveDataMethod = fileServiceType.GetMethod("SaveData");

            string fileName = "employees.json";
            saveDataMethod.Invoke(fileService, new object[] { employees, fileName });

            MethodInfo readFileMethod = fileServiceType.GetMethod("ReadFile");
            var result = readFileMethod.Invoke(fileService, new object[] { fileName }) as IEnumerable<Employee>;

            Console.WriteLine("Employees from file:");
            foreach (var employee in result)
            {
                Console.WriteLine($"{employee.Name}, Age: {employee.Age}, WorkDone: {employee.WorkDone}");
            }
        }
    }
}
