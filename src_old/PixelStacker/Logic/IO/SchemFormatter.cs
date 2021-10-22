using fNbt;
using System.Collections.Generic;
using System.IO;

namespace PixelStacker.Logic
{
    class SchemFormatter
    {
        public static void writeBlueprint(string filePath, BlueprintPA blueprint)
        {
            bool isv = Options.Get.IsSideView;

            #region metadata
            var metadata = new NbtCompound("Metadata");
            metadata.Add(new NbtString("Name", "NameOfSchematic"));
            metadata.Add(new NbtString("Author", "Taylor Love"));
            metadata.Add(new NbtString("Generator", "PixelStacker (" + Constants.Version + ")"));
            metadata.Add(new NbtString("Generator Website", Constants.Website));
            metadata.Add(new NbtList("RequiredMods", new List<NbtTag>(), NbtTagType.String));
            metadata.Add(new NbtInt("WEOriginX", 1));
            metadata.Add(new NbtInt("WEOriginY", 1));
            metadata.Add(new NbtInt("WEOriginZ", 1));

            if (blueprint.WorldEditOrigin != null)
            {
                metadata.Add(new NbtInt("WEOffsetX", -blueprint.WorldEditOrigin.X));

                if (isv)
                {
                    metadata.Add(new NbtInt("WEOffsetY", blueprint.WorldEditOrigin.Y - blueprint.Mapper.GetYLength(isv)));
                    metadata.Add(new NbtInt("WEOffsetZ", -blueprint.Mapper.GetZLength(isv))); //  Options.Get.IsMultiLayer ? -3 : -1
                }
                else
                {

                    metadata.Add(new NbtInt("WEOffsetY", 0));
                    metadata.Add(new NbtInt("WEOffsetZ", -blueprint.WorldEditOrigin.Y));
                }
            }

            #endregion

            var nbt = new NbtCompound("Schematic");
            // Missing "Offset" which defaults to [0,0,0]. Basically the world offset. Coordinates.
            nbt.Add(new NbtInt("Version", 1));
            nbt.Add(new NbtShort("Width", (short)blueprint.Mapper.GetXLength(isv)));
            nbt.Add(new NbtShort("Height", (short)blueprint.Mapper.GetYLength(isv)));
            nbt.Add(new NbtShort("Length", (short)blueprint.Mapper.GetZLength(isv)));
            nbt.Add(new NbtIntArray("Offset", new int[] { 0, 0, 0 }));
            nbt.Add(metadata);

            //PaletteMax integer Specifies the size of the block palette in number of bytes needed for the maximum palette index.Implementations may use this as a hint for the case that the palette data fits within a datatype smaller than a 32 - bit integer that they may allocate a smaller sized array.
            //Palette Palette Object  Specifies the block palette.This is a mapping of block states to indices which are local to this schematic.These indices are used to reference the block states from within the BlockData array.It is recommeneded for maximum data compression that your indices start at zero and skip no values.The maximum index cannot be greater than PaletteMax - 1.While not required it is highly recommended that you include a palette in order to tightly pack the block ids included in the data array.
            //BlockData   varint[]    Required.Specifies the main storage array which contains Width * Height * Length entries.Each entry is specified as a varint and refers to an index within the Palette.The entries are indexed by x + z * Width + y * Width * Length.
            //TileEntities    TileEntity Object[] Specifies additional data for blocks which require extra data.If no additional data is provided for a block which normally requires extra data then it is assumed that the TileEntity for the block is initialized to its default state.
            var palette = new Dictionary<string, int>();
            var tileEntities = new List<NbtCompound>();


            //Required.Specifies the main storage array which contains Width *Height * Length entries.Each entry is specified 
            //as a varint and refers to an index within the Palette.The entries are indexed by 
            //x +z * Width + y * Width * Length.
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter buffer = new BinaryWriter(ms))
                {
                    int xMax = blueprint.Mapper.GetXLength(isv);
                    int yMax = blueprint.Mapper.GetYLength(isv);
                    int zMax = blueprint.Mapper.GetZLength(isv);

                    for (int y = yMax-1; y >= 0; y--)
                    {
                        for (int z = 0; z < zMax; z++)
                        {
                            for (int x = 0; x < xMax; x++)
                            {
                                Material m = blueprint.Mapper.GetMaterialAt(isv, x, y, z);
                                
                                string key = m.GetBlockNameAndData(isv);
                                if (!palette.ContainsKey(key))
                                {
                                    palette.Add(key, palette.Count);
                                }

                                int blockId = palette[key];

                                while ((blockId & -128) != 0)
                                {
                                    buffer.Write((byte) (blockId & 127 | 128));
                                    blockId = (int) ((uint) blockId >> 7);
                                }
                                buffer.Write((byte) blockId);
                            }
                        }
                    }
                }

                // size of block palette in number of bytes needed for the maximum  palette index. Implementations may use
                // this as a hint for the case that the palette data fits within a datatype smaller than a 32 - bit integer
                // that they may allocate a smaller sized array.
                nbt.Add(new NbtInt("PaletteMax", palette.Count));
                var paletteTag = new NbtCompound("Palette");
                foreach(var kvp in palette)
                {
                    paletteTag.Add(new NbtInt(kvp.Key, kvp.Value));
                }
                nbt.Add(paletteTag);
                var blockData = ms.ToArray();
                nbt.Add(new NbtByteArray("BlockData", blockData));
            }

            nbt.Add(new NbtList("TileEntities", NbtTagType.End));

            var serverFile = new NbtFile(nbt);
            serverFile.SaveToFile(filePath, NbtCompression.GZip);
        }
    }
}
