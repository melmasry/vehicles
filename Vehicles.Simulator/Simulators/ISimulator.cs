using System;
using System.Collections.Generic;
using System.Text;

namespace Vehicles.Simulator.Simulators
{
    public interface ISimulator
    {
        void InitializeSimulator(string baseURL);
        void StartSimulation();
        void StopSimulation();
    }
}
