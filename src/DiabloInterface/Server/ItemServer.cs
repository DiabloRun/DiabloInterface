using DiabloInterface.D2.Struct;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;

namespace DiabloInterface.Server
{
    class ItemServer
    {
        string pipeName;
        Thread listenThread;
        D2DataReader dataReader;

        public ItemServer(D2DataReader dataReader, string pipeName)
        {
            this.dataReader = dataReader;
            this.pipeName = pipeName;

            listenThread = new Thread(new ThreadStart(ServerListen));
            listenThread.Start();
        }

        void ServerListen()
        {
            var ps = new PipeSecurity();
            ps.AddAccessRule(new PipeAccessRule("Everyone", PipeAccessRights.ReadWrite,
                System.Security.AccessControl.AccessControlType.Allow));

            while (true)
            {
                var pipe = new NamedPipeServerStream(pipeName,
                    PipeDirection.InOut, 1,
                    PipeTransmissionMode.Message,
                    PipeOptions.Asynchronous,
                    1024, 1024, ps);
                try
                {
                    Console.WriteLine("Awaiting connection...");
                    pipe.WaitForConnection();
                    HandleClientConnection(pipe);
                }
                catch (IOException e)
                {
                    Console.WriteLine("Error: {0}", e.Message);
                }
                finally
                {
                    pipe.Close();
                }
            }
        }

        void HandleClientConnection(NamedPipeServerStream pipe)
        {
            Console.WriteLine("Client connected.");

            var reader = new JsonStreamReader(pipe, Encoding.UTF8);
            var request = reader.ReadJson<ItemRequest>();
            var equipmentLocations = GetItemLocations(request);

            Console.WriteLine("Got request: {0}", equipmentLocations.Count);

            ItemResponse response = new ItemResponse();
            dataReader.ItemSlotAction(equipmentLocations, (itemReader, item) => {
                ItemResponseData data = new ItemResponseData();
                data.ItemName = itemReader.GetFullItemName(item);
                data.Properties = itemReader.GetMagicalStrings(item);
                response.Items.Add(data);
            });

            Console.WriteLine("Got items data.");

            response.Success = response.Items.Count > 0;
            var writer = new JsonStreamWriter(pipe, Encoding.UTF8);
            writer.WriteJson(response);
            writer.Flush();

            Console.WriteLine("Response sent.");
        }

        List<BodyLocation> GetItemLocations(ItemRequest request)
        {
            List<BodyLocation> locations = new List<BodyLocation>();
            var name = request.EquipmentSlot.ToLowerInvariant();
            switch (name)
            {
                case "helm":
                case "head":
                    locations.Add(BodyLocation.Head);
                    break;
                case "armor":
                case "body":
                case "torso":
                    locations.Add(BodyLocation.BodyArmor);
                    break;
                case "amulet":
                    locations.Add(BodyLocation.Amulet);
                    break;
                case "ring":
                case "rings":
                    locations.Add(BodyLocation.RingLeft);
                    locations.Add(BodyLocation.RingRight);
                    break;
                case "belt":
                    locations.Add(BodyLocation.Belt);
                    break;
                case "glove":
                case "gloves":
                case "hand":
                    locations.Add(BodyLocation.Gloves);
                    break;
                case "boot":
                case "boots":
                case "foot":
                case "feet":
                    locations.Add(BodyLocation.Boots);
                    break;
                case "primary":
                case "weapon":
                    locations.Add(BodyLocation.PrimaryLeft);
                    break;
                case "offhand":
                case "shield":
                    locations.Add(BodyLocation.PrimaryRight);
                    break;
                case "weapon2":
                case "secondary":
                    locations.Add(BodyLocation.SecondaryLeft);
                    break;
                case "secondaryshield":
                case "secondaryoffhand":
                case "shield2":
                    locations.Add(BodyLocation.SecondaryRight);
                    break;
                default: break;
            }

            return locations;
        }
    }
}
