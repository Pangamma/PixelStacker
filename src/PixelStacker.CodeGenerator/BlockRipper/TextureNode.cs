using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.CodeGenerator.BlockRipper
{
    public class TextureNode
    {
        public string Label { get; set; }

        /// <summary>
        /// "minecraft:dirt"
        /// </summary>
        public string BlockIdAndNamespace { get; set; }

        /// <summary>
        /// "dirt"
        /// </summary>
        public string BlockId => BlockIdAndNamespace?.Split(":").Last();

        /// <summary>
        /// "minecraft"
        /// </summary>
        public string Namespace => BlockIdAndNamespace?.Split(":").First();


        /// <summary>
        /// "minecraft:dirt"
        /// </summary>
        public string BlockIdAndNamespaceAndData => $"{BlockIdAndNamespace}{BlockStateData}";

        /// <summary>
        /// "" or "[facing=north]"
        /// </summary>
        public string BlockStateData { get; set; }

        /// <summary>
        /// minecraft:blocks/dirt
        /// </summary>
        public string TextureForUp { get; set; }

        /// <summary>
        /// dirt
        /// </summary>
        public string TextureForUp_ResourceName => TextureForUp.Split("/").Last();

        /// <summary>
        /// minecraft:blocks/dirt
        /// </summary>
        public string TextureForEast { get; set; }

        /// <summary>
        /// dirt
        /// </summary>
        public string TextureForEast_ResourceName => TextureForEast.Split("/").Last();

        public bool IsSameTextureOnAllSides { get; set; }

        public bool AlreadyInUseByPixelStacker { get; set; }

        public string GetMatchyMatchyString()
        {
            return BlockIdAndNamespaceAndData+"@"+BlockIdAndNamespaceAndData;
        }
        public override string ToString()
        {
            return $"Up={TextureForUp}, East={TextureForEast}, Id={BlockIdAndNamespace}{BlockStateData}";
        }
    }
}
