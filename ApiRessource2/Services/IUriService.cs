using ApiRessource2.Models.Filter;

namespace ApiRessource2.Services
{
    public interface IUriService
    {
        Uri GetPageUri(PaginationFilter paginationFilter, string route);
    }
}
