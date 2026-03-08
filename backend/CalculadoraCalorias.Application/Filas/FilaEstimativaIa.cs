using CalculadoraCalorias.Application.DTOs.Records;
using System.Threading.Channels;

namespace CalculadoraCalorias.Application.Filas
{
    public class FilaEstimativaIa
    {
        private readonly Channel<EstimativaIaRequest> _canal;

        public FilaEstimativaIa()
        {
            var options = new UnboundedChannelOptions
            {
                SingleReader = true
            };

            _canal = Channel.CreateUnbounded<EstimativaIaRequest>(options);
        }

        public async ValueTask EnviarParaFilaAsync(EstimativaIaRequest request, CancellationToken cancellationToken = default)
        {
            await _canal.Writer.WriteAsync(request, cancellationToken);
        }

        public IAsyncEnumerable<EstimativaIaRequest> LerFilaAsync(CancellationToken cancellationToken = default)
        {
            return _canal.Reader.ReadAllAsync(cancellationToken);
        }
    }
}