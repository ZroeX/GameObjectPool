using ONIK.ObjectPool.Design;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ONIK.ObjectPool {
    public class EffectPool : MonoBehaviour {
        [Serializable]
        public class PoolPrefab {
            public string name = string.Empty;
            public int defaultNumber = 0;
            public GameObject[] prefabs = new GameObject[] { };
        }

        [SerializeField]
        private PoolPrefab[] poolPrefabs;

        private readonly Dictionary<string, GameObjectPool> poolTable = new Dictionary<string, GameObjectPool>();

        private static EffectPool instance;
        public static EffectPool Instance {
            get {
                if ( instance == null ) {
                    instance = FindObjectOfType<EffectPool>();
                    if ( instance == null ) {
                        Debug.LogError( "EffectPool is missing! Create an empty one." );

                        GameObject obj = new GameObject( "EffectPool" );
                        instance = obj.AddComponent<EffectPool>();
                    }
                }

                return instance;
            }
        }

        private void Awake() {
            for ( int i = 0; i < poolPrefabs.Length; ++i ) {
                PoolPrefab poolPrefab = poolPrefabs[ i ];

                GameObject root = new GameObject( poolPrefab.name );
                GameObjectPool pool = new GameObjectPool( poolPrefab.prefabs, poolPrefab.defaultNumber, root.transform, poolPrefab.name );

                poolTable.Add( poolPrefab.name, pool );
                root.transform.SetParent( transform );
            }

            instance = this;
        }

        public GameObject Get( string name ) {
            GameObjectPool pool;
            if ( poolTable.TryGetValue( name, out pool ) ) {
                return pool.Get();
            }
            else {
                Debug.LogWarningFormat( "User tried to get {0} but PoolManager cannot find this pool!", name );
                return new GameObject( string.Format( "{0} (null)", name ) );
            }
        }

        public void Return( GameObject obj ) {
            GameObjectPool pool;
            if ( poolTable.TryGetValue( obj.name, out pool ) ) {
                pool.Return( obj );
            }
            else {
                Debug.LogWarningFormat( "User returned {0} but PoolManager cannot find it's pool!", obj.name );
                Destroy( obj );
            }
        }
    }
}