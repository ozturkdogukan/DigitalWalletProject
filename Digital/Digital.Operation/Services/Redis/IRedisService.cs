using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Operation.Services.Redis
{
    public interface IRedisService
    {
        public Task<int> Ping();
        public Task<string> Get(string key);
        public Task<bool> Set(string key, string value);
        public Task<bool> Exist(string key);
        public Task<bool> Delete(string key);
        public void Flush();
        public bool SetDynamic<T>(string key, T value);
        public T GetDynamic<T>(string key);

    }
}
