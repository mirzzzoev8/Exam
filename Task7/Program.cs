int SumDigit(int x)
{
    if(x == 0) return 0;
    else return x%10 + SumDigit(x/10);
}
System.Console.WriteLine(SumDigit(111));
System.Console.WriteLine(SumDigit(222));
System.Console.WriteLine(SumDigit(333));