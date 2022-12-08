using Microsoft.AspNetCore.Mvc;


namespace ApiManager.ViewComponents
{
	public class PageTitleViewComponent : ViewComponent
	{ 
		public PageTitleViewComponent() 
		{ 
		
		}

		public async Task<IViewComponentResult> InvokeAsync(string title, string pageName)
		{
			return View("Index", new
			{
				title = title,
				pageName = pageName,
			});
		}
	}
}
