using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Model.Entidades;
using Data.Bitso;

namespace Domain
{
    public class SessionStateBitso
    {
        #region singleton

        private static volatile SessionStateBitso _instance;

        private static object _syncRoot = new object();

        public static SessionStateBitso Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SessionStateBitso();
                        }
                    }
                }
                return _instance;
            }
        }


        #endregion

        private Subject<string> subjectMessage = null;

        public IObservable<string> myObservableMessage
        {
            get
            {
                return subjectMessage.AsObservable();
            }
        }

        private ConcurrentDictionary<string, int> CountTrades { get; set; }
        private ConcurrentDictionary<string, AvailableBook> DiccionarioBooks { get; set; }
        private ConcurrentDictionary<string, Ticker> DiccionaryTicker { get; set; }
        private ConcurrentDictionary<string, Orders> DiccionaryOrderBook { get; set; }
        private ConcurrentDictionary<string, Dictionary<int, Trades>> DiccionaryTrades { get; set; }

        private Task TaskGetBooks { get; set; }
        private Task TaskGetTicker { get; set; }
        private Task TaskGetOrder { get; set; }
        private Task TaskGetTrade { get; set; }
        private Task TaskTemporal { get; set; }

        public void Init()
        {
            DiccionarioBooks = new ConcurrentDictionary<string, AvailableBook>();
            DiccionaryTicker = new ConcurrentDictionary<string, Ticker>();
            DiccionaryOrderBook = new ConcurrentDictionary<string, Orders>();
            DiccionaryTrades = new ConcurrentDictionary<string, Dictionary<int, Trades>>();

            CountTrades = new ConcurrentDictionary<string, int>();
            subjectMessage = new Subject<string>();

            GetBooks();
            GetTickers();
            GetOrders();
            GetTradeBitso();
        }

        public void GetTickers() =>
            PublicObject.GetTickersAsync().Result.ForEach(AddDiccionaryTicker);

        public List<string> GetListBooks() =>
            DiccionarioBooks.Keys.ToList();

        public void GetBooks() =>
            PublicObject.GetAvailableBooksAsync().Result.ForEach(AddDiccionarioBooks);

        public void AddDiccionarioBooks(AvailableBook item) =>
            DiccionarioBooks.AddOrUpdate(item.book, item, (a, b) => item);

        public void GetOrders() =>
            DiccionarioBooks.Values.ToList().ForEach(_ => AddGetOrders(_.book));

        public void GetTradeBitso() =>
            DiccionarioBooks.Values.ToList().ForEach(_ => AddGetTrade(_.book));



        public void AddDiccionaryTicker(Ticker item)
        {
            DiccionaryTicker.AddOrUpdate(item.book, item, (a, b) => item);
            CountTrades.AddOrUpdate(item.book, 0, (a, b) => 0);
        }

        public void AddGetOrders(string book)
        {
            var data = PublicObject.GetOrderBookAsync(book, true).Result;

            if (!DiccionaryOrderBook.ContainsKey(book) || DiccionaryOrderBook[book].sequence != data.sequence)
                DiccionaryOrderBook.AddOrUpdate(book, data, (a, b) => data);
        }

        public void AddGetTrade(string book)
        {
            var data = PublicObject.GetTradesAsync(book).Result;

            if (!DiccionaryTrades.ContainsKey(book))
                DiccionaryTrades.AddOrUpdate(book, new Dictionary<int, Trades>(), (a, b) => null);

            foreach (var trade in data)
            {
                if (DiccionaryTrades[book].ContainsKey(trade.tid))
                    continue;

                DiccionaryTrades[book].Add(trade.tid, trade);

                if (trade.tid > CountTrades[book])
                    CountTrades[book] = trade.tid;
            }
        }


        private void SendOrders(string SelectCorro, string book)
        {
            if (SelectCorro == "Bid")

                foreach (var ord in DiccionaryOrderBook[book].bids)
                {
                    subjectMessage.OnNext($" {ord.book}  {ord.amount.PadRight(7)} {ord.price.PadRight(7)}  ");
                }

            else if (SelectCorro == "Ask")

                foreach (var ord in DiccionaryOrderBook[book].asks)
                {
                    subjectMessage.OnNext($" {ord.book}  {ord.amount.PadRight(7)} {ord.price.PadRight(7)}  ");
                }

        }

        public void StartTimerOrders(string book, string SelectCorro)
        {
            SendOrders(SelectCorro, book);

            Observable
                .Interval(TimeSpan.FromSeconds(3))
                .Subscribe(
                    x =>
                    {
                        TaskTemporal = Task.Run(() =>
                        {

                            try
                            {
                                var data = PublicObject.GetOrderBookAsync(book, true).Result;

                                if (data == null)
                                    return;

                                if (data.sequence == DiccionaryOrderBook[book].sequence)
                                    return;

                                DiccionaryOrderBook[book] = data;
                                subjectMessage.OnNext(string.Empty);
                                SendOrders(SelectCorro, book);

                            }
                            catch (Exception ex)
                            {
                                ;
                            }
                        });


                        TaskTemporal.Wait();

                    });
        }


        public void SendTrades(string book)
        {
            var count = 0;
            foreach (var trade in DiccionaryTrades[book].Values)
            {
                subjectMessage.OnNext($" {trade?.book.PadRight(7)} {trade?.maker_side.PadRight(7)}  { trade?.price.PadRight(15)} {trade?.amount.PadRight(15)} {trade?.created_at.Split('T')[1].PadRight(15)}  ");

                count++;
                if (count > 5)
                    break;
            }
        }


        public void StartTimerTrade(string book)
        {
            SendTrades(book);

            Observable
                .Interval(TimeSpan.FromSeconds(3))
                .Subscribe(
                    x =>
                    {
                        TaskTemporal = Task.Run(() =>
                        {
                            try
                            {
                                var data = PublicObject.GetTradesAsync(book, CountTrades[book], "asc").Result;

                                if (data == null)
                                    return;

                                foreach (var trade in data)
                                {

                                    if (!DiccionaryTrades[book].ContainsKey(trade.tid))
                                    {
                                        DiccionaryTrades[book].Add(trade.tid, trade);

                                        if (trade.tid > CountTrades[book])
                                            CountTrades[book] = trade.tid;

                                        subjectMessage.OnNext($" {trade?.book.PadRight(7)} {trade?.maker_side.PadRight(7)}  { trade?.price.PadRight(15)} {trade?.amount.PadRight(15)} {trade?.created_at.Split('T')[1].PadRight(15)}  ");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                ;
                            }
                        });

                        TaskTemporal.Wait();

                    });
        }

    }
}