using System.Collections.Generic;

namespace Model.Entidades
{

    public class Bid
    {
        public string book { get; set; }
        public string price { get; set; }
        public string amount { get; set; }
        public string oid { get; set; }
    }
    public class Ask
    {
        public string book { get; set; }
        public string price { get; set; }
        public string amount { get; set; }
        public string oid { get; set; }
    }

    public class Orders
    {
        public string updated_at { get; set; }
        public List<Bid> bids { get; set; }
        public List<Ask> asks { get; set; }
        public string sequence { get; set; }
    }

    public class OrderObject
    {
        public bool success { get; set; }
        public Orders payload { get; set; }

    }
}