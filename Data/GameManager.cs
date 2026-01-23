using System;
using TextRPG.Models;
using TextRPG.Systems;
using TextRPG.Utils;

namespace TextRPG.Data;

class GameManager
{
    //싱글톤 패턴 (Singleton Pattern) 구현
    #region 싱글톤 패턴
    //싱글톤 인스턴스(필드)
    private static GameManager instance;

    //외부에서 인스턴스에 접근할 수 있는 정적 속성(property)
    public static GameManager Instance
    {
        get
        {
            //인스턴스가 없으면 새로 생성
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }

    private GameManager()
    {
        //클래스가 생성될 때 초기화 작업 수행

        //전투 시스템 초기화
        BattleSystem = new BattleSystem();
        //상점 시스템 초기화
        ShopSystem = new ShopSystem();
    }

    #endregion

    #region 프로퍼티

    //플레이어 캐릭터
    public Player? Player { get; private set; }

    //전투 시스템
    public BattleSystem BattleSystem { get; private set; }

    //인벤토리 시스템
    public InventorySystem Inventory { get; private set; }

    //상점 시스템
    public ShopSystem ShopSystem { get; private set; }

    //게임 실행 여부
    public bool IsRunning { get; private set; } = true;
    #endregion

    #region 게임 시작 / 종료
    //게임 시작 메서드
    public void StartGame(bool loadedGame = false)
    {
        //타이틀 표시
        ConsoleUI.ShowTitle();
        Console.WriteLine("빡센 게임에 오신 것을 환영합니다.\n");

        //새로 시작하는 게임에만 새 캐릭터및 설정을 처리 
        
        if (!loadedGame)
        {
            //캐릭터 생성
            CreateCharacter();
      
            //인벤토리 초기화
            Inventory = new InventorySystem();

            //초기 아이템 지급
            SetupInitItems();
        }

        //메인 게임 루프
        IsRunning = true;
        while (IsRunning)
        {
            ShowMainMenu();
        }

        //게임 종료 처리
        if (!IsRunning)
        {
            ConsoleUI.ShowGameOver();
        }
    }
    #endregion

    #region 캐릭터 생성
    private void CreateCharacter()
    {
        //이름 입력 
        Console.Write("캐릭터의 이름을 입력하세요: ");
        string? name = Console.ReadLine(); //nullable 허용

        if (string.IsNullOrWhiteSpace(name))
        {
            name = "무명용사"; //기본 이름 설정
        }

        Console.WriteLine($"{name}님, 모험을 시작하겠습니다!");

        //직업 선택
        Console.WriteLine("직업을 선택하세요:");
        Console.WriteLine("1: 전사");
        Console.WriteLine("2: 궁수");
        Console.WriteLine("3: 마법사");

        JobType job = JobType.Warrior; //기본 값 설정
        while (true)
        {
            Console.WriteLine("선택 (1-3): ");
            string? input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    job = JobType.Warrior;
                    break;
                case "2":
                    job = JobType.Archer;
                    break;
                case "3":
                    job = JobType.Wizard;
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다. 다시 선택해주세요.");
                    continue;
            }

            break;
        }
        //입력한 이름과 선택한 직업으로 플레이어 캐릭터 생성
        Player = new Player(name, job);
        Console.WriteLine($"\n{name}님, {job}직업으로 캐릭터가 생성되었습니다.");

        ////적 캐릭터 생성
        //Enemy enemy = Enemy.CreateEnemy(Player.Level);
        //enemy.DisplayInfo();

        ////전투 테스트
        //BattleSystem battleSystem = new BattleSystem();
        //bool playerWin = battleSystem.StartBattle(Player, enemy);

        ConsoleUI.PressAnyKey();
    }

    //초기 아이템 지급
    private void SetupInitItems()
    {
        //기본 장비 
        var weapon = Equipment.CreateWeapon("목검");
        var armor = Equipment.CreateArmor("천갑옷");
        Inventory.AddItem(weapon);
        Inventory.AddItem(armor);

        //포션 지급
        Inventory.AddItem(Consumable.CreatePotion("체력포션"));
        Inventory.AddItem(Consumable.CreatePotion("체력포션"));
        Inventory.AddItem(Consumable.CreatePotion("마나포션"));

        //기본 장비 착용
        Player.EquipItem(weapon);
        Player.EquipItem(armor);

        Console.WriteLine("\n초기 장비가 지급되었습니다.");
        ConsoleUI.PressAnyKey();
    }
    #endregion

    #region 메인 메뉴
    public void ShowMainMenu()
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════════╗");
        Console.WriteLine("║             메인 메뉴                    ║");
        Console.WriteLine("╚══════════════════════════════════════════╝\n");
        Console.WriteLine("\n1. 상태 보기");
        Console.WriteLine("2. 인벤토리");
        Console.WriteLine("3. 상점");
        Console.WriteLine("4. 던전 입장 (전투)");
        Console.WriteLine("5. 휴식 (HP/MP 회복)");
        Console.WriteLine("6. 저장");
        Console.WriteLine("0. 게임 종료");

        Console.Write("\n선택 (1-6): ");
        string? input = Console.ReadLine();
        switch (input)
        {
            case "1":
                Player.DisplayInfo();
                ConsoleUI.PressAnyKey();
                break;
            case "2":
                //인벤토리 기능 구현
                Inventory.ShowInventoryMenu(Player);
                break;
            case "3":
                //상점 기능 구현
                ShopSystem.ShowShopMenu(Player, Inventory);
                break;
            case "4":
                //던전 입장 및 전투 기능 구현
                EnterDungeon();
                break;
            case "5":
                //휴식 기능 구현
                rest();
                break;
            case "6":   
                //저장 기능 구현
                SaveGame();            
                break;
            case "0":
                IsRunning = false;
                Console.Write("\n게임을 종료합니다. 이용해주셔서 감사합니다!");
                break;
            default:
                Console.WriteLine("\n잘못된 입력입니다. 다시 선택해주세요.");
                ConsoleUI.PressAnyKey();
                break;
        }

    }
    #endregion

    #region 메뉴 기능
    //던전 입장 
    public void EnterDungeon()
    {
        Console.Clear();
        Console.WriteLine("\n던전에 입장합니다.");

        //적 캐릭터 생성
        Enemy enemy = Enemy.CreateEnemy(Player.Level);
        ConsoleUI.PressAnyKey();


        //전투 시작
        BattleSystem.StartBattle(Player, enemy);

        Console.WriteLine("\n던전 탐험을 마치고 마을로 돌아갑니다...");
        ConsoleUI.PressAnyKey();
    }

    // 휴식 (HP/MP) 회복
    private void rest()
    {
        //상수(Constant)
        const int restCost = 50;

        Console.Clear();
        Console.WriteLine("\n여관에서 휴식을 취합니다...");
        Console.WriteLine($"\n비용: {restCost}골드");
        if (Player.Gold < restCost)
        {
            Console.WriteLine("\n골드가 부족합니다.");
            ConsoleUI.PressAnyKey();
            return;
        }

        Console.Write("\n휴식을 취하겠습니까? (y/n): ");
        if (Console.ReadLine()?.ToLower() == "y")
        {
            Player.SpendGold(restCost);
            Player.HealHp(Player.MaxHp);
            Player.HealMp(Player.MaxMp);
            Console.WriteLine("\n휴식을 취했습니다. HP와 MP모두 회복되었습니다.");
            ConsoleUI.PressAnyKey();
        }
    }
    #endregion

    #region 저장 및 로드 기능
    //게임 저장
    public void SaveGame()
    {
        if(Player==null || Inventory == null)
        {
            Console.WriteLine("\n저장할 게임 데이터가 없습니다.");
            ConsoleUI.PressAnyKey();
            return;
        }

        if(SaveLoadSystem.SaveGame(Player, Inventory))
        {
            Console.WriteLine("\n정상적으로 게임이 저장되었습니다.");
            ConsoleUI.PressAnyKey();
        }
    }
    //게임 로드
    public bool LoadGame()
    {
        var saveData = SaveLoadSystem.LoadGame();
        if (saveData == null) return false;

        //1. Player 복원
        Player = SaveLoadSystem.LoadPlayer(saveData.Player);
        //2. Inventory 복원
        //3. 장착 아이템 복원

        Console.WriteLine("게임을 불러왔습니다.");
        ConsoleUI.PressAnyKey();
        return true;
    }

    #endregion
}

