using System;

namespace TextRPG.Models;

public class Consumable : Item
{
    #region 프로퍼티
    //HP 회복량 
    public int HpAmount { get; private set; }
    //MP 회복량
    public int MpAmount { get; private set; }
    #endregion

    #region 생성자
    public Consumable(string name,
        string description,
        int price,
        int hpAmount = 0,
        int mpAmount = 0)
        : base(name, description, price, ItemType.Potion)
    {
        HpAmount = hpAmount;
        MpAmount = mpAmount;
    }

    #endregion

    #region 메서드
    public override bool Use(Player player)
    {
        //플레이어의 HP/MP 회복하는 로직
        bool isUsed = false;

        //HP 회복
        if (HpAmount > 0)
        {
            int healedHp = player.HealHp(HpAmount);
            if (healedHp>0)
            {
                Console.WriteLine($"{Name}사용! HP +{healedHp}만큼 회복됨!");
                isUsed = true;
            }
            else
            {
                Console.WriteLine("HP가 이미 최대치 입니다.");
            }

        }
        //MP 회복
        if (MpAmount > 0)
        {
            int healedMp = player.HealMp(MpAmount);
            if (healedMp > 0)
            {
                Console.WriteLine($"{Name}사용! MP +{healedMp}만큼 회복됨!");
                isUsed = true;
            }
            else
            {
                Console.WriteLine("MP가 이미 최대치 입니다.");
            }
        }

        return isUsed;
    }
    #endregion

    #region 포션 생성 메서드
    public static Consumable CreatePotion(string potionType) => potionType switch
    {
        "체력포션" => new Consumable("체력포션", "50 HP를 회복하는 물약", 50, hpAmount: 50),
        "대형체력포션" => new Consumable("대형체력포션", "100 HP를 회복하는 물약", 100, hpAmount: 100),
        "마나포션" => new Consumable("마나포션", "50 MP를 회복하는 물약", 50, mpAmount: 50),
        "대형마나포션" => new Consumable("대형마나포션", "100 MP를 회복하는 물약", 100, mpAmount: 100),
        _ => null!
    };
    #endregion
}
