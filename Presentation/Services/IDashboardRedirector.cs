using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Services;

/// <summary>
/// Decides which dashboard a freshly signed-in member should land on, based on
/// the roles they hold. Kept here (not in Application) because the result is an
/// MVC <see cref="IActionResult"/> pointing at specific controllers/actions.
/// </summary>
public interface IDashboardRedirector
{
    IActionResult Resolve(JamaatMember member);
}
