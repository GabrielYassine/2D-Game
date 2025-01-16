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
        /// NetworkManager on this object or within this objects parents.
        /// </summary>
        private NetworkManager _networkManager;
        /// <summary>
        /// Next spawns to use.
        /// </summary>
        private int _nextSpawn;

        /// <summary>
        /// Index for selecting player prefabs.
        /// </summary>
        private int _nextPlayerPrefabIndex;
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

            // Select a prefab to spawn.
            NetworkObject selectedPrefab = GetNextPlayerPrefab();

            Vector3 position;
            Quaternion rotation;
            SetSpawn(selectedPrefab.transform, out position, out rotation);

            NetworkObject nob = _networkManager.GetPooledInstantiated(selectedPrefab, position, rotation, true);
            _networkManager.ServerManager.Spawn(nob, conn);

            // If there are no global scenes.
            if (_addToDefaultScene)
                _networkManager.SceneManager.AddOwnerToDefaultScene(nob);

            OnSpawned?.Invoke(nob);
        }

        /// <summary>
        /// Gets the next player prefab in the list.
        /// </summary>
        /// <returns></returns>
        private NetworkObject GetNextPlayerPrefab()
        {
            NetworkObject selectedPrefab = _playerPrefabs[_nextPlayerPrefabIndex];
            _nextPlayerPrefabIndex = (_nextPlayerPrefabIndex + 1) % _playerPrefabs.Count;
            return selectedPrefab;
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
