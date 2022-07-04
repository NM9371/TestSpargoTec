using System.Data.SqlClient;

namespace TestSpargoTec.Database.Models
{
    public class Party
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public int StockId { get; set; }
        public int Quantity { get; set; }
        public void deleteFromDB()
        {
            using (SqlConnection con = new SqlConnection(Program.conString))
            {
                SqlCommand cmd = new SqlCommand("delete from Parties where id=" + this.Id.ToString(), con);
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
                string a = $"insert into Parties (Id, MedicineId, StockId, Quantity) " +
                    $"values ({this.Id.ToString()},{this.MedicineId.ToString()},{this.StockId.ToString()},{this.Quantity.ToString()})";
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
