using System;
using System.IO;
using UnityEngine;
public class SaveFile
{
    private static ushort saveData = 0b0000000000000000;
    public static int GetSavePoint()
    {
        return saveData >> 13;
    }
    public static void SetSavePoint(int savePoint)
    {
        if (savePoint > 4) throw new Exception("Save point index must not exceed 4");
        saveData = (ushort)((savePoint << 13) | (ushort)((savePoint << 3) >> 3));
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
    public static bool GetSapSlash()
    {
        return ((saveData >> 10) & 1) == 1;
    }
    public static void SetSapSlash(bool sapSlash)
    {
        saveData = (ushort)(saveData ^ (Convert.ToUInt16(sapSlash) ^ saveData) & (1 << 10));
    }
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
        if (biscuit > 8) throw new Exception("Biscuit index must not exceed 8");
        return ((saveData >> biscuit) & 1) == 1;
    }
    public static void SetBiscuit(bool collected, int biscuit)
    {
        if (biscuit > 8) throw new Exception("Biscuit index must not exceed 8");
        saveData = (ushort)(saveData ^ (Convert.ToUInt16(collected) ^ saveData) & (1 << biscuit));
    }
    public static void SaveToFile()
    {
        FileStream fileStream = new FileStream(Path.Combine(Application.persistentDataPath, "return.dat"), FileMode.Create);
        fileStream.WriteByte((byte)saveData);
        fileStream.WriteByte((byte)(saveData >> 8));
    }
    public static void LoadFromFile()
    {
        FileStream fileStream = new FileStream(Path.Combine(Application.persistentDataPath, "return.dat"), FileMode.Create);
        saveData = (ushort)(fileStream.ReadByte() + (fileStream.ReadByte() >> 8));
    }
    public static ushort SetBit(ushort number, int bit, bool value)
    {
        if (value) return number |= (1 << 12);
        else return (ushort)(number & ~(1 << 12));

    }
}
