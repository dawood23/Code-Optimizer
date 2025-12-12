using CodeOptimizer.Domain.Models;
using CodeOptimizer.Domain.Services.Metrices;
using CodeOptimizer.Domain.Services.Telemetry;
using System.Diagnostics.Metrics;

public class BusinessMetrics : IBusinessMetrics
{
    private readonly Counter<long> _signupCounter;
    private readonly Counter<long> _optimizationCounter;

    
    internal List<UserPerMonthCurrentYear> _cachedMonthlyUsers = new();
    public BusinessMetrics(Meter meter)
    {
        
        _signupCounter = meter.CreateCounter<long>("codeoptimizer.user_signups");
        _optimizationCounter = meter.CreateCounter<long>("codeoptimizer.optimization_calls");

        meter.CreateObservableGauge(
            "codeoptimizer.users_per_month",
            ObserveMonthlyUsers,
            description: "Users per month for current year");
    }

    public void TrackUserSignup()
    {
        _signupCounter.Add(1);
    }

    public void TrackOptimizationCall(string language)
    {
        _optimizationCounter.Add(
            1,
            new KeyValuePair<string, object?>("language", language));
    }

    private IEnumerable<Measurement<long>> ObserveMonthlyUsers()
    {
        return _cachedMonthlyUsers.Select(m =>
            new Measurement<long>(
                m.User,    
                new KeyValuePair<string, object?>("month", m.Month)
            )
        );
    }
}
