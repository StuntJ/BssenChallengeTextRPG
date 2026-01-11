using TextRPG.Models;
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
            if(instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }
    
    private GameManager()
    {
        //클래스가 생성될 때 초기화 작업 수행
    }

    #endregion

    #region 프로퍼티
    public Player? Player { get; private set; }
    //게임 실행 여부
    public bool IsRunning { get; private set; } = true;
    #endregion

    #region 게임 시작 / 종료
    //게임 시작 메서드
    public void StartGame()
    {
        //타이틀 표시
        ConsoleUI.ShowTitle();
        Console.WriteLine("빡센 게임에 오신 것을 환영합니다.\n");

        //캐릭터 생성
        CreateCharacter();

        //메인 게임 루프
        IsRunning = true;
        while (IsRunning)
        {
            ShowMainMenu();
        }
        
        //게임 종료 처리
        if(!IsRunning)
        {
            ConsoleUI.ShowGameOver();
        }
        //TODO: 인벤토리 초기화
        //TODO: 초기 아이템 지급
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

        //적 캐릭터 생성
        Enemy enemy = Enemy.CreateEnemy(Player.Level);
        enemy.DisplayInfo();

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
                //TODO: 인벤토리 기능 구현
                break;
            case "3":
                //TODO: 상점 기능 구현
                break;
            case "4":
                //TODO: 던전 입장 및 전투 기능 구현
                break;
            case "5":
                //TODO: 휴식 기능 구현 
                break;
            case "6":
                //TODO: 저장 기능 구현 
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
}

