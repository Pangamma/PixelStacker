using fNbt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic
{
    class SchematicFormatter
    {
        public static void writeBlueprint(string filePath, BlueprintPA blueprint)
        {
            var nbt = new NbtCompound("Schematic");
            bool isv = Options.Get.IsSideView;
            nbt.Add(new NbtShort("Width", (short)blueprint.Mapper.GetXLength(isv)));
            nbt.Add(new NbtShort("Height", (short)blueprint.Mapper.GetYLength(isv)));
            nbt.Add(new NbtShort("Length", (short)blueprint.Mapper.GetZLength(isv)));
            if (Options.Get.IsSideView)
            {
                //-z = + y
                nbt.Add(new NbtString("Materials", "Alpha"));
                WriteBlockDataAndIDs(nbt, blueprint, Options.Get.IsSideView);
                nbt.Add(new NbtList("Entities", NbtTagType.End));
                nbt.Add(new NbtList("TileEntities", NbtTagType.End));
                if (blueprint.WorldEditOrigin != null)
                {
                    nbt.Add(new NbtInt("WEOriginX", 1));
                    nbt.Add(new NbtInt("WEOriginY", 1));
                    nbt.Add(new NbtInt("WEOriginZ", 1));
                    nbt.Add(new NbtInt("WEOffsetX", -blueprint.WorldEditOrigin.X));
                    nbt.Add(new NbtInt("WEOffsetY", blueprint.WorldEditOrigin.Y-blueprint.Height));
                    nbt.Add(new NbtInt("WEOffsetZ", Options.Get.IsMultiLayer ? -3 : -1));
                }
            }
            else
            {
                nbt.Add(new NbtString("Materials", "Alpha"));
                WriteBlockDataAndIDs(nbt, blueprint,Options.Get.IsSideView);
                nbt.Add(new NbtList("Entities", NbtTagType.End));
                nbt.Add(new NbtList("TileEntities", NbtTagType.End));
                if (blueprint.WorldEditOrigin != null)
                {
                    nbt.Add(new NbtInt("WEOriginX", 1));
                    nbt.Add(new NbtInt("WEOriginY", 1));
                    nbt.Add(new NbtInt("WEOriginZ", 1));
                    nbt.Add(new NbtInt("WEOffsetX", -blueprint.WorldEditOrigin.X));
                    nbt.Add(new NbtInt("WEOffsetY", 0));
                    nbt.Add(new NbtInt("WEOffsetZ", -blueprint.WorldEditOrigin.Y));
                }
            }

            var serverFile = new NbtFile(nbt);
            serverFile.SaveToFile(filePath, NbtCompression.GZip);
        }

        private static void WriteBlockDataAndIDs(NbtCompound nbt, BlueprintPA blueprint, bool isSideView)
        {
            Dictionary<string, short> schematicaDic = new Dictionary<string, short>();
            //int depth = Options.Get.IsMultiLayer ? 3 : 1;
            byte[] bs = new byte[blueprint.Mapper.GetXLength(isSideView) * blueprint.Mapper.GetYLength(isSideView) * blueprint.Mapper.GetZLength(isSideView)];
            byte[] bsD = new byte[blueprint.Mapper.GetXLength(isSideView) * blueprint.Mapper.GetYLength(isSideView) * blueprint.Mapper.GetZLength(isSideView)];
            int ti = 0;
            //-z = + y
            for (int y = blueprint.Mapper.GetYLength(isSideView) - 1; y >= 0; y--)
            {
                for (int z = 0; z < blueprint.Mapper.GetZLength(isSideView); z++)
                {
                    for (int x = 0; x < blueprint.Mapper.GetXLength(isSideView); x++)
                    {
                        Material ma = blueprint.Mapper.GetMaterialAt(isSideView, x, y, z);
                        schematicaDic[ma.SchematicaMaterialName ?? "minecraft:barrier"] = Convert.ToInt16(ma.BlockID);

                        bsD[ti] = Convert.ToByte(ma?.Data ?? 0);
                        bs[ti++] = Convert.ToByte(ma?.BlockID ?? 0);
                    }

                }
            }

            var schematicaIcon = new NbtCompound("Icon", new List<NbtTag>() {
                new NbtByte("Count", 1),
                new NbtShort("Damage", 0),
                new NbtString("id", "minecraft:white_wool")
            });

            nbt.Add(schematicaIcon);

            var schematicaMapping = new NbtCompound("SchematicaMapping");
            schematicaDic.ToList().ForEach(kvp => {
                schematicaMapping.Add(new NbtShort(kvp.Key, kvp.Value));
            });
            nbt.Add(schematicaMapping);

            nbt.Add(new NbtByteArray("Blocks", bs));
            nbt.Add(new NbtByteArray("Data", bsD));
        }
    }
}
