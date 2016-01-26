

using System;
using System.Collections;





[Serializable()]public class ArrayListStack
{
	
	private ArrayList _list = new ArrayList();
	
	public void Push(object item)
	{
		_list.Add(item);
	}
	
	public void Pop()
	{
		if (!IsEmpty())
		{
			_list.RemoveAt(_list.Count - 1);
		}
	}
	
	public object Peek()
	{
		if (!IsEmpty())
		{
			return _list[_list.Count - 1];
		}
		return null;
	}
	
	public void Clear()
	{
		_list.Clear();
	}
	
	public bool IsEmpty()
	{
		return _list.Count == 0;
	}
	
	public int Count()
	{
		return _list.Count;
	}
}

