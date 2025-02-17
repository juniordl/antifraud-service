namespace Common.Messaging.Core;

public class KafkaConfiguration
{
    public const string SectionName = "Kafka";
    public string Server { get; set; }
    public string ProducerTopic { get; set; }
    public string ConsumerGroup { get; set; }
    public string ConsumerTopic { get; set; }
    public string Offset { get; set; }
}