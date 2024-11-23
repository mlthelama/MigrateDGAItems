using Microsoft.Xna.Framework;
using MigrateDGAItems.DGAClasses;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Internal;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using SObject = StardewValley.Object;

namespace MigrateDGAItems;

/// <summary>The mod entry point.</summary>
internal sealed class CustomModEntry : Mod
{
    /*********
     ** Public methods
     *********/
    /// <summary>The mod entry point, called after the mod is first loaded.</summary>
    /// <param name="helper">Provides simplified APIs for writing mods.</param>
    public override void Entry(IModHelper helper)
    {
        helper.Events.GameLoop.GameLaunched += OnGameLaunched;
        helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
    }


    /*********
     ** Private methods
     *********/

    private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
    {
        var spacecore = Helper.ModRegistry.GetApi<ISpaceCoreApi>("spacechase0.SpaceCore");

        if (spacecore is null)
        {
            Monitor.Log("No SpaceCore API found! Mod will not work!", LogLevel.Error);
        }
        else
        {
            spacecore.RegisterSerializerType(typeof(CustomObject));
            spacecore.RegisterSerializerType(typeof(CustomBasicFurniture));
            spacecore.RegisterSerializerType(typeof(CustomBedFurniture));
            spacecore.RegisterSerializerType(typeof(CustomTvFurniture));
            spacecore.RegisterSerializerType(typeof(CustomFishTankFurniture));
            spacecore.RegisterSerializerType(typeof(CustomStorageFurniture));
            spacecore.RegisterSerializerType(typeof(CustomCrop));
            spacecore.RegisterSerializerType(typeof(CustomGiantCrop));
            spacecore.RegisterSerializerType(typeof(CustomMeleeWeapon));
            spacecore.RegisterSerializerType(typeof(CustomBoots));
            spacecore.RegisterSerializerType(typeof(CustomHat));
            spacecore.RegisterSerializerType(typeof(CustomFence));
            spacecore.RegisterSerializerType(typeof(CustomBigCraftable));
            spacecore.RegisterSerializerType(typeof(CustomFruitTree));
            spacecore.RegisterSerializerType(typeof(CustomShirt));
            spacecore.RegisterSerializerType(typeof(CustomPants));
            Monitor.Log("Registered subclasses with SpaceCore!", LogLevel.Info);
        }
    }

    private void OnSaveLoaded(object? sender, SaveLoadedEventArgs e)
    {
        // Add a error item for debugging
        //Game1.player.addItemByMenuIfNecessary(new CustomObject());

        //Utility.ForEachItem(FixItem);
        Utility.ForEachItemContext(delegate(in ForEachItemContext x)
        {
            FixItem(x.Item, x.ReplaceItemWith);
            return true;
        });
        Utility.ForEachLocation(FixTerrainFeatures);

        // Debugging stuff to alert on furniture in the farmhouse
        //foreach (Furniture f in Game1.getLocationFromName("Farmhouse").furniture)
        //{
        //    Monitor.Log($"Furniture found in farmhouse with name {f.Name}", LogLevel.Trace);
        //}
        Monitor.Log("Done!", LogLevel.Info);
    }

    private void FixItem(Item item, Action<Item> replaceWith)
    {
        // If it's a DGA furniture
        if (item is CustomBasicFurniture or CustomBedFurniture or CustomFishTankFurniture or CustomStorageFurniture
            or CustomTvFurniture)
        {
            // Type = (F)
            var locName = "unknown location";
            if (((Furniture) item).Location is {Name: not null})
                locName = ((Furniture) item).Location.Name;
            Monitor.Log($"Error item found with name: {item.Name} in {locName}");
            var itemId = GetBestItemGuess("(F)", item.Name);
            var newItem = (Furniture) ItemRegistry.Create(itemId);
            if (item is CustomStorageFurniture or CustomFishTankFurniture && newItem is StorageFurniture furniture)
                foreach (var heldThing in ((StorageFurniture) item).heldItems)
                    furniture.heldItems.Add(heldThing);

            Monitor.Log($"Replacing {item.Name} with {newItem.QualifiedItemId}", LogLevel.Debug);
            replaceWith(newItem);
        }
        // If it's a DGA object
        else if (item is CustomObject)
        {
            // Type = (O)
            Monitor.Log($"Error item found with name: {item.Name}", LogLevel.Debug);
            var itemId = GetBestItemGuess("(O)", item.Name);
            var newObject = (SObject) ItemRegistry.Create(itemId);
            Monitor.Log($"Replacing {item.Name} with {newObject.QualifiedItemId}", LogLevel.Debug);
            replaceWith(newObject);
        }
        // If it's a DGA big craftable
        else if (item is CustomBigCraftable)
        {
            // Type = (BC)
            Monitor.Log($"Error item found with name: {item.Name}", LogLevel.Debug);
            var itemId = GetBestItemGuess("(BC)", item.Name);
            Monitor.Log($"Replacing {item.Name} with {ItemRegistry.Create(itemId).QualifiedItemId}", LogLevel.Debug);
            replaceWith(ItemRegistry.Create(itemId));
        }
        // If it's a DGA weapon
        else if (item is CustomMeleeWeapon)
        {
            // Type = (W)
            Monitor.Log($"Error item found with name: {item.Name}", LogLevel.Debug);
            var itemId = GetBestItemGuess("(W)", item.Name);
            Monitor.Log($"Replacing {item.Name} with {ItemRegistry.Create(itemId).QualifiedItemId}", LogLevel.Debug);
            replaceWith(ItemRegistry.Create(itemId));
        }
        // If it's a DGA clothing item
        else if (item is CustomPants or CustomShirt or CustomHat or CustomBoots)
        {
            // Type = (P)
            Monitor.Log($"Error item found with name: {item.Name}", LogLevel.Debug);
            var itemId = GetBestItemGuess("(P)", item.Name);
            Monitor.Log($"Replacing {item.Name} with {ItemRegistry.Create(itemId).QualifiedItemId}", LogLevel.Debug);
            replaceWith(ItemRegistry.Create(itemId));
        }
        else if (item is CustomShirt)
        {
            // Type = (S)
            Monitor.Log($"Error item found with name: {item.Name}", LogLevel.Debug);
            var itemId = GetBestItemGuess("(S)", item.Name);
            Monitor.Log($"Replacing {item.Name} with {ItemRegistry.Create(itemId).QualifiedItemId}", LogLevel.Debug);
            replaceWith(ItemRegistry.Create(itemId));
        }
        else if (item is CustomHat)
        {
            // Type = (H)
            Monitor.Log($"Error item found with name: {item.Name}", LogLevel.Debug);
            var itemId = GetBestItemGuess("(H)", item.Name);
            Monitor.Log($"Replacing {item.Name} with {ItemRegistry.Create(itemId).QualifiedItemId}", LogLevel.Debug);
            replaceWith(ItemRegistry.Create(itemId));
        }
        else if (item is CustomBoots)
        {
            // Type = (B)
            Monitor.Log($"Error item found with name: {item.Name}", LogLevel.Debug);
            var itemId = GetBestItemGuess("(B)", item.Name);
            Monitor.Log($"Replacing {item.Name} with {ItemRegistry.Create(itemId).QualifiedItemId}", LogLevel.Debug);
            replaceWith(ItemRegistry.Create(itemId));
        }
    }

    private bool FixTerrainFeatures(GameLocation l)
    {
        Dictionary<Vector2, SObject> fencesToAdd = new();
        Dictionary<Vector2, SObject> fencesToRemove = new();
        foreach (KeyValuePair<Vector2, SObject> pair in l.objects.Pairs)
            if (pair.Value is CustomFence fence)
            {
                // Fix the fence
                fencesToRemove.Add(pair.Key, fence);
                var bestGuessId = GetBestFenceGuess(fence.Name);
                var newFence = new Fence(fence.TileLocation, bestGuessId,
                    fence.isGate.Value);
                fencesToAdd.Add(pair.Key, newFence);
            }

        Dictionary<Vector2, TerrainFeature> terrainFeaturesToAdd = new();
        Dictionary<Vector2, TerrainFeature> terrainFeaturesToRemove = new();
        foreach (KeyValuePair<Vector2, TerrainFeature> pair in l.terrainFeatures.Pairs)
            if (pair.Value is CustomFruitTree)
            {
                // Replace the fruit tree properly
            }
            else if (pair.Value is CustomGiantCrop)
            {
                // Replace the custom giant crop properly
            }
            else if (pair.Value is HoeDirt)
            {
                var hoeDirt = (HoeDirt) pair.Value;
                if (hoeDirt.crop is not null && hoeDirt.crop is CustomCrop)
                {
                    // replace the crop properly!
                }
            }

        return true;
    }

    private static string GetBestItemGuess(string type, string itemName)
    {
        // Do some fancy string splitting on the item's name, assuming DGA formatting
        var name = itemName.Split("/").Last();
        var packName = itemName.Split("/").First();
        var packNameWithoutDga = packName.Split(".").First() + packName.Split(".").Last();

        // Build the dictionary of name to qualified item ID strings to search over
        var itemType = ItemRegistry.GetTypeDefinition(type);
        Dictionary<string, string> allItems = new();
        foreach (var itemId in itemType.GetAllIds())
        {
            var itemData = itemType.GetData(itemId);
            if (!allItems.ContainsKey(itemData.InternalName))
                allItems[itemData.InternalName] = itemType.Identifier + itemId;
        }

        // Check if itemName exactly is there
        if (allItems.Keys.Contains(itemName)) return allItems[itemName];
        // Check it {{ModId}}_ItemName is there
        if (allItems.Keys.Contains(packName + "_" + name)) return allItems[packName + "_" + name];
        // Check it {{ModId}}.ItemName is there
        if (allItems.Keys.Contains(packName + "." + name)) return allItems[packName + "." + name];
        // Check it {{ModId}}_ItemName is there without DGA
        if (allItems.Keys.Contains(packNameWithoutDga + "_" + name)) return allItems[packNameWithoutDga + "_" + name];
        // Check it {{ModId}}.ItemName is there without DGA
        if (allItems.Keys.Contains(packNameWithoutDga + "." + name)) return allItems[packNameWithoutDga + "." + name];

        // Try the stupid thing of just fuzzy searching on the whole name
        var fuzzyResult = Utility.fuzzySearch(itemName, allItems.Keys);
        if (fuzzyResult is not null) return allItems[fuzzyResult];

        // Try searching for the item name generated by {{ModId}}_ItemName
        fuzzyResult = Utility.fuzzySearch(packName + "_" + name, allItems.Keys);
        if (fuzzyResult is not null) return allItems[fuzzyResult];

        // Try searching for the item name generated by {{ModId}}.ItemName
        fuzzyResult = Utility.fuzzySearch(packName + "." + name, allItems.Keys);
        if (fuzzyResult is not null) return allItems[fuzzyResult];

        // Try searching for the item name generated by {{ModId}}_ItemName without DGA
        fuzzyResult = Utility.fuzzySearch(packNameWithoutDga + "_" + name, allItems.Keys);
        if (fuzzyResult is not null) return allItems[fuzzyResult];

        // Try searching for the item name generated by {{ModId}}.ItemName without DGA
        fuzzyResult = Utility.fuzzySearch(packNameWithoutDga + "." + name, allItems.Keys);
        if (fuzzyResult is not null) return allItems[fuzzyResult];

        return type + itemName;
    }

    private static string GetBestFenceGuess(string originalName)
    {
        // Do some fancy string splitting on the item's name, assuming DGA formatting
        var name = originalName.Split("/").Last();
        var packName = originalName.Split("/").First();
        var packNameWithoutDga = packName.Split(".").First() + packName.Split(".").Last();

        if (Fence.TryGetData(originalName, out _)) return originalName;

        if (Fence.TryGetData(packName + "_" + name, out _)) return packName + "_" + name;

        if (Fence.TryGetData(packName + "_" + name, out _)) return packName + "." + name;

        if (Fence.TryGetData(packName + "_" + name, out _)) return packNameWithoutDga + "_" + name;

        if (Fence.TryGetData(packName + "_" + name, out _)) return packNameWithoutDga + "." + name;
        return "-1";
    }

    private static string GetBestFruitTreeGuess(string originalName)
    {
        // Do some fancy string splitting on the item's name, assuming DGA formatting
        var name = originalName.Split("/").Last();
        var packName = originalName.Split("/").First();
        var packNameWithoutDga = packName.Split(".").First() + packName.Split(".").Last();

        if (FruitTree.TryGetData(originalName, out _)) return originalName;

        if (FruitTree.TryGetData(packName + "_" + name, out _)) return packName + "_" + name;

        if (FruitTree.TryGetData(packName + "_" + name, out _)) return packName + "." + name;

        if (FruitTree.TryGetData(packName + "_" + name, out _)) return packNameWithoutDga + "_" + name;

        if (FruitTree.TryGetData(packName + "_" + name, out _)) return packNameWithoutDga + "." + name;
        return "-1";
    }
}