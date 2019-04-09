using System;
using System.Collections.Generic;
using System.Text;

using Confluent.Kafka;
using Confluent.Kafka.Serialization;

namespace Manager
{
    public class DigiLogKafka
    {

        public void Main()
        {
            var config = new Dictionary<string, object>
            {
                { "bootstrap.servers", "localhost:9092" }
            };

            using (var producer = new Producer<Null, string>(config, null, new StringSerializer(Encoding.UTF8)))
            {
                string text = null;
                while (text != "exit")
                {
                    text = "Hi";
                    producer.ProduceAsync("hello-topic", null, text);
                }
                producer.Flush(100);
            }


        }

    }
}
