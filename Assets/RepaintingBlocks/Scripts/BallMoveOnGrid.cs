using UnityEngine;
using System.Collections;

namespace RepaintingBlocks
{
    public class BallMoveOnGrid : MonoBehaviour
    {
        private Ball _ball;
        private IEnumerator _moveCoroutine;

        private void Awake()
        {
            _ball = GetComponent<Ball>();
        }

        public void Move(int newX, int newY, float moveTime)
        {
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
            }

            _moveCoroutine = PerformMove(newX, newY, moveTime);
            StartCoroutine(_moveCoroutine);
        }

        private IEnumerator PerformMove(int newX, int newY, float moveTime)
        {
            _ball.X = newX;
            _ball.Y = newY;

            Vector3 startPosition = transform.position;
            Vector3 endPosition = GridSystem.Instance.GridMap.GetWorldPosition(newX, newY);

            for (float t = 0; t < 1 * moveTime; t += Time.deltaTime)
            {
                _ball.transform.position = Vector3.Lerp(startPosition, endPosition, t);
                yield return 0;
            }

            _ball.transform.position = endPosition;
        }
    }
}