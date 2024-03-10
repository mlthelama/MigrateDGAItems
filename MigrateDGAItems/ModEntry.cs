using System;
using Microsoft.Xna.Framework;
using MigrateDGAItems.DGAClasses;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Internal;
using StardewValley.ItemTypeDefinitions;
using StardewValley.Locations;
using StardewValley.TerrainFeatures;
using xTile.Dimensions;

namespace MigrateDGAItems
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
            helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
        }


        /*********
        ** Private methods
        *********/

        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            var spacecore = this.Helper.ModRegistry.GetApi<ISpaceCoreApi>("spacechase0.SpaceCore");

            if (spacecore is null)
            {
                Monitor.Log("No SpaceCore API found! Mod will not work!", LogLevel.Error);
            }
            else
            {
                spacecore.RegisterSerializerType(typeof(CustomObject));
                spacecore.RegisterSerializerType(typeof(CustomBasicFurniture));
                spacecore.RegisterSerializerType(typeof(CustomBedFurniture));
                spacecore.RegisterSerializerType(typeof(CustomTVFurniture));
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
                Monitor.Log("Registered subclasses with SpaceCore!", LogLevel.Alert);
            }
        }

        private void OnSaveLoaded(object? sender, SaveLoadedEventArgs e)
        {
            Game1.player.addItemByMenuIfNecessary(new CustomObject());
            Utility.ForEachItem(fixItem);
            Utility.ForEachLocation(l => fixTerrainFeatures(l));
        }

        private bool fixItem(Item item, Action remove, Action<Item> replaceWith)
        {
            // If it's a DGA furniture
            if (item is CustomBasicFurniture or CustomBedFurniture or CustomFishTankFurniture or CustomStorageFurniture or CustomTVFurniture)
            {
                // Type = (F)
                Monitor.Log($"Error item found with name: {item.Name}", LogLevel.Debug);
                string itemId = getBestGuess("(F)", item.Name);
                Monitor.Log($"Replacing {item.Name} with {itemId}", LogLevel.Alert);
                replaceWith(ItemRegistry.Create(itemId));
            }
            // If it's a DGA object
            else if (item is CustomObject)
            {
                // Type = (O)
                Monitor.Log($"Error item found with name: {item.Name}", LogLevel.Debug);
                string itemId = getBestGuess("(O)",item.Name);
                Monitor.Log($"Replacing {item.Name} with {itemId}", LogLevel.Alert);
                replaceWith(ItemRegistry.Create(itemId));
            }
            // If it's a DGA big craftable
            else if (item is CustomBigCraftable)
            {
                // Type = (BC)
                Monitor.Log($"Error item found with name: {item.Name}", LogLevel.Debug);
                string itemId = getBestGuess("(BC)", item.Name);
                Monitor.Log($"Replacing {item.Name} with {itemId}", LogLevel.Alert);
                replaceWith(ItemRegistry.Create(itemId));
            }
            // If it's a DGA weapon
            else if (item is CustomMeleeWeapon)
            {
                // Type = (W)
            }
            // If it's a DGA clothing item
            else if (item is CustomPants or CustomShirt or CustomHat or CustomBoots)
            {
                // Type = (P) for pants, (S) for shirts, (H) for hats, (B) for boots
            }
            else
            {
            }
            return true;
        }

        private bool fixTerrainFeatures(GameLocation l)
        {
            foreach (KeyValuePair<Vector2, StardewValley.Object> pair in l.objects.Pairs)
            {
                if (pair.Value is CustomFence)
                {
                    // Fix the fence
                }
            }
            foreach (TerrainFeature terrain in l.terrainFeatures.Values)
            {
                if (terrain is CustomFruitTree)
                {
                    // Replace the fruit tree properly
                }
                else if (terrain is CustomGiantCrop)
                {
                    // Replace the custom giant crop properly
                }
                else if (terrain is HoeDirt)
                {
                    HoeDirt hoeDirt = (HoeDirt)terrain;
                    if (hoeDirt.crop is not null && hoeDirt.crop is CustomCrop)
                    {
                        // replace the crop properly!
                    }
                }
            }
            return true;
        }

        private string getBestGuess(string type, string itemName)
        {
            // Do some fancy string splitting on the item's name, assuming DGA formatting
            string name = itemName.Split("/").Last();
            string packName = itemName.Split("/").First();
            string packNameWithoutDGA = packName.Split(".").First() + packName.Split(".").Last();

            // Build the dictionary of name to qualified item ID strings to search over
            IItemDataDefinition itemType = ItemRegistry.GetTypeDefinition(type);
            Dictionary<string, string> allItems = new Dictionary<string, string>();
            foreach (string itemId in itemType.GetAllIds())
            {
                ParsedItemData itemData = itemType.GetData(itemId);
                if (!allItems.ContainsKey(itemData.InternalName))
                {
                    allItems[itemData.InternalName] = itemType.Identifier + itemId;
                }
            }

            // Check if itemName exactly is there
            if (allItems.Keys.Contains(itemName))
            {
                return allItems[itemName];
            }
            // Check it {{ModId}}_ItemName is there
            if (allItems.Keys.Contains(packName + "_" + name))
            {
                return allItems[packName + "_" + name];
            }
            // Check it {{ModId}}.ItemName is there
            if (allItems.Keys.Contains(packName + "." + name))
            {
                return allItems[packName + "." + name];
            }
            // Check it {{ModId}}_ItemName is there without DGA
            if (allItems.Keys.Contains(packNameWithoutDGA + "_" + name))
            {
                return allItems[packNameWithoutDGA + "_" + name];
            }
            // Check it {{ModId}}.ItemName is there without DGA
            if (allItems.Keys.Contains(packNameWithoutDGA + "." + name))
            {
                return allItems[packNameWithoutDGA + "." + name];
            }

            // Try the stupid thing of just fuzzy searching on the whole name
            string fuzzyResult = Utility.fuzzySearch(itemName, allItems.Keys);
            if (fuzzyResult is not null)
            {
                Monitor.Log($"First guess is {fuzzyResult}", LogLevel.Debug);
                return allItems[fuzzyResult];
            }

            // Try searching for the item name generated by {{ModId}}_ItemName
            fuzzyResult = Utility.fuzzySearch(packName + "_" + name, allItems.Keys);
            if (fuzzyResult is not null)
            {
                Monitor.Log($"Second guess is {fuzzyResult}", LogLevel.Debug);
                return allItems[fuzzyResult];
            }

            // Try searching for the item name generated by {{ModId}}.ItemName
            fuzzyResult = Utility.fuzzySearch(packName + "." + name, allItems.Keys);
            if (fuzzyResult is not null)
            {
                Monitor.Log($"Third guess is {fuzzyResult}", LogLevel.Debug);
                return allItems[fuzzyResult];
            }

            // Try searching for the item name generated by {{ModId}}_ItemName without DGA
            fuzzyResult = Utility.fuzzySearch(packNameWithoutDGA + "_" + name, allItems.Keys);
            if (fuzzyResult is not null)
            {
                Monitor.Log($"Fourth guess is {fuzzyResult}", LogLevel.Debug);
                return allItems[fuzzyResult];
            }

            // Try searching for the item name generated by {{ModId}}.ItemName without DGA
            fuzzyResult = Utility.fuzzySearch(packNameWithoutDGA + "." + name, allItems.Keys);
            if (fuzzyResult is not null)
            {
                Monitor.Log($"Fifth guess is {fuzzyResult}", LogLevel.Debug);
                return allItems[fuzzyResult];
            }

            //Try the crazy option of searching for the bare item name(probably makes bad things happen!)
            //fuzzyResult = Utility.fuzzySearch(name, allItems.Keys);
            //if (fuzzyResult is not null)
            //{
            //    Monitor.Log($"First guess is {fuzzyResult}", LogLevel.Debug);
            //    return allItems[fuzzyResult];
            //}

            return type + "-1";
        }
    }
}