using System;
using System.Collections.Generic;
using hiload.MemDb;

namespace hiload.Model
{
    public class HiloadContext
    {
        public HiloadContext()
        {
            Users = new DbSet<User>(4000000);
            Locations = new DbSet<Location>(3000000);
            Visits = new DbSet<Visit>(40000000); 

            _set = new Dictionary<Type, object>()
            {
                {typeof(User), Users},
                {typeof(Location), Locations},
                {typeof(Visit), Visits},
            };
        }

        Dictionary<Type, object> _set;

        public DbSet<User> Users { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Visit> Visits { get; set; }

        public DbSet<T> Set<T>() where T:class, IEntity, new()
        {
            return (DbSet<T>) _set[typeof(T)];
        }
    }
}