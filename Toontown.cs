using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace ToontownLGP
{
    internal class Toontown
    {
        public Toontown()
        {
            _is_working = true;
            _thread = new Thread(() => this.ToontownThread());

            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            _session_authorization = new String(Enumerable.Repeat(chars, 16).Select(s => s[random.Next(s.Length)]).ToArray());

            _http_local_client = new HttpClient()
            {
                BaseAddress = new Uri(Resources.toontown_local_server_address)
            };
            _http_local_client.DefaultRequestHeaders.Add("User-Agent", Resources.toontown_server_user_agent);
            _http_local_client.DefaultRequestHeaders.Add("Accept", "*/*");
            _http_local_client.DefaultRequestHeaders.Add("Authorization", _session_authorization);


            _http_populations_client = new HttpClient()
            {
                BaseAddress = new Uri(Resources.toontown_populations_server_address)
            };
            _http_populations_client.DefaultRequestHeaders.Add("User-Agent", Resources.toontown_server_user_agent);
            _http_populations_client.DefaultRequestHeaders.Add("Accept", "*/*");
            _http_populations_client.DefaultRequestHeaders.Add("Authorization", _session_authorization);

            _http_silly_client = new HttpClient()
            {
                BaseAddress = new Uri(Resources.toontown_silly_server_address)
            };
            _http_silly_client.DefaultRequestHeaders.Add("User-Agent", Resources.toontown_server_user_agent);
            _http_silly_client.DefaultRequestHeaders.Add("Accept", "*/*");
            _http_silly_client.DefaultRequestHeaders.Add("Authorization", _session_authorization);
        }

        public void Start()
        {
            _thread.Start();
        }

        public void Stop()
        {
            _is_working = false;
            _http_local_client.CancelPendingRequests();
            _http_populations_client.CancelPendingRequests();
            _thread.Join();
        }

        public ToontownState? GetUpdate()
        {
            lock (this)
            {
                if (!_toontown_previous.Equals(_toontown_new))
                {
                    _toontown_previous = (ToontownState)_toontown_new.Clone();
                    return (ToontownState)_toontown_previous.Clone();
                }
            }

            return null;
        }

        private void ToontownThread()
        {
            while (_is_working)
            {
                ToontownState temp_state;
                lock (this)
                {
                    temp_state = (ToontownState)_toontown_new.Clone();
                }

                try
                {
                    Task<HttpResponseMessage> response_task = _http_local_client.GetAsync(Resources.toontown_local_server_path_info);
                    response_task.Wait();

                    Task<String> content_task = response_task.Result.Content.ReadAsStringAsync();
                    content_task.Wait();

                    ToontownInfoResponseRaw? toontown_local_response_raw = JsonSerializer.Deserialize<ToontownInfoResponseRaw>(content_task.Result);

                    temp_state.toon_name = toontown_local_response_raw?.toon.name;
                    temp_state.toon_species = toontown_local_response_raw?.toon.species;
                    temp_state.toon_current_laf = toontown_local_response_raw?.laff.current;
                    temp_state.toon_location_district = toontown_local_response_raw?.location.district;

                    temp_state.local_connected = true;
                    temp_state.local_connection_issue = false;
                }
                catch (Exception ex)
                {
                    temp_state.local_connected = false;
                    temp_state.local_connection_issue = true;
                    Console.WriteLine("[ERROR] Failed to execure local request: " + ex.ToString());
                }

                try
                {
                    Task<HttpResponseMessage> response_task = _http_populations_client.GetAsync(Resources.toontown_populations_server_path);
                    response_task.Wait();

                    Task<String> content_task = response_task.Result.Content.ReadAsStringAsync();
                    content_task.Wait();

                    ToontownPopulationResponseRaw? toontown_population_response_raw = JsonSerializer.Deserialize<ToontownPopulationResponseRaw>(content_task.Result);

                    temp_state.toon_popolation = GetDistrictPopulation(toontown_population_response_raw, temp_state.toon_location_district);
                    temp_state.total_popolation = toontown_population_response_raw?.totalPopulation;

                    temp_state.population_connected = true;
                    temp_state.population_connection_issue = false;
                }
                catch (Exception ex)
                {
                    temp_state.population_connected = false;
                    temp_state.population_connection_issue = true;
                    Console.WriteLine("[ERROR] Failed to execure populatios request: " + ex.ToString());
                }

                try
                {
                    Task<HttpResponseMessage> response_task = _http_silly_client.GetAsync(Resources.toontown_silly_server_path);
                    response_task.Wait();

                    Task<String> content_task = response_task.Result.Content.ReadAsStringAsync();
                    content_task.Wait();

                    ToontownSillyResponseRaw? toontown_silly_response_raw = JsonSerializer.Deserialize<ToontownSillyResponseRaw>(content_task.Result);

                    temp_state.silly_meter_state = toontown_silly_response_raw?.state;
                    temp_state.silly_meter_hp = toontown_silly_response_raw?.hp;

                    temp_state.silly_connected = true;
                    temp_state.silly_connection_issue = false;
                }
                catch (Exception ex)
                {
                    temp_state.silly_connected = false;
                    temp_state.silly_connection_issue = true;
                    Console.WriteLine("[ERROR] Failed to execure populatios request: " + ex.ToString());
                }

                lock (this)
                {
                    _toontown_new = (ToontownState)temp_state.Clone();
                }
            }
        }

        private static int? GetDistrictPopulation(ToontownPopulationResponseRaw? population_info_raw, String? district)
        {
            switch (district)
            {
                case "Zoink Falls": return population_info_raw?.populationByDistrict.ZoinkFalls;
                case "Zapwood": return population_info_raw?.populationByDistrict.Zapwood;
                case "Splatville": return population_info_raw?.populationByDistrict.Splatville;
                case "Bounceboro": return population_info_raw?.populationByDistrict.Bounceboro;
                case "Whoosh Rapids": return population_info_raw?.populationByDistrict.WhooshRapids;
                case "Splashport": return population_info_raw?.populationByDistrict.Splashport;
                case "Kaboom Cliffs": return population_info_raw?.populationByDistrict.KaboomCliffs;
                case "Blam Canyon": return population_info_raw?.populationByDistrict.BlamCanyon;
                case "Thwackville": return population_info_raw?.populationByDistrict.Thwackville;
                case "Hiccup Hills": return population_info_raw?.populationByDistrict.HiccupHills;
                case "Splat Summit": return population_info_raw?.populationByDistrict.SplatSummit;
                case "Gulp Gulch": return population_info_raw?.populationByDistrict.GulpGulch;
                case "Boingbury": return population_info_raw?.populationByDistrict.Boingbury;
                case "Fizzlefield": return population_info_raw?.populationByDistrict.Fizzlefield;
                default: return null;
            }
        }

        public struct ToontownState : ICloneable
        {
            public bool local_connected = false;
            public bool local_connection_issue = false;

            public bool population_connected = false;
            public bool population_connection_issue = false;

            public bool silly_connected = false;
            public bool silly_connection_issue = false;

            public String? toon_name;
            public String? toon_species;
            public int? toon_current_laf;
            public String? toon_location_district;

            public int? toon_popolation;
            public int? total_popolation;

            public String? silly_meter_state;
            public int? silly_meter_hp;

            public ToontownState()
            {
            }

            public bool Equals(ToontownState obj)
            {
                return local_connected == obj.local_connected &&
                        local_connection_issue == obj.local_connection_issue &&
                        population_connected == obj.population_connected &&
                        population_connection_issue == obj.population_connection_issue &&
                        silly_connected == obj.silly_connected &&
                        silly_connection_issue == obj.silly_connection_issue &&
                        toon_name == obj.toon_name &&
                        toon_species == obj.toon_species &&
                        toon_current_laf == obj.toon_current_laf &&
                        toon_location_district == obj.toon_location_district &&
                        toon_popolation == obj.toon_popolation &&
                        total_popolation == obj.total_popolation &&
                        silly_meter_state == obj.silly_meter_state &&
                        silly_meter_hp == obj.silly_meter_hp;
            }

            public object Clone()
            {
                return this.MemberwiseClone();
            }
        }

        private class ToontownPopulationResponseRaw
        {
            public class ToontownPopulationResponsePopulationByDistrictRaw
            {
                [JsonPropertyName("Zoink Falls")]
                public int ZoinkFalls { get; set; }
                public int Zapwood { get; set; }
                public int Splatville { get; set; }
                public int Bounceboro { get; set; }

                [JsonPropertyName("Whoosh Rapids")]
                public int WhooshRapids { get; set; }
                public int Splashport { get; set; }
                
                [JsonPropertyName("Kaboom Cliffs")]
                public int KaboomCliffs { get; set; }
                
                [JsonPropertyName("Blam Canyon")]
                public int BlamCanyon { get; set; }
                public int Thwackville { get; set; }
                
                [JsonPropertyName("Hiccup Hills")]
                public int HiccupHills { get; set; }
                
                [JsonPropertyName("Splat Summit")]
                public int SplatSummit { get; set; }
                
                [JsonPropertyName("Gulp Gulch")]
                public int GulpGulch { get; set; }
                public int Boingbury { get; set; }
                public int Fizzlefield { get; set; }
            }

            public struct ToontownPopulationResponseStatusByDistrictRaw
            {
                [JsonPropertyName("Zoink Falls")]
                public String ZoinkFalls { get; set; }
                public String Zapwood { get; set; }
                public String Splatville { get; set; }
                public String Bounceboro { get; set; }

                [JsonPropertyName("Whoosh Rapids")]
                public String WhooshRapids { get; set; }
                public String Splashport { get; set; }
                public String KaboomCliffs { get; set; }

                [JsonPropertyName("Blam Canyon")]
                public String BlamCanyon { get; set; }
                public String Thwackville { get; set; }

                [JsonPropertyName("Hiccup Hills")]
                public String HiccupHills { get; set; }

                [JsonPropertyName("Splat Summit")]
                public String SplatSummit { get; set; }

                [JsonPropertyName("Gulp Gulch")]
                public String GulpGulch { get; set; }
                public String Boingbury { get; set; }
                public String Fizzlefield { get; set; }
            }

            public int lastUpdated { get; set; }
            public int totalPopulation { get; set; }

            public ToontownPopulationResponsePopulationByDistrictRaw populationByDistrict { get; set; }
            public ToontownPopulationResponseStatusByDistrictRaw statusByDistrict { get; set; }
        }


        private struct ToontownInfoResponseRaw
        {
            public struct ToontownInfoResponseToonRaw
            {
                public String name { get; set; }
                public String species { get; set; }
                public String headColor { get; set; }
                public String style { get; set; }
            }

            public struct ToontownInfoResponseLaffRaw
            {
                public int current { get; set; }
                public int max { get; set; }
            }

            public struct ToontownInfoResponseLocationRaw
            {
                public String zone { get; set; }
                public String neighborhood { get; set; }
                public String district { get; set; }
            }

            public struct ToontownInfoResponseInvasionRaw
            {
                public String cog { get; set; }
                public int quantity { get; set; }
                public bool mega { get; set; }
            }

            public ToontownInfoResponseToonRaw toon { get; set; }
            public ToontownInfoResponseLaffRaw laff { get; set; }
            public ToontownInfoResponseLocationRaw location { get; set; }
            public ToontownInfoResponseInvasionRaw? invasion { get; set; }
        }

        private struct ToontownSillyResponseRaw
        {
            public String state { get; set; }

            public String[] rewards { get; set; }

            public String[] rewardDescriptions { get; set; }

            public int?[] rewardPoints { get; set; }

            public String winner { get; set; }

            public int winnerId { get; set; }

            public int hp { get; set; }

            public int nextUpdateTimestamp { get; set; }

            public int asOf { get; set; }
        }

        Thread _thread;
        bool _is_working;

        private String _session_authorization;
        private HttpClient _http_local_client;
        private HttpClient _http_populations_client;
        private HttpClient _http_silly_client;

        ToontownState _toontown_previous;
        ToontownState _toontown_new;
    }
}
