using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;

namespace crmapp.Pages.Customers
{
    public class index : PageModel
    {
        public List<CustomerInfo> CustomerList { get; set; } = [];

        public void OnGet()
        {
            try{
                string connectionString = "Server=SANDEEPA\\SQLEXPRESS;Database=crmnewdb;Trusted_Connection=True;TrustServerCertificate=True;"; 
                using (SqlConnection connection = new SqlConnection(connectionString)){
                    connection.Open();

                    string sql = "SELECT * FROM customers ORDER BY id DESC";

                    using (SqlCommand command = new SqlCommand(sql, connection)) {
                        using (SqlDataReader reader = command.ExecuteReader()) {
                            while (reader.Read())
                            {
                                CustomerInfo customerInfo = new CustomerInfo();
                                customerInfo.Id = reader.GetInt32(0);
                                customerInfo.Firstname = reader.GetString(1);
                                customerInfo.Lastname = reader.GetString(2);
                                customerInfo.Email = reader.GetString(3);
                                customerInfo.Phone = reader.GetString(4);
                                customerInfo.CreatedAt = reader.GetDateTime(5).ToString("MM/dd/yyyy");

                                CustomerList.Add(customerInfo);
                            }
                        }
                    }
                }
            }
            catch(Exception ex) {
                Console.WriteLine("we have an error : " + ex.Message);
            }
        }
    }


    public class CustomerInfo {
        public int Id {get; set;}
        public string Firstname {get; set;} = "";
        public string Lastname {get; set;} = "";
        public string Email {get; set;} = "";
        public string Phone {get; set;} = "";
        public string CreatedAt {get; set;} = "";
    }
}