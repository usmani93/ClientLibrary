using RabbitMQ.Client;
using System;
using System.Text;

namespace ClientLibrary
{
    internal class Send
    {
        ConnectionFactory factory = new ConnectionFactory();
        private IModel channel;

        public void SetupConnection(string hostName)
        {
            factory.HostName = hostName;
            var connection = factory.CreateConnection();
            channel = connection.CreateModel();
        }

        public void SendMessage(string queue, string message)
        {
            try
            {
                channel.QueueDeclare(queue: queue,
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                             routingKey: queue,
                                             basicProperties: null,
                                             body: body);
                Console.WriteLine(" [x] Sent {0}", message);

                Console.WriteLine(" Press [enter] to exit.");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    }
}
