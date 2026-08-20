using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class MarriageApplicationFormService : IMarriageApplicationFormService
    {
        private readonly RishtanataDbContext _context;
        private readonly ILogger<MarriageApplicationFormService> _logger;

        public MarriageApplicationFormService(
            RishtanataDbContext context,
            ILogger<MarriageApplicationFormService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<MarriageApplicationForm> CreateAsync(
            MarriageApplicationForm application,
            CancellationToken cancellationToken = default)
        {
            if (application is null)
                throw new ArgumentNullException(nameof(application));

            _context.MarriageApplicationForms.Add(application);

            try
            {
                var saved = await _context.SaveChangesAsync(cancellationToken);

                if (saved == 0)
                {
                    _logger.LogWarning(
                        "SaveChangesAsync returned 0 when creating MarriageApplicationForm (Id: {Id})",
                        application.Id);
                }

                return application;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Concurrency conflict saving MarriageApplicationForm (Id: {Id})",
                    application.Id);

                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Database error while saving MarriageApplicationForm (Id: {Id})",
                    application.Id);

                throw new InvalidOperationException(
                    "Unable to save marriage application to the database.",
                    ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unexpected error while saving MarriageApplicationForm (Id: {Id})",
                    application.Id);

                throw;
            }
        }

        public async Task<MarriageApplicationForm?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await _context.MarriageApplicationForms
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<MarriageApplicationForm?> GetByMarriageApplicationIdAsync(
            Guid marriageAplicationId)
        {
            return await _context.MarriageApplicationForms
                .FirstOrDefaultAsync(
                    x => x.MarriageApplicationId == marriageAplicationId);
        }

        public async Task<bool> UpdateAsync(
            MarriageApplicationForm application,
            CancellationToken cancellationToken = default)
        {
            if (application is null)
                throw new ArgumentNullException(nameof(application));

            _context.MarriageApplicationForms.Update(application);

            try
            {
                var affected = await _context.SaveChangesAsync(cancellationToken);

                return affected > 0;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Concurrency conflict updating MarriageApplicationForm (Id: {Id})",
                    application.Id);

                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Database error while updating MarriageApplicationForm (Id: {Id})",
                    application.Id);

                throw new InvalidOperationException(
                    "Unable to update marriage application.",
                    ex);
            }
        }
    }
}