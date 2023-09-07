using UnityEngine;

namespace RepaintingBlocks
{
    public class Ball : MonoBehaviour
    {
        public static event System.Action OnBallClicked;
        private int _score;
        private int _x;
        private int _y;



        #region Properties
        public int Score { get => _score; }
        public BallMoveOnGrid BallMoveOnGrid { get; private set; }
        public ColorBall ColorBall { get; private set; }
        public int X { get => _x; set => _x = value; }
        public int Y { get => _y; set => _y = value; }
        #endregion


        private void Awake()
        {
            BallMoveOnGrid = GetComponent<BallMoveOnGrid>();
            ColorBall = GetComponent<ColorBall>();  
        }

        public void InitOnGrid(int x, int y)
        {
            this.X = x;
            this.Y = y; 
        }



        private void OnMouseDown()
        {
            Debug.Log("Mouse down");
            if(ColorBall != null && Y == 0)
            {
                Shooter.Instance.LookAt(transform.position, -135f);
                Shooter.Instance.Shoot(transform.position, (ball) =>
                {
                    var listNB = GridSystem.Instance.FindConnectedGroup(X, Y, ColorBall.BallColorType);
                    foreach (var nb in listNB)
                    {
                        nb.ColorBall.SetColor(ball.ColorBall.BallColorType);
                    }

                    OnBallClicked?.Invoke();
                });


            }
        }
    }
}