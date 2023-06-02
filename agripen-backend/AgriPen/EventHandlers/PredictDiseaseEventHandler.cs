using FastEndpoints;
using StackExchange.Redis;

namespace AgriPen.EventHandlers;

public class PredictionCreatedEvent
{
    public Ulid Id { get; set; }
}

public class PredictDiseaseEventHandler : IEventHandler<PredictionCreatedEvent>
{
    private const string ChannelName = "analyze_image";

    private readonly IConnectionMultiplexer _redisCn;

    public PredictDiseaseEventHandler(IConnectionMultiplexer redisCn)
    {
        _redisCn = redisCn;
    }

    public async Task HandleAsync(PredictionCreatedEvent eventModel, CancellationToken ct = default)
    {
        // publish to redis
        await _redisCn.GetDatabase(0).PublishAsync(ChannelName, eventModel.Id.ToString());
    }
}
