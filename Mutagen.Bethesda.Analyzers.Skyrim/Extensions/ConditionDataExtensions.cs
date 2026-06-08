using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Extensions;

public static class ConditionDataExtensions
{
    public static bool RunsOnPlayer(this IConditionDataGetter condition)
    {
        return condition.RunOnType == Condition.RunOnType.Reference
               && condition.Reference.FormKey == FormKeys.SkyrimSE.Skyrim.PlayerRef.FormKey;
    }

    public enum ReturnType
    {
        Boolean, // 0 or 1
        SignedFloat, // float >= 0
        SignedInt, // int >= 0
        UnsignedFloat, // any float
        UnsignedInt, // any int
        UnsupportedFunction, // never valid
    };

    public static ReturnType GetReturnType(this IConditionDataGetter condition)
    {
        switch (condition.Function)
        {
            // Booleans
            case Condition.Function.CanFlyHere:
            case Condition.Function.CanPayCrimeGold:
            case Condition.Function.DoesNotExist:
            case Condition.Function.EffectWasDualCast:
            case Condition.Function.EPAlchemyEffectHasKeyword:
            case Condition.Function.EPAlchemyGetMakingPoison:
            case Condition.Function.EPMagic_IsAdvanceSkill:
            case Condition.Function.EPMagic_SpellHasKeyword:
            case Condition.Function.EPMagic_SpellHasSkill:
            case Condition.Function.EPModSkillUsage_AdvanceObjectHasKeyword:
            case Condition.Function.EPModSkillUsage_IsAdvanceAction:
            case Condition.Function.EPTemperingItemHasKeyword:
            case Condition.Function.EPTemperingItemIsEnchanted:
            case Condition.Function.Exists:
            case Condition.Function.GetActorAggroRadiusViolated:
            case Condition.Function.GetAlarmed:
            case Condition.Function.GetAllowWorldInteractions:
            case Condition.Function.GetArrestedState:
            case Condition.Function.GetArrestingActor:
            case Condition.Function.GetAttacked:
            case Condition.Function.GetBribeSuccess:
            case Condition.Function.GetCannibal:
            case Condition.Function.GetCombatTargetHasKeyword:
            case Condition.Function.GetCrime:
            case Condition.Function.GetCurrentAIPackage:
            case Condition.Function.GetDead:
            case Condition.Function.GetDefaultOpen:
            case Condition.Function.GetDestroyed:
            case Condition.Function.GetDisabled:
            case Condition.Function.GetDisease:
            case Condition.Function.GetEquipped:
            case Condition.Function.GetEquippedShout:
            case Condition.Function.GetIgnoreCrime:
            case Condition.Function.GetIgnoreFriendlyHits:
            case Condition.Function.GetInCell:
            case Condition.Function.GetInCellParam:
            case Condition.Function.GetInContainer:
            case Condition.Function.GetInCurrentLoc:
            case Condition.Function.GetInCurrentLocAlias:
            case Condition.Function.GetInCurrentLocFormList:
            case Condition.Function.GetInFaction:
            case Condition.Function.GetInSameCell:
            case Condition.Function.GetInSharedCrimeFaction:
            case Condition.Function.GetIntimidateSuccess:
            case Condition.Function.GetInWorldspace:
            case Condition.Function.GetInZone:
            case Condition.Function.GetIsAlerted:
            case Condition.Function.GetIsAliasRef:
            case Condition.Function.GetIsClass:
            case Condition.Function.GetIsCrashLandRequest:
            case Condition.Function.GetIsCrimeFaction:
            case Condition.Function.GetIsCurrentPackage:
            case Condition.Function.GetIsCurrentWeather:
            case Condition.Function.GetIsEditorLocAlias:
            case Condition.Function.GetIsEditorLocation:
            case Condition.Function.GetIsFlying:
            case Condition.Function.GetIsGhost:
            case Condition.Function.GetIsHastyLandRequest:
            case Condition.Function.GetIsID:
            case Condition.Function.GetIsInjured:
            case Condition.Function.GetIsObjectType:
            case Condition.Function.GetIsPlayableRace:
            case Condition.Function.GetIsRace:
            case Condition.Function.GetIsReference:
            case Condition.Function.GetIsSex:
            case Condition.Function.GetIsUsedItem:
            case Condition.Function.GetIsUsedItemEquipType:
            case Condition.Function.GetIsUsedItemType:
            case Condition.Function.GetIsVoiceType:
            case Condition.Function.GetKnockedState:
            case Condition.Function.GetLineOfSight:
            case Condition.Function.GetLocationAliasCleared:
            case Condition.Function.GetLocationCleared:
            case Condition.Function.GetLocked:
            case Condition.Function.GetNoBleedoutRecovery:
            case Condition.Function.GetOffersServicesNow:
            case Condition.Function.GetPairedAnimation:
            case Condition.Function.GetPCEnemyofFaction:
            case Condition.Function.GetPCExpelled:
            case Condition.Function.GetPCFactionAttack:
            case Condition.Function.GetPCFactionMurder:
            case Condition.Function.GetPCInFaction:
            case Condition.Function.GetPCIsRace:
            case Condition.Function.GetPCIsSex:
            case Condition.Function.GetPCMiscStat:
            case Condition.Function.GetPlayerAction:
            case Condition.Function.GetPlayerTeammate:
            case Condition.Function.GetQuestCompleted:
            case Condition.Function.GetQuestRunning:
            case Condition.Function.GetRestrained:
            case Condition.Function.GetScale:
            case Condition.Function.GetStageDone:
            case Condition.Function.GetTalkedToPC:
            case Condition.Function.GetTalkedToPCParam:
            case Condition.Function.GetUnconscious:
            case Condition.Function.GetVampireFeed:
            case Condition.Function.GetWantBlocking:
            case Condition.Function.GetWithinPackageLocation:
            case Condition.Function.HasAssociationType:
            case Condition.Function.HasAssociationTypeAny:
            case Condition.Function.HasBeenEaten:
            case Condition.Function.HasBoundWeaponEquipped:
            case Condition.Function.HasEquippedSpell:
            case Condition.Function.HasFamilyRelationship:
            case Condition.Function.HasFamilyRelationshipAny:
            case Condition.Function.HasKeyword:
            case Condition.Function.HasLinkedRef:
            case Condition.Function.HasLoaded3D:
            case Condition.Function.HasMagicEffect:
            case Condition.Function.HasMagicEffectKeyword:
            case Condition.Function.HasParentRelationship:
            case Condition.Function.HasPerk:
            case Condition.Function.HasRefType:
            case Condition.Function.HasSameEditorLocAsRef:
            case Condition.Function.HasSameEditorLocAsRefAlias:
            case Condition.Function.HasShout:
            case Condition.Function.HasSpell:
            case Condition.Function.HasTwoHandedWeaponEquipped:
            case Condition.Function.IsActor:
            case Condition.Function.IsActorAVictim:
            case Condition.Function.IsAllowedToFly:
            case Condition.Function.IsAttacking:
            case Condition.Function.IsAttackType:
            case Condition.Function.IsBeingRidden:
            case Condition.Function.IsBleedingOut:
            case Condition.Function.IsBlocking:
            case Condition.Function.IsBribedbyPlayer:
            case Condition.Function.IsCarryable:
            case Condition.Function.IsCasting:
            case Condition.Function.IsCellOwner:
            case Condition.Function.IsChild:
            case Condition.Function.IsCloserToAThanB:
            case Condition.Function.IsCloudy:
            case Condition.Function.IsCombatTarget:
            case Condition.Function.IsCommandedActor:
            case Condition.Function.IsCurrentFurnitureObj:
            case Condition.Function.IsCurrentFurnitureRef:
            case Condition.Function.IsDualCasting:
            case Condition.Function.IsEnteringInteractionQuick:
            case Condition.Function.IsEssential:
            case Condition.Function.IsExitingInstant:
            case Condition.Function.IsExitingInteractionQuick:
            case Condition.Function.IsFacingUp:
            case Condition.Function.IsFleeing:
            case Condition.Function.IsFlyingMountFastTravelling:
            case Condition.Function.IsFlyingMountPatrolQueud:
            case Condition.Function.IsFurnitureAnimType:
            case Condition.Function.IsFurnitureEntryType:
            case Condition.Function.IsGoreDisabled:
            case Condition.Function.IsGreetingPlayer:
            case Condition.Function.IsGuard:
            case Condition.Function.IsHostileToActor:
            case Condition.Function.IsIgnoringCombat:
            case Condition.Function.IsInCombat:
            case Condition.Function.IsInCriticalStage:
            case Condition.Function.IsInDangerousWater:
            case Condition.Function.IsInDialogueWithPlayer:
            case Condition.Function.IsInFavorState:
            case Condition.Function.IsInFriendStateWithPlayer:
            case Condition.Function.IsInFurnitureState:
            case Condition.Function.IsInInterior:
            case Condition.Function.IsInList:
            case Condition.Function.IsInMyOwnedCell:
            case Condition.Function.IsInSameCurrentLocAsRef:
            case Condition.Function.IsInSameCurrentLocAsRefAlias:
            case Condition.Function.IsInScene:
            case Condition.Function.IsIntimidatedbyPlayer:
            case Condition.Function.IsKiller:
            case Condition.Function.IsKillerObject:
            case Condition.Function.IsLastHostileActor:
            case Condition.Function.IsLeftUp:
            case Condition.Function.IsLimbGone:
            case Condition.Function.IsLinkedTo:
            case Condition.Function.IsLocAliasLoaded:
            case Condition.Function.IsLocationLoaded:
            case Condition.Function.IsMoving:
            case Condition.Function.IsNullPackageData:
            case Condition.Function.IsOnFlyingMount:
            case Condition.Function.IsOverEncumbered:
            case Condition.Function.IsOwner:
            case Condition.Function.IsPathing:
            case Condition.Function.IsPC1stPerson:
            case Condition.Function.IsPCAMurderer:
            case Condition.Function.IsPCSleeping:
            case Condition.Function.IsPlayerGrabbedRef:
            case Condition.Function.IsPlayerInRegion:
            case Condition.Function.IsPlayerMovingIntoNewSpace:
            case Condition.Function.IsPlayersLastRiddenMount:
            case Condition.Function.IsPleasant:
            case Condition.Function.IsPoison:
            case Condition.Function.IsPowerAttacking:
            case Condition.Function.IsProtected:
            case Condition.Function.IsPS3:
            case Condition.Function.IsRaining:
            case Condition.Function.IsRecoiling:
            case Condition.Function.IsRidingMount:
            case Condition.Function.IsRotating:
            case Condition.Function.IsRunning:
            case Condition.Function.IsSceneActionComplete:
            case Condition.Function.IsScenePackageRunning:
            case Condition.Function.IsScenePlaying:
            case Condition.Function.IsShieldOut:
            case Condition.Function.IsSmallBump:
            case Condition.Function.IsSneaking:
            case Condition.Function.IsSnowing:
            case Condition.Function.IsSpellTarget:
            case Condition.Function.IsSprinting:
            case Condition.Function.IsStaggered:
            case Condition.Function.IsSwimming:
            case Condition.Function.IsTalking:
            case Condition.Function.IsTorchOut:
            case Condition.Function.IsTrespassing:
            case Condition.Function.IsTurning:
            case Condition.Function.IsUndead:
            case Condition.Function.IsUnique:
            case Condition.Function.IsUnlockedDoor:
            case Condition.Function.IsWardState:
            case Condition.Function.IsWarningAbout:
            case Condition.Function.IsWaterObject:
            case Condition.Function.IsWeaponInList:
            case Condition.Function.IsWeaponMagicOut:
            case Condition.Function.IsWeaponSkillType:
            case Condition.Function.IsWin32:
            case Condition.Function.IsXBox:
            case Condition.Function.LocAliasHasKeyword:
            case Condition.Function.LocAliasIsLocation:
            case Condition.Function.LocationHasKeyword:
            case Condition.Function.LocationHasRefType:
            case Condition.Function.PlayerKnows:
            case Condition.Function.SameRace:
            case Condition.Function.SameRaceAsPC:
            case Condition.Function.SameSex:
            case Condition.Function.SameSexAsPC:
            case Condition.Function.ShouldAttackKill:
            case Condition.Function.SpellHasCastingPerk:
            case Condition.Function.SpellHasKeyword:
            case Condition.Function.WornHasKeyword:
                return ReturnType.Boolean;

            // UFloat
            // Most of these should not use ==
            case Condition.Function.GetActivatorHeight:
            case Condition.Function.GetDistance:
            case Condition.Function.GetItemHealthPercent:
            case Condition.Function.GetMovementSpeed:
            case Condition.Function.GetPathingCurrentSpeed:
            case Condition.Function.GetPathingTargetOffset:
            case Condition.Function.GetPathingTargetSpeed:
            case Condition.Function.GetRealHoursPassed:
            case Condition.Function.GetThreatRatio:
            case Condition.Function.GetTimeDead:
            case Condition.Function.GetVATSBackAreaFree:
            case Condition.Function.GetVATSBackTargetVisible:
            case Condition.Function.GetVATSFrontAreaFree:
            case Condition.Function.GetVATSFrontTargetVisible:
            case Condition.Function.GetVATSLeftAreaFree:
            case Condition.Function.GetVATSLeftTargetVisible:
            case Condition.Function.GetVATSRightAreaFree:
            case Condition.Function.GetVATSRightTargetVisible:
            case Condition.Function.GetVatsTargetHeight:
            case Condition.Function.GetWalkSpeed:
                return ReturnType.UnsignedFloat;

            // SFloat
            // Most of these should not use ==
            case Condition.Function.GetGlobalValue:
            case Condition.Function.GetGraphVariableFloat:
            case Condition.Function.GetKeywordDataForAlias:
            case Condition.Function.GetKeywordDataForCurrentLocation:
            case Condition.Function.GetKeywordDataForLocation:
            case Condition.Function.GetPos:
            case Condition.Function.GetStartingAngle:
            case Condition.Function.GetStartingPos:
            case Condition.Function.GetTargetHeight:
            case Condition.Function.GetVelocity:
            case Condition.Function.GetWithinDistance:
                return ReturnType.SignedFloat;

            // UInt
            // Some of these should not use ==
            case Condition.Function.GetActorsInHigh:
            case Condition.Function.GetActorWarmth:
            case Condition.Function.GetClothingValue:
            case Condition.Function.GetCombatGroupMemberCount:
            case Condition.Function.GetCrimeGold:
            case Condition.Function.GetCrimeGoldNonviolent:
            case Condition.Function.GetCrimeGoldViolent:
            case Condition.Function.GetDaysInJail:
            case Condition.Function.GetDeadCount:
            case Condition.Function.GetDestructionStage:
            case Condition.Function.GetDetected:
            case Condition.Function.GetGold:
            case Condition.Function.GetGroupMemberCount:
            case Condition.Function.GetGroupTargetCount:
            case Condition.Function.GetItemCount:
            case Condition.Function.GetKeywordItemCount:
            case Condition.Function.GetLevel:
            case Condition.Function.GetLocAliasRefTypeAliveCount:
            case Condition.Function.GetLocAliasRefTypeDeadCount:
            case Condition.Function.GetPlayerTeammateCount:
            case Condition.Function.GetRefTypeAliveCount:
            case Condition.Function.GetRefTypeDeadCount:
            case Condition.Function.GetShouldAttack:
            case Condition.Function.GetShouldHelp:
            case Condition.Function.GetStage:
            case Condition.Function.WornApparelHasKeywordCount:
                return ReturnType.UnsignedInt;

            // SInt
            case Condition.Function.GetGraphVariableInt:
                return ReturnType.SignedInt;

            // Edge cases. These could be extended to be more specific

            // Bool, int, or float depending on papyrus
            case Condition.Function.GetVMScriptVariable:
            case Condition.Function.GetVMQuestVariable:
                return ReturnType.SignedFloat;

            // Depends on target variable
            case Condition.Function.GetNumericPackageData:
                return ReturnType.SignedFloat;

            // Depends on addtional params/it's complicated
            case Condition.Function.GetEventData:
            case Condition.Function.GetVATSValue:
                return ReturnType.SignedFloat;

            // Float or enum depending on AV
            case Condition.Function.GetActorValue:
            case Condition.Function.GetBaseActorValue:
            case Condition.Function.GetPermanentActorValue:
                return ReturnType.SignedFloat;

            // 0..99 inclusive int percentage. Should not use ==
            case Condition.Function.GetRandomPercent:
                return ReturnType.UnsignedInt;

            // 0..100 int. Should not use ==
            case Condition.Function.GetDialogueEmotionValue:
                return ReturnType.UnsignedInt;

            // 0..150
            case Condition.Function.GetLightLevel:
                return ReturnType.UnsignedFloat;

            // Enums
            case Condition.Function.GetAttackState:
            case Condition.Function.GetCombatState:
            case Condition.Function.GetCurrentAIProcedure:
            case Condition.Function.GetCurrentCastingType:
            case Condition.Function.GetCurrentDeliveryType:
            case Condition.Function.GetCurrentShoutVariation:
            case Condition.Function.GetDayOfWeek:
            case Condition.Function.GetDialogueEmotion:
            case Condition.Function.GetEquippedItemType:
            case Condition.Function.GetFactionCombatReaction:
            case Condition.Function.GetFactionRelation:
            case Condition.Function.GetFlyingState:
            case Condition.Function.GetFriendHit:
            case Condition.Function.GetHighestRelationshipRank:
            case Condition.Function.GetKnockedStateEnum:
            case Condition.Function.GetLockLevel:
            case Condition.Function.GetLowestRelationshipRank:
            case Condition.Function.GetMapMarkerVisible:
            case Condition.Function.GetMovementDirection:
            case Condition.Function.GetOpenState:
            case Condition.Function.GetRelationshipRank:
            case Condition.Function.GetReplacedItemType:
            case Condition.Function.GetSitting:
            case Condition.Function.GetSleeping:
            case Condition.Function.GetTrespassWarningLevel: // 0..iGuardWarnings
            case Condition.Function.GetVATSMode:
            case Condition.Function.GetWeaponAnimType:
            case Condition.Function.IsWeaponOut:
                return ReturnType.SignedInt;

            // 0..24. Should not use ==
            case Condition.Function.GetCurrentTime:
                return ReturnType.UnsignedFloat;

            // Depends on number of ranks in faction. -2..max rank
            case Condition.Function.GetFactionRank:
            case Condition.Function.GetFactionRankDifference:
                return ReturnType.SignedInt;

            // 0..1. Should not use ==
            case Condition.Function.GetActorValuePercent:
            case Condition.Function.GetCurrentWeatherPercent:
            case Condition.Function.GetHealthPercentage:
            case Condition.Function.GetStaminaPercentage:
            case Condition.Function.GetWindSpeed:
                return ReturnType.UnsignedFloat;

            // Angle, 0..360. Should not use ==
            case Condition.Function.GetAngle:
            case Condition.Function.GetLastBumpDirection:
            case Condition.Function.GetPathingCurrentSpeedAngle:
            case Condition.Function.GetPathingTargetAngleOffset:
            case Condition.Function.GetPathingTargetSpeedAngle:
            case Condition.Function.GetRelativeAngle:
                return ReturnType.UnsignedFloat;

            // -180..180
            case Condition.Function.GetHeadingAngle:
                return ReturnType.SignedFloat;

            default:
                return ReturnType.UnsupportedFunction;
        }
    }
}
