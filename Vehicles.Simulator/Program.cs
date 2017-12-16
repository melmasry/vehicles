using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Vehicles.Simulator.Simulators;

namespace Vehicles.Simulator
{
    class Program
    {
        static List<ISimulator> simulators;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Vehicles Simulator!");
            Console.Write("Please Enter API URL: ");
            string baseUrl = Console.ReadLine();
            if (baseUrl.EndsWith("/"))
                baseUrl = baseUrl.Substring(0, baseUrl.Length - 1);
            simulators = new List<ISimulator>();

            //Register any other simulators
            simulators.Add(new VehiclesPingSimulator());

            StartSimulators(baseUrl);
            Console.WriteLine("Simulating ...");

            // Establish an event handler to process key press events.
            Console.CancelKeyPress += new ConsoleCancelEventHandler(cancelHandler);
            while (true)
            {
                Console.Write("Press any key, or 'X' to quit or ");
                Console.WriteLine("CTRL+C to interrupt the simulation operation:");

                // Start a console read operation. Do not display the input.
                var cki = Console.ReadKey(true);

                // Announce the name of the key that was pressed .
                Console.WriteLine("  Key pressed: {0}\n", cki.Key);

                // Exit if the user pressed the 'X' key.
                if (cki.Key == ConsoleKey.X) break;
            }
        }
        protected static void cancelHandler(object sender, ConsoleCancelEventArgs args)
        {
            Console.WriteLine("\nThe simulation operation has been interrupted.");

            StopSimulators();
        }
        static void StartSimulators(string baseUrl)
        {
            foreach (ISimulator simulator in simulators)
            {
                simulator.InitializeSimulator(baseUrl);
                simulator.StartSimulation();
            }
        }
        static void StopSimulators()
        {
            foreach (ISimulator simulator in simulators)
            {
                simulator.StopSimulation();
            }
        }
    }
}