using FluentValidation;

namespace CAMS.Auctions.Application.Commands.Auctions.CloseAuction;

/// <summary>
/// Validator for the CloseAuctionCommand.
/// </summary>
public class CloseAuctionCommandValidator : AbstractValidator<CloseAuctionCommand>
{
    public CloseAuctionCommandValidator()
    {
        RuleFor(cmd => cmd.AuctionId)
            .NotEmpty().WithMessage("AuctionId must not be empty.");
    }
}
