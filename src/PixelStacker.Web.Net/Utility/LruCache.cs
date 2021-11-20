using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace PixelStacker.Web.Net.Utility
{
    /// <summary>
    /// A Least Recently Used cache implementation with forced expiry logic added on.
    /// https://stackoverflow.com/a/3719378/1582837
    /// https://searchstorage.techtarget.com/definition/cache-algorithm
    /// Any extra questions just ask Taylor Love.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class LruCache<K, V>
    {
        private int capacity;
        private TimeSpan? MaxTTL;
        private Dictionary<K, LinkedListNode<LRUCacheItem<K, V>>> cacheMap = new Dictionary<K, LinkedListNode<LRUCacheItem<K, V>>>();
        private LinkedList<LRUCacheItem<K, V>> lruList = new LinkedList<LRUCacheItem<K, V>>();
        public int Count { get { lock (lruList) { return lruList.Count; } } }

        public LruCache(int capacity)
        {
            this.capacity = capacity;
            MaxTTL = null;
        }

        public LruCache(int capacity, TimeSpan maxTTL)
        {
            this.capacity = capacity;
            this.MaxTTL = maxTTL;
        }

        public List<K> Keys => cacheMap.Keys.ToList();

        /// <summary>
        /// Retrieves the vale by key and then attempts to cast to type T. Returns 
        /// default(T) if value not in cache or value is not of needed type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool TryGetValue<T>(K key, out T val) where T : V
        {
            val = default(T);

            lock (cacheMap)
            {
                LinkedListNode<LRUCacheItem<K, V>> node;
                if (cacheMap.TryGetValue(key, out node))
                {
                    if (node.Value.ForceExpireAt != null && node.Value.ForceExpireAt.Value < DateTime.UtcNow)
                    {
                        lruList.Remove(node);
                        cacheMap.Remove(key);
                        return false;
                    }

                    lruList.Remove(node);
                    lruList.AddLast(node);
                    if (node.Value.value is T)
                    {
                        val = (T)node.Value.value;
                        return true;
                    }
                }

                return false;
            }
        }

        public void Set(K key, V val)
        {
            lock (cacheMap)
            {
                LinkedListNode<LRUCacheItem<K, V>> node;
                if (cacheMap.TryGetValue(key, out node))
                {
                    if (this.MaxTTL != null)
                    {
                        node.Value.ForceExpireAt = DateTime.UtcNow.Add(this.MaxTTL.Value);
                    }
                    node.Value.value = val;

                    lruList.Remove(node);   // Removal from LinkedList is O(1) if you have direct link to the Node.
                    lruList.AddLast(node);
                }
                else
                {
                    if (cacheMap.Count >= capacity)
                    {
                        RemoveFirst();
                    }

                    LRUCacheItem<K, V> cacheItem = this.MaxTTL.HasValue ?
                    new LRUCacheItem<K, V>(key, val, DateTime.UtcNow.Add(this.MaxTTL.Value))
                    : new LRUCacheItem<K, V>(key, val);
                    node = new LinkedListNode<LRUCacheItem<K, V>>(cacheItem);
                    lruList.AddLast(node);
                    cacheMap[key] = node;
                }
            }
        }

        public void Remove(K key)
        {
            lock (cacheMap)
            {
                LinkedListNode<LRUCacheItem<K, V>> node;
                if (cacheMap.TryGetValue(key, out node))
                {
                    lruList.Remove(node);   // Removal from LinkedList is O(1) if you have direct link to the Node.
                    cacheMap.Remove(key);
                }
            }
        }

        private void RemoveFirst()
        {
            // Remove from LRUPriority
            LinkedListNode<LRUCacheItem<K, V>> node = lruList.First;
            lruList.RemoveFirst();

            // Remove from cache
            cacheMap.Remove(node.Value.key);
        }

    }

    class LRUCacheItem<K, V>
    {
        public K key;
        public V value;
        public DateTime? ForceExpireAt;

        public LRUCacheItem(K k, V v)
        {
            key = k;
            value = v;
        }

        public LRUCacheItem(K k, V v, DateTime forceExpireAt)
        {
            key = k;
            value = v;
            ForceExpireAt = forceExpireAt;
        }

        public override string ToString()
        {
            return key + " => " + value;
        }
    }
}