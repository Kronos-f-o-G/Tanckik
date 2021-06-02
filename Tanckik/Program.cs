using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanchik
{

    class Program
    {
        static public Random gen = new Random();
        static void Main(string[] args)
        {
            byte lvl = 1;
            string result = "";

            Console.WriteLine("Выберите имя для своего куртого танка!");
            Tank I = new Tank();
            I.name = Console.ReadLine();
            Console.Clear();

            while (true)//повторные игры
            {
                while (lvl < 6)
                {
                    if (Program.gen.Next(1, 11) == 1) { lvl = 10; }; //проверка на босса\

                    Enemy enemy = new Enemy(lvl);
                    while (true)
                    {
                        Console.WriteLine($"игра Танчики\nВы - {Convert.ToString(I.name)}\nЗдоровье: {I.armor[1]}\nБоезапас: {I.amunition[1]}\nПротивник - {enemy.name}\nЗдоровье: {enemy.armor[1]}\nБоезапас: {enemy.amunition[1]}");
                        result = I.Move(ref enemy);
                        if (result == "win")
                        {
                            Console.WriteLine($"{enemy.name} повержен");
                            Console.WriteLine(" \nНажмите ENTER, для начал следующего хода");
                            Console.ReadLine();
                            Console.Clear();
                            result = "";
                            break;
                        }
                        Console.WriteLine(" \nХод противника:");
                        result = enemy.Move(ref I);
                        if (result == "lose")
                        {
                            Console.WriteLine(" \nНажмите любую клавишу, для начал следующего хода");
                            Console.ReadKey();
                            Console.Clear();
                            break;
                        }
                        Console.WriteLine(" \nНажмите любую клавишу, для начал следующего хода");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    if (result == "lose") { break; }
                    lvl += 1;
                }
                switch (result)
                {
                    case "win":
                        Console.WriteLine("Вы одержали сокрушительную победу!!!");
                        break;
                    case "lose":
                        Console.WriteLine("Вы пали смертью храбрых...");
                        break;
                }
                Console.WriteLine("Спасибо за игру\nХотите сыграть еще? \n1-да \n2-нет");
                if (Console.ReadLine() != "1") { break; }

                I.New_game();
                result = "";
                lvl = 1;
            }


        }

    }

    interface ITank
    {
        
        public void New_game();
        public string Move(ref Enemy enemy);
        public string Repair();
        public string Shot();
        public string Fix();
    }

    class Tank:ITank
    {

        public int[] amunition = new int[2];     //боезапас(максимальный, реальный)
        public int[] armor = new int[2];         //броня(максимальный, реальный)
        public int damadge;                       //урон от выстрела
        public string name;                      //имя машинны

        public Tank() //конструктор
        {
            this.armor[0] = 25;
            this.amunition[0] = 5;
            this.damadge = 7;

            this.armor[1] = this.armor[0];
            this.amunition[1] = this.amunition[0];

        }
        public void New_game() //для повторной игры
        {
            this.armor[1] = this.armor[0];
            this.amunition[1] = this.amunition[0];
        }
        public string Move(ref Enemy enemy) //ход
        {
            Console.WriteLine("Ваш ход:\n1-Ремонт\n2-Перезарядка");
            if (amunition[1] != 0) { Console.WriteLine("3-Выстрел"); } else { Console.WriteLine("Боезапас пуст. Выстрел невозможен."); } 
            string result = Console.ReadLine();

            switch (result)
            {
                case "1"://Ремонт
                    Console.WriteLine(Fix());
                    break;
                case "2"://Перезарядка
                    Console.WriteLine(Repair());
                    break;
                case "3"://Выстрел
                    if (amunition[1] == 0)
                    {
                        Console.WriteLine("Не честно играешь...\nПринудительная перезарядка");
                        Repair();
                    }
                    else
                    {
                        result = Shot();
                        switch (result)
                        {
                            case "Промах...":
                                break;
                            case "Критическое попадание!!!":
                                enemy.armor[1] -= damadge * 2;
                                break;
                            default:
                                enemy.armor[1] -= damadge;
                                break;
                        }
                        Console.WriteLine(result);
                    }
                    break;
                default:
                    Console.WriteLine("Экипаж не понимает вашей команды. Вы пропускаете ход.");
                    break;
            }

            if (enemy.armor[1] <= 0) { result = "win"; }
            return result;
        }
        public string Repair()  //перезарядка
        {
            this.amunition[1] = this.amunition[0];
            return "Боезапас пополнен!";
        }
        public string Shot()   //выстрел
        {
            this.amunition[1] -= 1;
            if (Program.gen.Next(1, 11) == 1) { return "Промах..."; };
            if (Program.gen.Next(1, 6) == 1) { return "Критическое попадание!!!"; }
            else { return "Есть пробитие"; }
        }
        public string Fix()    //починка
        {
            this.armor[1] += 10;
            if (this.armor[0] <= this.armor[1])
            {
                this.armor[1] = this.armor[0];
                return "Танк как новенький!";
            }
            else
            {
                return "Броня востановленна.";
            }

        }

    }
    class Enemy : Tank
    {


        public Enemy(int lvl)   //конструктор
        {
            switch (lvl)
            {
                case 1:
                    this.name = "Маленький";
                    this.armor[0] = 8;
                    this.damadge = 4;
                    this.amunition[0] = 2;
                    break;
                case 2:
                    this.name = "Скороходный";
                    this.armor[0] = 12;
                    this.damadge = 4;
                    this.amunition[0] = 3;
                    break;
                case 3:
                    this.name = "Боевой";
                    this.armor[0] = 8;
                    this.damadge = 4;
                    this.amunition[0] = 4;
                    break;
                case 4:
                    this.name = "Угрожающий";
                    this.armor[0] = 8;
                    this.damadge = 4;
                    this.amunition[0] = 4;
                    break;
                case 5:
                    this.name = "Разящий";
                    this.armor[0] = 40;
                    this.damadge = 4;
                    this.amunition[0] = 6;
                    break;

                default:                       //секретный босс
                    this.name = "Большой босс";
                    this.armor[0] = 80;
                    this.damadge = 6;
                    this.amunition[0] = 10;
                    break;

            }
            this.armor[1] = this.armor[0];
            this.amunition[1] = this.amunition[0];
        }
        public string Move(ref Tank enemy) //ход
        {
            string result = "";
            if (amunition[1] == 0)
            {
                Repair();
                Console.WriteLine("Противник перезарядил орудия.");
            }
            else
            {
                if (Program.gen.Next(1, 5) == 1)
                {
                    Fix();
                    Console.WriteLine("Противник востановил броню.");
                }
                else
                {
                    result = Shot();

                    switch (result)
                    {
                        
                        case "Промах...":
                            Console.WriteLine("Противник промахнулся.");
                            break;
                        case "Критическое попадание!!!":
                            enemy.armor[1] -= damadge * 2;
                            Console.WriteLine("Противник крайне меткий!!! Критическое попадание.");
                            break;
                        default:
                            Console.WriteLine("Выстрел противника задел вас.");
                            enemy.armor[1] -= damadge;
                            break;
                    }
                }
            }
            if (enemy.armor[1] <= 0) { result = "lose"; }
            return result;
        }
    }

}

