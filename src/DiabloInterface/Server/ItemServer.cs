using DiabloInterface.D2.Struct;
using DiabloInterface.Logging;
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

        public void Stop()
        {
            if (listenThread != null)
            {
                listenThread.Abort();
                listenThread = null;
            }
        }

        void ServerListen()
        {
            var ps = new PipeSecurity();
            System.Security.Principal.SecurityIdentifier sid = new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.BuiltinUsersSid, null);
            ps.AddAccessRule(new PipeAccessRule(sid, PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow));

            while (true)
            {
                NamedPipeServerStream pipe = null;
                try
                {
                    pipe = new NamedPipeServerStream(pipeName,
                        PipeDirection.InOut, 1,
                        PipeTransmissionMode.Message,
                        PipeOptions.Asynchronous,
                        1024, 1024, ps);
                    pipe.WaitForConnection();
                    HandleClientConnection(pipe);
                    pipe.Close();
                }
                catch (UnauthorizedAccessException e )
                {
                    // note: should only come here if another pipe with same name is already open (= another instance of d2interface is running)
                    Console.WriteLine("Error: {0}", e.Message);
                    Thread.Sleep(1000); // try again in 1 sec to prevent tool from lagging
                }
                catch (IOException e)
                {
                    Logger.Instance.WriteLine("ItemServer Error: {0}", e.Message);

                    if (pipe != null) pipe.Close();
                }
            }
        }

        void HandleClientConnection(NamedPipeServerStream pipe)
        {
            var reader = new JsonStreamReader(pipe, Encoding.UTF8);
            var request = reader.ReadJson<QueryRequest>();

            QueryResponse response = new QueryResponse();
            var equipmentLocations = GetItemLocations(request);

            dataReader.ItemSlotAction(equipmentLocations, (itemReader, item) => {
                ItemResponse data = new ItemResponse();
                data.ItemName = itemReader.GetFullItemName(item);
                data.Properties = itemReader.GetMagicalStrings(item);
                response.Items.Add(data);
            });

            response.IsValid = equipmentLocations.Count > 0;
            response.Success = response.Items.Count > 0;
            var writer = new JsonStreamWriter(pipe, Encoding.UTF8);
            writer.WriteJson(response);
            writer.Flush();
        }

        List<BodyLocation> GetItemLocations(QueryRequest request)
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
