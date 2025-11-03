using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory
{
    public string itemName;
    public int quantity;
    public double weight;
}

[Serializable]
public class Metadata
{
    public string id;
    public bool @private;      // use @private to match JSON key "private"
    public string createdAt;   // keep as string, parse later if needed
    public string name;
}

[Serializable]
public class Position
{
    public int x;
    public int y;
    public int z;
}

[Serializable]
public class Record
{
    public string playerName;
    public int level;
    public double health;
    public Position position;
    public List<Inventory> inventory;
}

[Serializable]
public class Root
{
    public Record record;
    public Metadata metadata;
}
