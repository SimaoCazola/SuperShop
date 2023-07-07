using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace SuperShop.Helpers
{
    public class NotFoundViewResult: ViewResult // Herda a propriedade do ViewResult
    {
        public NotFoundViewResult(string viewName)
        {
            ViewName = viewName;
            StatusCode = (int)HttpStatusCode.NotFound;
        }
    }
}
