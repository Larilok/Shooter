using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace server
{
    class ServerHandle
    {
        public static void WelcomeReceived(int fromClient, Packet packet)
        {
            int clientIdCheck = packet.ReadInt();
            string username = packet.ReadString();

            Console.WriteLine($"{Server.clients[fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {fromClient}.");
            if (fromClient != clientIdCheck)
            {
                Console.WriteLine($"Player \"{username}\" (ID: {fromClient}) has assumed the wrong client ID ({clientIdCheck})!");
            }
            Server.clients[fromClient].SendIntoGame(username);
        }

        public static void PlayerMovement(int fromClient, Packet packet)
        {
            bool[] inputs = new bool[packet.ReadInt()];
            for (int i = 0; i < inputs.Length; i++)
            {
                inputs[i] = packet.ReadBool();
            }
            bool invert = packet.ReadBool();
            float angle = packet.ReadFloat();

            Server.clients[fromClient].player.SetInput(inputs, invert, angle);
        }

        public static void EnemyHit(int fromClient, Packet packet)
        {
            if(!Server.IsRoundStarted())
            {
                return;
            }
            int playerId = packet.ReadInt();
            Server.clients[playerId].player.SetHealth(fromClient);
            Console.WriteLine($"{playerId} was hit by player {fromClient}.");
            Server.clients[fromClient].SendHitIntoGame(playerId);
        }

        internal static void BulletSpawn(int fromClient, Packet packet)
        {
            Vector3 pos = packet.ReadVector3();
            Vector2 vel = packet.ReadVector2();
            // Console.WriteLine($"\n\nSERVER RECEIVE: Bullet: Pos: {pos.X} {pos.Y} {pos.Z}, Vel: {vel.X} {vel.Y}\n");
            Server.clients[fromClient].SendBulletIntoGame(pos, vel);
        }

        internal static void BoostHandle(int fromClient, Packet packet)
        {
            bool intent = packet.ReadBool();//Add/Remove boost - should always be false(Remove) here
            int boostId = packet.ReadInt();
            Vector3 boostPos = packet.ReadVector3();
            Server.clients[fromClient].RemoveBoostFromGame(boostId, boostPos);
            Server.clients[fromClient].player.AddBoost(boostId);
            Server.spawners[boostId + 1].SpawnItem();
        }

        public static void RoundStart(int fromClient, Packet packet)
        {
            int clientId = packet.ReadInt();
            if (clientId != 1)
            {
                Console.WriteLine($"Forbidden attempt to start a round from client: {clientId}");
                return;
            }
            Server.StartRound();
        }

    }
}
