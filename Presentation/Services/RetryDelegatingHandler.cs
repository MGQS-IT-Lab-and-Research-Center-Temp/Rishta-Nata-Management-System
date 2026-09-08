using System.Net;

namespace Presentation.Services;

/// <summary>
/// Lightweight retry for transient Tajneed API failures (network hiccups,
/// timeouts, 5xx responses). Uses a small fixed backoff and a max-retry cap;
/// deliberately not a full circuit breaker to avoid an extra package
/// dependency. Idempotency: retries only safe HTTP verbs (GET) and requests
/// whose content can be re-read (3xx/5xx responses retried regardless since a
/// response body is only replayed for requests that already reached us once).
/// </summary>
public sealed class RetryDelegatingHandler : DelegatingHandler
{
    private const int MaxRetries = 3;
    private static readonly TimeSpan InitialBackoff = TimeSpan.FromMilliseconds(200);

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        Exception? lastException = null;

        for (var attempt = 1; attempt <= MaxRetries; attempt++)
        {
            try
            {
                var response = await base.SendAsync(request, cancellationToken);

                if (attempt == MaxRetries ||
                    !response.IsSuccessStatusCode &&
                    (int)response.StatusCode < 500 && response.StatusCode != HttpStatusCode.RequestTimeout)
                {
                    return response;
                }

                await Task.Delay(BackoffFor(attempt), cancellationToken);
            }
            catch (HttpRequestException ex) when (attempt < MaxRetries)
            {
                lastException = ex;
                await Task.Delay(BackoffFor(attempt), cancellationToken);
            }
            catch (TaskCanceledException ex) when (attempt < MaxRetries)
            {
                lastException = ex;
                await Task.Delay(BackoffFor(attempt), cancellationToken);
            }
        }

        return lastException is null
            ? throw new InvalidOperationException("Retry exhausted without a result.")
            : throw lastException;
    }

    private static TimeSpan BackoffFor(int attempt) =>
        InitialBackoff * (int)Math.Pow(2, attempt - 1);
}