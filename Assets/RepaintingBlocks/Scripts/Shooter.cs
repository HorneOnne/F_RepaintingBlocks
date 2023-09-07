using UnityEngine;
using System.Collections;

namespace RepaintingBlocks
{
    public class Shooter : MonoBehaviour
    {
        public static Shooter Instance { get; private set; }

        [SerializeField] private Transform _ballPosition01;
        [SerializeField] private Transform _ballPosition02;
        [SerializeField] private Ball _ballPrefab;

        [SerializeField] private Ball _currentShootBall;
        [SerializeField] private Ball _nextShootBall;
        private Ball _ball01;
        private Ball _ball02;


        private float _rotationSpeed = 10f;
        private bool _isAddedOffsetRotate = false;
        private bool _canShoot = true;
        private float _shootTime = 0.5f;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _ball01 = SpawnBall(Utilities.GetRandomEnumFromToMax<ColorBall.ColorType>(1), _ballPosition01.position);
            _ball02 = SpawnBall(Utilities.GetRandomEnumFromToMax<ColorBall.ColorType>(1), _ballPosition02.position);
            _currentShootBall = _ball01;
            _nextShootBall = _ball02;
        }


        private Ball SpawnBall(ColorBall.ColorType colorType, Vector3 ballPosition)
        {
            Ball ball = Instantiate(_ballPrefab, ballPosition, Quaternion.identity, this.transform);
            ball.ColorBall.SetColor(colorType);
            return ball;
        }

        public void LookAt(Vector3 lookPosition, float rotationOffset)
        {
            if (_isAddedOffsetRotate)
                rotationOffset += 180f;

            Vector3 direction = lookPosition - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + rotationOffset;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        public void Shoot(Vector3 targetPosition, System.Action<Ball> OnBallReachTarget)
        {
            if(_canShoot)
            {
                _canShoot = false;
                Invoke(nameof(ResetCanShoot), _shootTime);

                Ball ballShooted = SpawnBall(_currentShootBall.ColorBall.BallColorType, GetShootPosition());
                StartCoroutine(MoveToTarget(ballShooted.transform, targetPosition, 0.25f, () =>
                {
                    OnBallReachTarget?.Invoke(ballShooted);
                    Destroy(ballShooted.gameObject);
                }));

                _currentShootBall.ColorBall.SetColor(_nextShootBall.ColorBall.BallColorType);
                _nextShootBall.ColorBall.SetColor(Utilities.GetRandomEnumFromToMax<ColorBall.ColorType>(1));
            }
        }

        private void ResetCanShoot()
        {
            _canShoot = true;
        }



        private void SwapBall()
        {
            if (_currentShootBall == _ball01)
            {
                _isAddedOffsetRotate = true;
                _currentShootBall = _ball02;
                _nextShootBall = _ball01;
            }          
            else
            {
                _isAddedOffsetRotate = false;
                _currentShootBall = _ball01;
                _nextShootBall = _ball02;
            }
                
        }

        private void OnMouseDown()
        {
            SwapBall();
            StartCoroutine(Rotate180Degrees());
        }


        private IEnumerator Rotate180Degrees()
        {
            float initialRotation = transform.eulerAngles.z;
            float targetRotation = initialRotation + 180f;
            float elapsedTime = 0f;

            while (elapsedTime < 1f)
            {
                float currentRotation = Mathf.Lerp(initialRotation, targetRotation, elapsedTime);
                transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);
                elapsedTime += Time.deltaTime * _rotationSpeed;
                yield return null;
            }

            // Ensure the object reaches exactly 180 degrees of rotation
            transform.rotation = Quaternion.Euler(0f, 0f, targetRotation);
        }

        #region Utilities
        private IEnumerator MoveToTarget(Transform objectToMove, Vector3 targetPosition, float moveDuration, System.Action OnFinished)
        {
            Vector3 startPosition = objectToMove.position;
            float elapsedTime = 0f;

            while (elapsedTime < moveDuration)
            {
                objectToMove.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            objectToMove.position = targetPosition;
            OnFinished?.Invoke();
        }

        public Vector3 GetShootPosition()
        {
            if(_ballPosition01.position.y > _ballPosition02.position.y)
                return _ballPosition01.position;
            else
                return _ballPosition02.position;
        }
        #endregion
    }
}