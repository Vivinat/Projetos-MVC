using Microsoft.AspNetCore.Mvc;
using MVCSite.Models;
using System.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVCSite.Controllers
{
	public class TimelineController : Controller
	{
		public IActionResult Index(List<TimelineModel> query)
		{
			try
			{
				string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "timeline.json");

				using (StreamReader reader = new StreamReader(jsonFilePath))
				{
					string jsonContent = reader.ReadToEnd();
					TimelineData timelineData = JsonConvert.DeserializeObject<TimelineData>(jsonContent);

					List<TimelineModel> allTimelineModels = timelineData.timeline;
					List<TimelineModel> currentTimelineModels = new List<TimelineModel>();
					currentTimelineModels.Add(allTimelineModels[0]);

					int pageNumber = 0; 
					string queryFromTempData = TempData["query"] as string;

					if (!string.IsNullOrEmpty(queryFromTempData))
					{ 
						{
                            pageNumber = Convert.ToInt32(queryFromTempData);
						}
					}


					Dictionary<int, int> timelineModelMapping = new Dictionary<int, int>
					{
						{ 5, 1 },
						{ 10, 2 },
						{ 20, 3 },
						{ 30, 4 },
						{ 40, 5 },
						{ 50, 6 },
						{ 60, 7 },
						{ 75, 8 }
					};

					for (int i = 0; i <= pageNumber; i++)
					{
						if (timelineModelMapping.ContainsKey(i))
						{
							currentTimelineModels.Add(allTimelineModels[timelineModelMapping[i]]);
						}
					}

					return View(currentTimelineModels);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Erro: {ex.Message}");
				return View("Error"); 
			}
		}

		[HttpPost] 
		public IActionResult Search(string query)
		{
			try
			{
				if (string.IsNullOrEmpty(query))
				{
                    ViewBag.ErrorMessage = "O campo de pesquisa não pode estar vazio.";
                    return RedirectToAction("Index");
				}

				TimelineModel selectedPage = new TimelineModel();

				try
				{
					selectedPage.pageNumber = Convert.ToInt32(query);
				}
				catch (Exception ex)
				{
                    Console.WriteLine(ex);
                    TempData["Message"] = "Insira um valor numérico válido ";
					return RedirectToAction("Index");
				}

				List<TimelineModel> searchResults = new List<TimelineModel>();
				
				searchResults.Add(selectedPage);

                TempData["query"] = query;

				return RedirectToAction("Index");


			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				ViewBag.ErrorMessage = "O campo de pesquisa não pode estar vazio. ";
				return RedirectToAction("Index");

			}
		}

	}
}
