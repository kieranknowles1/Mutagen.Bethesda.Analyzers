using Mutagen.Bethesda.Analyzers.Skyrim.Record.Placed.Npc;
using Mutagen.Bethesda.Analyzers.Testing.Frameworks;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Testing.AutoData;
using Xunit;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Tests.ContextualRecords.Placed.Npcs;

using Fixture = ContextualRecordTestFixture<PersistenceLocationAnalyzer, PlacedNpc, IPlacedNpcGetter>;

public class PersistenceLocationAnalyzerTest
{
    // Place an NPC in a new interior cell and assign its persist location to a new location
    static void Setup(PlacedNpc npc, ISkyrimMod mod, Fixture fixture, out Cell cell, out Location location)
    {
        cell = fixture.Create<Cell>();
        cell.Flags |= Cell.Flag.IsInteriorCell;
        cell.Temporary.Add(npc);
        mod.Cells.AddInteriorCell(cell);

        location = fixture.Create<Location>();
        mod.Locations.Add(location);

        npc.PersistentLocation.SetTo(location);
    }

    // Persist location analyzers do not apply to Npcs with PersistAll or VirtualLocation
    [Theory, MutagenModAutoData]
    public void PersistAll(Fixture fixture)
    {
        Cell? cell = null;
        Location? location = null;
        fixture.Run(
            prepForError: (rec, mod) =>
            {
                Setup(rec, mod, fixture, out cell, out location);
            },
            prepForFix: (rec, mod) =>
            {
                rec.PersistentLocation.SetTo(FormKeys.SkyrimSE.Skyrim.Location.PersistAll);
            },
            PersistenceLocationAnalyzer.PersistenceLocationWithCellWithoutLocation);
    }

    // Persist location analyzers do not apply to Npcs with PersistAll or VirtualLocation
    [Theory, MutagenModAutoData]
    public void PersistVirtual(Fixture fixture)
    {
        Cell? cell = null;
        Location? location = null;
        fixture.Run(
            prepForError: (rec, mod) =>
            {
                Setup(rec, mod, fixture, out cell, out location);
            },
            prepForFix: (rec, mod) =>
            {
                rec.PersistentLocation.SetTo(FormKeys.SkyrimSE.Skyrim.Location.VirtualLocation);
            },
            PersistenceLocationAnalyzer.PersistenceLocationWithCellWithoutLocation);
    }

    // An NPC with a persist location should be in a cell with a location
    [Theory, MutagenModAutoData]
    public void PersistWithoutCellLocation(Fixture fixture)
    {
        Cell? cell = null;
        Location? location = null;
        fixture.Run(
            prepForError: (rec, mod) =>
            {
                Setup(rec, mod, fixture, out cell, out location);
            },
            prepForFix: (rec, mod) =>
            {
                cell!.Location.SetTo(location);
            },
            PersistenceLocationAnalyzer.PersistenceLocationWithCellWithoutLocation);
    }

    // An exterior cell may inherit its location from a worldspace
    [Theory, MutagenModAutoData]
    public void PersistWithoutCellLocationWorldspace(Fixture fixture)
    {
        Location? location = null;
        Worldspace? world = null;
        fixture.Run(
            prepForError: (rec, mod) =>
            {
                var cell = fixture.Create<Cell>();
                cell.Grid = new();
                cell.Temporary.Add(rec);

                world = fixture.Create<Worldspace>();
                world.AddCell(cell);
                mod.Worldspaces.Add(world);

                location = fixture.Create<Location>();
                mod.Locations.Add(location);

                rec.Placement = new();
                rec.PersistentLocation.SetTo(location);
            },
            prepForFix: (rec, mod) =>
            {
                world!.Location.SetTo(location);
            },
            PersistenceLocationAnalyzer.PersistenceLocationWithCellWithoutLocation);
    }

    // An NPCs cell location should be in its persist location
    [Theory, MutagenModAutoData]
    public void NotInPersistLocation(Fixture fixture)
    {
        Cell? cell = null;
        Location? location = null;
        fixture.Run(
            prepForError: (rec, mod) =>
            {
                Setup(rec, mod, fixture, out cell, out location);
                var loc2 = fixture.Create<Location>();
                mod.Locations.Add(loc2);
                cell.Location.SetTo(loc2);
            },
            prepForFix: (rec, mod) =>
            {
                cell!.Location.SetTo(location);
            },
            PersistenceLocationAnalyzer.NotInsidePersistenceLocation);
    }

    // An NPCs cell location should be in its persist location, including the persist location's children
    // I.e., a persist location of a city is suitable for an npc in a house interior
    [Theory, MutagenModAutoData]
    public void NotInPersistLocationChildren(Fixture fixture)
    {
        Cell? cell = null;
        Location? location = null;
        fixture.Run(
            prepForError: (rec, mod) =>
            {
                Setup(rec, mod, fixture, out cell, out location);
                var loc2 = fixture.Create<Location>();
                mod.Locations.Add(loc2);
                cell.Location.SetTo(loc2);
            },
            prepForFix: (rec, mod) =>
            {
                var child = fixture.Create<Location>();
                mod.Locations.Add(child);
                child.ParentLocation.SetTo(location);
                cell!.Location.SetTo(child);
            },
            PersistenceLocationAnalyzer.NotInsidePersistenceLocation);
    }

    // An NPCs cell location should be in its persist location, not including the persist location's parents
    // I.e., a persist location of a house is not suitable for an npc in the city's exterior
    [Theory, MutagenModAutoData]
    public void NotInPersistLocationParent(Fixture fixture)
    {
        Cell? cell = null;
        Location? parent = null;
        fixture.Run(
            prepForError: (rec, mod) =>
            {
                Setup(rec, mod, fixture, out cell, out var location);
                parent = fixture.Create<Location>();
                mod.Locations.Add(parent);
                location.ParentLocation.SetTo(parent);
                cell.Location.SetTo(parent);
            },
            prepForFix: (rec, mod) =>
            {
                rec.PersistentLocation.SetTo(parent);
            },
            PersistenceLocationAnalyzer.NotInsidePersistenceLocation);
    }

    // An NPCs cell location should be in its persist location, not including the persist location's siblings
    // I.e., a persist location of a house is not suitable for an npc in a different house
    [Theory, MutagenModAutoData]
    public void NotInPersistLocationSibling(Fixture fixture)
    {
        Cell? cell = null;
        Location? location = null;
        fixture.Run(
            prepForError: (rec, mod) =>
            {
                Setup(rec, mod, fixture, out cell, out location);
                var child = fixture.Create<Location>();
                mod.Locations.Add(child);
                var sibling = fixture.Create<Location>();
                mod.Locations.Add(sibling);
                child.ParentLocation.SetTo(location);
                sibling.ParentLocation.SetTo(location);

                cell.Location.SetTo(child);
                rec.PersistentLocation.SetTo(sibling);
            },
            prepForFix: (rec, mod) =>
            {
                rec.PersistentLocation.SetTo(location);
            },
            PersistenceLocationAnalyzer.NotInsidePersistenceLocation);
    }

    //// An NPCs persist location should not be an interior
    //[Theory, MutagenModAutoData]
    //public void PersistLocationDwelling(Fixture fixture)
    //{
    //    Cell? cell = null;
    //    Location? location = null;
    //    fixture.Run(
    //        prepForError: (rec, mod) =>
    //        {
    //            Setup(rec, mod, fixture, out cell, out location);
    //        },
    //        prepForFix: (rec, mod) =>
    //        {

    //        },
    //        PersistenceLocationAnalyzer.PersistenceLocationIsDwelling);
    //}
}
