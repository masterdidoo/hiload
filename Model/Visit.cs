using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace hiload.Model
{
    public class Visit : IEntity
    {

        public Visit(){
            _userVisit =  new ListNode<Visit>(this);
            _locationVisit =  new ListNode<Visit>(this);
        }
        
        public int id { get; set; }
        public int location { 
                get {
                    return _locationId;
                } 
                set {
                    _locationVisit.Remove();
                    _locationId = value;
                    _location = (context.Locations.Connect(value));
                    if (_location != null) _location.AddVisit(_locationVisit);
                } 
            }

        private int _locationId;

        private ListNode<Visit> _locationVisit;
        private Location _location;

        internal Location GetLocation()
        {
            return _location;
        }

        public int user { 
                get {
                    return _userId;
                }
                set {
                    _userVisit.Remove();
                    _userId = value;
                    _user = (context.Users.Connect(value));
                    if (_user != null) _user.AddVisit(_userVisit);
                } 
            }

        private int _userId;

        private ListNode<Visit> _userVisit;

        private User _user;

        internal User GetUser()
        {
            return _user;
        }

        int _visited_at;

        public int visited_at {
            get {
                    return _visited_at;
                }
            set {
                _visited_at = value;

                if (_user != null) {
                    _userVisit.Remove();
                    _user.AddVisit(_userVisit);
                }
                
                if (_location != null) 
                {
                    _locationVisit.Remove();
                    _location.AddVisit(_locationVisit);
                }
            }
        }
        public int mark  { get; set; }

        internal static HiloadContext context;

        public override string ToString() 
        {
            return $"{id} {user} {location} {mark}";
        }
    }

    public class NotNull : ValidationAttribute {
        public override bool IsValid(object value) {
            return value != null;
        }
    }
}