
using MVCSite.Models;
using Newtonsoft.Json;

namespace ProjetosViniciusVieiraMota.Services
{
	public class GameLogicService
	{
        
        public void StartGame(GameModel gameModel)
		{
			if (gameModel.Characters.Count < 2)
			{
				Console.WriteLine("Vazio ou Incompleto");
				return;
			}

            List<string> battleResults = new List<string>();

            Character player = gameModel.Characters[0];
			Character enemy = gameModel.Characters[1];

			Console.WriteLine(enemy.id + enemy.name);
            Console.WriteLine(player.id + player.name);

            GameModel turnOrder = new GameModel();
            turnOrder.Characters = new List<Character>
            {
                player,
                enemy
            };

            CallTurnOrder(player, enemy, turnOrder, battleResults);
			Character victor = Battle(turnOrder, battleResults);
			EndGame(gameModel, victor, battleResults);
		}

        private Character Battle(GameModel turnOrder, List<string> battleResults)
		{
			int i = 0;
			bool specialAttack = false;
			bool missed = false;
			while (turnOrder.Characters[0].health > 0 
				|| turnOrder.Characters[1].health > 0)
			{
				if (turnOrder.Characters[0].health == 0 || turnOrder.Characters[1].health == 0)
				{
					break;
				}

				battleResults.Add("Turno de " + turnOrder.Characters[i].name + "!");

                specialAttack = CanSpecialAttack(turnOrder.Characters[i], battleResults);
				if (specialAttack)
				{
					TakeSpecialDamage(turnOrder, i, turnOrder.Characters[i].specialDamage, battleResults);
					specialAttack = false;
				}

                if (turnOrder.Characters[0].health == 0 || turnOrder.Characters[1].health == 0)
                {
                    break;
                }

                missed = CalulateMissChance(turnOrder.Characters[i], battleResults);
				if(!missed)
				{
                    TakeNormalAttack(turnOrder, i, turnOrder.Characters[i].damage, battleResults);
                }

                turnOrder.Characters[i].mana += turnOrder.Characters[i].manaRegen / 2;
				battleResults.Add(turnOrder.Characters[i].name + " regenera " + 
					turnOrder.Characters[i].manaRegen/2 + " de mana passiva neste turno!");

                i = (i + 1) % 2;
			}
			if (turnOrder.Characters[0].health > 0)
			{
				return turnOrder.Characters[0];
			}
            return turnOrder.Characters[1];
        }

        private bool CalulateMissChance(Character character, List<string> battleResults)
        {
            Random random = new Random();
			int hitChance = random.Next(1, 11);
			if (hitChance < 5 ) 
			{
				battleResults.Add(character.name + " errou seu ataque normal!");
				return true;
			}
			return false;
        }

        private void TakeNormalAttack(GameModel turnOrder, int i, int attackDamage, List<string> battleResults)
		{
			if (i == 0)
			{
				int damage = attackDamage - turnOrder.Characters[1].defense;
				if (damage <= 0)
				{
					damage = 0;
				}
				turnOrder.Characters[1].health -= damage;

				battleResults.Add(turnOrder.Characters[1].name + " tomou " + damage + " de dano do ataque de " +
					turnOrder.Characters[0].name + "!");

				battleResults.Add(turnOrder.Characters[1].name + " está com " +
                    turnOrder.Characters[1].health + " de vida!");

                if (turnOrder.Characters[1].health < 0)
				{
					turnOrder.Characters[1].health = 0;
				}
				turnOrder.Characters[0].mana += turnOrder.Characters[0].manaRegen;
			}
			else
			{
				int damage =  attackDamage - turnOrder.Characters[0].defense;
				if (damage <= 0)
				{
					damage = 0;
				}
				turnOrder.Characters[0].health -= damage;

                battleResults.Add(turnOrder.Characters[0].name + " tomou " + damage + " de dano do ataque de " +
					turnOrder.Characters[1].name + "!");

                battleResults.Add(turnOrder.Characters[0].name + " está com " +
					turnOrder.Characters[0].health + " de vida!");

                if (turnOrder.Characters[0].health < 0)
				{
                    turnOrder.Characters[0].health = 0;
				}
				turnOrder.Characters[1].mana += turnOrder.Characters[1].manaRegen;
			}
		}

		private void TakeSpecialDamage(GameModel turnOrder, int i, int specialDamage, List<string> battleResults)
		{
			if (i == 0)
			{
				int damage = specialDamage - turnOrder.Characters[1].specialDefense;
				battleResults.Add(turnOrder.Characters[1].name + " recebeu " + damage + " de dano especial!");
				if (damage <= 0)
				{
					damage = 0;
				}
				turnOrder.Characters[1].health -= damage;
                battleResults.Add(turnOrder.Characters[1].name + " está com " +
					turnOrder.Characters[1].health + " de vida");
                if (turnOrder.Characters[1].health < 0)
				{
					turnOrder.Characters[1].health = 0;
				}
				turnOrder.Characters[0].mana = 0;
			}
			else
			{
				int damage =  specialDamage - turnOrder.Characters[0].specialDefense;
                battleResults.Add(turnOrder.Characters[0].name + " recebeu " + damage + " de dano especial!");
                if (damage <= 0)
				{
					damage = 0;
				}
				turnOrder.Characters[0].health -= damage;
                battleResults.Add(turnOrder.Characters[0].name + " está com " +
					turnOrder.Characters[0].health + " de vida");
                if (turnOrder.Characters[0].health < 0)
				{
                    turnOrder.Characters[0].health = 0;
				}
				turnOrder.Characters[1].mana = 0;
			}
		}

		private bool CanSpecialAttack(Character character, List<string> battleResults)
		{
			if (character.mana >= 100)
			{
				battleResults.Add(character.name + " consegue efetuar um ataque especial!");
				return true;
			}
			else
			{
				return false;
			}
		}

		private GameModel CallTurnOrder(Character player, Character enemy, GameModel turnOrder, List<string> battleResults)
		{
			if (player.agility > enemy.agility)
			{
				turnOrder.Characters[0] = player;
				turnOrder.Characters[1] = enemy;
			}
			else if (player.agility < enemy.agility)
			{
				turnOrder.Characters[0] = enemy;
				turnOrder.Characters[1] = player;
			}else 
			{
				battleResults.Add("Ambos possuem mesmo valor de agilidade. A sorte será lançada");
				Random random = new Random();
				int coinFlip = random.Next(1, 3);
				battleResults.Add("Coinflip: " + coinFlip);
				if (coinFlip == 1)
				{
					turnOrder.Characters[0] = player;
					turnOrder.Characters[1] = enemy;
				}
				else
				{
					turnOrder.Characters[0] = enemy;
					turnOrder.Characters[1] = player;
				}
			}
			battleResults.Add("Ordem:" + turnOrder.Characters[0].name + " começará primeiro e "
				+ turnOrder.Characters[1].name + " Começará depois!");
			foreach (Character character in turnOrder.Characters)
			{
				battleResults.Append(character.ToString());	
			}
			return turnOrder;
		}

		public void EndGame(GameModel gameModel, Character victor, List<string> battleResults)
		{
			battleResults.Add("O Vencedor é " + victor.name + "!");
			Console.WriteLine("O vencedor é " + victor.name + "!");
			CreateResultsList(battleResults);
		}

        private void CreateResultsList(List<string> battleResults)
        {
            Console.WriteLine("Salvando logs de batalha em Game Data");
			try
			{
				string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "battleResults.json");
				string updateResultsLog = JsonConvert.SerializeObject(battleResults, Formatting.Indented);

				using (StreamWriter writer = new StreamWriter(jsonFilePath))
				{
					writer.Write(updateResultsLog);
					Console.WriteLine("Log de batalha salvo");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
        }
    }
}
