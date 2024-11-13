using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using Random = UnityEngine.Random;

//https://velog.io/@1217pgy/%EC%9C%A0%EB%8B%88%ED%8B%B0-%EC%A0%88%EC%B0%A8%EC%A0%81-%EC%83%9D%EC%84%B1%EC%9D%84-%EC%9C%84%ED%95%9C-%EB%8D%98%EC%A0%84-%EC%83%9D%EC%84%B1-%EC%95%8C%EA%B3%A0%EB%A6%AC%EC%A6%98
namespace DungeonGenerator
{
    public class TreeNode
    {
        public TreeNode(int x, int y, int width, int height)
        {
            TreeSize.x = x;
            TreeSize.y = y;
            TreeSize.width = width;
            TreeSize.height = height;
        }
        
        public RectInt TreeSize;
        
        public TreeNode LeftTree;
        public TreeNode RightTree;
        
        public RectInt DungeonSize;
    }

    public class DungeonGenerator : MonoBehaviour
    {
        [SerializeField] private Vector2Int mapSize;

        [SerializeField] private int maxNode;
        [SerializeField] private float minDivideSize;
        [SerializeField] private float maxDivideSize;
        [SerializeField] private int minRoomSize;
        [SerializeField] private NavMeshSurface surface;
        
        [SerializeField] private GameObject wallPrefab;
        [SerializeField] private GameObject floorPrefab;
        [SerializeField] private Transform wallTransform;
        [SerializeField] private Transform floorTransform;

        private readonly HashSet<Vector3> _roadPositions = new HashSet<Vector3>();
        
        public readonly List<RectInt> DungeonData = new List<RectInt>();
        
        public void Init()
        {
            TreeNode rootNode = new TreeNode(0, 0, mapSize.x, mapSize.y); //루트가 될 트리 생성

            DivideTree(rootNode, 0); //트리 분할
            GenerateDungeon(rootNode, 0); //방 생성
            GenerateRoad(rootNode, 0); //길 연결
            GenerateRoom(rootNode, 0);
            
            surface.BuildNavMesh();
        }
        
        private void DivideTree(TreeNode treeNode, int n) //재귀 함수
        {
            if (n >= maxNode) return;
            
            RectInt size = treeNode.TreeSize; //이전 트리의 범위 값 저장, 사각형의 범위를 담기 위해 Rect 사용
            var length = size.width >= size.height ? size.width : size.height; //사각형의 가로와 세로 중 길이가 긴 축을, 트리를 반으로 나누는 기준선으로 사용
            
            var split = Mathf.RoundToInt(Random.Range(length * minDivideSize, length * maxDivideSize)); //기준선 위에서 최소 범위와 최대 범위 사이의 값을 무작위로 선택
            
            if (size.width >= size.height) //가로
            {
                treeNode.LeftTree = new TreeNode(size.x, size.y, split, size.height); //기준선을 나눈 값인 split을 가로 길이로, 이전 트리의 height값을 세로 길이로 사용
                treeNode.RightTree = new TreeNode(size.x + split, size.y, size.width - split, size.height); //x값에 split값을 더해 좌표 설정, 이전 트리의 width값에 split값을 빼 가로 길이 설정
            }
            else //세로
            {
                treeNode.LeftTree = new TreeNode(size.x, size.y, size.width, split);
                treeNode.RightTree = new TreeNode(size.x, size.y + split, size.width, size.height - split);
            }
            
            DivideTree(treeNode.LeftTree, n + 1); //재귀 함수, 자식 트리를 매개변수로 넘기고 노드 값 1 증가 시킴
            DivideTree(treeNode.RightTree, n + 1);
        }

        private RectInt GenerateDungeon(TreeNode treeNode, int n) //방 생성
        {
            if (n == maxNode) //노드가 최하위일 때만 조건문 실행
            {
                RectInt size = treeNode.TreeSize;
                var width = Mathf.Max(Random.Range(size.width / 2, size.width - 1)); //트리 범위 내에서 무작위 크기 선택, 최소 크기 : width / 2
                var height = Mathf.Max(Random.Range(size.height / 2, size.height - 1));
                var x = treeNode.TreeSize.x + Random.Range(1, size.width - width); //최대 크기 : width / 2
                var y = treeNode.TreeSize.y + Random.Range(1, size.height - height);

                //OnDrawDungeon(x, y, width, height);
                
                return new RectInt(x, y, width, height); //리턴 값은 던전의 크기로 길을 생성할 때 크기 정보로 활용
            }
            treeNode.LeftTree.DungeonSize = GenerateDungeon(treeNode.LeftTree, n + 1); //리턴 값 = 던전 크기
            treeNode.RightTree.DungeonSize = GenerateDungeon(treeNode.RightTree, n + 1);
            
            return treeNode.LeftTree.DungeonSize; //부모 트리의 던전 크기는 자식 트리의 던전 크기 그대로 사용
        }

        private void GenerateRoom(TreeNode treeNode, int n)
        {
            if (n == maxNode)
            {
                OnDrawDungeon(treeNode.DungeonSize.x, treeNode.DungeonSize.y, treeNode.DungeonSize.width, treeNode.DungeonSize.height); //던전 렌더링
                DungeonData.Add(new RectInt(treeNode.DungeonSize.x - mapSize.x / 2, treeNode.DungeonSize.y - mapSize.y / 2, treeNode.DungeonSize.width, treeNode.DungeonSize.height));
                
                var go = new GameObject("POINT");
                go.transform.position = new Vector3(treeNode.DungeonSize.x - mapSize.x / 2, 0,treeNode.DungeonSize.y - mapSize.y / 2);
                
                
                return;
            }

            GenerateRoom(treeNode.LeftTree, n + 1);
            GenerateRoom(treeNode.RightTree, n + 1);
        }

        private void GenerateRoad(TreeNode treeNode, int n)
        {
            if (n == maxNode) return;

            var x1 = GetCenterX(treeNode.LeftTree.DungeonSize);
            var x2 = GetCenterX(treeNode.RightTree.DungeonSize);
            var y1 = GetCenterY(treeNode.LeftTree.DungeonSize);
            var y2 = GetCenterY(treeNode.RightTree.DungeonSize);

            int roadWidth = 5; // 길의 너비를 조절할 수 있습니다 (홀수로 설정 권장)
            int halfWidth = roadWidth / 2;

            // 수평 길 생성
            for (var x = Mathf.Min(x1, x2); x <= Mathf.Max(x1, x2); x++)
            {
                for (int offset = -halfWidth; offset <= halfWidth; offset++)
                {
                    int z = y1 + offset;

                    if (treeNode.LeftTree.DungeonSize.Contains(new Vector2Int(x, z)))
                        continue;

                    if (treeNode.RightTree.DungeonSize.Contains(new Vector2Int(x, z)))
                        continue;

                    _roadPositions.Add(new Vector3(x - mapSize.x / 2, 0, z - mapSize.y / 2));
                    Vector3 position = new Vector3(x - mapSize.x / 2, 0, z - mapSize.y / 2);
                    Instantiate(floorPrefab, position, Quaternion.identity, floorTransform);
                }

                // 양 옆에 벽 생성
                int leftWallZ = y1 + (-halfWidth - 1);
                int rightWallZ = y1 + (halfWidth + 1);

                // 왼쪽 벽
                if (!treeNode.LeftTree.DungeonSize.Contains(new Vector2Int(x, leftWallZ)) &&
                    !treeNode.RightTree.DungeonSize.Contains(new Vector2Int(x, leftWallZ)))
                {
                    Vector3 wallPosition = new Vector3(x - mapSize.x / 2, 0, leftWallZ- mapSize.y / 2);
                    Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallTransform);
                }

                // 오른쪽 벽
                if (!treeNode.LeftTree.DungeonSize.Contains(new Vector2Int(x, rightWallZ)) &&
                    !treeNode.RightTree.DungeonSize.Contains(new Vector2Int(x, rightWallZ)))
                {
                    Vector3 wallPosition = new Vector3(x - mapSize.x / 2, 0, rightWallZ- mapSize.y / 2);
                    Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallTransform);
                }
            }

            // 수직 길 생성
            for (var y = Mathf.Min(y1, y2); y <= Mathf.Max(y1, y2); y++)
            {
                for (int offset = -halfWidth; offset <= halfWidth; offset++)
                {
                    int x = x2 + offset;

                    if (treeNode.LeftTree.DungeonSize.Contains(new Vector2Int(x, y)))
                        continue;

                    if (treeNode.RightTree.DungeonSize.Contains(new Vector2Int(x, y)))
                        continue;

                    _roadPositions.Add(new Vector3(x - mapSize.x / 2, 0, y - mapSize.y / 2));
                    Vector3 position = new Vector3(x - mapSize.x / 2, 0, y - mapSize.y / 2);
                    Instantiate(floorPrefab, position, Quaternion.identity, floorTransform);
                }

                // 양 옆에 벽 생성
                int bottomWallX = x2 + (-halfWidth - 1);
                int topWallX = x2 + (halfWidth + 1);

                // 아래쪽 벽
                if (!treeNode.LeftTree.DungeonSize.Contains(new Vector2Int(bottomWallX, y)) &&
                    !treeNode.RightTree.DungeonSize.Contains(new Vector2Int(bottomWallX, y)))
                {
                    Vector3 wallPosition = new Vector3(bottomWallX - mapSize.x / 2, 0, y - mapSize.y / 2);
                    Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallTransform);
                }

                // 위쪽 벽
                if (!treeNode.LeftTree.DungeonSize.Contains(new Vector2Int(topWallX, y)) &&
                    !treeNode.RightTree.DungeonSize.Contains(new Vector2Int(topWallX, y)))
                {
                    Vector3 wallPosition = new Vector3(topWallX - mapSize.x / 2, 0, y - mapSize.y / 2);
                    Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallTransform);
                }
            }

            GenerateRoad(treeNode.LeftTree, n + 1);
            GenerateRoad(treeNode.RightTree, n + 1);
        }



        private void OnDrawDungeon(int x, int y, int width, int height)
        {
            // 바닥 생성
            for (int i = x; i < x + width; i++)
            {
                for (int j = y; j < y + height; j++)
                {
                    Vector3 position = new Vector3(i - mapSize.x / 2, 0, j - mapSize.y / 2);
                    Instantiate(floorPrefab, position, Quaternion.identity, floorTransform);
                }
            }

            // 벽 생성 (주변을 둘러싸는 벽)
            for (int i = x - 1; i <= x + width; i++)
            {
                for (int j = y - 1; j <= y + height; j++)
                {
                    // 바닥이 없는 곳에 벽 생성
                    if (i == x - 1 || i == x + width || j == y - 1 || j == y + height)
                    {
                        Vector3 position = new Vector3(i - mapSize.x / 2, 0, j - mapSize.y / 2);
                        if(_roadPositions.Contains(position))
                            continue;
                        
                        Instantiate(wallPrefab, position, Quaternion.identity, wallTransform);
                    }
                }
            }
        }

        private int GetCenterX(RectInt size)
        {
            return size.x + size.width / 2;
        }

        private int GetCenterY(RectInt size)
        {
            return size.y + size.height / 2;
        }
    }
}