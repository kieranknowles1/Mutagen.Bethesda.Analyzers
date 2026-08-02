using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Analyzers.Skyrim.Record.Dialog.Responses;
using Mutagen.Bethesda.Analyzers.Testing.Frameworks;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Testing.AutoData;
using Xunit;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Tests.ContextualRecords.Dialog.Responses;

using Fixture = ContextualRecordTestFixture<SpeakerAnalyzer, DialogResponses, IDialogResponsesGetter>;

public class SpeakerAnalyzerTest
{
    ConditionFloat CreateSpeakerCondition(Npc speaker)
    {
        var data = new GetIsIDConditionData();
        data.Object.Link.SetTo(speaker);
        return new ConditionFloat()
        {
            Data = data,
            CompareOperator = CompareOperator.EqualTo,
            ComparisonValue = 1,
            Flags = Condition.Flag.OR,
        };
    }

    (VoiceType, DialogTopic) CommonSetup(DialogResponses response, ISkyrimMod mod)
    {
        var topic = mod.DialogTopics.AddNew();
        var quest = mod.Quests.AddNew();
        topic.Quest.SetTo(quest);
        topic.Responses.Add(response);

        var voice = mod.VoiceTypes.AddNew();
        voice.EditorID = voice.FormKey.ToString(); // VoiceTypeAssetLookup doesn't include voices with no editor ID
        return (voice, topic);
    }

    [Theory, MutagenModAutoData]
    public void NoSpeakers(Fixture fixture, Npc npc, Faction faction)
    {
        fixture.Run(
            prepForError: (rec, mod) =>
            {
                var (voice, _) = CommonSetup(rec, mod);
                npc.Voice.SetTo(voice);

                // IsInFaction faction AND GetIsId npc
                var factionData = new GetInFactionConditionData();
                factionData.Faction.Link.SetTo(faction);
                rec.Conditions.Add(new ConditionFloat()
                {
                    Data = factionData,
                    CompareOperator = CompareOperator.EqualTo,
                    ComparisonValue = 1,
                });
                rec.Conditions.Add(CreateSpeakerCondition(npc));
            },
            prepForFix: (rec, mod) =>
            {
                // Add missing faction entry
                npc.Factions.Add(new() { Faction = faction.ToLink() });
            },
            SpeakerAnalyzer.MissingSpeaker);
    }

    [Theory, MutagenModAutoData]
    public void NoCommonSpeakersShared(Fixture fixture, Npc npc1, Npc npc2, DialogResponses sharedInfo)
    {
        fixture.Run(
            prepForError: (rec, mod) =>
            {
                var (voice, topic) = CommonSetup(rec, mod);
                npc1.Voice.SetTo(voice);
                npc2.Voice.SetTo(voice);

                topic.Responses.Add(sharedInfo);

                rec.ResponseData.SetTo(sharedInfo);
                // GetIsId npc1
                rec.Conditions.Add(CreateSpeakerCondition(npc1));

                // GetIsId npc2
                sharedInfo.Conditions.Add(CreateSpeakerCondition(npc2));
            },
            prepForFix: (rec, mod) =>
            {
                // Add missing condition to shared
                sharedInfo.Conditions.Add(CreateSpeakerCondition(npc1));
            },
            SpeakerAnalyzer.DifferentSpeakerInSharedInfo);
    }

    [Theory, MutagenModAutoData]
    public void SharedSpeakersNotSubset(Fixture fixture, Npc npc1, Npc npc2, DialogResponses sharedInfo)
    {
        fixture.Run(
            prepForError: (rec, mod) =>
            {
                var (voice, topic) = CommonSetup(rec, mod);
                npc1.Voice.SetTo(voice);
                npc2.Voice.SetTo(voice);

                topic.Responses.Add(sharedInfo);

                rec.ResponseData.SetTo(sharedInfo);
                // GetIsId npc1 OR GetisId npc2
                rec.Conditions.Add(CreateSpeakerCondition(npc1));
                rec.Conditions.Add(CreateSpeakerCondition(npc2));

                // GetIsId npc2
                sharedInfo.Conditions.Add(CreateSpeakerCondition(npc2));
            },
            prepForFix: (rec, mod) =>
            {
                // Add missing condition to shared
                sharedInfo.Conditions.Add(CreateSpeakerCondition(npc1));
            },
            SpeakerAnalyzer.DifferentSpeakerInSharedInfo);
    }
}
