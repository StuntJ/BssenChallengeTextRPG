using System;
using System.Collections.Generic;

namespace TextRPG.Data;

public class GameSaveData
{
    //player 데이터
    public PlayerData Player { get; set; }
    //Item 데이터
    public List<ItemData> Inventory { get; set; } = new List<ItemData>();

}

public class PlayerData
{
    //기본 정보
    public string Name { get; set; }
    public string Job { get; set; }

    //플레이어 스텟 정보
    public int Level { get; set; }
    public int CurrentHp { get; set;}
    public int MaxHp { get; set; }  
    public int CurrentMp { get; set;}
    public int MaxMp { get; set; }
    public int AttackPower { get; set; }
    public int Defense { get; set; }
    public int Gold { get; set; }
    
    //장착 아이템
    public string? EquipedWeaponName { get; set; }
    public string? EquipedArmorName { get; set; }

}

public class ItemData 
{ 
    public string ItemType { get; set; }
    public string Name { get; set; }
    public string? Slot { get; set; }
}

