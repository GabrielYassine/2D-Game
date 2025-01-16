using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FishNet.Component.Spawning
{
    /// <summary>
    /// Spawns a player object for clients when they connect.
    /// Must be placed on or beneath the NetworkManager object.
    /// </summary>
    [AddComponentMenu("FishNet/Component/PlayerSpawner")]
    public class PlayerSpawner : MonoBehaviour
    {
        #region Public.
        /// <summary>
        /// Called on the server when a player is spawned.
        /// </summary>
        public event Action<NetworkObject> OnSpawned;
        #endregion

        #region Serialized.
        /// <summary>
        /// List of prefabs to spawn for the player.
        /// </summary>
        [Tooltip("List of prefabs to spawn for the player.")]
        [SerializeField]
        private List<NetworkObject> _playerPrefabs = new List<NetworkObject>();

        /// <summary>
        /// Sets the PlayerPrefabs to use.
        /// </summary>
        /// <param name="nobs"></param>
        public void SetPlayerPrefabs(List<NetworkObject> nobs) => _playerPrefabs = nobs;

        /// <summary>
        /// True to add player to the active scene when no global scenes are specified through the SceneManager.
        /// </summary>
        [Tooltip("True to add player to the active scene when no global scenes are specified through the SceneManager.")]
        [SerializeField]
        private bool _addToDefaultScene = true;

        /// <summary>
        /// Areas in which players may spawn.
        /// </summary>
        [Tooltip("Areas in which players may spawn.")]
        public Transform[] Spawns = new Transform[0];
        #endregion

        #region Private.
        /// <summary>
        /// NetworkManager on this object or within this object's parents.
        /// </summary>
        private NetworkManager _networkManager;

        /// <summary>
        /// Next spawns to use.
        /// </summary>
        private int _nextSpawn;

        /// <summary>
        /// Tracks which prefab is currently assigned to each player connection.
        /// </summary>
        private Dictionary<NetworkConnection, NetworkObject> _connectedPlayerPrefabs = new Dictionary<NetworkConnection, NetworkObject>();

        /// <summary>
        /// Tracks which prefabs are currently in use.
        /// </summary>
        private HashSet<NetworkObject> _usedPrefabs = new HashSet<NetworkObject>();
        #endregion

        private void Start()
        {
            InitializeOnce();
        }

        private void OnDestroy()
        {
            if (_networkManager != null)
                _networkManager.SceneManager.OnClientLoadedStartScenes -= SceneManager_OnClientLoadedStartScenes;
        }

        /// <summary>
        /// Initializes this script for use.
        /// </summary>
        private void InitializeOnce()
        {
            _networkManager = InstanceFinder.NetworkManager;
            if (_networkManager == null)
            {
                NetworkManagerExtensions.LogWarning($"PlayerSpawner on {gameObject.name} cannot work as NetworkManager wasn't found on this object or within parent objects.");
                return;
            }

            _networkManager.SceneManager.OnClientLoadedStartScenes += SceneManager_OnClientLoadedStartScenes;
            _networkManager.ServerManager.OnRemoteConnectionState += ServerManager_OnRemoteConnectionState;
        }

        /// <summary>
        /// Called when a remote client's connection state changes.
        /// </summary>
        private void ServerManager_OnRemoteConnectionState(NetworkConnection conn, FishNet.Transporting.RemoteConnectionStateArgs args)
        {
            if (args.ConnectionState == FishNet.Transporting.RemoteConnectionState.Stopped)
            {
                if (_connectedPlayerPrefabs.TryGetValue(conn, out NetworkObject prefab))
                {
                    // Remove the prefab from the used set and mapping when the player disconnects.
                    _usedPrefabs.Remove(prefab);
                    _connectedPlayerPrefabs.Remove(conn);
                }
            }
        }


        /// <summary>
        /// Called when a client loads initial scenes after connecting.
        /// </summary>
        private void SceneManager_OnClientLoadedStartScenes(NetworkConnection conn, bool asServer)
        {
            if (!asServer)
                return;

            if (_playerPrefabs == null || _playerPrefabs.Count == 0)
            {
                NetworkManagerExtensions.LogWarning($"Player prefabs list is empty and cannot spawn a player for connection {conn.ClientId}.");
                return;
            }

            // Select a prefab to spawn that is not already in use.
            NetworkObject selectedPrefab = GetNextAvailablePlayerPrefab();
            if (selectedPrefab == null)
            {
                NetworkManagerExtensions.LogWarning($"No available player prefabs to spawn for connection {conn.ClientId}.");
                return;
            }

            Vector3 position;
            Quaternion rotation;
            SetSpawn(selectedPrefab.transform, out position, out rotation);

            NetworkObject nob = _networkManager.GetPooledInstantiated(selectedPrefab, position, rotation, true);
            _networkManager.ServerManager.Spawn(nob, conn);

            // Track prefab usage for this player.
            _connectedPlayerPrefabs[conn] = selectedPrefab;
            _usedPrefabs.Add(selectedPrefab);

            // If there are no global scenes.
            if (_addToDefaultScene)
                _networkManager.SceneManager.AddOwnerToDefaultScene(nob);

            OnSpawned?.Invoke(nob);
        }

        /// <summary>
        /// Gets the next available player prefab that is not in use.
        /// </summary>
        /// <returns></returns>
        private NetworkObject GetNextAvailablePlayerPrefab()
        {
            foreach (var prefab in _playerPrefabs)
            {
                if (!_usedPrefabs.Contains(prefab))
                    return prefab;
            }
            return null; // No available prefab.
        }

        /// <summary>
        /// Sets a spawn position and rotation.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="rot"></param>
        private void SetSpawn(Transform prefab, out Vector3 pos, out Quaternion rot)
        {
            // No spawns specified.
            if (Spawns.Length == 0)
            {
                SetSpawnUsingPrefab(prefab, out pos, out rot);
                return;
            }

            Transform result = Spawns[_nextSpawn];
            if (result == null)
            {
                SetSpawnUsingPrefab(prefab, out pos, out rot);
            }
            else
            {
                pos = result.position;
                rot = result.rotation;
            }

            // Increase next spawn and reset if needed.
            _nextSpawn++;
            if (_nextSpawn >= Spawns.Length)
                _nextSpawn = 0;
        }

        /// <summary>
        /// Sets spawn using values from prefab.
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="pos"></param>
        /// <param name="rot"></param>
        private void SetSpawnUsingPrefab(Transform prefab, out Vector3 pos, out Quaternion rot)
        {
            pos = prefab.position;
            rot = prefab.rotation;
        }
    }
}
