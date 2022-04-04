using UnityEngine;
using LitJson;
using System.Collections.Generic;
using System.IO;
using System;

public class ItemDatabase : MonoBehaviour {
	private List<Item> database = new List<Item>();
	private JsonData itemData;

	void Start()
	{
		
	}

	public void Instatiate()
    {
		itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
		ConstructItemDatabase();
	}

	public Item FetchItemById(int id)
	{
		Debug.Log(database.Count);
		for (int i = 0; i < database.Count; i++)
		{
			if (database[i].Id == id)
			{
				return database[i];
			}
		}

		return null;
	}
	
	void ConstructItemDatabase()
	{
		for (int i = 0; i < itemData.Count; i++)
		{
			Item newItem = new Item();
			newItem.Id = (int)itemData[i]["id"];
			newItem.Title = itemData[i]["title"]["en"].ToString();
			newItem.Value = (int)itemData[i]["value"];
			newItem.Strength = (int)itemData[i]["stats"]["strength"];
			newItem.Intellect = (int)itemData[i]["stats"]["intellect"];
			newItem.Dexterity = (int)itemData[i]["stats"]["dexterity"];
			newItem.Endurance = (int)itemData[i]["stats"]["endurance"];
			newItem.MemorySpirit = (int)itemData[i]["stats"]["memoryspirit"];
			newItem.Wisdom = (int)itemData[i]["stats"]["wisdom"];
			newItem.Description = itemData[i]["description"]["en"].ToString();
			newItem.Stackable = (bool)itemData[i]["stackable"];
			newItem.Rarity = (int)itemData[i]["rarity"];
			newItem.Slug = itemData[i]["slug"].ToString();
			newItem.Sprite = Resources.Load<Sprite>("Sprites/Items/" + newItem.Slug);

			database.Add(newItem);
		}
	}
}

public class Item
{
	public int Id { get; set; }
	public string Title { get; set; }

	public int Strength { get; set; }// огонь = сила 
	public int Intellect { get; set; }// вода = интеллект
	public int Dexterity { get; set; }// ветер = ловкость
	public int Endurance { get; set; }// земля = выносливость
	public int MemorySpirit { get; set; }// свет = дух
	public int Wisdom { get; set; }// тьма = мудрость
	public int Value { get; set; }
	public string Description { get; set; }
	public bool Stackable { get; set; }
	public int Rarity { get; set; }
	public string Slug { get; set; }
	public Sprite Sprite { get; set; }
    public string GearScore { get { return getGearScore(); } }

	public Color[] colors = new Color[7] {
		Color.gray,
		Color.white,
		Color.green,
		Color.blue,
		new Color(153/100f, 62/100f, 176/100f, 1.0f),
		new Color(255/100f, 153/100f, 0/100f, 1.0f),
		new Color(255/100f, 215/100f, 0/100f, 1.0f) // quests
	};

	public Dictionary<string, Color> aspectsColors = new Dictionary<string, Color>(6) 
	{ 
		{ "Strength", Color.white },
		{ "Dexterity", Color.white },
		{ "Intellect", Color.white },
		{ "Endurance", Color.white },
		{ "MemorySpirit", Color.white },
		{ "Wisdom", Color.white }
	};

	public static string ColorToHex(Color actColor)
	{
		return "#" + Snippets00002.ColorToHexString(actColor);
	}

	private string getGearScore()
    {
		int statsSum = Strength + Intellect + Dexterity + Endurance + MemorySpirit + Wisdom;

		return (statsSum % 100 * 1.4).ToString();
	}

    public object getCharactersDescription()
    {
		string characters = "";
        Dictionary<string, int> pairs = new Dictionary<string, int>
        {
            { "Strength", Strength },
			{ "Dexterity", Dexterity },
			{ "Intellect", Intellect },
			{ "Endurance", Endurance },
			{ "MemorySpirit", MemorySpirit },
			{ "Wisdom", Wisdom }
        };

        foreach (string sChar in pairs.Keys)
        {
			characters += $"<color={aspectsColors[sChar]}>{sChar}</color>: {pairs[sChar]}";

		}
		return characters;
    }

    public Item()
	{
		this.Id = -1;
	}
}

public class Snippets00002
{
	#region -- Data Members --
	static char[] hexDigits = {
		 '0', '1', '2', '3', '4', '5', '6', '7',
		 '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};
	#endregion

	public Snippets00002()
	{
	}

	/// <summary>
	/// Convert a .NET Color to a hex string.
	/// </summary>
	/// <returns>ex: "FFFFFF", "AB12E9"</returns>
	public static string ColorToHexString(Color color)
	{
		byte[] bytes = new byte[3];
		bytes[0] = (byte)color.r;
		bytes[1] = (byte)color.g;
		bytes[2] = (byte)color.b;
		char[] chars = new char[bytes.Length * 2];
		for (int i = 0; i < bytes.Length; i++)
		{
			int b = bytes[i];
			chars[i * 2] = hexDigits[b >> 4];
			chars[i * 2 + 1] = hexDigits[b & 0xF];
		}
		return new string(chars);
	}


}
