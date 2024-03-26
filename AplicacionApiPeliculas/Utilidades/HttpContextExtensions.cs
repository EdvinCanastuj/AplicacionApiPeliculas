using Microsoft.EntityFrameworkCore;

namespace AplicacionApiPeliculas.Utilidades
{
    public static class HttpContextExtensions
    {
        public async static Task InsertarParametrosPaginacionEnCabecera<T>(this HttpContext httpContext,
                       IQueryable<T> queryable)
        {
            if(httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }
            double cantidad = await queryable.CountAsync();
            httpContext.Response.Headers.Append("cantidadTotalRegistros", cantidad.ToString());
        }
    }
}
