using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DrugClimateControlSystem
{
    class Program
    {
        //  بيانات الدواء
        class Drug
        {
            public string DrugID;
            public string Name;
            public DateTime ExpiryDate;
            public double RequiredTemperature;
            public double RequiredHumidity;

            //  معلومات الدواء
            public void DisplayDrugInfo()
            {
                Console.WriteLine($"ID: {DrugID}, Name: {Name}, Expiry: {ExpiryDate.ToShortDateString()}");
            }
        }

        //  لقراءة درجة الحرارة والرطوبة 
        class TemperatureSensor
        {
            public double CurrentTemperature;
            public double CurrentHumidity;

            // إدخال درجة الحرارة 
            public void ReadTemperature()
            {
                Console.Write("Enter current temperature (°C): ");
                double.TryParse(Console.ReadLine(), out CurrentTemperature);
            }

            // إدخال نسبة الرطوبة 
            public void ReadHumidity()
            {
                Console.Write("Enter current humidity (%): ");
                double.TryParse(Console.ReadLine(), out CurrentHumidity);
            }

            public void DisplayReadings()
            {
                Console.WriteLine($"Temp: {CurrentTemperature}°C, Humidity: {CurrentHumidity}%");
            }
        }

        //  لفحص الظروف المناخية وإرسال تنبيه 
        class AlertSystem
        {
            public bool IsAlertActive = false;

            // التحقق من توافق الشروط مع متطلبات الدواء
            public void CheckConditions(double temp, double humidity, Drug drug)
            {
                if (temp > drug.RequiredTemperature || humidity > drug.RequiredHumidity)
                {
                    SendAlert("Warning: Climate conditions not safe!");
                }
            }

            // إرسال التنبيه
            public void SendAlert(string message)
            {
                IsAlertActive = true;
                Console.WriteLine(message);
            }

            // إلغاء التنبيه
            public void ClearAlert()
            {
                IsAlertActive = false;
            }
        }

        //  يمثل موظف 
        class Employee
        {
            public string EmployeeID;
            public string Name;
            public string Role;

            // تسجيل أي عملية يقوم بها الموظف
            public void LogAction(string action, Logger logger)
            {
                string log = $"{DateTime.Now}: {Name} ({Role}) - {action}";
                logger.AddLog(log);
            }
        }

        // لتخزين وعرض السجلات 
        class Logger
        {
            public List<string> Logs = new List<string>();

            // إضافة سجل جديد
            public void AddLog(string log)
            {
                Logs.Add(log);
            }

            // عرض كل السجلات
            public void DisplayLogs()
            {
                Console.WriteLine("System Logs:");
                foreach (string log in Logs)
                {
                    Console.WriteLine(log);
                }
            }
        }

        // قائمة لحفظ الأدوية
        static List<Drug> drugs = new List<Drug>();

        // كائن لتسجيل العمليات
        static Logger logger = new Logger();

        // تعريف الموظف الحالي
        static Employee currentUser = new Employee { EmployeeID = "E001", Name = "Mahmoud", Role = "Admin" };

        // نقطة البداية للتطبيق
        static void Main()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Medical Drug Climate Control System ===");
                Console.WriteLine("1. Add Drug");
                Console.WriteLine("2. Update Drug");
                Console.WriteLine("3. Search Drug");
                Console.WriteLine("4. Remove Drug");
                Console.WriteLine("5. Check Expiry");
                Console.WriteLine("6. Generate Report");
                Console.WriteLine("7. View Logs");
                Console.WriteLine("8. Exit");
                Console.Write("Choose: ");
                string choice = Console.ReadLine();

                // تنفيذ الوظيفة حسب اختيار المستخدم
                switch (choice)
                {
                    case "1": AddDrug();
                        break;
                    case "2": UpdateDrug();
                        break;
                    case "3": SearchDrug();
                        break;
                    case "4": RemoveDrug();
                        break;
                    case "5": CheckExpiry();
                        break;
                    case "6": GenerateReport(); 
                        break;
                    case "7": logger.DisplayLogs();
                        break;
                    case "8":
                        return;
                    default: 
                        Console.WriteLine("Invalid choice.");
                        break;
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        // إضافة دواء جديد
        static void AddDrug()
        {
            Console.Write("Drug ID: ");
            string id = Console.ReadLine();
            Console.Write("Name: ");
            string name = Console.ReadLine();
            Console.Write("Expiry Date (yyyy/mm/dd): ");
            DateTime expiry = DateTime.Parse(Console.ReadLine());
            Console.Write("Required Temp: ");
            double temp = double.Parse(Console.ReadLine());
            Console.Write("Required Humidity: ");
            double humidity = double.Parse(Console.ReadLine());

            drugs.Add(new Drug { DrugID = id, Name = name, ExpiryDate = expiry, RequiredTemperature = temp, RequiredHumidity = humidity });
            currentUser.LogAction($"Added drug {name}", logger);
        }

        // تعديل بيانات دواء موجود
        static void UpdateDrug()
        {
            Console.Write("Enter Drug ID to update: ");
            string id = Console.ReadLine();
            Drug drug = drugs.Find(d => d.DrugID == id);
            if (drug != null)
            {
                Console.Write("New Expiry Date (yyyy-mm-dd): ");
                drug.ExpiryDate = DateTime.Parse(Console.ReadLine());
                Console.Write("New Required Temp: ");
                drug.RequiredTemperature = double.Parse(Console.ReadLine());
                Console.Write("New Required Humidity: ");
                drug.RequiredHumidity = double.Parse(Console.ReadLine());
                currentUser.LogAction($"Updated drug {drug.Name}", logger);
            }
            else Console.WriteLine("Drug not found.");
        }

        // id اليحث عن دواء ياستخام الاسم او 
        static void SearchDrug()
        {
            Console.Write("Enter Drug ID or Name: ");
            string input = Console.ReadLine();
            Drug drug = drugs.Find(d => d.DrugID == input || d.Name == input);
            if (drug != null)
            {
                drug.DisplayDrugInfo();
                currentUser.LogAction($"Searched for drug {drug.Name}", logger);
            }
            else Console.WriteLine("Drug not found.");
        }

        // حذف دواء من القائمة
        static void RemoveDrug()
        {
            Console.Write("Enter Drug ID to remove: ");
            string id = Console.ReadLine();
            Drug drug = drugs.Find(d => d.DrugID == id);
            if (drug != null)
            {
                drugs.Remove(drug);
                currentUser.LogAction($"Removed drug {drug.Name}", logger);
            }
            else Console.WriteLine("Drug not found.");
        }

        // علشان فحص الأدوية القريبة من تاريخ الانتهاء
        static void CheckExpiry()
        {
            Console.WriteLine("Expired or Near Expiry Drugs:");
            foreach (Drug drug in drugs)
            {
                if (drug.ExpiryDate <= DateTime.Now.AddDays(30))
                {
                    drug.DisplayDrugInfo();
                }
            }
            currentUser.LogAction("Checked expiry status", logger);
        }

        // علشان يعمل  تقرير كامل عن الأدوية والظروف لمناخية
        static void GenerateReport()
        {
            TemperatureSensor sensor = new TemperatureSensor();
            AlertSystem alert = new AlertSystem();

            sensor.ReadTemperature();
            sensor.ReadHumidity();
            sensor.DisplayReadings();

            Console.WriteLine("Inventory Report:");
            Console.WriteLine("Inventory Report:");
            for (int i = 0; i < drugs.Count; i++)
            {
                Console.WriteLine($"ID: {drugs[i].DrugID}, Name: {drugs[i].Name}, Expiry: {drugs[i].ExpiryDate.ToShortDateString()}");
                if (sensor.CurrentTemperature > drugs[i].RequiredTemperature || sensor.CurrentHumidity > drugs[i].RequiredHumidity)
                {
                    Console.WriteLine("Warning: Climate conditions not safe!");
                }
                Console.WriteLine("-----------------------------");
            }

            currentUser.LogAction("Generated report", logger);
        }
    }
}