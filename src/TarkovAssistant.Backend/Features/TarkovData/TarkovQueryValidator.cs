using System.Collections.Frozen;
using TarkovAssistant.Backend.Infrastructure;

namespace TarkovAssistant.Backend.Features.TarkovData;

internal static class TarkovQueryValidator
{
    private static readonly FrozenSet<string> Languages = new[]
    {
        "cs", "de", "en", "es", "fr", "hu", "it", "ja", "ko", "pl", "pt", "ro", "ru", "sk", "tr", "zh"
    }.ToFrozenSet(StringComparer.Ordinal);

    private static readonly FrozenSet<string> GameModes = new[]
    {
        "regular", "pve"
    }.ToFrozenSet(StringComparer.Ordinal);

    private static readonly FrozenSet<string> ItemTypes = new[]
    {
        "ammo", "ammoBox", "any", "armor", "armorPlate", "backpack", "barter", "container", "glasses",
        "grenade", "gun", "headphones", "helmet", "injectors", "keys", "markedOnly", "meds", "mods",
        "noFlea", "pistolGrip", "poster", "preset", "provisions", "rig", "specialSlot", "suppressor", "wearable"
    }.ToFrozenSet(StringComparer.Ordinal);

    internal static void Validate(
        string language,
        string gameMode,
        int limit,
        int offset,
        string? itemType,
        bool allowItemType)
    {
        ValidatePage(language, gameMode, limit, offset);

        if (itemType is not null && (!allowItemType || !ItemTypes.Contains(itemType)))
        {
            throw new RequestValidationException($"Unsupported item type '{itemType}'.");
        }
    }

    internal static void ValidatePage(string language, string gameMode, int limit, int offset)
    {
        ValidateLocale(language, gameMode);

        if (limit is < 1 or > 500)
        {
            throw new RequestValidationException("Limit must be between 1 and 500.");
        }

        if (offset < 0)
        {
            throw new RequestValidationException("Offset cannot be negative.");
        }
    }

    internal static void ValidateLocale(string language, string gameMode)
    {
        if (!Languages.Contains(language))
        {
            throw new RequestValidationException($"Unsupported language '{language}'.");
        }

        if (!GameModes.Contains(gameMode))
        {
            throw new RequestValidationException($"Unsupported game mode '{gameMode}'.");
        }
    }
}
