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

            Pharmacy newPharmacy = DBInteractor.addPharmacy("Новая аптека", "Новая улица", "3813215186"); //Добавление аптеки
            Stock newStock = DBInteractor.addStock(10, "Новый склад" ); //Добавление склада
            Medicine newMedicine = DBInteractor.addMedicine("Новый товар"); //Добавление товара
            Party newParty = DBInteractor.addParty(10, 10, 15); //Добавление партии

            int lastPharmacyId = PharmaciesList.OrderBy(Pharmacy => Pharmacy.Id).Last().Id; //Последний Id аптеки для удаления
            DBInteractor.deletePharmacy(10); //Удаление аптеки

            int lastStockId = StocksList.OrderBy(Stock => Stock.Id).Last().Id; //Последний Id склада для удаления
            DBInteractor.deleteStock(10); //Удаление склада

            int lastMedicineId = MedicinesList.OrderBy(Medicine => Medicine.Id).Last().Id; //Последний Id товара для удаления
            DBInteractor.deleteMedicine(10); //Удаление товара

            int lastPartyId = PartiesList.OrderBy(Party => Party.Id).Last().Id; //Последний Id партии для удаления
            DBInteractor.deleteParty(lastPartyId); //Удаление партии


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
                        foreach (KeyValuePair<Medicine, int> Dict in Pharmacy.Range)
                        {
                            Console.WriteLine("Ассортимент {0}", Pharmacy.Name);
                            Console.WriteLine("Наименование: {0} Кол-во: {1}", Dict.Key.Name, Dict.Value);
                        }
                    }
                }
            }

        }
    }
}