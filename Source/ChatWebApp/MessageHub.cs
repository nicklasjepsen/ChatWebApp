using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace ChatWebApp
{
    public class MessageHub : Hub
    {
        /// <summary>
        /// Used to make sure usernames are unique
        /// </summary>
        public static ConcurrentDictionary<string, string> Usernames = new ConcurrentDictionary<string, string>();
        // TODO: No need for redundant dictionaries...
        public static ConcurrentDictionary<string, string> UserList = new ConcurrentDictionary<string, string>();
        
        public bool Login(string username)
        {
            if (Usernames.ContainsKey(username))
                return false;

            Usernames.TryAdd(username, Context.ConnectionId);
            UserList.TryAdd(Context.ConnectionId, username);

            // Notify all clients that a user is logged in
            Clients.All.showConnected(UserList);

            return true;
        }

        public bool SendMessage(string sender, string message)
        {
            Clients.All.displayMessage(sender, message);
            return true;
        }

        public bool SendMessage(string sender, string message, string recipient)
        {
            Clients.Client(Usernames[recipient]).displayMessage(sender, message);
            return true;
        }

        public void Logoff(string username)
        {
            RemoveUser();
            Clients.All.showConnected(UserList);
        }

        public override Task OnDisconnected()
        {
            RemoveUser();
            return Clients.All.showConnected(UserList);
        }

        /// <summary>
        /// Needed as of SignalR 2.1.0: http://stackoverflow.com/questions/24878187/signalr-detecting-alive-connection-in-c-sharp-clients
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            RemoveUser();
            return Clients.All.showConnected(UserList);
        }

        private void RemoveUser()
        {
            string username;
            string connectionId;
            UserList.TryRemove(Context.ConnectionId, out username);
            if (!string.IsNullOrEmpty(username))
                Usernames.TryRemove(username, out connectionId);
        }

        public void GetActiveClients(string requster)
        {
            Clients.All.showConnected(UserList);
        }
    }
}