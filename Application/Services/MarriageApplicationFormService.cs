using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class MarriageApplicationFormService : IMarriageApplicationFormService
{
    private readonly RishtanataDbContext _context;
    private readonly ILogger<MarriageApplicationFormService> _logger;

    public MarriageApplicationFormService(RishtanataDbContext context, ILogger<MarriageApplicationFormService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<MarriageApplicationForm> CreateAsync(MarriageApplicationForm application, CancellationToken ct = default)
    {
        if (application is null) throw new ArgumentNullException(nameof(application));

        if (application.MarriageApplicationId == Guid.Empty)
            return await CreateDraftForGroomAsync(application, ct);

        _context.MarriageApplicationForms.Add(application);

        try
        {
            await _context.SaveChangesAsync(ct);
            return application;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating MarriageApplicationForm (Id: {Id})", application.MarriageApplicationId);
            throw;
        }
    }

    public async Task<MarriageApplicationForm> CreateDraftForGroomAsync(
        MarriageApplicationForm form,
        CancellationToken ct = default)
    {
        if (form is null) throw new ArgumentNullException(nameof(form));

        // MarriageApplicationId is a required foreign key, so create the
        // parent application and its groom form as one EF Core object graph.
        var application = new FormApplication
        {
            Status = ApplicationStatus.Draft,
            MarriageApplicationFormId = form.Id,
            AppliedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            MarriageApplicationForm = form
        };

        form.MarriageApplicationId = application.Id;
        form.MarriageApplication = application;
        form.CreatedAt = DateTime.UtcNow;
        form.ModifiedAt = DateTime.UtcNow;

        _context.FormApplications.Add(application);

        try
        {
            await _context.SaveChangesAsync(ct);
            return form;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating groom draft application (FormId: {FormId})", form.Id);
            throw;
        }
    }

    public async Task<MarriageApplicationForm?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.MarriageApplicationForms
            .FirstOrDefaultAsync(x => x.MarriageApplicationId == id, ct);
    }

    public async Task<MarriageApplicationForm?> GetByMembershipNoAsync(string membershipNo, CancellationToken ct = default)
    {
        return await _context.MarriageApplicationForms
            .FirstOrDefaultAsync(x => x.BridegroomMembershipNo == membershipNo, ct);
    }

    public async Task<bool> UpdateAsync(MarriageApplicationForm application, CancellationToken ct = default)
    {
        if (application is null) throw new ArgumentNullException(nameof(application));

        _context.MarriageApplicationForms.Update(application);

        try
        {
            var affected = await _context.SaveChangesAsync(ct);
            return affected > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating MarriageApplicationForm (Id: {Id})", application.MarriageApplicationId);
            throw;
        }
    }

    Task IMarriageApplicationFormService.CreateAsync(MarriageApplicationForm form, CancellationToken ct)
    {
        return CreateAsync(form, ct);
    }

    Task IMarriageApplicationFormService.UpdateAsync(MarriageApplicationForm form, CancellationToken ct)
    {
        return UpdateAsync(form, ct);
    }
}
