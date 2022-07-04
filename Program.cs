using TestSpargoTec.Database.Models;
using TestSpargoTec.DataBase;

namespace TestSpargoTec
{
    public static class Program
    {
        public static string conString = "Data Source=(LocalDB)\\MSSQLLocalDB; " +
                "AttachDbFilename= D:\\Job\\C#\\TestSpargoTec\\DataBase\\LocalDB.mdf; " +
                "Initial Catalog=VocabularyDB; Integrated Security=True;" +
                "Connect Timeout=30;Encrypt=False;TrustServerCertificate=False; " +
                "ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public static List<Medicine> MedicinesList = DBInteractor.GetMedicines(); //Получение товаров из БД
        public static List<Party> PartiesList = DBInteractor.GetPartys(); //Получение партий из БД
        public static List<Stock> StocksList = DBInteractor.GetStocks(); //Получение складов из БД
        public static List<Pharmacy> PharmaciesList = DBInteractor.GetPharmacies(); //Получение аптек из БД
        public static void Main()
        {
            DBInteractor.setRangesPharmacies(); //Формирование ассортимента в аптеках

            Medicine newMedicine = DBInteractor.addMedicine("Новый товар"); //Добавление товара
            Pharmacy newPharmacy = DBInteractor.addPharmacy("Новая аптека", "Новая улица", "3813215186"); //Добавление аптеки
            Stock newStock = DBInteractor.addStock(newPharmacy.Id, "Новый склад" ); //Добавление склада
            Party newParty = DBInteractor.addParty(newMedicine.Id, newStock.Id, 15); //Добавление партии

            DBInteractor.deletePharmacy(newPharmacy.Id); //Удаление аптеки (ее складов и их партий)

            newStock = DBInteractor.addStock(newPharmacy.Id, "Новый склад"); //Добавление склада
            newParty = DBInteractor.addParty(newMedicine.Id, newStock.Id, 15); //Добавление партии
            DBInteractor.deleteStock(newStock.Id); //Удаление склада (и его партий)

            newParty = DBInteractor.addParty(newMedicine.Id, newStock.Id, 15); //Добавление партии
            DBInteractor.deleteParty(newParty.Id); //Удаление партии

            DBInteractor.deleteMedicine(newMedicine.Id); //Удаление товара


            while (true) //Просмотр ассортимента аптеки
            {
                foreach (Pharmacy i in PharmaciesList)
                {
                    Console.WriteLine("{0} {1}", i.Id, i.Name);
                }
                int PharmacyId = 0;
                int.TryParse(Console.ReadLine(), out PharmacyId);
                Pharmacy Pharmacy = PharmaciesList.FirstOrDefault(Medicine => Medicine.Id == PharmacyId);
                if (Pharmacy == null)
                {
                    Console.WriteLine("Не найдено");
                }
                else { 
                    if (Pharmacy.Range.Count==0)
                    {
                        Console.WriteLine("Пусто");
                    } else {
                        Console.WriteLine("Ассортимент {0}", Pharmacy.Name);
                        foreach (KeyValuePair<Medicine, int> Dict in Pharmacy.Range)
                        {
                            Console.WriteLine("Наименование: {0} Кол-во: {1}", Dict.Key.Name, Dict.Value);
                        }
                    }
                }
            }

        }
    }
}