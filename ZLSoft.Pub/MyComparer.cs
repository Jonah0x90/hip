using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MyComparer<T> : System.Collections.Generic.IComparer<T>
{
    private Func<T, T, int> _MyCompare;
    public MyComparer(Func<T, T, int> compare)
    {
        this._MyCompare = compare;
    }
    public int Compare(T x, T y)
    {
        return this._MyCompare(x, y);
    }
}