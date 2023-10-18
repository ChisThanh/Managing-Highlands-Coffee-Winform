using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DataPlayer
{
    public class TableSalary
    {
        int nam = 2023;
        int thang = 2;
        private string conStr = "Data Source=highlanddb.database.windows.net;Initial Catalog=highland;User ID=vudance;Password=Vudang0402;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public void ThemKyCong(int thang, int nam)
        {
            var listEmployee = GetEmployees();
            if (listEmployee.Count == 0)
            {
                return;
            }
            foreach (var item in listEmployee)
            {
                List<string> listDay = new List<string>();
                for (int j = 1; j <= GetDayNumber(thang, nam); j++)
                {
                    DateTime date = new DateTime(nam, thang, j);
                    switch (date.DayOfWeek.ToString())
                    {
                        case "Sunday":
                            listDay.Add("CN");
                            break;
                        case "Saturday":
                            listDay.Add("T7");
                            break;
                        default:
                            listDay.Add("X");
                            break;
                    }
                    switch (listDay.Count)
                    {
                        case 28:
                            listDay.Add("");
                            listDay.Add("");
                            listDay.Add("");
                            break;
                        case 29:
                            listDay.Add("");
                            listDay.Add("");
                            break;
                        case 30:
                            listDay.Add("");
                            break;
                    }
                }

            }
        }
        private int GetDayNumber(int thang, int nam)
        {
            int dayNumber = 0;
            switch (thang)
            {
                case 2:
                    dayNumber = (nam % 4 == 0 && nam % 100 != 0) || nam % 400 == 0 ? 29 : 28;
                    break;
                case 4:
                case 6:
                case 9:
                case 11:
                    dayNumber = 30;
                    break;
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    dayNumber = 31;
                    break;
            }
            return dayNumber;
        }
        public List<Employee> GetEmployees()
        {
            List<Employee> listEmployee = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(conStr))
            {
                connection.Open();

                string sqlQuery = "SELECT * FROM Employee";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Employee employee = new Employee
                            {
                                Name = (string)reader["Name"],

                            };
                            listEmployee.Add(employee);
                        }
                    }
                }
            }

            return listEmployee;
        }
    }
}

