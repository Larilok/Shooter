using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace server
{
    class ServerSend
    {
        private static void SendTCPData(int toClient, Packet packet)
        {
            packet.WriteLength();
            Server.clients[toClient].tcp.SendData(packet);
        }

        private static void SendUDPData(int toClient, Packet packet)
        {
            packet.WriteLength();
            Server.clients[toClient].udp.SendData(packet);
        }

        private static void SendTCPDataToAll(Packet packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].tcp.SendData(packet);
            }
        }
        private static void SendTCPDataToAll(int exceptClient, Packet packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != exceptClient)
                {
                    Server.clients[i].tcp.SendData(packet);
                }
            }
        }

        private static void SendUDPDataToAll(Packet packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].udp.SendData(packet);
            }
        }
        private static void SendUDPDataToAll(int exceptClient, Packet packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != exceptClient)
                {
                    Server.clients[i].udp.SendData(packet);
                }
            }
        }

        #region Packets
        public static void Welcome(int toClient, string msg)
        {
            using (Packet packet = new Packet((int)ServerPackets.welcome))
            {
                packet.Write(msg);
                packet.Write(toClient);

                SendTCPData(toClient, packet);
            }
        }

        public static void SpawnPlayer(int toClient, Player player)
        {
            Console.WriteLine($"Spawn player {player.id} sent to {toClient}");
            
            using (Packet packet = new Packet((int)ServerPackets.spawnPlayer))
            {
                packet.Write(player.id);
                packet.Write(player.username);
                packet.Write(player.position);
                packet.Write(player.rotation);

                SendTCPData(toClient, packet);
            }
        }

        public static void PlayerPosition(Player player)
        {
            using (Packet packet = new Packet((int)ServerPackets.playerPosition))
            {
                packet.Write(player.id);
                packet.Write(player.position);
                packet.Write(player.invertGunSprite);
                packet.Write(player.gunRotation);
                SendUDPDataToAll(packet);
            }
        }
        
        public static void PlayerHealth(int toClient, Player hitPlayer)
        {
            using (Packet packet = new Packet((int)ServerPackets.playerHealth))
            {
                packet.Write(hitPlayer.id);
                packet.Write(hitPlayer.health);
                SendTCPData(toClient, packet);
            }
        }
        public static void PlayerHealthToAll(Player hitPlayer)
        {
            using (Packet packet = new Packet((int)ServerPackets.playerHealth))
            {
                packet.Write(hitPlayer.id);
                packet.Write(hitPlayer.health);
                SendTCPDataToAll(packet);
            }
        }

        internal static void SpawnBullet(int toClient, Vector3 pos, Vector2 vel)
        {
            // Console.WriteLine($"\n\nSERVER SEND: Bullet: Pos: {pos.X} {pos.Y} {pos.Z}, Vel: {vel.X} {vel.Y}\n");
            using (Packet packet = new Packet((int)ServerPackets.bulletSpawn))
            {
                packet.Write(pos);
                packet.Write(vel);
                SendUDPData(toClient, packet);
            }
        }
        internal static void HandleBoost(int toClient, bool intent, int boostId, Vector3 boostPos)
        {
            using (Packet packet = new Packet((int)ServerPackets.boostHandle))
            {
                packet.Write(intent);
                packet.Write(boostId);
                packet.Write(boostPos);
                SendUDPData(toClient, packet);
            }
        }

        public static void PlayerDisconnected(int playerId)
        {
            Console.WriteLine($"Player {playerId} disconnected");
            
            using (Packet packet = new Packet((int)ServerPackets.playerDisconnected))
            {
                packet.Write(playerId);
                SendTCPDataToAll(packet);
            }
        }
        #endregion

    }

}