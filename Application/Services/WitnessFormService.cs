using Application.Interfaces;
using Infrastructure.DTOs;
using Infrastructure.Mappings;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class WitnessFormService : IWitnessFormService
    {
        private readonly RishtanataDbContext _context;
        private readonly ILogger<WitnessFormService> _logger;

        public WitnessFormService(
            RishtanataDbContext context,
            ILogger<WitnessFormService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<WitnessDto> AddWitnessAsync(
           WitnessDto witness,
           CancellationToken cancellationToken = default)
        {
            if (witness is null)
                throw new ArgumentNullException(nameof(witness));

            var entity = WitnessFormMapper.ToEntity(witness);

            _context.Witnesses.Add(entity);

            try
            {
                var saved = await _context.SaveChangesAsync(cancellationToken);

                if (saved == 0)
                {
                    _logger.LogWarning(
                        "SaveChangesAsync returned 0 when creating Witness (Id: {Id})",
                        entity.Id);
                }
                return WitnessFormMapper.ToDto(entity);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Concurrency conflict saving Witness (Id: {Id})",
                    entity.Id);

                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Database error while saving Witness (Id: {Id})",
                    entity.Id);

                throw new InvalidOperationException(
                    "Unable to save witness to the database.",
                    ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unexpected error while saving Witness (Id: {Id})",
                    entity.Id);

                throw;
            }
        }


        public async Task<WitnessDto?> GetWitnessByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var witness = await _context.Witnesses
                    .FirstOrDefaultAsync(
                        w => w.Id == id,
                        cancellationToken);

                if (witness is null)
                {
                    _logger.LogWarning(
                        "Witness not found (Id: {Id})",
                        id);

                    return null;
                }
;
                return WitnessFormMapper.ToDto(witness);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Database error while retrieving Witness (Id: {Id})",
                    id);

                throw new InvalidOperationException(
                    "Unable to retrieve witness from the database.",
                    ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unexpected error while retrieving Witness (Id: {Id})",
                    id);

                throw;
            }
        }

        public async Task<WitnessDto?> GetWitnessByTokenAsync(
            string token
            , CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException(
                    "Invitation token is required.",
                    nameof(token));

            try
            {
                var witness = await _context.Witnesses
                    .FirstOrDefaultAsync(
                        w => w.InvitationToken == token,
                        cancellationToken);

                if (witness is null)
                {
                    _logger.LogWarning(
                        "Witness not found for invitation token.");

                    return null;
                }

                return WitnessFormMapper.ToDto(witness);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
      ex,
      "Database error while retrieving Witness by invitation token.");

                throw new InvalidOperationException(
                    "Unable to retrieve witness.",
                    ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unexpected error while retrieving Witness by invitation token.");

                throw;
            }
        }


        public async Task<bool> CompleteWitnessAsync(
            WitnessDto witness,
            CancellationToken cancellationToken = default)
        {
            if (witness is null)
                throw new ArgumentNullException(nameof(witness));

            try
            {
                var existingWitness = await _context.Witnesses
                    .FirstOrDefaultAsync(
                        w => w.Id == witness.Id,
                        cancellationToken);

                if (existingWitness is null)
                {
                    _logger.LogWarning(
                        "Witness not found when attempting to complete it (Id: {Id})",
                        witness.Id);

                    return false;
                }

                existingWitness.FullName = witness.FullName;
                existingWitness.Email = witness.Email;
                existingWitness.PhoneNumber = witness.PhoneNumber;
                existingWitness.SignatureDate = witness.SignatureDate ?? string.Empty;

                existingWitness.IsCompleted = true;
                existingWitness.CompletedAt = DateTime.UtcNow;

                var affected = await _context.SaveChangesAsync(
                    cancellationToken);

                if (affected == 0)
                {
                    _logger.LogWarning(
                        "No changes saved when completing Witness (Id: {Id})",
                        witness.Id);

                    return false;
                }

                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Concurrency conflict completing Witness (Id: {Id})",
                    witness.Id);

                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Database error while completing Witness (Id: {Id})",
                    witness.Id);

                throw new InvalidOperationException(
                    "Unable to complete witness information.",
                    ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unexpected error while completing Witness (Id: {Id})",
                    witness.Id);

                throw;
            }
        }
    }
}
