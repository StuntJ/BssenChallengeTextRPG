using System;
using System.Collections.Generic;
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
                    //TODO: 구매 메서드
                    break;
                case "2":
                    //TODO: 판매 메서드
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
}
