using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace crmapp.Pages.Customers
{
    public class Delete : PageModel
    {
        public void OnGet()
        {
            // You can implement any necessary code here for initial load if needed
        }

        public IActionResult OnPost(int id)
        {
            deleteCustomer(id);
            return RedirectToPage("/Customers/Index");
        }

        private void deleteCustomer(int id)
        {
            try
            {
                string connectionString = "Server=SANDEEPA\\SQLEXPRESS;Database=crmnewdb;Trusted_Connection=True;TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "DELETE FROM customers WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id); // Corrected from 'Id' to 'id'
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot delete customer: " + ex.Message);
            }
        }
    }
}
