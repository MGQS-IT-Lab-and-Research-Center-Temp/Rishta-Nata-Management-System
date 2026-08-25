using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class MarriageApplicationFormService : IMarriageApplicationFormService
{
    private readonly RishtanataDbContext _dbContext;
    private readonly ILogger<MarriageApplicationFormService> _logger;

    public MarriageApplicationFormService(
        RishtanataDbContext dbContext,
        ILogger<MarriageApplicationFormService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    // =========================================================
    // CREATE APPLICATION
    // =========================================================

    public async Task<MarriageApplicationForm> CreateAsync(
        MarriageApplicationForm application,
        CancellationToken cancellationToken = default)
    {
        if (application == null)
            throw new ArgumentNullException(nameof(application));

        _dbContext.MarriageApplicationForms.Add(application);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return application;
    }


    // =========================================================
    // GET BY ID
    // =========================================================

    public async Task<MarriageApplicationForm?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.MarriageApplicationForms
            .Include(x => x.GuardianOrWakeelSection)
            .Include(x => x.WitnessSignatures)
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }


    // =========================================================
    // GET BY MARRIAGE APPLICATION ID
    // =========================================================

    public async Task<MarriageApplicationForm?> GetByMarriageApplicationIdAsync(
        Guid marriageApplicationId)
    {
        return await _dbContext.MarriageApplicationForms
            .Include(x => x.GuardianOrWakeelSection)
            .Include(x => x.WitnessSignatures)
            .FirstOrDefaultAsync(
                x => x.MarriageApplicationId == marriageApplicationId);
    }


    // =========================================================
    // GET BY BRIDEGROOM MEMBERSHIP NUMBER
    // =========================================================

    public async Task<MarriageApplicationForm?> GetByMembershipNoAsync(
        string membershipNo,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.MarriageApplicationForms
            .FirstOrDefaultAsync(
                x => x.BridegroomMembershipNo == membershipNo,
                cancellationToken);
    }


    // =========================================================
    // UPDATE APPLICATION
    // =========================================================

    public async Task<bool> UpdateAsync(
        MarriageApplicationForm application,
        CancellationToken cancellationToken = default)
    {
        if (application == null)
            throw new ArgumentNullException(nameof(application));

        _dbContext.MarriageApplicationForms.Update(application);

        var affected = await _dbContext.SaveChangesAsync(
            cancellationToken);

        return affected > 0;
    }


    // =========================================================
    // GUARDIAN / WAKEEL SIGNATURE
    // =========================================================

    public async Task<bool> SubmitGuardianOrWakeelAsync(
        Guid marriageApplicationFormId,
        string signature,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(signature))
            return false;

        var application = await _dbContext.MarriageApplicationForms
            .Include(x => x.GuardianOrWakeelSection)
            .Include(x => x.WitnessSignatures)
            .FirstOrDefaultAsync(
                x => x.Id == marriageApplicationFormId,
                cancellationToken);

        if (application == null)
        {
            _logger.LogWarning(
                "Marriage application {ApplicationId} was not found.",
                marriageApplicationFormId);

            return false;
        }

        // Make sure Guardian/Wakeel section exists
        if (application.GuardianOrWakeelSection == null)
        {
            _logger.LogWarning(
                "Guardian/Wakeel section not found for application {ApplicationId}.",
                marriageApplicationFormId);

            return false;
        }

        // Save signature
        application.GuardianOrWakeelSection.Signature = signature;
        application.GuardianOrWakeelSection.Date = DateTime.UtcNow;

        // Check whether guardian/wakeel has signed
        bool guardianOrWakeelSigned =
            !string.IsNullOrWhiteSpace(
                application.GuardianOrWakeelSection.Signature);

        // Check whether both witnesses have signed
        bool bothWitnessesSigned =
            application.WitnessSignatures.Count >= 2 &&
            application.WitnessSignatures.All(
                w => !string.IsNullOrWhiteSpace(w.Signature));

        // Only advance when EVERYONE has signed
        if (guardianOrWakeelSigned && bothWitnessesSigned)
        {
            application.FormStage =
                MarriageFormStage.AwaitingImamVerification;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }


    // =========================================================
    // WITNESS SIGNATURE
    // =========================================================

    public async Task<bool> SubmitWitnessSignatureAsync(
        Guid marriageApplicationFormId,
        Guid witnessSignatureId,
        string signature,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(signature))
            return false;

        var application = await _dbContext.MarriageApplicationForms
            .Include(x => x.GuardianOrWakeelSection)
            .Include(x => x.WitnessSignatures)
            .FirstOrDefaultAsync(
                x => x.Id == marriageApplicationFormId,
                cancellationToken);

        if (application == null)
        {
            _logger.LogWarning(
                "Marriage application {ApplicationId} was not found.",
                marriageApplicationFormId);

            return false;
        }

        // Find the specific witness
        var witness = application.WitnessSignatures
            .FirstOrDefault(
                w => w.Id == witnessSignatureId);

        if (witness == null)
        {
            _logger.LogWarning(
                "Witness {WitnessId} was not found for application {ApplicationId}.",
                witnessSignatureId,
                marriageApplicationFormId);

            return false;
        }

        // Save witness signature
        witness.Signature = signature;
        witness.Date = DateTime.UtcNow;

        // Check guardian/wakeel
        bool guardianOrWakeelSigned =
            application.GuardianOrWakeelSection != null &&
            !string.IsNullOrWhiteSpace(
                application.GuardianOrWakeelSection.Signature);

        // Check both witnesses
        bool bothWitnessesSigned =
            application.WitnessSignatures.Count >= 2 &&
            application.WitnessSignatures.All(
                w => !string.IsNullOrWhiteSpace(w.Signature));

        // Advance only when guardian/wakeel AND both witnesses signed
        if (guardianOrWakeelSigned && bothWitnessesSigned)
        {
            application.FormStage =
                MarriageFormStage.AwaitingImamVerification;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
