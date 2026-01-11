using System;
using TextRPG.Models;

namespace TextRPG.Systems;

public class BattleSystem
{
    #region 던전 입장 - 전투 실행
    //전투 시작 메서드
    //반환값 : 전투 승리 여부
    public bool StartBattle(Player player, Enemy enemy)
    {
        Console.Clear();
        Console.WriteLine("\n╔════════════════════════════════╗");
        Console.WriteLine("║       전투 시작!               ║");
        Console.WriteLine("╚════════════════════════════════╝\n");

        // 등장한 적 캐릭터 정보 출력
        enemy.DisplayInfo();

        //턴 변수 정의
        int turn = 1;

        //전투 루프
        while (player.IsAlive && enemy.IsAlive)
        {
            Console.WriteLine($"\n======= 턴 {turn} =======");
            //플레이어 턴
            PlayerTurn(player,enemy);
            //TODO: 적 사망여부 판단 
            //TODO: 적 턴
            turn++;
        }

        //전투 결과 반환
        return player.IsAlive;
    }
    #endregion

    #region 플레이어 턴
    //플레이어 턴 (1. 공격, 2. 스킬, 3. 도망)
    private void PlayerTurn(Player player, Enemy enemy)
    {
        Console.WriteLine($"\n{player.Name}의 턴!");
        Console.WriteLine($"HP: {player.CurrentHp}/{player.MaxHp} | MP: {player.CurrentMp}/{player.MaxMp}");
        Console.WriteLine("\n행동을 선택하세요.");
        Console.WriteLine("1. 공격");
        Console.WriteLine("2. 스킬");
        Console.WriteLine("3. 도망");

        while(true)
        {
            Console.Write("\n선택 (1-3): ");
            string? input = Console.ReadLine();
            
            switch(input)
            {
                case "1":
                    int damage = player.Attack(enemy);
                    Console.WriteLine($"{player.Name}의 공격! {enemy.Name}에게 {damage}의 피해를 입혔습니다.");
                    Console.WriteLine($"{enemy.Name}의 남은 HP: {enemy.CurrentHp}/{enemy.MaxHp}");
                    break;
                case "2":
                    //스킬 사용 전에 MP 체크
                    if (player.CurrentMp < 15)
                    {
                        Console.WriteLine("MP가 부족합니다.");
                        continue;
                    }
                    //스킬 발동
                    int skillDamage = player.SkillAttack(enemy);
                    Console.WriteLine($"{player.Name}의 스킬공격! {enemy.Name}에게 {skillDamage}의 피해를 입혔습니다.");
                    Console.WriteLine($"{enemy.Name}의 남은 HP: {enemy.CurrentHp}/{enemy.MaxHp}");
                    
                    
                    break;
                case "3":
                    //도망 시도
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다. 다시 선택해주세요.");
                    break;
            }
        }
    }
    #endregion

    #region 적 턴

    #endregion
}
