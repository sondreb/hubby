using System;
using System.Threading;
using LiteNetLib;
using LiteNetLib.Utils;

namespace hubby.gateway
{
   class Program
   {
      static void Main(string[] args)
      {
         EventBasedNetListener listener = new EventBasedNetListener();
         NetManager server = new NetManager(listener);
         server.Start(15050);

         listener.ConnectionRequestEvent += request =>
         {
            if (server.ConnectedPeersCount < 10)
               request.AcceptIfKey("SomeConnectionKey");
            else
               request.Reject();
         };

         listener.PeerConnectedEvent += peer =>
         {
            Console.WriteLine("We got connection: {0}", peer.EndPoint); // Show peer ip

            NetDataWriter writer = new NetDataWriter();                 // Create writer class
            writer.Put("Hello client!");                                // Put some string
            peer.Send(writer, DeliveryMethod.ReliableOrdered);             // Send with reliability
         };

         while (!Console.KeyAvailable)
         {
            server.PollEvents();
            Thread.Sleep(15);
         }
         server.Stop();
      }
   }
}
