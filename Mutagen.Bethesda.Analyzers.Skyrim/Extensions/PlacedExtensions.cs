using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Extensions;

public static class PlacedExtensions
{
    public static bool IsPersistent(this IPlacedGetter placed)
    {
        return placed.SkyrimMajorRecordFlags.HasFlag((SkyrimMajorRecord.SkyrimMajorRecordFlag)PlacedObject.DefaultMajorFlag.Persistent);
    }

    public static IFormLinkGetter<ILocationGetter> GetPersistLocation(this IPlacedGetter placed)
    {
        return placed switch
        {
            IPlacedNpcGetter npc => npc.PersistentLocation,
            IPlacedObjectGetter placedObject => placedObject.PersistentLocation,
            _ => FormLink<ILocationGetter>.Null
        };
    }
}
