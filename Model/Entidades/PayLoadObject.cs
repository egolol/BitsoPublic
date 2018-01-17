using System.Collections.Generic;

namespace Model.Entidades
{
    public class PayLoadObject
    {
        public bool success { get; set; }
        public List<AvailableBook> payload { get; set; }
    }
}