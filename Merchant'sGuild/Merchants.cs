using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchant_sGuild {
    enum MerchantTypes {
        altruist,       //Альтруист
        swindler,       //Кидала
        slyboots,       //Хитрец
        unpredictable,  //Непредсказуемый
        resentful,      //Злопамятный
        quirky          //Ушлый
    }

    abstract class Merchant {
        protected static readonly Random GetRand = new Random(5);

        public String Name { get; set; }
        public MerchantTypes OwnType { get; set; }
        public UInt32 Gold { get; private set; }
        protected Guild OwnGuild { get; set; }

        public Merchant(string name, Guild ownGuild) {
            Name = name;
            Gold = 0;
            OwnGuild = ownGuild;
        }

        public void TakeGold(UInt32 gold) {
            if (gold <= 5)
                Gold += gold;
        }

        public void NullifyGold() {
            Gold = 0;
        }

        public abstract bool Strategy(Merchant otherMerch);

        protected bool DoMisstake() => (GetRand.Next(1, 101) <= 5);
    }

    class Altruist : Merchant {
        public Altruist(string name, Guild ownGuild) : base(name, ownGuild) => OwnType = MerchantTypes.altruist;

        public override bool Strategy(Merchant otherMerch) => !DoMisstake();
        public bool Strategy() => !DoMisstake();
    }

    class Swindler : Merchant {
        public Swindler(string name, Guild ownGuild) : base(name, ownGuild) => OwnType = MerchantTypes.swindler;

        public override bool Strategy(Merchant otherMerch) => DoMisstake();
        public bool Strategy() => DoMisstake();
    }

    class Slyboots : Merchant {
        public Slyboots(string name, Guild ownGuild) : base(name, ownGuild) => OwnType = MerchantTypes.slyboots;

        public override bool Strategy(Merchant otherMerch) {
            List<Deal> deals = OwnGuild.Merchant_sDeals(this);

            Int16 i = 0;
            while (i < deals.Count) {
                if (deals[i].Merch1 != otherMerch && deals[i].Merch2 != otherMerch)
                    deals.RemoveAt(i);
                else
                    i++;
            }

            if (deals.Count == 0) {
                return !DoMisstake();
            }
            else {
                Boolean otherWasHonest;
                if (deals.Last().Merch1 == otherMerch) {
                    otherWasHonest = deals.Last().IsHonest1;
                }
                else
                    otherWasHonest = deals.Last().IsHonest2;

                return !DoMisstake() && otherWasHonest;
            }            
        }
    }

    class Unpredictable : Merchant {
        public Unpredictable(string name, Guild ownGuild) : base(name, ownGuild) => OwnType = MerchantTypes.unpredictable;

        public override bool Strategy(Merchant otherMerch) => Convert.ToBoolean(GetRand.Next(0,2));
    }

    class Resentful : Merchant {
        public Resentful(string name, Guild ownGuild) : base(name, ownGuild) => OwnType = MerchantTypes.resentful;

        public override bool Strategy(Merchant otherMerch) {
            List<Deal> deals = OwnGuild.Merchant_sDeals(this);
            
            Int16 i = 0;
            while (i < deals.Count) {
                if (deals[i].Merch1 != otherMerch && deals[i].Merch2 != otherMerch)
                    deals.RemoveAt(i);
                else
                    i++;
            }

            if (deals.Count == 0) {
                return !DoMisstake();
            }
            else {
                Boolean otherWasAlwaysHonest = true;
                Int16 j = 0;
                while (otherWasAlwaysHonest && j < deals.Count()) {
                    if (deals[j].Merch1 == otherMerch)
                        otherWasAlwaysHonest = deals[j].IsHonest1;
                    else
                        otherWasAlwaysHonest = deals[j].IsHonest2;
                    j++;
                }

                return !DoMisstake() && otherWasAlwaysHonest;
            }
        }
    }

    class Quirky : Merchant {
        public Quirky(string name, Guild ownGuild) : base(name, ownGuild) => OwnType = MerchantTypes.quirky;

        public override bool Strategy(Merchant otherMerch) {
            List<Deal> deals = OwnGuild.Merchant_sDeals(this);

            Int16 i = 0;
            while (i < deals.Count) {
                if (deals[i].Merch1 != otherMerch && deals[i].Merch2 != otherMerch)
                    deals.RemoveAt(i);
                else
                    i++;
            }

            switch (deals.Count) {
                case 0: return !DoMisstake();                
                case 1: return DoMisstake();
                case 2: return !DoMisstake();                
                case 3: return !DoMisstake();                

                default: {
                    Boolean otherWasAlwaysHonest = true;
                    Int16 j = 0;
                    while (otherWasAlwaysHonest && j < 4) {
                        if (deals[j].Merch1 == otherMerch)
                            otherWasAlwaysHonest = deals[j].IsHonest1;
                        else
                            otherWasAlwaysHonest = deals[j].IsHonest2;
                        j++;
                    }

                    if (otherWasAlwaysHonest) {
                        if (deals.Last().Merch1 == otherMerch)
                            return !DoMisstake() && deals.Last().IsHonest1;                        
                        else
                            return !DoMisstake() && deals.Last().IsHonest2;
                    }
                    else
                        return DoMisstake();
                }
            }
        }
    }
}
