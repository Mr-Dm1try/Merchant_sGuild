using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchant_sGuild {
    class Deal {
        public Merchant Merch1 { get; }
        public Merchant Merch2 { get; }
        public Boolean IsHonest1 { get; }
        public Boolean IsHonest2 { get; }

        public Deal(Merchant merch1, Merchant merch2, Boolean isHonest1, Boolean isHonest2) {
            if (merch1 != merch2) {
                Merch1 = merch1;
                Merch2 = merch2;
                IsHonest1 = isHonest1;
                IsHonest2 = isHonest2;
            }
            else
                throw new Exception("Merch1 = Merch2!!!");
        }

        public String MakeDeal() {
            String str;

            if (IsHonest1 && IsHonest2) {
                Merch1.TakeGold(4);
                Merch2.TakeGold(4);

                str = "44";
            }
            else if (!IsHonest1 && IsHonest2) {
                Merch1.TakeGold(5);
                Merch2.TakeGold(1);

                str = "51";
            }
            else if (IsHonest1 && !IsHonest2) {
                Merch1.TakeGold(1);
                Merch2.TakeGold(5);

                str = "15";
            }
            else {
                Merch1.TakeGold(2);
                Merch2.TakeGold(2);

                str = "22";
            }
            return str;
        }
    }


    class Guild {
        private List<Merchant> Merchants { get; set; }
        private List<Deal> Deals { get; set; }
        public Int16 Age { get; private set; }
        public Int16 CountOfDeals { get; }
        private Boolean IsSummedUp { get; set; }

        public Guild(Int16 merchCount, Int16 countOfDealsBetween2Merch) {
            Age = 1;
            Merchants = new List<Merchant>();
            Deals = new List<Deal>();
            IsSummedUp = true;
            if (countOfDealsBetween2Merch >= 5 && countOfDealsBetween2Merch <= 10)
                CountOfDeals = countOfDealsBetween2Merch;
            else
                throw new Exception("Wrong deals count!");

            if (merchCount % 6 == 0) {
                for (int i = 0; i < merchCount / 6; i++) 
                    Merchants.Add(new Altruist("Altruist_" + Age + "_" + i, this));
                
                for (int i = 0; i < merchCount / 6; i++) 
                    Merchants.Add(new Quirky("Quirky_" + Age + "_" + i, this));
                
                for (int i = 0; i < merchCount / 6; i++) 
                    Merchants.Add(new Resentful("Resentful_" + Age + "_" + i, this));
                
                for (int i = 0; i < merchCount / 6; i++) 
                    Merchants.Add(new Slyboots("Slyboots_" + Age + "_" + i, this));
                
                for (int i = 0; i < merchCount / 6; i++) 
                    Merchants.Add(new Swindler("Swindler_" + Age + "_" + i, this));
                
                for (int i = 0; i < merchCount / 6; i++) 
                    Merchants.Add(new Unpredictable("Unpredict_" + Age + "_" + i, this));                
            }
            else
                throw new Exception("Wrong merchants count!");
        }

        public void AddMerchant(Merchant merchant) {
            Merchants.Add(merchant);
        }

        public void PrintAllMerchants() {
            foreach (var item in Merchants)
                Console.WriteLine("\t" + item.Name + "\t: " + item.Gold + " gold");
            Console.WriteLine();
        }

        public String NewDeal(Merchant merch1, Merchant merch2) {
            Deals.Add(new Deal(merch1, merch2, merch1.Strategy(merch2), merch2.Strategy(merch1)));
            String result = Deals.Last().MakeDeal();

            return merch1.Name + ": +" + result[0] + "\t" + merch2.Name + ": +" + result[1];
        }

        public List<Deal> Merchant_sDeals(Merchant merchant) {
            List<Deal> result = new List<Deal>();

            foreach (var deal in Deals)
                if (deal.Merch1 == merchant || deal.Merch2 == merchant)
                    result.Add(deal);

            return result;
        }

        public void StartNewYear() {
            if (IsSummedUp) {
                Console.WriteLine("Year #" + Age);
                for (int i = 0; i < Merchants.Count - 1; i++) 
                    for (int j = i + 1; j < Merchants.Count; j++) {
                        Console.WriteLine("Deals between " + Merchants[i].Name + " and " + Merchants[j].Name + ":");

                        for (int c = 0; c < CountOfDeals; c++)
                            Console.WriteLine("\t#" + c + " " + NewDeal(Merchants[i], Merchants[j]));
                    }
                
                IsSummedUp = false;
            }
            else 
                Console.WriteLine("Summarize the previous year, please");
            Console.WriteLine();
        }

        public void SummingUpYear() {            
            Age++;
            Merchants.Sort((a, b) => b.Gold.CompareTo(a.Gold));

            Console.WriteLine("All merchants and they gold:");
            PrintAllMerchants();

            Console.WriteLine("20% of the most unsuccessful merchants, who will be excluded:");            

            Int32 toDelete = Merchants.Count / 5;
            Int32 eightyPercent = Merchants.Count - toDelete;
            for (int i = Merchants.Count - 1; i >= eightyPercent; i--) {
                Console.WriteLine("\tPlace #" + (i + 1) + " " + Merchants[i].Name + " with " + Merchants[i].Gold + " gold");
                Merchants.RemoveAt(i);
            }
            Console.WriteLine();

            Console.WriteLine("New merchants:");
            for (int i = 0; i < toDelete; i++) {
                MerchantTypes type = Merchants[i].OwnType;
                switch (type) {
                    case MerchantTypes.altruist: {
                        Merchants.Add(new Altruist("Altruist_" + Age + "_" + i, this));
                        break;
                    }
                    case MerchantTypes.quirky: {
                        Merchants.Add(new Quirky("Quirky_" + Age + "_" + i, this));
                        break;
                    }
                    case MerchantTypes.resentful: {
                        Merchants.Add(new Resentful("Resentful_" + Age + "_" + i, this));
                        break;
                    }
                    case MerchantTypes.slyboots: {
                        Merchants.Add(new Slyboots("Slyboots_" + Age + "_" + i, this));
                        break;
                    }
                    case MerchantTypes.swindler: {
                        Merchants.Add(new Swindler("Swindler_" + Age + "_" + i, this));
                        break;
                    }
                    case MerchantTypes.unpredictable: {
                        Merchants.Add(new Unpredictable("Unpredictable_" + Age + "_" + i, this));
                        break;
                    }
                }
                Console.WriteLine("\t" + Merchants.Last().Name);
            }
            Console.WriteLine();

            foreach (var item in Merchants) {
                item.NullifyGold();
            }

            Deals = new List<Deal>();

            IsSummedUp = true;
        }
    }
}
