int a = Convert.ToInt32(Console.ReadLine());
int n = Convert.ToInt32(Console.ReadLine());
int [] arr = new int[a];
void IncrementArrayElements(ref int[] arr, ref int n)
{
    for (int i = 0; i < arr.Length; i++)
    {
        arr[i] = Convert.ToInt32(Console.ReadLine());
        arr[i]+=a;
    }

   for (int i = 0; i < arr.Length; i++)
   {
      System.Console.WriteLine(arr[i]+ " ");
   }
}
IncrementArrayElements(ref arr, ref a);
