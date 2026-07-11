using Microsoft.EntityFrameworkCore;
using TarkovAssistant.Backend.Database.Entities;

namespace TarkovAssistant.Backend.Database;

internal sealed class TarkovAssistantDbContext(DbContextOptions<TarkovAssistantDbContext> options) : DbContext(options)
{
    internal DbSet<Item> Items => Set<Item>();

    internal DbSet<ItemCategory> ItemCategories => Set<ItemCategory>();

    internal DbSet<ItemCategoryAssignment> ItemCategoryAssignments => Set<ItemCategoryAssignment>();

    internal DbSet<ItemType> ItemTypes => Set<ItemType>();

    internal DbSet<ItemImage> ItemImages => Set<ItemImage>();

    internal DbSet<ItemMarketSnapshot> ItemMarketSnapshots => Set<ItemMarketSnapshot>();

    internal DbSet<Vendor> Vendors => Set<Vendor>();

    internal DbSet<ItemPrice> ItemPrices => Set<ItemPrice>();

    internal DbSet<Caliber> Calibers => Set<Caliber>();

    internal DbSet<Ammunition> Ammunition => Set<Ammunition>();

    internal DbSet<Weapon> Weapons => Set<Weapon>();

    internal DbSet<WeaponFireMode> WeaponFireModes => Set<WeaponFireMode>();

    internal DbSet<Magazine> Magazines => Set<Magazine>();

    internal DbSet<WeaponAmmunitionCompatibility> WeaponAmmunitionCompatibilities => Set<WeaponAmmunitionCompatibility>();

    internal DbSet<MagazineAmmunitionCompatibility> MagazineAmmunitionCompatibilities => Set<MagazineAmmunitionCompatibility>();

    internal DbSet<Trader> Traders => Set<Trader>();

    internal DbSet<TraderLevel> TraderLevels => Set<TraderLevel>();

    internal DbSet<Map> Maps => Set<Map>();

    internal DbSet<Quest> Quests => Set<Quest>();

    internal DbSet<QuestObjective> QuestObjectives => Set<QuestObjective>();

    internal DbSet<QuestObjectiveMap> QuestObjectiveMaps => Set<QuestObjectiveMap>();

    internal DbSet<QuestObjectiveItem> QuestObjectiveItems => Set<QuestObjectiveItem>();

    internal DbSet<HideoutStation> HideoutStations => Set<HideoutStation>();

    internal DbSet<HideoutStationLevel> HideoutStationLevels => Set<HideoutStationLevel>();

    internal DbSet<HideoutItemRequirement> HideoutItemRequirements => Set<HideoutItemRequirement>();

    internal DbSet<CatalogVersion> CatalogVersions => Set<CatalogVersion>();

    internal DbSet<ImportState> ImportStates => Set<ImportState>();

    internal DbSet<ImportRun> ImportRuns => Set<ImportRun>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        DatabaseModel.Configure(modelBuilder);
    }
}
