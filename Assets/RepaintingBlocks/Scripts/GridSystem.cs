using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RepaintingBlocks
{
    public class GridSystem : MonoBehaviour
    {
        public static GridSystem Instance { get; private set; }

        [Header("Grid Properties")]
        private Grid<Ball> _gridMap;
        [SerializeField] private int _width = 3;
        [SerializeField] private int _height = 4;
        [SerializeField] private float _cellSize = 1.55f;
        [SerializeField] private Vector3 _girdOffset;

        [Header("Prefabs")]
        [SerializeField] private Ball _ballPrefab;


        // Timer 
        private float _timeSpawnNewRow = 5.0f;
        private float _timeCounter = 0.0f;
        private float _timeFillMinSpawnRow = 1.0f;
        private float _timeFillMinCounter = 0.0f;
        private bool _initWhenStart = false;

        #region Properites
        public Grid<Ball> GridMap { get => _gridMap; }

        #endregion

        private void Awake()
        {
            Instance = this;

        }

        private void OnEnable()
        {
            Ball.OnBallClicked += HandleGridLogic;
        }

        private void OnDisable()
        {
            Ball.OnBallClicked -= HandleGridLogic;
        }



        private void Start()
        {
            Vector3 offset = new Vector3(-_width / 2f, -_height / 2f) + _girdOffset;
            _gridMap = new Grid<Ball>(_width, _height, _cellSize, offset);

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Ball ball = SpawnBall(x, y, ColorBall.ColorType.EMPTY);

                }
            }

            StartCoroutine(PerformFillRows(4));
        }



        private void Update()
        {
            if (GameplayManager.Instance.CurrentState != GameplayManager.GameState.PLAYING) return;
            if (Time.time - _timeFillMinCounter > _timeFillMinSpawnRow && _initWhenStart == true)
            {
                _timeFillMinCounter = Time.time;

                if (GetFilledRows() < 2)
                {
                    FillRows(3);
                }
            }


            if (Time.time - _timeCounter > _timeSpawnNewRow)
            {
                _timeCounter = Time.time;

                bool isFull = FillHorizontal();
                MoveBallsDown();

                if (isFull)
                {
                    GameplayManager.Instance.ChangeGameState(GameplayManager.GameState.GAMEOVER);
                }

            }
        }


        public void RemoveRows(int row)
        {
            if (row < 0 || row > _width - 1) return;
            for (int x = 0; x < _width; ++x)
            {
                GridMap.GetValue(x, row).ColorBall.SetColor(ColorBall.ColorType.EMPTY);
            }
        }

        public void RemoveRowWasFill()
        {
            for (int y = 0; y < _height; y++)
            {
                ColorBall.ColorType firstBallInRowColor = GridMap.GetValue(0, y).ColorBall.BallColorType;
                if (firstBallInRowColor == ColorBall.ColorType.EMPTY)
                    continue;

                bool isFillInRow = true;
                for (int x = 0; x < _width; x++)
                {
                    if (GridMap.GetValue(x, y).ColorBall.BallColorType != firstBallInRowColor)
                    {
                        isFillInRow = false;
                        break;
                    }
                }

                if (isFillInRow == true)
                {
                    GameManager.Instance.ScoreUp();
                    RemoveRows(y);

                    SoundManager.Instance.PlaySound(SoundType.ScoreUp, false);
                }
            }
        }

        public int GetFilledRows()
        {
            int emptyRows = 0;
            for(int  y = 0; y < _height; y++)
            {
                if(GridMap.GetValue(0,y).ColorBall.BallColorType != ColorBall.ColorType.EMPTY)
                {
                    emptyRows++;
                }
            }
            return emptyRows;
        }

        private void HandleGridLogic()
        {
            RemoveRowWasFill();
            MoveBallsDown();
        }


        private IEnumerator PerformFillRows(int numRows)
        {
            if (numRows > 0 && numRows < _height)
            {
                for (int i = 0; i < numRows; i++)
                {
                    FillHorizontal();
                    MoveBallsDown();

                    yield return new WaitForSeconds(0.2f);
                }
            }

            _initWhenStart = true;
        }

        private void FillRows(int numRows)
        {
            if (numRows > 0 && numRows < _height)
            {
                for (int i = 0; i < numRows; i++)
                {
                    FillHorizontal();
                    MoveBallsDown();

                }
            }
        }


        private Ball SpawnBall(int x, int y, ColorBall.ColorType colorType)
        {
            Ball ball = Instantiate(_ballPrefab, _gridMap.GetWorldPosition(x, y), Quaternion.identity);
            ball.InitOnGrid(x, y);
            ball.ColorBall.SetColor(colorType);

            _gridMap.SetValue(x, y, ball);
            return ball;
        }




        private IEnumerator PerformFillAllGrid(float fillTime, System.Action OnFinished)
        {
            while (true)
            {
                if (FillHorizontal() == true)
                {
                    OnFinished?.Invoke();
                    yield break;
                }
                else
                {
                    MoveBallsDown();
                }

                yield return new WaitForSeconds(fillTime);
            }
        }


        private bool FillHorizontal()
        {
            bool isfull = true;

            for (int x = 0; x < _width; x++)
            {
                Ball ballOnTop = _gridMap.GetValue(x, _height - 1);

                if (ballOnTop == null || ballOnTop.ColorBall.BallColorType == ColorBall.ColorType.EMPTY)
                {
                    ClearGridValue(x, _height - 1);
                    Ball ball = SpawnBall(x, _height - 1, Utilities.GetRandomEnumFromToMax<ColorBall.ColorType>(1));
                    isfull = false;
                }
            }

            return isfull;
        }


        public bool MoveBallsDown()
        {
            bool movedPiece = false;

            // Iterate through the grid from top to bottom and left to right
            for (int y = 1; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    Ball ball = _gridMap.GetValue(x, y);
                    Ball nbBelow = _gridMap.GetValue(x, y - 1); // Check the block below

                    if (ball.ColorBall.BallColorType != ColorBall.ColorType.EMPTY && nbBelow.ColorBall.BallColorType == ColorBall.ColorType.EMPTY)
                    {
                        ClearGridValue(x, y - 1);
                        SpawnBall(x, y, ColorBall.ColorType.EMPTY);

                        ball.BallMoveOnGrid.Move(x, y - 1, 0.2f);
                        _gridMap.SetValue(x, y - 1, ball);


                        x = -1;
                        y = 1;
                    }
                }
            }
            return movedPiece;
        }


        #region Grid Utilities
        public void ClearGridValue(int x, int y)
        {
            Destroy(GridMap.GetValue(x, y).gameObject);
            GridMap.SetValue(x, y, null);
        }

        public List<Ball> FindConnectedGroup(int x, int y, ColorBall.ColorType targetColor)
        {
            List<Ball> connectedGroup = new List<Ball>();
            bool[,] visited = new bool[_width, _height];

            // Define the relative positions of neighboring blocks (up, down, left, right)
            int[][] directions = new int[][] {
            new int[] { 0, 1 },   // Down
            new int[] { 0, -1 },  // Up
            new int[] { -1, 0 },  // Left
            new int[] { 1, 0 }    // Right
                };

            // DFS function to recursively find connected blocks
            void DFS(int currX, int currY)
            {
                visited[currX, currY] = true;
                connectedGroup.Add(GridMap.GetValue(currX, currY));

                foreach (var dir in directions)
                {
                    int newX = currX + dir[0];
                    int newY = currY + dir[1];

                    // Check if the new position is within the grid boundaries
                    if (newX >= 0 && newX < _width && newY >= 0 && newY < _height &&
                        !visited[newX, newY] &&
                        GridMap.GetValue(newX, newY).ColorBall.BallColorType == targetColor)
                    {
                        DFS(newX, newY);
                    }
                }
            }

            // Start DFS from the specified position (x, y)
            if (x >= 0 && x < _width && y >= 0 && y < _height && GridMap.GetValue(x, y).ColorBall.BallColorType == targetColor)
            {
                DFS(x, y);
            }

            return connectedGroup;
        }
        #endregion
    }
}