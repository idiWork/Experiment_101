using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppServiceBusReceive.Messages
{
    public class Telemetry
    {
            public float humidity { get; set; }
            public float temp { get; set; }
            public float pressure { get; set; }
            public float magnetometerX { get; set; }
            public float magnetometerY { get; set; }
            public float magnetometerZ { get; set; }
            public float accelerometerX { get; set; }
            public float accelerometerY { get; set; }
            public float accelerometerZ { get; set; }
            public float gyroscopeX { get; set; }
            public float gyroscopeY { get; set; }
            public float gyroscopeZ { get; set; }
            public string deviceState { get; set; }
    }
}
