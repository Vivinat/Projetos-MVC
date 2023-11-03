using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MVCSite.Models;
using MVCSite.Services;
using Newtonsoft.Json;

namespace MVCSite.Controllers
{
	public class AutoBattlerController : Controller
	{
		private readonly GameLogicService _gameLogicService;
        bool gameStarted = false;

        public AutoBattlerController(GameLogicService gameLogicService)
		{
			_gameLogicService = gameLogicService;
		}

		public IActionResult Index()
		{

            try
			{
				string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "charactersData.json");

				using (StreamReader reader = new StreamReader(jsonFilePath))
				{
					string jsonContent = reader.ReadToEnd();
					GameModel GameData = JsonConvert.DeserializeObject<GameModel>(jsonContent);

                    List<Character> charactersData = GameData.Characters;

                    return View(charactersData);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return View("Error"); 
			}
		}

        [HttpPost]
        public IActionResult SelectAlly(int allyID)
        {
            Console.WriteLine("Aliado de id " + allyID + " selecionado");
            GameModel gameData = LoadGameData();

            Console.WriteLine("Game data carregado");

            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "charactersData.json");

            using (StreamReader reader = new StreamReader(jsonFilePath))
            {
                string jsonContent = reader.ReadToEnd();
                GameModel allGameData = new GameModel();
                allGameData.Characters = new List<Character>();
                allGameData = JsonConvert.DeserializeObject<GameModel>(jsonContent);

                Console.WriteLine(allGameData.Characters);

                List<Character> charactersData = new List<Character>(); 

                charactersData = allGameData.Characters;

                if (charactersData == null)
                {
                    Console.WriteLine("characters Data é nulo");
                }

                if (charactersData != null)
                {
                    foreach (Character c in charactersData)
                    {
                        if (c.id == allyID)
                        {
                            gameData.Characters[0] = c;
                            break;
                        }
                    }
                    SaveGameData(gameData);
                }
            }
            return Json(gameData.Characters[0].name);
        }

        [HttpPost]
        public IActionResult SelectEnemy(int enemyID)
        {
            GameModel gameData = LoadGameData();

            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "charactersData.json");

            using (StreamReader reader = new StreamReader(jsonFilePath))
            {
                string jsonContent = reader.ReadToEnd();
                GameModel AllGameData = JsonConvert.DeserializeObject<GameModel>(jsonContent);

                List<Character> charactersData = AllGameData.Characters;

                if (charactersData != null)
                {
                    foreach (Character c in charactersData)
                    {
                        if (c.id == enemyID)
                        {
                            gameData.Characters[1] = c;
                            break;
                        }
                    }
                    SaveGameData(gameData);
                }
            }
            return Json(gameData.Characters[1].name);
        }

        private GameModel LoadGameData()
        {
            Console.WriteLine("Carregando Game Data");
            try
            {
                string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "choosenCharacters.json");

                using (StreamReader reader = new StreamReader(jsonFilePath))
                {
                    string jsonContent = reader.ReadToEnd();
                    GameModel gameData = JsonConvert.DeserializeObject<GameModel>(jsonContent);

                    return gameData;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private void SaveGameData(GameModel gameData)
        {
            Console.WriteLine("Salvando Game Data");
            try
            {
                string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "choosenCharacters.json");
                string updatedGameData = JsonConvert.SerializeObject(gameData);

                using (StreamWriter writer = new StreamWriter(jsonFilePath))
                {
                    writer.Write(updatedGameData);
                    Console.WriteLine("Game data salva");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }

        public IActionResult Initiate()
        {
            GameModel gameData = LoadGameData();
            if (gameData == null || gameData.Characters.Count < 2 || gameStarted == true
                || gameData.Characters[0].id == 0)
            {
                TempData["Message"] = "Insira os personagens!";
                return RedirectToAction("Index");
            }

            if (!gameStarted)
            {
                gameStarted = true;
                _gameLogicService.StartGame(gameData);
                gameData.Characters[0].id = 0;
                SaveGameData(gameData);
            }
            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "battleResults.json");
            string jsonContent = System.IO.File.ReadAllText(jsonFilePath);
            return Content(jsonContent, "application/json");
        }
    }

}

