using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Activity.Message.Send
{
    class Producer
    {

        static void Main()
        {
            var messagesThread = new Thread(new ThreadStart(SendMessageToRabbitMq));
            messagesThread.Start();

        }

        public static void SendMessageToRabbitMq()
        {

            var activities = ReadActivitiesFromFile();

            var factory = new ConnectionFactory
            {
                HostName = "squid.rmq.cloudamqp.com",
                VirtualHost = "umvatzyf",
                UserName = "umvatzyf",
                Password = "GkrCoxT3sPmP-crezwf3194pAbHhc7p2"
            };


            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "Activities",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                foreach (Activity activity in activities)
                {
                    var json = JsonConvert.SerializeObject(activity);
                    var body = Encoding.UTF8.GetBytes(json);
                    channel.BasicPublish(exchange: "", routingKey: "Activities", basicProperties: null, body: body);

                    Thread.Sleep(1000);

                    Console.WriteLine(json);
                }

            }

        }

        public static List<Activity> ReadActivitiesFromFile()
        {
            List<Activity> activities = new List<Activity>();

            string text = System.IO.File.ReadAllText(@"D:\POLI\IV\SD\tema 2\Activity.Message.Send\activity.txt");

            string[] lines = text.Split("\t\r\n");
            foreach (string line in lines)
            {
                string[] lineData = line.Split("\t\t");
                if (lineData[2].Contains("\t"))
                {
                    lineData[2] = lineData[2][0..^1];
                }
                activities.Add(new Activity { StartDate = DateTime.Parse(lineData[0]), EndDate = DateTime.Parse(lineData[1]), ActivityName = lineData[2], PatientId = 5 });

            }

            return activities;
        }

    }
}
