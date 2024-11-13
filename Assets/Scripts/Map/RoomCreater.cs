using UnityEngine;

public class RoomCreator : MonoBehaviour
{
    public enum Axis { X, Z }

    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject[] floorPrefab;
    [SerializeField] private float wallSegmentSize = 4f;
    [SerializeField] private float floorTileSize = 1f;

    private void CreateWall(Vector3 startPosition, float length, Axis axis)
    {
        var segmentCount = Mathf.Max(1, Mathf.CeilToInt(length / wallSegmentSize));
        var actualSegmentSize = length / segmentCount;

        var direction = axis == Axis.X ? Vector3.right : Vector3.forward;
        var rotation = axis == Axis.X ? Quaternion.identity : Quaternion.Euler(0, 90, 0);

        for (var i = 0; i < segmentCount; i++)
        {
            var segment = Instantiate(wallPrefab, transform);
            segment.transform.position = startPosition + direction * (actualSegmentSize * i + actualSegmentSize / 2f);
            segment.transform.localScale = new Vector3(actualSegmentSize / wallSegmentSize, 1, 1);
            segment.transform.rotation = rotation;
        }
    }

    private void CreateFloor(Rect roomRect)
    {
        var tilesX = Mathf.CeilToInt(roomRect.width / floorTileSize);
        var tilesZ = Mathf.CeilToInt(roomRect.height / floorTileSize);

        var startX = roomRect.xMin + floorTileSize / 2f;
        var startZ = roomRect.yMin + floorTileSize / 2f;

        for (var x = 0; x < tilesX; x++)
        {
            for (var z = 0; z < tilesZ; z++)
            {
                var position = new Vector3(startX + x * floorTileSize, 0, startZ + z * floorTileSize);
                Instantiate(floorPrefab[Random.Range(0, floorPrefab.Length)], position, Quaternion.identity, transform);
            }
        }
    }

    public void CreateRoom(Rect roomRect)
    {
        var topLeft = new Vector3(roomRect.xMin, 0, roomRect.yMax);
        var bottomLeft = new Vector3(roomRect.xMin, 0, roomRect.yMin);
        var bottomRight = new Vector3(roomRect.xMax, 0, roomRect.yMin);

        // 벽 생성
        CreateWall(topLeft, roomRect.width, Axis.X);       // 상단 벽
        CreateWall(bottomLeft, roomRect.width, Axis.X);    // 하단 벽
        CreateWall(bottomLeft, roomRect.height, Axis.Z);   // 좌측 벽
        CreateWall(bottomRight, roomRect.height, Axis.Z);  // 우측 벽

        // 바닥 생성
        CreateFloor(roomRect);
    }

    private void Awake()
    {
        //CreateRoom(new Rect(0, 0, 20, 20));
    }
}
