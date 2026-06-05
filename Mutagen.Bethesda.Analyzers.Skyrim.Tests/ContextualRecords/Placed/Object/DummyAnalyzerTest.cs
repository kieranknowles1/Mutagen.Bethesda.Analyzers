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
        fixture.Run(
            prepForError: (rec, mod) =>
            {
                rec.Base.SetTo(FormKeys.SkyrimSE.Skyrim.Weapon.DummyBow);
            },
            prepForFix: (rec, mod) =>
            {
                rec.LeveledItemBaseObject.SetTo(FormKeys.SkyrimSE.Skyrim.LeveledItem.LItemBanditWeaponBow);
            },
            DummyAnalyzer.DummyItemWithoutLeveledList);
    }

    [Theory, MutagenModAutoData]
    public void NotDummy(Fixture fixture)
    {
        fixture.Run(
            prepForError: (rec, mod) =>
            {
                rec.Base.SetTo(FormKeys.SkyrimSE.Skyrim.Weapon.LongBow);
                rec.LeveledItemBaseObject.SetTo(FormKeys.SkyrimSE.Skyrim.LeveledItem.LItemBanditWeaponBow);
            },
            prepForFix: (rec, mod) =>
            {
                rec.LeveledItemBaseObject.SetToNull();
            },
            DummyAnalyzer.NonDummyItemWithLeveledList);
    }
}
