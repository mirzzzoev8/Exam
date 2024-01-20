int x = Convert.ToInt32(Console.ReadLine());
int y = Convert.ToInt32(Console.ReadLine());
int [] arr = new int[x];
for (int i = 0; i < x; i++)
{
    arr[i] = Convert.ToInt32(Console.ReadLine());
}

for (int i = 0; i < y; i++)
{
    if (i != y)
    {
        System.Console.Write(arr[i] + " ");
    }
}