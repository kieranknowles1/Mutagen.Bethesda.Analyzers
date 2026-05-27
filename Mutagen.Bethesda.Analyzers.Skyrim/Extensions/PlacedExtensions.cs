using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Extensions;

public static class PlacedExtensions
{
    public static bool IsPersistent(this IPlacedGetter placed)
    {
        return placed.SkyrimMajorRecordFlags.HasFlag((SkyrimMajorRecord.SkyrimMajorRecordFlag)PlacedObject.DefaultMajorFlag.Persistent);
    }
}
