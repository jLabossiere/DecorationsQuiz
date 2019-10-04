using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;

namespace DesignQuiz
{
    class Program
    {
        static void Main(string[] args)
        {
            Enemy enemy = new Enemy("Jimmy the Buff Wizard");
            Console.WriteLine("What is your Name?");
            string PlayerName = Console.ReadLine();
            Console.WriteLine("What weapon will you use?");
            Console.WriteLine("[1]: Axe");
            Console.WriteLine("[2]: Sword");
            string WeaponChoice = Console.ReadLine();
            Weapon MyWeapon;
            if(WeaponChoice == "1")
            {
                MyWeapon = new Axe();
            } else
            {
                MyWeapon = new Sword();
            }

            Console.WriteLine("With your new {0} you face your enemy,{1} , but you can tell you will be smacked around unless you get help.", MyWeapon.GetName(), enemy.Name);
            Console.WriteLine("Who will you ask to bless your weapon?");
            Console.WriteLine("[1]: Good God");
            Console.WriteLine("[2]: Evil God");
            Console.WriteLine("[3]: Tom Cruise");
            string UpgradeChoice = Console.ReadLine();
            if (UpgradeChoice == "1")
            {
                MyWeapon = new Good(MyWeapon, PlayerName);
            }
            else if (UpgradeChoice == "2")
            {
                MyWeapon = new Evil(MyWeapon, PlayerName);
            } else
            {
                MyWeapon = new TomCruise(MyWeapon, PlayerName);
            }

            Console.WriteLine("With my new {0} I face {1} and defeat him", MyWeapon.GetName(), enemy.Name);
            Console.WriteLine(MyWeapon.WinLine);
            Console.ReadLine();
        }
        public static List<string> Months = new List<string>()
        {
            "January",//0
            "February",//1
            "March",//2
            "April",//3
            "May",//4
            "June",//5
            "July",//6
            "August",//7
            "September",//8
            "October",//9
            "November",//10
            "December"//11
        };

        public static void ProcessTicketOrder(string Month)
        {
            int fee = Int32.Parse(ConfigurationSettings.AppSettings.Get("Fee"));
            if (!Months.Contains(Month))
            {
                throw new Exception("Not a Valid Month");
            }
            if (Month == Months[11])
            {
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@".\records.txt", true))
                {
                    file.WriteLine(fee + "|" + (Months.IndexOf(Month) + 1) + "|0|" + (fee * 2));
                }
            }
            else if (Month == Months[5] || Month == Months[6])
            {
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@".\records.txt", true))
                {
                    file.WriteLine(fee + "|" + (Months.IndexOf(Month) + 1) + "|25|" + (fee * .75));
                }
            }
            else
            {
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@".\records.txt", true))
                {
                    file.WriteLine(fee + "|" + (Months.IndexOf(Month) + 1) + "|0|" + fee);
                }
            }

        }

        public static List<Order> GetOrders()
        {
            List<Order> returnList = new List<Order>();
            using (StreamReader sr = new StreamReader(@".\records.txt"))
            {
                string line;
                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    var splitLine = line.Split('|');
                    returnList.Add(new Order()
                    {
                        InitialFee = Int32.Parse(splitLine[0]),
                        Month = Int32.Parse(splitLine[1]),
                        Discount = Int32.Parse(splitLine[2]),
                        FinalTotal = Double.Parse(splitLine[3])
                    });
                }
            }
            return returnList;
        }



    }

    public class Order
    {
        public int InitialFee;
        public int Month;
        public int Discount;
        public double FinalTotal;
    }

    public class Player
    {
        public string Name;
        public Player(string name)
        {
            this.Name = name;
        }
    }

    public class Enemy
    {
        public string Name;

        public Enemy(string name)
        {
            this.Name = name;
        }
    }

    public abstract class Weapon
    {
        public string Name;
        public string WinLine;
        public virtual string GetName()
        {
            return Name;
        }
    }

    public abstract class WeaponUpgrade : Weapon
    {
        public Weapon Weapon;
        public abstract override string GetName();
    }

    public class Sword : Weapon
    {
        public Sword()
        {
            this.Name = "Sword";
            this.WinLine = "You Dont";
        }
    }

    public class Axe : Weapon
    {
        public Axe()
        {
            this.Name = "Axe";
            this.WinLine = "You Dont";
        }
    }

    public class Good : WeaponUpgrade
    {
        public Good(Weapon weapon, string playerName)
        {
            this.Weapon = weapon;
            this.WinLine = "Congradulations" + playerName + " you have won by being a glorified BoyScout";
        }

        public override string GetName()
        {
            return "Holy " + Weapon.GetName();
        }
    }

    public class Evil : WeaponUpgrade
    {
        public Evil(Weapon weapon, string playerName)
        {
            this.Weapon = weapon;
            this.WinLine = "Congradulations " + playerName + " you won by being a huge jerk.";
        }

        public override string GetName()
        {
            return "Cursed " + Weapon.GetName();
        }
    }

    public class TomCruise : WeaponUpgrade
    {
        public TomCruise(Weapon weapon, string playerName)
        {
            this.Weapon = weapon;
            this.WinLine = "Congradulations " + playerName + " By the power of irrational thought and paying women to marry me you have won.";
        }

        public override string GetName()
        {
            return "Scientology " + Weapon.GetName();
        }
    }
}
