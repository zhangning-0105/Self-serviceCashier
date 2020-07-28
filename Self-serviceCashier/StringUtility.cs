// Decompiled with JetBrains decompiler
// Type: CommUtility.StringUtility
// Assembly: shelves, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F53175E9-F356-409A-B719-6E2FD48F2E04
// Assembly location: C:\Users\zhangning\Desktop\演示demo\Release\shelves.exe

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CommUtility
{
  public class StringUtility
  {
    public static byte[] string2bytes(string str)
    {
      byte[] bytes = Encoding.Default.GetBytes(str);
      byte[] numArray1 = new byte[str.Length + 1];
      byte[] numArray2 = numArray1;
      int length = str.Length;
      Array.Copy((Array) bytes, (Array) numArray2, length);
      return numArray1;
    }

    public static string bytes2string(byte[] byteArray)
    {
      return Encoding.Default.GetString(byteArray);
    }

    public static string bytes2string(byte[] byteArray, int index, int count)
    {
      return Encoding.Default.GetString(byteArray, index, count);
    }

    public static byte[] string2ascii(string str)
    {
      return Encoding.ASCII.GetBytes(str);
    }

    public static string ascii2string(byte[] byteArray)
    {
      return Encoding.ASCII.GetString(byteArray);
    }

    public static byte[] hexs2bytes(string hexStr)
    {
      if (string.IsNullOrEmpty(hexStr))
        return new byte[0];
      if (hexStr.StartsWith("0x"))
        hexStr = hexStr.Remove(0, 2);
      int length1 = hexStr.Length;
      if (length1 % 2 == 1)
        throw new ArgumentException("Invalid length of bytes:" + (object) length1);
      int length2 = length1 / 2;
      byte[] numArray = new byte[length2];
      for (int index = 0; index < length2; ++index)
      {
        byte num = byte.Parse(hexStr.Substring(2 * index, 2), NumberStyles.HexNumber);
        numArray[index] = num;
      }
      return numArray;
    }

    public static string bytes2hexs(byte[] bytes, int counts)
    {
      if (bytes == null || ((IEnumerable<byte>) bytes).Count<byte>() < 1)
        return string.Empty;
      int num = counts == 0 ? ((IEnumerable<byte>) bytes).Count<byte>() : counts;
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < num; ++index)
      {
        string upper = Convert.ToString(bytes[index], 16).ToUpper();
        stringBuilder.Append(upper.Length == 1 ? "0" + upper : upper);
      }
      return stringBuilder.ToString();
    }
  }
}
