using System;
using System.Threading;
using LiteNetLib;

namespace hubby
{
   class Program
   {
      static void Main(string[] args)
      {
         EventBasedNetListener listener = new EventBasedNetListener();
         NetManager client = new NetManager(listener);

         client.Start();
         client.Connect("city.hub.blockcore.net", 15050, "SomeConnectionKey");

         listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod) =>
         {
            Console.WriteLine("We got: {0}", dataReader.GetString(100 /* max length of string */));
            dataReader.Recycle();
         };

         while (!Console.KeyAvailable)
         {
            client.PollEvents();
            Thread.Sleep(15);
         }

         client.Stop();
      }
   }
}
