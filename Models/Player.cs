namespace TextRPG.Models;

public class Player : Character
{


    #region 프로퍼티
    //직업
    public JobType Job { get; private set; }
    //골드
    public int Gold { get; private set; }
    //TODO: 장착 무기
    //TODO: 장착 방어구
    #endregion
    #region 생성자
    public Player(string name, JobType job) : base(
        name:name,
        maxHp:GetInitHp(job),
        maxMp:GetInitMp(job),
        attackPower:GetInitAttack(job),
        defense:GetInitDefense(job),
        level:1)
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
        base.DisplayInfo();
        Console.WriteLine($"골드: {Gold}");

    }
    #endregion
}

