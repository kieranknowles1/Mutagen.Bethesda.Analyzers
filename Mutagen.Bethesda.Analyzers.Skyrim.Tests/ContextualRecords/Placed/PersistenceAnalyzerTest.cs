using Mutagen.Bethesda.Analyzers.Skyrim.Record.Placed;
using Mutagen.Bethesda.Analyzers.Testing.Frameworks;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Testing.AutoData;
using Xunit;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Tests.ContextualRecords.Placed;

using Activator = Bethesda.Skyrim.Activator;
using Fixture = ContextualRecordTestFixture<PersistenceAnalyzer, PlacedObject, IPlacedGetter>;

public class PersistenceAnalyzerTest
{
    void SetPersistent(IPlaced placed, ISkyrimMod? _mod = null)
    {
        placed.SkyrimMajorRecordFlags |= (SkyrimMajorRecord.SkyrimMajorRecordFlag)PlacedObject.DefaultMajorFlag.Persistent;
    }

    void SetTemporary(IPlaced placed, ISkyrimMod? _mod = null)
    {
        placed.SkyrimMajorRecordFlags &= ~(SkyrimMajorRecord.SkyrimMajorRecordFlag)PlacedObject.DefaultMajorFlag.Persistent;
    }

    [Theory, MutagenModAutoData]
    public void BaseIsMarker(Fixture fixture)
    {
        fixture.Run(
            prepForError: (rec, mod) =>
            {
                rec.Base.SetTo(FormKeys.SkyrimSE.Skyrim.Static.XMarker);
            },
            prepForFix: SetPersistent,
            PersistenceAnalyzer.NotPersistent);
    }

    [Theory, MutagenModAutoData]
    public void BaseIsTextureSet(Fixture fixture)
    {
        fixture.Run(
            prepForError: (rec, mod) =>
            {
                var txst = fixture.Create<TextureSet>();
                mod.TextureSets.Add(txst);
                rec.Base.SetTo(txst);
            },
            prepForFix: SetPersistent,
            PersistenceAnalyzer.NotPersistent);
    }

    [Theory, MutagenModAutoData]
    public void BaseIsWater(Fixture fixture)
    {
        fixture.Run(
            prepForError: (rec, mod) =>
            {
                var acti = fixture.Create<Activator>();
                acti.WaterType.SetTo(FormKeys.SkyrimSE.Skyrim.Water.DefaultWater);
                mod.Activators.Add(acti);
                rec.Base.SetTo(acti);
            },
            prepForFix: SetPersistent,
            PersistenceAnalyzer.NotPersistent);
    }

    [Theory, MutagenModAutoData]
    public void FullLod(Fixture fixture)
    {
        fixture.Run(
            prepForError: (rec, mod) =>
            {
                rec.SkyrimMajorRecordFlags |= (SkyrimMajorRecord.SkyrimMajorRecordFlag)PlacedObject.DefaultMajorFlag.IsFullLod;
            },
            prepForFix: SetPersistent,
            PersistenceAnalyzer.NotPersistent);
    }

    // The "never fades" flag for lights uses the same bit as "full lod" for other types
    [Theory, MutagenModAutoData]
    public void NeverFadeLight(Fixture fixture)
    {
        fixture.Run(
            prepForError: (rec, mod) =>
            {
                SetPersistent(rec);
                var light = fixture.Create<Light>();
                mod.Lights.Add(light);
                rec.SkyrimMajorRecordFlags |= (SkyrimMajorRecord.SkyrimMajorRecordFlag)PlacedObject.LightMajorFlag.NeverFades;
                rec.Base.SetTo(light);
            },
            prepForFix: SetTemporary,
            PersistenceAnalyzer.UnnecessaryPersistence);
    }

    [Theory, MutagenModAutoData]
    public void PersistAll(Fixture fixture)
    {
        fixture.Run(
            prepForError: (rec, mod) =>
            {
                rec.PersistentLocation.SetTo(FormKeys.SkyrimSE.Skyrim.Location.PersistAll);
            },
            prepForFix: SetPersistent,
            PersistenceAnalyzer.NotPersistent);
    }

    [Theory, MutagenModAutoData]
    public void Referenced(Fixture fixture)
    {
        fixture.Run(
            prepForError: (rec, mod) =>
            {
                var user = fixture.Create<MagicEffect>();
                var data = new GetIsReferenceConditionData();
                data.Reference.SetTo(rec);
                user.Conditions.Add(new ConditionFloat()
                {
                    Data = data
                });
                mod.MagicEffects.Add(user);
            },
            prepForFix: SetPersistent,
            PersistenceAnalyzer.NotPersistent);
    }

    [Theory, MutagenModAutoData]
    public void ReferencedLocation(Fixture fixture)
    {
        fixture.Run(
            prepForError: (rec, mod) =>
            {
                SetPersistent(rec);
                var location = fixture.Create<Location>();
                location.LocationRefTypeReferencesStatic = [new()
                {
                    Ref = rec.ToLink()
                }];
                mod.Locations.Add(location);
            },
            prepForFix: SetTemporary,
            PersistenceAnalyzer.UnnecessaryPersistence);
    }

    [Theory, MutagenModAutoData]
    public void ReferencedLargeRefs(Fixture fixture)
    {
        fixture.Run(
            prepForError: (rec, mod) =>
            {
                SetPersistent(rec);
                var world = fixture.Create<Worldspace>();
                world.LargeReferences.Add(new()
                {
                    References = [new() {
                        Reference = rec.ToLink(),
                    }]
                });
                mod.Worldspaces.Add(world);
            },
            prepForFix: SetTemporary,
            PersistenceAnalyzer.UnnecessaryPersistence);
    }

    [Theory, MutagenModAutoData]
    public void NotNeeded(Fixture fixture)
    {
        fixture.Run(
            prepForError: SetPersistent,
            prepForFix: SetTemporary,
            PersistenceAnalyzer.UnnecessaryPersistence);
    }
}
