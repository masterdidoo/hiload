using System.ComponentModel.DataAnnotations;


namespace hiload.Model
{
    public class GetAvgQuery {
        [Range(int.MinValue, int.MaxValue)]
        public int? fromDate { get; set; }
        
        [Range(int.MinValue, int.MaxValue)]
        public int? toDate { get; set; }
        
        [Range(int.MinValue, int.MaxValue)]
        public int? fromAge { get; set; }

        [Range(int.MinValue, int.MaxValue)]
        public int? toAge { get; set; }
        
        [RegularExpression(@"^m|f$")]
        public string gender { get; set; }
    }
}