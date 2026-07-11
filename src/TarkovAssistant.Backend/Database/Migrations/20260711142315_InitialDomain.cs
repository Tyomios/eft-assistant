using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarkovAssistant.Backend.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialDomain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "calibers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    NormalizedName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ExternalSource = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ExternalId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    SourceUpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_calibers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "catalog_versions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    PublishedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ContentHash = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catalog_versions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "hideout_stations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    TarkovDataId = table.Column<int>(type: "integer", nullable: true),
                    ExternalSource = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ExternalId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    SourceUpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hideout_stations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "import_runs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ImportName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    StartedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    FinishedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ImportedRecordCount = table.Column<int>(type: "integer", nullable: false),
                    FailureReason = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_import_runs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "item_categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    NormalizedName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    MinimumFleaLevel = table.Column<int>(type: "integer", nullable: true),
                    ExternalSource = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ExternalId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    SourceUpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    NormalizedName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    ShortName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(8192)", maxLength: 8192, nullable: true),
                    BasePrice = table.Column<int>(type: "integer", nullable: false),
                    Width = table.Column<int>(type: "integer", nullable: false),
                    Height = table.Column<int>(type: "integer", nullable: false),
                    BackgroundColor = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    WikiUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    WeightKilograms = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: true),
                    MinimumFleaLevel = table.Column<int>(type: "integer", nullable: true),
                    FleaMarketFee = table.Column<int>(type: "integer", nullable: true),
                    ExternalSource = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ExternalId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    SourceUpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "maps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ExternalSource = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ExternalId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    SourceUpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_maps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vendors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ExternalSource = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ExternalId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    SourceUpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vendors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "hideout_station_levels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HideoutStationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    ConstructionTimeSeconds = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(8192)", maxLength: 8192, nullable: false),
                    TarkovDataId = table.Column<int>(type: "integer", nullable: true),
                    ExternalSource = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ExternalId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    SourceUpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hideout_station_levels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_hideout_station_levels_hideout_stations_HideoutStationId",
                        column: x => x.HideoutStationId,
                        principalTable: "hideout_stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "import_states",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ImportName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    LastAttemptStartedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastAttemptFinishedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastSucceededAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    FailureReason = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    CurrentRunId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_import_states", x => x.Id);
                    table.ForeignKey(
                        name: "FK_import_states_import_runs_CurrentRunId",
                        column: x => x.CurrentRunId,
                        principalTable: "import_runs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ammunition",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    CaliberId = table.Column<Guid>(type: "uuid", nullable: false),
                    WeightKilograms = table.Column<decimal>(type: "numeric(10,5)", precision: 10, scale: 5, nullable: false),
                    StackMaximumSize = table.Column<int>(type: "integer", nullable: false),
                    IsTracer = table.Column<bool>(type: "boolean", nullable: false),
                    TracerColor = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    AmmunitionType = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ProjectileCount = table.Column<int>(type: "integer", nullable: true),
                    Damage = table.Column<int>(type: "integer", nullable: false),
                    ArmorDamage = table.Column<int>(type: "integer", nullable: false),
                    FragmentationChance = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: false),
                    RicochetChance = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: false),
                    PenetrationChance = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: false),
                    PenetrationPower = table.Column<int>(type: "integer", nullable: false),
                    PenetrationPowerDeviation = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: true),
                    AccuracyModifier = table.Column<decimal>(type: "numeric(9,4)", precision: 9, scale: 4, nullable: true),
                    RecoilModifier = table.Column<decimal>(type: "numeric(9,4)", precision: 9, scale: 4, nullable: true),
                    InitialSpeedMetersPerSecond = table.Column<decimal>(type: "numeric(10,3)", precision: 10, scale: 3, nullable: true),
                    LightBleedModifier = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: false),
                    HeavyBleedModifier = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: false),
                    StaminaBurnPerDamage = table.Column<decimal>(type: "numeric(9,4)", precision: 9, scale: 4, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ammunition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ammunition_calibers_CaliberId",
                        column: x => x.CaliberId,
                        principalTable: "calibers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ammunition_items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "item_category_assignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_category_assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_item_category_assignments_item_categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "item_categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_item_category_assignments_items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "item_images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Kind = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    SourceUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    ContentHash = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    LocalPath = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    DownloadedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_item_images_items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "item_market_snapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Average24HourPrice = table.Column<int>(type: "integer", nullable: true),
                    LastLowPrice = table.Column<int>(type: "integer", nullable: true),
                    Low24HourPrice = table.Column<int>(type: "integer", nullable: true),
                    High24HourPrice = table.Column<int>(type: "integer", nullable: true),
                    ChangeLast48Hours = table.Column<decimal>(type: "numeric(14,4)", precision: 14, scale: 4, nullable: true),
                    ChangeLast48HoursPercent = table.Column<decimal>(type: "numeric(9,4)", precision: 9, scale: 4, nullable: true),
                    LastOfferCount = table.Column<int>(type: "integer", nullable: true),
                    ObservedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_market_snapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_item_market_snapshots_items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "item_types",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_types", x => x.Id);
                    table.ForeignKey(
                        name: "FK_item_types_items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "magazines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Ergonomics = table.Column<decimal>(type: "numeric(9,4)", precision: 9, scale: 4, nullable: true),
                    RecoilModifier = table.Column<decimal>(type: "numeric(9,4)", precision: 9, scale: 4, nullable: true),
                    Capacity = table.Column<int>(type: "integer", nullable: true),
                    LoadModifier = table.Column<decimal>(type: "numeric(9,4)", precision: 9, scale: 4, nullable: true),
                    AmmunitionCheckModifier = table.Column<decimal>(type: "numeric(9,4)", precision: 9, scale: 4, nullable: true),
                    MalfunctionChance = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_magazines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_magazines_items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "traders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrencyItemId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "character varying(8192)", maxLength: 8192, nullable: true),
                    ResetAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Discount = table.Column<decimal>(type: "numeric(9,4)", precision: 9, scale: 4, nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    HighResolutionImageUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    TarkovDataId = table.Column<int>(type: "integer", nullable: true),
                    ExternalSource = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ExternalId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    SourceUpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_traders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_traders_items_CurrencyItemId",
                        column: x => x.CurrencyItemId,
                        principalTable: "items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "item_prices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    VendorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Direction = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: true),
                    CurrencyCode = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    PriceRoubles = table.Column<int>(type: "integer", nullable: true),
                    ObservedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_prices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_item_prices_items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_item_prices_vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "hideout_item_requirements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HideoutStationLevelId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    ExternalSource = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ExternalId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    SourceUpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hideout_item_requirements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_hideout_item_requirements_hideout_station_levels_HideoutSta~",
                        column: x => x.HideoutStationLevelId,
                        principalTable: "hideout_station_levels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_hideout_item_requirements_items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "weapons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    CaliberId = table.Column<Guid>(type: "uuid", nullable: false),
                    DefaultAmmunitionId = table.Column<Guid>(type: "uuid", nullable: true),
                    EffectiveDistanceMeters = table.Column<int>(type: "integer", nullable: true),
                    Ergonomics = table.Column<decimal>(type: "numeric(9,4)", precision: 9, scale: 4, nullable: true),
                    FireRateRoundsPerMinute = table.Column<int>(type: "integer", nullable: true),
                    MaximumDurability = table.Column<int>(type: "integer", nullable: true),
                    VerticalRecoil = table.Column<int>(type: "integer", nullable: true),
                    HorizontalRecoil = table.Column<int>(type: "integer", nullable: true),
                    SightingRangeMeters = table.Column<int>(type: "integer", nullable: true),
                    DefaultWidth = table.Column<int>(type: "integer", nullable: true),
                    DefaultHeight = table.Column<int>(type: "integer", nullable: true),
                    DefaultErgonomics = table.Column<decimal>(type: "numeric(9,4)", precision: 9, scale: 4, nullable: true),
                    DefaultVerticalRecoil = table.Column<int>(type: "integer", nullable: true),
                    DefaultHorizontalRecoil = table.Column<int>(type: "integer", nullable: true),
                    DefaultWeightKilograms = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weapons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_weapons_ammunition_DefaultAmmunitionId",
                        column: x => x.DefaultAmmunitionId,
                        principalTable: "ammunition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_weapons_calibers_CaliberId",
                        column: x => x.CaliberId,
                        principalTable: "calibers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_weapons_items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "magazine_ammunition_compatibilities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MagazineId = table.Column<Guid>(type: "uuid", nullable: false),
                    AmmunitionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_magazine_ammunition_compatibilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_magazine_ammunition_compatibilities_ammunition_AmmunitionId",
                        column: x => x.AmmunitionId,
                        principalTable: "ammunition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_magazine_ammunition_compatibilities_magazines_MagazineId",
                        column: x => x.MagazineId,
                        principalTable: "magazines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "quests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TraderId = table.Column<Guid>(type: "uuid", nullable: false),
                    MapId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    NormalizedName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    Experience = table.Column<int>(type: "integer", nullable: false),
                    MinimumPlayerLevel = table.Column<int>(type: "integer", nullable: true),
                    WikiUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    ImageUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    FactionName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    IsKappaRequired = table.Column<bool>(type: "boolean", nullable: true),
                    IsLightkeeperRequired = table.Column<bool>(type: "boolean", nullable: true),
                    ExternalSource = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ExternalId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    SourceUpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_quests_maps_MapId",
                        column: x => x.MapId,
                        principalTable: "maps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_quests_traders_TraderId",
                        column: x => x.TraderId,
                        principalTable: "traders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "trader_levels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TraderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    RequiredPlayerLevel = table.Column<int>(type: "integer", nullable: false),
                    RequiredReputation = table.Column<decimal>(type: "numeric(9,4)", precision: 9, scale: 4, nullable: false),
                    RequiredCommerce = table.Column<int>(type: "integer", nullable: false),
                    PayRate = table.Column<decimal>(type: "numeric(9,4)", precision: 9, scale: 4, nullable: false),
                    InsuranceRate = table.Column<decimal>(type: "numeric(9,4)", precision: 9, scale: 4, nullable: true),
                    RepairCostMultiplier = table.Column<decimal>(type: "numeric(9,4)", precision: 9, scale: 4, nullable: true),
                    ExternalSource = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ExternalId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    SourceUpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trader_levels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_trader_levels_traders_TraderId",
                        column: x => x.TraderId,
                        principalTable: "traders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "weapon_ammunition_compatibilities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WeaponId = table.Column<Guid>(type: "uuid", nullable: false),
                    AmmunitionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weapon_ammunition_compatibilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_weapon_ammunition_compatibilities_ammunition_AmmunitionId",
                        column: x => x.AmmunitionId,
                        principalTable: "ammunition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_weapon_ammunition_compatibilities_weapons_WeaponId",
                        column: x => x.WeaponId,
                        principalTable: "weapons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "weapon_fire_modes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WeaponId = table.Column<Guid>(type: "uuid", nullable: false),
                    Mode = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weapon_fire_modes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_weapon_fire_modes_weapons_WeaponId",
                        column: x => x.WeaponId,
                        principalTable: "weapons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "quest_objectives",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestId = table.Column<Guid>(type: "uuid", nullable: false),
                    ObjectiveType = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: false),
                    IsOptional = table.Column<bool>(type: "boolean", nullable: false),
                    RequiredCount = table.Column<int>(type: "integer", nullable: true),
                    MustBeFoundInRaid = table.Column<bool>(type: "boolean", nullable: true),
                    MinimumDogTagLevel = table.Column<int>(type: "integer", nullable: true),
                    MinimumDurability = table.Column<int>(type: "integer", nullable: true),
                    MaximumDurability = table.Column<int>(type: "integer", nullable: true),
                    ExternalSource = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ExternalId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    SourceUpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quest_objectives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_quest_objectives_quests_QuestId",
                        column: x => x.QuestId,
                        principalTable: "quests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "quest_objective_items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestObjectiveId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quest_objective_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_quest_objective_items_items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_quest_objective_items_quest_objectives_QuestObjectiveId",
                        column: x => x.QuestObjectiveId,
                        principalTable: "quest_objectives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "quest_objective_maps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestObjectiveId = table.Column<Guid>(type: "uuid", nullable: false),
                    MapId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quest_objective_maps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_quest_objective_maps_maps_MapId",
                        column: x => x.MapId,
                        principalTable: "maps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_quest_objective_maps_quest_objectives_QuestObjectiveId",
                        column: x => x.QuestObjectiveId,
                        principalTable: "quest_objectives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ammunition_CaliberId",
                table: "ammunition",
                column: "CaliberId");

            migrationBuilder.CreateIndex(
                name: "IX_ammunition_ItemId",
                table: "ammunition",
                column: "ItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_calibers_ExternalSource_ExternalId",
                table: "calibers",
                columns: new[] { "ExternalSource", "ExternalId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_calibers_NormalizedName",
                table: "calibers",
                column: "NormalizedName");

            migrationBuilder.CreateIndex(
                name: "IX_catalog_versions_ContentHash",
                table: "catalog_versions",
                column: "ContentHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_catalog_versions_Version",
                table: "catalog_versions",
                column: "Version",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_hideout_item_requirements_ExternalSource_ExternalId",
                table: "hideout_item_requirements",
                columns: new[] { "ExternalSource", "ExternalId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_hideout_item_requirements_HideoutStationLevelId_ItemId",
                table: "hideout_item_requirements",
                columns: new[] { "HideoutStationLevelId", "ItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_hideout_item_requirements_ItemId",
                table: "hideout_item_requirements",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_hideout_station_levels_ExternalSource_ExternalId",
                table: "hideout_station_levels",
                columns: new[] { "ExternalSource", "ExternalId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_hideout_station_levels_HideoutStationId_Level",
                table: "hideout_station_levels",
                columns: new[] { "HideoutStationId", "Level" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_hideout_stations_ExternalSource_ExternalId",
                table: "hideout_stations",
                columns: new[] { "ExternalSource", "ExternalId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_import_runs_ImportName_StartedAt",
                table: "import_runs",
                columns: new[] { "ImportName", "StartedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_import_states_CurrentRunId",
                table: "import_states",
                column: "CurrentRunId");

            migrationBuilder.CreateIndex(
                name: "IX_import_states_ImportName",
                table: "import_states",
                column: "ImportName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_item_categories_ExternalSource_ExternalId",
                table: "item_categories",
                columns: new[] { "ExternalSource", "ExternalId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_item_category_assignments_CategoryId",
                table: "item_category_assignments",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_item_category_assignments_ItemId_CategoryId",
                table: "item_category_assignments",
                columns: new[] { "ItemId", "CategoryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_item_images_ItemId_Kind",
                table: "item_images",
                columns: new[] { "ItemId", "Kind" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_item_market_snapshots_ItemId_ObservedAt",
                table: "item_market_snapshots",
                columns: new[] { "ItemId", "ObservedAt" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_item_prices_ItemId_VendorId_Direction_ObservedAt",
                table: "item_prices",
                columns: new[] { "ItemId", "VendorId", "Direction", "ObservedAt" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_item_prices_VendorId",
                table: "item_prices",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_item_types_ItemId_Value",
                table: "item_types",
                columns: new[] { "ItemId", "Value" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_items_ExternalSource_ExternalId",
                table: "items",
                columns: new[] { "ExternalSource", "ExternalId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_items_NormalizedName",
                table: "items",
                column: "NormalizedName");

            migrationBuilder.CreateIndex(
                name: "IX_magazine_ammunition_compatibilities_AmmunitionId",
                table: "magazine_ammunition_compatibilities",
                column: "AmmunitionId");

            migrationBuilder.CreateIndex(
                name: "IX_magazine_ammunition_compatibilities_MagazineId_AmmunitionId",
                table: "magazine_ammunition_compatibilities",
                columns: new[] { "MagazineId", "AmmunitionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_magazines_ItemId",
                table: "magazines",
                column: "ItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_maps_ExternalSource_ExternalId",
                table: "maps",
                columns: new[] { "ExternalSource", "ExternalId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_quest_objective_items_ItemId",
                table: "quest_objective_items",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_quest_objective_items_QuestObjectiveId_ItemId",
                table: "quest_objective_items",
                columns: new[] { "QuestObjectiveId", "ItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_quest_objective_maps_MapId",
                table: "quest_objective_maps",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_quest_objective_maps_QuestObjectiveId_MapId",
                table: "quest_objective_maps",
                columns: new[] { "QuestObjectiveId", "MapId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_quest_objectives_ExternalSource_ExternalId",
                table: "quest_objectives",
                columns: new[] { "ExternalSource", "ExternalId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_quest_objectives_QuestId",
                table: "quest_objectives",
                column: "QuestId");

            migrationBuilder.CreateIndex(
                name: "IX_quests_ExternalSource_ExternalId",
                table: "quests",
                columns: new[] { "ExternalSource", "ExternalId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_quests_MapId",
                table: "quests",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_quests_TraderId",
                table: "quests",
                column: "TraderId");

            migrationBuilder.CreateIndex(
                name: "IX_trader_levels_ExternalSource_ExternalId",
                table: "trader_levels",
                columns: new[] { "ExternalSource", "ExternalId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_trader_levels_TraderId_Level",
                table: "trader_levels",
                columns: new[] { "TraderId", "Level" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_traders_CurrencyItemId",
                table: "traders",
                column: "CurrencyItemId");

            migrationBuilder.CreateIndex(
                name: "IX_traders_ExternalSource_ExternalId",
                table: "traders",
                columns: new[] { "ExternalSource", "ExternalId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_vendors_ExternalSource_ExternalId",
                table: "vendors",
                columns: new[] { "ExternalSource", "ExternalId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_vendors_NormalizedName",
                table: "vendors",
                column: "NormalizedName");

            migrationBuilder.CreateIndex(
                name: "IX_weapon_ammunition_compatibilities_AmmunitionId",
                table: "weapon_ammunition_compatibilities",
                column: "AmmunitionId");

            migrationBuilder.CreateIndex(
                name: "IX_weapon_ammunition_compatibilities_WeaponId_AmmunitionId",
                table: "weapon_ammunition_compatibilities",
                columns: new[] { "WeaponId", "AmmunitionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_weapon_fire_modes_WeaponId_Mode",
                table: "weapon_fire_modes",
                columns: new[] { "WeaponId", "Mode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_weapons_CaliberId",
                table: "weapons",
                column: "CaliberId");

            migrationBuilder.CreateIndex(
                name: "IX_weapons_DefaultAmmunitionId",
                table: "weapons",
                column: "DefaultAmmunitionId");

            migrationBuilder.CreateIndex(
                name: "IX_weapons_ItemId",
                table: "weapons",
                column: "ItemId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "catalog_versions");

            migrationBuilder.DropTable(
                name: "hideout_item_requirements");

            migrationBuilder.DropTable(
                name: "import_states");

            migrationBuilder.DropTable(
                name: "item_category_assignments");

            migrationBuilder.DropTable(
                name: "item_images");

            migrationBuilder.DropTable(
                name: "item_market_snapshots");

            migrationBuilder.DropTable(
                name: "item_prices");

            migrationBuilder.DropTable(
                name: "item_types");

            migrationBuilder.DropTable(
                name: "magazine_ammunition_compatibilities");

            migrationBuilder.DropTable(
                name: "quest_objective_items");

            migrationBuilder.DropTable(
                name: "quest_objective_maps");

            migrationBuilder.DropTable(
                name: "trader_levels");

            migrationBuilder.DropTable(
                name: "weapon_ammunition_compatibilities");

            migrationBuilder.DropTable(
                name: "weapon_fire_modes");

            migrationBuilder.DropTable(
                name: "hideout_station_levels");

            migrationBuilder.DropTable(
                name: "import_runs");

            migrationBuilder.DropTable(
                name: "item_categories");

            migrationBuilder.DropTable(
                name: "vendors");

            migrationBuilder.DropTable(
                name: "magazines");

            migrationBuilder.DropTable(
                name: "quest_objectives");

            migrationBuilder.DropTable(
                name: "weapons");

            migrationBuilder.DropTable(
                name: "hideout_stations");

            migrationBuilder.DropTable(
                name: "quests");

            migrationBuilder.DropTable(
                name: "ammunition");

            migrationBuilder.DropTable(
                name: "maps");

            migrationBuilder.DropTable(
                name: "traders");

            migrationBuilder.DropTable(
                name: "calibers");

            migrationBuilder.DropTable(
                name: "items");
        }
    }
}
