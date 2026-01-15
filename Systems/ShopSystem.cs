using System;
using System.Collections.Generic;
using System.Numerics;
using TextRPG.Models;
using TextRPG.Utils;

namespace TextRPG.Systems;

// [상점 시스템]
// 메뉴선택 (구매/판매/취소)
public class ShopSystem
{
    #region 프로퍼티
    //판매중인 아이템 목록
    private List<Item>? ShopItems { get; set; }

    #endregion

    #region 생성자
    public ShopSystem()
    {
        ShopItems = new List<Item>();
        //아이템 초기화
        InitShop();
    }
    #endregion

    #region 초기화 메서드
    private void InitShop()
    {
        //무기
        ShopItems.Add(Equipment.CreateWeapon("목검"));
        ShopItems.Add(Equipment.CreateWeapon("철검"));
        ShopItems.Add(Equipment.CreateWeapon("전설검"));
        //방어구
        ShopItems.Add(Equipment.CreateArmor("천갑옷"));
        ShopItems.Add(Equipment.CreateArmor("철갑옷"));
        ShopItems.Add(Equipment.CreateArmor("전설갑옷"));
        //포션
        ShopItems.Add(Consumable.CreatePotion("체력포션"));
        ShopItems.Add(Consumable.CreatePotion("대형체력포션"));
        ShopItems.Add(Consumable.CreatePotion("대형마나포션"));
    }
    #endregion

    #region 상점 메뉴
    // 메뉴 표시
    public void ShowShopMenu(Player player, InventorySystem inventory)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("\n╔════════════════════════════════╗");
            Console.WriteLine("║         상  점                 ║");
            Console.WriteLine("╚════════════════════════════════╝\n");
            Console.WriteLine($"\n보유 골드: {player.Gold}골드");

            Console.WriteLine("\n1. 아이템 구매");
            Console.WriteLine("2. 아이템 판매");
            Console.WriteLine("0. 나가기");

            Console.Write("\n선택> ");
            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    //구매 메서드
                    BuyItem(player, inventory);
                    break;
                case "2":
                    //판매 메서드
                    SellItem(player,inventory);
                    break;
                case "0":
                    Console.WriteLine("상점을 나갑니다....");
                    ConsoleUI.PressAnyKey();
                    return;
                default:
                    Console.WriteLine("잘못된 선택입니다....");
                    ConsoleUI.PressAnyKey();
                    break;

            }
        }
    }
    #endregion

    #region 구매 메서드
    private void BuyItem(Player player, InventorySystem inventory)
    {
        //구매 가능한 아이템 목록
        Console.Clear();
        Console.WriteLine("\n[구매 가능한 아이템]");

        for(int i = 0; i < ShopItems.Count; i++)
        {
            Console.Write($"[{i + 1}] ");
            ShopItems[i].DisplayInfo();
        }

        Console.Write("\n구매할 아이템 번호를 선택하세요. (0:취소)> ");

        if(int.TryParse(Console.ReadLine(), out var index) && 0 < index && index <= ShopItems.Count)
        {
            Item selectedItem = ShopItems[index-1];

            //골드가 충분한지 확인
            if (player.Gold >= selectedItem.Price)
            {
                Console.Write($"{selectedItem.Name}을 {selectedItem.Price}골드로 구매하시겠습니까? (y/n): ");
                if(Console.ReadLine()?.ToLower() == "y")
                {
                    //골드를 차감
                    player.SpendGold(selectedItem.Price);
                    //구매한 아이템의 인스턴스 생성
                    Item? item = CreateItem(selectedItem);
                    //아이템 장착 또는 인벤토리에 추가
                    if (item is Equipment equipment)
                    {
                        inventory.AddItem(equipment);
                        player.EquipItem(equipment);
                    }
                    else if (item is Consumable consumable)
                    {
                        inventory.AddItem(consumable);
                    }

                    Console.WriteLine($"{selectedItem.Name}을 구매했습니다.");
                    ConsoleUI.PressAnyKey();
                }
            }
            else
            {
                Console.WriteLine("\n골드가 부족합니다.");
                ConsoleUI.PressAnyKey();
            }
        }
    }
    #endregion

    #region 아이템 복제 메서드
    private Item? CreateItem(Item item)
    {
        //장착 아이템
        if (item is Equipment equipment)
        {
            var newItem = new Equipment(equipment.Name, equipment.Description, equipment.Price, equipment.Slot, equipment.AttackBonus, equipment.DefenseBonus);
            return newItem;
        }
        //소모성 아이템
        else if (item is Consumable consumable)
        {
            return new Consumable(consumable.Name, consumable.Description, consumable.Price, consumable.HpAmount, consumable.MpAmount); 
        }

        return null;
    }
    #endregion

    #region 아이템 판매
    private void SellItem(Player player, InventorySystem inventory)
    {
        if (inventory.Count == 0)
        {
            Console.WriteLine("\n판매할 아이템이 없습니다.");
            return;
        }

        inventory.DisplayInventory();
        Console.Write("\n판매할 아이템 번호를 입력하세요. (0:취소)> ");
        if(int.TryParse(Console.ReadLine(), out var index) && 0 < index && index <= inventory.Count)
        {
            // 인벤토리에서 아이템 추출
            Item? item = inventory.GetItem(index-1);

            if(item != null)
            {
                //판매 가격 할인 (구매 가격의 50%)
                int sellPrice = (int)(item.Price * 0.5f);

                Console.WriteLine($"{item.Name}을 {sellPrice} 골드에 판매하시겠습니까? (y/n): ");
                if (Console.ReadLine()?.ToLower() == "y")
                {
                    //아이템 인벤토리에서 제거 및 골드 증가
                    inventory.RemoveItem(item);
                    player.GainGold(sellPrice);

                    //장착 해제
                    if (item is Equipment equipment)
                    {
                        player.UnequipItem(equipment.Slot);
                    }

                    Console.WriteLine($"{item.Name}을 판매했습니다.");
                    ConsoleUI.PressAnyKey();
                }
            }
        }

    }
    #endregion
}
