using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;


namespace MyStore.Pages.Clients
{
    public class editModel : PageModel
    {

        public ClientInfo ClientInfo = new ClientInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            string Id = Request.Query["id"];

            try
            {

                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=mystore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM clients WHERE id=@id";  
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", Id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ClientInfo.id = "" + reader.GetInt32(0); 
                                ClientInfo.name = reader.GetString(1);
                                ClientInfo.email = reader.GetString(2);
                                ClientInfo.phone = reader.GetString(3);
                                ClientInfo.address = reader.GetString(4);
                            }
                        }
                    }
                }

            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost()
        {

            ClientInfo.id = Request.Form["id"];
            ClientInfo.name = Request.Form["name"];
            ClientInfo.email = Request.Form["email"];
            ClientInfo.phone = Request.Form["phone"];
            ClientInfo.address = Request.Form["address"];

            if (ClientInfo.id.Length == 0 ||  ClientInfo.name.Length == 0 || ClientInfo.email.Length == 0 || ClientInfo.phone.Length == 0 || ClientInfo.address.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            }


            try
            {

                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=mystore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE clients" +
                                  "SET name=@name  email=@email, phone=@phone, address=@address, " +
                                    "WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("id", ClientInfo.id );
                        command.Parameters.AddWithValue("name", ClientInfo.name);
                        command.Parameters.AddWithValue("email", ClientInfo.email);
                        command.Parameters.AddWithValue("address", ClientInfo.address);
                        command.Parameters.AddWithValue("phone", ClientInfo.phone);

                        command.ExecuteNonQuery();

                    }
                }

            }

            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Clients/Index");




        }
    }
}
