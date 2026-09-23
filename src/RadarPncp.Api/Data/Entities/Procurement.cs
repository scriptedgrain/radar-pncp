namespace RadarPncp.Api.Data.Entities;

public class Procurement
{
    public long Id { get; set; }
    public string PncpControlNumber { get; set; } = null!;
    public long GovernmentUnitId { get; set; }
    public int Year { get; set; }
    public int Sequential { get; set; }
    public string Number { get; set; } = null!;
    public string ProcessNumber { get; set; } = null!;
    public string Object { get; set; } = null!;
    public int ModalityId { get; set; }
    public string ModalityName { get; set; } = null!;
    public int StatusId { get; set; }
    public string StatusName { get; set; } = null!;
    public decimal? EstimatedTotal { get; set; }
    public decimal? HomologatedTotal { get; set; }
    public bool IsPriceRegistration { get; set; }
    public bool HasParliamentaryAmendment { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public DateTime? ProposalOpeningDate { get; set; }
    public DateTime? ProposalClosureDate { get; set; }
    public string? Legislation { get; set; }
    public string? DisputeType { get; set; }
    public string? SolicitationName { get; set; }
    public string? Publisher { get; set; }
    public DateTime IngestedAt { get; set; }
}
