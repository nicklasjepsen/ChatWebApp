using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ChatWebApp;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;

namespace WcfService
{
    public class SystemService : ISystemService
    {
        private readonly IHubProxy messageHub;

        public SystemService()
        {
            //var connection = new HubConnection("http://systemout.net:56677/ChatWebApp/~/");
            var connection = new HubConnection("http://localhost:32986/~/");
            messageHub = connection.CreateHubProxy("MessageHub");
            connection.Start().Wait();
        }

        public void SendMessage(string sender, string message)
        {
            messageHub.Invoke("SendMessage", sender, message);
        }
    }
}
