using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarkovAssistant.Backend.Database.Entities;

namespace TarkovAssistant.Backend.Database;

internal static class DatabaseModel
{
    internal static void Configure(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        ConfigureItems(modelBuilder);
        ConfigureCommerce(modelBuilder);
        ConfigureWeapons(modelBuilder);
        ConfigureWorld(modelBuilder);
        ConfigureProgression(modelBuilder);
        ConfigureImports(modelBuilder);
    }

    private static void ConfigureItems(ModelBuilder modelBuilder)
    {
        var items = modelBuilder.Entity<Item>();
        ConfigureSourceEntity(items, "items");
        items.Property(item => item.Name).HasMaxLength(512);
        items.Property(item => item.NormalizedName).HasMaxLength(512);
        items.Property(item => item.ShortName).HasMaxLength(128);
        items.Property(item => item.Description).HasMaxLength(8_192);
        items.Property(item => item.BackgroundColor).HasMaxLength(32);
        items.Property(item => item.WikiUrl).HasMaxLength(2_048);
        items.Property(item => item.WeightKilograms).HasPrecision(10, 4);
        items.HasIndex(item => item.NormalizedName);

        var categories = modelBuilder.Entity<ItemCategory>();
        ConfigureSourceEntity(categories, "item_categories");
        categories.Property(category => category.Name).HasMaxLength(512);
        categories.Property(category => category.NormalizedName).HasMaxLength(512);
        categories.Property(category => category.ImageUrl).HasMaxLength(2_048);

        var assignments = modelBuilder.Entity<ItemCategoryAssignment>();
        ConfigureEntity(assignments, "item_category_assignments");
        assignments.HasIndex(assignment => new { assignment.ItemId, assignment.CategoryId }).IsUnique();
        assignments.HasOne<ItemCategory>().WithMany().HasForeignKey(assignment => assignment.CategoryId).OnDelete(DeleteBehavior.Cascade);
        assignments.HasOne<Item>().WithMany().HasForeignKey(assignment => assignment.ItemId).OnDelete(DeleteBehavior.Cascade);

        var types = modelBuilder.Entity<ItemType>();
        ConfigureEntity(types, "item_types");
        types.Property(type => type.Value).HasMaxLength(128);
        types.HasIndex(type => new { type.ItemId, type.Value }).IsUnique();
        types.HasOne<Item>().WithMany().HasForeignKey(type => type.ItemId).OnDelete(DeleteBehavior.Cascade);

        var images = modelBuilder.Entity<ItemImage>();
        ConfigureEntity(images, "item_images");
        images.Property(image => image.Kind).HasConversion<string>().HasMaxLength(32);
        images.Property(image => image.SourceUrl).HasMaxLength(2_048);
        images.Property(image => image.ContentHash).HasMaxLength(128);
        images.Property(image => image.LocalPath).HasMaxLength(1_024);
        images.HasIndex(image => new { image.ItemId, image.Kind }).IsUnique();
        images.HasOne<Item>().WithMany().HasForeignKey(image => image.ItemId).OnDelete(DeleteBehavior.Cascade);

        var snapshots = modelBuilder.Entity<ItemMarketSnapshot>();
        ConfigureEntity(snapshots, "item_market_snapshots");
        snapshots.Property(snapshot => snapshot.ChangeLast48Hours).HasPrecision(14, 4);
        snapshots.Property(snapshot => snapshot.ChangeLast48HoursPercent).HasPrecision(9, 4);
        snapshots.HasIndex(snapshot => new { snapshot.ItemId, snapshot.ObservedAt }).IsUnique();
        snapshots.HasOne<Item>().WithMany().HasForeignKey(snapshot => snapshot.ItemId).OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureCommerce(ModelBuilder modelBuilder)
    {
        var vendors = modelBuilder.Entity<Vendor>();
        ConfigureSourceEntity(vendors, "vendors");
        vendors.Property(vendor => vendor.Name).HasMaxLength(256);
        vendors.Property(vendor => vendor.NormalizedName).HasMaxLength(256);
        vendors.HasIndex(vendor => vendor.NormalizedName);

        var prices = modelBuilder.Entity<ItemPrice>();
        ConfigureEntity(prices, "item_prices");
        prices.Property(price => price.Direction).HasConversion<string>().HasMaxLength(16);
        prices.Property(price => price.CurrencyCode).HasMaxLength(32);
        prices.HasIndex(price => new { price.ItemId, price.VendorId, price.Direction, price.ObservedAt }).IsUnique();
        prices.HasOne<Item>().WithMany().HasForeignKey(price => price.ItemId).OnDelete(DeleteBehavior.Cascade);
        prices.HasOne<Vendor>().WithMany().HasForeignKey(price => price.VendorId).OnDelete(DeleteBehavior.Restrict);

        var traders = modelBuilder.Entity<Trader>();
        ConfigureSourceEntity(traders, "traders");
        traders.Property(trader => trader.Name).HasMaxLength(256);
        traders.Property(trader => trader.NormalizedName).HasMaxLength(256);
        traders.Property(trader => trader.Description).HasMaxLength(8_192);
        traders.Property(trader => trader.Discount).HasPrecision(9, 4);
        traders.Property(trader => trader.ImageUrl).HasMaxLength(2_048);
        traders.Property(trader => trader.HighResolutionImageUrl).HasMaxLength(2_048);
        traders.HasOne<Item>().WithMany().HasForeignKey(trader => trader.CurrencyItemId).OnDelete(DeleteBehavior.Restrict);

        var levels = modelBuilder.Entity<TraderLevel>();
        ConfigureSourceEntity(levels, "trader_levels");
        levels.Property(level => level.RequiredReputation).HasPrecision(9, 4);
        levels.Property(level => level.PayRate).HasPrecision(9, 4);
        levels.Property(level => level.InsuranceRate).HasPrecision(9, 4);
        levels.Property(level => level.RepairCostMultiplier).HasPrecision(9, 4);
        levels.HasIndex(level => new { level.TraderId, level.Level }).IsUnique();
        levels.HasOne<Trader>().WithMany().HasForeignKey(level => level.TraderId).OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureWeapons(ModelBuilder modelBuilder)
    {
        var calibers = modelBuilder.Entity<Caliber>();
        ConfigureSourceEntity(calibers, "calibers");
        calibers.Property(caliber => caliber.Name).HasMaxLength(128);
        calibers.Property(caliber => caliber.NormalizedName).HasMaxLength(128);
        calibers.HasIndex(caliber => caliber.NormalizedName);

        var ammunition = modelBuilder.Entity<Ammunition>();
        ConfigureEntity(ammunition, "ammunition");
        ammunition.Property(ammo => ammo.WeightKilograms).HasPrecision(10, 5);
        ammunition.Property(ammo => ammo.TracerColor).HasMaxLength(64);
        ammunition.Property(ammo => ammo.AmmunitionType).HasMaxLength(128);
        ammunition.Property(ammo => ammo.FragmentationChance).HasPrecision(9, 6);
        ammunition.Property(ammo => ammo.RicochetChance).HasPrecision(9, 6);
        ammunition.Property(ammo => ammo.PenetrationChance).HasPrecision(9, 6);
        ammunition.Property(ammo => ammo.PenetrationPowerDeviation).HasPrecision(9, 6);
        ammunition.Property(ammo => ammo.AccuracyModifier).HasPrecision(9, 4);
        ammunition.Property(ammo => ammo.RecoilModifier).HasPrecision(9, 4);
        ammunition.Property(ammo => ammo.InitialSpeedMetersPerSecond).HasPrecision(10, 3);
        ammunition.Property(ammo => ammo.LightBleedModifier).HasPrecision(9, 6);
        ammunition.Property(ammo => ammo.HeavyBleedModifier).HasPrecision(9, 6);
        ammunition.Property(ammo => ammo.StaminaBurnPerDamage).HasPrecision(9, 4);
        ammunition.HasIndex(ammo => ammo.ItemId).IsUnique();
        ammunition.HasOne<Item>().WithOne().HasForeignKey<Ammunition>(ammo => ammo.ItemId).OnDelete(DeleteBehavior.Cascade);
        ammunition.HasOne<Caliber>().WithMany().HasForeignKey(ammo => ammo.CaliberId).OnDelete(DeleteBehavior.Restrict);

        var weapons = modelBuilder.Entity<Weapon>();
        ConfigureEntity(weapons, "weapons");
        weapons.Property(weapon => weapon.Ergonomics).HasPrecision(9, 4);
        weapons.Property(weapon => weapon.DefaultErgonomics).HasPrecision(9, 4);
        weapons.Property(weapon => weapon.DefaultWeightKilograms).HasPrecision(10, 4);
        weapons.HasIndex(weapon => weapon.ItemId).IsUnique();
        weapons.HasOne<Item>().WithOne().HasForeignKey<Weapon>(weapon => weapon.ItemId).OnDelete(DeleteBehavior.Cascade);
        weapons.HasOne<Caliber>().WithMany().HasForeignKey(weapon => weapon.CaliberId).OnDelete(DeleteBehavior.Restrict);
        weapons.HasOne<Ammunition>().WithMany().HasForeignKey(weapon => weapon.DefaultAmmunitionId).OnDelete(DeleteBehavior.Restrict);

        var fireModes = modelBuilder.Entity<WeaponFireMode>();
        ConfigureEntity(fireModes, "weapon_fire_modes");
        fireModes.Property(mode => mode.Mode).HasMaxLength(64);
        fireModes.HasIndex(mode => new { mode.WeaponId, mode.Mode }).IsUnique();
        fireModes.HasOne<Weapon>().WithMany().HasForeignKey(mode => mode.WeaponId).OnDelete(DeleteBehavior.Cascade);

        var magazines = modelBuilder.Entity<Magazine>();
        ConfigureEntity(magazines, "magazines");
        magazines.Property(magazine => magazine.Ergonomics).HasPrecision(9, 4);
        magazines.Property(magazine => magazine.RecoilModifier).HasPrecision(9, 4);
        magazines.Property(magazine => magazine.LoadModifier).HasPrecision(9, 4);
        magazines.Property(magazine => magazine.AmmunitionCheckModifier).HasPrecision(9, 4);
        magazines.Property(magazine => magazine.MalfunctionChance).HasPrecision(9, 6);
        magazines.HasIndex(magazine => magazine.ItemId).IsUnique();
        magazines.HasOne<Item>().WithOne().HasForeignKey<Magazine>(magazine => magazine.ItemId).OnDelete(DeleteBehavior.Cascade);

        var weaponAmmo = modelBuilder.Entity<WeaponAmmunitionCompatibility>();
        ConfigureEntity(weaponAmmo, "weapon_ammunition_compatibilities");
        weaponAmmo.HasIndex(link => new { link.WeaponId, link.AmmunitionId }).IsUnique();
        weaponAmmo.HasOne<Weapon>().WithMany().HasForeignKey(link => link.WeaponId).OnDelete(DeleteBehavior.Cascade);
        weaponAmmo.HasOne<Ammunition>().WithMany().HasForeignKey(link => link.AmmunitionId).OnDelete(DeleteBehavior.Cascade);

        var magazineAmmo = modelBuilder.Entity<MagazineAmmunitionCompatibility>();
        ConfigureEntity(magazineAmmo, "magazine_ammunition_compatibilities");
        magazineAmmo.HasIndex(link => new { link.MagazineId, link.AmmunitionId }).IsUnique();
        magazineAmmo.HasOne<Magazine>().WithMany().HasForeignKey(link => link.MagazineId).OnDelete(DeleteBehavior.Cascade);
        magazineAmmo.HasOne<Ammunition>().WithMany().HasForeignKey(link => link.AmmunitionId).OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureWorld(ModelBuilder modelBuilder)
    {
        var maps = modelBuilder.Entity<Map>();
        ConfigureSourceEntity(maps, "maps");
        maps.Property(map => map.Name).HasMaxLength(256);
        maps.Property(map => map.NormalizedName).HasMaxLength(256);

        var quests = modelBuilder.Entity<Quest>();
        ConfigureSourceEntity(quests, "quests");
        quests.Property(quest => quest.Name).HasMaxLength(512);
        quests.Property(quest => quest.NormalizedName).HasMaxLength(512);
        quests.Property(quest => quest.WikiUrl).HasMaxLength(2_048);
        quests.Property(quest => quest.ImageUrl).HasMaxLength(2_048);
        quests.Property(quest => quest.FactionName).HasMaxLength(128);
        quests.HasOne<Trader>().WithMany().HasForeignKey(quest => quest.TraderId).OnDelete(DeleteBehavior.Restrict);
        quests.HasOne<Map>().WithMany().HasForeignKey(quest => quest.MapId).OnDelete(DeleteBehavior.SetNull);

        var objectives = modelBuilder.Entity<QuestObjective>();
        ConfigureSourceEntity(objectives, "quest_objectives");
        objectives.Property(objective => objective.ObjectiveType).HasMaxLength(128);
        objectives.Property(objective => objective.Description).HasMaxLength(4_096);
        objectives.HasOne<Quest>().WithMany().HasForeignKey(objective => objective.QuestId).OnDelete(DeleteBehavior.Cascade);

        var objectiveMaps = modelBuilder.Entity<QuestObjectiveMap>();
        ConfigureEntity(objectiveMaps, "quest_objective_maps");
        objectiveMaps.HasIndex(link => new { link.QuestObjectiveId, link.MapId }).IsUnique();
        objectiveMaps.HasOne<QuestObjective>().WithMany().HasForeignKey(link => link.QuestObjectiveId).OnDelete(DeleteBehavior.Cascade);
        objectiveMaps.HasOne<Map>().WithMany().HasForeignKey(link => link.MapId).OnDelete(DeleteBehavior.Cascade);

        var objectiveItems = modelBuilder.Entity<QuestObjectiveItem>();
        ConfigureEntity(objectiveItems, "quest_objective_items");
        objectiveItems.HasIndex(link => new { link.QuestObjectiveId, link.ItemId }).IsUnique();
        objectiveItems.HasOne<QuestObjective>().WithMany().HasForeignKey(link => link.QuestObjectiveId).OnDelete(DeleteBehavior.Cascade);
        objectiveItems.HasOne<Item>().WithMany().HasForeignKey(link => link.ItemId).OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureProgression(ModelBuilder modelBuilder)
    {
        var stations = modelBuilder.Entity<HideoutStation>();
        ConfigureSourceEntity(stations, "hideout_stations");
        stations.Property(station => station.Name).HasMaxLength(256);
        stations.Property(station => station.NormalizedName).HasMaxLength(256);
        stations.Property(station => station.ImageUrl).HasMaxLength(2_048);

        var levels = modelBuilder.Entity<HideoutStationLevel>();
        ConfigureSourceEntity(levels, "hideout_station_levels");
        levels.Property(level => level.Description).HasMaxLength(8_192);
        levels.HasIndex(level => new { level.HideoutStationId, level.Level }).IsUnique();
        levels.HasOne<HideoutStation>().WithMany().HasForeignKey(level => level.HideoutStationId).OnDelete(DeleteBehavior.Cascade);

        var requirements = modelBuilder.Entity<HideoutItemRequirement>();
        ConfigureSourceEntity(requirements, "hideout_item_requirements");
        requirements.HasIndex(requirement => new { requirement.HideoutStationLevelId, requirement.ItemId }).IsUnique();
        requirements.HasOne<HideoutStationLevel>().WithMany().HasForeignKey(requirement => requirement.HideoutStationLevelId).OnDelete(DeleteBehavior.Cascade);
        requirements.HasOne<Item>().WithMany().HasForeignKey(requirement => requirement.ItemId).OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureImports(ModelBuilder modelBuilder)
    {
        var versions = modelBuilder.Entity<CatalogVersion>();
        ConfigureEntity(versions, "catalog_versions");
        versions.Property(version => version.ContentHash).HasMaxLength(128);
        versions.HasIndex(version => version.Version).IsUnique();
        versions.HasIndex(version => version.ContentHash).IsUnique();

        var runs = modelBuilder.Entity<ImportRun>();
        ConfigureEntity(runs, "import_runs");
        runs.Property(run => run.ImportName).HasMaxLength(128);
        runs.Property(run => run.Status).HasConversion<string>().HasMaxLength(32);
        runs.Property(run => run.FailureReason).HasMaxLength(2_048);
        runs.HasIndex(run => new { run.ImportName, run.StartedAt });

        var states = modelBuilder.Entity<ImportState>();
        ConfigureEntity(states, "import_states");
        states.Property(state => state.ImportName).HasMaxLength(128);
        states.Property(state => state.Status).HasConversion<string>().HasMaxLength(32);
        states.Property(state => state.FailureReason).HasMaxLength(2_048);
        states.HasIndex(state => state.ImportName).IsUnique();
        states.HasOne<ImportRun>().WithMany().HasForeignKey(state => state.CurrentRunId).OnDelete(DeleteBehavior.SetNull);
    }

    private static void ConfigureEntity<TEntity>(EntityTypeBuilder<TEntity> builder, string tableName)
        where TEntity : Entity
    {
        builder.ToTable(tableName);

        foreach (var property in typeof(TEntity).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
            builder.Property(property.PropertyType, property.Name);
        }

        builder.HasKey(entity => entity.Id);
        builder.Property(entity => entity.Id).ValueGeneratedNever();
    }

    private static void ConfigureSourceEntity<TEntity>(EntityTypeBuilder<TEntity> builder, string tableName)
        where TEntity : SourceEntity
    {
        ConfigureEntity(builder, tableName);
        builder.Property(entity => entity.ExternalSource).HasMaxLength(64);
        builder.Property(entity => entity.ExternalId).HasMaxLength(256);
        builder.HasIndex(entity => new { entity.ExternalSource, entity.ExternalId }).IsUnique();
    }
}
