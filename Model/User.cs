using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace hiload.Model
{
    public class User : VisitConnected, IEntity
    {
        public static int Now { get; set; }
        public static DateTime NowDate { get; set; }
        public static DateTime UnixDate = new DateTime(1970,1,1);
        private int _birth_date;

        [Required]
        [Range(0, int.MaxValue)]
        public int id { get; set; }
        
        [Required]
        public string email { get; set; }
        
        [Required]
        public string first_name { get; set; }
        
        [Required]
        public string last_name { get; set; }
        
        [Required]
        [RegularExpression(@"^m|f$")]
        public string gender { get; set; }

        [Required]
        [Range(int.MinValue, int.MaxValue)]
        public int birth_date { 
            get {
                return _birth_date;
            } 
            set {
                _birth_date = value;
                SetAge();
            } 
        }

        private int age;

        internal int GetAge()
        {
            return age;
        }

        public void SetAge() { 
            var now = NowDate;
            //var now = DateTime.Now;
            var bDate = UnixDate.AddSeconds(birth_date);
            age = now.Year - bDate.Year;
            if ( bDate.Month > now.Month || bDate.Month == now.Month && bDate.Day > now.Day ) age--;
        }

        public override string ToString() 
        {
            return $"{id}";
        }
    }
    
}