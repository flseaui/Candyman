using UnityEngine;

namespace New
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject _grassPrefab;
        [SerializeField] private GameObject _factoryPrefab;

        private GameObject _map;

        private void Awake()
        {
            _map = GameObject.Find("Map");
        }
        
        private void Start()
        {
            var width = _grassPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
            var height = _grassPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
            for (var i = -15; i < 15; i++)
            {
                for (var j = -15; j < 15; j++)
                {
                    Instantiate(_grassPrefab, new Vector3(width * i, height * j, 0), Quaternion.identity, _map.transform);
                }
            }

            Instantiate(_factoryPrefab, new Vector3(0, 7, 0), Quaternion.identity, _map.transform);
        }
        
    }
}