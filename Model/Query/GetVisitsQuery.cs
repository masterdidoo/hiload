using System.ComponentModel.DataAnnotations;

namespace hiload.Model
{
    public class GetVisitsQuery
    {
        [Range(int.MinValue, int.MaxValue)]
        public int? fromDate {get;set;} 

        [Range(int.MinValue, int.MaxValue)]
        public int? toDate {get;set;}
        
        [Range(int.MinValue, int.MaxValue)]
        public int? toDistance {get;set;}
        
        public string country {get;set;}
    }
}