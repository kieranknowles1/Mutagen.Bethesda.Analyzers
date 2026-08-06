using Mutagen.Bethesda.Analyzers.Skyrim.Record.IdleMarker;
using Mutagen.Bethesda.Analyzers.Testing.Frameworks;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Testing.AutoData;
using Xunit;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Tests.IsolatedRecords.IdleMarkers;

public class MissingFieldsAnalyzerTest
{
    [Theory, MutagenModAutoData]
    public void NoIdlesNull(IsolatedRecordTestFixture<MissingFieldsAnalyzer, IdleMarker, IIdleMarkerGetter> fixture)
    {
        fixture.Run(
            prepForError: rec =>
            {
                rec.Animations = null;
            },
            prepForFix: rec =>
            {
                rec.Animations = [FormKeys.SkyrimSE.Skyrim.IdleAnimation.IdleGetAttention];
            },
            MissingFieldsAnalyzer.NoIdles);
    }

    [Theory, MutagenModAutoData]
    public void NoIdlesEmpty(IsolatedRecordTestFixture<MissingFieldsAnalyzer, IdleMarker, IIdleMarkerGetter> fixture)
    {
        fixture.Run(
            prepForError: rec =>
            {
                rec.Animations = [];
            },
            prepForFix: rec =>
            {
                rec.Animations = [FormKeys.SkyrimSE.Skyrim.IdleAnimation.IdleGetAttention];
            },
            MissingFieldsAnalyzer.NoIdles);
    }
}
