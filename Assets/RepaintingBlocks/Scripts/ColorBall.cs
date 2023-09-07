using UnityEngine;


namespace RepaintingBlocks
{
    public class ColorBall : MonoBehaviour
    {
        [System.Serializable]
        public enum ColorType
        {
            EMPTY,
            ORANGE,
            GREEN,
            WHITE,
            BLUE,
        }

        [System.Serializable]
        public struct BallColorData
        {
            public ColorType ColorType;
            public Color Color;
        }

        public BallColorData[] BallColors;
        public ColorType BallColorType { get; private set; }
        [SerializeField] private SpriteRenderer _sr;

  
        public void SetColor(ColorType colorType)
        {
            this.BallColorType = colorType;
            _sr.color = GetColorByType(BallColorType);
        }

        private Color GetColorByType(ColorType colorType)
        {
            for(int i = 0; i < BallColors.Length; i++)
            {
                if (BallColors[i].ColorType == colorType)
                {
                    return BallColors[i].Color;
                }
            }

            return Color.clear;
        }

    }
}