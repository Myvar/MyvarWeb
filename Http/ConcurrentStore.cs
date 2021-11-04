using System;
using System.Collections.Concurrent;

namespace Prototype.Http
{
    public class ConcurrentStore<T>
    {
        private ConcurrentDictionary<Guid, T> _streams = new ConcurrentDictionary<Guid, T>();

        public T this[Guid id]
        {
            get { return _streams[id]; }
            set
            {
                if (_streams.ContainsKey(id))
                {
                    _streams[id] = value;
                }
                else
                {
                    while (_streams.TryAdd(id, value))
                    {
                    }
                }
            }
        }

        public void Delete(Guid id)
        {
            while (_streams.TryRemove(id, out var s))
            {
            }
        }
    }
}