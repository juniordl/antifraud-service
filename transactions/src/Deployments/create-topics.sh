sleep 10

kafka-topics.sh --create --topic evaluated-transactions-topic\
  --bootstrap-server kafka:29092 \
  --partitions 1 \
  --replication-factor 1

echo "created topic: evaluated-transactions-topic"

kafka-topics.sh --create --topic created-transactions-topic\
  --bootstrap-server kafka:29092 \
  --partitions 1 \
  --replication-factor 1

echo "created topic: created-transactions-topic"