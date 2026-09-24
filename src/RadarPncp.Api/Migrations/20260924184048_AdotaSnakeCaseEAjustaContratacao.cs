using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RadarPncp.Api.Migrations
{
    /// <inheritdoc />
    public partial class AdotaSnakeCaseEAjustaContratacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GovernmentUnits_GovernmentEntities_GovernmentEntityId",
                table: "GovernmentUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_Procurements_GovernmentUnits_GovernmentUnitId",
                table: "Procurements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Procurements",
                table: "Procurements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GovernmentUnits",
                table: "GovernmentUnits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GovernmentEntities",
                table: "GovernmentEntities");

            migrationBuilder.RenameTable(
                name: "Procurements",
                newName: "procurements");

            migrationBuilder.RenameTable(
                name: "GovernmentUnits",
                newName: "government_units");

            migrationBuilder.RenameTable(
                name: "GovernmentEntities",
                newName: "government_entities");

            migrationBuilder.RenameColumn(
                name: "Year",
                table: "procurements",
                newName: "year");

            migrationBuilder.RenameColumn(
                name: "Sequential",
                table: "procurements",
                newName: "sequential");

            migrationBuilder.RenameColumn(
                name: "Publisher",
                table: "procurements",
                newName: "publisher");

            migrationBuilder.RenameColumn(
                name: "Object",
                table: "procurements",
                newName: "object");

            migrationBuilder.RenameColumn(
                name: "Number",
                table: "procurements",
                newName: "number");

            migrationBuilder.RenameColumn(
                name: "Legislation",
                table: "procurements",
                newName: "legislation");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "procurements",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdateDate",
                table: "procurements",
                newName: "update_date");

            migrationBuilder.RenameColumn(
                name: "StatusName",
                table: "procurements",
                newName: "status_name");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "procurements",
                newName: "status_id");

            migrationBuilder.RenameColumn(
                name: "SolicitationName",
                table: "procurements",
                newName: "solicitation_name");

            migrationBuilder.RenameColumn(
                name: "PublishDate",
                table: "procurements",
                newName: "publish_date");

            migrationBuilder.RenameColumn(
                name: "ProposalOpeningDate",
                table: "procurements",
                newName: "proposal_opening_date");

            migrationBuilder.RenameColumn(
                name: "ProposalClosureDate",
                table: "procurements",
                newName: "proposal_closure_date");

            migrationBuilder.RenameColumn(
                name: "ProcessNumber",
                table: "procurements",
                newName: "process_number");

            migrationBuilder.RenameColumn(
                name: "PncpControlNumber",
                table: "procurements",
                newName: "pncp_control_number");

            migrationBuilder.RenameColumn(
                name: "ModalityName",
                table: "procurements",
                newName: "modality_name");

            migrationBuilder.RenameColumn(
                name: "ModalityId",
                table: "procurements",
                newName: "modality_id");

            migrationBuilder.RenameColumn(
                name: "IsPriceRegistration",
                table: "procurements",
                newName: "is_price_registration");

            migrationBuilder.RenameColumn(
                name: "IngestedAt",
                table: "procurements",
                newName: "ingested_at");

            migrationBuilder.RenameColumn(
                name: "HomologatedTotal",
                table: "procurements",
                newName: "homologated_total");

            migrationBuilder.RenameColumn(
                name: "HasParliamentaryAmendment",
                table: "procurements",
                newName: "has_parliamentary_amendment");

            migrationBuilder.RenameColumn(
                name: "GovernmentUnitId",
                table: "procurements",
                newName: "government_unit_id");

            migrationBuilder.RenameColumn(
                name: "EstimatedTotal",
                table: "procurements",
                newName: "estimated_total");

            migrationBuilder.RenameColumn(
                name: "DisputeType",
                table: "procurements",
                newName: "dispute_type");

            migrationBuilder.RenameIndex(
                name: "IX_Procurements_PncpControlNumber",
                table: "procurements",
                newName: "ix_procurements_pncp_control_number");

            migrationBuilder.RenameIndex(
                name: "IX_Procurements_GovernmentUnitId",
                table: "procurements",
                newName: "ix_procurements_government_unit_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "government_units",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "government_units",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "StateCode",
                table: "government_units",
                newName: "state_code");

            migrationBuilder.RenameColumn(
                name: "PncpUnitCode",
                table: "government_units",
                newName: "pncp_unit_code");

            migrationBuilder.RenameColumn(
                name: "GovernmentEntityId",
                table: "government_units",
                newName: "government_entity_id");

            migrationBuilder.RenameColumn(
                name: "AbbreviationName",
                table: "government_units",
                newName: "abbreviation_name");

            migrationBuilder.RenameIndex(
                name: "IX_GovernmentUnits_GovernmentEntityId_PncpUnitCode",
                table: "government_units",
                newName: "ix_government_units_government_entity_id_pncp_unit_code");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "government_entities",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "government_entities",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "TaxId",
                table: "government_entities",
                newName: "tax_id");

            migrationBuilder.RenameIndex(
                name: "IX_GovernmentEntities_TaxId",
                table: "government_entities",
                newName: "ix_government_entities_tax_id");

            migrationBuilder.AddColumn<int>(
                name: "solicitation_id",
                table: "procurements",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "pk_procurements",
                table: "procurements",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_government_units",
                table: "government_units",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_government_entities",
                table: "government_entities",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_government_units_government_entities_government_entity_id",
                table: "government_units",
                column: "government_entity_id",
                principalTable: "government_entities",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_procurements_government_units_government_unit_id",
                table: "procurements",
                column: "government_unit_id",
                principalTable: "government_units",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_government_units_government_entities_government_entity_id",
                table: "government_units");

            migrationBuilder.DropForeignKey(
                name: "fk_procurements_government_units_government_unit_id",
                table: "procurements");

            migrationBuilder.DropPrimaryKey(
                name: "pk_procurements",
                table: "procurements");

            migrationBuilder.DropPrimaryKey(
                name: "pk_government_units",
                table: "government_units");

            migrationBuilder.DropPrimaryKey(
                name: "pk_government_entities",
                table: "government_entities");

            migrationBuilder.DropColumn(
                name: "solicitation_id",
                table: "procurements");

            migrationBuilder.RenameTable(
                name: "procurements",
                newName: "Procurements");

            migrationBuilder.RenameTable(
                name: "government_units",
                newName: "GovernmentUnits");

            migrationBuilder.RenameTable(
                name: "government_entities",
                newName: "GovernmentEntities");

            migrationBuilder.RenameColumn(
                name: "year",
                table: "Procurements",
                newName: "Year");

            migrationBuilder.RenameColumn(
                name: "sequential",
                table: "Procurements",
                newName: "Sequential");

            migrationBuilder.RenameColumn(
                name: "publisher",
                table: "Procurements",
                newName: "Publisher");

            migrationBuilder.RenameColumn(
                name: "object",
                table: "Procurements",
                newName: "Object");

            migrationBuilder.RenameColumn(
                name: "number",
                table: "Procurements",
                newName: "Number");

            migrationBuilder.RenameColumn(
                name: "legislation",
                table: "Procurements",
                newName: "Legislation");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Procurements",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "update_date",
                table: "Procurements",
                newName: "UpdateDate");

            migrationBuilder.RenameColumn(
                name: "status_name",
                table: "Procurements",
                newName: "StatusName");

            migrationBuilder.RenameColumn(
                name: "status_id",
                table: "Procurements",
                newName: "StatusId");

            migrationBuilder.RenameColumn(
                name: "solicitation_name",
                table: "Procurements",
                newName: "SolicitationName");

            migrationBuilder.RenameColumn(
                name: "publish_date",
                table: "Procurements",
                newName: "PublishDate");

            migrationBuilder.RenameColumn(
                name: "proposal_opening_date",
                table: "Procurements",
                newName: "ProposalOpeningDate");

            migrationBuilder.RenameColumn(
                name: "proposal_closure_date",
                table: "Procurements",
                newName: "ProposalClosureDate");

            migrationBuilder.RenameColumn(
                name: "process_number",
                table: "Procurements",
                newName: "ProcessNumber");

            migrationBuilder.RenameColumn(
                name: "pncp_control_number",
                table: "Procurements",
                newName: "PncpControlNumber");

            migrationBuilder.RenameColumn(
                name: "modality_name",
                table: "Procurements",
                newName: "ModalityName");

            migrationBuilder.RenameColumn(
                name: "modality_id",
                table: "Procurements",
                newName: "ModalityId");

            migrationBuilder.RenameColumn(
                name: "is_price_registration",
                table: "Procurements",
                newName: "IsPriceRegistration");

            migrationBuilder.RenameColumn(
                name: "ingested_at",
                table: "Procurements",
                newName: "IngestedAt");

            migrationBuilder.RenameColumn(
                name: "homologated_total",
                table: "Procurements",
                newName: "HomologatedTotal");

            migrationBuilder.RenameColumn(
                name: "has_parliamentary_amendment",
                table: "Procurements",
                newName: "HasParliamentaryAmendment");

            migrationBuilder.RenameColumn(
                name: "government_unit_id",
                table: "Procurements",
                newName: "GovernmentUnitId");

            migrationBuilder.RenameColumn(
                name: "estimated_total",
                table: "Procurements",
                newName: "EstimatedTotal");

            migrationBuilder.RenameColumn(
                name: "dispute_type",
                table: "Procurements",
                newName: "DisputeType");

            migrationBuilder.RenameIndex(
                name: "ix_procurements_pncp_control_number",
                table: "Procurements",
                newName: "IX_Procurements_PncpControlNumber");

            migrationBuilder.RenameIndex(
                name: "ix_procurements_government_unit_id",
                table: "Procurements",
                newName: "IX_Procurements_GovernmentUnitId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "GovernmentUnits",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "GovernmentUnits",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "state_code",
                table: "GovernmentUnits",
                newName: "StateCode");

            migrationBuilder.RenameColumn(
                name: "pncp_unit_code",
                table: "GovernmentUnits",
                newName: "PncpUnitCode");

            migrationBuilder.RenameColumn(
                name: "government_entity_id",
                table: "GovernmentUnits",
                newName: "GovernmentEntityId");

            migrationBuilder.RenameColumn(
                name: "abbreviation_name",
                table: "GovernmentUnits",
                newName: "AbbreviationName");

            migrationBuilder.RenameIndex(
                name: "ix_government_units_government_entity_id_pncp_unit_code",
                table: "GovernmentUnits",
                newName: "IX_GovernmentUnits_GovernmentEntityId_PncpUnitCode");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "GovernmentEntities",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "GovernmentEntities",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "tax_id",
                table: "GovernmentEntities",
                newName: "TaxId");

            migrationBuilder.RenameIndex(
                name: "ix_government_entities_tax_id",
                table: "GovernmentEntities",
                newName: "IX_GovernmentEntities_TaxId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Procurements",
                table: "Procurements",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GovernmentUnits",
                table: "GovernmentUnits",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GovernmentEntities",
                table: "GovernmentEntities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GovernmentUnits_GovernmentEntities_GovernmentEntityId",
                table: "GovernmentUnits",
                column: "GovernmentEntityId",
                principalTable: "GovernmentEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Procurements_GovernmentUnits_GovernmentUnitId",
                table: "Procurements",
                column: "GovernmentUnitId",
                principalTable: "GovernmentUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
