namespace RunningRacesApi.Models;
/// <summary>
/// Lapozási eredmény wrapper
/// </summary>
public class PagedResult<T>
{
    /// <summary>
    /// Az aktuális oldal elemei
    /// </summary>
    public List<T> Items { get; set; } = new();

    /// <summary>
    /// Összes elem száma (szűrés után)
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Aktuális oldal (1-től indul)
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Oldalméret
    /// </summary>
    public int PageSize { get; set; }
}