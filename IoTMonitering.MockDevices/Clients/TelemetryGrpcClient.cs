using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceMock.Models;

namespace DeviceMock.Clients
{
    internal class TelemetryGrpcClient : TelemetryClient
    {
        public TelemetryGrpcClient(string endpoint) : base(endpoint)
        {
        }

        public override bool IsDeviceRegistered(string deviceId)
        {
            throw new NotImplementedException();
        }

        public override Task SendTelemetryAsync(Telemetry telemetry)
        {
            throw new NotImplementedException();
        }
    }
}
