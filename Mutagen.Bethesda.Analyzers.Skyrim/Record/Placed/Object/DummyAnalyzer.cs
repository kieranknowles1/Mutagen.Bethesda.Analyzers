using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Aspects;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Placed.Object;

public class DummyAnalyzer : IContextualRecordAnalyzer<IPlacedObjectGetter>
{
    public static readonly TopicDefinition<IFormLinkNullableGetter<IPlaceableObjectGetter>> DummyItemWithoutLeveledList = MutagenTopicBuilder.FromDiscussion(
            388,
            "Dummy Item Without Leveled List",
            Severity.Warning)
        .WithFormatting<IFormLinkNullableGetter<IPlaceableObjectGetter>>("Placed Object is a dummy item {0} without a leveled list");

    public static readonly TopicDefinition<IFormLinkNullableGetter<IPlaceableObjectGetter>, IFormLinkNullableGetter<ILeveledItemGetter>> NonDummyItemWithLeveledList = MutagenTopicBuilder.FromDiscussion(
            387,
            "Non-Dummy Item With Leveled List",
            Severity.Warning)
        .WithFormatting<IFormLinkNullableGetter<IPlaceableObjectGetter>, IFormLinkNullableGetter<ILeveledItemGetter>>("Placed Object is not a dummy item {0} but has a leveled list {1}");

    public IEnumerable<TopicDefinition> Topics { get; } = [DummyItemWithoutLeveledList, NonDummyItemWithLeveledList];

    public void AnalyzeRecord(ContextualRecordAnalyzerParams<IPlacedObjectGetter> param)
    {
        var placedObject = param.Record;
        if (!placedObject.Base.TryResolve(param.LinkCache, out var baseObject))
            return;

        var hasLeveledList = placedObject.LeveledItemBaseObject.IsNull == false;
        var isDummyItem = baseObject is IKeywordedGetter<IKeywordGetter> keyworded && keyworded.HasKeyword(FormKeys.SkyrimSE.Skyrim.Keyword.Dummy);

        if (isDummyItem && !hasLeveledList)
        {
            param.AddTopic(
                DummyItemWithoutLeveledList.Format(placedObject.Base));
        }
        else if (!isDummyItem && hasLeveledList)
        {
            param.AddTopic(
                NonDummyItemWithLeveledList.Format(placedObject.Base, placedObject.LeveledItemBaseObject));
        }
    }

    public IEnumerable<Func<IPlacedObjectGetter, object?>> FieldsOfInterest()
    {
        yield return x => x.Base;
        yield return x => x.LeveledItemBaseObject;
    }
}
