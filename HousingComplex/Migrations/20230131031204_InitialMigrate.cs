using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HousingComplex.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "m_image_house_type",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    filename = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    filesize = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    filepath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contenttype = table.Column<string>(name: "content_type", type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_image_house_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "m_role",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "m_spesification",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    bedrooms = table.Column<int>(type: "int", nullable: false),
                    bathrooms = table.Column<int>(type: "int", nullable: false),
                    kitchens = table.Column<int>(type: "int", nullable: false),
                    carport = table.Column<bool>(type: "bit", nullable: false),
                    swimmingpool = table.Column<bool>(name: "swimming_pool", type: "bit", nullable: false),
                    secondfloor = table.Column<bool>(name: "second_floor", type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_spesification", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "m_user_credential",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false),
                    roleid = table.Column<Guid>(name: "role_id", type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_user_credential", x => x.id);
                    table.ForeignKey(
                        name: "FK_m_user_credential_m_role_role_id",
                        column: x => x.roleid,
                        principalTable: "m_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "m_customer",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    firstname = table.Column<string>(name: "first_name", type: "Varchar(50)", nullable: false),
                    lastname = table.Column<string>(name: "last_name", type: "Varchar(50)", nullable: false),
                    city = table.Column<string>(type: "Varchar(50)", nullable: false),
                    postalcode = table.Column<string>(name: "postal_code", type: "Varchar(5)", nullable: false),
                    address = table.Column<string>(type: "Varchar(100)", nullable: false),
                    phonenumber = table.Column<string>(name: "phone_number", type: "Varchar(13)", nullable: false),
                    usercredentialid = table.Column<Guid>(name: "user_credential_id", type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_customer", x => x.id);
                    table.ForeignKey(
                        name: "FK_m_customer_m_user_credential_user_credential_id",
                        column: x => x.usercredentialid,
                        principalTable: "m_user_credential",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "m_developer",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "Varchar(100)", nullable: false),
                    phonenumber = table.Column<string>(name: "phone_number", type: "Varchar(13)", nullable: false),
                    usercredentialid = table.Column<Guid>(name: "user_credential_id", type: "uniqueidentifier", nullable: false),
                    address = table.Column<string>(type: "Varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_developer", x => x.id);
                    table.ForeignKey(
                        name: "FK_m_developer_m_user_credential_user_credential_id",
                        column: x => x.usercredentialid,
                        principalTable: "m_user_credential",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_purchase",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    transdate = table.Column<DateTime>(name: "trans_date", type: "datetime2", nullable: false),
                    customerid = table.Column<Guid>(name: "customer_id", type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_purchase", x => x.id);
                    table.ForeignKey(
                        name: "FK_t_purchase_m_customer_customer_id",
                        column: x => x.customerid,
                        principalTable: "m_customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "m_housing",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "Varchar(100)", nullable: false),
                    developerid = table.Column<Guid>(name: "developer_id", type: "uniqueidentifier", nullable: false),
                    address = table.Column<string>(type: "Varchar(100)", nullable: false),
                    opentime = table.Column<string>(name: "open_time", type: "Varchar(50)", nullable: false),
                    city = table.Column<string>(type: "Varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_housing", x => x.id);
                    table.ForeignKey(
                        name: "FK_m_housing_m_developer_developer_id",
                        column: x => x.developerid,
                        principalTable: "m_developer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "m_house_type",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    specificationid = table.Column<Guid>(name: "specification_id", type: "uniqueidentifier", nullable: false),
                    description = table.Column<string>(type: "Varchar(500)", nullable: false),
                    housingid = table.Column<Guid>(name: "housing_id", type: "uniqueidentifier", nullable: false),
                    price = table.Column<long>(type: "bigint", nullable: false),
                    stockunit = table.Column<int>(name: "stock_unit", type: "int", nullable: false),
                    imageid = table.Column<Guid>(name: "image_id", type: "uniqueidentifier", nullable: false),
                    ImageHouseTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_house_type", x => x.id);
                    table.ForeignKey(
                        name: "FK_m_house_type_m_housing_housing_id",
                        column: x => x.housingid,
                        principalTable: "m_housing",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_m_house_type_m_image_house_type_ImageHouseTypeId",
                        column: x => x.ImageHouseTypeId,
                        principalTable: "m_image_house_type",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_m_house_type_m_spesification_specification_id",
                        column: x => x.specificationid,
                        principalTable: "m_spesification",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "m_meet",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    meetdate = table.Column<string>(name: "meet_date", type: "nvarchar(max)", nullable: false),
                    ismeet = table.Column<bool>(name: "is_meet", type: "bit", nullable: false),
                    housingid = table.Column<Guid>(name: "housing_id", type: "uniqueidentifier", nullable: false),
                    customerid = table.Column<Guid>(name: "customer_id", type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_meet", x => x.id);
                    table.ForeignKey(
                        name: "FK_m_meet_m_customer_customer_id",
                        column: x => x.customerid,
                        principalTable: "m_customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_m_meet_m_housing_housing_id",
                        column: x => x.housingid,
                        principalTable: "m_housing",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "t_purchase_detail",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    housetypeid = table.Column<Guid>(name: "house_type_id", type: "uniqueidentifier", nullable: false),
                    purchaseid = table.Column<Guid>(name: "purchase_id", type: "uniqueidentifier", nullable: false),
                    housingid = table.Column<Guid>(name: "housing_id", type: "uniqueidentifier", nullable: false),
                    referencepg = table.Column<string>(name: "reference_pg", type: "nvarchar(max)", nullable: false),
                    nominal = table.Column<long>(type: "bigint", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_purchase_detail", x => x.id);
                    table.ForeignKey(
                        name: "FK_t_purchase_detail_m_house_type_house_type_id",
                        column: x => x.housetypeid,
                        principalTable: "m_house_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_purchase_detail_m_housing_housing_id",
                        column: x => x.housingid,
                        principalTable: "m_housing",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_t_purchase_detail_t_purchase_purchase_id",
                        column: x => x.purchaseid,
                        principalTable: "t_purchase",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_m_customer_phone_number",
                table: "m_customer",
                column: "phone_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_m_customer_user_credential_id",
                table: "m_customer",
                column: "user_credential_id");

            migrationBuilder.CreateIndex(
                name: "IX_m_developer_phone_number",
                table: "m_developer",
                column: "phone_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_m_developer_user_credential_id",
                table: "m_developer",
                column: "user_credential_id");

            migrationBuilder.CreateIndex(
                name: "IX_m_house_type_housing_id",
                table: "m_house_type",
                column: "housing_id");

            migrationBuilder.CreateIndex(
                name: "IX_m_house_type_ImageHouseTypeId",
                table: "m_house_type",
                column: "ImageHouseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_m_house_type_specification_id",
                table: "m_house_type",
                column: "specification_id");

            migrationBuilder.CreateIndex(
                name: "IX_m_housing_developer_id",
                table: "m_housing",
                column: "developer_id");

            migrationBuilder.CreateIndex(
                name: "IX_m_meet_customer_id",
                table: "m_meet",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_m_meet_housing_id",
                table: "m_meet",
                column: "housing_id");

            migrationBuilder.CreateIndex(
                name: "IX_m_user_credential_email",
                table: "m_user_credential",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_m_user_credential_role_id",
                table: "m_user_credential",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_purchase_customer_id",
                table: "t_purchase",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_purchase_detail_house_type_id",
                table: "t_purchase_detail",
                column: "house_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_purchase_detail_housing_id",
                table: "t_purchase_detail",
                column: "housing_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_purchase_detail_purchase_id",
                table: "t_purchase_detail",
                column: "purchase_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "m_meet");

            migrationBuilder.DropTable(
                name: "t_purchase_detail");

            migrationBuilder.DropTable(
                name: "m_house_type");

            migrationBuilder.DropTable(
                name: "t_purchase");

            migrationBuilder.DropTable(
                name: "m_housing");

            migrationBuilder.DropTable(
                name: "m_image_house_type");

            migrationBuilder.DropTable(
                name: "m_spesification");

            migrationBuilder.DropTable(
                name: "m_customer");

            migrationBuilder.DropTable(
                name: "m_developer");

            migrationBuilder.DropTable(
                name: "m_user_credential");

            migrationBuilder.DropTable(
                name: "m_role");
        }
    }
}
