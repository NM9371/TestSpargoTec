using System.Data.SqlClient;
using TestSpargoTec.Database.Models;

namespace TestSpargoTec.DataBase
{
    internal static class DBInteractor
    {
        public static List<Medicine> GetMedicines()
        {
            List<Medicine> EntityList = new();
            using (SqlConnection con = new SqlConnection(Program.conString))
            {
                SqlCommand cmd = new SqlCommand("select * from Medicines", con);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EntityList.Add(new Medicine
                        {
                            Id = (int)reader["Id"],
                            Name = (string)reader["Name"],
                        });
                    }
                }
            }
            return EntityList;
        }
        public static List<Party> GetPartys()
        {
            List<Party> EntityList = new();
            using (SqlConnection con = new SqlConnection(Program.conString))
            {
                SqlCommand cmd = new SqlCommand("select * from Parties", con);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EntityList.Add(new Party
                        {
                            Id = (int)reader["Id"],
                            MedicineId = (int)reader["MedicineId"],
                            StockId = (int)reader["StockId"],
                            Quantity = (int)reader["Quantity"]
                        });
                    }
                }
            }
            return EntityList;
        }
        public static List<Stock> GetStocks()
        {
            List<Stock> EntityList = new();
            using (SqlConnection con = new SqlConnection(Program.conString))
            {
                SqlCommand cmd = new SqlCommand("select * from Stocks", con);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EntityList.Add(new Stock
                        {
                            Id = (int)reader["Id"],
                            PharmacyId = (int)reader["PharmacyId"],
                            Name = (string)reader["Name"],
                        });
                    }
                }
            }
            return EntityList;
        }
        public static List<Pharmacy> GetPharmacies()
        {
            List<Pharmacy> EntityList = new();
            using (SqlConnection con = new SqlConnection(Program.conString))
            {
                SqlCommand cmd = new SqlCommand("select * from Pharmacies", con);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EntityList.Add(new Pharmacy
                        {
                            Id = (int)reader["Id"],
                            Name = (string)reader["Name"],
                            Address = (string)reader["Address"],
                            Phone = (string)reader["Phone"]
                        });
                    }
                }
            }
            return EntityList;
        }
        public static void setRangesPharmacies()
        {
            foreach (Pharmacy Pharmacy in Program.PharmaciesList)
            {
                Pharmacy.Range.Clear();
                List<Stock> filteredStocksList = Program.StocksList.Where(Stock => Stock.PharmacyId == Pharmacy.Id).ToList();
                foreach (Stock Stock in filteredStocksList)
                {
                    List<Party> filteredPartysList = Program.PartiesList.Where(Party => Party.StockId == Stock.Id).ToList();
                    foreach (Party Party in filteredPartysList)
                    {
                        Medicine Medicine = Program.MedicinesList.First(Medicine => Medicine.Id == Party.MedicineId);
                        Pharmacy.addRange(Medicine, Party.Quantity);
                    }
                }
            }
        }
        public static Medicine addMedicine(string Name)
        {
            int newId = Program.MedicinesList.OrderBy(Medicine => Medicine.Id).Last().Id + 1;
            Medicine newMedicine = new Medicine()
            {
                Id = newId,
                Name = Name
            };
            newMedicine.insertIntoDB();
            Program.MedicinesList.Add(newMedicine);
            return newMedicine;

        }
        public static Party addParty(int MedicineId, int StockId, int Quantity)
        {
            int newId = Program.PartiesList.OrderBy(Party => Party.Id).Last().Id + 1;
            Party newParty = new Party()
            {
                Id = newId,
                MedicineId = MedicineId,
                StockId = StockId,
                Quantity = Quantity
            };
            newParty.insertIntoDB();
            Program.PartiesList.Add(newParty);
            return newParty;

        }
        public static Stock addStock(int PharmacyId, string Name)
        {
            int newId = Program.StocksList.OrderBy(Stock => Stock.Id).Last().Id + 1;
            Stock newStock = new Stock()
            {
                Id = newId,
                PharmacyId = PharmacyId,
                Name = Name
            };
            newStock.insertIntoDB();
            Program.StocksList.Add(newStock);
            return newStock;

        }
        public static Pharmacy addPharmacy(string Name, string Address, string Phone)
        {
            int newId = Program.PharmaciesList.OrderBy(Pharmacy => Pharmacy.Id).Last().Id+1;
            Pharmacy newPharmacy = new Pharmacy()
            {
                Id = newId,
                Name = Name,
                Address = Address,
                Phone = Phone
            };
            newPharmacy.insertIntoDB();
            Program.PharmaciesList.Add(newPharmacy);
            return newPharmacy;

        }
        public static void deleteMedicine( int Id)
        {
            Medicine Medicine = Program.MedicinesList.FirstOrDefault(Medicine => Medicine.Id == Id);
            if (Medicine == null)
            {
                Console.WriteLine("Не найдено");
            }
            else
            {
                List<Party> filteredPartysList = Program.PartiesList.Where(Party => Party.MedicineId == Medicine.Id).ToList();
                foreach (Party Party in filteredPartysList)
                {
                    deleteParty(Party.Id);
                }
                Medicine.deleteFromDB();
                Program.MedicinesList.Remove(Medicine);
            }
        }
        public static void deleteParty(int Id)
        {
            Party Party = Program.PartiesList.FirstOrDefault(Party => Party.Id == Id);
            if (Party == null)
            {
                Console.WriteLine("Не найдено");
            }
            else
            {
                Party.deleteFromDB();
                Program.PartiesList.Remove(Party);
                setRangesPharmacies(); //обновление ассортимента в аптеках
            }
        }
        public static void deleteStock(int Id)
        {
            Stock Stock = Program.StocksList.FirstOrDefault(Stock => Stock.Id == Id);
            if (Stock == null)
            {
                Console.WriteLine("Не найдено");
            }
            else
            {
                List<Party> filteredPartysList = Program.PartiesList.Where(Party => Party.StockId == Stock.Id).ToList();
                foreach (Party Party in filteredPartysList)
                {
                    deleteParty(Party.Id);
                }

                Stock.deleteFromDB();
                Program.StocksList.Remove(Stock);
            }
        }
        public static void deletePharmacy(int Id)
        {
            Pharmacy Pharmacy = Program.PharmaciesList.FirstOrDefault(Pharmacy => Pharmacy.Id == Id);
            if (Pharmacy == null)
            {
                Console.WriteLine("Не найдено");
            }
            else
            {
                List<Stock> filteredStocksList = Program.StocksList.Where(Stock => Stock.PharmacyId == Pharmacy.Id).ToList();
                foreach (Stock Stock in filteredStocksList)
                {
                    deleteStock(Stock.Id);
                }

                Pharmacy.deleteFromDB();
                Program.PharmaciesList.Remove(Pharmacy);
            }
        }
    }
}
