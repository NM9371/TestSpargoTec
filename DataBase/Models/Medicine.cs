using System.Data.SqlClient;

namespace TestSpargoTec.Database.Models
{
    public class Medicine
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public void deleteFromDB()
        {
            using (SqlConnection con = new SqlConnection(Program.conString))
            {
                SqlCommand cmd = new SqlCommand("delete from Medicines where id=" + this.Id.ToString(), con);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("Успешно");
                }
            }

        }
        public void insertIntoDB()
        {
            using (SqlConnection con = new SqlConnection(Program.conString))
            {
                string a = $"insert into Medicines (Id, Name) values ({this.Id.ToString()},N'{this.Name}')";
                SqlCommand cmd = new SqlCommand(a, con);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("Успешно");
                }
            }

        }
    }
}
