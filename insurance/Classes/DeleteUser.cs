using Microsoft.Data.SqlClient;
using System.Data;

namespace insurance.Classes
{
    public class DeleteUser
    {
        /// <summary>
        /// Deletes user from AspNetUsers
        /// </summary>
        /// <param name="Email">Email</param>
        public void Delete(string Email)
        {

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=aspnet-pojisteni-08b659ba-3cc1-4249-bf77-4aa744b5e327;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            using (var deleteAccount = new SqlCommand("DELETE FROM AspNetUsers WHERE Email = @Email"))
            {
                deleteAccount.Parameters.Add("@Email", SqlDbType.NVarChar).Value = Email;
                deleteAccount.Connection = connection;
                connection.Open();
                deleteAccount.ExecuteNonQuery();
                connection.Close();
            }


        }
    }
}
