namespace eutazas
{
    public class Utazas
    {
        public int Megallo { get; set; }
        public int FelDatum { get; set; }
        public int FelIdo { get; set; }
        public int Azonosito { get; set; }
        public string Tipus { get; set; }
        public int Ervenyesseg { get; set; }
    }
    class Program
    {
        static List<Utazas> utazas = new List<Utazas>();
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            //1. feladat
            Beolvas();

            //2. feladat
            Feladat2();

            //3. feladat
            Feladat3();

            //4. feladat
            Feladat4();

            //5. feladat
            Feladat5();

            //6. feladat
            Feladat6();

            Console.ReadLine();
        }
        private static void Beolvas()
        {
            StreamReader sr = new StreamReader(@"utasadat.txt");

            while (!sr.EndOfStream)
            {
                string[] sor = sr.ReadLine().Split(' ');
                Utazas utas = new Utazas();

                utas.Megallo = Convert.ToInt32(sor[0]);
                utas.FelDatum = Convert.ToInt32(sor[1].Substring(0, 8));
                utas.FelIdo = Convert.ToInt32(sor[1].Substring(9, 4));
                utas.Azonosito = Convert.ToInt32(sor[2]);
                utas.Tipus= sor[3];
                utas.Ervenyesseg = Convert.ToInt32(sor[4]);

                utazas.Add(utas);
            }

            sr.Close();
        }
        private static void Feladat2()
        {
            Console.WriteLine("2. feladat");
            Console.WriteLine("A buszra {0} utas akart felszállni.",utazas.Count);
        }
        private static void Feladat3()
        {
            Console.WriteLine("3. feladat");

            int count = 0;

            foreach (var item in utazas)
            {
                if (item.Ervenyesseg < item.FelDatum && item.Ervenyesseg > 10 || item.Ervenyesseg == 0) count++;
            }

            Console.WriteLine("A buszra {0} utas nem szállhatott fel.", count);
        }
        private static void Feladat4()
        {
            Console.WriteLine("4. feladat");

            int megallo = 0, maxFo = 0;
            var csoport = utazas.GroupBy(x => x.Megallo).OrderByDescending(g => g.Key).ToList();

            foreach (var group in csoport)
            {
                if (group.Count() >= maxFo)
                {
                    maxFo = group.Count();
                    megallo = group.Key;
                }
            }

            Console.WriteLine($"A legtöbb utas ({maxFo} fő) a {megallo}. megállóban próbált felszállni.");
        }
        private static void Feladat5()
        {
            Console.WriteLine("5. feladat");

            int ingyenUtazo = 0, kedvezUtazo = 0;

            foreach (var item in utazas)
            {
                if (item.Ervenyesseg >= item.FelDatum && item.Ervenyesseg > 10)
                {
                    if (item.Tipus.Equals("TAB") || item.Tipus.Equals("NYB")) kedvezUtazo++;
                    else if (item.Tipus.Equals("NYP") || item.Tipus.Equals("RVS") || item.Tipus.Equals("GYK")) ingyenUtazo++;
                }   
            }

            Console.WriteLine($"Ingyenesen utazók száma: {ingyenUtazo} fő");
            Console.WriteLine($"A kedvezményesen utazók száma: {kedvezUtazo} fő");
        }
        private static int napokszama(int e1, int h1, int n1, int e2, int h2, int n2)
        {
            h1 = (h1 + 9) % 12;
            e1 = e1 - h1 / 10;
            int d1 = 365 * e1 + e1 / 4 - e1 / 100 + e1 / 400 + (h1 * 306 + 5) / 10 + n1 - 1;

            h2 = (h2 + 9) % 12;
            e2 = e2 - h2 / 10;
            int d2 = 365 * e2 + e2 / 4 - e1 / 100 + e2 / 400 + (h2 * 306 + 5) / 10 + n2 - 1;

            return d2 - d1;
        }
        private static string swDatum(int a)
        {
            string b = Convert.ToString(a);
            return $"{b.Substring(0, 4)}-{b.Substring(4, 2)}-{b.Substring(6, 2)}";
        }
        private static void Feladat6()
        {
            StreamWriter sw = new StreamWriter(@"figyelmeztetes.txt");

            foreach (var item in utazas.OrderBy(x => x.Ervenyesseg))
            {
                if (item.Ervenyesseg >= item.FelDatum && item.Ervenyesseg > 10)
                {
                    if (item.Ervenyesseg - item.FelDatum <= 3)
                    {
                        sw.WriteLine($"{item.Azonosito} {swDatum(item.Ervenyesseg)}");
                    }
                }
            }

            sw.Close();
        }
    }
}
