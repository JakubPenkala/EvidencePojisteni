using Microsoft.Data.SqlClient;
using System.Data;


namespace insurance.Classes
{
    public class DeleteFromDatabase
    {
    /// <summary>
    /// Finds and deletes from database
    /// </summary>
    /// <param name="table">Table name</param>
    /// <param name="column">Column name</param>
    /// <param name="Id">Id</param>
        public void Delete(string table, string column, int Id)
        {

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=aspnet-pojisteni-08b659ba-3cc1-4249-bf77-4aa744b5e327;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

            using (var deleteInsurance = new SqlCommand("DELETE FROM " + table + " WHERE " + column + " = @Id"))
            {
                deleteInsurance.Parameters.Add("@Id", SqlDbType.Int).Value = Id;
                deleteInsurance.Connection = connection;
                connection.Open();
                deleteInsurance.ExecuteNonQuery();
                connection.Close();
            }
        }  
    }
}
