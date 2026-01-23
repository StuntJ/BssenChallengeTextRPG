using System;
using System.IO;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.Encodings.Web;
using System.Text.Json;
using TextRPG.Data;
using TextRPG.Models;   

namespace TextRPG.Systems;

public class SaveLoadSystem
{
    //저장 경로 및 파일명
    private const string SaveFilePath = "savegame.json";

    //JSON 직렬화 옵션
    //직렬화의 의미 : 객체 -> 문자열
    //readonly는 상수의 의미 const는 컴파일 시점에서 고정 readonly는 런타임 시점에서 고정
    private static readonly JsonSerializerOptions jsonOptions = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping //한글 지원
    };


    #region 저장 기능
    public static bool SaveGame(Player player, InventorySystem inventory)
    {
        try
        {
            //1. 게임 객체 (클래스) -> Data Transfer Object (DTO)로 변환
            var saveData = new GameSaveData
            {
                Player = ConvertToPlayerData(player),
                Inventory = ConvertToItemData(inventory)
            };

            //2.DTO 객체를 JSON문자열로 변환
            string jsonString = JsonSerializer.Serialize(saveData, jsonOptions);

            //3. JSON문자열을 파일로 저장
            File.WriteAllText(SaveFilePath, jsonString);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    //Player -> PlayerData로 변환
    private static PlayerData ConvertToPlayerData(Player player)
    {
        return new PlayerData
        {
            Name = player.Name,
            Job = player.Job.ToString(),
            Level = player.Level,
            CurrentHp = player.CurrentHp,
            MaxHp = player.MaxHp,
            CurrentMp = player.CurrentMp,
            MaxMp = player.MaxMp,
            AttackPower = player.AttackPower,
            Defense = player.Defense,
            Gold = player.Gold,
            EquipedWeaponName = player.EquipedWeapon?.Name,
            EquipedArmorName = player.EquipedArmor?.Name
        };
    }
    //Inventory -> ItemData로 변환
    private static List<ItemData> ConvertToItemData(InventorySystem inventory)
    {
        var itemDataList = new List<ItemData>();

        for(int i = 0; i < inventory.Count; i++)
        {
            var item = inventory.GetItem(i);
            if (item == null) continue;

            var itemData = new ItemData
            {
                Name = item.Name
            };

            if(item is Equipment equipment)
            {
                itemData.ItemType = "Equipment";
                itemData.Slot = equipment.Slot.ToString();
            }
            else if(item is Consumable consumable)
            {
                itemData.ItemType = "Consumable";
            }

            itemDataList.Add(itemData); 
        }
        return itemDataList;    
    }
    #endregion

    #region 불러오기 기능
    //저장 파일 여부 확인
    public static bool IsSaveFileExist()
    {
        return File.Exists(SaveFilePath);
    }
    public static GameSaveData? LoadGame()
    {
        try
        {
            //1. JSON 파일에서 문자열 읽기
            string jsonString = File.ReadAllText(SaveFilePath);
            Console.WriteLine(jsonString);

            //2. JSON 문자열 -> DTO 변환 (역직렬화)
            var saveData = JsonSerializer.Deserialize<GameSaveData>(jsonString, jsonOptions);
            Console.WriteLine("\n게임 데이터가 로드되었습니다.");
            return saveData;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    //PlayerData DTO를 Player 클래스로 변환하는 메서드
    public static Player LoadPlayer(PlayerData data)
    {
        //JobType을 열거형으로 변환
        var job = Enum.Parse<JobType>(data.Job);
        //Player 객체 생성 
        var player = new Player(data.Name, job);
        //스텟 설정
        player.Level = data.Level;
        player.CurrentHp = data.CurrentHp;
        player.MaxHp = data.MaxHp;
        player.CurrentMp = data.CurrentMp;
        player.MaxMp = data.MaxMp;
        player.AttackPower = data.AttackPower;
        player.Gold = data.Gold;
        player.Defense = data.Defense;

        return player;
    }
    //ItemData DTO를 Inventory 클래스로 변환하는 메서드

    //저장된 장착 아이템을 복원하는 메서드 (무기/방어구)

    //아이템 생성 -> Inventory 추가
    #endregion
}
