using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.CodeGenerator.BlockRipper
{
    using System.Collections.Generic;

    public class ModelDefinition
    {
        private string _parent;
        public string Parent { get => _parent; 
            set { 
                if (value != null && !value.Contains(":"))
                {
                    _parent = "minecraft:" + value;
                } else
                {
                    _parent = value;
                }
            }
        }

        public bool? AmbientOcclusion { get; set; }
        public Dictionary<string, DisplayPosition> Display { get; set; }
        public Dictionary<string, string> Textures { get; set; }
        public List<Element> Elements { get; set; }


        /// <summary>
        /// Use for figuring out the textures of each model.
        /// </summary>
        public static ModelDefinition GetEffectiveModel(string modelFile, Dictionary<string, ModelDefinition> allModels)
        {
            if (!allModels.TryGetValue(modelFile, out var current)){
                return null;
            }

            var parent = current.Parent != null ? GetEffectiveModel(current.Parent, allModels) : null;

            var effective = new ModelDefinition()
            {
                AmbientOcclusion = current?.AmbientOcclusion ?? parent?.AmbientOcclusion,
                Display = null, // I do not care about this.
                Parent = current.Parent,
                Textures = MergeTextures(current?.Textures, parent?.Textures),

                // https://minecraft.wiki/w/Model
                // If both "parent" and "elements" are set, the "elements" tag overrides the "elements" tag from the previous model.
                Elements = current?.Elements ?? parent?.Elements
                
            };

            return effective;
        }

        private static Dictionary<string, string> MergeTextures(Dictionary<string, string> current, Dictionary<string, string> parent)
        {
            Dictionary<string, string> merged = new Dictionary<string, string>();
            if (parent != null)
            {
                foreach (var kvp in parent)
                {
                    merged[kvp.Key] = kvp.Value;
                }
            }
            if (current != null)
            {
                foreach (var kvp in current)
                {
                    merged[kvp.Key] = kvp.Value;
                }
            }
            return merged;
        }
    }

    public class DisplayPosition
    {
        public List<float> Rotation { get; set; }
        public List<float> Translation { get; set; }
        public List<float> Scale { get; set; }
    }

    public class Element
    {
        public List<float> From { get; set; }
        public List<float> To { get; set; }
        public Rotation Rotation { get; set; }
        public bool? Shade { get; set; }
        public int? LightEmission { get; set; }
        public Dictionary<string, Face> Faces { get; set; }

        [Obsolete("Elements is not merged, just overridden.")]
        public static List<Element> MergeElements(List<Element> current, List<Element> parent)
        {
            List<Element> output = new List<Element>();
            int max = Math.Max(current.Count, parent.Count);
            for (int i = 0; i < max; i++)
            {
                var currentElement = current.Count < i ? current[i] : null;
                var parentElement = parent.Count < i ? parent[i] : null;
                Element toReturn = new Element()
                {
                    Shade = currentElement?.Shade ?? parentElement?.Shade,
                    From = currentElement?.From ?? parentElement?.From,
                    To = currentElement?.To ?? parentElement?.To,
                    Rotation = currentElement?.Rotation ?? parentElement?.Rotation,
                    LightEmission = currentElement?.LightEmission ?? parentElement?.LightEmission,

                    // NOTE: We may need to handle the merging of each individual face here.
                    Faces = currentElement?.Faces ?? parentElement?.Faces
                };

                output.Add(toReturn);
            }

            return output;
        }

    }

    public class Rotation
    {
        public List<float> Origin { get; set; }
        public string Axis { get; set; }
        public float? Angle { get; set; }
        public bool? Rescale { get; set; }
    }

    public class Face
    {
        public List<float> UV { get; set; }
        public string Texture { get; set; }
        public string CullFace { get; set; }
        public int? Rotation { get; set; }
        public int? TintIndex { get; set; }
    }
}
