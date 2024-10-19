using Microsoft.AspNetCore.Mvc;

namespace BlogEngineSGC.Components
{
    public class BlogMonthsViewComponent : ViewComponent
    {


        public BlogMonthsViewComponent()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync(int maxPriority, bool isDone)
        {

            return View();
        }

    }
}