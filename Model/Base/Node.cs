using System;
using System.Collections;
using System.Collections.Generic;

namespace hiload.Model
{

    public class ListNode<T>
    {
        public T Value { get; }
        public ListNode<T> Prev {get; private set;}
        public ListNode<T> Next {get; protected set;}

        public ListNode(T value)
        {
            this.Value = value;
        }

        internal void Add(ListNode<T> prev)
        {
            lock (prev)
            {
                lock (this)
                {
                    Prev = prev;
                    Next = prev.Next;
                    if (Next != null) Next.Prev = this;
                    Prev.Next = this;
                }
            }
        }

        internal void Remove()
        {
            if (Prev == null) return;
            lock (Prev)
            {
                lock (this)
                {
                    Prev.Next = Next;
                    if (Next != null) Next.Prev = Prev;
                }
            }
        }

        public IEnumerable<T> GetAll()
        {
            var val = Next;
            while(val != null)
            {
                yield return val.Value;
                val = val.Next;
            }
        }

        public override string ToString() 
        {
            return $"{Value}";
        }
    }
}