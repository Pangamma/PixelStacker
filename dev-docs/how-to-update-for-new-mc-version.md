# How to update for a new version of Minecraft
This guide covers how to check what has changed since the previous version and how to add the new materials into the pixelstacker system.

## Reference links
- [List of Java versions](https://minecraft.wiki/w/Java_Edition_version_history)
- [Changelog for 26.2](https://minecraft.wiki/w/Java_Edition_26.2)
- [Data Versions](https://minecraft.wiki/w/Data_version)


## Instructions
1) First gather information.
    1) First note the current version of PixelStacker. 
        - Check `GameVersionText` and `DataVersion` from this file: `src\PixelStacker.Logic\IO\Config\Constants.cs`. Version should match the version of Minecraft at the time PixelStacker was last updated. The letters at the end just represent hotfixes or patches for that same version. We start with letter `a` and progress to letter `z` as needed. `DataVersion` directly maps to the data version being used by the game. [See data versions](https://minecraft.wiki/w/Data_version)
        - The previous version of PixelStacker was `1.21.5`, data version `4325`.
    1) Then, note the version you want to migrate to. Check for the latest full version of Minecraft available. Currently the latest version is: `26.2`, and its data version is: `4903`. A newer version has snapshots `26.3`, but it hasn't been fully released yet so we will not update to that version yet.
    1) Summary of versions noted for the migration:

        | | GameVersionText | Data Version |
        |---|---|---|
        | Before | `1.21.5` | `4325` |
        | After | `26.2` | `4903` |

        **Goal:** Update PixelStacker to support the data introduced between data version `4325` and `4903`, adding any new blocks/materials.

2) Check the change logs between the versions to figure out which blocks need to be added or updated.
    1) Find the current version and future versions on either the [Data Versions](https://minecraft.wiki/w/Data_version) page or the [Java versions](https://minecraft.wiki/w/Java_Edition_version_history) page.
    2) Check the block additions and updates for each [change log](https://minecraft.wiki/w/Java_Edition_26.2) page starting with the pre-release version right after the current version and ending on the target version. (It might be possible to just compare between full versions and skip reading pre-release change logs though.)
    3) If a texture changes, the texture will need to be updated in our project. (More instructions below)
    4) If a new block is added, it will need to be added to our project if it is a solid cubic block. (More instructions below)

3) Download the latest full version of Minecraft.
    1) Download the latest version of Minecraft to your computer and let the JAR file update.
    1) Navigate to the downloaded minecraft jar file location. On windows it will be:  
        `%appdata%\Roaming\.minecraft\versions\{version}\{version}.jar`

4) Update the Constants file of PixelStacker to use the newer target version values.
    - `GameVersionText` => "26.2"
    - `DataVersion` => 4903
    - Do this before running the tools in steps 6 and 8 below — they build the path to the downloaded jar from `Constants.GameVersionText` (`%appdata%\.minecraft\versions\{GameVersionText}\{GameVersionText}.jar`).

5) Add the new version string to the `Material` class's version whitelist.
    - `Material.MinimumSupportedMinecraftVersion` validates against a hardcoded `ValidMinecraftVersions` array in `src\PixelStacker.Logic\Model\Material.cs`. Add the new `GameVersionText` value (e.g. `"26.2"`) to that array.
    - Skipping this makes `Materials.List` throw `ArgumentNullException` the first time it's accessed, since the ripper (step 6) stamps every new material with the current `GameVersionText`.

6) Find candidate new materials using the block ripper tool.
    1) Run the `ExtractFromZipFile` test in the `RipperMain` test class: `src\PixelStacker.CodeGenerator\BlockRipper\RipperMain.cs` (CodeGenerator project, "Tools" test category).
    2) It scrapes the jar's block models/blockstates for anything shaped like a solid cube and writes candidate `new Material(...)` lines to `src\PixelStacker.CodeGenerator\BlockRipper\ripped.txt`.
    3) This output is noisy. It includes blocks that already exist in PixelStacker but under a different top/side texture-state combo (they show up because that specific combo isn't registered yet, not because the block is new). Cross-reference against `src\PixelStacker.Logic\Model\Materials.cs` and only keep genuinely new blocks.
    4) Some blocks are deliberately excluded by design — see the "Rejected due to instable state" comment block near the top of `Materials.cs` (Creaking Heart, Respawn Anchor, Trial Spawner, Grass Block, leaves, etc.). Don't add these even if the ripper surfaces them.

7) For each genuinely new, solid cubic block:
    1) Its texture PNG(s) must exist in `src\PixelStacker.Resources\Images\Textures\x16\` before it can be referenced. `TestZipUpdate` (step 8) only refreshes textures that already exist on disk — it will not pull in brand new files. Extract them manually from the jar instead, e.g.:
        ```
        unzip -j "%appdata%\.minecraft\versions\{version}\{version}.jar" "assets/minecraft/textures/block/{name}.png" -d src\PixelStacker.Resources\Images\Textures\x16
        ```
        (No `.csproj` edit needed — `PixelStacker.Resources.csproj` already globs `Images\Textures\x16\*.png` as content.)
    2) Add a `new Material(...)` line to `Materials.cs`, following the existing category/ID conventions — e.g. base/polished/chiseled variants go under category `"Other"`, brick variants under `"Bricks"`; IDs are short uppercase abbreviations (`CHZL_*`, `POLISHED_*`, `*_BRICKS`).
    3) Skip anything that isn't a full 1×1×1 cube — stairs, slabs, walls, and other non-cubic shapes are out of scope for PixelStacker.

8) Automatically update *existing* textures with the `TestZipUpdate` test located in the `ImageTextureUpdater` test class of the CodeGenerator project. (Only touches files that already exist in the textures folder — see step 7 for brand new blocks.)

9) Regenerate the material palette so any newly added materials get color-combination data (used by the quantizer/renderer, and by the web app's `MaterialPaletteMap` endpoint).
    - Run `GenerateMaterialCombinationPalette()` in the `MaterialPaletteGenerator` test class — **`src\PixelStacker.CodeGenerator\MaterialPaletteGenerator.cs`** (category `"Generators"`). This is the same step documented in `.claude\commands\add-material.md` for adding a single material; see that file for the full field-by-field reference if you're hand-writing `Material(...)` entries.
    - This updates `src\PixelStacker.Resources\Files\materialPalette.json` and touches `Data.resx` so the updated JSON gets re-embedded on the next build.