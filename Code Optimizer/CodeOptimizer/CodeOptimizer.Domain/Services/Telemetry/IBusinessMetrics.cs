using System;
using System.Collections.Generic;
using System.Text;

namespace CodeOptimizer.Domain.Services.Telemetry
{
    public interface IBusinessMetrics
    {
        void TrackUserSignup();
        void TrackOptimizationCall(string language);
    }

}
