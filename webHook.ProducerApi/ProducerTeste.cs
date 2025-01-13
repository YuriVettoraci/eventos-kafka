using Confluent.Kafka;

namespace webHook.ProducerApi
{
    public class ProducerTeste
    {
        private readonly ILogger<ProducerTeste> logger;

        public ProducerTeste(ILogger<ProducerTeste> logger)
        {
            this.logger = logger;
        }

        public async Task PublicaEventoAsync(CancellationToken cancellationToken)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                AllowAutoCreateTopics = true,
                Acks = Acks.All
            };

            using var producer = new ProducerBuilder<Null, string>(config).Build();

            try
            {
                var resultado = await producer.ProduceAsync("topico-teste", new Message<Null, string>
                {
                    Value = "Evento de teste"
                },
                cancellationToken);

                logger.LogInformation("Mensagem enviada com sucesso: {0}", resultado.Value);
            }
            catch (ProduceException<Null, string> ex)
            {
                logger.LogError("Erro ao enviar mensagem: {0}", ex.Error.Reason);
            }

            producer.Flush(cancellationToken);
        }
    }
}
