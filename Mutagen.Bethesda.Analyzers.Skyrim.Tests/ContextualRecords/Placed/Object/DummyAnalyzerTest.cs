using Mutagen.Bethesda.Analyzers.Skyrim.Record.Placed.Object;
using Mutagen.Bethesda.Analyzers.Testing.Frameworks;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Testing.AutoData;
using Xunit;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Tests.ContextualRecords.Placed.Object;

using Fixture = ContextualRecordTestFixture<DummyAnalyzer, PlacedObject, IPlacedObjectGetter>;

public class DummyAnalyzerTest
{
    [Theory, MutagenModAutoData]
    public void NoList(Fixture fixture, Armor item)
    {
        fixture.Run(
            prepForError: (rec, mod) =>
            {
                item.Keywords = [FormKeys.SkyrimSE.Skyrim.Keyword.Dummy];
                rec.Base.SetTo(item);
            },
            prepForFix: (rec, mod) =>
            {
                rec.LeveledItemBaseObject.SetTo(FormKeys.SkyrimSE.Skyrim.LeveledItem.LItemArmorBootsAny);
            },
            DummyAnalyzer.DummyItemWithoutLeveledList);
    }

    [Theory, MutagenModAutoData]
    public void NotDummy(Fixture fixture, Armor item)
    {
        fixture.Run(
            prepForError: (rec, mod) =>
            {
                rec.Base.SetTo(item);
                rec.LeveledItemBaseObject.SetTo(FormKeys.SkyrimSE.Skyrim.LeveledItem.LItemArmorBootsAny);
            },
            prepForFix: (rec, mod) =>
            {
                rec.LeveledItemBaseObject.SetToNull();
            },
            DummyAnalyzer.NonDummyItemWithLeveledList);
    }
}
