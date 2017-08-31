using System;
using System.Collections;
using System.Collections.Generic;
using hiload.Model;
using System.Collections.Concurrent;

namespace hiload.MemDb
{
    public class DbSet<T> where T : class, IEntity, new()
    {
        // BTree<int, T> _set = new BTree<int, T>(2);
        T[] _set;

        public DbSet(int cp)
        {
            _set = new T[cp];
        }

        public T Find(int id)
        {
            return _set[id];
            // var ent = _set.Search(id);
            // T rez = ent == null ? null : ent.Pointer;
            // return rez;
        }

        public void Add(T value)
        {
            _set[value.id] = value;
            // lock(_set)
            // _set.Insert(value.id, value);
        }

        internal T Connect(int id)
        {
            return Find(id) ?? Proxy(id);
        }

        private T Proxy(int id)
        {
            return new T(){
                id = id
            };
        }
    }
}