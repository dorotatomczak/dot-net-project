using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebClinicGUI.Models.Users;

namespace WebClinicGUI.Services
{
    public interface ICacheService
    {
        public Task<List<Physician>> GetPhysiciansAsync();
        public void SetPhysiciansAsync(List<Physician> physicians);
        public Task<List<Patient>> GetPatientsAsync();
        public void SetPatientsAsync(List<Patient> patients);
        public Task<bool> InvalidateCacheAsync();
    }
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly string _physicians = "Physicians";
        private readonly string _patients = "Patients";
        private TimeSpan _cacheTimeout = TimeSpan.FromSeconds(60);

        public CacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<List<Physician>> GetPhysiciansAsync()
        {
            var cacheResult = await _distributedCache.GetAsync(_physicians);
            if (cacheResult != null)
            {
                return GetObject<List<Physician>>(cacheResult);
            }
            return null;
        }

        public void SetPhysiciansAsync(List<Physician> physicians)
        {
            _distributedCache.SetAsync(_physicians, GetBytes(physicians),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _cacheTimeout
                });
        }
        public async Task<List<Patient>> GetPatientsAsync()
        {
            var cacheResult = await _distributedCache.GetAsync(_patients);
            if (cacheResult != null)
            {
                return GetObject<List<Patient>>(cacheResult);
            }
            return null;
        }

        public void SetPatientsAsync(List<Patient> patients)
        {
            _distributedCache.SetAsync(_patients, GetBytes(patients),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _cacheTimeout
                });
        }

        private byte[] GetBytes<T>(T element)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(element));
        }
        private T GetObject<T>(byte[] bytes)
        {
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes));
        }

        public async Task<bool> InvalidateCacheAsync()
        {
            await _distributedCache.RemoveAsync(_physicians);
            await _distributedCache.RemoveAsync(_patients);

            return true;
        }
    }
}
