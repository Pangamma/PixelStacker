using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.CodeGenerator.BlockRipper
{
    public static class TextureResolver
    {
        /// <summary>
        /// Gets the resolved texture for a given side of a block, considering variant and rotation.
        /// </summary>
        /// <param name="blockState">The parsed blockstate object.</param>
        /// <param name="variant">The variant string (e.g., "axis=y").</param>
        /// <param name="side">The side to query (e.g., "up", "north").</param>
        /// <param name="modelProvider">A function to load BlockModel by model path.</param>
        /// <returns>The texture path used on that side, or null if not found.</returns>
        public static string GetTextureForSide(
            BlockStateDefinition blockState,
            ModelDefinition modelDef,
            Variant variant,
            string side,
            Func<string, ModelDefinition> modelProvider)
        {
            //var modelDef = blockState.GetAllPossibleModels();
            
            //GetModelDefinitionForVariant(variant);
            //if (modelDef == null)
                //return null;

            // Start with model path and rotation from definition
            string modelPath = variant.Model;
            int xRot = variant.X ?? 0;
            int yRot = variant.Y ?? 0;

            var model = modelProvider(modelPath);
            if (model == null)
                return null;

            // Reverse-rotate the requested side to match stored side in model
            string actualSide = ApplyRotationToSide(side, -xRot, -yRot);

            // Assume the first element represents the textures (simplification)
            var element = model.Elements?.FirstOrDefault();
            if (element == null || true != element.Faces?.TryGetValue(actualSide, out var face))
                return null;

            // Resolve texture variable like "#side" using the model's texture map
            return ResolveTexture(face.Texture, model);
        }

        private static string ResolveTexture(string textureRef, ModelDefinition model)
        {
            if (!textureRef.StartsWith("#"))
                return textureRef; // Already a resolved path

            string key = textureRef.Substring(1);
            if (model.Textures != null && model.Textures.TryGetValue(key, out var resolved))
            {
                if (!resolved.StartsWith("#"))
                {
                    return resolved;
                }

                return ResolveTexture(resolved, model);
            }

            return null;
        }

        private static string ApplyRotationToSide(string side, int xDegrees, int yDegrees)
        {
            // Normalize degrees to [0, 360)
            xDegrees = ((xDegrees % 360) + 360) % 360;
            yDegrees = ((yDegrees % 360) + 360) % 360;

            // Y-axis rotation (rotating around up-down axis)
            if (yDegrees != 0)
            {
                side = RotateY(side, yDegrees);
            }

            // X-axis rotation (tilting north/south up or down)
            if (xDegrees != 0)
            {
                side = RotateX(side, xDegrees);
            }

            return side;
        }

        private static string RotateY(string side, int degrees)
        {
            var map = new Dictionary<string, string>
        {
            {"north", "east"}, {"east", "south"}, {"south", "west"}, {"west", "north"}
        };

            for (int i = 0; i < degrees; i += 90)
            {
                if (map.ContainsKey(side))
                    side = map[side];
            }
            return side;
        }

        private static string RotateX(string side, int degrees)
        {
            var x90 = new Dictionary<string, string>
        {
            {"up", "north"}, {"north", "down"}, {"down", "south"}, {"south", "up"}
        };

            for (int i = 0; i < degrees; i += 90)
            {
                if (x90.ContainsKey(side))
                    side = x90[side];
            }
            return side;
        }

        public static bool IsSameTextureOnAllSides(BlockStateDefinition blockState, ModelDefinition modelDef, Variant variant, Func<object, ModelDefinition> modelProvider)
        {
            string[] directions = new string[] { "up", "north", "down", "south", "east", "west"};
            return directions.Select(dir => GetTextureForSide(blockState, modelDef, variant, dir, modelProvider)).Distinct().Count() == 1;
        }
    }

}
