using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PixelStacker.Logic.Model;
using PixelStacker.Resources;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;


/**
 * assets / [namespace] / 
 * 
 * BLOCK_STATE --> MODEL --> TEXTURE
 * 
 * 
 * 
 */
namespace PixelStacker.CodeGenerator.BlockRipper
{
    /// <summary>
    /// This test class will automamtically update Minecraft textures for you based on the already existing jar file
    /// for Minecraft.
    /// </summary>
    [TestClass]
    [TestCategory("Tools")]
    public class RipperMain
    {
        private string RootDir = AppDomain.CurrentDomain.BaseDirectory.Split(new string[] { "\\PixelStacker.CodeGenerator\\bin\\" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        private string McVersion = "1.21.5";
        private string PxImageDir => Path.Combine(RootDir, "PixelStacker.Resources", "Images", "Textures", "x16");
        private string McImageJar => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            ".minecraft", "versions", McVersion, McVersion + ".jar");

        private static readonly System.Text.Json.JsonSerializerOptions JSON_OPTIONS = new System.Text.Json.JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        /**
         * For a block located at \assets\minecraft\models\block\foo.json,  the reference for this would be... 
         * minecraft:[[models]]/block
         */

        [TestMethod]
        public void ExtractFromZipFile()
        {
            var dstDir = new DirectoryInfo(PxImageDir);

            #region Gather raw json files.
            // Gather raw json files.
            Dictionary<string, string> nameToPaths = new Dictionary<string, string>();
            Dictionary<string, ModelDefinition> allBlockModels = ScrapeBlockModelFiles();
            Dictionary<string, BlockStateDefinition> blockStates = ScrapeBlockStateFiles();
            Dictionary<string, string> localizationTexts = ScrapeLocalizationTexts();
            #endregion Gather raw json files.

            #region Filter json files
            // Filter json files.
            Dictionary<string, ModelDefinition> cubicBlockModels = allBlockModels.Where(kvp => true == IsCubeShaped(kvp.Key, allBlockModels))
                 .Where(kvp => DoesModelInheritFromAny(kvp.Key, new string[] {
                "minecraft:block/block",
                "minecraft:block/cube_all",
                "minecraft:block/cube",
                "minecraft:block/cube_column_horizontal",
                "minecraft:block/cube_column"
                 }, allBlockModels)).ToDictionary(x => x.Key, x => x.Value);

            // Mushroom blocks for some reason.
            Dictionary<string, ModelDefinition> cubicBlockModels_MultiPart = allBlockModels
                 .Where(kvp => true == IsCubeShaped_If_Multipart(kvp.Key, allBlockModels))
                 .Where(kvp => DoesModelInheritFromAny(kvp.Key, new string[] { "minecraft:block/template_single_face" }, allBlockModels))
                .ToDictionary(x => x.Key, x => x.Value);

            // A bunch of things we don't need or care about. Useful for debugging.
            Dictionary<string, ModelDefinition> remainingBlockModels = allBlockModels
                 .Where(kvp => true == IsCubeShaped(kvp.Key, allBlockModels))
                 .Except(cubicBlockModels.ToList()).Except(cubicBlockModels_MultiPart.ToList())
                .ToDictionary(x => x.Key, x => x.Value);

            var blockModels = cubicBlockModels.Concat(cubicBlockModels_MultiPart).ToDictionary(x => x.Key, x => x.Value);
            blockStates = blockStates.Where(bs => bs.Value.GetAllPossibleModels().Any(m => blockModels.ContainsKey(m)))
                .ToDictionary(x => x.Key, x => x.Value);
            #endregion Filter json files



            //needle = blockStates.Where(x => x.Key.Contains("birch_log")).ToList();

            // 1. Anything that inherits from cube_all will not change based on the orientation of the art.
            // 2. Anything inheriting from cube_column or cube_column_horizontal will use [axis=x],
            //    or [axis=y] and [axis=z] if you want to look at the top of the texture.

            #region Resolve texture mappings.
            List<string> blockIds = new List<string>();
            var textureNodes = new List<TextureNode>();
            foreach (var blockStateKvp in blockStates)
            {
                //Debug.WriteLine(blockStateKvp.Key);
                BlockStateDefinition blockState = blockStateKvp.Value;
                string blockId = blockStateKvp.Key.Split(":").Last();
                string blockIdAndNamespace = blockStateKvp.Key;

                // TODO: REMOVE
                //if (!blockId.Contains("acacia_log") && !blockId.Contains("acacia_wood") && !blockId.Contains("acacia"))
                //{
                //    continue;
                //}

                if (blockState.Variants != null && blockState.Multipart != null)
                {
                    // Invalid??
                    continue;
                }


                if (blockState.Variants != null)
                {
                    // Not every variant is worth using.
                    foreach (var variantKvp in blockState.Variants)
                    {
                        // Any time a model is randomly selected, it is just the same model file but
                        // rotated for the sake of of randomizing textures. Just pick the first in the list.
                        var variant = variantKvp.Value.Values.First();

                        string modelFile = variant.Model;
                        string materialAndData = (variantKvp.Key == "*") ? blockStateKvp.Key : $"{blockStateKvp.Key}[{variantKvp.Key}]";


                        // If we get a double half slab, only one of the variants will be in our list of models. It would be the double slab variant.
                        // If it ain't in here, it ain't worth using.
                        if (!blockModels.TryGetValue(modelFile, out ModelDefinition modelDef))
                        {
                            continue;
                        }

                        if (modelDef?.Textures == null)
                        {
                            // This so far only happens for the crafter, and honestly I just don't care enough about that one block to make a proper fix.
                            // The proper fix would be to search ancestor models until you get to a model with texture data, and then use that texture data.
                            //Debug.WriteLine($"Textures was null for {materialAndData}");
                            continue;
                        }


                        blockIds.Add(blockId);
                        string texturePath = modelDef.Textures.Values.First();
                        string textureName = texturePath.Split("/").Last();
                        string localizedName = localizationTexts[$"block.minecraft.{blockId}"] ?? blockStateKvp.Key.Split(":").Last();
                        var effectiveModel = ModelDefinition.GetEffectiveModel(modelFile, allBlockModels);


                        if (effectiveModel.Elements.Count != 1)
                        {
                            Debug.WriteLine("Skipping multi element model: "+modelFile);
                            continue;
                        }

                        var element = effectiveModel.Elements.First();

                        TextureNode node = new TextureNode();
                        node.Label = localizedName;
                        node.BlockIdAndNamespace = blockIdAndNamespace;
                        node.BlockStateData = variantKvp.Key == "*" ? "" : $"[{variantKvp.Key}]";
                        node.TextureForUp = TextureResolver.GetTextureForSide(blockState, modelDef, variant, "up", (modelKey) => ModelDefinition.GetEffectiveModel(modelFile, allBlockModels));
                        node.TextureForEast = TextureResolver.GetTextureForSide(blockState, modelDef, variant, "east", (modelKey) => ModelDefinition.GetEffectiveModel(modelFile, allBlockModels));
                        node.IsSameTextureOnAllSides = TextureResolver.IsSameTextureOnAllSides(blockState, modelDef, variant, (modelKey) => ModelDefinition.GetEffectiveModel(modelFile, allBlockModels));
                        textureNodes.Add(node);
                    }
                }
                else if (blockState.Multipart != null)
                {
                    TextureNode node = new TextureNode();
                    node.BlockIdAndNamespace = blockIdAndNamespace;
                    foreach (var multipart in blockState.Multipart)
                    {
                        Debug.WriteLine($"\t- Has multipart = true, ({multipart.Apply.Values.Count()})");
                        //multipart.When?.ToList().ForEach(c => Debug.WriteLine($"\t-- {c.Key} = {c.Value}"));
                        multipart.Apply?.Values.ToList().ForEach(v => Debug.WriteLine($"\t-- {v.Model}"));
                    }
                }
            }
            #endregion Resolve texture mappings.

            string[] bannedWords = new string[] { "suspicious", "crafter", "jigsaw", "structure_block", "infested", "command_block" };
            textureNodes = textureNodes.Where(x => !bannedWords.Any(bannedWord => x.BlockId.Contains(bannedWord))).ToList();

            //{
            //    var nodesToPrint = textureNodes.Where(x => x.IsSameTextureOnAllSides).ToList();
                
            //}
            
            
            var alreadyInUse = Materials.List.Select(m => { return m.GetBlockNameAndData(false) + "@" + m.GetBlockNameAndData(true); }).Distinct().ToList();
            textureNodes.ForEach(texture => {
                string needle = $"{texture.BlockIdAndNamespaceAndData}@{texture.BlockIdAndNamespaceAndData}";
                texture.AlreadyInUseByPixelStacker = alreadyInUse.Contains(needle);
            });

            //textureNodes = textureNodes.Where(x => x.IsSameTextureOnAllSides).ToList();
            //textureNodes = textureNodes.Where(x => !x.AlreadyInUseByPixelStacker).ToList();
             var upTextures = textureNodes.GroupBy(node => node.TextureForUp).ToArray();
            //upTextures = upTextures.Where(g =>  g.Any(t => alreadyInUse.Contains(t.GetMatchyMatchyString()))).ToArray();

            var sideTextures = textureNodes.GroupBy(node => node.TextureForEast).ToArray();

            int countMissing = 0;
            string output = "";
            List<string> outputLines = new List<string>();
            // Match textures with blocks in a reasonable way.
            foreach (var sideTextureGroup in sideTextures)
            {
                var sideTexturesToUse = sideTextureGroup
                    .OrderByDescending(x => x.AlreadyInUseByPixelStacker)
                    .ThenByDescending(x => x.IsSameTextureOnAllSides)
                    .ThenByDescending(x => x.BlockId.Contains("waxed"))
                    .ThenBy(x => x.BlockStateData.Length).ToList();

                // When you can find the same texture on both top and bottom, with the same block ID data.
                var sideTexturesThatHaveSameTextureOnAllSides = sideTexturesToUse.Where(x => x.TextureForEast == x.TextureForUp).ToList();
                if (sideTexturesThatHaveSameTextureOnAllSides.Count > 0)
                {
                    var sideNode = sideTexturesToUse.FirstOrDefault();
                    string line = @$"new Material(""{McVersion}"", false, ""Other"", ""PIXELSTACKER_ID"", ""{sideNode.Label}"", Textures.GetBitmap(""{sideNode.TextureForUp_ResourceName}""), Textures.GetBitmap(""{ sideNode.TextureForEast_ResourceName}""), $""{sideNode.BlockIdAndNamespaceAndData}"", $""{sideNode.BlockIdAndNamespaceAndData}"", """"),";
                    output += line + "\n";
                    outputLines.Add(line);
                    Debug.WriteLine(line);
                    //Debug.WriteLine($"No joins needed for {node.BlockIdAndNamespaceAndData}");
                    continue;
                }

                // When you can find the texture on both top and bottom, but with different block ID data.
                var upTextureGroup = upTextures.FirstOrDefault(g => g.Key == sideTextureGroup.Key);
                if (upTextureGroup != null)
                {
                    if (upTextureGroup.Count() > 1)
                    {
                        // This happens only twice. To barrels, and a crafter.
                        // TODO: Figure out how to edit this.
                        //Debug.WriteLine(sideTexturesToUse.FirstOrDefault().Label);
                        continue;
                    }

                    var sideNode = sideTexturesToUse.FirstOrDefault();
                    var topNode = upTextureGroup.FirstOrDefault();
                    string line = @$"new Material(""{McVersion}"", false, ""Other"", ""PIXELSTACKER_ID"", ""{sideNode.Label}"", Textures.GetBitmap(""{topNode.TextureForUp_ResourceName}""), Textures.GetBitmap(""{sideNode.TextureForEast_ResourceName}""), $""{topNode.BlockIdAndNamespaceAndData}"", $""{sideNode.BlockIdAndNamespaceAndData}"", """"),";
                    output += line + "\n";
                    outputLines.Add(line);
                    Debug.WriteLine(line);

                    continue;
                }

                // When you have to give up and accept that you're probably working with a furnace block.
                //countMissing++;
                //Debug.WriteLine($"No side texture exists for {sideTextureGroup.Key}");

                {
                    var sideNode = sideTexturesToUse.FirstOrDefault();
                    string line = @$"new Material(""{McVersion}"", false, ""Other"", ""PIXELSTACKER_ID"", ""{sideNode.Label}"", Textures.GetBitmap(""{sideNode.TextureForUp_ResourceName}""), Textures.GetBitmap(""{sideNode.TextureForEast_ResourceName}""), $""{sideNode.BlockIdAndNamespaceAndData}"", $""{sideNode.BlockIdAndNamespaceAndData}"", """"),";
                    output += line + "\n";
                    outputLines.Add(line);
                    Debug.WriteLine(line);
                }
                // TODO: Match by block ID instead. There are 61 things left.
            }

            output = string.Join("\n", outputLines.OrderBy(x => x));
            File.WriteAllText(Path.Join(RootDir, "PixelStacker.CodeGenerator", "BlockRipper", "ripped.txt"), output);
            string FU = JsonConvert.SerializeObject((blockIds.Distinct().OrderBy(x => x).ToArray()), Formatting.Indented);
        }

        private static bool DoesModelInheritFromAny(string currentModelKey, string[] ancestorModelKey, Dictionary<string, ModelDefinition> blockModels)
        {
            if (!blockModels.TryGetValue(currentModelKey, out ModelDefinition currentModel))
            {
                return false;
            }

            if (ancestorModelKey.Any(a => a == currentModel.Parent))
            {
                return true;
            }

            if (currentModel.Parent == null)
            {
                return false;
            }

            return DoesModelInheritFromAny(currentModel.Parent, ancestorModelKey, blockModels);
        }

        private static Element GetEffectiveElement(string currentModelKey, Dictionary<string, ModelDefinition> blockModels)
        {
            if (!blockModels.TryGetValue(currentModelKey, out ModelDefinition currentModel))
            {
                return null; // false
            }

            if (currentModel.Elements != null && currentModel.Elements.Count > 1)
            {
                //Debug.WriteLine("Skipping block model with more than one element. " +currentModelKey);
                return null;
            }

            Element currentElement = currentModel.Elements?.FirstOrDefault();
            Element parentElement = currentModel.Parent != null ? GetEffectiveElement(currentModel.Parent, blockModels) : null;

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

            return toReturn;
        }

        private static bool? IsCubeShaped(string currentModelKey, Dictionary<string, ModelDefinition> blockModels)
        {
            Element element = GetEffectiveElement(currentModelKey, blockModels);
            if (element == null || element.To == null || element.From == null)
            {
                return null;
            }

            if (currentModelKey.Contains("slab"))
            {

            }
            float a = Math.Abs(element.From[0] - element.To[0]);
            float b = Math.Abs(element.From[1] - element.To[1]);
            float c = Math.Abs(element.From[2] - element.To[2]);
            bool isCube = (a == 16 && b == 16 && c == 16);
            if (!isCube)
            {

            }
            return isCube;
        }

        private static bool?  IsCubeShaped_If_Multipart(string currentModelKey, Dictionary<string, ModelDefinition> blockModels)
        {
            Element element = GetEffectiveElement(currentModelKey, blockModels);
            if (element == null || element.To == null || element.From == null)
            {
                return null;
            }
            float a = Math.Abs(element.From[0] - element.To[0]);
            float b = Math.Abs(element.From[1] - element.To[1]);
            float c = Math.Abs(element.From[2] - element.To[2]);
            bool isCube = (a == 16 && b == 16 && c == 0);
            return isCube;
        }

        /**
         * /assets/{namespace}/block/somewhere/foo.json
         * {namespace}:block/somewhere/foo
         */
        public static string FromZipPathToJsonFilePath(string fileFullName)
        {
            StringBuilder sb = new StringBuilder();
            string[] parts = fileFullName.Split('/', StringSplitOptions.RemoveEmptyEntries);
            sb.Append(parts[1]);    // namespace
            sb.Append(":");         // separator
            // Skip assets/{namespace}/{model|blockstate|textures}
            sb.Append(string.Join('/', parts.Skip(3)).Replace(".json", "").Replace(".png", ""));
            return sb.ToString();
        }

        /**
         * /assets/{namespace}/block/somewhere/foo.json
         * {namespace}:block/somewhere/foo
         */
        public static string ToZipFolderPath(string resourcePath, string subfolder)
        {
            switch (subfolder)
            {
                case "models":
                    return $"assets/{resourcePath.Split(':')[0]}/models/{resourcePath.Split(':')[1]}.json";
                case "blockstates":
                    return $"assets/{resourcePath.Split(':')[0]}/blockstates/{resourcePath.Split(':')[1]}.json";
                case "textures":
                    return $"assets/{resourcePath.Split(':')[0]}/textures/{resourcePath.Split(':')[1]}.png";
                default:
                    throw new ArgumentException("Available options: models, blockstates, textures.", "subfolder");

            }
        }

        private Dictionary<string, string> ScrapeLocalizationTexts()
        {
            Dictionary<string, string> parsed = new Dictionary<string, string>();
            GetJarFiles(McImageJar, (f) => { 
                if (f.Name == "en_us.json")
                {
                    using (var zipStream = f.Open())
                    {
                        using (var ms = new MemoryStream())
                        {
                            zipStream.CopyTo(ms); // here
                            ms.Position = 0;
                            var arr = ms.ToArray();
                            string text = Encoding.UTF8.GetString(arr);
                            parsed = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(text, JSON_OPTIONS);
                        }
                    }
                }
            });
            return parsed;
        }

        /// <summary>
        /// Scrapes ALL block state files. The key will be the resource key, and the value will be the block state definition.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, BlockStateDefinition> ScrapeBlockStateFiles()
        {
            Dictionary<string, BlockStateDefinition> blockStates = new Dictionary<string, BlockStateDefinition>();
            GetJarFiles(McImageJar, (f) =>
            {
                string[] pathParts = f.FullName.Split('/', StringSplitOptions.None);

                if (pathParts.Length < 4 || pathParts[2] != "blockstates" || !f.Name.EndsWith(".json"))
                {
                    return;
                }

                string resourceFilePath = RipperMain.FromZipPathToJsonFilePath(f.FullName);
                //filePathFromZipPathToJsonFilePath[f.FullName] = resourceFilePath;
                using (var zipStream = f.Open())
                {
                    using (var ms = new MemoryStream())
                    {
                        zipStream.CopyTo(ms); // here
                        ms.Position = 0;
                        var arr = ms.ToArray();
                        string text = Encoding.UTF8.GetString(arr);
                        var parsed = System.Text.Json.JsonSerializer.Deserialize<BlockStateDefinition>(text, JSON_OPTIONS);
                        blockStates[resourceFilePath] = parsed;
                    }
                }
            });

            return blockStates;
        }

        /// <summary>
        /// Only returns models which extend from the cube_all model.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, ModelDefinition> ScrapeBlockModelFiles () {
            
            Dictionary<string, ModelDefinition> blockModels = new Dictionary<string, ModelDefinition>();
            GetJarFiles(McImageJar, (f) =>
            {
                string[] pathParts = f.FullName.Split('/', StringSplitOptions.None);

                if (pathParts.Length < 4 || pathParts[2] != "models" || pathParts[3] != "block" ||!f.Name.EndsWith(".json"))
                {
                    return;
                }

                string resourceFilePath = RipperMain.FromZipPathToJsonFilePath(f.FullName);
                //filePathFromZipPathToJsonFilePath[f.FullName] = resourceFilePath;
                using (var zipStream = f.Open())
                {
                    using (var ms = new MemoryStream())
                    {
                        zipStream.CopyTo(ms); // here
                        ms.Position = 0;
                        var arr = ms.ToArray();
                        string text = Encoding.UTF8.GetString(arr);
                        var parsed = System.Text.Json.JsonSerializer.Deserialize<ModelDefinition>(text, JSON_OPTIONS);
                        blockModels[resourceFilePath] = parsed;
                    }
                }
            });

            return blockModels;
        }

        public void GetJarFiles(string filePath, Action<ZipArchiveEntry> actn)
        {
            if (!File.Exists(filePath)) return;
            string entryPath = "";

            try
            {
                using (FileStream zipToOpen = new FileStream(filePath, FileMode.Open))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
                    {
                        var entries = archive.Entries;
                        foreach (var entry in entries)
                        {
                            entryPath = entry.FullName;
                            //if (!entry.Name.EndsWith(".png")) continue;
                            //if (!entry.FullName.Contains("textures")) continue;
                            if (!entry.FullName.Contains("assets")) continue;
                            actn.Invoke(entry);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(entryPath + " has a problem.");
                Debug.WriteLine(ex);
            }

            return;
        }
    }
}
