using AdminApi.Models;
using AdminApi.Dtos;
using AdminApi.helperFunctions;
using System.Text.RegularExpressions;
using ImageMagick;

namespace AdminApi.AccessCards;

public static class AccessCard
{
    /*
        Takes in a user and file path to uploaded image, returns path to access card image as string
    */
    public static Task<string> GenerateAccessCardData(User user, string imageFilePath)
    {
        Console.WriteLine("Generate Access Card Data");
        string uniqueUserId = GenerateUniqueId(user);

        if(string.IsNullOrEmpty(user.Klass))
        {
            Console.WriteLine("Whoops someone did a fucky wucky");
            throw new ArgumentException("User not autherized to get access card");
        }

        //    If user.klass is Personal then set klassYear to 0, else set klassYear to numbers in klass
        int klassYear = user.Klass == "Personal" ? 0 : int.Parse(Regex.Match(user.Klass, @"\d+").Value);
        Console.WriteLine("Before barcode");
        string barcodeDirectory = $"{Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName}/AccessCards/Barcodes";
        string barcodePath = Code39Barcode.GenerateBarcode($"{klassYear}_{uniqueUserId}.png", uniqueUserId, barcodeDirectory);
        Console.WriteLine("After barcode");
        AccessCardDataDto accessCardData = new(user.Id, user.Name, uniqueUserId, barcodePath, HelperFunctions.ConvertBitRolesToString(user.Roles), imageFilePath);

        //    Generate AccessCard Image
        string generatedCardPath = CreateAccessCardImage(accessCardData);

        return Task.FromResult(generatedCardPath);
    }

    /*
        Takes in a object that contains all data needed to create an access card. Returns a string to the image file path if successful, else crashes.
    */
    private static string CreateAccessCardImage(AccessCardDataDto data)
    {  
        Console.WriteLine("Create Access Card Image");
        string accessCardDirectory = $"{Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName}/AccessCards";
        string imageSaveFilePath = $"{accessCardDirectory}/FinishedAccessCards/{data.UniqueId}.png";
        int imageHeight = 1280;
        int imageWidth = 720;

        var settings = new MagickReadSettings
        {
            Font = @"~/../../System/Library/Fonts/ArialHB.ttc",
            TextGravity = Gravity.Center,
            BackgroundColor = MagickColors.Transparent,
            Height = 100, //    height of text box
            Width = Convert.ToUInt32(imageWidth) //    width of text box
        };

        MagickGeometry userImageSize = new(500, 700);
        MagickGeometry accessCardSize = new(Convert.ToUInt32(imageWidth), Convert.ToUInt32(imageHeight));

        string rolesInReadableFormat = "";

        foreach(string role in data.Roles)
        {
            rolesInReadableFormat += $"{role}\n";
        }

        //    Load the base image (background)
        using (var baseImage = new MagickImage($"{accessCardDirectory}/StaticImages/background.jpg"))
        {
            //    Load the images you want
            using (var userImage = new MagickImage(data.ImageFilePath))
            using (var barcode = new MagickImage(data.BarCodeFilePath))
            using (var userName = new MagickImage($"caption:{data.Name}", settings))
            using (var userUniqueId = new MagickImage($"caption:{data.UniqueId}", settings))
            using (var userRoles = new MagickImage($"caption:{rolesInReadableFormat}", settings))
            {
                userImage.Resize(userImageSize);
                baseImage.Resize(accessCardSize);

                baseImage.Composite(userImage, 0, 0, CompositeOperator.Over);
                baseImage.Composite(barcode, 0, imageHeight-250, CompositeOperator.Over); //    250 is barcode height

                baseImage.Composite(userName, 0, 700, CompositeOperator.Over);
                baseImage.Composite(userUniqueId, 0, 800, CompositeOperator.Over);
                baseImage.Composite(userRoles, 0, 900, CompositeOperator.Over);

                if(File.Exists(imageSaveFilePath))
                    File.Delete(imageSaveFilePath);
                
                baseImage.Write(imageSaveFilePath);
            }
        }

        return imageSaveFilePath;
    }

    /*
        Takes a in a user, returns an unique id as string
        (This is kinda a shit way to do it and should probably be rewritten)
    */
    private static string GenerateUniqueId(User user)
    {
        string uniqueId = "";
        if(HelperFunctions.CheckIfStaff(user.Roles)) //     User is staff
        {
            string[] nameSplit = user.Email.Split(".");
            uniqueId = $"ntijoh-{nameSplit[0][0..2]}{nameSplit[1][0..2]}";
        } else
        {
            string studentStartYear = Regex.Match(user.Klass, @"\d+").Value;
            uniqueId = $"ntijoh{studentStartYear}{GetNextIdByKlass(int.Parse(studentStartYear)):D3}"; //     will break if more that 1000 students in a year
        }
        return uniqueId;
    }

    /*
        Takes in a klassyear as int and returns the next number for the unique id based on the amount of barcodes generated for that class.
    */
    private static int GetNextIdByKlass(int KlassYear) 
    {
        string barcodeDirectory = $"{Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName}/AccessCards/Barcodes";

        if(!Directory.Exists(barcodeDirectory))
        {
            throw new ArgumentException("Barcode directory not found");
        }

        string[] files = Directory.GetFiles(barcodeDirectory);

        int counter = 0;

        foreach(string fileName in files)
        {
            string[] fileNameSplit = Path.GetFileName(fileName).Split("_");
            if(fileNameSplit[0] == KlassYear.ToString())
            {
                counter++;
            }
        }

        return counter;
    }
}
