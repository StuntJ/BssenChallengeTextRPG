using System;
using System.Collections.Generic;
using TextRPG.Models;

namespace TextRPG.Systems;
public class InventorySystem
{
    #region 프로퍼티 
    //아이템 목록
    private List<Item> Items { get; set; }

    //아이템 개수 (읽기 전용)
    public int Count => Items.Count; //=>를 goes to 로 읽음

    #endregion

    #region 생성자
    public InventorySystem()
    {
        Items = new List<Item>();
    }
    #endregion

    #region 아이템 관리 메서드
    //아이템 추가
    public void AddItem(Item item)
    {
        Items.Add(item);
        Console.WriteLine($"{item.Name}을 인벤토리에 추가했습니다.");
    }
    //아이템 삭제
    public bool RemoveItem(Item item)
    {
        if (Items.Remove(item))
        {
            Console.WriteLine($"{item.Name}을 인벤토리에서 제거했습니다.");
            return true;
        }
        return false;
    }
    #endregion

    #region 인벤토리 표시

    public void DisplayInventory()
    {
        Console.Clear();
        Console.WriteLine("\n╔════════════════════════════════╗");
        Console.WriteLine("║         인벤토리                ║");
        Console.WriteLine("╚════════════════════════════════╝\n");

        if (Items.Count == 0)
        {
            Console.WriteLine("인벤토리가 비어있습니다.");
            return;
        }

        Console.WriteLine("\n[보유 아이템]");
        for(int i = 0; i < Items.Count; i++)
        {
            Console.Write($"[{i + 1}] ");
            Items[i].DisplayInfo();
        }
    }

    public void ShowInventoryMenu(Player player)
    {
        while (true)
        {
            DisplayInventory();
            Console.WriteLine("\n선택하세요");
            Console.WriteLine("1. 아이템 사용");
            Console.WriteLine("2. 아이템 버리기");
            Console.WriteLine("0. 나가기");
            Console.Write("선택: ");
            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    UseItem(player);            
                    break;
                case "2":
                    //아이템 버리기 로직
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("잘못된 선택입니다. 다시 선택하세요.");
                    break;
            }
        }
    }

    #endregion

    #region 아이템 사용
    private void UseItem(Player player)
    {
        if (Items.Count == 0)
        {
            Console.WriteLine("인벤토리가 비어있습니다.");
            return;
        }

        Console.Write("\n사용할 아이템 번호 (0: 취소)> ");

        if(int.TryParse(Console.ReadLine(), out int index)&&index>0&&index<=Items.Count)
        {
            Item item = Items[index-1];
            if (item.Use(player))
            {
                //소모품일 경우 사용 후 리스트에서 제거함
                if(item is Consumable)
                {
                    RemoveItem(item);
                }
            }
        }
        else if (index != 0)
        {
            Console.WriteLine("잘못된 선택입니다.");
        }
    } 
    #endregion
}

