namespace TarkovAssistant.Backend.Integrations.TarkovDev;

internal static class TarkovDevQueries
{
    internal const string Items = $$"""
        query Items($lang: LanguageCode!, $gameMode: GameMode!, $limit: Int!, $offset: Int!, $type: ItemType!) {
          items(lang: $lang, gameMode: $gameMode, limit: $limit, offset: $offset, type: $type) {
            {{ItemFields}}
          }
        }
        """;

    internal const string Item = $$"""
        query Item($id: ID!, $lang: LanguageCode!, $gameMode: GameMode!) {
          item(id: $id, lang: $lang, gameMode: $gameMode) {
            {{ItemFields}}
          }
        }
        """;

    internal const string Ammo = """
        query Ammo($lang: LanguageCode!, $gameMode: GameMode!, $limit: Int!, $offset: Int!) {
          ammo(lang: $lang, gameMode: $gameMode, limit: $limit, offset: $offset) {
            item { id name normalizedName shortName iconLink gridImageLink }
            weight caliber stackMaxSize tracer tracerColor ammoType projectileCount damage armorDamage
            fragmentationChance ricochetChance penetrationChance penetrationPower penetrationPowerDeviation
            accuracyModifier recoilModifier initialSpeed lightBleedModifier heavyBleedModifier staminaBurnPerDamage
          }
        }
        """;

    internal const string Tasks = """
        query Tasks($lang: LanguageCode!, $gameMode: GameMode!, $limit: Int!, $offset: Int!) {
          tasks(lang: $lang, gameMode: $gameMode, limit: $limit, offset: $offset) {
            id name normalizedName experience minPlayerLevel wikiLink taskImageLink factionName kappaRequired lightkeeperRequired
            trader { id name normalizedName }
            map { id name normalizedName }
            objectives {
              __typename id type description optional
              maps { id name normalizedName }
              ... on TaskObjectiveItem {
                items { id name normalizedName shortName iconLink }
                count foundInRaid dogTagLevel minDurability maxDurability
              }
            }
          }
        }
        """;

    internal const string Traders = """
        query Traders($lang: LanguageCode!, $gameMode: GameMode!, $limit: Int!, $offset: Int!) {
          traders(lang: $lang, gameMode: $gameMode, limit: $limit, offset: $offset) {
            id name normalizedName description resetTime discount imageLink image4xLink tarkovDataId
            currency { id name normalizedName shortName }
            levels { id level requiredPlayerLevel requiredReputation requiredCommerce payRate insuranceRate repairCostMultiplier }
          }
        }
        """;

    internal const string HideoutStations = """
        query HideoutStations($lang: LanguageCode!, $gameMode: GameMode!, $limit: Int!, $offset: Int!) {
          hideoutStations(lang: $lang, gameMode: $gameMode, limit: $limit, offset: $offset) {
            id name normalizedName imageLink tarkovDataId
            levels {
              id level constructionTime description tarkovDataId
              itemRequirements { id count quantity item { id name normalizedName shortName iconLink } }
            }
          }
        }
        """;

    private const string ItemFields = """
        id name normalizedName shortName description basePrice updated width height backgroundColor
        iconLink gridImageLink baseImageLink inspectImageLink image512pxLink image8xLink wikiLink
        types avg24hPrice lastLowPrice low24hPrice high24hPrice changeLast48h changeLast48hPercent
        lastOfferCount weight minLevelForFlea fleaMarketFee
        categories { id name normalizedName imageLink minLevelForFlea }
        sellFor { vendor { name normalizedName } price currency priceRUB }
        buyFor { vendor { name normalizedName } price currency priceRUB }
        properties {
          __typename
          ... on ItemPropertiesWeapon {
            caliber effectiveDistance ergonomics fireModes fireRate maxDurability recoilVertical recoilHorizontal
            sightingRange defaultWidth defaultHeight defaultErgonomics defaultRecoilVertical defaultRecoilHorizontal defaultWeight
            defaultAmmo { id name normalizedName shortName }
            allowedAmmo { id name normalizedName shortName }
          }
          ... on ItemPropertiesMagazine {
            ergonomics recoilModifier capacity loadModifier ammoCheckModifier malfunctionChance
            allowedAmmo { id name normalizedName shortName }
          }
        }
        """;
}
