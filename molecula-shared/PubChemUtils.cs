using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace molecula_shared
{
    public static class PubChemUtils
    {//mainly to make sure to not "harm" PubChem 
     //see https://pubchemdocs.ncbi.nlm.nih.gov/programmatic-access$_requestvolumelimitations
     //No more than 5 requests per second.
     //No more than 400 requests per minute.
     //No longer than 300 second running time per minute.

        private static RestClient _client = new RestClient("https://pubchem.ncbi.nlm.nih.gov/rest/pug/compound/");

        private static Dictionary<DateTime, double> _requests = new Dictionary<DateTime, double>();

        public static async Task<T> GetData<T>(string requestUrl)
        {
            CleanUpRequests();
            var start = DateTime.Now;
            await Wait(start);
            var request = new RestRequest(requestUrl, DataFormat.Json);
            var res = await _client.GetAsync<T>(request);
            _requests[start] = (DateTime.Now - start).TotalSeconds;
            return res;
        }

        private static async Task Wait(DateTime startTime)
        {
            if (_requests.Count == 0) return;
            //No more than 5 requests per second
            var requests = (from req in _requests.Keys
                            where (startTime - req).TotalSeconds < 1
                            select startTime - req).OrderByDescending(t => t.TotalSeconds).ToList();
            if (requests != null && requests.Count > 4)
            {
                await Task.Delay(1000 - (int)requests[0].TotalMilliseconds);
            }
            //No more than 400 requests per minute.
            var minutRequests = (from req in _requests.Keys
                                 where (startTime - req).TotalMinutes < 1
                                 select new { TotalMinutes = (startTime - req).TotalMinutes, Duration = _requests[req] }).OrderByDescending(t => t.TotalMinutes).ToList();
            if (minutRequests != null)
            {
                if (minutRequests.Count > 399)
                    await Task.Delay(TimeSpan.FromMinutes(1d - minutRequests[0].TotalMinutes));
                //No longer than 300 second running time per minute.
                var totalSecsInLastMinute = minutRequests.Select(t => t.Duration).Aggregate((secs1, secs2) => secs1 + secs2);
                if (totalSecsInLastMinute > 300d)
                {
                    //leave at least 10 seconds for the upcoming request
                    await Task.Delay(TimeSpan.FromSeconds(totalSecsInLastMinute - 300d + 10d));
                }
            }
        }

        /// <summary>
        /// remove everything from the history older than one minute
        /// </summary>
        private static void CleanUpRequests()
        {
            var now = DateTime.Now;
            var keys = _requests.Keys.ToArray();
            foreach (var item in keys)
            {
                if ((now - item).TotalMinutes > 1d)
                {
                    _requests.Remove(item);
                }
            }
        }
    }
}
