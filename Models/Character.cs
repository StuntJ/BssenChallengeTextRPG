namespace TextRPG.Models;

//캐릭터 기본 추상 클래스
public abstract class Character
{
    #region 프로퍼티
    public string Name { get; protected set; }
    public int CurrentHp { get; protected set; }  
    public int MaxHp { get; protected set; }
    public int CurrentMp { get; protected set; }
    public int MaxMp {  get; protected set; }   
    public int AttackPower { get; protected set; }
    public int Defense { get; protected set; }
    public int Level {  get; protected set; }
       
    //생존 여부
    public bool IsAlive => CurrentHp > 0;
    #endregion

    #region 생성자
    protected Character(string name, int maxHp, int maxMp, int attackPower, int defense, int level) 
    {
        Name = name;
        CurrentHp = MaxHp = maxHp;
        CurrentMp = MaxMp = maxMp;
        AttackPower = attackPower;
        Defense = defense;  
        Level = level;
    }
    #endregion

    #region //메서드
    //공통으로 사용할 메서드들

    //캐릭터 스텟 출력
    public virtual void DisplayInfo()
    {
        Console.WriteLine($"==== {Name} 정보 ====");
        Console.WriteLine($"레벨: {Level}");
        Console.WriteLine($"체력: {CurrentHp}/{MaxHp}");
        Console.WriteLine($"마나: {CurrentMp}/{MaxMp}");
        Console.WriteLine($"공격력: {AttackPower}");
        Console.WriteLine($"방어력: {Defense}");
        Console.WriteLine("=======================");
    }
    #endregion
}

