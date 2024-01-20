int n = Convert.ToInt32(Console.ReadLine());
int EvenCount(int n)
{
    int cnt = 0;
    for (int i = n; i > 0; i/=10)
    {
         if((i%10) % 2 == 0) cnt++;
    }
    return cnt;

}

int OddCount(int n)
{
    int cnt = 0;
    for (int i = n; i > 0; i/=10)
    {
          if((i%10) % 2 != 0) cnt++;
    }
    return cnt;

}

int DigitCount(int n)
{
    int cnt = 0;
    for (int i = n; i > 0; i/=10)
    {
        cnt++;
    }
    return cnt;
    
}
int SumCount(int n)
{
    int cnt = 0;    
    for (int i = n; i > 0; i/=10)
    {
        cnt+=i%10;
    }
    return cnt;
    
}
System.Console.Write("Even : ");
System.Console.WriteLine(EvenCount(n));

System.Console.Write("Odd : ");
System.Console.WriteLine(OddCount(n));

System.Console.Write("Digits : ");
System.Console.WriteLine(DigitCount(n));

System.Console.Write("Sum : ");
System.Console.WriteLine(SumCount(n));



