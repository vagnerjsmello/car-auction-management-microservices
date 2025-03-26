using CAMS.Auctions.Domain.Entities;
using CAMS.Auctions.Domain.Repositories;
using CAMS.Common.Exceptions;
using CAMS.Common.ResponseApi;
using CAMS.Infrastructure.Messaging.Events;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CAMS.Auctions.Application.Commands.Auctions.PlaceBid
{
    /// <summary>
    /// Handles the PlaceBidCommand logic.
    /// </summary>
    public class PlaceBidCommandHandler : IRequestHandler<PlaceBidCommand, ResponseResult<PlaceBidResponse>>
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IValidator<PlaceBidCommand> _validator;
        private readonly ILogger<PlaceBidCommandHandler> _logger;
        private readonly IDomainEventPublisher _eventPublisher;

        public PlaceBidCommandHandler(IAuctionRepository auctionRepository, IValidator<PlaceBidCommand> validator, ILogger<PlaceBidCommandHandler> logger, IDomainEventPublisher eventPublisher)
        {
            _auctionRepository = auctionRepository ?? throw new ArgumentNullException(nameof(auctionRepository));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
        }

        public async Task<ResponseResult<PlaceBidResponse>> Handle(PlaceBidCommand command, CancellationToken cancellationToken)
        {

            ValidationResult validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning($"Validation failed for {command.GetType().Name}: {validationResult.Errors}");
                return ResponseResult<PlaceBidResponse>.Fail(validationResult);

            }

            _logger.LogInformation($"Placing bid on auction {command.AuctionId}");


            var auction = await _auctionRepository.GetByIdAsync(command.AuctionId);
            if (auction == null)
            {
                var ex = new AuctionNotFoundException(command.AuctionId);
                _logger.LogWarning(ex.Message);
                throw ex;
            }

            var bid = new Bid(command.BidAmount, command.BidderId);            

            auction.PlaceBid(bid);

            await _auctionRepository.UpdateAsync(auction);
            await _eventPublisher.PublishEventsAsync(auction);

            _logger.LogInformation($"Bid placed successfully on auction {command.AuctionId}. New highest bid: {auction.HighestBid}");
            return ResponseResult<PlaceBidResponse>.Success(new PlaceBidResponse(auction.Id, auction.HighestBid));
        }
    }
}
