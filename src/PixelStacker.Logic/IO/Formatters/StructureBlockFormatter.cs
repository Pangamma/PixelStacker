using FNBT;
using FNBT.Tags;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic.IO.Formatters
{
    /// <summary>
    /// Generated with ChatGPT 4o, based on this spec:
    /// https://minecraft.wiki/w/Structure_file
    /// </summary>
    public class StructureBlockFormatter : PixelStacker.Logic.IO.Formatters.IExportFormatter
    {
        public static byte[] StructureToNBT(Schem2Details details, bool isMultilayer, bool isv)
        {
            var nbt = new NbtCompound("Structure");

            int xMax = details.WidthX;
            int yMax = details.HeightY;
            int zMax = details.LengthZ;

            // Palette creation
            var palette = new Dictionary<string, int>();
            var paletteList = new List<NbtCompound>();
            var blockList = new List<NbtCompound>();

            var region = details.RegionXYZ;
            for (int y = 0; y < yMax; y++)
            {
                for (int z = 0; z < zMax; z++)
                {
                    for (int x = 0; x < xMax; x++)
                    {
                        var mat = region[x][y][z];
                        string blockKey = mat.GetBlockNameAndData(isv);

                        if (!palette.ContainsKey(blockKey))
                        {

                            palette[blockKey] = paletteList.Count;
                            //var stateTag = new NbtCompound();
                            //stateTag.Add(new NbtString("Name", blockKey));
                            //stateTag.Add(new NbtCompound("Properties")); // Optionally fill with real data



                            string blockName = blockKey;
                            var properties = new Dictionary<string, string>();

                            // Split blockKey like "minecraft:stripped_oak_wood[axis=y]"
                            int bracketIndex = blockKey.IndexOf('[');
                            if (bracketIndex > 0)
                            {
                                blockName = blockKey.Substring(0, bracketIndex);
                                string propsRaw = blockKey.Substring(bracketIndex + 1, blockKey.Length - bracketIndex - 2);
                                var keyValuePairs = propsRaw.Split(',');
                                foreach (var pair in keyValuePairs)
                                {
                                    var kv = pair.Split('=');
                                    if (kv.Length == 2)
                                    {
                                        properties[kv[0]] = kv[1];
                                    }
                                }
                            }

                            var stateTag = new NbtCompound();
                            stateTag.Add(new NbtString("Name", blockName));

                            var propsTag = new NbtCompound("Properties");
                            foreach (var kv in properties)
                            {
                                propsTag.Add(new NbtString(kv.Key, kv.Value));
                            }
                            stateTag.Add(propsTag);


                            paletteList.Add(stateTag);
                        }

                        int stateIndex = palette[blockKey];

                        var blockTag = new NbtCompound();
                        blockTag.Add(new NbtList("pos", new int[] { x, y, z }.Select(x => new NbtInt(x))));
                        blockTag.Add(new NbtInt("state", stateIndex));
                        blockList.Add(blockTag);
                    }
                }
            }

            nbt.Add(new NbtList("palette", paletteList, NbtTagType.Compound));
            //nbt.Add(new NbtInt("paletteMax", paletteList.Count));
            nbt.Add(new NbtList("blocks", blockList, NbtTagType.Compound));
            nbt.Add(new NbtList("entities", NbtTagType.Compound)); // Empty list
            nbt.Add(new NbtList("size", new int[] { xMax, yMax, zMax }.Select(x => new NbtInt(x))));
            nbt.Add(new NbtInt("DataVersion", Constants.DataVersion));

            var file = new NbtFile(nbt);
            using var msOut = new MemoryStream();
            file.SaveToStream(msOut, NbtCompression.GZip);
            return msOut.ToArray();
        }

        public Task<byte[]> ExportAsync(PixelStackerProjectData canvas, CancellationToken? worker = null)
        {
            bool isv = canvas.IsSideView;
            bool isMultiLayer = canvas.CanvasData.Any(x => canvas.MaterialPalette[x.PaletteID].IsMultiLayer);
            Schem2Details details = IExportFormatter.ConvertCanvasToDetails(canvas);
            byte[] data = StructureToNBT(details, isMultiLayer, isv);
            return Task.FromResult(data);
        }
    }
}
