using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace AplicacionApiPeliculas.Utilidades
{
    public class TypeBinder<T> : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var nombrePropiedad = bindingContext.ModelName;
            var valor = bindingContext.ValueProvider.GetValue(nombrePropiedad);
            if(valor == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }
            try
            {
                var vlorDeserializado = JsonConvert.DeserializeObject<T>(valor.FirstValue);
                bindingContext.Result = ModelBindingResult.Success(vlorDeserializado);
            }catch
            {
                bindingContext.ModelState.TryAddModelError(nombrePropiedad, "Valor invalido para tipo List<int>");
            }
            return Task.CompletedTask;
        }
    }
}
