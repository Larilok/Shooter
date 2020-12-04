using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace server
{
    public class BoostSpawner
    {
        public static Random rng = new Random();

        public int boostId;

        public BoostSpawner(int boostId)
        {
            this.boostId = boostId - 1;
        }

        public async void SpawnItem()
        {
            await Task.Delay(10000);

            Vector3 randomBoostPos = new Vector3(
              rng.Next(-Constants.HORIZONTAL_BORDER, Constants.HORIZONTAL_BORDER),
              rng.Next(-Constants.VERTICAL_BORDER, Constants.VERTICAL_BORDER),
              0
            );
            AddBoostIntoGame(boostId, randomBoostPos);
        }

        public static void AddBoostIntoGame(int boostId, Vector3 boostPos)
        {
            foreach (Client client in Server.clients.Values)
            {
                if (client.player != null)
                {
                    ServerSend.HandleBoost(client.id, true, boostId, boostPos);//Tell client to add a boost
                }
            }
        }
    }
}