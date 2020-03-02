using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;

namespace ReactUnity.Styling
{
    public static class BorderGraphic
    {
        public static Dictionary<int, Sprite> SpriteCache = new Dictionary<int, Sprite>();


        static public Sprite CreateBorderSprite(int borderRadius)
        {
            if (SpriteCache.ContainsKey(borderRadius)) return SpriteCache[borderRadius];

            var svg = new Scene() { Root = new SceneNode() { Shapes = new List<Shape>() } };

            var totalSize = borderRadius * 2 + 2;

            var rad = Vector2.one * borderRadius;

            var contour = VectorUtils.BuildRectangleContour(new Rect(0, 0, totalSize, totalSize), rad, rad, rad, rad);
            var roundedRect = new Shape()
            {
                Contours = new BezierContour[] { contour },
                Fill = new SolidFill() { Color = Color.white, Opacity = 1 },
                PathProps = new PathProperties() { Corners = PathCorner.Round },
                IsConvex = true,
            };
            svg.Root.Shapes.Add(roundedRect);

            var geo = VectorUtils.TessellateScene(svg, new VectorUtils.TessellationOptions() { StepDistance = 1, SamplingStepSize = 1 });
            var sprite = VectorUtils.BuildSprite(geo, 1, VectorUtils.Alignment.Center, Vector2.one / 2, 100);

            var size = Mathf.CeilToInt(totalSize);
            var spriteRect = new Vector4(borderRadius, borderRadius, borderRadius, borderRadius);
            var mat = new Material(Shader.Find("Unlit/Vector"));
            var texture = VectorUtils.RenderSpriteToTexture2D(sprite, size, size, mat);
            var newSprite = Sprite.Create(texture, new Rect(0, 0, size, size), Vector2.one / 2, 1, 0, SpriteMeshType.FullRect, spriteRect);


            Object.DestroyImmediate(sprite);
            SpriteCache[borderRadius] = newSprite;
            return newSprite;
        }
    }
}