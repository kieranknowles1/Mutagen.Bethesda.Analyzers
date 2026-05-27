using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Placed;

public class PersistenceAnalyzer : IContextualRecordAnalyzer<IPlacedGetter>
{
    public static readonly TopicDefinition UnnecessaryPersistence = MutagenTopicBuilder.FromDiscussion(
            250,
            "Unnecessary Persistence",
            Severity.Warning)
        .WithoutFormatting("Placed record is persistent but does not need to be");

    public static readonly TopicDefinition NotPersistent = MutagenTopicBuilder.FromDiscussion(
            286,
            "Not persistent",
            Severity.Error)
        .WithoutFormatting("Placed record is not persistent but needs to be");

    public IEnumerable<TopicDefinition> Topics { get; } = [UnnecessaryPersistence, NotPersistent];

    private static readonly HashSet<FormKey> AlwaysPersistentObjects =
    [
        FormKeys.SkyrimSE.Skyrim.Static.XMarker.FormKey,
        FormKeys.SkyrimSE.Skyrim.Static.XMarkerHeading.FormKey,
        FormKeys.SkyrimSE.Skyrim.Static.MapMarker.FormKey,
        FormKeys.SkyrimSE.Skyrim.Static.DragonMarker.FormKey,
        FormKeys.SkyrimSE.Skyrim.Static.DragonMarkerCrashStrip.FormKey,
    ];

    static bool RequiresPersistence(ContextualRecordAnalyzerParams<IPlacedGetter> param)
    {
        var placed = param.Record;
        var referenced = param.ResolveCache<ILinkUsageCache>()
            .GetUsagesOf(placed).UsageLinks
            .Select(l => l.Resolve(param.LinkCache))
            // Locations list their ref types and persistent location NPCs, but do not require them to be persistent
            .Where(r => r is not ILocationGetter)
            // Worldspaces list their large references, but do not require them to be persistent
            .Where(r => r is not IWorldspaceGetter);

        if (referenced.Any())
            return true;

        switch (placed)
        {
            case IPlacedObjectGetter placedObject:
                // The CK always sets these as persistent, even if not referenced elsewhere
                if (AlwaysPersistentObjects.Contains(placedObject.Base.FormKey))
                    return true;
                if (placedObject.Base.TryResolve<ITextureSetGetter>(param.LinkCache, out var _))
                    return true;
                if (placedObject.Base.TryResolve<IActivatorGetter>(param.LinkCache, out var activator) && !activator.WaterType.IsNull)
                    return true;

                // Full LOD references need to be persistent. Lights use the same bit with a different meaning that doesn't require persistence
                if (placedObject.SkyrimMajorRecordFlags.HasFlag((SkyrimMajorRecord.SkyrimMajorRecordFlag)PlacedObject.DefaultMajorFlag.IsFullLod))
                    if (!placedObject.Base.TryResolve<ILightGetter>(param.LinkCache, out var _))
                        return true;

                break;
            case IPlacedNpcGetter placedNpc:
                if (placedNpc.PersistentLocation.Equals(FormKeys.SkyrimSE.Skyrim.Location.PersistAll))
                    return true;
                break;
        }
        return false;
    }

    public void AnalyzeRecord(ContextualRecordAnalyzerParams<IPlacedGetter> param)
    {
        var placed = param.Record;

        var persistent = placed.IsPersistent();
        var expected = RequiresPersistence(param);

        if (persistent && !expected)
        {
            // TODO: Narrow down when persistence is not required
            param.AddTopic(UnnecessaryPersistence.Format());
        }
        else if (!persistent && expected)
        {
            param.AddTopic(NotPersistent.Format());
        }
    }

    public IEnumerable<Func<IPlacedGetter, object?>> FieldsOfInterest()
    {
        yield return x => x.SkyrimMajorRecordFlags;
        yield return x => x is IPlacedObjectGetter o ? o.Base : null;
        yield return x => x is IPlacedNpcGetter n ? n.PersistentLocation : null;
    }
}
