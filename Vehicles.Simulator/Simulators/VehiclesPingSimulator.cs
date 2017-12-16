using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Vehicles.Models;
using Vehicles.Simulator.Models;

namespace Vehicles.Simulator.Simulators
{
    public class VehiclesPingSimulator : ISimulator
    {
        static HttpClient client = new HttpClient();
        private IEnumerable<Vehicle> _vehicles;
        private string _baseUrl;
        public void InitializeSimulator(string baseURL)
        {
            _baseUrl = baseURL + "/Vehicles";
            _vehicles = LoadVehicles().Result;
        }

        private async Task<IEnumerable<Vehicle>> LoadVehicles()
        {
            IEnumerable<Vehicle> vehicles = null;

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(_baseUrl);
                response.EnsureSuccessStatusCode();
                using (HttpContent content = response.Content)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    vehicles = JsonConvert.DeserializeObject<Page<Vehicle>>(responseBody).Items;
                }
            }
            return vehicles;
        }

        public void StartSimulation()
        {
            var tokenSource = new CancellationTokenSource();
            foreach (Vehicle vehicle in _vehicles)
            {
                var cancellableTask = Task.Run(() =>
                {
                    SimulateVehicle(vehicle.Id, tokenSource);
                }, tokenSource.Token);
            }
        }

        private void SimulateVehicle(int vehicleId, CancellationTokenSource tokenSource)
        {
            Random rand = new Random();
            while(true)
            {
                Thread.Sleep((rand.NextDouble() < 0.8) ? 1 : rand.Next(1, 5) * 60000);
                PingVehicle(vehicleId);

                if (tokenSource.Token.IsCancellationRequested)
                {
                    // clean up before exiting
                    tokenSource.Token.ThrowIfCancellationRequested();
                    break;
                }
            }
        }
        private async void PingVehicle(int vehicleId)
        {
            string url = "/ping/" + vehicleId.ToString();
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.PostAsync(_baseUrl + url, null);
                response.EnsureSuccessStatusCode();
            }
        }
        public void StopSimulation()
        {
            
        }
    }
}
