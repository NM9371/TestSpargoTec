using System.Data.SqlClient;

namespace TestSpargoTec.Database.Models
{
    public class Pharmacy
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public Dictionary<Medicine, int> Range { get; set; } = new Dictionary<Medicine, int>();
        public void addRange(Medicine Medicine, int Quantity)
        {
            if (Range.Any((s) => s.Key == Medicine))
            {
                Range[Medicine] += Quantity;
            } else
            {
                Range.Add(Medicine, Quantity);
            }
        }
        public void deleteFromDB()
        {
            using (SqlConnection con = new SqlConnection(Program.conString))
            {
                SqlCommand cmd = new SqlCommand("delete from Pharmacies where id="+this.Id.ToString(), con);
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
                string a = $"insert into Pharmacies (Id, Name, Address, Phone) " +
                    $"values ({this.Id.ToString()},N'{this.Name}',N'{this.Address}',N'{this.Phone}')";
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
