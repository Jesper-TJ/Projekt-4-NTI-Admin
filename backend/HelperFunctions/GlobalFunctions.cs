using System.Collections;
using AdminApi.Models;

namespace AdminApi.helperFunctions;
// TODO REWRITE TO USE BITMASK
public static class HelperFunctions
{
    /*
        Contains every role and their offset
    */
    readonly static Dictionary<int, string> rolesDictionary = new Dictionary<int, string>
    {
        {0, "Deleted"},
        {1, "Elev"},
        {2, "Personal"},
        {3, "Bibliotekarie"},
        {4, "Vaktmästare"},
        {5, "Rektor"},
        {6, "Admin"},
        {7, "Unused"},
    };

    /*
        Takes a BitArray as arg and returns a list of strings containing the corrosponding role from rolesDictionary
    */
    public static List<string> ConvertBitRolesToString(BitArray roles)
    {
        List<string> userRoles = [];
        for (int i = 0; i < roles.Length; i++)
        {
            if (roles[i])
                userRoles.Add(rolesDictionary[i]);
        }
        return userRoles;
    }

    /*
        Takes a list of strings containing roles from roles dictionary as arg and returns a bitArray
    */
    public static BitArray ConvertRolesToBitArray(List<string> roles)
    {
        BitArray bitArray = new(8);
        
        foreach (string role in roles)
        {
            /*
                Look for the role in the dictionary and set the corresponding index to true in the BitArray
            */
            int key = rolesDictionary.FirstOrDefault(x => x.Value == role).Key;
            if (key != -1) //   If role found in dictionary
                bitArray[key] = true;
        }

        return bitArray;
    }

    /*
        Takes BitArray as arg and returns a bool
    */
    public static bool CheckIfStaff(BitArray roles)
    {
        for (int i = 0; i < roles.Length; i++)
        {
            if (roles[i] && rolesDictionary[i] == "Personal")
                return true;
        }
        return false;
    }
}
