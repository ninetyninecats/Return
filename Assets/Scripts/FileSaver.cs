using System;
using System.IO;
using UnityEngine;
public class SaveFile
{
    private static ushort saveData = 0;
    public static int GetSavePoint()
    {
        return saveData >> 13;
    }
    public static void SetSavePoint(int savePoint)
    {
        if (savePoint > 4) throw new Exception("Save point index must not exceed 4");
        saveData = (ushort)((savePoint << 13) | (ushort)((ushort)(saveData << 3) >> 3));
    }
    public static bool GetDoubleJump()
    {
        return ((saveData >> 12) & 1) == 1;
    }
    public static void SetDoubleJump(bool doubleJump)
    {
        saveData = SetBit(saveData, 12, doubleJump);
    }
    public static bool GetDash()
    {
        return ((saveData >> 11) & 1) == 1;
    }
    public static void SetDash(bool dash)
    {
        saveData = SetBit(saveData, 11, dash);
    }
    //Did not implement due to time constraints
    public static bool GetSapSlash()
    {
        return ((saveData >> 10) & 1) == 1;
    }
    public static void SetSapSlash(bool sapSlash)
    {
        saveData = (ushort)(saveData ^ (Convert.ToUInt16(sapSlash) ^ saveData) & (1 << 10));
    }
    //Did not implement due to time constraints
    public static bool GetMiniBoss()
    {
        return ((saveData >> 9) & 1) == 1;
    }
    public static void SetMiniBoss(bool miniBoss)
    {
        saveData = (ushort)(saveData ^ (Convert.ToUInt16(miniBoss) ^ saveData) & (1 << 9));
    }
    public static bool GetBiscuit(int biscuit)
    {
        Debug.Log(saveData);
        if (biscuit > 8) throw new Exception("Biscuit index must not exceed 8");
        return ((saveData >> biscuit) & 1) == 1;
    }
    public static void SetBiscuit(bool collected, int biscuit)
    {
        if (biscuit > 8) throw new Exception("Biscuit index must not exceed 8");
        saveData = SetBit(saveData, biscuit, collected);
    }
    public static void SaveToFile()
    {
        FileStream fileStream = new FileStream(Path.Combine(Application.persistentDataPath, "return.dat"), FileMode.Create);
        fileStream.WriteByte((byte)saveData);
        fileStream.WriteByte((byte)(saveData >> 8));
        fileStream.Close();
    }
    public static void LoadFromFile()
    {
        FileStream fileStream = new FileStream(Path.Combine(Application.persistentDataPath, "return.dat"), FileMode.OpenOrCreate);
        saveData = (ushort)(fileStream.ReadByte() + (fileStream.ReadByte() << 8));
        fileStream.Close();
        Debug.Log(saveData);    
    }
    public static void InitializeFile()
    {
        FileStream fileStream = new FileStream(Path.Combine(Application.persistentDataPath, "return.dat"), FileMode.OpenOrCreate, FileAccess.ReadWrite);
        if (fileStream.Length != 2)
        {
            fileStream.WriteByte(0);
            fileStream.WriteByte(0);
        }
        fileStream.Close();
    }
    public static ushort SetBit(ushort number, int bit, bool value)
    {
        if (value) return number |= (ushort)(1 << bit);
        else return (ushort)(number & ~(1 << bit));

    }
}
