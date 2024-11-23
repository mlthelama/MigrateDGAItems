using System.Xml.Serialization;
using StardewValley;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using Object = StardewValley.Object;

namespace MigrateDGAItems.DGAClasses;

[XmlType("Mods_DGABasicFurniture")]
public class CustomBasicFurniture : Furniture
{
}

[XmlType("Mods_DGABedFurniture")]
public class CustomBedFurniture : BedFurniture
{
}

[XmlType("Mods_DGABigCraftable")]
public class CustomBigCraftable : Object
{
}

[XmlType("Mods_DGABoots")]
public class CustomBoots : Boots
{
}

[XmlType("Mods_DGACrop")]
public class CustomCrop : Crop
{
}

[XmlType("Mods_DGAFence")]
public class CustomFence : Fence
{
}

[XmlType("Mods_DGAFishTankFurniture")]
public class CustomFishTankFurniture : FishTankFurniture
{
}

[XmlType("Mods_DGAFruitTree")]
public class CustomFruitTree : FruitTree
{
}

[XmlType("Mods_DGAGiantCrop")]
public class CustomGiantCrop : GiantCrop
{
}

[XmlType("Mods_DGAHat")]
public class CustomHat : Hat
{
}

[XmlType("Mods_DGAMeleeWeapon")]
public class CustomMeleeWeapon : MeleeWeapon
{
}

[XmlType("Mods_DGAObject")]
public class CustomObject : Object
{
}

[XmlType("Mods_DGAPants")]
public class CustomPants : Clothing
{
}

[XmlType("Mods_DGAShirt")]
public class CustomShirt : Clothing
{
}

[XmlType("Mods_DGAStorageFurniture")]
public class CustomStorageFurniture : StorageFurniture
{
}

[XmlType("Mods_DGATVFurniture")]
public class CustomTvFurniture : TV
{
}