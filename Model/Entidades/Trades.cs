using System.Collections.Generic;

namespace Model.Entidades
{
    public class Trades
    {
        public string book { get; set; }
        public string created_at { get; set; }
        public string amount { get; set; }
        public string maker_side { get; set; }
        public string price { get; set; }
        public int tid { get; set; }
    }

    public class TradesObject
    {
        public bool success { get; set; }
        public List<Trades> payload { get; set; }
    }
}