namespace IssueSystem.Services.HelpersServices.Cache
{
    using Microsoft.Extensions.Caching.Memory;

    public class InMemoryCach : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public InMemoryCach(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T Get<T>(string cacheID, Func<T> getItemCallback) where T : class
        {
            T item = _memoryCache.Get(cacheID) as T;
            if (item == null)
            {
                item = getItemCallback();
                _memoryCache.Set(cacheID, item);
            }
            return item;
        }

        public void Clear(string cacheId)
        {
            _memoryCache.Remove(cacheId);
        }
    }
}
