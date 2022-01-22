using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace WordleBlazorWASMApp
{
    public static class JSInteropFocusExtensions
    {
        public static ValueTask FocusAsync(this IJSRuntime jsRuntime, ElementReference elementReference)
        {
            return jsRuntime.InvokeVoidAsync("BlazorFocusElement", elementReference);
        }
    }
}
