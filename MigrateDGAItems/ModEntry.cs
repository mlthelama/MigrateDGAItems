using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

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
            helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnSaveLoaded(object? sender, SaveLoadedEventArgs e)
        {
            Utility.ForEachItem(i => { fixItem(i); return true; });
        }

        private Item fixItem(Item item) {
            // if it's an error item, try to fix it
            if (ItemRegistry.GetDataOrErrorItem(item.QualifiedItemId).IsErrorItem)
            {
                Monitor.Log($"Error item found with name: {item.Name}", LogLevel.Debug);
                Item fuzzyResult = Utility.fuzzyItemSearch(item.Name);
                if (fuzzyResult is null)
                {
                    string nameWithoutQualifiers = item.Name.Split(".").Last();
                    string nameWithoutDGA = item.Name.Split(".").First() + "." + nameWithoutQualifiers;
                    // Try searching for the item name
                    fuzzyResult = Utility.fuzzyItemSearch(nameWithoutDGA);
                    // Give up if that didn't work
                    if (fuzzyResult is null)
                    {
                        fuzzyResult = item;
                    }
                }
                return fuzzyResult;
            }
            else
            {
                return item;
            }
        }
    }
}