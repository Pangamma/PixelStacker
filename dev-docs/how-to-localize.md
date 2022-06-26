# PixelStacker localization guide
This guide covers how to add, manage, and modify localization entries for PixelStacker so that the program can more comfortably reach a greater audience. All strings showing in the PixelStacker UI should be localized. 

**Here are some quick links:**
* [Environment Setup](#setup)
* [Adding entries](#adding)
* [Modifying entries](#modifying)
* [Using entries](#using)


## Setting up your developer environment for localization
Before you can begin localizing, you will want to install the Google API key for this.
1. Open up your user secrets file for the code generator test project. ![user secrets](https://user-images.githubusercontent.com/1046026/175830673-9e576365-cbe5-4c32-8069-5e8fad878efa.png)
2. You will want to add your Google API key here. ![secrets file](https://user-images.githubusercontent.com/1046026/175830720-3709d2ed-e0b8-4ce5-9337-00859f72329e.png)

3. You can get an API key here: https://console.cloud.google.com/apis/. You will want to get an API key that can access the ```Cloud Translation API```.


## Adding a new localized text entry
If you want to **modify** an existing key, check out the [modifying](#modifying) instructions.
1. Open up PixelStacker.Resources.Text.resx in Visual Studio.
2. Add your new entry into the file and give it a value. Be sure to use good namespacing so the overall file is maintainable.
![RESX editor](https://user-images.githubusercontent.com/1046026/175829324-c1f2510a-4bbe-410c-acab-60e4483d338e.png)
3. Build the PixelStacker.Resources project. (Right click in solution explorer and find the build button)
4. In your test explorer (open it up) run the ```Generators >> Text_Translate``` test.
![text explorer example](https://user-images.githubusercontent.com/1046026/175829652-6e02ed35-bbeb-493b-b27e-bcf0cd4cd50b.png)
5. This "test" will generate the localization entries in all of the localized json files, and will automatically localize the content into the other localization json files. 
![generated difference](https://user-images.githubusercontent.com/1046026/175829748-03a34ffc-c344-4c59-a795-926d59d0686c.png)
6. Your key is now ready to be used when you call PixelStacker.Resources.Text.MainForm_ToggleTextures (or whatever your key was). 


## Modifying an existing entry
Entries that are already localized will not be automatically re-localized when the English version is modified. If you want to **modify** the English version, you must follow a special procedure.
1. Open up PixelStacker.Resources.Text.resx in Visual Studio.
2. Delete the entry you want to replace.
3. Build the PixelStacker.Resources project. (Right click in solution explorer and find the build button)
4. In your test explorer (open it up) run the ```Generators >> Text_Translate``` test.
5. Your localization entry will now be removed from all the different localization json files, and you are ready to re-add the localization key under the original name.
6. Follow the guide for [adding new entries](#adding).


## Actually USING the localized text entries
Any UI components that will be using localized text should implement the ```ILocalized``` interface. ```ILocalized``` interface contains a ```void ApplyLocalization(CultureInfo locale);``` method which is called when localization should be applied. Unfortunately, this is up to the developer to update the text of individudal UI components when this method is called. 
![example usage of localized texts](https://user-images.githubusercontent.com/1046026/175830472-397a4fc8-b28e-4475-a416-671b66585409.png)


[setup]: #setting-up-your-developer-environment-for-localization
[adding]: #adding-a-new-localized-text-entry
[modifying]: #what-if-i-want-to-modify-an-existing-key
[using]: #actually-using-the-localized-text-entries
