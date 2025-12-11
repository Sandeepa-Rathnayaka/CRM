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
    public class Create : PageModel
    {
        [BindProperty, Required(ErrorMessage = "The First Name is required")]
        public string Firstname { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "The Last Name is required")]
        public string Lastname { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "The Email is required"), EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = "";

        [BindProperty, Phone]
        public string? Phone { get; set; } // Can be null or optional, hence the ?

        // Add ErrorMessage property to store error messages
        public string ErrorMessage { get; set; } = "";

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Return the page if the model state is invalid
            }

            if (Phone == null) Phone = "";

            // Create new customer
            try
            {
                string connectionString = "Server=SANDEEPA\\SQLEXPRESS;Database=crmnewdb;Trusted_Connection=True;TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "INSERT INTO customers (firstname, lastname, email, phone) " +
                                 "VALUES (@firstname, @lastname, @email, @phone);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@firstname", Firstname);
                        command.Parameters.AddWithValue("@lastname", Lastname);
                        command.Parameters.AddWithValue("@email", Email);
                        command.Parameters.AddWithValue("@phone", Phone);

                        command.ExecuteNonQuery(); // Use ExecuteNonQuery for insert, update, delete
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page(); // Return the page with the error message
            }

            return RedirectToPage("/Customers/Index"); // Correct way to redirect
        }
    }
}
