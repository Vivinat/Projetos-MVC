namespace MVCSite.Models
{
	public class Character
	{
		public int id { get; set; }
		public string name { get; set; }
		public string description { get; set; }
		public string photo { get; set; }
		public int health { get; set; }
		public int mana { get; set; }
		public int damage { get; set; }
		public int specialDamage { get; set; }
		public int manaRegen { get; set; }
		public int defense { get; set; }
		public int specialDefense { get; set; }
		public int agility { get; set; }
	}
}
