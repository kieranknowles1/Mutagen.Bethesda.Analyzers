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
    public void NoLeveledList(Fixture fixture)
    {
        var item = fixture.Create<Weapon>();
        item.Keywords = [FormKeys.SkyrimSE.Skyrim.Keyword.Dummy];
        var list = fixture.Create<LeveledItem>();
        fixture.Run(
            prepForError: (rec, mod) =>
            {
                mod.Weapons.Add(item);
                rec.Base.SetTo(item);
            },
            prepForFix: (rec, mod) =>
            {
                rec.LeveledItemBaseObject.SetTo(list);
            },
            DummyAnalyzer.DummyItemWithoutLeveledList);
    }

    [Theory, MutagenModAutoData]
    public void NotDummy(Fixture fixture)
    {
        var item = fixture.Create<Weapon>();
        var list = fixture.Create<LeveledItem>();
        fixture.Run(
            prepForError: (rec, mod) =>
            {
                mod.Weapons.Add(item);
                rec.Base.SetTo(item);
                rec.LeveledItemBaseObject.SetTo(list);
            },
            prepForFix: (rec, mod) =>
            {
                rec.LeveledItemBaseObject.SetToNull();
            },
            DummyAnalyzer.NonDummyItemWithLeveledList);
    }
}
