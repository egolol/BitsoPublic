using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace TradeRobotBitso
{
    class Program
    {
        static void Main(string[] args)
        {
            IDisposable monitorSubscription = null;
            SessionStateBitso _session = SessionStateBitso.Instance;

            _session.Init();

            while (SelectMenuInicial())
                Console.Clear();


            Console.Clear();

            switch (Mercado)
            {
                case "Bitso":

                    while (SelectLibroBitso(_session.GetListBooks()))
                    {
                        Console.Clear();
                    }
                    Console.Clear();

                    while (SelectSubMenuBitso())
                    {
                        Console.Clear();
                    }
                    Console.Clear();

                    if (Operacion == "Trades")
                    {
                        Console.WriteLine($" {"Book".PadRight(7)} { "Side".PadRight(7)}  { "Price".PadRight(15)} {"Amount".PadRight(15)} {"Created_at".PadRight(15)}  ");
                        Console.WriteLine(" ");
                        monitorSubscription = _session.myObservableMessage.Subscribe(_ =>
                        {
                            Console.WriteLine(_);
                        }
                        );
                        _session.StartTimerTrade(libro);
                    }

                    else if (Operacion == "Orders")
                    {
                        Console.SetWindowSize(40, 40);

                        while (SelectCorroBitso())
                        {
                            Console.Clear();
                        }

                        Console.Clear();
                        Console.WriteLine($"              {Corro} ");
                        Console.WriteLine(" ");
                        Console.WriteLine($" {"Book".PadRight(10)} {"Amount".PadRight(8)}  { "Price".PadRight(15)}  ");
                        Console.WriteLine(" ");
                        monitorSubscription = _session.myObservableMessage.Subscribe(_ =>
                        {
                            if (string.IsNullOrEmpty(_))
                            {
                                Console.Clear();
                                Console.WriteLine($"              {Corro} ");
                                Console.WriteLine(" ");
                                Console.WriteLine($" {"Book".PadRight(10)} {"Amount".PadRight(8)}  { "Price".PadRight(15)}  ");
                                Console.WriteLine(" ");

                                return;
                            }

                            Console.WriteLine(_);
                        }
                        );
                        _session.StartTimerOrders(libro, Corro);
                    }

                    break;
            }
            Console.Read();
        }

        private static string Mercado;

        private static bool SelectMenuInicial()
        {
            try
            {
                Mercado = string.Empty;

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("   Ingresa un Mercado: ");
                Console.WriteLine();
                Console.WriteLine("    1  -> Bitso ");               

                var x = Convert.ToChar(Console.ReadLine());

                if (x == '1')
                {
                    Mercado = "Bitso";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {               
                return true;
            }
        }

        private static string Operacion;

        private static bool SelectSubMenuBitso()
        {
            try
            {
                Operacion = string.Empty;

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("   Ingresa una opción:");
                Console.WriteLine();
                Console.WriteLine("    1  -> Orders ");
                Console.WriteLine();
                Console.WriteLine("    2  -> Trades ");

                var x = Convert.ToChar(Console.ReadLine());

                int value = 0;

                if (!int.TryParse(x.ToString(), out value))
                    return true;

                if (value == 1)
                {
                    Operacion = "Orders";
                    return false;
                }
                else if (value == 2)
                {
                    Operacion = "Trades";
                    return false;
                }            

                return true;
            }
            catch (Exception ex)
            {
                return true;
            }

        }

        private static string libro;

        private static bool SelectLibroBitso( List<string> books )
        {
            try
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("   Selecciona un libro:");
                Console.WriteLine();

                for (int i = 1; i <= books.Count(); i++)
                {
                    Console.WriteLine($"    {i}  -> {books[i - 1]} ");
                    Console.WriteLine();
                }

                var x = Convert.ToChar(Console.ReadLine());

                int value = 0;

                if (!int.TryParse(x.ToString(), out value))
                    return true;
                
                if (value < 1 || value > books.Count())
                    return true;


                libro = books[value -1];
                return false;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static string Corro;

        private static bool SelectCorroBitso()
        {
            try
            {

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("   Ingresa un Corro:");
                Console.WriteLine();


                Console.WriteLine($"    1  ->  Ask ");
                Console.WriteLine();

                Console.WriteLine($"    2  ->  Bid ");
                Console.WriteLine();



                var x = Convert.ToChar(Console.ReadLine());

                int value = 0;

                if (!int.TryParse(x.ToString(), out value))
                    return true;

                if (value == 1)
                {
                    Corro = "Ask";

                    return false;
                }


                else if (value == 2)
                {
                    Corro = "Bid";

                    return false;
                }

                return true;


            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
