using System.Collections.Generic;

namespace Model.Entidades
{
    public class Ticker
    {
        public string high { get; set; }
        public string last { get; set; }
        public string created_at { get; set; }
        public string book { get; set; }
        public string volume { get; set; }
        public string vwap { get; set; }
        public string low { get; set; }
        public string ask { get; set; }
        public string bid { get; set; }
    }

   public class TickerObject
    {
        public bool success { get; set; }
        public List<Ticker> payload { get; set; }
    }
}