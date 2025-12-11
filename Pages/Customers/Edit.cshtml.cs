using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;

namespace crmapp.Pages.Customers
{
    public class Edit : PageModel
    {
        [BindProperty]
        public int Id { get; set; }

        [BindProperty, Required(ErrorMessage = "The First Name is required")]
        public string Firstname { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "The Last Name is required")]
        public string Lastname { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "The Email is required"), EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = "";

        [BindProperty, Phone]
        public string? Phone { get; set; } // Can be null or optional, hence the ?

        public string ErrorMessage { get; set; } = "";

        public void OnGet(int id)
        {
            try{
                 string connectionString = "Server=SANDEEPA\\SQLEXPRESS;Database=crmnewdb;Trusted_Connection=True;TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM customers WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                    
                        if (reader.Read())
                        {
                            Id = reader.GetInt32(0);
                            Firstname = reader.GetString(1);
                            Lastname = reader.GetString(2);
                            Email = reader.GetString(3);
                            Phone = reader.GetString(4);
                        }
                        else
                        {
                            Response.Redirect("/Customers/Index");
                        }
                    }
                }

            }
            catch(Exception ex){
                ErrorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
            if (!ModelState.IsValid)
            {
                return;
            }
            if (Phone == null) Phone = "";

            //update customer details
            try
            {
                string connectionString = "Server=SANDEEPA\\SQLEXPRESS;Database=crmnewdb;Trusted_Connection=True;TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "UPDATE customers SET firstname=@firstname, lastname=@lastname, email=@email, phone=@phone WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", Id);
                        command.Parameters.AddWithValue("@firstname", Firstname);
                        command.Parameters.AddWithValue("@lastname", Lastname);
                        command.Parameters.AddWithValue("@email", Email);
                        command.Parameters.AddWithValue("@phone", Phone ?? (object)DBNull.Value);

                        command.ExecuteNonQuery();
                    }
                }

               
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return;
            }
            Response.Redirect("/Customers/Index");

        }

    }
}