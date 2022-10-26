using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ClientLibrary
{
    public class Receive
    {
        public event Action<string> DataReceived;

        ConnectionFactory factory = new ConnectionFactory();
        private IModel channel;

        public void SetupConnection(string hostName)
        {
            factory = new ConnectionFactory() { HostName = hostName };
            var connection = factory.CreateConnection();
            channel = connection.CreateModel();
        }

        public void ReceiveMessageInitialize(string queue)
        {
            channel.QueueDeclare(queue: queue,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += OnDataReceivedFromMQ;
            channel.BasicConsume(queue: queue,
                                 autoAck: true,
                                 consumer: consumer);
        }

        private void OnDataReceivedFromMQ(object sender, BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            if (DataReceived != null)
            {
                DataReceived(message);
            }
        }
    }
}