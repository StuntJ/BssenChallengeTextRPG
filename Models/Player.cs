using System;
using System.Numerics;
namespace TextRPG.Models;

public class Player : Character
{


    #region 프로퍼티
    //직업
    public JobType Job { get; private set; }
    //골드
    public int Gold { get; set; }
    //장착 무기
    public Equipment? EquipedWeapon { get; private set; }
    //장착 방어구
    public Equipment? EquipedArmor { get; private set; }
    #endregion
    #region 생성자
    public Player(string name, JobType job) : base(
        name: name,
        maxHp: GetInitHp(job),
        maxMp: GetInitMp(job),
        attackPower: GetInitAttack(job),
        defense: GetInitDefense(job),
        level: 1)
    {
        Job = job;
        Gold = 1000;

    }
    #endregion

    #region 직업별 초기 스텟
    private static int GetInitHp(JobType job)
    {
        switch (job)
        {
            case JobType.Warrior: return 150;
            case JobType.Archer: return 100;
            case JobType.Wizard: return 80;
            default: return 100;
        }
    }

    private static int GetInitMp(JobType job)
    {
        switch (job)
        {
            case JobType.Warrior: return 30;
            case JobType.Archer: return 50;
            case JobType.Wizard: return 100;
            default: return 30;
        }
    }

    private static int GetInitAttack(JobType job) =>
        job switch
        {
            JobType.Warrior => 20,
            JobType.Archer => 30,
            JobType.Wizard => 40,
            _ => 20
        };

    private static int GetInitDefense(JobType job) =>
        job switch
        {
            JobType.Warrior => 15,
            JobType.Archer => 10,
            JobType.Wizard => 5,
            _ => 15
        };
    #endregion

    #region 메서드
    //플레이어 정보 출력(오버라이드)
    public override void DisplayInfo()
    {
        //base.DisplayInfo();
        Console.Clear();
        Console.WriteLine($"===== {Name} 정보 =====");
        Console.WriteLine($"레벨: {Level}");
        Console.WriteLine($"HP: {CurrentHp}/{MaxHp}");
        Console.WriteLine($"MP: {CurrentMp}/{MaxMp}");
        int attackBonus = (EquipedWeapon != null) ? EquipedWeapon.AttackBonus : 0;
        int defenseBonus = (EquipedArmor != null) ? EquipedArmor.DefenseBonus : 0;
        Console.WriteLine($"ATK: {AttackPower} (+{attackBonus})");
        Console.WriteLine($"DEF: {Defense} (+{defenseBonus})");
        Console.WriteLine($"골드: {Gold}");

        //장착 아이템 목록
        if(EquipedWeapon !=null || EquipedArmor != null)
        {
            Console.WriteLine("\n[장착 중인 장비 목록]");
            if(EquipedWeapon != null)
            {
                Console.WriteLine($"무기: {EquipedWeapon.Name}");
            }

            if(EquipedArmor != null)
            {
                Console.WriteLine($"방어구: {EquipedArmor.Name}");
            }
        }
    }

    //기본 공격 메서드 (override)
    public override int Attack(Character target)
    {
        //장착무기 또는 방어구에 따른 추가 데미지 계산
        //null 병합 연산자 : ?? 
        int attackDamage = AttackPower;
        attackDamage += EquipedWeapon?.AttackBonus ?? 0;

        //if (EquipedWeapon != null)
        //{
        //    attackDamage += EquipedWeapon.AttackBonus;
        //}

        return target.TakeDamage(attackDamage);
    }

    //스킬 공격 (MP 소모) : Player 전용 메서드
    public int SkillAttack(Character target)
    {
        int mpCost = 15;

        //스킬 공격 = 기본 공격의 1.5배 데미지
        int totalDamage = AttackPower;
        totalDamage += EquipedWeapon?.AttackBonus ?? 0;
        totalDamage = (int)(totalDamage * 1.5f);

        //MP 소모
        CurrentMp -= mpCost;

        //데미지 전달
        return target.TakeDamage(totalDamage);
    }

    //골드 획득 메서드
    public void GainGold(int amount)
    {
        Gold += amount;
        Console.WriteLine($"골드 +{amount} 획득! 현재 골드: {Gold}");
    }

    //장비 착용 메서드
    public void EquipItem(Equipment newEquipment)
    {
        Equipment? prevEquipment = null;
        switch (newEquipment.Slot)
        {
            case EquipmentSlot.Weapon:
                prevEquipment = EquipedWeapon;
                EquipedWeapon = newEquipment;
                break;
            case EquipmentSlot.Armor:
                prevEquipment = EquipedArmor;
                EquipedArmor = newEquipment;
                break;
        }

        //이전 장비 해제 메시지
        if (prevEquipment != null)
        {
            Console.WriteLine($"{prevEquipment.Name} 장착 해제");
        }
        Console.WriteLine($"{newEquipment.Name} 장착 완료");
    }

    //골드 차감 메서드
    public void SpendGold(int amount)
    {
        if (Gold >= amount)
        {
            Gold -= amount;
        }
    }

    //장비 해제 메서드
    public Equipment? UnequipItem(EquipmentSlot slot)
    {
        Equipment? equipment = null;
        switch (slot)
        {
            case EquipmentSlot.Weapon:
                equipment = EquipedWeapon;
                EquipedWeapon = null;
                break;
            case EquipmentSlot.Armor:
                equipment = EquipedArmor;
                EquipedArmor = null;
                break;

        }

        if(equipment != null)
        {
            Console.WriteLine($"{equipment.Name} 장착 해제");
        }

        return equipment;
    }
    #endregion
}

