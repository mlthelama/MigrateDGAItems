using System;
using System.Xml.Serialization;
using StardewValley;
using StardewValley.Objects;

namespace MigrateDGAItems.DGAClasses
{
    [XmlType("Mods_DGABasicFurniture")]
    public class CustomBasicFurniture: Furniture
	{
		public CustomBasicFurniture()
		{
		}
	}

    [XmlType("Mods_DGABedFurniture")]
    public class CustomBedFurniture : BedFurniture
    {
        public CustomBedFurniture()
        {
        }
    }

    [XmlType("Mods_DGABigCraftable")]
    public class CustomBigCraftable : StardewValley.Object
    {
        public CustomBigCraftable()
        {
        }
    }

    [XmlType("Mods_DGABoots")]
    public class CustomBoots : Boots
    {
        public CustomBoots()
        {
        }
    }

    [XmlType("Mods_DGACrop")]
    public class CustomCrop : Crop
    {
        public CustomCrop()
        {
        }
    }

    [XmlType("Mods_DGAFence")]
    public class CustomFence : Fence
    {
        public CustomFence()
        {
        }
    }

    [XmlType("Mods_DGAFishTankFurniture")]
    public class CustomFishTankFurniture : FishTankFurniture
    {
        public CustomFishTankFurniture()
        {
        }
    }
}

