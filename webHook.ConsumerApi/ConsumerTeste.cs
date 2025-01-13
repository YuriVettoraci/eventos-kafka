
using Confluent.Kafka;

namespace webHook.ConsumerApi
{
    public class ConsumerTesteJob : BackgroundService
    {
        private readonly ILogger<ConsumerTesteJob> logger;

        public ConsumerTesteJob(ILogger<ConsumerTesteJob> logger)
        {
            this.logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "teste",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

            consumer.Subscribe("topico-teste");

            while(!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var resultado = consumer.Consume(TimeSpan.FromSeconds(5));

                    if (resultado == null)
                        continue;

                    logger.LogInformation("Mensagem recebida: {0}", resultado.Message.Value);
                }
                catch(Exception ex)
                {
                    logger.LogError("Erro ao consumir mensagem: {0}", ex.Message);
                }
            }

            return Task.CompletedTask;
        }
    }
}
