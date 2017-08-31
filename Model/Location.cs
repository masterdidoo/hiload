using System;
using Newtonsoft.Json;

namespace hiload.Model
{
    public class Location : VisitConnected, IEntity
    {
        public int id { get; set; }
        public string place { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public int distance { get; set; }

        public override string ToString() 
        {
            return $"{id}";
        }
    }    
}